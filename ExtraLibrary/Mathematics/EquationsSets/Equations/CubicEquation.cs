using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ExtraLibrary.Mathematics.Equations {
    //Кубическое уравнение
    public class CubicEquation {
        double a;       //Коэффициент при x^3
        double b;       //Коэффициент при x^2
        double c;       //Коэффициент при x
        double d;       //Свободный член

        //----------------------------------------------------------------------------------------------
        public CubicEquation( double a, double b, double c, double d ) {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
        //-----------------------------------------------------------------------------------------
        //Корни уравнения
        public Complex[] GetRoots() {
            VietoKardanoSolver solver = new VietoKardanoSolver
                ( this.b / this.a, this.c / this.a, this.d / this.a );
            Complex[] roots = solver.Solve();
            return roots;
        }
        //-----------------------------------------------------------------------------------------               
        //Корни уравнения
        public static Complex[] GetRoots(double a, double b, double c, double d) {
            CubicEquation cubicEquation = new CubicEquation( a, b, c, d );
            Complex[] roots = cubicEquation.GetRoots();
            return roots;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
