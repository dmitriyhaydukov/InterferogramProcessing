using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arraying.ArrayOperation;
using Mathematics.Statistics;
using Mathematics.Vectors;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Mathematics.Matrices {
    
    [Serializable]
    public class RealMatrix {

        private int rowCount;           //Количество строк
        private int columnCount;        //Количество столбцов

        private double[ , ] dataArray;  //Массив данных   

        //-----------------------------------------------------------------------------------------
        //Конструкторы
        public RealMatrix( int rowCount, int columnCount, double initializeValue ) {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            
            this.dataArray = new double[ this.rowCount, this.columnCount ];
            
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    this.dataArray[ row, column ] = initializeValue;
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        public RealMatrix( int rowCount, int columnCount ) :
            this( rowCount, columnCount, 0 ) {
        }
        //-----------------------------------------------------------------------------------------
        public RealMatrix( double[ , ] array ) {
            int rowCount = array.GetLength( 0 );
            int columnCount = array.GetLength( 1 );

            this.rowCount = rowCount;
            this.columnCount = columnCount;

            this.dataArray = new double[ rowCount, columnCount ];
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    this.dataArray[ row, column ] = array[ row, column ];        
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        public RealMatrix( RealMatrix matrix ) : this( matrix.dataArray ) {
        }
        //-----------------------------------------------------------------------------------------
        //Создание из MatrixInt
        public RealMatrix( MatrixInt matrix ) {
            this.rowCount = matrix.RowCount;
            this.columnCount = matrix.ColumnCount;
            this.dataArray = new double[ this.rowCount, this.columnCount ];
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    this.dataArray[ row, column ] = matrix[ row, column ];
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //Создание из MatrixByte
        public RealMatrix( MatrixByte matrix ) {
            this.rowCount = matrix.RowCount;
            this.columnCount = matrix.ColumnCount;
            this.dataArray = new double[ this.rowCount, this.columnCount ];
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    this.dataArray[ row, column ] = matrix[ row, column ];
                }
            }
        }
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
        public double this[ int row, int column ] {
            get {
                return this.dataArray[ row, column ];
            }
            set {
                this.dataArray[ row, column ] = value;
            }
        }
        //-----------------------------------------------------------------------------------------
        //Строка матрицы в виде массива
        public double[] GetDataRow( int row ) {
            if ( this.rowCount <= row ) {
                throw new MatrixException();
            }

            double[] dataRow = new double[ this.columnCount ];

            for ( int column = 0; column < this.columnCount; column++ ) {
                dataRow[ column ] = this.dataArray[ row, column ];    
            }

            return dataRow;
        }
        //-----------------------------------------------------------------------------------------
        //Столбец матрицы в виде массива
        public double[] GetDataColumn( int column ) {
            if ( this.columnCount <= column ) {
                throw new MatrixException();
            }

            double[] dataColumn = new double[ this.rowCount ];

            for ( int row = 0; row < this.rowCount; row++ ) {
                dataColumn[ row ] = this.dataArray[ row, column ];
            }

            return dataColumn;

        }
        //-----------------------------------------------------------------------------------------
        //Сложение матриц
        public static RealMatrix operator +( RealMatrix matrixOne, RealMatrix matrixTwo ) {
            int rowCountMatrixOne = matrixOne.rowCount;
            int rowCountMatrixTwo = matrixTwo.rowCount;
            int columnCountMatrixOne = matrixOne.columnCount;
            int columnCountMatrixTwo = matrixTwo.columnCount;

            bool equalitySize = RealMatrix.IsEqualMatricesSize( matrixOne, matrixTwo );

            if ( equalitySize ) {
                RealMatrix resultMatrix =
                    new RealMatrix( rowCountMatrixOne, columnCountMatrixOne );
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
        public static RealMatrix operator -( RealMatrix matrixOne, RealMatrix matrixTwo ) {
            int rowCountMatrixOne = matrixOne.rowCount;
            int rowCountMatrixTwo = matrixTwo.rowCount;
            int columnCountMatrixOne = matrixOne.columnCount;
            int columnCountMatrixTwo = matrixTwo.columnCount;

            bool equalitySize = RealMatrix.IsEqualMatricesSize( matrixOne, matrixTwo );

            if ( equalitySize ) {
                RealMatrix resultMatrix =
                    new RealMatrix( rowCountMatrixOne, columnCountMatrixOne );
                for ( int row = 0; row < resultMatrix.rowCount; row++ ) {
                    for ( int column = 0; column < resultMatrix.columnCount; column++ ) {
                        resultMatrix[ row, column ] =
                            matrixOne[ row, column ] - matrixTwo[ row, column ];
                    }
                }
                return resultMatrix;
            }
            else {
                throw new MatrixException();
            }
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //Умножение матриц
        public static RealMatrix operator *( RealMatrix matrixOne, RealMatrix matrixTwo ) {
            int rowCountMatrixOne = matrixOne.rowCount;
            int rowCountMatrixTwo = matrixTwo.rowCount;
            int columnCountMatrixOne = matrixOne.columnCount;
            int columnCountMatrixTwo = matrixTwo.columnCount;

            if ( columnCountMatrixOne != rowCountMatrixTwo ) {
                throw new MatrixException();
            }

            RealMatrix resultMatrix =
                new RealMatrix( rowCountMatrixOne, columnCountMatrixTwo );
            
            for ( int row = 0; row < rowCountMatrixOne; row++ ) {
                double[] dataRow = matrixOne.GetDataRow( row );
                for ( int column = 0; column < columnCountMatrixTwo; column++ ) {
                    double[] dataColumn = matrixTwo.GetDataColumn( column );
                    double result = ArrayDoubleOperator.ScalarProduct( dataRow, dataColumn );
                    resultMatrix[ row, column ] = result;
                }
            }
            return resultMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Умножение на вектор
        public static RealVector operator *( RealMatrix matrix, RealVector vector ) {
            int columnCountMatrix = matrix.columnCount;
            int sizeVector = vector.Size;
            if ( columnCountMatrix != sizeVector ) {
                throw new MatrixException();
            }
            RealVector newVector = new RealVector( sizeVector );
            int rowCountMatrix = matrix.rowCount;
            double[] dataVector = vector.GetDataArray();
            for ( int row = 0; row < rowCountMatrix; row++ ) {
                double[] dataRow = matrix.GetDataRow( row );
                double value = ArrayDoubleOperator.ScalarProduct( dataRow, dataVector );
                newVector[ row ] = value;
            }
            return newVector;
        }
        //------------------------------------------------------------------------------------------
        //Умножение на число
        public static RealMatrix operator *( RealMatrix matrix, double value ) {
            int rowCount = matrix.rowCount;
            int columnCount = matrix.columnCount;
            RealMatrix newMatrix = new RealMatrix( rowCount, columnCount );
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    double newValue = matrix[ row, column ] * value;
                    newMatrix[ row, column ] = newValue;
                }
            }
            return newMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Вывод на консоль
        public void WriteConsole(string matrixName) {
            Console.WriteLine(matrixName);
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    Console.Write( this[ row, column ] + "\t" );
                }
                Console.WriteLine();
            }
        }
        //------------------------------------------------------------------------------------------
        //Вывод на консоль
        public void WriteConsole() {
            this.WriteConsole( "Matrix:" );
        }
        //------------------------------------------------------------------------------------------
        //Транспонирование матрицы
        public RealMatrix GetTransposedMatrix() {
            int columnCountNewMatrix = this.rowCount;
            int rowCountNewMatrix = this.columnCount;

            RealMatrix newMatrix = new RealMatrix( rowCountNewMatrix, columnCountNewMatrix );
            for ( int rowThisMatrix = 0; rowThisMatrix < this.rowCount; rowThisMatrix++ ) {
                for (
                    int columnThisMatrix = 0;
                    columnThisMatrix < this.columnCount;
                    columnThisMatrix++
                ) {
                    int rowNewMatrix = columnThisMatrix;
                    int columnNewMatrix = rowThisMatrix;
                    newMatrix[ rowNewMatrix, columnNewMatrix ] =
                        this.dataArray[ rowThisMatrix, columnThisMatrix ];
                }
            }
            return newMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Дополнительная матрица
        public RealMatrix GetAugmentedMatrix( int rowOfMatrix, int columnOfMatrix ) {
            int rowCountAugmentedMatrix = this.rowCount - 1;
            int columnCountAugmentedMatrix = this.columnCount - 1;

            RealMatrix augmentedMatrix =
                new RealMatrix( rowCountAugmentedMatrix, columnCountAugmentedMatrix );
            int rowAugmentMatrix = 0;
            int columnAugmentMatrix = 0;
            for ( int rowThisMatrix = 0; rowThisMatrix < this.rowCount; rowThisMatrix++ ) {
                if ( rowThisMatrix != rowOfMatrix ) {
                    for (
                        int columnThisMatrix = 0;
                        columnThisMatrix < this.columnCount;
                        columnThisMatrix++
                    ) {
                        if (columnThisMatrix != columnOfMatrix) {
                            augmentedMatrix[ rowAugmentMatrix, columnAugmentMatrix ] =
                                this.dataArray[ rowThisMatrix, columnThisMatrix ];
                            columnAugmentMatrix++;
                        }
                    }
                    rowAugmentMatrix++;
                    columnAugmentMatrix = 0;
                }
            }
            return augmentedMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Минор элемента (row, column)
        public double GetMinor( int row, int column ) {
            RealMatrix augmentMatrix = this.GetAugmentedMatrix( row, column );
            double minor = augmentMatrix.GetDeterminant();
            return minor;
        }
        //------------------------------------------------------------------------------------------
        //Алгебраическое дополнение элемента (row, column)
        public double GetAlgebraicalComplement( int row, int column ) {
            double minor = this.GetMinor( row, column );
            if ( ( row + 1 + column + 1 ) % 2 == 0 ) {
                return minor;
            }
            else {
                return -minor;
            }
        }
        //------------------------------------------------------------------------------------------
        //Размер матрицы 1x1
        public bool IsSize1x1() {
            if ( this.rowCount == 1 && this.columnCount == 1 ) {
                return true;
            }
            else {
                return false;
            }
        }
        //------------------------------------------------------------------------------------------
        //Определитель матрицы
        public double GetDeterminant() {
            if ( this.IsSize1x1() ) {
                return this.dataArray[ 0, 0 ];
            }
            double determinant = 0;
            for ( int column = 0; column < this.columnCount; column++ ) {
                determinant +=
                    this.dataArray[ 0, column ] * this.GetAlgebraicalComplement( 0, column );
            }
            return determinant;
        }
        //------------------------------------------------------------------------------------------
        //Массив данных матрицы
        public double[ , ] GetDataArray() {
            double[ , ] dataArray = new double[ this.rowCount, this.columnCount ];
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    dataArray[ row, column ] = this.dataArray[ row, column ];
                }
            }
            return dataArray;
        }
        //------------------------------------------------------------------------------------------
        //Минимальное значение
        public double GetMinValue() {
            double minValue = this.dataArray[ 0, 0 ];
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    double value = this.dataArray[ row, column ];
                    if ( value < minValue ) {
                        minValue = value;
                    }
                }
            }
            return minValue;
        }
        //------------------------------------------------------------------------------------------
        //Максимальное значение
        public double GetMaxValue() {
            double maxValue = this.dataArray[ 0, 0 ];
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    double value = this.dataArray[ row, column ];
                    if ( maxValue < value ) {
                        maxValue = value;
                    }
                }
            }
            return maxValue;
        }
        //------------------------------------------------------------------------------------------
        //Поэлемнетное деление
        public static RealMatrix ElementwiseDivision(
            RealMatrix dividendMatrix,
            RealMatrix dividerMatrix
        ) {
            bool equalSize = RealMatrix.IsEqualMatricesSize( dividendMatrix, dividerMatrix );
            if ( !equalSize ) {
                throw new MatrixException();
            }
            int rowCount = dividendMatrix.rowCount;
            int columnCount = dividerMatrix.columnCount;
            RealMatrix newMatrix = new RealMatrix( rowCount, columnCount );
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    double dividend = dividendMatrix[ row, column ];
                    double divider = dividerMatrix[ row, column ];
                    double value = dividend / divider;
                    newMatrix[ row, column ] = value;
                }
            }
            return newMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Проверка размеров матриц
        public static bool IsEqualMatricesSize(
            RealMatrix matrixOne,
            RealMatrix matrixTwo
        ) {
            bool equalityRowsCount = ( matrixOne.rowCount == matrixTwo.rowCount );
            bool equalityColumnsCount = ( matrixOne.columnCount == matrixTwo.columnCount );
            bool result = equalityRowsCount && equalityColumnsCount;
            return result;
        }
        //------------------------------------------------------------------------------------------
        //Средняя матрица
        public static RealMatrix GetMeanMatrix( RealMatrix[] matrices ) {
            int countMatrix = matrices.Length;
            int rowCount = matrices[0].rowCount;
            int columnCount = matrices[0].columnCount;
            RealMatrix meanMatrix = new RealMatrix( rowCount, columnCount );
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    double[] values = RealMatrix.GeValuesAtElementFromMatrices( matrices, row, column );
                    double meanValue = Statistician.GetMeanValue( values );
                    meanMatrix[ row, column ] = meanValue;
                }
            }
            return meanMatrix;
        }
        //------------------------------------------------------------------------------------------
        //Массив значений в точке из массива матриц
        public static double[] GeValuesAtElementFromMatrices(
            RealMatrix[] matrieces,
            int row,
            int column
        ) {
            int count = matrieces.Length;
            double[] values = new double[ count ];
            for ( int index = 0; index < count; index++ ) {
                RealMatrix matrix = matrieces[ index ];
                values[ index ] = matrix[ row, column ];
            }
            return values; 
        }
        //------------------------------------------------------------------------------------------
        //Установить строку
        public void SetDataRow( int row, double[] dataRow ) {
            if (dataRow.Length != this.columnCount) {
                throw new MatrixException();
            }
            for ( int column = 0; column < this.columnCount; column++ ) {
                this.dataArray[ row, column ] = dataRow[ column ];
            }
        }
        //------------------------------------------------------------------------------------------
        //Установить стобец
        public void SetDataColumn( int column, double[] dataColumn ) {
            if ( dataColumn.Length != this.rowCount ) {
                throw new MatrixException();
            }
            for ( int row = 0; row < this.rowCount; row++ ) {
                this.dataArray[ row, column ] = dataColumn[ row ];
            }
        }
        //------------------------------------------------------------------------------------------
        //Сохранить в текстовый файл
        public void WriteTextFile( string nameFile ) {
            FileStream fileStream = new FileStream(nameFile, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter( fileStream );
            for ( int row = 0; row < this.rowCount - 1; row++ ) {
                this.WriteRow( row, streamWriter );
                streamWriter.WriteLine();
            }
            int lastRow = this.rowCount - 1;
            this.WriteRow( lastRow, streamWriter );
            streamWriter.Close();
            fileStream.Close();
        }
        //------------------------------------------------------------------------------------------
        //Запись строки в поток
        private void WriteRow( int row, StreamWriter streamWriter ) {
            for ( int column = 0; column < this.columnCount; column++ ) {
                double value = this.dataArray[ row, column ];
                streamWriter.Write( "\t{0}", value );
            }
        }
        //------------------------------------------------------------------------------------------
        //Сериализация
        public void Serialize( string nameFile ) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream fileStream = new FileStream
                ( nameFile, FileMode.Create, FileAccess.Write, FileShare.None );
            binaryFormatter.Serialize( fileStream, this );
            fileStream.Close();
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //Десериализация
        public static RealMatrix Deserialize( string nameFile ) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream fileStream = new FileStream
                ( nameFile, FileMode.Open, FileAccess.Read, FileShare.None );
            RealMatrix matrix = (RealMatrix)binaryFormatter.Deserialize( fileStream );
            fileStream.Close();
            return matrix;
        }
        //------------------------------------------------------------------------------------------
        //Сумма элементов
        public double GetSum() {
            double sum = 0;
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    sum += this.dataArray[ row, column ];
                }
            }
            return sum;
        }
        //------------------------------------------------------------------------------------------
        //Подматрица
        public RealMatrix GetSubMatrix(
            int rowTopLeft,
            int columnTopLeft,
            int rowBottomRight,
            int columnBottomRight
        ) {
            int rowCountSubMatrix = rowBottomRight - rowTopLeft + 1;
            int columnCountSubMatrix = columnBottomRight - columnTopLeft + 1;
            RealMatrix subMatrix = new RealMatrix( rowCountSubMatrix, columnCountSubMatrix );
            for (
                int rowThisMatrix = rowTopLeft, rowSubMatrix = 0;
                rowThisMatrix <= rowBottomRight;
                rowThisMatrix++, rowSubMatrix++
            ) {
                for (
                    int columnThisMatrix = columnTopLeft, columnSubMatrix = 0;
                    columnThisMatrix <= columnBottomRight;
                    columnThisMatrix++, columnSubMatrix++
                ) {
                    subMatrix[ rowSubMatrix, columnSubMatrix ] =
                        this.dataArray[ rowThisMatrix, columnThisMatrix ];
                }
            }
            return subMatrix;
        }
        //------------------------------------------------------------------------------------------
    }
}
