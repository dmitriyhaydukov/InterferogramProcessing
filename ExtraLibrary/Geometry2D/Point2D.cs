using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Vectors;

namespace ExtraLibrary.Geometry2D {
    //Точка на плоскости
    [Serializable]
    public struct Point2D {
        double x;
        double y;
        //------------------------------------------------------------------------------------------------
        //Конструкторы
        public Point2D( double x, double y ) {
            this.x = x;
            this.y = y;
        }
        //------------------------------------------------------------------------------------------------
        public Point2D( Point2D point ) {
            this.x = point.x;
            this.y = point.y;
        }
        //------------------------------------------------------------------------------------------------
        public Point2D( double[] coordinates ) {
            this.x = coordinates[ 0 ];
            this.y = coordinates[ 1 ];
        }
        //------------------------------------------------------------------------------------------------
        public Point2D( RealVector coordinatesVector ) {
            this.x = coordinatesVector[ 0 ];
            this.y = coordinatesVector[ 1 ];
        }
        //------------------------------------------------------------------------------------------------
        //Координата X
        public double X {
            get {
                return this.x;
            }
            set {
                this.x = value;
            }
        }
        //------------------------------------------------------------------------------------------------
        //Координата Y
        public double Y {
            get {
                return this.y;
            }
            set {
                this.y = value;
            }
        }
        //------------------------------------------------------------------------------------------------
        //Сложение
        public static Point2D operator +(Point2D operandOne, Point2D operandTwo) {
            double newX = operandOne.x + operandTwo.x;
            double newY = operandOne.y + operandTwo.y;
            Point2D newPoint = new Point2D( newX, newY );
            return newPoint;
        }
        //------------------------------------------------------------------------------------------------
        //Разность
        public static Point2D operator -( Point2D operandOne, Point2D operandTwo ) {
            double newX = operandOne.x - operandTwo.x;
            double newY = operandOne.y - operandTwo.y;
            Point2D newPoint = new Point2D( newX, newY );
            return newPoint;
        }
        //------------------------------------------------------------------------------------------------
        //Расстояние до заданной точки
        public double GetDistanceToPoint( Point2D point ) {
            double distance = PlaneManager.DistanceBetweenTwoPoints( this, point );
            return distance;
        }
        //------------------------------------------------------------------------------------------------
        //Координаты в виде массива
        public double[] GetCoordinates() {
            double[] coordinates = new double[] { this.x, this.y };
            return coordinates;
        }
        //-------------------------------------------------------------------------------------------
        //Координаты в виде вектора
        public RealVector GetCoordinatesVector() {
            double[] coordinates = this.GetCoordinates();
            RealVector vector = new RealVector( coordinates );
            return vector;
        }
        //-------------------------------------------------------------------------------------------
        //В виде строки
        public override string ToString() {
            return "Point " + "X: " + this.x + " " + "Y: " + this.y;
        }
        //------------------------------------------------------------------------------------------------
        //Точка с минимальной координатой X
        public static Point2D GetPointWithMinimumX( Point2D[] points ) {
            if ( points == null ) {
                throw new Exception();
            }
            Point2D resultPoint = points[ 0 ];
            for ( int index = 1; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                if ( point.X < resultPoint.X ) {
                    resultPoint = point;
                }
            }
            return resultPoint;
        }
        //----------------------------------------------------------------------------------------------
        //Точка с минимальной координатой Y
        public static Point2D GetPointWithMinimumY( Point2D[] points ) {
            if ( points == null ) {
                throw new Exception();
            }
            Point2D resultPoint = points[ 0 ];
            for ( int index = 1; index < points.Length; index++ ) {
                Point2D point = points[ index ];
                if ( point.Y < resultPoint.Y ) {
                    resultPoint = point;
                }
            }
            return resultPoint;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
}
