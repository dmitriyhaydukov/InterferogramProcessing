using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Arraying.ArrayOperation {

    public delegate double ComputationalFunction( double value );

    public class ArrayOperator {
        //--------------------------------------------------------------------------------------------
        //Проверка совпадения размеров массивов
        public static bool IsArraySizesEqual( Array arrayOne, Array arrayTwo ) {
            int arraySizeOne = arrayOne.Length;
            int arraySizeTwo = arrayTwo.Length;

            if ( arraySizeOne == arraySizeTwo ) {
                return true;
            }
            else {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------------
        //Сумма элементов
        public static double GetSum( double[] array ) {
            double sum = 0;
            for ( int index = 0; index < array.Length; index++ ) {
                sum += array[ index ];
            }
            return sum;
        }
        //--------------------------------------------------------------------------------------------
        //Массив абсолютных значений
        public static double[] GetAbsoluteValues( double[] array ) {
            int size = array.Length;
            double[] newArray = new double[ size ];
            for ( int index = 0; index < size; index++ ) {
                double absoluteValue = Math.Abs( array[ index ] );
                newArray[ index ] = absoluteValue;
            }
            return newArray;
        }
        //--------------------------------------------------------------------------------------------
        //Массив значений в степени
        public static double[] GetValuesInPower( double[] array, double power ) {
            double[] resultArray = new double[ array.Length ];
            for ( int index = 0; index < array.Length; index++ ) {
                resultArray[ index ] = Math.Pow( array[ index ], power );
            }
            return resultArray;
        }
        //--------------------------------------------------------------------------------------------
        //Индекс минимального значения
        public static int GetMinValueIndex( double[] array ) {
            if ( array.Length == 1 ) {
                return 0;
            }
            int indexMinValue = 0;
            double minValue = array[ indexMinValue ];
            for ( int index = 1; index < array.Length; index++ ) {
                if ( array[ index ] < minValue ) {
                    minValue = array[ index ];
                    indexMinValue = index;
                }
            }
            return indexMinValue;
        }

        //--------------------------------------------------------------------------------------------
        //Индекс максимального значения
        public static int GetMaxValueIndex( double[] array ) {
            if ( array.Length == 1 ) {
                return 0;
            }
            int indexMaxValue = 0;
            double maxValue = array[ indexMaxValue ];
            for ( int index = 1; index < array.Length; index++ ) {
                if ( maxValue < array[ index ] ) {
                    maxValue = array[ index ];
                    indexMaxValue = index;
                }
            }
            return indexMaxValue;
        }
        //--------------------------------------------------------------------------------------------
        //Вычисление функции для каждого значения массива
        public static double[] ComputeFunction(
            double[] array,
            ComputationalFunction function
        ) {
            int size = array.Length;
            double[] newArray = new double[ size ];
            for ( int index = 0; index < size; index++ ) {
                double value = array[ index ];
                double newValue = function( value );
                newArray[ index ] = newValue;
            }
            return newArray;
        }
        
        //--------------------------------------------------------------------------------------------
        //Скалярное произведение
        public static double ScalarProduct( double[] arrayOne, double[] arrayTwo ) {
            int sizeArrayOne = arrayOne.Length;
            int sizeArrayTwo = arrayTwo.Length;

            if ( sizeArrayOne != sizeArrayTwo ) {
                throw new ArrayOperationException();
            }

            double scalarProduct = 0;
            for ( int index = 0; index < sizeArrayOne; index++ ) {
                scalarProduct += arrayOne[ index ] * arrayTwo[ index ];
            }
            return scalarProduct;
        }
        //--------------------------------------------------------------------------------------------
        //Вывод на консоль
        public static void WriteConsole( Array array ) {
            Console.WriteLine();
            for ( int index = 0; index < array.Length; index++ ) {
                object value = array.GetValue( index );
                Console.WriteLine( "[{0}]\t{1}", index, value );
            }
        }
        //--------------------------------------------------------------------------------------------
        //Сложение каждого элемента с заданным значением
        public static double[] AddValueToEachValue( double[] array, double addOnValue ) {
            int size = array.Length;
            double[] newArray = new double[ size ];
            for ( int index = 0; index < size; index++ ) {
                newArray[ index ] = array[ index ] + addOnValue;
            }
            return newArray;
        }
        //--------------------------------------------------------------------------------------------
        //Массив в виде строки
        public static string ToString( int[] array ) {
            int count = array.Length;
            StringBuilder stringBuilder = new StringBuilder();
            for ( int index = 0; index < count - 1; index++ ) {
                int value = array[ index ];
                stringBuilder.Append( value );
                stringBuilder.Append( " " );
            }
            int lastValue = array[ count - 1 ];
            stringBuilder.Append( lastValue );
            string result = stringBuilder.ToString();
            return result;
        }
        //--------------------------------------------------------------------------------------------
        //Добавление заданной строки к каждой строке в массиве
        public static string[] AddStringToEachValue( string[] array, string addOnString ) {
            string[] newArray = new string[ array.Length ];
            for ( int index = 0; index < array.Length; index++ ) {
                newArray[ index ] = array[ index ] + addOnString;
            }
            return newArray;
        }
        //--------------------------------------------------------------------------------------------
        //Скалярное произведение
        public static int ScalarProduct( int[] arrayOne, int[] arrayTwo ) {
            int sizeArrayOne = arrayOne.Length;
            int sizeArrayTwo = arrayTwo.Length;

            if ( sizeArrayOne != sizeArrayTwo ) {
                throw new ArrayOperationException();
            }

            int scalarProduct = 0;
            for ( int index = 0; index < sizeArrayOne; index++ ) {
                scalarProduct += arrayOne[ index ] * arrayTwo[ index ];
            }
            return scalarProduct;
        }
        //--------------------------------------------------------------------------------------------
        //Получить словарь
        public static Dictionary<int, double> GetDictionary( double[] array ) {
            Dictionary<int, double> dictionary = new Dictionary<int, double>();
            for ( int index = 0; index < array.Length; index++ ) {
                double value = array[ index ];
                dictionary.Add( index, value );
            }
            return dictionary;
        }
        //--------------------------------------------------------------------------------------------
        //Сумма квадратов элементов
        public static double GetSquareSum( double[] array ) {
            double squareSum = 0;
            for ( int index = 0; index < array.Length; index++ ) {
                double x = array[ index ];
                squareSum += x * x;
            }
            return squareSum;
        }
        //--------------------------------------------------------------------------------------------
        //Элементы массива по заданным индексам
        public static T[] GetElementsByIndecies<T>( T[] array, int[] indecies ) {
            T[] newArray = new T[ indecies.Length ];
            for ( int index = 0; index < indecies.Length; index++ ) {
                int indexValue = indecies[ index ];
                T value = array[ indexValue ];
                newArray[ index ] = value;
            }
            return newArray;
        }
        //--------------------------------------------------------------------------------------------
        //Циклический сдвиг с минимальным значением в начале (from finish to start)
        public static double[] GetArrayByCyclingShiftWithMinimumValueInOrigin(
            double[] array, out int shiftsCount
        ) {
            double minValue = array.Min();
            double maxValue = array.Max();

            shiftsCount = 0;
            List<double> values = new List<double>();
            values.AddRange( array );

            while ( values[ 0 ] != minValue ) {
                int lastValueIndex = values.Count - 1;
                double finishValue = values[ lastValueIndex ];
                values.RemoveAt( lastValueIndex );
                values.Insert( 0, finishValue );
                shiftsCount++;
            }
            
            return values.ToArray();
        }
        //--------------------------------------------------------------------------------------------
        //Обратный циклический сдвиг (from start to finish)
        public static double[] GetArrayByReverseCyclingShift( double[] array, int shiftsCount ) {
            List<double> values = new List<double>();
            values.AddRange( array );

            for ( int shiftNumber = 0; shiftNumber < shiftsCount; shiftNumber++ ) {
                double startValue = values[0];
                values.RemoveAt( 0 );
                values.Add( startValue );
            }

            return values.ToArray();
        }
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
    }
}
