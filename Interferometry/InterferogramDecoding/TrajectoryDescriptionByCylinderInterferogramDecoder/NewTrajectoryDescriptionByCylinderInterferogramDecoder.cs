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
using ExtraLibrary.Randomness;

using ExtraLibrary.Arraying.ArrayOperation;
using System.Numerics;

namespace Interferometry.InterferogramDecoding {

    public class NewTrajectoryDescriptionByCylinderInterferogramDecoder : BaseAdvancedInterferogramDecoder {
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
        public Point2D[] PlaneXYPoints {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        public RealVector EigenVector1 {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        public RealVector EigenVector2 {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------------
        public NewTrajectoryDescriptionByCylinderInterferogramDecoder() {
            //--
        }
        //---------------------------------------------------------------------------------------------
        public InterferogramDecodingResult DecodeInterferogram( RealMatrix[] interferograms, BitMask2D bitMask ) {

            //Выбранные точки изображения
            Point[] selectedImagePoints = bitMask.GetTruePoints();

            //Ортогональные векторы
            RealVector[] orthogonalVectors = this.GetOrthogonalVectors( interferograms, selectedImagePoints );
            
            RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

            int indexVector1 = randomNumberGenerator.GetNextInteger( orthogonalVectors.Length - 1 );
            int indexVector2 = randomNumberGenerator.GetNextInteger( orthogonalVectors.Length - 1 );
            
            RealVector vector1 = orthogonalVectors[ indexVector1 ];
            RealVector vector2 = orthogonalVectors[ indexVector2 ];

            RealVector normalY;
            RealVector normalX;

            RealMatrix rotMatrixY;
            RealMatrix rotMatrixX;
            RealMatrix rotMatrix;

            double alfa;
            double beta;
            
            normalY = SpaceManager.GetVectorsCrossProduct( vector1, vector2 );
            alfa = Math.Atan( -( normalY[ 0 ] / normalY[ 2 ] ) );
            rotMatrixY = SpaceManager.GetRotationMatrixAroundAxisY( alfa );

            normalX = SpaceManager.RotateVector( normalY, rotMatrixY );
            beta = Math.Atan( -( normalX[ 1 ] / normalX[ 2 ] ) );
            rotMatrixX = SpaceManager.GetRotationMatrixAroundAxisX( beta );
                                    
            rotMatrix = rotMatrixX * rotMatrixY;

            RealVector[] rotVectors = SpaceManager.RotateVectors( orthogonalVectors, rotMatrix );
            Point3D[] points3D = this.GetPoints3D( rotVectors );
            
            //this.OrthogonalVectorsPoints = points3D;
                        
            Point2D[] points2D = SpaceManager.GetProjectionXY( points3D );
            this.PlaneXYPoints = points2D;

            RealMatrix covariationMatrix = Statistician.GetCovariationMatrix( points2D );
            double a11 = covariationMatrix[ 0, 0 ];
            double a22 = covariationMatrix[ 1, 1 ];
            double a12 = covariationMatrix[ 0, 1 ];

            double[] eigenValues = MathHelper.GetEigenValuesMatrix2x2( covariationMatrix );
            double minEigenValue = eigenValues.Min();
            double maxEigenValue = eigenValues.Max();

            RealVector eigenVector1 = new RealVector
            (
                1, - ( (a11 + a12 - minEigenValue) / (a12 + a22 - minEigenValue) )
            );

            RealVector eigenVector2 = new RealVector
            (
                1, -( ( a11 + a12 - maxEigenValue ) / ( a12 + a22 - maxEigenValue ) )
            );
            
            Complex complex = new Complex( eigenVector1[ 0 ], eigenVector1[ 1 ] );
            double omega = complex.Phase;
            
            double a = Math.Sqrt( minEigenValue );
            double b = Math.Sqrt( maxEigenValue );
            double h = a / Math.Sqrt( b * b - a * a );
            RealVector Nc = new RealVector( Math.Cos( omega ), -Math.Sin( omega ), h );
                        
            Point3D[] circlePoints = this.GetCirclePoints( points3D, Nc );
            //this.OrthogonalVectorsPoints = circlePoints;

            RealVector[] circlePointsVectors = new RealVector[ circlePoints.Length ];
            for ( int index = 0; index < circlePoints.Length; index++ ) {
                circlePointsVectors[ index ] = new RealVector( circlePoints[ index ] );
            }

            indexVector1 = randomNumberGenerator.GetNextInteger( circlePointsVectors.Length - 1 );
            indexVector2 = randomNumberGenerator.GetNextInteger( circlePointsVectors.Length - 1 );

            vector1 = circlePointsVectors[ indexVector1 ];
            vector2 = circlePointsVectors[ indexVector2 ];

            normalY = SpaceManager.GetVectorsCrossProduct( vector1, vector2 );
            alfa = Math.Atan( -( normalY[ 0 ] / normalY[ 2 ] ) );
            rotMatrixY = SpaceManager.GetRotationMatrixAroundAxisY( alfa );

            normalX = SpaceManager.RotateVector( normalY, rotMatrixY );
            beta = Math.Atan( -( normalX[ 1 ] / normalX[ 2 ] ) );
            rotMatrixX = SpaceManager.GetRotationMatrixAroundAxisX( beta );

            rotMatrix = rotMatrixX * rotMatrixY;
            
            rotVectors = SpaceManager.RotateVectors( circlePointsVectors, rotMatrix );
            points3D = this.GetPoints3D( rotVectors );
            points2D = SpaceManager.GetProjectionXY( points3D );
            
            this.OrthogonalVectorsPoints = points3D;
            
            RealMatrix phaseMatrix = new RealMatrix( interferograms[ 0 ].RowCount, interferograms[ 0 ].ColumnCount ); 

            for ( int index = 0; index < selectedImagePoints.Length; index++ ) {
                Point2D circlePoint = points2D[ index ];
                int x = selectedImagePoints[ index ].X;
                int y = selectedImagePoints[ index ].Y;

                phaseMatrix[ y, x ] = Math.PI + Math.Atan2( circlePoint.Y, circlePoint.X );
            }


            this.EigenVector1 =
                new RealVector( eigenVector1[ 0 ] / eigenVector1.Length, eigenVector1[ 1 ] / eigenVector1.Length );
            
            this.EigenVector2 =
                new RealVector( eigenVector2[ 0 ] / eigenVector2.Length, eigenVector2[ 1 ] / eigenVector2.Length );




            InterferogramDecodingResult decodingResult = new InterferogramDecodingResult( phaseMatrix );
            return decodingResult;
        }
        //---------------------------------------------------------------------------------------------
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
        private Point3D GetCorrectedCirclePoint( Point3D point, RealVector cylinderGuidLine ) {
            /*
            RealVector pointVector = new RealVector( point );
            Point3D newPoint =
                point - ( ( cylinderGuidLine * pointVector ) * cylinderGuidLine ).ToPoint3D();
            return newPoint;
            */

            RealVector pointVector = new RealVector( point );
            Point3D newPoint =
                point - ( ( 1 / (cylinderGuidLine * cylinderGuidLine) ) * ( cylinderGuidLine * pointVector ) * cylinderGuidLine ).ToPoint3D();
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
