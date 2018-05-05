using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Numbers;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Mathematics.Approximation {
    public partial class EllipseApproximator {
        private double ComputeCoefficientSquareX( RealMatrix matrix ) {
            BigRationalNumber d11 = new BigRationalNumber( matrix[ 0, 0 ] );
            BigRationalNumber d12 = new BigRationalNumber( matrix[ 0, 1 ] );
            BigRationalNumber d13 = new BigRationalNumber( matrix[ 0, 2 ] );
            BigRationalNumber d14 = new BigRationalNumber( matrix[ 0, 3 ] );
            BigRationalNumber d15 = new BigRationalNumber( matrix[ 0, 4 ] );
            BigRationalNumber d16 = new BigRationalNumber( matrix[ 0, 5 ] );

            BigRationalNumber d21 = new BigRationalNumber( matrix[ 1, 0 ] );
            BigRationalNumber d22 = new BigRationalNumber( matrix[ 1, 1 ] );
            BigRationalNumber d23 = new BigRationalNumber( matrix[ 1, 2 ] );
            BigRationalNumber d24 = new BigRationalNumber( matrix[ 1, 3 ] );
            BigRationalNumber d25 = new BigRationalNumber( matrix[ 1, 4 ] );
            BigRationalNumber d26 = new BigRationalNumber( matrix[ 1, 5 ] );

            BigRationalNumber d31 = new BigRationalNumber( matrix[ 2, 0 ] );
            BigRationalNumber d32 = new BigRationalNumber( matrix[ 2, 1 ] );
            BigRationalNumber d33 = new BigRationalNumber( matrix[ 2, 2 ] );
            BigRationalNumber d34 = new BigRationalNumber( matrix[ 2, 3 ] );
            BigRationalNumber d35 = new BigRationalNumber( matrix[ 2, 4 ] );
            BigRationalNumber d36 = new BigRationalNumber( matrix[ 2, 5 ] );

            BigRationalNumber d41 = new BigRationalNumber( matrix[ 3, 0 ] );
            BigRationalNumber d42 = new BigRationalNumber( matrix[ 3, 1 ] );
            BigRationalNumber d43 = new BigRationalNumber( matrix[ 3, 2 ] );
            BigRationalNumber d44 = new BigRationalNumber( matrix[ 3, 3 ] );
            BigRationalNumber d45 = new BigRationalNumber( matrix[ 3, 4 ] );
            BigRationalNumber d46 = new BigRationalNumber( matrix[ 3, 5 ] );

            BigRationalNumber d51 = new BigRationalNumber( matrix[ 4, 0 ] );
            BigRationalNumber d52 = new BigRationalNumber( matrix[ 4, 1 ] );
            BigRationalNumber d53 = new BigRationalNumber( matrix[ 4, 2 ] );
            BigRationalNumber d54 = new BigRationalNumber( matrix[ 4, 3 ] );
            BigRationalNumber d55 = new BigRationalNumber( matrix[ 4, 4 ] );
            BigRationalNumber d56 = new BigRationalNumber( matrix[ 4, 5 ] );

            BigRationalNumber d61 = new BigRationalNumber( matrix[ 5, 0 ] );
            BigRationalNumber d62 = new BigRationalNumber( matrix[ 5, 1 ] );
            BigRationalNumber d63 = new BigRationalNumber( matrix[ 5, 2 ] );
            BigRationalNumber d64 = new BigRationalNumber( matrix[ 5, 3 ] );
            BigRationalNumber d65 = new BigRationalNumber( matrix[ 5, 4 ] );
            BigRationalNumber d66 = new BigRationalNumber( matrix[ 5, 5 ] );

            BigRationalNumber bigRationalkoefficientSquareX =
                2 * d13 * d44 * d55 * d66 -
                2 * d13 * d44 * d56 * d65 -
                2 * d13 * d45 * d54 * d66 +
                2 * d13 * d45 * d56 * d64 +
                2 * d13 * d46 * d54 * d65 -
                2 * d13 * d46 * d55 * d64 -
                2 * d14 * d43 * d55 * d66 +
                2 * d14 * d43 * d56 * d65 +
                2 * d14 * d45 * d53 * d66 -
                2 * d14 * d45 * d56 * d63 - 
                2 * d14 * d46 * d53 * d65 +
                2 * d14 * d46 * d55 * d63 +
                2 * d15 * d43 * d54 * d66 -
                2 * d15 * d43 * d56 * d64 -
                2 * d15 * d44 * d53 * d66 +
                2 * d15 * d44 * d56 * d63 +
                2 * d15 * d46 * d53 * d64 -
                2 * d15 * d46 * d54 * d63 -
                2 * d16 * d43 * d54 * d65 +
                2 * d16 * d43 * d55 * d64 +
                2 * d16 * d44 * d53 * d65 -
                2 * d16 * d44 * d55 * d63 -
                2 * d16 * d45 * d53 * d64 +
                2 * d16 * d45 * d54 * d63 -
                4 * d22 * d44 * d55 * d66 +
                4 * d22 * d44 * d56 * d65 +
                4 * d22 * d45 * d54 * d66 -
                4 * d22 * d45 * d56 * d64 -
                4 * d22 * d46 * d54 * d65 +
                4 * d22 * d46 * d55 * d64 +
                4 * d24 * d42 * d55 * d66 -
                4 * d24 * d42 * d56 * d65 -
                4 * d24 * d45 * d52 * d66 +
                4 * d24 * d45 * d56 * d62 +
                4 * d24 * d46 * d52 * d65 -
                4 * d24 * d46 * d55 * d62 -
                4 * d25 * d42 * d54 * d66 +
                4 * d25 * d42 * d56 * d64 +
                4 * d25 * d44 * d52 * d66 -
                4 * d25 * d44 * d56 * d62 -
                4 * d25 * d46 * d52 * d64 +
                4 * d25 * d46 * d54 * d62 +
                4 * d26 * d42 * d54 * d65 -
                4 * d26 * d42 * d55 * d64 - 
                4 * d26 * d44 * d52 * d65 +
                4 * d26 * d44 * d55 * d62 +
                4 * d26 * d45 * d52 * d64 -
                4 * d26 * d45 * d54 * d62 +
                2 * d31 * d44 * d55 * d66 -
                2 * d31 * d44 * d56 * d65 -
                2 * d31 * d45 * d54 * d66 +
                2 * d31 * d45 * d56 * d64 +
                2 * d31 * d46 * d54 * d65 -
                2 * d31 * d46 * d55 * d64 -
                2 * d34 * d41 * d55 * d66 +
                2 * d34 * d41 * d56 * d65 +
                2 * d34 * d45 * d51 * d66 -
                2 * d34 * d45 * d56 * d61 -
                2 * d34 * d46 * d51 * d65 +
                2 * d34 * d46 * d55 * d61 +
                2 * d35 * d41 * d54 * d66 -
                2 * d35 * d41 * d56 * d64 -
                2 * d35 * d44 * d51 * d66 +
                2 * d35 * d44 * d56 * d61 +
                2 * d35 * d46 * d51 * d64 -
                2 * d35 * d46 * d54 * d61 -
                2 * d36 * d41 * d54 * d65 +
                2 * d36 * d41 * d55 * d64 +
                2 * d36 * d44 * d51 * d65 -
                2 * d36 * d44 * d55 * d61 - 
                2 * d36 * d45 * d51 * d64 +
                2 * d36 * d45 * d54 * d61;

            double koefficientSquareX = bigRationalkoefficientSquareX.ToDouble();
            return koefficientSquareX;
        }
    }
}