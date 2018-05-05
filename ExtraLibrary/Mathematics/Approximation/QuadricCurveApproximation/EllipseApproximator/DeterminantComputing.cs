using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Mathematics.Approximation {
    public partial class EllipseApproximator {
        //-----------------------------------------------------------------------------------------
        //Вычисление коэффициентов кубического уравнения
        private double[] ComputeKoefficients( RealMatrix matrix ) {
            double koefficientCubeX = this.ComputeCoefficientCubeX( matrix );
            double KoefficientSquareX = this.ComputeCoefficientSquareX( matrix );
            double koefficientX = this.ComputeCoefficientX( matrix );
            double absoluteTerm = this.ComputeAbsoluteTerm( matrix );
            double[] koefficients = new double[] {
                koefficientCubeX,
                KoefficientSquareX,
                koefficientX,
                absoluteTerm
            };
            return koefficients;
        }
        //-----------------------------------------------------------------------------------------
    }
}