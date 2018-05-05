using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Geometry2D {
    //----------------------------------------------------------------------------------------------
    //Ориентация эллипса
    public enum EllipseOrientaion {
        NorthWest, NorthEast, AxisX, AxisY, Undefined
    };
    //----------------------------------------------------------------------------------------------
    //Эллипс
    public class EllipseDescriptor : QuadricCurveDescriptor {
        //------------------------------------------------------------------------------------------
        //Конструктор
        public EllipseDescriptor(
            double a11,
            double a22,
            double a12,
            double a13,
            double a23,
            double a33
        ) : base( a11, a22, a12, a13, a23, a33 ) {
            double invarintD = this.GetInvariantD();
            if ( invarintD <= 0 ) {
                throw new EllipseException();
            }
            else {
                double invariantA = this.GetInvariantA();
                double invariantI = this.GetInvariantI();
                if ( (invariantA / invariantI) >= 0 ) {
                    throw new EllipseException();
                }
            }
        }
        //------------------------------------------------------------------------------------------
        //Коэффициент сжатия эллипса
        public double GetCompressionRatio() {
            double[] roots = this.GetCharacteristicEquationRoots();
            double rootOne = roots[ 0 ];
            double rootTwo = roots[ 1 ]; 
            
            double rootsRatio = rootOne / rootTwo;
            double principalAxesRatio;
            if ( rootsRatio < 1 ) {
                principalAxesRatio = Math.Sqrt( rootOne / rootTwo );
            }
            else {
                principalAxesRatio = Math.Sqrt( rootTwo / rootOne );
            }
            return principalAxesRatio;
        }
        //------------------------------------------------------------------------------------------
        //Ориентация эллипса на плоскости
        public EllipseOrientaion GetOrientaion() {
            int signA11 = Math.Sign( this.A11 );
            int signA12 = Math.Sign( this.A12 );
            EllipseOrientaion orientation = EllipseOrientaion.Undefined;
            if ( signA11 == signA12 ) {
                orientation = EllipseOrientaion.NorthWest;
            }
            else {
                orientation = EllipseOrientaion.NorthEast;
            }
            return orientation;
        }
        //------------------------------------------------------------------------------------------
        //Большая полуось эллипса
        public double GetSemiMajorAxis() {
            double[] halfAxlesLengths = this.GetSemiAxesLengths();
            double halfAxleOne = halfAxlesLengths[ 0 ];
            double halfAxleTwo = halfAxlesLengths[ 1 ];
            if ( halfAxleTwo < halfAxleOne ) {
                return halfAxleOne;
            }
            else {
                return halfAxleTwo;
            }
        }
        //------------------------------------------------------------------------------------------
        //Малая полуось эллипса
        public double GetSemiMinorAxis() {
            double[] halfAxlesLengths = this.GetSemiAxesLengths();
            double halfAxleOne = halfAxlesLengths[ 0 ];
            double halfAxleTwo = halfAxlesLengths[ 1 ];
            if ( halfAxleTwo < halfAxleOne ) {
                return halfAxleTwo;
            }
            else {
                return halfAxleOne;
            }
        }
        //------------------------------------------------------------------------------------------
        //Длины полуосей эллипса
        public double[] GetSemiAxesLengths() {
            double[] roots = this.GetCharacteristicEquationRoots();
            double rootOne = roots[ 0 ];
            double rootTwo = roots[ 1 ];

            double invariantA = this.GetInvariantA();
            double invariantD = this.GetInvariantD();

            double ratioInvariantAtoInvariantD = invariantA / invariantD;
            
            double halfAxleOne = Math.Sqrt( -1 / rootOne * ratioInvariantAtoInvariantD );
            double halfAxleTwo = Math.Sqrt( -1 / rootTwo * ratioInvariantAtoInvariantD );

            double[] halfAxlesLengths = new double[] { halfAxleOne, halfAxleTwo };
            return halfAxlesLengths;
        }
        //------------------------------------------------------------------------------------------
        //Эксцентриситет эллипса
        public double GetEccentricity() {
            double bigHalfAxle = this.GetSemiMajorAxis();
            double smallHalfAxle = this.GetSemiMinorAxis();

            double eccentricity =
                Math.Sqrt( 1 - ( smallHalfAxle * smallHalfAxle ) / ( bigHalfAxle * bigHalfAxle ) );
            return eccentricity;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

    }
}
