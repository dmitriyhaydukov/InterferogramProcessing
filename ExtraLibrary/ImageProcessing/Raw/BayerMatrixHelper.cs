using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class BayerMatrixHelper
    {
        public static ComponentEnum GetComponent(int rowIndex, int columnIndex)
        {
            int rowMod = rowIndex % 2;
            int columnMod = columnIndex % 2;

            if (rowMod == 0 && columnMod == 0) //Red
            {
                return ComponentEnum.Red;
            }
            if (rowMod == 0 && columnMod == 1) //Green1
            {
                return ComponentEnum.Green1;
            }
            if (rowMod == 1 && columnMod == 0) // Green2
            {
                return ComponentEnum.Green2;
            }
            if (rowMod == 1 && columnMod == 1) //Blue
            {
                return ComponentEnum.Blue;
            }

            return 0;
        }
        
        public static int? GetComponentValue(
            
            IntegerMatrix matrix, 
            int rowIndex, int columnIndex,
            ComponentLocationEnum componentLocation
        )
        {
            int targetRow = 0;
            int targetColumn = 0;

            int? resValue = null;
            bool isCorrect = true;

            switch (componentLocation)
            {
                case ComponentLocationEnum.Nord:
                    {
                        targetRow = rowIndex - 1;
                        targetColumn = columnIndex;
                        isCorrect = targetRow >= 0;
                        break;
                    }

                case ComponentLocationEnum.East:
                    {
                        targetRow = rowIndex;
                        targetColumn = columnIndex + 1;
                        isCorrect = targetColumn < matrix.ColumnCount;
                        break;
                    }
                case ComponentLocationEnum.South:
                    {
                        targetRow = rowIndex + 1;
                        targetColumn = columnIndex;
                        isCorrect = targetRow < matrix.RowCount;
                        break;
                    }

                case ComponentLocationEnum.West:
                    {
                        targetRow = rowIndex;
                        targetColumn = columnIndex - 1;
                        isCorrect = targetColumn >= 0;
                        break;
                    }

                case ComponentLocationEnum.NordWest:
                    {
                        targetRow = rowIndex - 1;
                        targetColumn = columnIndex - 1;
                        isCorrect = targetRow >= 0 && targetColumn >= 0;
                        break;
                    }

                case ComponentLocationEnum.NordEast:
                    {
                        targetRow = rowIndex - 1;
                        targetColumn = columnIndex + 1;
                        isCorrect = targetRow >= 0 && targetColumn < matrix.ColumnCount;
                        break;
                    }

                case ComponentLocationEnum.SouthEast:
                    {
                        targetRow = rowIndex + 1;
                        targetColumn = columnIndex + 1;
                        isCorrect = targetRow < matrix.RowCount && targetColumn < matrix.ColumnCount;
                        break;
                    }

                case ComponentLocationEnum.SouthWest:
                    {
                        targetRow = rowIndex + 1;
                        targetColumn = columnIndex - 1;
                        isCorrect = targetRow < matrix.RowCount && targetColumn >= 0;
                        break;
                    }
            }

            if (isCorrect)
            {
                resValue = matrix[targetRow, targetColumn];
            }
            else
            {
                resValue = null;
            }

            return resValue;
        }
    }
}
