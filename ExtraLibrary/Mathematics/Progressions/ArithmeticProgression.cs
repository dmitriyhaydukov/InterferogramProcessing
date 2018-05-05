using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Mathematics.Progressions {
    //Арифметическая прогрессия
    public class ArithmeticProgression {
        //-----------------------------------------------------------------------------------
        public static double[] GetValues( double startValue, double step, int count ) {
            double[] values = new double[ count ];
            values[ 0 ] = startValue;
            for ( int index = 1; index < count; index++ ) {
                values[ index ] = values[ index - 1 ] + step;
            }
            return values;
        }
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------
    }
}
