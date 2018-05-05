using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Algorithms {
    //-------------------------------------------------------------------------------------
    //Направление развертки
    public enum UnwrapDirection {
        Up,
        Down
    };
    //-------------------------------------------------------------------------------------
    //Алгоритм развертки
    public class UnwrappingAlgorithm {
        private int trendSignAnalyseSamplesCount = 11;
        //---------------------------------------------------------------------------------
        public UnwrappingAlgorithm() {
            //--
        }
        //--------------------------------------------------------------------------------------------------------------
        //Развертка по строкам
        public RealMatrix UnwrapMatrixByRows(
            RealMatrix matrix, int extremumIndex, UnwrapDirection unwrapDirection,
            double thresholdDifferenceValue, double increasingStep
        ) {
            int rowCount = matrix.RowCount;
            int columnCount = matrix.ColumnCount;
            RealMatrix newMatrix = new RealMatrix( rowCount, columnCount );

            for ( int row = 0; row < rowCount; row++ ) {
                double[] rowData = matrix.GetRow( row );
                double[] correctedRowData = this.DeleteFirstDisplacement( rowData );
                double[] newRowData =
                    this.UnwrapArray( correctedRowData, thresholdDifferenceValue, extremumIndex, unwrapDirection, increasingStep );
                newMatrix.SetRowData( row, newRowData );
            }
            return newMatrix;
        }
        //---------------------------------------------------------------------------------------------------------------
        public double[] DeleteFirstDisplacement(double[] array) {
            double[] newArray = new double[ array.Length ];
            for (int index = 0; index < array.Length; index++) {
                newArray[ index ] = array[ index ] - array[ 0 ];
            }
            return newArray;
        }
        //---------------------------------------------------------------------------------------------------------------
        //Развертка массива
        public double[] UnwrapArray(
            double[] array,
            double thresholdDifferenceValue,    //Пороговое значение разрыва
            int extremumIndex,                  //Индекс экстремума
            UnwrapDirection unwrapDirection,    //Направление развертки
            double increasingStep               //Шаг наращивания
        ) {
            int size = array.Length;
            double[] newArray = new double[ size ];
            
            int trendSign = unwrapDirection == UnwrapDirection.Down ? -1 : 1;
            int previousTrendSign = trendSign;
            bool trendChanging = false;
            double cumulative = 0;

            double newValue;
            for ( int index = extremumIndex + 1; index < size; index++ ) {
                double currentValue = array[ index ];
                double prevValue = array[ index - 1 ];
                if ( thresholdDifferenceValue <= Math.Abs( currentValue - prevValue ) ) {
                    if ( trendSignAnalyseSamplesCount < index ) {
                        double trend = array[ index - 1 - trendSignAnalyseSamplesCount ] - prevValue;
                        trendSign = trend < 0 ? +1 : -1;
                        cumulative = cumulative + trendSign * increasingStep;
                        previousTrendSign = trendSign;
                    }
                    else {
                        double trend = array[ index ] - array[ 0 ];
                        trendSign = trend < 0 ? +1 : -1;
                        cumulative = cumulative + trendSign * increasingStep;
                        previousTrendSign = trendSign;
                    }
                }
                
                newValue = cumulative + currentValue;
                newArray[ index ] = newValue;
            }
                                
            for ( int index = extremumIndex - 1; 0 < index; index-- ) {
                double currentValue = array[ index ];
                double prevValue = array[ index + 1 ];
                if ( thresholdDifferenceValue <= Math.Abs( currentValue - prevValue ) ) {
                    if ( trendSignAnalyseSamplesCount < index ) {
                        double trend = array[ index - 1 - trendSignAnalyseSamplesCount ] - prevValue;
                        trendSign = trend < 0 ? +1 : -1;
                        cumulative = cumulative + trendSign * increasingStep;
                        previousTrendSign = trendSign;
                    }
                }
                newValue = cumulative + cumulative;
                newArray[ index ] = newValue;
            }
            

            return newArray;
        }
        //---------------------------------------------------------------------------------------------------------------
        private int GetSign( double currentValue, double prevValue ) {
            int sign;
            if ( currentValue - prevValue < 0 ) {
                sign = -1;
            }
            else {
                sign = +1;
            }
            return sign;
        }
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
    }
}
