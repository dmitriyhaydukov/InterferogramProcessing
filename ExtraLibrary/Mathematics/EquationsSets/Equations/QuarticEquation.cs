using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ExtraLibrary.Mathematics.Equations {
    public class QuarticEquation {
        double a;     //Коэффициент при x^4  
        double b;     //Коэффициент при x^3  
        double c;     //Коэффициент при x^2
        double d;     //Коэффициент при x  
        double e;     //Свободный член
        //-----------------------------------------------------------------------------------------
        public QuarticEquation(
            double a,
            double b,
            double c,
            double d,
            double e
        ) {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
        }
        //-----------------------------------------------------------------------------------------       
        public Complex[] GetRoots() {
            double bb = this.b / this.a;
            double cc = this.c / this.a;
            double dd = this.d / this.a;
            double ee = this.e / this.a;

            double f = cc - 3 * bb * bb / 8;
            double g = dd + bb * bb * bb / 8 - bb * cc / 2;
            double h = ee - 3 * bb * bb * bb * bb / 256 + bb * bb * cc / 16 - bb * dd / 4;

            Complex[] cubicEquationRoots = 
                CubicEquation.GetRoots( 1, f / 2, ( f * f - 4 * h ) / 16, -g * g / 64 );

            Complex p = Complex.Sqrt( cubicEquationRoots[ 1 ] );
            Complex q = Complex.Sqrt( cubicEquationRoots[ 2 ] );

            Complex r = -g / ( 8 * p * q );
            Complex s = b / ( 4 * a );

            Complex x1 = p + q + r - s;
            Complex x2 = p - q - r - s;
            Complex x3 = -p + q - r - s;
            Complex x4 = -p - q + r - s;

            Complex[] roots = new Complex[] { x1, x2, x3, x4 };
            return roots;

        }
        //-----------------------------------------------------------------------------------------           
        public static Complex[] GetRoots(
            double a,
            double b,
            double c,
            double d,
            double e
        ) {
            QuarticEquation quarticEquation = new QuarticEquation( a, b, c, d, e );
            Complex[] roots = quarticEquation.GetRoots();
            return roots;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
