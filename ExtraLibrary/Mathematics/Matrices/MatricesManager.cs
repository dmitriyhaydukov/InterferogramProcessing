using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Mathematics.GammaCorrection;

namespace ExtraLibrary.Mathematics.Matrices {
    public class MatricesManager {
        //----------------------------------------------------------------------------------------------
        //Создание матриц
        public static RealMatrix[] CreateMatrices( int rowCount, int columnCount, int count ) {
            RealMatrix[] matrices = new RealMatrix[ count ];
            for ( int index = 0; index < count; index++ ) {
                RealMatrix matrix = new RealMatrix( rowCount, columnCount );
                matrices[ index ] = matrix;
            }
            return matrices;
        }
        //----------------------------------------------------------------------------------------------
        //Получить подматрицы
        public static RealMatrix[] GetSubMatrices(
            int rowTopLeft, int columnTopLeft,
            int rowBottomRight, int columnBottomRight,
            params RealMatrix[] matrices
        ) {
            RealMatrix[] subMatrices = new RealMatrix[ matrices.Length ];
            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[ index ];
                RealMatrix subMatrix = matrix.GetSubMatrix
                    ( rowTopLeft, columnTopLeft, rowBottomRight, columnBottomRight );
                subMatrices[ index ] = subMatrix;
            }
            return subMatrices;
        }
        //----------------------------------------------------------------------------------------------
        //Привести матрицы от диапазона к диапазону
        public static RealMatrix[] TransformMatrices(
            Interval<double> startInterval,
            Interval<double> finishInterval,
            params RealMatrix[] matrices
        ) {
            RealIntervalTransform intervalTransformer = 
                new RealIntervalTransform( startInterval, finishInterval );
            RealMatrix[] newMatrices = new RealMatrix[ matrices.Length ];

            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[ index ];
                RealMatrix newMatrix = 
                    RealMatrixValuesTransform.TransformMatrixValues( matrix, intervalTransformer );
                newMatrices[ index ] = newMatrix;
            }
            return newMatrices;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //Привести матрицы к диапазону
        public static RealMatrix[] TransformMatricesValuesToFinishIntervalValues(
            Interval<double> finishInterval,
            params RealMatrix[] matrices
        ) {
            RealMatrix[] newMatrices = new RealMatrix[ matrices.Length ];

            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[ index ];
                RealMatrix newMatrix = 
                    RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                    ( matrix, finishInterval );
                newMatrices[ index ] = newMatrix;
            }
            return newMatrices;
        }
        //----------------------------------------------------------------------------------------------
        //Средняя матрица
        public static RealMatrix GetAverageMatrix( params RealMatrix[] matrices ) {
            int countMatrix = matrices.Length;
            int rowCount = matrices[ 0 ].RowCount;
            int columnCount = matrices[ 0 ].ColumnCount;
            RealMatrix meanMatrix = new RealMatrix( rowCount, columnCount );
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    double[] values = MatricesManager.GeValuesFromMatrices( row, column, matrices );
                    double meanValue = Statistician.GetMeanValue( values );
                    meanMatrix[ row, column ] = meanValue;
                }
            }
            return meanMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Массив значений в точке из массива матриц
        public static double[] GeValuesFromMatrices(
            int row, int column,
            params RealMatrix[] matrieces
        ) {
            int count = matrieces.Length;
            double[] values = new double[ count ];
            for ( int index = 0; index < count; index++ ) {
                RealMatrix matrix = matrieces[ index ];
                values[ index ] = matrix[ row, column ];
            }
            return values;
        }
        //-----------------------------------------------------------------------------------------------------
        //Установка значений в матрицы для определенной строки и столбца
        public static void SetValuesInMatrices( RealMatrix[] matrices, double[] values, int row, int column ) {
            for ( int index = 0; index < matrices.Length; index++ ) {
                matrices[ index ][ row, column ] = values[ index ];
            }
        }
        //-----------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //Транспонированные матрицы
        public static RealMatrix[] GetTransposedMatrices( params RealMatrix[] matrices ) {
            RealMatrix[] transposedMatrices = new RealMatrix[ matrices.Length ];
            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[ index ];
                RealMatrix transposedMatrix = matrix.GetTransposedMatrix();
                transposedMatrices[ index ] = transposedMatrix;
            }
            return transposedMatrices;
        }
        //-----------------------------------------------------------------------------------------------------
        //Гамма-коррекция матриц
        public static RealMatrix[] GetGammaCorrectedMatrices(
            double[] coefficients, double[] powers, params RealMatrix[] matrices
        ) {
            RealMatrix[] newMatrices = new RealMatrix[ matrices.Length ];
            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[ index ];
                RealMatrix newMatrix = GammaCorrector.GetGammaCorrectedMatrix( matrix, coefficients, powers );
                newMatrices[ index ] = newMatrix;
            }
            return newMatrices;
        }
        //-----------------------------------------------------------------------------------------------------
        public static RealMatrix[] GetGammaCorrectedMatrices(
            double gamma, params RealMatrix[] matrices
        ) {
            RealMatrix[] newMatrices = new RealMatrix[ matrices.Length ];
            
            for ( int index = 0; index < matrices.Length; index++ ) {
                
                RealMatrix matrix = matrices[ index ];
                RealMatrix newMatrix = new RealMatrix( matrix.RowCount, matrix.ColumnCount );
                
                for ( int row = 0; row < matrix.RowCount; row++ ) {
                    for ( int column = 0; column < matrix.ColumnCount; column++ ) {
                        newMatrix[ row, column ] = Math.Pow( matrix[ row, column ], gamma );
                    }
                }

                newMatrices[ index ] = newMatrix;
            }
            return newMatrices;
        }

        //-----------------------------------------------------------------------------------------------------
        //Фильтрация матриц по маске
        public static RealMatrix[] FilterMatricesByMask( RealMatrix[] matrices, RealMatrix mask ) {
            RealMatrix[] newMatrices = new RealMatrix[ matrices.Length ];
            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[ index ];
                RealMatrix newMatrix = MatrixHandler.FilterMatrixByMask( matrix, mask );
                newMatrices[ index ] = newMatrix;
            }
            return newMatrices;
        }
        //-----------------------------------------------------------------------------------------------------
        public static RealMatrix[] GetRealMatrices( ComplexMatrix[] complexMatrices ) {
            RealMatrix[] realMatrices = new RealMatrix[ complexMatrices.Length ];
            for ( int index = 0; index < complexMatrices.Length; index++ ) {
                ComplexMatrix complexMatrix = complexMatrices[ index ];
                RealMatrix realMatrix = complexMatrix.GetRealMatrix();
                realMatrices[ index ] = realMatrix;
            }
            return realMatrices;
        }
        //-----------------------------------------------------------------------------------------------------
        //Логарифмическое преобразование
        public static RealMatrix[] GetLogTransforms( RealMatrix[] matrices, double coefficient ) {
            LogTransform logTransform = new LogTransform( coefficient );
            RealMatrix[] newMatrices = new RealMatrix[ matrices.Length ];
            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[ index ];
                RealMatrix newMatrix = RealMatrixValuesTransform.TransformMatrixValues( matrix, logTransform );
                newMatrices[ index ] = newMatrix;
            }
            return newMatrices;
        }
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
    }
}
