using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using ExtraLibrary.Mathematics.Numbers;
using ExtraLibrary.Mathematics.Equations;

namespace ExtraLibrary.Geometry2D {
    //Каноническое уравнение эллипса
    public class CanonicalEllipseDescriptor {
        double radiusX;
        double radiusY;
        //-------------------------------------------------------------------------------------------
        //Конструктор
        public CanonicalEllipseDescriptor( double radiusX, double radiusY ) {
            this.radiusX = radiusX;
            this.radiusY = radiusY;
        }
        //-------------------------------------------------------------------------------------------
        //Эксцентриситет эллипса
        public double GetEccentricity() {
            double bigRadius = Math.Max( this.radiusX, this.radiusY );
            double smallRadius = Math.Min( this.radiusX, this.radiusY );

            double eccentricity =
                Math.Sqrt( 1 - ( smallRadius * smallRadius ) / ( bigRadius * bigRadius ) );
            return eccentricity;
        }
        //-------------------------------------------------------------------------------------------
        //Рассчитать невязку для данной точки
        public double GetMesalignment( Point2D point ) {
            double x = point.X;
            double y = point.Y;

            double a = this.radiusX;
            double b = this.radiusY;

            double discrepancy = Math.Abs( ( x * x ) / ( a * a ) + ( y * y ) / ( b * b ) - 1 );
            return discrepancy;
        }
        //-------------------------------------------------------------------------------------------
        //Коэффициент сжатия эллипса
        public double GetCompressionRatio() {
            double compressionRatio;

            if ( this.radiusX < this.radiusY ) {
                compressionRatio = radiusX / radiusY;
            }
            else {
                compressionRatio = radiusY / radiusX;
            }
            return compressionRatio;
        }
        //-------------------------------------------------------------------------------------------
        //Точки эллипса, соответствующие углам
        public Point2D[] GetPoints(double[] angles) {
            Point2D[] points = new Point2D[ angles.Length ];
            for ( int index = 0; index < angles.Length; index++ ) {
                double angle = angles[ index ];
                double x = this.radiusX * Math.Cos( angle );
                double y = this.radiusY * Math.Sin( angle );
                Point2D point = new Point2D( x, y );
                points[ index ] = point;
            }
            return points;
        }
        //-------------------------------------------------------------------------------------------
        //Ближайшая точка эллипса
        public Point2D GetNearestEllipsePoint( Point2D point ) {
            double a = this.radiusX;
            double b = this.radiusY;

            double x = point.X;
            double y = point.Y;

            double a0 = ( b * b * b * b * y * y ) / ( a * a );
            double a1 = 2 * b * b * ( 1 - ( b * b ) / ( a * a ) ) * y;
            double a2 =
                a * a - 2 * b * b - x * x + ( b * b * b * b ) / ( a * a ) -
                ( b * b * y * y ) / ( a * a );
            double a3 = 2 * ( ( b * b ) / ( a * a ) - 1 ) * y;
            double a4 = -Math.Pow( ( a * a - b * b ) / ( a * b ), 2 );

            Complex[] roots = QuarticEquation.GetRoots( a4, a3, a2, a1, a0 );
            Complex[] integerRoots = NumbersManager.GetNumbersWithZeroImaginaryPart( roots );

            double[] coordinatesY = NumbersManager.GetRealParts( integerRoots );
            double[] coordinatesX = this.GetCoordinatesXOfPossiblePoints( point, coordinatesY );
            
            Point2D[] possiblePoints = PlaneManager.CreatePoints2D( coordinatesX, coordinatesY );
            Point2D[] ellipsePoints = this.SelectPoints( possiblePoints, 0.01 );
                                    
            Point2D nearestPoint = PlaneManager.GetNearestPoint( point, ellipsePoints );
            return nearestPoint;
        }
        //-------------------------------------------------------------------------------------------
        //Координаты X возможных точек эллипса
        private double[] GetCoordinatesXOfPossiblePoints( Point2D point, double[] coordinatesY ) {
            double x0 = point.X;
            double y0 = point.Y;

            double a = this.radiusX;
            double b = this.radiusY;

            double[] coordinatesX = new double[ coordinatesY.Length ];

            for ( int index = 0; index < coordinatesY.Length; index++ ) {
                double y = coordinatesY[ index ];
                double x;
                if ( y != 0 ) {
                    double numerator = x0;
                    double g =  ( b * b ) / ( a * a );
                    double ratioY0toY = y0 / y;
                    double denominator = 1 - g + g * ( ratioY0toY );
                    x = ( numerator / denominator );
                }
                else {
                    x = x0 > 0 ? x = this.radiusX : x = -this.radiusX;
                }
                coordinatesX[ index ] = x;
            }
            return coordinatesX;
        }
        //-------------------------------------------------------------------------------------------
        //Выбрать возможные точки эллипса
        private Point2D[] SelectPoints( Point2D[] points, double maxMisalignment ) {
            List<Point2D> pointsList = new List<Point2D>();
            for ( int index = 0; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                double misalignment = this.GetMesalignment( point );
                if ( misalignment < maxMisalignment ) {
                    pointsList.Add( point );
                }
            }
            return pointsList.ToArray();
        }
        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
    }
}
