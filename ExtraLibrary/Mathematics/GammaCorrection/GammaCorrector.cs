using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Mathematics.GammaCorrection {
    public class GammaCorrector {
        //--------------------------------------------------------------------------------------------------
        //Гамма-корркция
        public static double GetGammaCorrectedValue(
            double value, double[] coefficients, double[] powers
        ) {
            double sum = 0;
            for ( int index = 0; index < coefficients.Length; index++ ) {
                double coefficient = coefficients[ index ];
                double power = powers[ index ];
                double summand = coefficient * Math.Pow( value, power );
                sum += summand;
            }
            double resultValue = sum;
            return resultValue;
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //Гамма-коррекция значений матрицы
        public static RealMatrix GetGammaCorrectedMatrix(
            RealMatrix matrix, double[] coefficients, double[] powers
        ) {
            RealMatrix newMatrix = new RealMatrix( matrix.RowCount, matrix.ColumnCount );
            for ( int row = 0; row < matrix.RowCount; row++ ) {
                for ( int column = 0; column < matrix.ColumnCount; column++ ) {
                    double value = matrix[ row, column ];
                    double newValue = GammaCorrector.GetGammaCorrectedValue( value, coefficients, powers );
                    newMatrix[ row, column ] = newValue;
                }
            }
            return newMatrix;
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
    }
}
