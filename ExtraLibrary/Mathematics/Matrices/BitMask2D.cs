using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ExtraLibrary.Mathematics.Matrices {
    //Битовая матрица
    public class BitMask2D {
        private int rowCount;           //Количество строк
        private int columnCount;        //Количество столбцов

        private bool[ , ] dataArray;    //Массив данных   

        //-----------------------------------------------------------------------------------------
        public BitMask2D( int rowCount, int columnCount, bool value ) {
            this.rowCount = rowCount;
            this.columnCount = columnCount;

            this.dataArray = new bool[ this.rowCount, this.columnCount ];
            
            for ( int row = 0; row < this.rowCount; row++ ) {
                for ( int column = 0; column < this.columnCount; column++ ) {
                    this.dataArray[ row, column ] = value;
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        public BitMask2D( bool[ , ] array ) {
            this.rowCount = array.GetLength( 0 );
            this.columnCount = array.GetLength( 1 );

            this.dataArray = new bool[ this.rowCount, this.columnCount ];
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    this.dataArray[ row, column ] = array[ row, column ];
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //Создание битововой маски из матрицы (пороговое значение)
        public BitMask2D( RealMatrix matrix, double thresholdValue ) {
            this.rowCount = matrix.RowCount;
            this.columnCount = matrix.ColumnCount;

            this.dataArray = new bool[ this.rowCount, this.columnCount ];
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    double value = matrix[ row, column ];
                    if ( value < thresholdValue ) {
                        this.dataArray[ row, column ] = false;
                    }
                    else {
                        this.dataArray[ row, column ] = true;
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------
        //Индексатор
        public bool this[ int row, int column ] {
            get {
                return this.dataArray[ row, column ];
            }
            set {
                this.dataArray[ row, column ] = value;
            }
        }
        //-----------------------------------------------------------------------------------------
        public int RowCount {
            get {
                return this.rowCount;
            }
        }
        //-----------------------------------------------------------------------------------------
        public int ColumnCount {
            get {
                return this.columnCount;
            }
        }
        //-----------------------------------------------------------------------------------------
        public Point[] GetTruePoints() {
            List<Point> pointsList = new List<Point>();
            for ( int x = 0; x < this.ColumnCount; x++ ) {
                for ( int y = 0; y < this.RowCount; y++ ) {
                    if ( this[ y, x ] == true ) {
                        Point point = new Point( x, y );
                        pointsList.Add( point );
                    }
                }
            }
            Point[] points = pointsList.ToArray();
            return points;
        }
        //-----------------------------------------------------------------------------------------
        public Point[] GetFalsePoints() {
            List<Point> pointsList = new List<Point>();
            for ( int x = 0; x < this.ColumnCount; x++ ) {
                for ( int y = 0; y < this.RowCount; y++ ) {
                    if ( this[ y, x ] == false ) {
                        Point point = new Point( x, y );
                        pointsList.Add( point );
                    }
                }
            }
            Point[] points = pointsList.ToArray();
            return points;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
