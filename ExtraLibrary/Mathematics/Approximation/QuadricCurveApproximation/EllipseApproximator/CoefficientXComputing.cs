using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Numbers;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Mathematics.Approximation {
    public partial class EllipseApproximator {
        private double ComputeCoefficientX( RealMatrix matrix ) {
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

            BigRationalNumber bigRationalkoefficientX =
                -2 * d12 * d23 * d44 * d55 * d66 +
                2 * d12 * d23 * d44 * d56 * d65 +
                2 * d12 * d23 * d45 * d54 * d66 -
                2 * d12 * d23 * d45 * d56 * d64 -
                2 * d12 * d23 * d46 * d54 * d65 +
                2 * d12 * d23 * d46 * d55 * d64 +
                2 * d12 * d24 * d43 * d55 * d66 -
                2 * d12 * d24 * d43 * d56 * d65 -
                2 * d12 * d24 * d45 * d53 * d66 +
                2 * d12 * d24 * d45 * d56 * d63 +
                2 * d12 * d24 * d46 * d53 * d65 -
                2 * d12 * d24 * d46 * d55 * d63 -
                2 * d12 * d25 * d43 * d54 * d66 +
                2 * d12 * d25 * d43 * d56 * d64 +
                2 * d12 * d25 * d44 * d53 * d66 -
                2 * d12 * d25 * d44 * d56 * d63 -
                2 * d12 * d25 * d46 * d53 * d64 +
                2 * d12 * d25 * d46 * d54 * d63 +
                2 * d12 * d26 * d43 * d54 * d65 -
                2 * d12 * d26 * d43 * d55 * d64 -
                2 * d12 * d26 * d44 * d53 * d65 +
                2 * d12 * d26 * d44 * d55 * d63 +
                2 * d12 * d26 * d45 * d53 * d64 -
                2 * d12 * d26 * d45 * d54 * d63 +
                2 * d13 * d22 * d44 * d55 * d66 -
                2 * d13 * d22 * d44 * d56 * d65 -
                2 * d13 * d22 * d45 * d54 * d66 +
                2 * d13 * d22 * d45 * d56 * d64 +
                2 * d13 * d22 * d46 * d54 * d65 -
                2 * d13 * d22 * d46 * d55 * d64 -
                2 * d13 * d24 * d42 * d55 * d66 +
                2 * d13 * d24 * d42 * d56 * d65 +
                2 * d13 * d24 * d45 * d52 * d66 -
                2 * d13 * d24 * d45 * d56 * d62 -
                2 * d13 * d24 * d46 * d52 * d65 +
                2 * d13 * d24 * d46 * d55 * d62 +
                2 * d13 * d25 * d42 * d54 * d66 -
                2 * d13 * d25 * d42 * d56 * d64 -
                2 * d13 * d25 * d44 * d52 * d66 +
                2 * d13 * d25 * d44 * d56 * d62 +
                2 * d13 * d25 * d46 * d52 * d64 -
                2 * d13 * d25 * d46 * d54 * d62 -
                2 * d13 * d26 * d42 * d54 * d65 +
                2 * d13 * d26 * d42 * d55 * d64 +
                2 * d13 * d26 * d44 * d52 * d65 -
                2 * d13 * d26 * d44 * d55 * d62 -
                2 * d13 * d26 * d45 * d52 * d64 +
                2 * d13 * d26 * d45 * d54 * d62 -
                2 * d14 * d22 * d43 * d55 * d66 +
                2 * d14 * d22 * d43 * d56 * d65 +
                2 * d14 * d22 * d45 * d53 * d66 -
                2 * d14 * d22 * d45 * d56 * d63 -
                2 * d14 * d22 * d46 * d53 * d65 +
                2 * d14 * d22 * d46 * d55 * d63 +
                2 * d14 * d23 * d42 * d55 * d66 -
                2 * d14 * d23 * d42 * d56 * d65 -
                2 * d14 * d23 * d45 * d52 * d66 +
                2 * d14 * d23 * d45 * d56 * d62 +
                2 * d14 * d23 * d46 * d52 * d65 -
                2 * d14 * d23 * d46 * d55 * d62 -
                2 * d14 * d25 * d42 * d53 * d66 +
                2 * d14 * d25 * d42 * d56 * d63 +
                2 * d14 * d25 * d43 * d52 * d66 -
                2 * d14 * d25 * d43 * d56 * d62 -
                2 * d14 * d25 * d46 * d52 * d63 +
                2 * d14 * d25 * d46 * d53 * d62 +
                2 * d14 * d26 * d42 * d53 * d65 -
                2 * d14 * d26 * d42 * d55 * d63 -
                2 * d14 * d26 * d43 * d52 * d65 +
                2 * d14 * d26 * d43 * d55 * d62 +
                2 * d14 * d26 * d45 * d52 * d63 -
                2 * d14 * d26 * d45 * d53 * d62 +
                2 * d15 * d22 * d43 * d54 * d66 -
                2 * d15 * d22 * d43 * d56 * d64 -
                2 * d15 * d22 * d44 * d53 * d66 +
                2 * d15 * d22 * d44 * d56 * d63 +
                2 * d15 * d22 * d46 * d53 * d64 -
                2 * d15 * d22 * d46 * d54 * d63 -
                2 * d15 * d23 * d42 * d54 * d66 +
                2 * d15 * d23 * d42 * d56 * d64 +
                2 * d15 * d23 * d44 * d52 * d66 -
                2 * d15 * d23 * d44 * d56 * d62 -
                2 * d15 * d23 * d46 * d52 * d64 +
                2 * d15 * d23 * d46 * d54 * d62 +
                2 * d15 * d24 * d42 * d53 * d66 -
                2 * d15 * d24 * d42 * d56 * d63 -
                2 * d15 * d24 * d43 * d52 * d66 +
                2 * d15 * d24 * d43 * d56 * d62 +
                2 * d15 * d24 * d46 * d52 * d63 -
                2 * d15 * d24 * d46 * d53 * d62 -
                2 * d15 * d26 * d42 * d53 * d64 +
                2 * d15 * d26 * d42 * d54 * d63 +
                2 * d15 * d26 * d43 * d52 * d64 -
                2 * d15 * d26 * d43 * d54 * d62 -
                2 * d15 * d26 * d44 * d52 * d63 +
                2 * d15 * d26 * d44 * d53 * d62 -
                2 * d16 * d22 * d43 * d54 * d65 +
                2 * d16 * d22 * d43 * d55 * d64 +
                2 * d16 * d22 * d44 * d53 * d65 -
                2 * d16 * d22 * d44 * d55 * d63 -
                2 * d16 * d22 * d45 * d53 * d64 + 
                2 * d16 * d22 * d45 * d54 * d63 +
                2 * d16 * d23 * d42 * d54 * d65 -
                2 * d16 * d23 * d42 * d55 * d64 -
                2 * d16 * d23 * d44 * d52 * d65 +
                2 * d16 * d23 * d44 * d55 * d62 +
                2 * d16 * d23 * d45 * d52 * d64 -
                2 * d16 * d23 * d45 * d54 * d62 -
                2 * d16 * d24 * d42 * d53 * d65 +
                2 * d16 * d24 * d42 * d55 * d63 +
                2 * d16 * d24 * d43 * d52 * d65 -
                2 * d16 * d24 * d43 * d55 * d62 -
                2 * d16 * d24 * d45 * d52 * d63 +
                2 * d16 * d24 * d45 * d53 * d62 +
                2 * d16 * d25 * d42 * d53 * d64 -
                2 * d16 * d25 * d42 * d54 * d63 -
                2 * d16 * d25 * d43 * d52 * d64 +
                2 * d16 * d25 * d43 * d54 * d62 +
                2 * d16 * d25 * d44 * d52 * d63 -
                2 * d16 * d25 * d44 * d53 * d62 +
                d11 * d33 * d44 * d55 * d66 -
                d11 * d33 * d44 * d56 * d65 -
                d11 * d33 * d45 * d54 * d66 +
                d11 * d33 * d45 * d56 * d64 +
                d11 * d33 * d46 * d54 * d65 -
                d11 * d33 * d46 * d55 * d64 -
                d11 * d34 * d43 * d55 * d66 +
                d11 * d34 * d43 * d56 * d65 +
                d11 * d34 * d45 * d53 * d66 -
                d11 * d34 * d45 * d56 * d63 -
                d11 * d34 * d46 * d53 * d65 +
                d11 * d34 * d46 * d55 * d63 +
                d11 * d35 * d43 * d54 * d66 -
                d11 * d35 * d43 * d56 * d64 -
                d11 * d35 * d44 * d53 * d66 +
                d11 * d35 * d44 * d56 * d63 +
                d11 * d35 * d46 * d53 * d64 -
                d11 * d35 * d46 * d54 * d63 -
                d11 * d36 * d43 * d54 * d65 +
                d11 * d36 * d43 * d55 * d64 +
                d11 * d36 * d44 * d53 * d65 -
                d11 * d36 * d44 * d55 * d63 -
                d11 * d36 * d45 * d53 * d64 +
                d11 * d36 * d45 * d54 * d63 -
                d13 * d31 * d44 * d55 * d66 +
                d13 * d31 * d44 * d56 * d65 +
                d13 * d31 * d45 * d54 * d66 -
                d13 * d31 * d45 * d56 * d64 -
                d13 * d31 * d46 * d54 * d65 +
                d13 * d31 * d46 * d55 * d64 +
                d13 * d34 * d41 * d55 * d66 -
                d13 * d34 * d41 * d56 * d65 -
                d13 * d34 * d45 * d51 * d66 +
                d13 * d34 * d45 * d56 * d61 +
                d13 * d34 * d46 * d51 * d65 -
                d13 * d34 * d46 * d55 * d61 -
                d13 * d35 * d41 * d54 * d66 +
                d13 * d35 * d41 * d56 * d64 +
                d13 * d35 * d44 * d51 * d66 -
                d13 * d35 * d44 * d56 * d61 -
                d13 * d35 * d46 * d51 * d64 +
                d13 * d35 * d46 * d54 * d61 +
                d13 * d36 * d41 * d54 * d65 -
                d13 * d36 * d41 * d55 * d64 -
                d13 * d36 * d44 * d51 * d65 +
                d13 * d36 * d44 * d55 * d61 +
                d13 * d36 * d45 * d51 * d64 -
                d13 * d36 * d45 * d54 * d61 +
                d14 * d31 * d43 * d55 * d66 -
                d14 * d31 * d43 * d56 * d65 -
                d14 * d31 * d45 * d53 * d66 +
                d14 * d31 * d45 * d56 * d63 +
                d14 * d31 * d46 * d53 * d65 -
                d14 * d31 * d46 * d55 * d63 -
                d14 * d33 * d41 * d55 * d66 +
                d14 * d33 * d41 * d56 * d65 +
                d14 * d33 * d45 * d51 * d66 -
                d14 * d33 * d45 * d56 * d61 -
                d14 * d33 * d46 * d51 * d65 +
                d14 * d33 * d46 * d55 * d61 +
                d14 * d35 * d41 * d53 * d66 -
                d14 * d35 * d41 * d56 * d63 -
                d14 * d35 * d43 * d51 * d66 +
                d14 * d35 * d43 * d56 * d61 +
                d14 * d35 * d46 * d51 * d63 -
                d14 * d35 * d46 * d53 * d61 -
                d14 * d36 * d41 * d53 * d65 +
                d14 * d36 * d41 * d55 * d63 +
                d14 * d36 * d43 * d51 * d65 -
                d14 * d36 * d43 * d55 * d61 -
                d14 * d36 * d45 * d51 * d63 +
                d14 * d36 * d45 * d53 * d61 -
                d15 * d31 * d43 * d54 * d66 +
                d15 * d31 * d43 * d56 * d64 +
                d15 * d31 * d44 * d53 * d66 -
                d15 * d31 * d44 * d56 * d63 -
                d15 * d31 * d46 * d53 * d64 +
                d15 * d31 * d46 * d54 * d63 +
                d15 * d33 * d41 * d54 * d66 -
                d15 * d33 * d41 * d56 * d64 -
                d15 * d33 * d44 * d51 * d66 +
                d15 * d33 * d44 * d56 * d61 +
                d15 * d33 * d46 * d51 * d64 -
                d15 * d33 * d46 * d54 * d61 -
                d15 * d34 * d41 * d53 * d66 +
                d15 * d34 * d41 * d56 * d63 +
                d15 * d34 * d43 * d51 * d66 -
                d15 * d34 * d43 * d56 * d61 -
                d15 * d34 * d46 * d51 * d63 +
                d15 * d34 * d46 * d53 * d61 +
                d15 * d36 * d41 * d53 * d64 -
                d15 * d36 * d41 * d54 * d63 -
                d15 * d36 * d43 * d51 * d64 +
                d15 * d36 * d43 * d54 * d61 +
                d15 * d36 * d44 * d51 * d63 -
                d15 * d36 * d44 * d53 * d61 +
                d16 * d31 * d43 * d54 * d65 -
                d16 * d31 * d43 * d55 * d64 -
                d16 * d31 * d44 * d53 * d65 +
                d16 * d31 * d44 * d55 * d63 +
                d16 * d31 * d45 * d53 * d64 -
                d16 * d31 * d45 * d54 * d63 -
                d16 * d33 * d41 * d54 * d65 +
                d16 * d33 * d41 * d55 * d64 +
                d16 * d33 * d44 * d51 * d65 -
                d16 * d33 * d44 * d55 * d61 -
                d16 * d33 * d45 * d51 * d64 +
                d16 * d33 * d45 * d54 * d61 +
                d16 * d34 * d41 * d53 * d65 -
                d16 * d34 * d41 * d55 * d63 -
                d16 * d34 * d43 * d51 * d65 +
                d16 * d34 * d43 * d55 * d61 +
                d16 * d34 * d45 * d51 * d63 -
                d16 * d34 * d45 * d53 * d61 -
                d16 * d35 * d41 * d53 * d64 +
                d16 * d35 * d41 * d54 * d63 +
                d16 * d35 * d43 * d51 * d64 -
                d16 * d35 * d43 * d54 * d61 -
                d16 * d35 * d44 * d51 * d63 +
                d16 * d35 * d44 * d53 * d61 -
                2 * d21 * d32 * d44 * d55 * d66 +
                2 * d21 * d32 * d44 * d56 * d65 +
                2 * d21 * d32 * d45 * d54 * d66 -
                2 * d21 * d32 * d45 * d56 * d64 -
                2 * d21 * d32 * d46 * d54 * d65 +
                2 * d21 * d32 * d46 * d55 * d64 +
                2 * d21 * d34 * d42 * d55 * d66 -
                2 * d21 * d34 * d42 * d56 * d65 -
                2 * d21 * d34 * d45 * d52 * d66 +
                2 * d21 * d34 * d45 * d56 * d62 +
                2 * d21 * d34 * d46 * d52 * d65 -
                2 * d21 * d34 * d46 * d55 * d62 -
                2 * d21 * d35 * d42 * d54 * d66 +
                2 * d21 * d35 * d42 * d56 * d64 +
                2 * d21 * d35 * d44 * d52 * d66 -
                2 * d21 * d35 * d44 * d56 * d62 -
                2 * d21 * d35 * d46 * d52 * d64 +
                2 * d21 * d35 * d46 * d54 * d62 +
                2 * d21 * d36 * d42 * d54 * d65 -
                2 * d21 * d36 * d42 * d55 * d64 -
                2 * d21 * d36 * d44 * d52 * d65 +
                2 * d21 * d36 * d44 * d55 * d62 +
                2 * d21 * d36 * d45 * d52 * d64 -
                2 * d21 * d36 * d45 * d54 * d62 +
                2 * d22 * d31 * d44 * d55 * d66 -
                2 * d22 * d31 * d44 * d56 * d65 -
                2 * d22 * d31 * d45 * d54 * d66 +
                2 * d22 * d31 * d45 * d56 * d64 +
                2 * d22 * d31 * d46 * d54 * d65 -
                2 * d22 * d31 * d46 * d55 * d64 -
                2 * d22 * d34 * d41 * d55 * d66 +
                2 * d22 * d34 * d41 * d56 * d65 +
                2 * d22 * d34 * d45 * d51 * d66 -
                2 * d22 * d34 * d45 * d56 * d61 -
                2 * d22 * d34 * d46 * d51 * d65 +
                2 * d22 * d34 * d46 * d55 * d61 +
                2 * d22 * d35 * d41 * d54 * d66 -
                2 * d22 * d35 * d41 * d56 * d64 -
                2 * d22 * d35 * d44 * d51 * d66 +
                2 * d22 * d35 * d44 * d56 * d61 +
                2 * d22 * d35 * d46 * d51 * d64 -
                2 * d22 * d35 * d46 * d54 * d61 -
                2 * d22 * d36 * d41 * d54 * d65 +
                2 * d22 * d36 * d41 * d55 * d64 +
                2 * d22 * d36 * d44 * d51 * d65 -
                2 * d22 * d36 * d44 * d55 * d61 -
                2 * d22 * d36 * d45 * d51 * d64 +
                2 * d22 * d36 * d45 * d54 * d61 -
                2 * d24 * d31 * d42 * d55 * d66 +
                2 * d24 * d31 * d42 * d56 * d65 +
                2 * d24 * d31 * d45 * d52 * d66 -
                2 * d24 * d31 * d45 * d56 * d62 -
                2 * d24 * d31 * d46 * d52 * d65 +
                2 * d24 * d31 * d46 * d55 * d62 +
                2 * d24 * d32 * d41 * d55 * d66 -
                2 * d24 * d32 * d41 * d56 * d65 - 
                2 * d24 * d32 * d45 * d51 * d66 +
                2 * d24 * d32 * d45 * d56 * d61 +
                2 * d24 * d32 * d46 * d51 * d65 -
                2 * d24 * d32 * d46 * d55 * d61 -
                2 * d24 * d35 * d41 * d52 * d66 +
                2 * d24 * d35 * d41 * d56 * d62 +
                2 * d24 * d35 * d42 * d51 * d66 -
                2 * d24 * d35 * d42 * d56 * d61 -
                2 * d24 * d35 * d46 * d51 * d62 +
                2 * d24 * d35 * d46 * d52 * d61 +
                2 * d24 * d36 * d41 * d52 * d65 -
                2 * d24 * d36 * d41 * d55 * d62 -
                2 * d24 * d36 * d42 * d51 * d65 +
                2 * d24 * d36 * d42 * d55 * d61 +
                2 * d24 * d36 * d45 * d51 * d62 -
                2 * d24 * d36 * d45 * d52 * d61 +
                2 * d25 * d31 * d42 * d54 * d66 -
                2 * d25 * d31 * d42 * d56 * d64 -
                2 * d25 * d31 * d44 * d52 * d66 +
                2 * d25 * d31 * d44 * d56 * d62 +
                2 * d25 * d31 * d46 * d52 * d64 -
                2 * d25 * d31 * d46 * d54 * d62 -
                2 * d25 * d32 * d41 * d54 * d66 +
                2 * d25 * d32 * d41 * d56 * d64 +
                2 * d25 * d32 * d44 * d51 * d66 -
                2 * d25 * d32 * d44 * d56 * d61 -
                2 * d25 * d32 * d46 * d51 * d64 +
                2 * d25 * d32 * d46 * d54 * d61 +
                2 * d25 * d34 * d41 * d52 * d66 -
                2 * d25 * d34 * d41 * d56 * d62 -
                2 * d25 * d34 * d42 * d51 * d66 +
                2 * d25 * d34 * d42 * d56 * d61 +
                2 * d25 * d34 * d46 * d51 * d62 -
                2 * d25 * d34 * d46 * d52 * d61 -
                2 * d25 * d36 * d41 * d52 * d64 +
                2 * d25 * d36 * d41 * d54 * d62 +
                2 * d25 * d36 * d42 * d51 * d64 -
                2 * d25 * d36 * d42 * d54 * d61 -
                2 * d25 * d36 * d44 * d51 * d62 +
                2 * d25 * d36 * d44 * d52 * d61 -
                2 * d26 * d31 * d42 * d54 * d65 +
                2 * d26 * d31 * d42 * d55 * d64 +
                2 * d26 * d31 * d44 * d52 * d65 -
                2 * d26 * d31 * d44 * d55 * d62 -
                2 * d26 * d31 * d45 * d52 * d64 +
                2 * d26 * d31 * d45 * d54 * d62 +
                2 * d26 * d32 * d41 * d54 * d65 -
                2 * d26 * d32 * d41 * d55 * d64 -
                2 * d26 * d32 * d44 * d51 * d65 +
                2 * d26 * d32 * d44 * d55 * d61 +
                2 * d26 * d32 * d45 * d51 * d64 -
                2 * d26 * d32 * d45 * d54 * d61 -
                2 * d26 * d34 * d41 * d52 * d65 +
                2 * d26 * d34 * d41 * d55 * d62 +
                2 * d26 * d34 * d42 * d51 * d65 -
                2 * d26 * d34 * d42 * d55 * d61 -
                2 * d26 * d34 * d45 * d51 * d62 +
                2 * d26 * d34 * d45 * d52 * d61 +
                2 * d26 * d35 * d41 * d52 * d64 -
                2 * d26 * d35 * d41 * d54 * d62 -
                2 * d26 * d35 * d42 * d51 * d64 +
                2 * d26 * d35 * d42 * d54 * d61 + 
                2 * d26 * d35 * d44 * d51 * d62 -
                2 * d26 * d35 * d44 * d52 * d61;

            double koefficientX = bigRationalkoefficientX.ToDouble();
            return koefficientX;
        }
    }
}