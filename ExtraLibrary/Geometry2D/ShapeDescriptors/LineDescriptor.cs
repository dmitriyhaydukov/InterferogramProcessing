using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Geometry2D {
    //Прямая ax + by + c = 0
    public class LineDescriptor {
        private double coefficientOfX;
        private double coefficientOfY;
        private double absoluteTerm;
        //--------------------------------------------------------------------------------------------
        //Конструктор
        public LineDescriptor( double coefficientOfX, double coefficientOfY, double absoluteTerm ) {
            this.coefficientOfX = coefficientOfX;
            this.coefficientOfY = coefficientOfY;
            this.absoluteTerm = absoluteTerm;
        }
        //--------------------------------------------------------------------------------------------
        public double CoefficientOfX {
            get {
                return this.coefficientOfX;
            }
            set {
                this.coefficientOfX = value;
            }
        }
        //--------------------------------------------------------------------------------------------
        public double CoefficientOfY {
            get {
                return this.coefficientOfY;
            }
            set {
                this.coefficientOfY = value;
            }
        }
        //--------------------------------------------------------------------------------------------
        public double AbsoluteTerm {
            get {
                return this.absoluteTerm;
            }
            set {
                this.absoluteTerm = value;
            }
        }
        //--------------------------------------------------------------------------------------------
        //Координата Y
        public double GetCoordinateY( double coordinateX ) {
            double y = 
                ( -this.absoluteTerm - this.coefficientOfX * coordinateX ) / this.coefficientOfY;
            return y;
        }
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //Координата X
        public double GetCoordinateX( double coordinateY ) {
            double x =
                ( -this.absoluteTerm - this.coefficientOfY * coordinateY ) / this.coefficientOfX;
            return x;
        }
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //Координаты Y
        public double[] GetCoordinatesY( double[] coordinatesX ) {
            double[] coordinatesY = new double[ coordinatesX.Length ];
            for ( int index = 0; index < coordinatesX.Length; index++ ) {
                double x = coordinatesX[ index ];
                double y = this.GetCoordinateY( x );
                coordinatesY[ index ] = y;
            }
            return coordinatesY;
        }
        //--------------------------------------------------------------------------------------------
        //Координаты X
        public double[] GetCoordinatesX( double[] coordinatesY ) {
            double[] coordinatesX = new double[ coordinatesY.Length ];
            for ( int index = 0; index < coordinatesY.Length; index++ ) {
                double y = coordinatesY[ index ];
                double x = this.GetCoordinateX( y );
                coordinatesX[ index ] = x;
            }
            return coordinatesX;
        }
        //--------------------------------------------------------------------------------------------
        //Проверка параллельности прямых
        public static bool AreLinesParallel( LineDescriptor lineOne, LineDescriptor lineTwo ) {
            bool isParallel =
                ( lineOne.coefficientOfX / lineOne.coefficientOfY ) ==
                ( lineTwo.coefficientOfX / lineTwo.coefficientOfY );
            return isParallel;
        }
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
    }
}
