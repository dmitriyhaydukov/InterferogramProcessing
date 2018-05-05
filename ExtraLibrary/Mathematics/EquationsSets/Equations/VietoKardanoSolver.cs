using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ExtraLibrary.Mathematics.Equations {
    //Решение кубического уравнения вида x^3 + a*x^2 + b*x + c = 0
    public class VietoKardanoSolver {
        double a;       //Коэффициент при x^2
        double b;       //Коэффициент при x
        double c;       //Свободный член
        //----------------------------------------------------------------------------------------------
        public VietoKardanoSolver( double a, double b, double c ) {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        //-----------------------------------------------------------------------------------------
        //Три действительных корня
        private Complex[] GetRealRoots(double q, double r, double qCube) {
            
            double t = Math.Acos( r / Math.Sqrt( qCube ) ) / 3;
            double oneThirdFractionOfA = a / 3;
            double x1 = -2 * Math.Sqrt( q ) * Math.Cos( t ) -
                oneThirdFractionOfA;
            double x2 = -2 * Math.Sqrt( q ) * Math.Cos( t + 2 * Math.PI / 3 ) -
                oneThirdFractionOfA;
            double x3 = -2 * Math.Sqrt( q ) * Math.Cos( t - 2 * Math.PI / 3 ) -
                oneThirdFractionOfA;
            Complex complex1 = new Complex( x1, 0 );
            Complex complex2 = new Complex( x2, 0 );
            Complex complex3 = new Complex( x3, 0 );

            Complex[] roots = new Complex[] { complex1, complex2, complex3 };
            return roots;

        }
        //-----------------------------------------------------------------------------------------
        //Один действительный и два комплексно-сопряженных корня
        private Complex[] GetOneRealAndTwoComplexRoots(double q, double r, double qCube) {
            
            Complex complex1 = new Complex();
            Complex complex2 = new Complex();
            Complex complex3 = new Complex();
            
            double absR = Math.Abs( r );
            if ( q > 0 ) {
                double t = Mathem.Arch( absR / Math.Sqrt( qCube ) ) / 3;
                double x1 = -2 * Math.Sign( r ) * Math.Sqrt( q ) * Mathem.Ch( t ) - a / 3;

                double realPart = Math.Sign( r ) * Math.Sqrt( q ) * Mathem.Ch( t ) - a / 3;
                double imaginaryPart = Math.Sqrt( 3 ) * Math.Sqrt( q ) * Mathem.Sh( t );

                complex1 = new Complex( x1, 0 );
                complex2 = new Complex( realPart, imaginaryPart );
                complex3 = new Complex( realPart, -imaginaryPart );
            }
            else {
                double absQ = Math.Abs( q );
                double t = Mathem.Arsh( absR / Math.Sqrt( absQ * absQ * absQ ) ) / 3;
                double x1 = -2 * Math.Sign( r ) * Math.Sqrt( absQ ) * Mathem.Sh( t ) - a / 3;

                double realPart = Math.Sign( r ) * Math.Sqrt( absQ ) * Mathem.Sh( t ) - a / 3;
                double imaginaryPart = Math.Sqrt( 3 ) * Math.Sqrt( absQ ) * Mathem.Ch( t );

                complex1 = new Complex( x1, 0 );
                complex2 = new Complex( realPart, imaginaryPart );
                complex3 = new Complex( realPart, -imaginaryPart );
            }

            Complex[] roots = new Complex[] { complex1, complex2, complex3 };
            return roots;
        }

        //-----------------------------------------------------------------------------------------      
        //Решить уравнение
        public Complex[] Solve() {
            double q = (a * a - 3 * b ) / 9;
            double r = ( 2 * a * a * a - 9 * a * b + 27 * c ) / 54;
            double rSquare = r * r;
            double qCube = q * q * q;

            double s = qCube - rSquare;
                       
            Complex[] roots = null;

            //Три действительных корня
            if ( s > 0 ) {
                roots = this.GetRealRoots( q, r, qCube );            
            }
            //Один действительный и два комплексно-сопряженных корня
            else if ( s <= 0) {
                roots = this.GetOneRealAndTwoComplexRoots( q, r, qCube );
            }

            return roots;
        }
        //----------------------------------------------------------------------------------------------
        //Решить уравнение
        public static Complex[] Solve( double a, double b, double c ) {
            VietoKardanoSolver vietoKardanoSolver = new VietoKardanoSolver( a, b, c );
            Complex[] roots = vietoKardanoSolver.Solve();
            return roots;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------


    }
}
