using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Vectors;

namespace ExtraLibrary.Geometry3D {
    //Плоскость ax + by + cz + d = 0
    public class PlaneDescriptor {
        private double coefficientOfX;
        private double coefficientOfY;
        private double coefficientOfZ;
        private double absoluteTerm;
        //--------------------------------------------------------------------------------------------
        //Конструкторы
        public PlaneDescriptor(
            double coefficientOfX,
            double coefficientOfY,
            double coefficientOfZ,
            double absoluteTerm
        ) {
            this.coefficientOfX = coefficientOfX;
            this.coefficientOfY = coefficientOfY;
            this.coefficientOfZ = coefficientOfZ;
            this.absoluteTerm = absoluteTerm;
        }

        //--------------------------------------------------------------------------------------------
        public PlaneDescriptor( RealVector coefficientsVector ) {
            this.coefficientOfX = coefficientsVector[ 0 ];
            this.coefficientOfY = coefficientsVector[ 1 ];
            this.coefficientOfZ = coefficientsVector[ 2 ];
            this.absoluteTerm = coefficientsVector[ 3 ];
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
        public double CoefficientOfZ {
            get {
                return this.coefficientOfZ;
            }
            set {
                this.coefficientOfZ = value;
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
        //Координата z
        public double GetCoordinateZ( double x, double y ) {
            double z = 
                - ( this.absoluteTerm + this.coefficientOfX * x + this.coefficientOfY * y ) 
                / this.coefficientOfZ;
            return z;
        }
        //--------------------------------------------------------------------------------------------
        //Вектор коэффициентов
        public RealVector GetCoefficientsVector() {
            double[] coefficients = this.GetCoefficients();
            RealVector coefficientsVector = new RealVector( coefficients );
            return coefficientsVector;
        }

        //--------------------------------------------------------------------------------------------
        //Коэффициенты
        public double[] GetCoefficients() {
            double[] coefficients = new double[] {
                this.coefficientOfX, this.coefficientOfY, this.coefficientOfZ, this.absoluteTerm
            };
            return coefficients;
        }
        //--------------------------------------------------------------------------------------------
        //Коэффициенты вектора нормали
        public double[] GetNormalCoefficients() {
            double[] normalCoefficients = new double[] {
                this.coefficientOfX, this.coefficientOfY, this.coefficientOfZ
            };
            return normalCoefficients;
        }
        //--------------------------------------------------------------------------------------------
        //Вектор нормали
        public RealVector GetNormalVector() {
            double[] normalCoefficients = this.GetNormalCoefficients();
            RealVector normalVector = new RealVector( normalCoefficients );
            return normalVector;
        }
        //--------------------------------------------------------------------------------------------
        //Невязка
        public double GetPointMisalignment( Point3D point ) {
            double misalignment =
                this.coefficientOfX * point.X +
                this.coefficientOfY * point.Y +
                this.coefficientOfZ * point.Z +
                this.absoluteTerm;
            return misalignment;
        }
        //--------------------------------------------------------------------------------------------
        //Невязка для точек
        public double[] GetPointsMisalignments( Point3D[] points ) {
            double[] pointsMisalignments = new double[ points.Length ];
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                double misaligntment = this.GetPointMisalignment( point );
                pointsMisalignments[ index ] = misaligntment;
            }
            return pointsMisalignments;
        }
        //--------------------------------------------------------------------------------------------
        //Вектор, принадлежащий плоскости
        public RealVector GetVectorInPlane() {
            double startX = 0;
            double startY = 0;
            double startZ = this.GetCoordinateZ( startX, startY );

            double finishX = 1;
            double finishY = 1;
            double finishZ = this.GetCoordinateZ( finishX, finishY );

            double x = finishX - startX;
            double y = finishY - startY;
            double z = finishZ - startZ;

            RealVector vector = new RealVector( x, y, z );
            return vector;
               
        }
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
    }
}
