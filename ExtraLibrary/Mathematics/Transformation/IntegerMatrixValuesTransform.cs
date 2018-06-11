using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Mathematics.Transformation
{
    public class IntegerMatrixValuesTransform
    {
        //-------------------------------------------------------------------------------------
        //Преобразование значений матрицы 
        public static IntegerMatrix TransformMatrixValues(
            IntegerMatrix matrix,
            RealIntervalTransform intervalTransform
        )
        {
            int rowCount = matrix.RowCount;
            int columnCount = matrix.ColumnCount;
            IntegerMatrix transformedMatrix = new IntegerMatrix(rowCount, columnCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    double value = matrix[row, column];
                    double transformedValue =
                        intervalTransform.TransformToFinishIntervalValue(value);
                    transformedMatrix[row, column] = Convert.ToInt32(transformedValue);
                }
            }
            return transformedMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        //Преобразование значений матрицы по шаблону
        public static IntegerMatrix TransformMatrixValues(
            IntegerMatrix matrix, BitMask2D stencilMatrix,
            RealIntervalTransform intervalTransform, int prickedValue
        )
        {
            int rowCount = matrix.RowCount;
            int columnCount = matrix.ColumnCount;
            IntegerMatrix transformedMatrix = new IntegerMatrix(rowCount, columnCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    if (stencilMatrix[row, column] == true)
                    {
                        double value = matrix[row, column];
                        double transformedValue =
                            intervalTransform.TransformToFinishIntervalValue(value);
                        transformedMatrix[row, column] = Convert.ToInt32(transformedValue);
                    }
                    else
                    {
                        transformedMatrix[row, column] = prickedValue;
                    }
                }
            }
            return transformedMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        //Приведение значений матрицы к интервалу
        public static IntegerMatrix TransformMatrixValuesToFinishIntervalValues(
            IntegerMatrix matrix, Interval<double> finishInterval
        )
        {
            double minValue = matrix.GetMinValue();
            double maxValue = matrix.GetMaxValue();

            Interval<double> startInterval = new Interval<double>(minValue, maxValue);
            RealIntervalTransform intervalTransform =
                new RealIntervalTransform(startInterval, finishInterval);

            int rowCount = matrix.RowCount;
            int columnCount = matrix.ColumnCount;
            IntegerMatrix transformedMatrix = new IntegerMatrix(rowCount, columnCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    double value = matrix[row, column];
                    double transformedValue =
                        intervalTransform.TransformToFinishIntervalValue(value);
                    transformedMatrix[row, column] = Convert.ToInt32(transformedValue);
                }
            }
            return transformedMatrix;
        }
        //---------------------------------------------------------------------------------------
        //Преобразвание значений матрицы
        public static IntegerMatrix TransformMatrixValues(IntegerMatrix matrix, LogTransform logTransform)
        {
            IntegerMatrix newMatrix = new IntegerMatrix(matrix.RowCount, matrix.ColumnCount);
            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int column = 0; column < matrix.ColumnCount; column++)
                {
                    double newValue = logTransform.GetLogTransform(matrix[row, column]);
                    newMatrix[row, column] = Convert.ToInt32(newValue);
                }
            }
            return newMatrix;
        }
        //---------------------------------------------------------------------------------------
    }
}