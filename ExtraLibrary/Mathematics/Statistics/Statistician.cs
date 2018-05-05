using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Geometry2D;

namespace ExtraLibrary.Mathematics.Statistics {
    public class Statistician {
        //-----------------------------------------------------------------------------------
        //Среднее значение
        public static double GetMeanValue( double[] array ) {
            int valuesCount = array.Length;
            double sum = 0;
            for ( int index = 0; index < valuesCount; index++ ) {
                sum += array[ index ];
            }
            double meanValue = sum / valuesCount;
            return meanValue;
        }
        //-----------------------------------------------------------------------------------
        //Дисперсия
        public static double GetDispersion( double[] array ) {
            double meanValue = Statistician.GetMeanValue( array );
            double count = array.Length;
            double sum = 0;
            for ( int index = 0; index < array.Length; index++ ) {
                double value = array[ index ];
                double d = value - meanValue;
                sum += d*d;
            }
            double dispersion = sum / count;
            return dispersion;
        }
        //-----------------------------------------------------------------------------------------------
        //Среднекваддратическая ошибка
        public static double GetRootMeanSquareError( double[] observationValues, double[] estimatedValues ) {
            int n = observationValues.Length;
            double sum = 0;
            for ( int index = 0; index < n; index++ ) {
                double obsValue = observationValues[ index ];
                double estValue = estimatedValues[ index ];
                double difference = obsValue - estValue;
                sum += difference * difference;
            }

            double rootMeanSquareError = Math.Sqrt( sum / n );
            return rootMeanSquareError;
        }
        //-----------------------------------------------------------------------------------------------
        //Среднекваддратическая ошибка для матриц
        public static double GetRootMeanSquareError( RealMatrix observationMatrix, RealMatrix estimatedMatrix ) {
            int n = observationMatrix.RowCount * observationMatrix.ColumnCount;
            double sum = 0;
            for ( int row = 0; row < observationMatrix.RowCount; row++ ) {
                for ( int column = 0; column < observationMatrix.ColumnCount; column++ ) {
                    double obsValue = observationMatrix[ row, column ];
                    double estValue = estimatedMatrix[ row, column ];
                    double difference = obsValue - estValue;
                    sum += difference * difference;
                }
            }

            double rootMeanSquareError = Math.Sqrt( sum / n );
            return rootMeanSquareError;
        }
        
        //-----------------------------------------------------------------------------------------------
        public static RealMatrix GetCovariationMatrix( Point2D[] points2D ) {
            
            double sumY2 = 0;
            double sumX2 = 0;
            double sumXY = 0;

            for ( int index = 0; index < points2D.Length; index++ ) {
                Point2D point = points2D[ index ];
                sumY2 += point.Y * point.Y;
                sumX2 += point.X * point.X;
                sumXY += point.X * point.Y;
            }

            RealMatrix matrix = new RealMatrix( 2, 2 );

            matrix[ 0, 0 ] = sumY2;
            matrix[ 0, 1 ] = sumXY;
            matrix[ 1, 0 ] = sumXY;
            matrix[ 1, 1 ] = sumX2;

            matrix = matrix * ( 1.0 / points2D.Length );

            return matrix;
        }
        //-----------------------------------------------------------------------------------------------
        public static double GetMatricesCovariance(RealMatrix matrixOne, RealMatrix matrixTwo) {
            double result;
            
            double averageOne = matrixOne.GetAverageValue();
            double averageTwo = matrixTwo.GetAverageValue();

            int elementsCount = matrixOne.RowCount * matrixOne.ColumnCount;
            double sum = 0;

            for ( int row = 0; row < matrixOne.RowCount; row++ ) {
                for ( int column = 0; column < matrixOne.ColumnCount; column++ ) {
                    sum += ( matrixOne[ row, column ] - averageOne ) * ( matrixTwo[ row, column ] - averageTwo );
                }
            }
            result = sum / elementsCount;
            return result;
        }
        //-----------------------------------------------------------------------------------------------
        public static double GetMatricesPearsonsCorrelationCcoefficient(RealMatrix matrixOne, RealMatrix matrixTwo) {
            double deviationOne = Statistician.GetMatrixStandardDeviation(matrixOne);
            double deviationTwo = Statistician.GetMatrixStandardDeviation(matrixTwo);
            double result =
                Statistician.GetMatricesCovariance( matrixOne, matrixTwo ) /
                ( deviationOne * deviationTwo );

            return result;
        }
         
        //-----------------------------------------------------------------------------------------------
        public static double GetMatrixPopulationVariance( RealMatrix matrix ) {
            int elementsCount = matrix.RowCount * matrix.ColumnCount;
            double averageValue = matrix.GetAverageValue();
            double sum = 0;

            for ( int row = 0; row < matrix.RowCount; row++ ) {
                for ( int column = 0; column < matrix.ColumnCount; column++ ) {
                    double value = matrix[row, column] - averageValue;
                    sum += value * value;
                }
            }

            double result = sum / elementsCount;
            return result;
        }
        //-----------------------------------------------------------------------------------------------
        public static double GetMatrixStandardDeviation( RealMatrix matrix ) {
            double matrixPopulationVariance = Statistician.GetMatrixPopulationVariance( matrix );
            return Math.Sqrt( matrixPopulationVariance );
        }
        //-----------------------------------------------------------------------------------------------
    }
}
