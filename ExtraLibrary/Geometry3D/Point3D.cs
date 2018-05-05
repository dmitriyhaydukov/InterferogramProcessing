using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Vectors;

namespace ExtraLibrary.Geometry3D {
    //-------------------------------------------------------------------------------------------
    //Точка в пространстве
    public struct Point3D {
        private double x;
        private double y;
        private double z;
        //--------------------------------------------------------------------------------------------
        public Point3D(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        //--------------------------------------------------------------------------------------------
        public Point3D( double[] coordinates ) {
            this.x = coordinates[ 0 ];
            this.y = coordinates[ 1 ];
            this.z = coordinates[ 2 ];
        }
        //--------------------------------------------------------------------------------------------
        public Point3D( RealVector coordinatesVector ) {
            this.x = coordinatesVector[ 0 ];
            this.y = coordinatesVector[ 1 ];
            this.z = coordinatesVector[ 2 ];
        }
        //--------------------------------------------------------------------------------------------
        public double X {
            get {
                return this.x;
            }
            set {
                this.x = value;
            }
        }
        //--------------------------------------------------------------------------------------------
        public double Y {
            get {
                return this.y;
            }
            set {
                this.y = value;
            }
        }
        //--------------------------------------------------------------------------------------------
        public double Z {
            get {
                return this.z;
            }
            set {
                this.z = value;
            }
        }
        //-------------------------------------------------------------------------------------------
        //Сложение
        public static Point3D operator +( Point3D operandOne, Point3D operandTwo ) {
            double newX = operandOne.x + operandTwo.x;
            double newY = operandOne.y + operandTwo.y;
            double newZ = operandOne.z + operandTwo.z;

            Point3D newPoint = new Point3D( newX, newY, newZ );
            return newPoint;
        }
        //-------------------------------------------------------------------------------------------
        //Разность
        public static Point3D operator -( Point3D operandOne, Point3D operandTwo ) {
            double newX = operandOne.x - operandTwo.x;
            double newY = operandOne.y - operandTwo.y;
            double newZ = operandOne.z - operandTwo.z;

            Point3D newPoint = new Point3D( newX, newY, newZ );
            return newPoint;
        }
        //-------------------------------------------------------------------------------------------
        //Расстояние до заданной точки
        public double GetDistanceToPoint( Point3D point ) {
            double distance = SpaceManager.DistanceBetweenTwoPoints( this, point );
            return distance;
        }
        //-------------------------------------------------------------------------------------------
        //Координаты в виде массива
        public double[] GetCoordinates() {
            double[] coordinates = new double[] { this.x, this.y, this.z };
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
        public override string ToString() {
            string stringPresentation =
                "( " + this.X.ToString() + "; " + this.Y.ToString() + "; " + this.Z.ToString() + " )";
            return stringPresentation;
        }
        //-------------------------------------------------------------------------------------------

    }
}
