using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

namespace ExtraLibrary.Mathematics.Vectors {
    public class RealVector {
        double[] dataArray;
        //------------------------------------------------------------------------------------
        public RealVector( int size) {
            this.dataArray = new double[ size ];
        }
        //------------------------------------------------------------------------------------
        public RealVector( params double[] values ) {
            this.dataArray = new double[ values.Length ];
            for ( int index = 0; index < values.Length; index++ ) {
                this.dataArray[ index ] = values[ index ];
            }
        }
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public RealVector( Point3D point3D ) : this( point3D.X, point3D.Y, point3D.Z ) {
        }
        //------------------------------------------------------------------------------------
        public RealVector( RealVector vector ) : this( vector.dataArray ) {
        }
        //------------------------------------------------------------------------------------
        //Размер
        public int Size {
            get {
                return this.dataArray.Length;
            }
        }
        //-------------------------------------------------------------------------------------
        //Индексатор
        public double this[ int index ] {
            get {
                return this.dataArray[ index ];
            }
            set {
                this.dataArray[ index ] = value;
            }
        }
        //-------------------------------------------------------------------------------------
        //Массив значений вектора
        public double[] GetDataArray() {
            double[] array = new double[ this.Size ];
            this.dataArray.CopyTo( array, 0 );
            return array;
        }
        //-------------------------------------------------------------------------------------
        //Сложение векторов
        public static RealVector operator +( RealVector operandOne, RealVector operandTwo ) {
            RealVector newVector = new RealVector( operandOne.Size );
            for ( int index = 0; index < newVector.Size; index++ ) {
                newVector[ index ] = operandOne[ index ] + operandTwo[ index ];
            }
            return newVector;
        }
        //-------------------------------------------------------------------------------------
        //Скалярное произведение
        public static double operator *( RealVector operandOne, RealVector operandTwo ) {
            double scalarProduct = ArrayOperator.ScalarProduct
                ( operandOne.dataArray, operandTwo.dataArray );
            return scalarProduct;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        //Умножение на число
        public static RealVector operator *( double value, RealVector vector ) {
            RealVector newVector = new RealVector( vector.Size );
            for ( int index = 0; index < vector.dataArray.Length; index++ ) {
                newVector[ index ] = vector[ index ] * value;
            }
            return newVector;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        //Матрица преобразования для вычисления ортогонального вектора
        private RealMatrix CreateOrthogonalVectorTransformingMatrix( int size ) {
            RealMatrix transformingMatrix = new RealMatrix( size, size );

            for ( int column = 1, row = 0; column < size; column++, row++ ) {
                transformingMatrix[ row, column ] = 1;
            }

            for ( int row = 1, column = 0; row < size; row++, column++ ) {
                transformingMatrix[ row, column ] = -1;
            }

            transformingMatrix[ size - 1, 0 ] = 1;
            transformingMatrix[ 0, size - 1 ] = -1;

            return transformingMatrix;
        }
        //-------------------------------------------------------------------------------------------------
        //Ортогональный вектор
        public RealVector GetOrthogonalVector() {
            RealMatrix transformatedMatrix = this.CreateOrthogonalVectorTransformingMatrix( this.Size );
            RealVector orthogonalVector = transformatedMatrix * this;
            return orthogonalVector;
        }
        //-------------------------------------------------------------------------------------------------
        //Вывод на консоль
        public void WriteConsole( string name ) {
            Console.WriteLine( name );
            for ( int index = 0; index < dataArray.Length; index++ ) {
                Console.WriteLine(dataArray[index]);
            }
        }
        //------------------------------------------------------------------------------------
        public Point3D ToPoint3D() {
            if ( this.Size != 3 ) {
                throw new Exception();
            }
            Point3D point3D = new Point3D( this );
            return point3D;
        }
        //------------------------------------------------------------------------------------
        //Длина вектора
        public double Length {
            get {
                double squareSum = ArrayOperator.GetSquareSum( this.dataArray );
                double length = Math.Sqrt( squareSum );
                return length;
            }
        }
        //------------------------------------------------------------------------------------
        public RealVector GetNormalizedVector() {
            double length = this.Length;

            RealVector newVector = new RealVector( this.Size );
            for ( int index = 0; index < this.Size; index++ ) {
                newVector[ index ] = this[ index ] / length;
            }
            return newVector;
        }
        //------------------------------------------------------------------------------------
        //Единичный вектор
        public static RealVector GetUnitVector( int size ) {
            RealVector unitVector = new RealVector( size );
            for ( int index = 0; index < unitVector.Size; index++ ) {
                unitVector[ index ] = 1;
            }
            return unitVector;
        }
        //------------------------------------------------------------------------------------
        //Строковое представление вектора
        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append( "( " );
            for ( int index = 0; index < this.dataArray.Length - 1; index++ ) {
                stringBuilder.Append( this.dataArray[ index ] );
                stringBuilder.Append( ';' );
            }
            int lastIndex = this.dataArray.Length - 1;
            stringBuilder.Append( this.dataArray[ lastIndex ] );
            stringBuilder.Append( " )" );

            return stringBuilder.ToString();
        }
        //------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
    }
}
