using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Numerics;

using ExtraLibrary.Mathematics.Numbers;
using ExtraLibrary.Mathematics.Matrices;

using ExtraLibrary.Arraying.ArrayOperation;

namespace ExtraLibrary.Mathematics.Transformation {
    //Преобразование Фурье
    public class FourierTransform {
        //----------------------------------------------------------------------------------------------
        public FourierTransform() {

        }

        //----------------------------------------------------------------------------------------------
        public Complex[] GetFourierTransform(Complex[] src)
        {
            int count = src.Length;
            Complex[] res = new Complex[count];
            for (int k = 0; k < count; k++)
            {
                res[k] = new Complex(0, 0);
                for (int n = 0; n < count; n++)
                {
                    Complex temp = Complex.FromPolarCoordinates(1, -2 * Math.PI * n * k / count);
                    temp *= src[n];
                    res[k] += temp;
                }
            }
            return res;
        }
        //----------------------------------------------------------------------------------------------
        /*
        public Complex[] GetFourierTransform(Complex[] srcValues)
        {
            int m = srcValues.Length;
            Complex[] res = new Complex[srcValues.Length];

            for (int i = 0; i < srcValues.Length; i++)
            {
                double frequencyValue = i;
                double realSum = 0;
                double imaginarySum = 0;
                for (int j = 0; j < srcValues.Length; j++)
                {
                    double argValue = j;
                    Complex p = srcValues[j];
                    double phase = 2 * Math.PI * i * j / m;

                    realSum += i * Math.Cos(phase);
                    imaginarySum -= srcValues[j] * Math.Sin(phase);
                }
            }
        }
        */
        //----------------------------------------------------------------------------------------------
        //Преобразование Фурье
        public Complex[] GetFourierTransform(
            double[] argumentValues, double[] functionValues, double[] frequencyValues
        ) {
            int m = argumentValues.Length;
            //int count = argumentValues.Length * frequencyValues.Length;

            double[] realValues = new double[ frequencyValues.Length ];
            double[] imaginaryValues = new double[ frequencyValues.Length ];

            for ( int frequencyIndex = 0; frequencyIndex < frequencyValues.Length; frequencyIndex++ ) {
                double frequencyValue = frequencyValues[ frequencyIndex ];
                double realSum = 0;
                double imaginarySum = 0;
                for ( int index = 0; index < argumentValues.Length; index++ ) {
                    double argumentValue = argumentValues[ index ];
                    double functionValue = functionValues[ index ];

                    //double phase = 2 * Math.PI * frequencyValue * argumentValue / m;
                    double phase = 2 * Math.PI * frequencyValue * argumentValue / m;
                    realSum += functionValue * Math.Cos( phase );
                    imaginarySum -= functionValue * Math.Sin( phase );
                }

                realValues[ frequencyIndex ] = realSum / m;
                imaginaryValues[ frequencyIndex ] = imaginarySum / m;
            }

            Complex[] resultValues = NumbersManager.CreateComplexNumbers( realValues, imaginaryValues );
            return resultValues;
        }
        //----------------------------------------------------------------------------------------------
        //Обратное комплексное преобразование Фурье
        public Complex[] GetComplexInvereFourierTransform( double[] argumentValues, Complex[] fourierTransformValues ) {
            int m = argumentValues.Length;
            Complex[] resultValues = new Complex[ m ];
            for ( int index = 0; index < argumentValues.Length; index++ ) {
                double argumentValue = argumentValues[ index ];
                Complex sum = new Complex( 0, 0 );
                for ( int u = 0; u < fourierTransformValues.Length; u++ ) {
                    Complex phase = 2 * Math.PI * u * argumentValue / m;
                    sum += fourierTransformValues[ u ] * Complex.Pow( Math.E, phase );
                }
                resultValues[ index ] = sum;
            }
            return resultValues;
        }
        //----------------------------------------------------------------------------------------------
        //Обратное преобразование Фурье
        public double[] GetInvereFourierTransform( double[] argumentValues, Complex[] fourierTransformValues ) {
            int m = argumentValues.Length;
            double[] resultValues = new double[ m ];

            for ( int index = 0; index < argumentValues.Length; index++ ) {
                
                double argumentValue = argumentValues[ index ];
                double sum = 0;
                for ( int u = 0; u < fourierTransformValues.Length; u++ ) {

                    //double phase = 2 * Math.PI * u * argumentValue / m;

                    //sum += fourierTransformValues[ u ].Magnitude * Math.Cos( phase );
                    //sum += fourierTransformValues[ u ].Real * Math.Cos( phase );

                    //sum += fourierTransformValues[ u ].Real;

                    /*
                    double r = fourierTransformValues[u].Real;
                    double i = fourierTransformValues[u].Imaginary;
                    
                    double phase = Math.Atan( r / i );

                    sum += fourierTransformValues[ u ].Magnitude * Math.Pow( Math.E, phase );
                    */

                    double phase = 2 * Math.PI * u * argumentValue / m;
                    sum +=
                        fourierTransformValues[ u ].Real * Math.Cos( phase ) -
                        fourierTransformValues[ u ].Imaginary * Math.Sin( phase );
                }
                resultValues[ index ] = sum;
            }
            return resultValues;
        }
        //----------------------------------------------------------------------------------------------
        //Центрированное преобразование Фурье
        public Complex[] GetCenteredFourierTransform(
           double[] argumentValues, double[] functionValues, double[] freuencyValues
        ) {
            double[] newFunctionValues =
                this.GetFunctionValuesForCenteredFourierTransform( argumentValues, functionValues );

            Complex[] fourierTransformValues =
                this.GetFourierTransform( argumentValues, newFunctionValues, freuencyValues );
            return fourierTransformValues;
        }
        //----------------------------------------------------------------------------------------------
        public double[] GetFunctionValuesForCenteredFourierTransform(
            double[] argumentValues, double[] functionValues
        ) {
            double[] newFunctionValues = new double[ functionValues.Length ];
            for ( int index = 0; index < functionValues.Length; index++ ) {
                double argumentValue = argumentValues[ index ];
                double functionValue = functionValues[ index ];
                double newFunctionValue = functionValue * Math.Pow( -1, argumentValue );
                newFunctionValues[ index ] = newFunctionValue;
            }
            return newFunctionValues;
        }
        //----------------------------------------------------------------------------------------------
        //Спектр преобразования Фурье
        public double GetFourierTransformSpectrum( Complex fourierTransformValue ) {
            double resultValue = fourierTransformValue.Magnitude;
            return resultValue;
        }
        //----------------------------------------------------------------------------------------------
        //Спектр двумерного преобразования Фурье
        public RealMatrix GetFourierTransformSpectrum2D( ComplexMatrix fourierTransform2D ) {
            RealMatrix fourierTransformSpectrum2D =
                new RealMatrix( fourierTransform2D.RowCount, fourierTransform2D.ColumnCount );
            for ( int row = 0; row < fourierTransform2D.RowCount; row++ ) {
                for ( int column = 0; column < fourierTransform2D.ColumnCount; column++ ) {
                    Complex fourierTransformValue = fourierTransform2D[ row, column ];
                    double fourierTransformSpectrum = this.GetFourierTransformSpectrum( fourierTransformValue );
                    fourierTransformSpectrum2D[ row, column ] = fourierTransformSpectrum;
                }
            }
            return fourierTransformSpectrum2D;
        }
        //----------------------------------------------------------------------------------------------
        //Спектр двумерного преобразования Фурье
        public RealMatrix GetFourierTransformSpectrum2D( RealMatrix fourierTransform2D ) {
            RealMatrix fourierTransformSpectrum2D =
                new RealMatrix( fourierTransform2D.RowCount, fourierTransform2D.ColumnCount );
            for ( int row = 0; row < fourierTransform2D.RowCount; row++ ) {
                for ( int column = 0; column < fourierTransform2D.ColumnCount; column++ ) {
                    Complex fourierTransformValue = fourierTransform2D[ row, column ];
                    double fourierTransformSpectrum = this.GetFourierTransformSpectrum( fourierTransformValue );
                    fourierTransformSpectrum2D[ row, column ] = fourierTransformSpectrum;
                }
            }
            return fourierTransformSpectrum2D;
        }
        //----------------------------------------------------------------------------------------------
        //Спектр преобразования Фурье
        public double[] GetFourierTransformSpectrum( Complex[] fourierTransformValues ) {
            double[] resultValues = new double[ fourierTransformValues.Length ];
            for ( int index = 0; index < fourierTransformValues.Length; index++ ) {
                Complex value = fourierTransformValues[ index ];
                double spectrum = this.GetFourierTransformSpectrum( value );
                resultValues[ index ] = spectrum;
            }
            return resultValues;
        }
        //----------------------------------------------------------------------------------------------
        //Двумерное преобразование Фурье
        public RealMatrix GetFourierTransform2D( RealMatrix matrix ) {
            
            int m = matrix.ColumnCount;
            int n = matrix.RowCount;

            Complex[ , ] transformValues = new Complex[ n, m ];
            for ( int u = 0; u < m; u++ ) {
                for ( int v = 0; v < n; v++ ) {
                    Complex transformValue = this.GetFourierTransformValue( matrix, u, v );
                    transformValues[ v, u ] = transformValue;
                }
            }
            RealMatrix resultMatrix = new RealMatrix( transformValues );
            return resultMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Двумерное центрированнное преобразование Фурье
        public ComplexMatrix GetCenteredFourierTransform2D( RealMatrix matrix ) {
            if ( matrix.RowCount % 2 != 0 || matrix.ColumnCount % 2 != 0 ) {
                throw new FourierTransformException();
            }
            RealMatrix newMatrix = this.GetMatrixForCenteredFourierTransform( matrix );
                        
            int m = matrix.ColumnCount;
            int n = matrix.RowCount;

            ComplexMatrix transformValues = new ComplexMatrix( n, m );
            for ( int u = 1; u <= m; u++ ) {
                for ( int v = 1; v <= n; v++ ) {
                    Complex transformValue = this.GetFourierTransformValue( newMatrix, u, v );
                    transformValues[ v - 1, u - 1 ] = transformValue;
                }
            }
            ComplexMatrix resultMatrix = transformValues;
            return resultMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Значение двумернрого преобразования Фурье для частот u, v
        public Complex GetFourierTransformValue( RealMatrix matrix, double u, double v ) {
            int m = matrix.ColumnCount;
            int n = matrix.RowCount;

            double realSum = 0;
            double imaginarySum = 0;
            for ( int x = 0; x < matrix.ColumnCount; x++ ) {
                for ( int y = 0; y < matrix.RowCount; y++ ) {
                    double phase = 2 * Math.PI * ( u * x / m + v * y / n );
                    realSum += matrix[ y, x ] * Math.Cos( phase );
                    imaginarySum -= matrix[ y, x ] * Math.Sin( phase );
                }
            }
            
            Complex resultValue = new Complex( realSum / ( m * n ), imaginarySum / ( m * n ) );
            return resultValue;
        }
        //----------------------------------------------------------------------------------------------
        //Матрица для центрированного преобразования Фурье
        public RealMatrix GetMatrixForCenteredFourierTransform( RealMatrix matrix ) {
            RealMatrix newMatrix = new RealMatrix( matrix.RowCount, matrix.ColumnCount );
            for ( int x = 0; x < matrix.ColumnCount; x++ ) {
                for ( int y = 0; y < matrix.RowCount; y++ ) {
                    newMatrix[ y, x ] = matrix[ y, x ] * Math.Pow( -1, x + y );
                }
            }
            return newMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Двумерное обратное преобразование Фурье
        //( мнимая часть не учитывается )
        public RealMatrix GetInverseFourierTransform2D( ComplexMatrix fourierTransform2D ) {
            RealMatrix matrix = 
                new RealMatrix(fourierTransform2D.RowCount, fourierTransform2D.ColumnCount);
            for ( int x = 0; x < matrix.ColumnCount; x++ ) {
                for ( int y = 0; y < matrix.RowCount; y++ ) {
                    Complex inverseTransformValue = 
                        this.GetInverseFourierTransformValue( fourierTransform2D, x, y );
                    matrix[ y, x ] = inverseTransformValue.Real;
                }
            }
            RealMatrix resultMatrix = this.GetMatrixForCenteredFourierTransform( matrix );
            return resultMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Значение обратного двумерного преобразования Фурье для x, y
        public Complex GetInverseFourierTransformValue(
            ComplexMatrix fourierTransform2D, double x, double y
        ) {
            int m = fourierTransform2D.ColumnCount;
            int n = fourierTransform2D.RowCount;

            Complex sum = Complex.Zero;
            for ( int u = 0; u < fourierTransform2D.ColumnCount; u++ ) {
                for ( int v = 0; v < fourierTransform2D.RowCount; v++ ) {
                    Complex phase = new Complex( 0, 2 * Math.PI * ( u * x / m + v * y / n ) );
                    sum += fourierTransform2D[ v, u ] * Complex.Pow( Math.E, phase );
                }
            }

            Complex resultValue = sum;
            return resultValue;
        }
        //----------------------------------------------------------------------------------------------
        //Фильтрация по максимальному значению спектра
        public Complex[] FilterByMaxSpectrumValue( Complex[] fourierTransformValues ) {
            double[] spectrumValues = this.GetFourierTransformSpectrum( fourierTransformValues );
            
            int maxValueIndex1 = ArrayOperator.GetMaxValueIndex( spectrumValues );
            spectrumValues[ maxValueIndex1 ] = 0;
            
            int maxValueIndex2 = ArrayOperator.GetMaxValueIndex( spectrumValues );
            spectrumValues[ maxValueIndex2 ] = 0;

            int maxValueIndex3 = ArrayOperator.GetMaxValueIndex( spectrumValues );
            spectrumValues[ maxValueIndex3 ] = 0;
             
            Complex[] resultValues = new Complex[ fourierTransformValues.Length ];
            for ( int index = 0; index < fourierTransformValues.Length; index++ ) {
                resultValues[ index ] = new Complex( 0, 0 );
            }

            resultValues[ maxValueIndex1 ] = fourierTransformValues[ maxValueIndex1 ];
            resultValues[ maxValueIndex2 ] = fourierTransformValues[ maxValueIndex2 ];
            resultValues[ maxValueIndex3 ] = fourierTransformValues[ maxValueIndex3 ];
                                   
            return resultValues;
        }
        //----------------------------------------------------------------------------------------------
        //Первая гармоника
        public Complex GetFirstHarmonic( double[] array ) {
            int n = array.Length;
            double sinSum = 0;
            double cosSum = 0;
            for ( int i = 0; i < n; i++ ) {
                double value = array[ i ];
                sinSum += value * Math.Sin( 2 * Math.PI * i / n );
                cosSum += value * Math.Cos( 2 * Math.PI * i / n );
            }

            Complex result = new Complex( cosSum, sinSum );
            return result;
        }
        //----------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
}
