using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Randomness;

using System.Numerics;

namespace ExtraLibrary.Arraying.ArrayCreation {
    public class ArrayCreator {
        //--------------------------------------------------------------------------------------
        //Создание массива значений по арифметической прогрессии
        public static double[] CreateLinearSeriesArray(
            double startValue,
            double step,
            int count
        ) {
            double[] array = new double[ count ];
            array[ 0 ] = startValue;
            for ( int index = 1; index < count; index++ ) {
                array[ index ] = array[ index - 1 ] + step;
            }
            return array;
        }
        //--------------------------------------------------------------------------------------
        //Массив случайных значений от 0 до 1
        public static double[] CreateRandomArray( int size ) {
            RandomNumberGenerator randomGenerator = new RandomNumberGenerator();
            double[] randomArray = new double[ size ];
            for ( int index = 0; index < size; index++ ) {
                randomArray[ index ] = randomGenerator.GetNextDouble();
            }
            return randomArray;
        }
        //--------------------------------------------------------------------------------------
        //Создание массива комплексных значений
        public static Complex[] CreateComplexArray( double[] array ) {
            Complex[] complexArray = new Complex[ array.Length ];
            for ( int index = 0; index < array.Length; index++ ) {
                complexArray[ index ] = new Complex( array[ index ], 0.0 );
            }
            return complexArray;
        }
        //--------------------------------------------------------------------------------------
        
        //--------------------------------------------------------------------------------------
    }
}
