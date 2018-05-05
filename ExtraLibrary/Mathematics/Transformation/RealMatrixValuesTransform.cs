using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Mathematics.Transformation {
    public class RealMatrixValuesTransform {
        //-------------------------------------------------------------------------------------
        //Преобразование значений матрицы 
        public static RealMatrix TransformMatrixValues(
            RealMatrix matrix,
            RealIntervalTransform intervalTransform
        ) {
            int rowCount = matrix.RowCount;
            int columnCount = matrix.ColumnCount;
            RealMatrix transformedMatrix = new RealMatrix( rowCount, columnCount );
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    double value = matrix[ row, column ];
                    double transformedValue = 
                        intervalTransform.TransformToFinishIntervalValue( value );
                    transformedMatrix[ row, column ] = transformedValue;
                }
            }
            return transformedMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        //Преобразование значений матрицы по шаблону
        public static RealMatrix TransformMatrixValues(
            RealMatrix matrix, BitMask2D stencilMatrix,
            RealIntervalTransform intervalTransform, double prickedValue
        ) {
            int rowCount = matrix.RowCount;
            int columnCount = matrix.ColumnCount;
            RealMatrix transformedMatrix = new RealMatrix( rowCount, columnCount );
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    if ( stencilMatrix[ row, column ] == true ) {
                        double value = matrix[ row, column ];
                        double transformedValue = 
                            intervalTransform.TransformToFinishIntervalValue( value );
                        transformedMatrix[ row, column ] = transformedValue;
                    }
                    else {
                        transformedMatrix[ row, column ] = prickedValue;
                    }
                }
            }
            return transformedMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        //Приведение значений матрицы к интервалу
        public static RealMatrix TransformMatrixValuesToFinishIntervalValues(
            RealMatrix matrix, Interval<double> finishInterval
        ) {
            double minValue = matrix.GetMinValue();
            double maxValue = matrix.GetMaxValue();

            Interval<double> startInterval = new Interval<double>( minValue, maxValue );
            RealIntervalTransform intervalTransform = 
                new RealIntervalTransform( startInterval, finishInterval );
            
            int rowCount = matrix.RowCount;
            int columnCount = matrix.ColumnCount;
            RealMatrix transformedMatrix = new RealMatrix( rowCount, columnCount );
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    double value = matrix[ row, column ];
                    double transformedValue = 
                        intervalTransform.TransformToFinishIntervalValue( value );
                    transformedMatrix[ row, column ] = transformedValue;
                }
            }
            return transformedMatrix;
        }
        //---------------------------------------------------------------------------------------
        //Преобразвание значений матрицы
        public static RealMatrix TransformMatrixValues( RealMatrix matrix, LogTransform logTransform ) {
            RealMatrix newMatrix = new RealMatrix( matrix.RowCount, matrix.ColumnCount );
            for ( int row = 0; row < matrix.RowCount; row++ ) {
                for ( int column = 0; column < matrix.ColumnCount; column++ ) {
                    double newValue = logTransform.GetLogTransform( matrix[ row, column ] );
                    newMatrix[ row, column ] = newValue;
                }
            }
            return newMatrix;
        }
        //---------------------------------------------------------------------------------------
    }
}
