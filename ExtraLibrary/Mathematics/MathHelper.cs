using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Mathematics {
    public class MathHelper {
        //--------------------------------------------------------------------------------------------------------
        public static bool IsPowerOfTwo( int value ) {
            return ( value & ( value - 1 ) ) == 0;
        }
        //--------------------------------------------------------------------------------------------------------
        public static int GetNextHighestPowerOfTwo( int value ) {
            int power = Convert.ToInt32( Math.Ceiling( Math.Log( value ) / Math.Log( 2 ) ) );
            return power;
        }
        //--------------------------------------------------------------------------------------------------------
        //Минимальное целое число степени 2, большее заданного
        public static int GetNextHighestNumberAtPowerOfTwo( int value ) {
            int power = MathHelper.GetNextHighestPowerOfTwo( value );
            int number = Convert.ToInt32( Math.Pow( 2, power ) );
            return number;
            
            /*
            int power = 0;
            int number = 1;
            while ( number < value ) {
                power++;
                number = Convert.ToInt32( Math.Pow( 2, power ) );
            }
            return number;
            */
            
            /*
            int number = value;

            number--;
            number |= number >> 1;
            number |= number >> 2;
            number |= number >> 4;
            number |= number >> 8;
            number |= number >> 16;
            number++;

            return number;
            */ 
        }
        //--------------------------------------------------------------------------------------------------------
        //Максимальное целое число степени 2, меньшее заданного
        public static int GetPreviousHighestNumberAtPowerOfTwo( int value ) {
            int power = 0;
            int number = 1;
            while ( number < value ) {
                power++;
                number = Convert.ToInt32( Math.Pow( 2, power ) );
            }
            if ( value < number ) {
                power--;
                number = Convert.ToInt32( Math.Pow( 2, power ) );
            }
            return number;
        }
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        public static double[] GetEigenValuesMatrix2x2( RealMatrix matrix ) {
            
            double a11 = matrix[ 0, 0 ];
            double a22 = matrix[ 1, 1 ];
            double a12 = matrix[ 0, 1 ];

            double lambda1 =
                ( a11 + a22 ) / 2 -
                Math.Sqrt( a11 * a11 - 2 * a11 * a22 + 4 * a12 * a12 + a22 * a22 ) / 2;

            double lambda2 =
                ( a11 + a22 ) / 2 +
                Math.Sqrt( a11 * a11 - 2 * a11 * a22 + 4 * a12 * a12 + a22 * a22 ) / 2;

            return new double[] { lambda1, lambda2 };
        }
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
    }
}
