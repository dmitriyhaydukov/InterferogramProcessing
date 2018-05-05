using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

namespace ExtraLibrary.Mathematics.Approximation {
    public class PlaneApproximator {
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        public PlaneDescriptor Approximate( params Point3D[] points ) {
            int n = 3;
            RealMatrix matrixA = new RealMatrix( points.Length, n );
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];

                matrixA[ index, 0 ] = 1;
                matrixA[ index, 1 ] = point.X;
                matrixA[ index, 2 ] = point.Y;
            }
            
            RealVector vectorZ = new RealVector( SpaceManager.GetCoordinatesZ( points ) );

            RealMatrix transposedMatrixA = matrixA.GetTransposedMatrix();
            RealVector coefficientsVector =
                ( transposedMatrixA * matrixA ).GetInverseMatrix() * transposedMatrixA * vectorZ;

            double d = coefficientsVector[ 0 ];
            double a = coefficientsVector[ 1 ];
            double b = coefficientsVector[ 2 ];

            double coefficientOfX = a / d;
            double coefficientOfY = b / d;
            double coefficientOfZ = -1 / d;
            double absoluteTerm = 1;

            PlaneDescriptor planeDescriptor =
                new PlaneDescriptor( coefficientOfX, coefficientOfY, coefficientOfZ, absoluteTerm );
            return planeDescriptor;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //Аппроксимация точек плоскостью( метод наименьших квадратов )
        /*
        public PlaneDescriptor Approximate( params Point3D[] points ) {
            int n = 3;
            RealMatrix matrixA = new RealMatrix( points.Length, n );
            for ( int index = 0; index < points.Length; index++ ) {
                Point3D point = points[ index ];
                
                matrixA[ index, 0 ] = point.X;
                matrixA[ index, 1 ] = point.Y;
                matrixA[ index, 2 ] = point.Z;
                
            }

            RealVector unitVector = RealVector.GetUnitVector( n );
            
            RealMatrix transposedMatrixA = matrixA.GetTransposedMatrix();
            RealMatrix productMatrix = transposedMatrixA * matrixA;
            RealMatrix inverseMatrix = productMatrix.GetInverseMatrix();
            
            RealVector coefficientsVector = inverseMatrix * unitVector;
                       
            double coefficientOfX = coefficientsVector[ 0 ];
            double coefficientOfY = coefficientsVector[ 1 ];
            double coefficientOfZ = coefficientsVector[ 2 ];
            double absoluteTerm = 1; 
            
            PlaneDescriptor planeDescriptor = 
                new PlaneDescriptor( coefficientOfX, coefficientOfY, coefficientOfZ, absoluteTerm );

            return planeDescriptor;
        }
        */
        //----------------------------------------------------------------------------------------------
    }
}
