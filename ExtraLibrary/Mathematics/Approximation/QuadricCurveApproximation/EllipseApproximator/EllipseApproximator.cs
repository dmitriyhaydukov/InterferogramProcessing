using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Mathematics.Equations;
using ExtraLibrary.Mathematics.EquationsSets;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayOperation;


namespace ExtraLibrary.Mathematics.Approximation {
    //Аппроксимация эллипсом
    public partial class EllipseApproximator {
        //---------------------------------------------------------------------------------------
        //Аппроксимировать
        public EllipseDescriptor Approximate( Point2D[] points ) {

            RealMatrix matrixD = this.CreateMatrixD( points );
            RealMatrix transposedMatrixD = matrixD.GetTransposedMatrix();
            RealMatrix matrixS = transposedMatrixD * matrixD;

            double[] eigenValues = this.CalculateEigenValues( matrixS );
            double[] absoluteEigenValues = ArrayOperator.GetAbsoluteValues( eigenValues );

            int minAbsoluteEigenValueIndex = ArrayOperator.GetMinValueIndex( absoluteEigenValues );

            double lambda = eigenValues[ minAbsoluteEigenValueIndex ];
            RealMatrix matrixA = this.CreateMatrixA( matrixS, lambda );

            double[ , ] dataArrayMatrixA = matrixA.GetDataArray();
            double[] dataArrayVectorB = new double[] { 1, 0, 0, 0, 0, 0 };
            double[] values =
                this.SolveLinearSystem( dataArrayMatrixA, dataArrayVectorB );

            EllipseDescriptor ellipse = this.CreateEllipseDescriptor( values );
            return ellipse;
        }
        //---------------------------------------------------------------------------------------
        //Создание матрицы D
        private RealMatrix CreateMatrixD( Point2D[] points ) {

            int pointCount = points.Length;

            RealMatrix matrixD = new RealMatrix( pointCount, 6 );

            for ( int index = 0; index < pointCount; index++ ) {
                double x = points[ index ].X;
                double y = points[ index ].Y;
                double xy = x * y;
                double xSquare = x * x;
                double ySquare = y * y;

                matrixD[ index, 0 ] = xSquare;
                matrixD[ index, 1 ] = ySquare;
                matrixD[ index, 2 ] = xy;
                matrixD[ index, 3 ] = x;
                matrixD[ index, 4 ] = y;
                matrixD[ index, 5 ] = 1;
            }
            return matrixD;
        }
        //---------------------------------------------------------------------------------------
        //Создание матрицы C
        private RealMatrix CreateMatrixC() {
            RealMatrix matrixC = new RealMatrix( 6, 6 );
            matrixC[ 1, 1 ] = -1;
            matrixC[ 2, 0 ] = 2;
            matrixC[ 0, 2 ] = 2;
            return matrixC;
        }
        //---------------------------------------------------------------------------------------
        //Создание матрицы A
        private RealMatrix CreateMatrixA(
            RealMatrix productTransposedMatrixDbyMatrixD,
            double lambda
        ) {
            RealMatrix matrixA = new RealMatrix( productTransposedMatrixDbyMatrixD );
            matrixA[ 0, 0 ] = 1;
            matrixA[ 0, 2 ] = productTransposedMatrixDbyMatrixD[ 0, 2 ] - 2 * lambda;
            matrixA[ 2, 0 ] = productTransposedMatrixDbyMatrixD[ 2, 0 ] - 2 * lambda;
            matrixA[ 1, 1 ] = productTransposedMatrixDbyMatrixD[ 1, 1 ] + lambda;

            return matrixA;
        }
        //---------------------------------------------------------------------------------------
        //Вычисление собственных значений
        private double[] CalculateEigenValues( RealMatrix matrix ) {
            double[] koefficients =  this.ComputeKoefficients( matrix );

            double koefficientCubeX = koefficients[ 0 ];
            double koefficientSquareX = koefficients[ 1 ];
            double koefficientX = koefficients[ 2 ];
            double absoluteTerm = koefficients[ 3 ];

            VietoKardanoSolver cubicEquationSolver = new VietoKardanoSolver(
                koefficientSquareX / koefficientCubeX,
                koefficientX / koefficientCubeX,
                absoluteTerm / koefficientCubeX
            );

            //Собственные значения - корни кубического уравнения
            Complex[] complexEigenValues = cubicEquationSolver.Solve();
            double[] eigenValues = new double[ complexEigenValues.Length ];

            for ( int index = 0; index < complexEigenValues.Length; index++ ) {
                Complex complex = complexEigenValues[ index ];
                if ( complex.Imaginary == 0 ) {
                    eigenValues[ index ] = complex.Real;
                }
                else {
                    throw new ComplexRootException();
                }
            }
            return eigenValues;
        }
        //---------------------------------------------------------------------------------------
        //Решение системы линейных уравнений
        private double[] SolveLinearSystem( double[ , ] arrayA, double[] arrayB ) {
            LinearSystem linearSystem = new LinearSystem( arrayA, arrayB, 1E-17 );
            double[] solutionX = linearSystem.XVector;
            return solutionX;
        }
        //---------------------------------------------------------------------------------------
        //Создание эллипса
        private EllipseDescriptor CreateEllipseDescriptor( double[] array ) {
            double a11 = array[ 0 ];
            double a22 = array[ 1 ];
            double a12 = array[ 2 ] / 2;
            double a13 = array[ 3 ] / 2;
            double a23 = array[ 4 ] / 2;
            double a33 = array[ 5 ];

            EllipseDescriptor ellipse = new EllipseDescriptor( a11, a22, a12, a13, a23, a33 );
            return ellipse;
        }
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
    }
}
