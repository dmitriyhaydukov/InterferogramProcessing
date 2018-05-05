using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ExtraLibrary.Mathematics.Equations {
    //Квадратное уравнение ax^2 + bx + c = 0
    public class QuadraticEquation {

        //Коэффициенты квадратного уравнения
        private double a;
        private double b;
        private double c;

        //Дискриминант
        private double discriminant;
        //-----------------------------------------------------------------------------------------
        //Конструктор
        public QuadraticEquation( double a, double b, double c ) {
            this.a = a;
            this.b = b;
            this.c = c;

            this.discriminant = this.b * this.b - 4 * this.a * this.c;
        }
        //-----------------------------------------------------------------------------------------
        //Корень 1
        private Complex RootOne {
            get {
                this.ValidateRealRoots();
                double rootOne = 
                    ( -this.b + Math.Sqrt( this.discriminant ) ) / ( 2 * this.a );
                return rootOne;
            }
        }
        //-----------------------------------------------------------------------------------------
        //Корень 2
        private Complex RootTwo {
            get {
                this.ValidateRealRoots();
                double rootTwo =
                    ( -this.b - Math.Sqrt( this.discriminant ) ) / ( 2 * this.a );
                return rootTwo;
            }
        }
        //-----------------------------------------------------------------------------------------
        //Корни уравнения
        public Complex[] GetRoots() {
            Complex[] roots = new Complex[2];
            roots[ 0 ] = this.RootOne;
            roots[ 1 ] = this.RootTwo;
            return roots;
        }
        //-----------------------------------------------------------------------------------------
        //Проверка существоования действительных корней
        private void ValidateRealRoots() {
            if ( this.discriminant < 0 ) {
                throw new ComplexRootException();
            }
        }
        //-----------------------------------------------------------------------------------------
        //Корни уравнения
        public static Complex[] GetRoots( double a, double b, double c ) {
            QuadraticEquation quadraticEquation = new QuadraticEquation( a, b, c );
            Complex[] roots = quadraticEquation.GetRoots();
            return roots;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
