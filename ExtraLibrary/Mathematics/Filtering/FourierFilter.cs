using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayCreation;

namespace ExtraLibrary.Mathematics.Filtering {
    //Фильтрация преобразования Фурье
    public class FourierFilter {
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        public FourierFilter() {
        }
        //-----------------------------------------------------------------------------------------------
        public ComplexMatrix FilterFourierTransform(
            ComplexMatrix fourierTransform, RealMatrix filterMatrix
        ) {
            ComplexMatrix newFourierTransform =
                new ComplexMatrix( fourierTransform.RowCount, fourierTransform.ColumnCount );
            for ( int row = 0; row < fourierTransform.RowCount; row++ ) {
                for ( int column = 0; column < fourierTransform.ColumnCount; column++ ) {
                    Complex complexValue = fourierTransform[ row, column ];
                    double filterValue = filterMatrix[ row, column ];
                    double newReal = complexValue.Real * filterValue;
                    double newImaginary = complexValue.Imaginary * filterValue;
                    newFourierTransform[ row, column ] = new Complex( newReal, newImaginary );
                }
            }
            return newFourierTransform;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
    }
}
