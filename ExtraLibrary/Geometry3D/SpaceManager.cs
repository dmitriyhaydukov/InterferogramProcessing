using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Arraying.ArrayCreation;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Vectors;

namespace ExtraLibrary.Geometry3D {
    public class SpaceManager {
        //-------------------------------------------------------------------------------------------
        //Расстояние между двумя точками
        public static double DistanceBetweenTwoPoints( Point3D pointOne, Point3D pointTwo ) {
            double subX = pointOne.X - pointTwo.X;
            double subY = pointOne.Y - pointTwo.Y;
            double subZ = pointOne.Z - pointTwo.Z;
            double distance = Math.Sqrt( subX * subX + subY * subY + subZ * subZ );
            return distance;
        }
        //-------------------------------------------------------------------------------------------
        //Расстояния между точками массива и заданной точкой
        public static double[] GetDistances( Point3D[] points, Point3D targetPoint ) {
            double[] distances = new double[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                double distance = SpaceManager.DistanceBetweenTwoPoints( point, targetPoint );
                distances[ index ] = distance;
            }
            return distances;
        }
        //-------------------------------------------------------------------------------------------
        //Формирование 3D-точек из матрицы
        //(столбец - координата X, строка - координата Y, значение - координата Z)
        public static Point3D[] CreatePoints3DFromMatrix( RealMatrix matrix ) {
            Point3D[] points = new Point3D[ matrix.RowCount * matrix.ColumnCount ];
            int index = 0;
            for ( int row = 0; row < matrix.RowCount; row++ ) {
                for ( int column = 0; column < matrix.ColumnCount; column++ ) {
                    double x = column;
                    double y = row;
                    double z = matrix[ row, column ];
                    Point3D point = new Point3D( x, y, z );
                    points[ index ] = point;
                    index++;
                }
            }
            return points;
        }
        //-------------------------------------------------------------------------------------------
        //Плоскость, проходящая через три заданные точки
        public static PlaneDescriptor GetPlaneByThreePoints(
            Point3D point1,
            Point3D point2,
            Point3D point3
        ) {
            double subX2X1 = point2.X - point1.X;
            double subY2Y1 = point2.Y - point1.Y;
            double subZ2Z1 = point2.Z - point1.Z;
            
            double subX3X1 = point3.X - point1.X;
            double subY3Y1 = point3.Y - point1.Y;
            double subZ3Z1 = point3.Z - point1.Z;

            double coefficientOfX = subY2Y1 * subZ3Z1 - subZ2Z1 * subY3Y1;
            double coefficientOfY = subZ2Z1 * subX3X1 - subX2X1 * subZ3Z1;
            double coefficientOfZ = subX2X1 * subY3Y1 - subY2Y1 * subX3X1;

            double absoluteTerm =
                point1.X * subZ2Z1 * subY3Y1 - point1.X * subY2Y1 * subZ3Z1 +
                point1.Y * subX2X1 * subZ3Z1 - point1.Y * subZ2Z1 * subX3X1 +
                point1.Z * subY2Y1 * subX3X1 - point1.Z * subX2X1 * subY3Y1;
            PlaneDescriptor planeDescriptor = new PlaneDescriptor
                ( coefficientOfX, coefficientOfY, coefficientOfZ, absoluteTerm );
            return planeDescriptor;
        }
        //-----------------------------------------------------------------------------------------------
        //Фильтрация точек по координате Z
        public static List<Point3D> GetPointsFilteredByMinimumCoordinateZ(
            Point3D[] points, double minimumThreshold
        ) {
            List<Point3D> filteredPoints = new List<Point3D>();
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                if ( point.Z >= minimumThreshold ) {
                    filteredPoints.Add( point );
                }
            }
            return filteredPoints;
        }
        //----------------------------------------------------------------------------------------------
        //Перемещение точек
        public static Point3D[] DisplacePoints(
            Point3D[] points,
            double displacementX,
            double displacementY,
            double displacementZ
        ) {
            Point3D[] newPoints = new Point3D[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                double newX = point.X + displacementX;
                double newY = point.Y + displacementY;
                double newZ = point.Z + displacementZ;
                Point3D newPoint = new Point3D( newX, newY, newZ );
                newPoints[ index ] = newPoint;
            }
            return newPoints;
        }
        //----------------------------------------------------------------------------------------------
        //Координаты X точек
        public static double[] GetCoordinatesX( Point3D[] points ) {
            double[] coordinatesX = new double[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                coordinatesX[ index ] = point.X;
            }
            return coordinatesX;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //Координаты Y точек
        public static double[] GetCoordinatesY( Point3D[] points ) {
            double[] coordinatesY = new double[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                coordinatesY[ index ] = point.Y;
            }
            return coordinatesY;
        }
        //----------------------------------------------------------------------------------------------
        //Координаты Z точек
        public static double[] GetCoordinatesZ( Point3D[] points ) {
            double[] coordinatesZ = new double[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                coordinatesZ[ index ] = point.Z;
            }
            return coordinatesZ;
        }
        //----------------------------------------------------------------------------------------------
        //Минимальные координаты точек
        public static double[] GetMinimalCoordinates( Point3D[] points ) {
            double[] coordinatesX = SpaceManager.GetCoordinatesX( points );
            double[] coordinatesY = SpaceManager.GetCoordinatesY( points );
            double[] coordinatesZ = SpaceManager.GetCoordinatesZ( points );

            double minX = coordinatesX.Min();
            double minY = coordinatesY.Min();
            double minZ = coordinatesZ.Min();

            double[] minimalCoordinates = new double[] { minX, minY, minZ };
            return minimalCoordinates;
        }
        //----------------------------------------------------------------------------------------------
        //Максимальные координаты точек
        public static double[] GetMaximalCoordinates( Point3D[] points ) {
            double[] coordinatesX = SpaceManager.GetCoordinatesX( points );
            double[] coordinatesY = SpaceManager.GetCoordinatesY( points );
            double[] coordinatesZ = SpaceManager.GetCoordinatesZ( points );

            double maxX = coordinatesX.Max();
            double maxY = coordinatesY.Max();
            double maxZ = coordinatesZ.Max();

            double[] maximalCoordinates = new double[] { maxX, maxY, maxZ };
            return maximalCoordinates;
        }
        //----------------------------------------------------------------------------------------------
        //Перемещение точек в первый октант
        public static Point3D[] DisplacePointsToFirstOctant(Point3D[] points) {
            double[] minimalCoordinates = SpaceManager.GetMinimalCoordinates( points );
            double displacementX = minimalCoordinates[ 0 ];
            double displacementY = minimalCoordinates[ 1 ];
            double displacementZ = minimalCoordinates[ 2 ];

            Point3D[] newPoints = SpaceManager.DisplacePoints
                ( points, -displacementX, -displacementY, -displacementZ );

            return newPoints;
        }
        //----------------------------------------------------------------------------------------------
        public static RealMatrix GetRotationMatrixAroundAxisX( double angle ) {
            double[ , ] arrayMatrix = new double[ , ] {
                {   1,  0,                  0                   },
                {   0,  Math.Cos(angle),    Math.Sin(angle)    },
                {   0,  -Math.Sin(angle),    Math.Cos(angle)     }
            };
            RealMatrix rotationMatrix = new RealMatrix( arrayMatrix );
            return rotationMatrix;
        }
        //----------------------------------------------------------------------------------------------
        public static RealMatrix GetRotationMatrixAroundAxisY( double angle ) {
            double[ , ] arrayMatrix = new double[ , ] {
                {   Math.Cos(angle),    0,  Math.Sin(angle) },
                {   0,                  1,  0               },
                {   -Math.Sin(angle),   0,  Math.Cos(angle) }
            };
            RealMatrix rotationMatrix = new RealMatrix( arrayMatrix );
            return rotationMatrix;
        }
        //----------------------------------------------------------------------------------------------
        public static RealMatrix GetRotationMatrixAroundAxisZ( double angle ) {
            double[ , ] arrayMatrix = new double[ , ] {
                {   Math.Cos(angle),    -Math.Sin(angle),   0   },
                {   Math.Sin(angle),    Math.Cos(angle),    0   },
                {   0,                  0,                  1   }
            };
            RealMatrix rotationMatrix = new RealMatrix( arrayMatrix );
            return rotationMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Матрица поворота от вектора до заданного вектора
        public static RealMatrix GetRotationMatrixToTargetVector(
            RealVector vector, RealVector targetVector
        ) {
            RealVector crossProduct = SpaceManager.GetVectorsCrossProduct( vector, targetVector );
            RealVector normalizedCrossProduct = ( 1 / crossProduct.Length ) * crossProduct;
            double angle = SpaceManager.GetAngleBetweenVectors( vector, targetVector );

            double x = normalizedCrossProduct[ 0 ];
            double y = normalizedCrossProduct[ 1 ];
            double z = normalizedCrossProduct[ 2 ];

            double[ , ] arrayA = new double[ , ] {
                {   0,      -z,     y   },
                {   z,      0,      -x  },
                {   -y,     x,      0   }
            };
            RealMatrix matrixA = new RealMatrix( arrayA );
            RealMatrix rotationMatrix =
                RealMatrix.IdentityMatrix( 3 ) + matrixA * Math.Sin( angle ) +
                ( matrixA * matrixA ) * ( 1 - Math.Cos( angle ) );
            
            return rotationMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Угол между векторами
        public static double GetAngleBetweenVectors( RealVector vectorOne, RealVector vectorTwo ) {
            double scalarProduct = vectorOne * vectorTwo;
            double cosAngle = scalarProduct / ( vectorOne.Length * vectorTwo.Length );
            double angle = Math.Acos( cosAngle );
            return angle;
        }
        //----------------------------------------------------------------------------------------------
        //Повернуть вектор
        public static RealVector RotateVector( RealVector vector, RealMatrix rotationMatrix ) {
            RealVector resultVector = rotationMatrix * vector;
            return resultVector;
        }
        //----------------------------------------------------------------------------------------------
        //vector и auxiliaryVector задают плоскость, в которой
        //vector будет повернут на угол angle
        public static RealVector RotateVectorInPlane(
            RealVector vector, RealVector auxiliaryVector, double angle
        ) {
            RealVector crossProductOne = SpaceManager.GetVectorsCrossProduct( vector, auxiliaryVector );
            RealVector crossProductTwo = SpaceManager.GetVectorsCrossProduct( vector, crossProductOne );
            RealVector newVector =
                Math.Cos( angle ) * vector + Math.Sin( angle ) * crossProductTwo;
            return newVector;
        }
        //----------------------------------------------------------------------------------------------
        //Повернуть векторы
        public static RealVector[] RotateVectors(
            RealVector[] vectors,
            RealMatrix rotationMatrix
        ) {
            RealVector[] newVectors = new RealVector[ vectors.Length ];
            for ( int index = 0; index < vectors.Length; index++ ) {
                RealVector vector = vectors[ index ];
                RealVector newVector = SpaceManager.RotateVector( vector, rotationMatrix );
                newVectors[ index ] = newVector;
            }
            return newVectors;
        }
        //----------------------------------------------------------------------------------------------
        //Векторы координат точек
        public static RealVector[] GetCoordinatesVectors( Point3D[] points ) {
            RealVector[] coordinatesVectors = new RealVector[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                RealVector coordinatesVector = point.GetCoordinatesVector();
                coordinatesVectors[ index ] = coordinatesVector;
            }
            return coordinatesVectors;
        }
        //----------------------------------------------------------------------------------------------
        //Создание точек из векторов координат
        public static Point3D[] CreatePoints3DFromVectors( RealVector[] vectors ) {
            Point3D[] points = new Point3D[ vectors.Length ];
            for ( int index = 0; index < vectors.Length; index++ ) {
                RealVector coordinatesVector = vectors[ index ];
                Point3D point = new Point3D( coordinatesVector );
                points[ index ] = point;
            }
            return points;
        }
        //----------------------------------------------------------------------------------------------
        public static Point3D[] CreatePoints3DFromPoints2D( Point2D[] points2D, double coordinateZ ) {
            Point3D[] points3D = new Point3D[ points2D.Length ];
            for ( int index = 0; index < points2D.Length; index++ ) {
                Point2D point2D = points2D[ index ];
                Point3D point3D = new Point3D( point2D.X, point2D.Y, coordinateZ );
                points3D[ index ] = point3D;
            }
            return points3D;
        }
        //----------------------------------------------------------------------------------------------
        //Проекция на плоскость XY
        public static Point2D[] GetProjectionXY( Point3D[] points3D ) {
            Point2D[] points2D = new Point2D[ points3D.Length ];
            for ( int index = 0; index < points3D.Length; index++ ) {
                Point3D point3D = points3D[ index ];
                Point2D point2D = new Point2D( point3D.X, point3D.Y );
                points2D[ index ] = point2D;
            }
            return points2D;
        }
        //----------------------------------------------------------------------------------------------
        //Проекция на плоскость XZ
        public static Point2D[] GetProjectionXZ( Point3D[] points3D ) {
            Point2D[] points2D = new Point2D[ points3D.Length ];
            for ( int index = 0; index < points3D.Length; index++ ) {
                Point3D point3D = points3D[ index ];
                Point2D point2D = new Point2D( point3D.X, point3D.Z );
                points2D[ index ] = point2D;
            }
            return points2D;
        }
        //----------------------------------------------------------------------------------------------
        //Проекция на плоскость YZ
        public static Point2D[] GetProjectionYZ( Point3D[] points3D ) {
            Point2D[] points2D = new Point2D[ points3D.Length ];
            for ( int index = 0; index < points3D.Length; index++ ) {
                Point3D point3D = points3D[ index ];
                Point2D point2D = new Point2D( point3D.Y, point3D.Z );
                points2D[ index ] = point2D;
            }
            return points2D;
        }
        //----------------------------------------------------------------------------------------------
        //Векторное произведение двух векторов
        public static RealVector GetVectorsCrossProduct( RealVector vectorOne, RealVector vectorTwo ) {
            double x1 = vectorOne[ 0 ];     
            double y1 = vectorOne[ 1 ];     
            double z1 = vectorOne[ 2 ];

            double x2 = vectorTwo[ 0 ];
            double y2 = vectorTwo[ 1 ];
            double z2 = vectorTwo[ 2 ];

            double newX = y1 * z2 - y2 * z1;
            double newY = -x1 * z2 + x2 * z1;
            double newZ = x1 * y2 - x2 * y1;

            RealVector crossProductVector = new RealVector( newX, newY, newZ );
            return crossProductVector;
        }
        //----------------------------------------------------------------------------------------------
        //Точка с минимальной координатой X
        public static Point3D GetPointWithMinimumX( Point3D[] points ) {
            if ( points == null ) {
                throw new Exception();
            }
            Point3D resultPoint = points[ 0 ];
            for ( int index = 1; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                if ( point.X < resultPoint.X ) {
                    resultPoint = point;
                }
            }
            return resultPoint;
        }
        //----------------------------------------------------------------------------------------------
        //Точка с минимальной координатой Y
        public static Point3D GetPointWithMinimumY( Point3D[] points ) {
            if ( points == null ) {
                throw new Exception();
            }
            Point3D resultPoint = points[ 0 ];
            for ( int index = 1; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                if ( point.Y < resultPoint.Y ) {
                    resultPoint = point;
                }
            }
            return resultPoint;
        }
        //----------------------------------------------------------------------------------------------
        //Точка с минимальной координатой Z
        public static Point3D GetPointWithMinimumZ( Point3D[] points ) {
            if ( points == null ) {
                throw new Exception();
            }
            Point3D resultPoint = points[ 0 ];
            for ( int index = 1; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                if ( point.Z < resultPoint.Z ) {
                    resultPoint = point;
                }
            }
            return resultPoint;
        }
        //----------------------------------------------------------------------------------------------
        //Средняя точка в пространстве
        public static Point3D GetMidPoint( Point3D[] points ) {
            Point3D accumulatedPoint = new Point3D( 0, 0, 0 );
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                accumulatedPoint += point;
            }
            int n = points.Length;
            double resultX = accumulatedPoint.X / n;
            double resultY = accumulatedPoint.Y / n;
            double resultZ = accumulatedPoint.Z / n;
            
            Point3D resultPoint = new Point3D( resultX, resultY, resultZ );
            return resultPoint;
        }
        //----------------------------------------------------------------------------------------------
        //Точки с индексами, кратными определенному значению
        public static Point3D[] GetPointsByMultipleIndex( Point3D[] points, int multipleValue ) {
            IList<Point3D> filteredPoints = new List<Point3D>();
            for ( int index = 0; index < points.Length; index++ ) {
                if ( index % multipleValue == 0 ) {
                    filteredPoints.Add( points[ index ] );
                }
            }
            return filteredPoints.ToArray();
        }
        //----------------------------------------------------------------------------------------------
        //Точка пересечения плоскости и прямой, заданной двумя точками
        public Point3D GetPlaneAndLineIntersectionPoint(
            PlaneDescriptor planeDescriptor, Point3D linePointOne, Point3D linePointTwo
        ) {
            double m = linePointTwo.X - linePointOne.X;
            double n = linePointTwo.Y - linePointOne.Y;
            double p = linePointTwo.Z - linePointOne.Z;

            double a = planeDescriptor.CoefficientOfX;
            double b = planeDescriptor.CoefficientOfY;
            double c = planeDescriptor.CoefficientOfZ;
            double d = planeDescriptor.AbsoluteTerm;

            double t =
                -( d + a * linePointOne.X + b * linePointOne.Y + c * linePointOne.Z ) /
                 ( a * m + b * n + c * p );

            double x = m * t + linePointOne.X;
            double y = n * t + linePointOne.Y;
            double z = p * t + linePointOne.Z;

            return new Point3D( x, y, z );
        }
        //----------------------------------------------------------------------------------------------
        public static RealVector GetLineDirectingVectorbyTwoPoints(Point3D pointOne, Point3D pointTwo ) {
            double m = pointTwo.X - pointOne.X;
            double n = pointTwo.Y - pointOne.Y;
            double p = pointTwo.Z - pointOne.Z;

            RealVector directingVector = new RealVector( m, n, p );
            return directingVector;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
}
