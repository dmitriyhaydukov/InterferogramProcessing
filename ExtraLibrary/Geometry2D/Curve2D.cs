using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Geometry2D {
    //Кривая на плоскости
    public class Curve2D {

        Point2D[] points;      //Массив точек

        //--------------------------------------------------------------------------------------
        //Конструкторы
        public Curve2D( double[] arrayX, double[] arrayY ) {
            int sizeX = arrayX.Length;
            int sizeY = arrayY.Length;
            
            if ( sizeX != sizeY ) {
                throw new Curve2DException();
            }

            int size = sizeX;
            this.points = new Point2D[ size ];

            for ( int index = 0; index < size; index++ ) {
                double x = arrayX[ index ];
                double y = arrayY[ index ];
                Point2D point = new Point2D( x, y );
                this.points[ index ] = point;
            }
                        
        }
        //--------------------------------------------------------------------------------------
        public Curve2D( Point2D[] arrayPoints ) {
            int size = arrayPoints.Length;
            this.points = new Point2D[ size ];
            for ( int index = 0; index < size; index++ ) {
                Point2D point = arrayPoints[index];
                this.points[ index ] = new Point2D( point );
            }
        }
        //--------------------------------------------------------------------------------------
        //Индексатор
        public Point2D this[ int index ] {
            get {
                Point2D point = this.points[ index ];
                return point;
            }
        }
        //--------------------------------------------------------------------------------------
        //Массив значений X
        public double[] GetArrayX() {
            int size = this.PointsCount;
            double[] array = new double[ size ];
            for ( int index = 0; index < size; index++ ) {
                Point2D point = this.points[index];
                array[ index ] = point.X; 
            }
            return array;
        }
        //--------------------------------------------------------------------------------------
        //Массив значений Y
        public double[] GetArrayY() {
            int size = this.PointsCount;
            double[] array = new double[ size ];
            for ( int index = 0; index < size; index++ ) {
                Point2D point = this.points[ index ];
                array[ index ] = point.Y;
            }
            return array;
        }
        //--------------------------------------------------------------------------------------
        //Количество точек
        public int PointsCount {
            get {
                return this.points.Length;
            }
        }
        //--------------------------------------------------------------------------------------
        //Массив точек
        public Point2D[] GetPoints() {
            Point2D[] arrayPoints = new Point2D[ this.PointsCount ];
            this.points.CopyTo( arrayPoints, 0 );
            return arrayPoints;
        }
        //--------------------------------------------------------------------------------------
        //Поворот на угол angle
        public Curve2D GetRotatedCurve(double angle) {
            int size = this.PointsCount;
            Point2D[] newArrayPoint = new Point2D[ size ];
            for ( int index = 0; index < size; index++ ) {
                Point2D point = this.points[index];
                double x = point.X;
                double y = point.Y;
                double newX =
                    Math.Cos( angle ) * x + Math.Sin( angle ) * y;
                double newY =
                    -Math.Sin( angle ) * x + Math.Cos( angle ) * y;
                Point2D newPoint = new Point2D( newX, newY );
                newArrayPoint[ index ] = newPoint;
            }
            Curve2D newCurve = new Curve2D( newArrayPoint );
            return newCurve;
        }
        //--------------------------------------------------------------------------------------
        //Смещение кривой
        public Curve2D GetDisplacementCurve(
            double axialDisplacementX,
            double axialdisplacementY
        ) {
            Point2D[] newPoints = new Point2D[ this.PointsCount ];
            for ( int index = 0; index < this.PointsCount; index++ ) {
                Point2D point = this.points[index];
                double newX = point.X + axialDisplacementX;
                double newY = point.Y + axialdisplacementY;
                Point2D newPoint = new Point2D( newX, newY );
                newPoints[ index ] = newPoint;
            }
            Curve2D newCurve = new Curve2D( newPoints );
            return newCurve;
        }
        //--------------------------------------------------------------------------------------
        //Растяжение по оси X
        public Curve2D GetStretchCurveAlongAxisX( double coefficientOfstretch ) {
            Point2D[] newPoints = new Point2D[ this.PointsCount ];
            for ( int index = 0; index < this.PointsCount; index++ ) {
                Point2D point = this.points[ index ];
                double newX = point.X * coefficientOfstretch;
                double newY = point.Y;
                Point2D newPoint = new Point2D( newX, newY );
                newPoints[ index ] = newPoint;
            }
            Curve2D newCurve = new Curve2D( newPoints );
            return newCurve;
        }
        //--------------------------------------------------------------------------------------
        //Растяжение по оси Y
        public Curve2D GetStretchCurveAlongAxisY( double coefficientOfstretch ) {
            Point2D[] newPoints = new Point2D[ this.PointsCount ];
            for ( int index = 0; index < this.PointsCount; index++ ) {
                Point2D point = this.points[ index ];
                double newX = point.X;
                double newY = point.Y * coefficientOfstretch;
                Point2D newPoint = new Point2D( newX, newY );
                newPoints[ index ] = newPoint;
            }
            Curve2D newCurve = new Curve2D( newPoints );
            return newCurve;
        }
        //--------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------
    }
}
