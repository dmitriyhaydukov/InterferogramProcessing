using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ExtraLibrary.Mathematics.Numbers {
    public class NumbersManager {
        //----------------------------------------------------------------------------------------------
        //Действительные части комлексных чисел
        public static double[] GetRealParts( params Complex[] array ) {
            double[] realParts = new double[ array.Length ];
            for ( int index = 0; index < array.Length; index++ ) {
                Complex complexNumber = array[ index ];
                double realPart = complexNumber.Real;
                realParts[ index ] = realPart;
            }
            return realParts;
        }
        //----------------------------------------------------------------------------------------------
        //Мнимые части комплексных чисел
        public static double[] GetImaginaryParts( params Complex[] array ) {
            double[] imaginaryParts = new double[ array.Length ];
            for ( int index = 0; index < array.Length; index++ ) {
                Complex complexNumber = array[ index ];
                double imaginaryPart = complexNumber.Real;
                imaginaryParts[ index ] = imaginaryPart;
            }
            return imaginaryParts;
        }
        //----------------------------------------------------------------------------------------------
        //Комплексные числа с нулевой мнимой частью
        public static Complex[] GetNumbersWithZeroImaginaryPart( params Complex[] array ) {
            List<Complex> complexNumbers = new List<Complex>();
            for ( int index = 0; index < array.Length; index++ ) {
                Complex complex = array[ index ];
                if ( complex.Imaginary == 0 ) {
                    complexNumbers.Add( complex );
                }
            }
            return complexNumbers.ToArray();
        }
        //----------------------------------------------------------------------------------------------
        //Создание комплексных чисел
        public static Complex[] CreateComplexNumbers( double[] realValues, double[] imaginaryValues ) {
            Complex[] complexNumbers = new Complex[ realValues.Length ];
            for ( int index = 0; index < realValues.Length; index++ ) {
                double realValue = realValues[ index ];
                double imaginaryValue = imaginaryValues[ index ];

                Complex complex = new Complex( realValue, imaginaryValue );
                complexNumbers[ index ] = complex;
            }
            return complexNumbers;
        }
        //----------------------------------------------------------------------------------------------
        //Создание комплексных чисел
        public static Complex[] CreateComplexNumbers( double[] realValues ) {
            Complex[] complexNumbers = new Complex[ realValues.Length ];
            for ( int index = 0; index < realValues.Length; index++ ) {
                double realValue = realValues[ index ];
                
                Complex complex = new Complex( realValue, 0 );
                complexNumbers[ index ] = complex;
            }
            return complexNumbers;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
}
