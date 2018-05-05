using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Approximation;
using ExtraLibrary.Geometry2D;

namespace Interferometry.InterferogramProcessing {
    public class AdvancedPhaseShiftComputer {
        //-------------------------------------------------------------------------------------------
        RealMatrix[] interferograms;  //Массив интерферограмм
        //-------------------------------------------------------------------------------------------
        public Point2D[] TrajectoryPoints {
            get;
            private set;
        }
        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
        public AdvancedPhaseShiftComputer( RealMatrix[] interferograms ) {
            this.interferograms = interferograms;
        }
        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
        public double[] Compute( 
            Point pointOne,
            Point pointTwo,
            int interferogramIndexOne,
            int interferogramIndexTwo
        ) {
            TrajectoryCreator trajectoryCreator = new TrajectoryCreator( this.interferograms );
            Curve2D trajectory = trajectoryCreator.GetTrajectory( pointOne, pointTwo );
            Point2D[] points = trajectory.GetPoints();
            this.TrajectoryPoints = points;
            EllipseApproximator ellipseApproximator = new EllipseApproximator();
            EllipseDescriptor ellipseDescriptor = ellipseApproximator.Approximate( points );
            Point2D ellipseCentre = ellipseDescriptor.GetCentre();
            points = PlaneManager.DisplacePoints( points, -ellipseCentre.X, -ellipseCentre.Y );

            double x0 = points[ 0 ].X;
            double y0 = points[ 0 ].Y;
            double x1 = points[ interferogramIndexOne ].X;
            double y1 = points[ interferogramIndexOne ].Y;
            double x2 = points[ interferogramIndexTwo ].X;
            double y2 = points[ interferogramIndexTwo ].Y;

            double[] phaseShifts = this.GetPhaseShifts( x0, y0, x1, y1, x2, y2 );
            return phaseShifts;
        }
        //-------------------------------------------------------------------------------------------
        //Отношение синусов 2-х углов фазовых сдвигов
        private double GetSinRatio(
            double x0, double y0,
            double x1, double y1,
            double x2, double y2
        ) {
            double sinRatio = ( x0 * y2 - x2 * y0 ) / ( x0 * y1 - x1 * y0 );
            return sinRatio;
        }
        //-------------------------------------------------------------------------------------------
        //Константа уравнения разности косинусов углов фазовых сдвигов
        private double GetEquationConstant(
            double sinRatio, double x0, double x1, double x2
        ) {
            double constant = ( sinRatio * x1 - x2 ) / x0;
            return constant;
        }
        //-------------------------------------------------------------------------------------------
        //Косинус 2-го угла фазового сдвига
        private double GetCosPhaseShiftTwo(double sinRatio, double equationConstant) {
            double cosPhaseShiftTwo =
                -( equationConstant * equationConstant - sinRatio * sinRatio + 1 ) /
                ( 2 * equationConstant );
            return cosPhaseShiftTwo;
        }
        //-------------------------------------------------------------------------------------------
        //Косинус 1-го угла фазового сдвига
        private double GetCosPhaseShiftOne(
            double sinRatio, double equationConstant, double cosPhaseShiftTwo
        ) {
            double cosPhaseShiftOne =
                ( cosPhaseShiftTwo + equationConstant ) / sinRatio;
            return cosPhaseShiftOne;
        }
        //-------------------------------------------------------------------------------------------
        //Вычисление фазовых сдвигов
        private double[] GetPhaseShifts(
            double x0, double y0,
            double x1, double y1,
            double x2, double y2
        ) {
            double sinRatio = this.GetSinRatio( x0, y0, x1, y1, x2, y2 );
            double equationConstant = this.GetEquationConstant( sinRatio, x0, x1, x2 );
            double cosPhaseShiftTwo = this.GetCosPhaseShiftTwo( sinRatio, equationConstant );
            double cosPhaseShiftOne = 
                this.GetCosPhaseShiftOne( sinRatio, equationConstant, cosPhaseShiftTwo );

            double phaseShiftOne = Math.Acos( cosPhaseShiftOne );
            double phaseShiftTwo = Math.Acos( cosPhaseShiftTwo );
            double[] phaseShifts = new double[] { phaseShiftOne, phaseShiftTwo };
            return phaseShifts;
        }
        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
    }
}
