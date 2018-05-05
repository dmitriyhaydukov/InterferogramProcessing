using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Imaging.ImageProcessing {
    //Простой фильтр
    public class SimpleGrayScaleFilter {
        //------------------------------------------------------------------------------------------------
        //Функция фильтрации
        private double GetFilteredValue(
            double prevValue,
            double currentValue,
            double nextValue
        ) {
            double filteredValue = ( prevValue + 2 * currentValue + nextValue ) / 4;
            return filteredValue;
        }
        //------------------------------------------------------------------------------------------------
        //Фильтрация строк изображения
        private RealMatrix FiltrateGrayScaleImageRows(
            RealMatrix grayScaleIntensityMatrix,
            int step
        ) {
            int width = grayScaleIntensityMatrix.ColumnCount;
            int height = grayScaleIntensityMatrix.RowCount;

            RealMatrix newMatrix = new RealMatrix( height, width );

            for ( int row = 0; row < height; row++ ) {
                for ( int column = 0; column < width; column++ ) {
                    double value = grayScaleIntensityMatrix[ row, column ];
                    
                    int columnLeft = column - step;
                    if ( columnLeft < 0 ) {
                        columnLeft = column;
                    }
                    int columnRight = column + step;
                    if ( width <= columnRight ) {
                        columnRight = column;
                    }

                    double valueLeft = grayScaleIntensityMatrix[ row, columnLeft ];
                    double valueRight = grayScaleIntensityMatrix[ row, columnRight ];

                    double newValue = 
                        this.GetFilteredValue( valueLeft, value, valueRight );
                    newMatrix[ row, column ] = newValue;
                }
            }
            return newMatrix;
        }
        //------------------------------------------------------------------------------------------------
        //Фильтрация изображения
        public RealMatrix ExecuteFiltration(
            RealMatrix grayScaleIntensityMatrix, int stepX, int stepY
        ) {
            RealMatrix filteredRowsMatrix = this.FiltrateGrayScaleImageRows
                ( grayScaleIntensityMatrix, stepX );
            RealMatrix transposedFilteredRowsMatrix = filteredRowsMatrix.GetTransposedMatrix();
            RealMatrix filteredColumnsMatrix =
                this.FiltrateGrayScaleImageRows( transposedFilteredRowsMatrix, stepY );
            RealMatrix resultMatrix = filteredColumnsMatrix.GetTransposedMatrix();
            return resultMatrix;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
