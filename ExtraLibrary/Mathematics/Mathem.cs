using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ExtraLibrary.Mathematics {
    public class Mathem {
        //-------------------------------------------------------------------------------------
        //Арктангенс (возвращает значение в диапазоне от 0 до 2p)
        public static double Arctg(double y, double x) {
            double correctedX = Math.Abs( x );
            double correctedY = Math.Abs( y );
            double angle = Math.Atan( correctedY / correctedX );
            if ( ( x < 0 ) && ( 0 < y ) ) {
                angle = Math.PI - angle;
            }
            if ( ( x < 0 ) && ( y < 0 ) ) {
                angle = Math.PI + angle;
            }
            if ( ( 0 < x ) && ( y < 0 ) ) {
                angle = 2 * Math.PI - angle;
            }
            return angle;
        }

        //-------------------------------------------------------------------------------------
        //Квадратный корень
        public static Complex SquareRoot( double value ) {
            double realPart;
            double imaginaryPart;
            if ( value >= 0 ) {
                realPart = Math.Sqrt( value );
                imaginaryPart = 0;
            }
            else {
                realPart = 0;
                imaginaryPart = Math.Sqrt( Math.Abs( value ) );
            }

            Complex complex = new Complex( realPart, imaginaryPart );
            return complex;
        }
        //-------------------------------------------------------------------------------------
        //Гиперболический арккосинус
        public static double Arch( double x ) {
            if ( x >= 1 ) {
                double result = Math.Log( x + Math.Sqrt( x * x - 1 ), Math.E );
                return result;
            }
            else {
                throw new Exception();
            }
        }
        //-------------------------------------------------------------------------------------
        //Гиперболический аркcинус
        public static double Arsh( double x ) {
            double result = Math.Log( x + Math.Sqrt( x * x + 1 ), Math.E );
            return result;
        }
        //-------------------------------------------------------------------------------------
        //Гиперболический косинус
        public static double Ch(double x) {
            double result = ( Math.Exp( x ) + Math.Exp( -x ) ) / 2;
            return result;
        }
        //-------------------------------------------------------------------------------------
        //Гиперболический синус
        public static double Sh( double x ) {
            double result = ( Math.Exp( x ) - Math.Exp( -x ) ) / 2;
            return result;
        }
        //-------------------------------------------------------------------------------------
        //Угол в радианах
        public static double GetAngleInRadians( double angleInDegrees ) {
            double angleInRadians = angleInDegrees * Math.PI / 180;
            return angleInRadians;
        }
        //-------------------------------------------------------------------------------------
        //Угол в градусах
        public static double GetAngleInDegrees( double angleInRadians ) {
            double angleInDegrees = 180 / Math.PI * angleInRadians;
            return angleInDegrees;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------

    }
}
