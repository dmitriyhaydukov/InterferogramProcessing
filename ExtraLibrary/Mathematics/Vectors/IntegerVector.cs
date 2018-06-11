﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

namespace ExtraLibrary.Mathematics.Vectors
{
    public class IntegerVector
    {
        int[] dataArray;
        //------------------------------------------------------------------------------------
        public IntegerVector(int size)
        {
            this.dataArray = new int[size];
        }
        //------------------------------------------------------------------------------------
        public IntegerVector(params int[] values)
        {
            this.dataArray = new int[values.Length];
            for (int index = 0; index < values.Length; index++)
            {
                this.dataArray[index] = values[index];
            }
        }
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public IntegerVector(Point3D point3D)
            : this(Convert.ToInt32(point3D.X), Convert.ToInt32(point3D.Y), Convert.ToInt32(point3D.Z))
        {
        }
        //------------------------------------------------------------------------------------
        public IntegerVector(IntegerVector vector)
            : this(vector.dataArray)
        {
        }
        //------------------------------------------------------------------------------------
        //Размер
        public int Size
        {
            get
            {
                return this.dataArray.Length;
            }
        }
        //-------------------------------------------------------------------------------------
        //Индексатор
        public int this[int index]
        {
            get
            {
                return this.dataArray[index];
            }
            set
            {
                this.dataArray[index] = value;
            }
        }
        //-------------------------------------------------------------------------------------
        //Массив значений вектора
        public int[] GetDataArray()
        {
            int[] array = new int[this.Size];
            this.dataArray.CopyTo(array, 0);
            return array;
        }
        //-------------------------------------------------------------------------------------
        //Сложение векторов
        public static IntegerVector operator +(IntegerVector operandOne, IntegerVector operandTwo)
        {
            IntegerVector newVector = new IntegerVector(operandOne.Size);
            for (int index = 0; index < newVector.Size; index++)
            {
                newVector[index] = operandOne[index] + operandTwo[index];
            }
            return newVector;
        }
        //-------------------------------------------------------------------------------------
        //Скалярное произведение
        public static int operator *(IntegerVector operandOne, IntegerVector operandTwo)
        {
            int scalarProduct = ArrayOperator.ScalarProduct
                (operandOne.dataArray, operandTwo.dataArray);
            return scalarProduct;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        //Умножение на число
        public static IntegerVector operator *(int value, IntegerVector vector)
        {
            IntegerVector newVector = new IntegerVector(vector.Size);
            for (int index = 0; index < vector.dataArray.Length; index++)
            {
                newVector[index] = vector[index] * value;
            }
            return newVector;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        //Матрица преобразования для вычисления ортогонального вектора
        private IntegerMatrix CreateOrthogonalVectorTransformingMatrix(int size)
        {
            IntegerMatrix transformingMatrix = new IntegerMatrix(size, size);

            for (int column = 1, row = 0; column < size; column++, row++)
            {
                transformingMatrix[row, column] = 1;
            }

            for (int row = 1, column = 0; row < size; row++, column++)
            {
                transformingMatrix[row, column] = -1;
            }

            transformingMatrix[size - 1, 0] = 1;
            transformingMatrix[0, size - 1] = -1;

            return transformingMatrix;
        }
        //-------------------------------------------------------------------------------------------------
        //Ортогональный вектор
        public IntegerVector GetOrthogonalVector()
        {
            IntegerMatrix transformatingMatrix = this.CreateOrthogonalVectorTransformingMatrix(this.Size);
            IntegerVector orthogonalVector = transformatingMatrix * this;
            return orthogonalVector;
        }
        //-------------------------------------------------------------------------------------------------
        //Вывод на консоль
        public void WriteConsole(string name)
        {
            Console.WriteLine(name);
            for (int index = 0; index < dataArray.Length; index++)
            {
                Console.WriteLine(dataArray[index]);
            }
        }
        //------------------------------------------------------------------------------------
        public Point3D ToPoint3D()
        {
            if (this.Size != 3)
            {
                throw new Exception();
            }
            Point3D point3D = new Point3D(this[0], this[1], this[2]);
            return point3D;
        }
        //------------------------------------------------------------------------------------
        //Длина вектора
        public double Length
        {
            get
            {
                double squareSum = ArrayOperator.GetSquareSum(this.dataArray);
                double length = Math.Sqrt(squareSum);
                return length;
            }
        }
        //------------------------------------------------------------------------------------
        public RealVector GetNormalizedVector()
        {
            double length = this.Length;

            RealVector newVector = new RealVector(this.Size);
            for (int index = 0; index < this.Size; index++)
            {
                newVector[index] = this[index] / length;
            }
            return newVector;
        }
        //------------------------------------------------------------------------------------
        //Единичный вектор
        public static IntegerVector GetUnitVector(int size)
        {
            IntegerVector unitVector = new IntegerVector(size);
            for (int index = 0; index < unitVector.Size; index++)
            {
                unitVector[index] = 1;
            }
            return unitVector;
        }
        //------------------------------------------------------------------------------------
        //Строковое представление вектора
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("( ");
            for (int index = 0; index < this.dataArray.Length - 1; index++)
            {
                stringBuilder.Append(this.dataArray[index]);
                stringBuilder.Append(';');
            }
            int lastIndex = this.dataArray.Length - 1;
            stringBuilder.Append(this.dataArray[lastIndex]);
            stringBuilder.Append(" )");

            return stringBuilder.ToString();
        }
        //------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
    }
}
