using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Diagnostics;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayCreation;
using System.Runtime.InteropServices;

namespace ExtraLibrary.Mathematics.Transformation {
        
    public class FastFourierTransform {

        [DllImport("CudaCalculationFFT.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static System.Int32 Add(int* a, int* b, int* c, int d);

        [DllImport("CudaCalculationFFT.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static System.Int32 FFT(ComplexNumber* inputMatrix, ComplexNumber* outputMatrix, int width, int height);
        
        //----------------------------------------------------------------------------------------------
        public FastFourierTransform() {
                    
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
        //Быстрое центрированное двумерное преобразование Фурье
        public ComplexMatrix GetCenteredFourierTransform2D( RealMatrix matrix ) {
            
            RealMatrix newMatrix = this.GetMatrixForCenteredFourierTransform( matrix );
            //ComplexMatrix resultMatrix = this.GetFourierTransform2D( newMatrix );
            
            //ComplexMatrix resultMatrix = this.GetCudaFourierTransform2D(matrix);

            ComplexMatrix resultMatrix = this.GetCudaFourierTransform2D(newMatrix);
            
            return resultMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Быстрое двумерное преобразование Фурье
        public ComplexMatrix GetFourierTransform2D( RealMatrix matrix )  {
            bool isMatrixSizeCorrect = 
                MathHelper.IsPowerOfTwo(matrix.RowCount)  &&
                MathHelper.IsPowerOfTwo(matrix.ColumnCount);
            
            if ( !isMatrixSizeCorrect ) {
                throw new FourierTransformException();
            }
            
            ComplexMatrix complexMatrix = new ComplexMatrix( matrix );
            ComplexMatrix resultMatrix = new ComplexMatrix( matrix.RowCount, matrix.ColumnCount );
                        
            //Обработка строк
            for ( int row = 0; row < matrix.RowCount; row++ ) {
                Complex[] complexData = complexMatrix.GetRow( row );
                Complex[] fourierTransform = this.GetFourierTransform( complexData );
                resultMatrix.SetRowData( row, fourierTransform );
            }

            //Обработка столбцов
            for ( int column = 0; column < resultMatrix.ColumnCount; column++ ) {
                Complex[] complexData = resultMatrix.GetColumn( column );
                Complex[] fourierTransform = this.GetFourierTransform( complexData );
                resultMatrix.SetColumnData( column, fourierTransform );
            }
            
            return resultMatrix;
        }
        //----------------------------------------------------------------------------------------------
        public ComplexMatrix GetInverseFourierTransform2D( ComplexMatrix fourierTransform2D ) {
            bool isMatrixSizeCorrect =
                MathHelper.IsPowerOfTwo( fourierTransform2D.RowCount ) &&
                MathHelper.IsPowerOfTwo( fourierTransform2D.ColumnCount );

            if ( !isMatrixSizeCorrect ) {
                throw new FourierTransformException();
            }

            ComplexMatrix resultMatrix =
                new ComplexMatrix( fourierTransform2D.RowCount, fourierTransform2D.ColumnCount );

            //Обработка строк
            for ( int row = 0; row < fourierTransform2D.RowCount; row++ ) {
                Complex[] complexData = fourierTransform2D.GetRow( row );
                Complex[] inverseFourierTransform = this.GetInverseFourierTransform( complexData );
                resultMatrix.SetRowData( row, inverseFourierTransform );
            }

            //Обработка столбцов
            for ( int column = 0; column < fourierTransform2D.ColumnCount; column++ ) {
                Complex[] complexData = resultMatrix.GetColumn( column );
                Complex[] inverseFourierTransform = this.GetInverseFourierTransform( complexData );
                resultMatrix.SetColumnData( column, inverseFourierTransform );
            }
                       
            return resultMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //Быстрое преобразование Фурье
        public Complex[] GetFourierTransform( Complex[] array ) {
            if ( !MathHelper.IsPowerOfTwo( array.Length ) ) {
                throw new FourierTransformException();
            }
            int powerOfTwo = MathHelper.GetNextHighestPowerOfTwo( array.Length );
            
            Complex[] resultArray = new Complex[ array.Length ];
            array.CopyTo( resultArray, 0 );
            Complex u, w, t;

            int i, j, ip, k, l;
            int n = Convert.ToInt32( Math.Pow( 2.0, powerOfTwo ) );
            int n1 = n >> 1;

            for ( i = 0, j = 0, k = n1; i < n - 1; i++, j = j + k ) {
                if ( i < j ) {
                    t = resultArray[ j ];
                    resultArray[ j ] = resultArray[ i ];
                    resultArray[ i ] = t;
                }
                k = n1;
                while ( k <= j ) {
                    j = j - k;
                    k = k >> 1;
                }
            }

            for ( l = 1; l <= powerOfTwo; l++ ) {
                int ll = Convert.ToInt32( Math.Pow( 2.0, l ) );
                int ll1 = ll >> 1;
                u = new Complex( 1.0, 0.0 );
                w = new Complex( Math.Cos( Math.PI / ll1 ), Math.Sin( Math.PI / ll1 ) );
                for ( j = 1; j <= ll1; j++ ) {
                    for ( i = j - 1; i < n; i = i + ll ) {
                        ip = i + ll1;
                        t = resultArray[ ip ] * u;
                        resultArray[ ip ] = resultArray[ i ] - t;
                        resultArray[ i ] = resultArray[ i ] + t;
                    }
                    u = u * w;
                }
            }

            for ( i = 0; i < n; i++ ) {
                resultArray[ i ] = resultArray[ i ] / Math.Sqrt( n );
            }
            return resultArray;
        }
        //----------------------------------------------------------------------------------------------
        //Быстрое обратное преобразование Фурье
        public Complex[] GetInverseFourierTransform( Complex[] array ) {
            if ( !MathHelper.IsPowerOfTwo( array.Length ) ) {
                throw new FourierTransformException();
            }
            int powerOfTwo = MathHelper.GetNextHighestPowerOfTwo( array.Length );

            Complex[] resultArray = new Complex[ array.Length ];
            array.CopyTo( resultArray, 0 );
            Complex u, w, t;

            int i, j, ip, k, l;
            int n = Convert.ToInt32( Math.Pow( 2.0, powerOfTwo ) );
            int n1 = n >> 1;

            for ( i = 0, j = 0, k = n1; i < n - 1; i++, j = j + k ) {
                if ( i < j ) {
                    t = resultArray[ j ];
                    resultArray[ j ] = resultArray[ i ];
                    resultArray[ i ] = t;
                }
                k = n1;
                while ( k <= j ) {
                    j = j - k;
                    k = k >> 1;
                }
            }

            for ( l = 1; l <= powerOfTwo; l++ ) {
                int ll = Convert.ToInt32( Math.Pow( 2.0, l ) );
                int ll1 = ll >> 1;
                u = new Complex( 1.0, 0.0 );
                w = new Complex( Math.Cos( Math.PI / ll1 ), Math.Sin( -Math.PI / ll1 ) );
                for ( j = 1; j <= ll1; j++ ) {
                    for ( i = j - 1; i < n; i = i + ll ) {
                        ip = i + ll1;
                        t = resultArray[ ip ] * u;
                        resultArray[ ip ] = resultArray[ i ] - t;
                        resultArray[ i ] = resultArray[ i ] + t;
                    }
                    u = u * w;
                }
            }

            for ( i = 0; i < n; i++ ) {
                resultArray[ i ] = resultArray[ i ] / Math.Sqrt( n );
            }

            return resultArray;
        }
        //----------------------------------------------------------------------------------------------
        public double[] GetFourierTransformSpectrum( Complex[] fourierTransform ) {
            double[] spectrumValues = new double[ fourierTransform.Length ];
            for ( int index = 0; index < fourierTransform.Length; index++ ) {
                Complex fourierTransformValue = fourierTransform[ index ];
                double spectrumValue = fourierTransformValue.Magnitude;
                spectrumValues[ index ] = spectrumValue;
            }
            return spectrumValues;
        }
        //----------------------------------------------------------------------------------------------
        public RealMatrix GetFourierTransformSpectrum2D( ComplexMatrix fourierTransform2D ) {
            RealMatrix resultMatrix = 
                new RealMatrix( fourierTransform2D.RowCount, fourierTransform2D.ColumnCount );
            for ( int row = 0; row < fourierTransform2D.RowCount; row++ ) {
                for ( int column = 0; column < fourierTransform2D.ColumnCount; column++ ) {
                    resultMatrix[ row, column ] = fourierTransform2D[ row, column ].Magnitude;
                }
            }
            return resultMatrix;
        }
        //----------------------------------------------------------------------------------------------
        //Cuda calculation FFT
        public static unsafe ComplexNumber[] GetCudaFFT(ComplexNumber[] input, int width, int height)
        {
            ComplexNumber[] output = new ComplexNumber[input.Length];
            fixed (ComplexNumber* pi = input, po = output)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                FFT(pi, po, width, height);
                sw.Stop();
                
                Console.WriteLine("CUDA FFT Calculation time: {0}", sw.Elapsed);
            }

            return output;
        }
        //----------------------------------------------------------------------------------------------
        public static unsafe int[] AddVectors(int[] a, int[] b, int size)
        {
            int[] c = new int[size];
            fixed (int* pa = a, pb = b, pc = c)
            {
                Add(pa, pb, pc, size);
            }
            return c;
        }
        //----------------------------------------------------------------------------------------------
        public ComplexMatrix GetCudaFourierTransform2D(RealMatrix matrix)
        {
            bool isMatrixSizeCorrect =
                MathHelper.IsPowerOfTwo(matrix.RowCount) &&
                MathHelper.IsPowerOfTwo(matrix.ColumnCount);

            if (!isMatrixSizeCorrect)
            {
                throw new FourierTransformException();
            }

            ComplexMatrix complexMatrix = new ComplexMatrix(matrix);
            
            ComplexNumber[] complexArray = complexMatrix.ToArrayByRows();
            int width = complexMatrix.ColumnCount;
            int height = complexMatrix.RowCount;

            ComplexNumber[] outputArray = FastFourierTransform.GetCudaFFT(complexArray, width, height);
            ComplexMatrix outputMatrix = new ComplexMatrix(outputArray, width, height);

            return outputMatrix;
        }
        //----------------------------------------------------------------------------------------------
        public int[] Add(int[] a, int[] b, int size)
        {
            return FastFourierTransform.AddVectors(a, b, size);
        }


    }
}
