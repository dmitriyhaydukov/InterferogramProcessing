using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Numbers;

namespace ExtraLibrary.Geometry2D {
    //Кривая второго порядка
    //a11*x^2 + a22*y^2 + 2*a12*x*y + 2*a13*x + 2*a23*y + a33 = 0
    public class QuadricCurveDescriptor {
        double a11;
        double a22;
        double a12;
        double a13;
        double a23;
        double a33;
        //------------------------------------------------------------------------------------------------
        public QuadricCurveDescriptor(
            double a11,
            double a22,
            double a12,
            double a13,
            double a23,
            double a33
        ) {
            this.a11 = a11;
            this.a22 = a22;
            this.a12 = a12;
            this.a13 = a13;
            this.a23 = a23;
            this.a33 = a33;
        }
        //------------------------------------------------------------------------------------------------
        //Инварианты относительно переноса и поворота осей
        public double GetInvariantI() {
            double invariantI = this.a11 + this.a22;
            return invariantI;
        }
        //------------------------------------------------------------------------------------------------
        public double GetInvariantD() {
            double[ , ] arrayMatrixD = new double[ , ] {
                { this.a11, this.a12 },
                { this.a12, this.a22 }
            };
            RealMatrix matrixD = new RealMatrix( arrayMatrixD );
            double invariantD = matrixD.GetDeterminant();
            return invariantD;
        }
        //------------------------------------------------------------------------------------------------
        public double GetInvariantA() {
            double[ , ] arrayMatrixA = new double[ , ] {
                { this.a11, this.a12, this.a13 },
                { this.a12, this.a22, this.a23 },
                { this.a13, this.a23, this.a33 }
            };
            RealMatrix matrixA = new RealMatrix( arrayMatrixA );
            double invariantA = matrixA.GetDeterminant();
            return invariantA;
        }
        //------------------------------------------------------------------------------------------------
        public double[] GetValuesY( double valueX ) {
            double[] valuesY = new double[ 2 ];

            double y1 = this.GetValueY1( valueX );
            double y2 = this.GetValueY2( valueX );

            valuesY[ 0 ] = y1;
            valuesY[ 1 ] = y2;

            return valuesY;
        }
        //------------------------------------------------------------------------------------------------
        public double[] GetValuesX( double valueY ) {
            double[] valuesX = new double[ 2 ];

            double x1 = this.GetValueY1( valueY );
            double x2 = this.GetValueY2( valueY );

            valuesX[ 0 ] = x1;
            valuesX[ 1 ] = x2;

            return valuesX;
        }
        //------------------------------------------------------------------------------------------------
        private double GetValueY1( double x ) {

            double radicand =
                a12 * a12 * x * x + 2 * a12 * a23 * x + a23 * a23 -
                a11 * a22 * x * x - 2 * a13 * a22 * x - a22 * a33;
            double y = -( a23 - Math.Sqrt( radicand ) + a12 * x ) / a22;
            return y;
        }
        //------------------------------------------------------------------------------------------------
        private double GetValueY2( double x ) {
            
            double radicand =
                a12 * a12 * x * x + 2 * a12 * a23 * x + a23 * a23 -
                a11 * a22 * x * x - 2 * a13 * a22 * x - a22 * a33;
            double y = -( a23 + Math.Sqrt( radicand ) + a12 * x ) / a22;
            return y;
        }
        //------------------------------------------------------------------------------------------------
        private double GetValueX1( double y ) {
            double radicand =
                a12 * a12 * y * y + 2 * a12 * a13 * y + a13 * a13 -
                a11 * a22 * y * y - 2 * a11 * a23 * y - a11 * a33;
            double x = -( a13 - Math.Sqrt( radicand ) + a12 * y ) / a11;
            return x;
        }
        //------------------------------------------------------------------------------------------------
        private double GetValueX2( double y ) {
            double radicand =
                a12 * a12 * y * y + 2 * a12 * a13 * y + a13 * a13 -
                a11 * a22 * y * y - 2 * a11 * a23 * y - a11 * a33;
            double x = -( a13 + Math.Sqrt( radicand ) + a12 * y ) / a11;
            return x;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //Вычисление невязки
        public double[] CalculateMisalignment(double[] arrayX, double[] arrayY) {
            int size = arrayX.Length;
            double[] misalignments = new double[ size ];
            for ( int index = 0; index < size; index++ ) {
                double x = arrayX[index];
                double y = arrayY[index];
                double misalignment =
                    this.a11 * x * x +
                    this.a22 * y * y +
                    2 * this.a12 * x * y +
                    2 * this.a13 * x +
                    2 * this.a23 * y +
                    this.a33;
                misalignments[ index ] = misalignment;
            }
            return misalignments;
        }
        //------------------------------------------------------------------------------------------------
        //Центр кривой
        public Point2D GetCentre() {
            double[ , ] arrayMatrixX = new double[ , ] {
                { this.a13, this.a12 },
                { this.a23, this.a22}
            };
            RealMatrix matrixX = new RealMatrix( arrayMatrixX );

            double[ , ] arrayMatrixY = new double[ , ] {
                { this.a11, this.a13 },
                { this.a12, this.a23 }
            };
            RealMatrix matrixY = new RealMatrix( arrayMatrixY );

            double coordinateX = - matrixX.GetDeterminant() / this.GetInvariantD();
            double coordinateY = - matrixY.GetDeterminant() / this.GetInvariantD();

            Point2D centre = new Point2D( coordinateX, coordinateY );
            return centre;
        }
        //------------------------------------------------------------------------------------------------
        //Корни характеристического уравнения кривой
        public double[] GetCharacteristicEquationRoots() {
            double b = -( this.a11 + this.a22 );
            double d =
                this.a11 * this.a11 -
                2 * this.a11 * this.a22 +
                this.a22 * this.a22 +
                4 * this.a12 * this.a12;
            double rootOne = ( -b + Math.Sqrt( d ) ) / 2;
            double rootTwo = ( -b - Math.Sqrt( d ) ) / 2;
            double[] roots = new double[] { rootOne, rootTwo };
            return roots;
        }
        //------------------------------------------------------------------------------------------------
        //Нормаль к кривой в точке
        public LineDescriptor GetNormalAtPoint( Point2D point ) {
            double x0 = point.X;
            double y0 = point.Y;

            double a = a12 * x0 + a22 * y0 + a23;
            double b = -a11 * x0 - a12 * y0 - a13;
            double c =
                a11 * x0 * y0 + a12 * y0 * y0 + a13 * y0 - a12 * x0 * x0 - a22 * y0 * x0 - a23 * x0;
            LineDescriptor normalLineDescriptor = new LineDescriptor( a, b, c );
            return normalLineDescriptor;
        }
        //------------------------------------------------------------------------------------------------
        //Угол между осью Ox и каждым из двух главных направлений кривой
        public double GetAngleBetweenOxAndPrincipalAxis() {
            double angle = Math.Atan2( 2 * this.a12, this.a11 - this.a22 ) / 2;
            return angle;
        }
        //------------------------------------------------------------------------------------------------
        //Коэффициенты уравнения кривой
        public double A11 {
            get {
                return this.a11;
            }
        }
        //------------------------------------------------------------------------------------------------
        public double A22 {
            get {
                return this.a22;
            }
        }
        //------------------------------------------------------------------------------------------------
        public double A12 {
            get {
                return this.a12;
            }
        }
        //------------------------------------------------------------------------------------------------
        public double A13 {
            get {
                return this.a13;
            }
        }
        //------------------------------------------------------------------------------------------------
        public double A23 {
            get {
                return this.a23;
            }
        }
        //------------------------------------------------------------------------------------------------
        public double A33 {
            get {
                return this.a33;
            }
        }
        //------------------------------------------------------------------------------------------------
        //Вывод на консоль
        public void WriteConsole() {
            Console.WriteLine( this.GetType() );
            Console.WriteLine( this.a11 );
            Console.WriteLine( this.a22 );
            Console.WriteLine( this.a12 );
            Console.WriteLine( this.a13 );
            Console.WriteLine( this.a23 );
            Console.WriteLine( this.a33 );
        }
        //------------------------------------------------------------------------------------------------
        public Point2D[] GetPoints( double startX, double finishX, double step ) {
            List<Point2D> points = new List<Point2D>();
            for ( double x = startX; x <= finishX; x += step ) {
                double y = this.GetValueY1( x );
                points.Add( new Point2D( x, y ) );
            }

            for ( double x = startX; x <= finishX; x += step ) {
                double y = this.GetValueY2( x );
                points.Add( new Point2D( x, y ) );
            }

            return points.ToArray();
        }
        //------------------------------------------------------------------------------------------------
    }
}
