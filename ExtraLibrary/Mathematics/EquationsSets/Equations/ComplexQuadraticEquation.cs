using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ExtraLibrary.Mathematics.Equations {
    public class ComplexQuadraticEquation {
        //Коэффициенты квадратного уравнения
        private Complex a;
        private Complex b;
        private Complex c;

        //Дискриминант
        private Complex discriminant;
        //-----------------------------------------------------------------------------------------
        //Конструктор
        public ComplexQuadraticEquation( Complex a, Complex b, Complex c ) {
            this.a = a;
            this.b = b;
            this.c = c;

            this.discriminant = this.b * this.b - 4 * this.a * this.c;
        }
        //-----------------------------------------------------------------------------------------
        //Корень 1
        private Complex RootOne {
            get {
                Complex rootOne = 
                    ( -this.b + Complex.Sqrt( this.discriminant ) ) / ( 2 * this.a );
                return rootOne;
            }
        }
        //-----------------------------------------------------------------------------------------
        //Корень 2
        private Complex RootTwo {
            get {
                Complex rootTwo =
                    ( -this.b - Complex.Sqrt( this.discriminant ) ) / ( 2 * this.a );
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
        //Корни уравнения
        public static Complex[] GetRoots( Complex a, Complex b, Complex c ) {
            ComplexQuadraticEquation quadraticEquation = new ComplexQuadraticEquation( a, b, c );
            Complex[] roots = quadraticEquation.GetRoots();
            return roots;
        }
        //-----------------------------------------------------------------------------------------
    }
}
