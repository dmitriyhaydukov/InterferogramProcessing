using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ExtraLibrary.Mathematics.Matrices {
    public class MatrixHandler {
        //------------------------------------------------------------------------------------------------
        //Фильтрация матрицы по маске
        public static RealMatrix FilterMatrixByMask(RealMatrix matrix, RealMatrix mask) {
            bool isMaskCorrect =
                ( mask.RowCount == mask.ColumnCount ) &&
                ( ( mask.RowCount % 2 ) != 0 ) &&
                ( ( mask.ColumnCount % 2 ) != 0 );
            if (!isMaskCorrect) {
                throw new MatrixException();
            }
                        
            int windowSize = mask.RowCount;
            int newWidth = matrix.ColumnCount - windowSize + 1;
            int newHeight = matrix.RowCount - windowSize + 1;

            RealMatrix newMatrix = new RealMatrix( newHeight, newWidth );
            
            int halfSize = windowSize / 2;

            int startX = halfSize;
            int startY = halfSize;

            int finishX = matrix.ColumnCount - halfSize - 1;
            int finishY = matrix.RowCount - halfSize - 1;

            for ( int x = startX, newX = 0; x <= finishX; x++, newX++ ) {
                for ( int y = startY, newY = 0; y <= finishY; y++, newY++ ) {
                    RealMatrix matrixArea =
                        matrix.GetSubMatrix( y - halfSize, x - halfSize, y + halfSize, x + halfSize );
                    double newValue = MatrixHandler.GetFilteredValue( matrixArea, mask );
                    newMatrix[ newY, newX ] = newValue;
                }
            }
            return newMatrix;
        }
        //------------------------------------------------------------------------------------------------
        //Фильтрованное значение
        public static double GetFilteredValue( RealMatrix matrix, RealMatrix mask ) {
            double sum = 0;
            for ( int x = 0; x < matrix.ColumnCount; x++ ) {
                for ( int y = 0; y < matrix.RowCount; y++ ) {
                    sum += matrix[ y, x ] * mask[ y, x ];
                }
            }
            return sum;
        }
        //------------------------------------------------------------------------------------------------
        //Фильтрующая маска по среднему значению
        public static RealMatrix GetFilteredMaskByAverageValue( int size ) {
            RealMatrix matrix = new RealMatrix( size, size, 1.0 );
            RealMatrix resultMatrix = matrix * ( 1.0 / ( matrix.RowCount * matrix.ColumnCount ) );
            return resultMatrix;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
