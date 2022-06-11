using System;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayOperation;

namespace ExtraLibrary.ImageProcessing {
    //Медианный фильтр
    public class MedianByRowsGrayScaleFilter {
        //------------------------------------------------------------------------------------------------
        //Функция фильтрации
        private double GetFilteredValue(
            double[] array
        ) {
            double filteredValue;
            double[] sortedArray = ArrayOperator.SortArrayAscending(array);
            int n = sortedArray.Length;
            if (n % 2 == 0) {
                filteredValue = (sortedArray[n / 2] + sortedArray[n / 2 + 1]) / 2;
            }
            else {
                filteredValue = sortedArray[n / 2];
            }

            return filteredValue;
        }
        //------------------------------------------------------------------------------------------------
        //Фильтрация строк изображения
        private RealMatrix FiltrateGrayScaleImageRows(
            RealMatrix grayScaleIntensityMatrix,
            int windowSize
        ) {
            int width = grayScaleIntensityMatrix.ColumnCount;
            int height = grayScaleIntensityMatrix.RowCount;

            int step = windowSize / 2;

            RealMatrix newMatrix = new RealMatrix( height, width );

            for ( int row = 0; row < height; row++ ) {
                for ( int column = 0; column < width; column++ ) {
                    
                    int columnLeft = column - step;
                    if ( columnLeft < 0 ) {
                        columnLeft = column;
                    }
                    int columnRight = column + step;
                    if ( width <= columnRight ) {
                        columnRight = column;
                    }

                    double[] originalValues = grayScaleIntensityMatrix.GetRowValues(row, columnLeft, columnRight);
                    
                    double newValue =  this.GetFilteredValue( originalValues );
                    newMatrix[ row, column ] = newValue;
                }
            }
            return newMatrix;
        }
        //------------------------------------------------------------------------------------------------
        //Фильтрация изображения
        public RealMatrix ExecuteFiltration(
            RealMatrix grayScaleIntensityMatrix, int windowSize
        ) {
            RealMatrix filteredRowsMatrix = this.FiltrateGrayScaleImageRows
                ( grayScaleIntensityMatrix, windowSize );

            RealMatrix resultMatrix = filteredRowsMatrix;
            return resultMatrix;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
