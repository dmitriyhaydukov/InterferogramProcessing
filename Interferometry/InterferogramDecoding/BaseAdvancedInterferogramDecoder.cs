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
using ExtraLibrary.Arraying.ArrayOperation;

namespace Interferometry.InterferogramDecoding {
    public abstract class BaseAdvancedInterferogramDecoder : InterferogramDecoder {
        //---------------------------------------------------------------------------------------------
        //Точки эллипса
        public Point2D[] EllipsePoints {
            get;
            protected set;
        }
        //---------------------------------------------------------------------------------------------
        //Точки проекции
        public Point2D[] ProjectionPoints {
            get;
            protected set;
        }
        //---------------------------------------------------------------------------------------------
        //Точки ортогональных векторов
        public Point3D[] OrthogonalVectorsPoints {
            get;
            protected set;
        }
        //---------------------------------------------------------------------------------------------
        public BaseAdvancedInterferogramDecoder() {
            //
        }
        //---------------------------------------------------------------------------------------------
        protected Point3D[] GetParallelToCoordinatePlanePoints3D(
            RealMatrix[] interferograms, Point[] imagePoints
        ) {
            //Ортогональные векторы
            RealVector[] orthogonalVectors = this.GetOrthogonalVectors( interferograms, imagePoints );
            Point3D[] points3D = this.GetPoints3D( orthogonalVectors );
            this.OrthogonalVectorsPoints = points3D;
            
            //Перемещение в первый октант
            points3D = SpaceManager.DisplacePointsToFirstOctant( points3D );

            PlaneDescriptor planeDescriptor = this.GetPlane( points3D );
            RealVector planeNormalVector = planeDescriptor.GetNormalVector();
            points3D = this.RotateParallelToPlaneXY( points3D, planeNormalVector );
            return points3D;
        }
        //---------------------------------------------------------------------------------------------
        //Ортогональные векторы
        protected RealVector[] GetOrthogonalVectors( RealMatrix[] interferograms, Point[] imagePoints ) {
            int pointsCount = imagePoints.Length;
            RealVector[] orthogonalVectors = new RealVector[ pointsCount ];

            for ( int index = 0; index < pointsCount; index++ ) {
                Point point = imagePoints[ index ];
                int x = point.X;
                int y = point.Y;
                double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                RealVector intensitiesVector = new RealVector( intensities );
                RealVector orthogonalVector = intensitiesVector.GetOrthogonalVector();
                orthogonalVectors[ index ] = orthogonalVector;
            }

            return orthogonalVectors;
        }
        //---------------------------------------------------------------------------------------------
        //Поворот параллельно плоскости XY
        protected Point3D[] RotateParallelToPlaneXY(
            Point3D[] points,
            RealVector planeNormalVector
        ) {
            RealVector[] coordinatesVectors = SpaceManager.GetCoordinatesVectors( points );
            RealVector[] newVectors;

            RealVector currentPlaneNormalVector = planeNormalVector;

            double rotationAngleAroundAxisX =
                Math.Atan( planeNormalVector[ 1 ] / planeNormalVector[ 2 ] );
            RealMatrix rotationMatrixAroundAxisX =
                SpaceManager.GetRotationMatrixAroundAxisX( rotationAngleAroundAxisX );
            newVectors =
                SpaceManager.RotateVectors( coordinatesVectors, rotationMatrixAroundAxisX );

            currentPlaneNormalVector =
                SpaceManager.RotateVector( currentPlaneNormalVector, rotationMatrixAroundAxisX );

            double rotationAngleAroundAxisY =
                -Math.Atan( currentPlaneNormalVector[ 0 ] / currentPlaneNormalVector[ 2 ] );
            RealMatrix rotationMatrixAroundAxisY =
                SpaceManager.GetRotationMatrixAroundAxisY( rotationAngleAroundAxisY );
            newVectors =
                SpaceManager.RotateVectors( newVectors, rotationMatrixAroundAxisY );

            Point3D[] newPoints = SpaceManager.CreatePoints3DFromVectors( newVectors );
            return newPoints;
        }
        //---------------------------------------------------------------------------------------------
        //Облако точек
        protected Point3D[] GetPoints3D( RealVector[] vectors ) {
            Point3D[] points3D = new Point3D[ vectors.Length ];
            for ( int index = 0; index < vectors.Length; index++ ) {
                RealVector vector = vectors[ index ];
                double x = vector[ 0 ];
                double y = vector[ 1 ];
                double z = vector[ 2 ];
                Point3D point = new Point3D( x, y, z );
                points3D[ index ] = point;
            }
            return points3D;
        }
        //---------------------------------------------------------------------------------------------
        //Плоскость, проходящая через три точки
        protected PlaneDescriptor GetPlane( Point3D[] points ) {
            Random random = new Random( DateTime.Now.Millisecond );

            int index1 = random.Next( points.Length - 1 );
            int index2 = random.Next( points.Length - 1 );
            int index3 = random.Next( points.Length - 1 );

            Point3D point1 = points[ index1 ];
            Point3D point2 = points[ index2 ];
            Point3D point3 = points[ index3 ];

            PlaneDescriptor planeSescriptor = SpaceManager.GetPlaneByThreePoints
                ( point1, point2, point3 );

            return planeSescriptor;
        }
        //---------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------
    }
}
