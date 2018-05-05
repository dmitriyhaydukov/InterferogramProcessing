using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;
using ExtraLibrary.Mathematics;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Mathematics.Approximation;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Mathematics.Sets;

using ExtraLibrary.Arraying.ArrayOperation;

namespace Interferometry.InterferogramDecoding {
    
    public class TrajectoryDescriptionByCylinderInterferogramDecoder : BaseAdvancedInterferogramDecoder {
        //---------------------------------------------------------------------------------------------
        public Dictionary<double, double> CirclePointsDispersion {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        public Point3D[] TrajectoryPoints {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        public Point3D[] CirclePoints {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        public Point3D[] RotatedCirclePoints {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        public RealVector[] RotatedVectors {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        public PlaneDescriptor CirclePointsPlane {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        public TrajectoryDescriptionByCylinderInterferogramDecoder() {
            //--
        }
        //---------------------------------------------------------------------------------------------
        public InterferogramDecodingResult DecodeInterferogram( RealMatrix[] interferograms, BitMask2D bitMask ) {
            
            //double gamma = 2;
            //interferograms = MatricesManager.GetGammaCorrectedMatrices( gamma, interferograms );
            //interferograms = GetNormalizedMatrices( interferograms );

            //Выбранные точки изображения
            Point[] selectedImagePoints = bitMask.GetTruePoints();
            Point3D[] points3D =
                this.GetParallelToCoordinatePlanePoints3D( interferograms, selectedImagePoints );
            this.TrajectoryPoints = points3D;
            
            Point2D[] points2D = SpaceManager.GetProjectionXY( points3D );
            this.ProjectionPoints = points2D;

            EllipseDescriptor ellipseDescriptor = this.GetEllipseDescriptor( points2D );
            Point2D ellipseCentre = ellipseDescriptor.GetCentre();
            Point3D pointsCentre = new Point3D( ellipseCentre.X, ellipseCentre.Y, points3D[ 0 ].Z );
            
            RealVector cylinderGuidLine = this.GetCylinderGuidLine( ellipseDescriptor );
            RealVector normalizedCylinderGuidLine = cylinderGuidLine.GetNormalizedVector();
            
            /*
            this.CalculateCirclePointsDispersion( cylinderGuidLine, points3D, pointsCentre );
            double cylinderGuidLineOptimalAngle = this.GetCylinderGuidLineOptimalAngle();
            cylinderGuidLine = this.GetRotatedInPlaneCylinderGuidLine
                ( cylinderGuidLine, cylinderGuidLineOptimalAngle );
            */ 
            
            Point3D[] circlePoints = this.GetCirclePoints( points3D, normalizedCylinderGuidLine );
            this.CirclePoints = circlePoints;
            Point3D circlePointsCentre = 
                this.GetCorrectedCirclePoint( pointsCentre, normalizedCylinderGuidLine );
            
            //Поворот точек
            RealMatrix rotationMatrix = this.GetRotationMatrix( circlePoints );
            //rotationMatrix.WriteToTextFile( @"d:\!!\RotationMatrix.txt" ); // debug

            Point3D[] rotatedCirclePoints = this.RotatePoints( circlePoints, rotationMatrix );
            this.RotatedCirclePoints = rotatedCirclePoints;
            
            Point3D rotatedCirclePointsCentre = PlaneManager.RotateVector
                ( new RealVector( circlePointsCentre ), rotationMatrix ).ToPoint3D();
            Point3D[] correctedPoints =
                this.DisplacePoints( rotatedCirclePoints, rotatedCirclePointsCentre );
            
            int sizeX = interferograms[ 0 ].ColumnCount;
            int sizeY = interferograms[ 0 ].RowCount;

            RealMatrix phaseMatrix = this.CalculatePhaseMatrixByCirclePoints
                ( correctedPoints, selectedImagePoints, sizeX, sizeY );

            InterferogramDecodingResult decodingResult = new InterferogramDecodingResult( phaseMatrix );
            return decodingResult;
        }
        //---------------------------------------------------------------------------------------------
        //Оптимальный угол наклона направляющей цилиндра
        private double GetCylinderGuidLineOptimalAngle() {
            double optimalAngle = 0;
            double minDispersion = 0;
            foreach ( KeyValuePair<double, double> keyValuePair in this.CirclePointsDispersion ) {
                double angle = keyValuePair.Key;
                double dispersion = keyValuePair.Value;
                if ( dispersion <= minDispersion ) {
                    minDispersion = dispersion;
                    optimalAngle = angle;
                }
            }
            return optimalAngle;
        }
        //---------------------------------------------------------------------------------------------
        //Перемещение точек
        private Point3D[] DisplacePoints( Point3D[] points, Point3D pointsCentre ) {
            double value = 500;
            Point3D pointVectorN = new Point3D( value, value, value );
            double displacementX = pointVectorN.X - pointsCentre.X;
            double displacementY = pointVectorN.Y - pointsCentre.Y;
            double displacementZ = pointVectorN.Z - pointsCentre.Z;
            Point3D[] newPoints = SpaceManager.DisplacePoints
                ( points, displacementX, displacementY, displacementZ );
            return newPoints;
        }
        //---------------------------------------------------------------------------------------------
        //Аппроксимация эллипсом
        private EllipseDescriptor GetEllipseDescriptor( Point2D[] points ) {
            EllipseApproximator ellipseApproximator = new EllipseApproximator();
            EllipseDescriptor ellipseDescriptor = ellipseApproximator.Approximate( points );
            return ellipseDescriptor;
        }
        //---------------------------------------------------------------------------------------------
        //Матрица вращения для точек окружности
        private RealMatrix GetRotationMatrix( Point3D[] circlePoints ) {
            //PlaneDescriptor circlePointsPlane = this.GetPlane( circlePoints );
            
            PlaneApproximator planeApproximator = new PlaneApproximator();
            PlaneDescriptor circlePointsPlane = planeApproximator.Approximate( circlePoints );
            
            this.CirclePointsPlane = circlePointsPlane; //debug
            
            RealVector circlePointsPlaneNormalVector = circlePointsPlane.GetNormalVector();
            RealVector vectorN = new RealVector( 1, 1, 1 );
            RealMatrix rotationMatrix = SpaceManager.GetRotationMatrixToTargetVector
                ( circlePointsPlaneNormalVector, vectorN );
            return rotationMatrix;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //Повернуть точки
        private Point3D[] RotatePoints( Point3D[] points, RealMatrix rotationMatrix ) {
            RealVector[] vectors = SpaceManager.GetCoordinatesVectors( points );
            RealVector[] rotatedVectors = SpaceManager.RotateVectors( vectors, rotationMatrix );
            
            this.RotatedVectors = rotatedVectors; //debug

            Point3D[] rotatedPoints = SpaceManager.CreatePoints3DFromVectors( rotatedVectors );
            return rotatedPoints;
        }
        //---------------------------------------------------------------------------------------------
        //Корректировка точки к окружности
        private Point3D GetCorrectedCirclePoint (Point3D point, RealVector cylinderGuidLine ) {
            RealVector pointVector = new RealVector( point );
            Point3D newPoint =
                point - ( ( cylinderGuidLine * pointVector ) * cylinderGuidLine ).ToPoint3D();
            return newPoint;
        }
        //---------------------------------------------------------------------------------------------
        //Преобразование к круговой траектории
        private Point3D[] GetCirclePoints( Point3D[] points, RealVector cylinderGuidLine ) {
            Point3D[] newPoints = new Point3D[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                Point3D newPoint = this.GetCorrectedCirclePoint( point, cylinderGuidLine );
                newPoints[ index ] = newPoint;
            }
            return newPoints;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //Формирование направляющей цилиндра
        private RealVector CreateCylinderGuidLine(
            double semiMinorAxis, double semiMajorAxis, double angle
        ) {
            double[] vectorValues = new double[] {
                Math.Cos(angle),
                Math.Sin(angle),
                (semiMinorAxis / semiMajorAxis)
            };
            RealVector guidLine = new RealVector( vectorValues );
            return guidLine;
        }
        //---------------------------------------------------------------------------------------------
        //Направляющая цилиндра
        private RealVector GetCylinderGuidLine( EllipseDescriptor ellipseDescriptor ) {
            double semiMinorAxis = ellipseDescriptor.GetSemiMinorAxis();
            double semiMajorAxis = ellipseDescriptor.GetSemiMajorAxis();
            double angle = ellipseDescriptor.GetAngleBetweenOxAndPrincipalAxis();
            RealVector cylinderGuidLine =
                this.CreateCylinderGuidLine( semiMinorAxis, semiMajorAxis, angle );
            return cylinderGuidLine;
        }
        //---------------------------------------------------------------------------------------------
        //Повернутая в плоскости направляющая цилиндра
        private RealVector GetRotatedInPlaneCylinderGuidLine( RealVector cylinderGuidLine, double angle ) {
            RealVector auxiliaryVector = new RealVector( 0, 0, -1 );
            RealVector newVector =
                SpaceManager.RotateVectorInPlane( cylinderGuidLine, auxiliaryVector, angle );
            return newVector;
        }
        //---------------------------------------------------------------------------------------------
        //Вычисление дисперсии точек окружности
        private void CalculateCirclePointsDispersion( 
            RealVector cylinderGuidLine, Point3D[] points, Point3D pointsCentre
        ) {
            this.CirclePointsDispersion = new Dictionary<double, double>();

            double startAngle = -Math.PI / 2;
            double finishAngle = Math.PI / 2;
            double angleStep = Math.PI / 30;

            for ( double angle = startAngle; angle < finishAngle; angle += angleStep ) {
                RealVector tiltedCylinderGuidLine = 
                    this.GetRotatedInPlaneCylinderGuidLine( cylinderGuidLine, angle );
                Point3D[] circlePoints = this.GetCirclePoints( points, tiltedCylinderGuidLine );
                Point3D circlePointsCentre = this.GetCorrectedCirclePoint( pointsCentre, tiltedCylinderGuidLine );
                double dispersion = this.GetPointsDispersion( circlePoints, circlePointsCentre );                
                this.CirclePointsDispersion.Add( angle, dispersion );
            }
        }
        //---------------------------------------------------------------------------------------------
        //Дисперсия
        private double GetPointsDispersion( Point3D[] points, Point3D pointsCentre ) {
            double[] distancesFromPointsCentre = SpaceManager.GetDistances( points, pointsCentre );
            double dispersion = Statistician.GetDispersion( distancesFromPointsCentre );
            return dispersion;
        }
        //---------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------
        //Вычисление матрицы фаз
        private RealMatrix CalculatePhaseMatrixByCirclePoints(
            Point3D[] circlePoints, Point[] imagePoints, int sizeX, int sizeY
        ) {
            bool sizeEuqality = ArrayOperator.IsArraySizesEqual( circlePoints, imagePoints );
            if ( !sizeEuqality ) {
                throw new Exception();
            }
            ThreePointInterferogramDecoder decoder = new ThreePointInterferogramDecoder();

            double[] phaseShifts = this.GetPhaseShifts();
            
            RealMatrix phaseMatrix = new RealMatrix( sizeY, sizeX, this.DefaultPhaseValue );
            for ( int index = 0; index < circlePoints.Length; index++ ) {
                Point3D point = circlePoints[ index ];
                Point imagePoint = imagePoints[ index ];
                double[] intensities = new double[] { point.X, point.Y, point.Z };
                
                double phase = decoder.Decode( intensities, phaseShifts );
                //double phase = 2 * Math.PI - decoder.Decode( intensities, phaseShifts );
                
                phaseMatrix[ imagePoint.Y, imagePoint.X ] = phase;
            }
            return phaseMatrix;
        }
        //---------------------------------------------------------------------------------------------
        private double[] GetPhaseShifts() {
            double[] phaseShifts = new double[] { 0, 2 * Math.PI / 3, 4 * Math.PI / 3 };
            //double[] phaseShifts = new double[] { 0, Math.PI / 3, 2 * Math.PI / 3 };
            return phaseShifts;
        }
        //---------------------------------------------------------------------------------------------
        private RealMatrix[] GetNormalizedMatrices( RealMatrix[] matrices ) {
            
            RealMatrix[] newMatrices = MatricesManager.TransformMatricesValuesToFinishIntervalValues
                ( new Interval<double>( 0, 255 ), matrices );
            
            return newMatrices;
        }

        //---------------------------------------------------------------------------------------------
    }
}
