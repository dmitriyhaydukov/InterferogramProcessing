using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Numerics;
using System.Diagnostics;
 
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Randomness;

namespace ExtraLibrary.Mathematics.Matrices {

    //Матрица комплексных чисел
    [Serializable]
    public class ComplexMatrix {

        private int rowCount;           //Количество строк
        private int columnCount;        //Количество столбцов

        private Complex[ , ] dataArray;  //Массив данных   

        //-----------------------------------------------------------------------------------------
        #region Constructors
        //Конструкторы
        public ComplexMatrix( int rowCount, int columnCount, Complex initializeValue ) {
            this.rowCount = rowCount;
            this.columnCount = columnCount;

            this.dataArray = new Complex[ this.rowCount, this.columnCount ];

            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    this.dataArray[ row, column ] = initializeValue;
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        public ComplexMatrix( int rowCount, int columnCount ) :
            this( rowCount, columnCount, 0 ) {
        }
        //-----------------------------------------------------------------------------------------
        public ComplexMatrix( Complex[ , ] array ) {
            this.rowCount = array.GetLength( 0 );
            this.columnCount = array.GetLength( 1 );

            this.dataArray = new Complex[ this.rowCount, this.columnCount ];
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    this.dataArray[ row, column ] = array[ row, column ];
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        public ComplexMatrix( RealMatrix realMatrix ) {
            this.rowCount = realMatrix.RowCount;
            this.columnCount = realMatrix.ColumnCount;

            this.dataArray = new Complex[ this.rowCount, this.columnCount ];
            for ( int row = 0; row < this.RowCount; row++ ) {
                for ( int column = 0; column < this.ColumnCount; column++ ) {
                    double real = realMatrix[row, column];
                    double imaginary = 0;
                    this.dataArray[ row, column ] = new Complex( real, imaginary );
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        public ComplexMatrix(ComplexNumber[] array, int width, int height)
        {
            this.rowCount = height;
            this.columnCount = width;

            this.dataArray = new Complex[ this.rowCount, this.columnCount ];

            int k = 0;
            for (int row = 0; row < this.rowCount; row++)
            {
                for (int col = 0; col < this.columnCount; col++)
                {
                    this.dataArray[row, col] = new Complex(array[k].Real, array[k].Imaginary);
                    k++;
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        #endregion
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //Количество строк
        public int RowCount {
            get {
                return this.rowCount;
            }
        }
        //-----------------------------------------------------------------------------------------
        //Количество столбцов
        public int ColumnCount {
            get {
                return this.columnCount;
            }
        }
        //-----------------------------------------------------------------------------------------
        //Индексатор
        public Complex this[ int row, int column ] {
            get {
                return this.dataArray[ row, column ];
            }
            set {
                this.dataArray[ row, column ] = value;
            }
        }
        //-----------------------------------------------------------------------------------------
        //Строка матрицы в виде массива
        public Complex[] GetRow( int row ) {
            if ( this.rowCount <= row ) {
                throw new MatrixException();
            }

            Complex[] rowData = new Complex[ this.columnCount ];

            for ( int column = 0; column < this.columnCount; column++ ) {
                rowData[ column ] = this.dataArray[ row, column ];
            }

            return rowData;
        }
        //-----------------------------------------------------------------------------------------
        //Столбец матрицы в виде массива
        public Complex[] GetColumn( int column ) {
            if ( this.columnCount <= column ) {
                throw new MatrixException();
            }

            Complex[] columnData = new Complex[ this.rowCount ];

            for ( int row = 0; row < this.rowCount; row++ ) {
                columnData[ row ] = this.dataArray[ row, column ];
            }
            return columnData;
        }
        //------------------------------------------------------------------------------------------
        //Установить строку
        public void SetRowData( int row, Complex[] dataRow ) {
            if ( dataRow.Length != this.ColumnCount ) {
                throw new MatrixException();
            }
            for ( int column = 0; column < this.columnCount; column++ ) {
                this.dataArray[ row, column ] = dataRow[ column ];
            }
        }
        //------------------------------------------------------------------------------------------
        //Установить стобец
        public void SetColumnData( int column, Complex[] dataColumn ) {
            if ( dataColumn.Length != this.RowCount ) {
                throw new MatrixException();
            }
            for ( int row = 0; row < this.rowCount; row++ ) {
                this.dataArray[ row, column ] = dataColumn[ row ];
            }
        }

        //-----------------------------------------------------------------------------------------
        //Сложение матриц
        public static ComplexMatrix operator +( ComplexMatrix matrixOne, ComplexMatrix matrixTwo ) {
            int rowCountMatrixOne = matrixOne.rowCount;
            int rowCountMatrixTwo = matrixTwo.rowCount;
            int columnCountMatrixOne = matrixOne.columnCount;
            int columnCountMatrixTwo = matrixTwo.columnCount;

            bool equalitySize = ComplexMatrix.IsEqualMatricesSize( matrixOne, matrixTwo );

            if ( equalitySize ) {
                ComplexMatrix resultMatrix = new ComplexMatrix( rowCountMatrixOne, columnCountMatrixOne );
                for ( int row = 0; row < resultMatrix.rowCount; row++ ) {
                    for ( int column = 0; column < resultMatrix.columnCount; column++ ) {
                        resultMatrix[ row, column ] =
                            matrixOne[ row, column ] + matrixTwo[ row, column ];
                    }
                }
                return resultMatrix;
            }
            else {
                throw new MatrixException();
            }
        }
        //------------------------------------------------------------------------------------------
        //Разность матриц
        public static ComplexMatrix operator -( ComplexMatrix matrixOne, ComplexMatrix matrixTwo ) {
            int rowCountMatrixOne = matrixOne.rowCount;
            int rowCountMatrixTwo = matrixTwo.rowCount;
            int columnCountMatrixOne = matrixOne.columnCount;
            int columnCountMatrixTwo = matrixTwo.columnCount;

            bool sizeEquality = ComplexMatrix.IsEqualMatricesSize( matrixOne, matrixTwo );
            if ( !sizeEquality ) {
                throw new MatrixException();
            }

            ComplexMatrix resultMatrix = new ComplexMatrix( rowCountMatrixOne, columnCountMatrixOne );
            for ( int row = 0; row < resultMatrix.rowCount; row++ ) {
                for ( int column = 0; column < resultMatrix.columnCount; column++ ) {
                    resultMatrix[ row, column ] =
                        matrixOne[ row, column ] - matrixTwo[ row, column ];
                }
            }
            return resultMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Проверка размеров матриц
        public static bool IsEqualMatricesSize(
            ComplexMatrix matrixOne,
            ComplexMatrix matrixTwo
        ) {
            bool equalityRowsCount = ( matrixOne.rowCount == matrixTwo.rowCount );
            bool equalityColumnsCount = ( matrixOne.columnCount == matrixTwo.columnCount );
            bool result = equalityRowsCount && equalityColumnsCount;
            return result;
        }
        //------------------------------------------------------------------------------------------
        //Матрица реальных частей
        public RealMatrix GetRealMatrix() {
            RealMatrix realMatrix = new RealMatrix(this.RowCount, this.ColumnCount);
            for ( int row = 0; row < this.RowCount; row++ ) {
                for ( int column = 0; column < this.ColumnCount; column++ ) {
                    realMatrix[ row, column ] = this[ row, column ].Real;
                }
            }
            return realMatrix;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //Матрица с размером степени 2
        public ComplexMatrix GetMatrixAtSizePowerOfTwoGreaterCurrentSize() {
            int newRowCount = MathHelper.GetNextHighestPowerOfTwo( this.RowCount );
            int newColumnCount = MathHelper.GetNextHighestPowerOfTwo( this.ColumnCount );

            ComplexMatrix newMatrix = new ComplexMatrix( newRowCount, newColumnCount );
            int rowTopLeft = ( newRowCount - this.RowCount ) / 2;
            int columnTopLeft = ( newColumnCount - this.ColumnCount ) / 2;
            newMatrix.SetSubMatrix( this, rowTopLeft, columnTopLeft );

            return newMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Установка подматрицы
        public void SetSubMatrix( ComplexMatrix matrix, int rowTopLeft, int columnTopLeft ) {
            bool incorrectParameters =
                this.RowCount < rowTopLeft + matrix.RowCount ||
                this.ColumnCount < columnTopLeft + matrix.ColumnCount;
            if ( incorrectParameters) {
                throw new MatrixException();
            }
            for (
                int row = rowTopLeft, subMatrixRow = 0; subMatrixRow < matrix.RowCount;
                row++, subMatrixRow++
            ) {
                for (
                    int column = columnTopLeft, subMatrixColumn = 0; subMatrixColumn < matrix.ColumnCount;
                    column++, subMatrixColumn++
                ) {
                    this[ row, column ] = matrix[ subMatrixRow, subMatrixColumn ];
                }
            }
        }
        //------------------------------------------------------------------------------------------
        public ComplexNumber[] ToArrayByRows()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int count = this.RowCount * this.ColumnCount;
            ComplexNumber[] array = new ComplexNumber[count];
            int k = 0;
            for (int row = 0; row < this.RowCount; row++)
            {
                for (int col = 0; col < this.ColumnCount; col++)
                {
                    Complex c = this[row, col];
                    array[k] = new ComplexNumber(c.Real, c.Imaginary);
                    k++;
                }
            }

            sw.Stop();
            Console.WriteLine("Copy matrix to one dimaensional array: {0}", sw.Elapsed);

            return array;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------
    }
}
