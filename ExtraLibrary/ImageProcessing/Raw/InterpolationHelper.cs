using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class InterpolationHelper
    {
        public static IntegerMatrix GetRedMatrix(IntegerMatrix scanDataMatrix)
        {
            IntegerMatrix resMatrix = new IntegerMatrix(scanDataMatrix.RowCount, scanDataMatrix.ColumnCount);
            
            for (int row = 0; row < scanDataMatrix.RowCount; row++)
            {
                for (int column = 0; column < scanDataMatrix.ColumnCount; column++)
                {
                    ComponentEnum component = BayerMatrixHelper.GetComponent(row, column);
                    if (component == ComponentEnum.Red)
                    {
                        resMatrix[row, column] = scanDataMatrix[row, column];
                    }
                    if (component == ComponentEnum.Green1)
                    {
                        ComponentLocationEnum[] locations = new ComponentLocationEnum[] 
                            { 
                                ComponentLocationEnum.West, 
                                ComponentLocationEnum.East 
                            };
                        resMatrix[row, column] = InterpolateValue(scanDataMatrix, row, column, locations);
                    }
                    if (component == ComponentEnum.Green2)
                    {
                        ComponentLocationEnum[] locations = new ComponentLocationEnum[] 
                            { 
                                ComponentLocationEnum.Nord, 
                                ComponentLocationEnum.South 
                            };
                        resMatrix[row, column] = InterpolateValue(scanDataMatrix, row, column, locations);
                    }
                    if (component == ComponentEnum.Blue)
                    {
                        ComponentLocationEnum[] locations = new ComponentLocationEnum[] 
                            { 
                                ComponentLocationEnum.NordWest,
                                ComponentLocationEnum.NordEast,
                                ComponentLocationEnum.SouthWest,
                                ComponentLocationEnum.SouthEast
                            };
                        resMatrix[row, column] = InterpolateValue(scanDataMatrix, row, column, locations);
                    }
                }
            }
            
            return resMatrix;
        }
        
        public static IntegerMatrix GetGreenMatrix(IntegerMatrix scanDataMatrix)
        {
            IntegerMatrix resMatrix = new IntegerMatrix(scanDataMatrix.RowCount, scanDataMatrix.ColumnCount);

            for (int row = 0; row < scanDataMatrix.RowCount; row++)
            {
                for (int column = 0; column < scanDataMatrix.ColumnCount; column++)
                {
                    ComponentEnum component = BayerMatrixHelper.GetComponent(row, column);
                    if (component == ComponentEnum.Green1 || component == ComponentEnum.Green2)
                    {
                        resMatrix[row, column] = scanDataMatrix[row, column];
                    }
                    if (component == ComponentEnum.Red)
                    {
                        ComponentLocationEnum[] locations = new ComponentLocationEnum[] 
                            { 
                                ComponentLocationEnum.Nord,
                                ComponentLocationEnum.East,
                                ComponentLocationEnum.South,
                                ComponentLocationEnum.West
 
                            };
                        resMatrix[row, column] = InterpolateValue(scanDataMatrix, row, column, locations);
                    }
                    if (component == ComponentEnum.Blue)
                    {
                        ComponentLocationEnum[] locations = new ComponentLocationEnum[] 
                            { 
                                ComponentLocationEnum.Nord,
                                ComponentLocationEnum.East,
                                ComponentLocationEnum.South,
                                ComponentLocationEnum.West 
                            };
                        resMatrix[row, column] = InterpolateValue(scanDataMatrix, row, column, locations);
                    }
                }
            }

            return resMatrix;
        }

        public static IntegerMatrix GetBlueMatrix(IntegerMatrix scanDataMatrix)
        {
            IntegerMatrix resMatrix = new IntegerMatrix(scanDataMatrix.RowCount, scanDataMatrix.ColumnCount);

            for (int row = 0; row < scanDataMatrix.RowCount; row++)
            {
                for (int column = 0; column < scanDataMatrix.ColumnCount; column++)
                {
                    ComponentEnum component = BayerMatrixHelper.GetComponent(row, column);
                    if (component == ComponentEnum.Blue)
                    {
                        resMatrix[row, column] = scanDataMatrix[row, column];
                    }
                    if (component == ComponentEnum.Green1)
                    {
                        ComponentLocationEnum[] locations = new ComponentLocationEnum[] 
                            { 
                                ComponentLocationEnum.Nord, 
                                ComponentLocationEnum.South 
                            };
                        resMatrix[row, column] = InterpolateValue(scanDataMatrix, row, column, locations);
                    }
                    if (component == ComponentEnum.Green2)
                    {
                        ComponentLocationEnum[] locations = new ComponentLocationEnum[] 
                            { 
                                ComponentLocationEnum.West, 
                                ComponentLocationEnum.East 
                            };
                        resMatrix[row, column] = InterpolateValue(scanDataMatrix, row, column, locations);
                    }
                    if (component == ComponentEnum.Red)
                    {
                        ComponentLocationEnum[] locations = new ComponentLocationEnum[] 
                            { 
                                ComponentLocationEnum.NordWest,
                                ComponentLocationEnum.NordEast,
                                ComponentLocationEnum.SouthWest,
                                ComponentLocationEnum.SouthEast
                            };
                        resMatrix[row, column] = InterpolateValue(scanDataMatrix, row, column, locations);
                    }
                }
            }

            return resMatrix;
        }
               
        private static int InterpolateValue(IntegerMatrix matrix, int row, int column, ComponentLocationEnum[] locationsArray)
        {
            int count = 0;
            int resValue = 0;
            double sum = 0;
            
            for (int k = 0; k < locationsArray.Length; k++)
            {
                int? value = BayerMatrixHelper.GetComponentValue(matrix, row, column, locationsArray[k]);
                if (value.HasValue) 
                {
                    sum += value.Value;
                    count++;
                }
            }
            resValue = Convert.ToInt32( sum / count );
            return resValue;
        }

    }
}
