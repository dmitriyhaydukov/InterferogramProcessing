using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using ExtraLibrary.Geometry2D;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics;
using ExtraLibrary.Mathematics.Approximation;

namespace Interferometry.InterferogramProcessing {
    
    //Ориентация траектории по оси
    enum TrajectoryOrientation {
        AxisX, AxisY, Undefined
    };

    //Вычилитель фазовых сдвигов
    public class PhaseShiftComputer {
        //-------------------------------------------------------------------------------------------
        RealMatrix[] interferograms;  //Массив интерферограмм
        //-------------------------------------------------------------------------------------------
        public Point2D[] TrajectoryPoints;
        public Point2D[] EllipsePoints;
        public double[] IntensitiesForPointOne;
        //-------------------------------------------------------------------------------------------
        public PhaseShiftComputer( RealMatrix[] interferograms ) {
            this.interferograms = interferograms;
        }
        //-------------------------------------------------------------------------------------------
        //Вычислить фазовые сдвиги
        public double[] Compute(
            Point pointOne,
            Point pointTwo
        ) {
            TrajectoryCreator trajectoryCreator = new TrajectoryCreator( this.interferograms ); 
            Curve2D trajectory = trajectoryCreator.GetTrajectory ( pointOne, pointTwo );
            IntensitiesForPointOne = trajectory.GetArrayX();

            EllipseApproximator approximator = new EllipseApproximator();
            Point2D[] trajectoryPoints = trajectory.GetPoints();
            this.TrajectoryPoints = trajectoryPoints;
            QuadricCurveDescriptor approximatingQuadricCurve = approximator.Approximate( trajectoryPoints );
            EllipseDescriptor approximatingEllipse = approximatingQuadricCurve as EllipseDescriptor;

            double startX = 0;
            double finishX = 255;
            double step = 1;
            this.EllipsePoints = approximatingEllipse.GetPoints( startX, finishX, step );

            Curve2D transformedTrajectory =
                this.TransformateTrajectory( trajectory, approximatingEllipse );
            double[] phaseShifts = this.CalculatePhaseShifts( transformedTrajectory );
            double[] correctedPhaseShifts = this.CorrectPhaseShifts( phaseShifts );
            return correctedPhaseShifts;
        }
        //---------------------------------------------------------------------------------------------
        //Трансформация траектоории (центрирование, поворот, растяжение)
        private Curve2D TransformateTrajectory(
            Curve2D trajectory,
            EllipseDescriptor approximateEllipse
        ) {
            //Центрирование
            Curve2D transformedTrajectory = this.CentreTrajectory( trajectory, approximateEllipse );
            
            //Поворот параллельно координатной оси
            double rotationAngleToAxis = approximateEllipse.GetAngleBetweenOxAndPrincipalAxis();
            transformedTrajectory =
                this.RotateTrajectory( transformedTrajectory, rotationAngleToAxis );
            
            //Растяжение
            EllipseOrientaion primaryOrientation = approximateEllipse.GetOrientaion();
            TrajectoryOrientation trajectoryOrientation =
                this.DetectTrajectoryOrientation( primaryOrientation, rotationAngleToAxis );
            double koefficientOfStretch = 1 / approximateEllipse.GetCompressionRatio();
            transformedTrajectory = this.StretchingTrajectory
                ( transformedTrajectory, trajectoryOrientation, koefficientOfStretch );
            
            //Поворот траектории до пересечения первой точки с осью OX
            Point2D firstPointTrajectory = transformedTrajectory[ 0 ];
            double rotationAngle = Mathem.Arctg( firstPointTrajectory.Y, firstPointTrajectory.X );
            transformedTrajectory =
                this.RotateTrajectory( transformedTrajectory, rotationAngle );
            return transformedTrajectory;
        }
        //---------------------------------------------------------------------------------------------
        //Центрирование траектории
        private Curve2D CentreTrajectory(
            Curve2D trajectory,                         //Траектория
            EllipseDescriptor approximatingEllipse      //Аппроксимирующий эллипс
        ) {
            Point2D centreEllipse = approximatingEllipse.GetCentre();
            Curve2D transformedTrajectory =
                trajectory.GetDisplacementCurve( -centreEllipse.X, -centreEllipse.Y );
            return transformedTrajectory;
        }
        //-------------------------------------------------------------------------------------------
        //Поворот траектории
        private Curve2D RotateTrajectory(
           Curve2D trajectory,
           double rotationAngle     //Угол поворота
        ) {
            Curve2D transformedTrajectory =
                trajectory.GetRotatedCurve( rotationAngle );
            return transformedTrajectory;
        }
        //-------------------------------------------------------------------------------------------
        //Растяжение траектории до окружности
        private Curve2D StretchingTrajectory(
            Curve2D trajectory,
            TrajectoryOrientation trajectoryOrientation,
            double koefficientOfStretch 
        ) {
            Curve2D transformedTrajectory = null;
            if ( trajectoryOrientation == TrajectoryOrientation.AxisX ) {
                transformedTrajectory =
                    trajectory.GetStretchCurveAlongAxisY( koefficientOfStretch );
            }
            if ( trajectoryOrientation == TrajectoryOrientation.AxisY ) {
                transformedTrajectory =
                    trajectory.GetStretchCurveAlongAxisX( koefficientOfStretch );
            }
            return transformedTrajectory;
        }
        //-------------------------------------------------------------------------------------------
        //Определение ориентации траектории
        private TrajectoryOrientation DetectTrajectoryOrientation(
            EllipseOrientaion primaryEllipseOrientation,    //Первоначальная ориентация
            double rotationAngle                            //Угол поворота первоначальной траектории
        ) {
            TrajectoryOrientation trajectoryOrientation = TrajectoryOrientation.Undefined;

            if ( primaryEllipseOrientation == EllipseOrientaion.NorthEast ) {
                if ( 0 < rotationAngle ) {
                    trajectoryOrientation = TrajectoryOrientation.AxisX;
                }
                else {
                    trajectoryOrientation = TrajectoryOrientation.AxisY;
                }
            }
            if ( primaryEllipseOrientation == EllipseOrientaion.NorthWest ) {
                if ( 0 < rotationAngle ) {
                    trajectoryOrientation = TrajectoryOrientation.AxisY;
                }
                else {
                    trajectoryOrientation = TrajectoryOrientation.AxisX;
                }
            }
            return trajectoryOrientation;
        }
        //-------------------------------------------------------------------------------------------
        //Вычисление фазовых сдвигов по траектории
        private double[] CalculatePhaseShifts( Curve2D trajectory ) {
            int shiftsCount = trajectory.PointsCount;
            double[] phaseShifts = new double[ shiftsCount ];

            for ( int index = 0; index < shiftsCount; index++ ) {
                Point2D point = trajectory[ index ];
                double phaseShift = Mathem.Arctg( point.Y, point.X );
                phaseShifts[ index ] = phaseShift;
            }
            return phaseShifts;
        }
        //-------------------------------------------------------------------------------------------
        //Корректирвка вектора фазовых сдвигов
        private double[] CorrectPhaseShifts( double[] phaseShifts ) {
            int shiftsCount = phaseShifts.Length;
            int[] signs = new int[ shiftsCount - 1 ];
 
            for ( int index = 1; index < shiftsCount - 1; index++ ) {
                double curValue = phaseShifts[ index ];
                double prevValue = phaseShifts[ index - 1 ];
                if ( curValue < prevValue ) {
                    signs[ index ] = -1;
                }
                else {
                    signs[ index ] = 1;
                }
            }

            double sum = 0;
            for ( int index = 0; index < signs.Length; index++ ) {
                sum += signs[ index ];
            }
            double[] correctedPhaseShifts = new double[ shiftsCount ];
            if ( sum < 0 ) {
                for ( int index = 0; index < shiftsCount; index++ ) {
                    correctedPhaseShifts[ index ] = 2 * Math.PI - phaseShifts[ index ];
                }
            }
            else {
                phaseShifts.CopyTo( correctedPhaseShifts, 0 );
            }
            return correctedPhaseShifts;
        }
        //-------------------------------------------------------------------------------------------
    }
}
