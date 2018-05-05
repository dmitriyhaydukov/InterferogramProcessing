using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.GammaCorrection;
using ExtraLibrary.Mathematics.Approximation;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

using Interferometry.Helpers;

namespace Interferometry.InterferogramProcessing {
    public class InterferogramGammaCorrector {
        //---------------------------------------------------------------------------------------
        public double[] GammaValues {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------
        public double[] TargetFunctionValues {
            get;
            private set;
        }
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
        //Автоматическая гамма-коррекция интерферограмм
        public RealMatrix[] GetGammaCorrectedInterferograms(
            RealMatrix[] interferograms, BitMask2D bitMask
        ) {
            if ( interferograms.Length != 3 ) {
                throw new Exception();
            }
            double[] gammaValues = this.GetGammaValues();
            
            this.GammaValues = gammaValues;
            this.TargetFunctionValues = new double[ gammaValues.Length ];
            
            double firstGammaValue = gammaValues[ 0 ];
            double optimalGammaValue = firstGammaValue;
            double minTargetFunctionValue = 
                this.GetTargetFunctionValue( firstGammaValue, interferograms, bitMask );
            this.TargetFunctionValues[ 0 ] = minTargetFunctionValue;
            
            for ( int index = 1; index < gammaValues.Length; index++ ) {
                double gamma = gammaValues[ index ];
                double targetFunctionValue = this.GetTargetFunctionValue( gamma, interferograms, bitMask );
                this.TargetFunctionValues[ index ] = targetFunctionValue;
                if ( targetFunctionValue < minTargetFunctionValue ) {
                    minTargetFunctionValue = targetFunctionValue;
                    optimalGammaValue = gamma;
                }
            }
            RealMatrix[] resultInterferograms = 
                this.GetGammaCorrectedInterferograms( optimalGammaValue, interferograms );

            return resultInterferograms;
        }
        //---------------------------------------------------------------------------------------
        //Гамма-коррекция интерферограмм
        public RealMatrix[] GetGammaCorrectedInterferograms(
            double gamma, RealMatrix[] interferograms
        ) {
            double[] gammaCoefficients = new double[] { 1 };
            double[] gammaPowers = new double[] { gamma };
            RealMatrix[] gammaCorrectedMatrices = MatricesManager.GetGammaCorrectedMatrices
                ( gammaCoefficients, gammaPowers, interferograms );
            return gammaCorrectedMatrices;
        }
        //---------------------------------------------------------------------------------------
        //Значения гаммы
        private double[] GetGammaValues() {
            double startValue = 1.0;
            double finishValue = 4.0;
            double step = 0.2;

            List<double> gammaValues = new List<double>();
            for ( double gamma = startValue; gamma <= finishValue; gamma += step ) {
                gammaValues.Add( gamma );
            }
            return gammaValues.ToArray();
        }
        //------------------------------------------------------------------------------------------
        private Point3D[] GetSpatialPoints(RealMatrix[] interferograms, BitMask2D bitMask) {
            Point3D[] points = 
                InterferometryHelper.GetSpatialPointsFromInterferograms( interferograms, bitMask );
            return points;
        }
        //------------------------------------------------------------------------------------------
        //Значение целевой функции
        private double CalculateTargetFunctionValue( double averageValue, params double[] values ) {
            double sum = 0;
            double n = values.Length;
            for ( int index = 0; index < values.Length; index++ ) {
                double value = values[ index ];
                double summmand = this.CalculateValue( averageValue, value );
                sum += summmand;
            }
            double result = sum / n;
            return result;
        }
        //------------------------------------------------------------------------------------------
        private double CalculateValue( double averageValue, double value ) {
            //double result = Math.Pow( ( value - averageValue ) / averageValue, 2 );
            double result = Math.Abs( ( value - averageValue ) / averageValue );
            return result;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        /*
        //Значение целевой функции для интерферограмм при определенном значении гамма
        //( Расстояния до средней точки ) 
        private double GetTargetFunctionValue(
            double gamma, RealMatrix[] interferograms, BitMask2D bitMask
        ) {
            RealMatrix[] gammaCorrectedInterferograms =
                this.GetGammaCorrectedInterferograms( gamma, interferograms );
            Point3D[] points = this.GetSpatialPoints( gammaCorrectedInterferograms, bitMask );
            Point3D midPoint = SpaceManager.GetMidPoint( points );
            double[] distancesFromPointsToMidPoint = SpaceManager.GetDistances( points, midPoint );
            double averageDistance = distancesFromPointsToMidPoint.Average();
            double targetFunctionValue =
                this.CalculateTargetFunctionValue( averageDistance, distancesFromPointsToMidPoint );
            return targetFunctionValue;
        }
        */ 
        //------------------------------------------------------------------------------------------
        //Значение целевой функции для интерферограмм при определенном значении гамма
        //( Аппроксимация плоскостью - вычисление невязки )
        private double GetTargetFunctionValue(
            double gamma, RealMatrix[] interferograms, BitMask2D bitMask
        ) {
            RealMatrix[] gammaCorrectedInterferograms =
                this.GetGammaCorrectedInterferograms( gamma, interferograms );
            Point3D[] points = this.GetSpatialPoints( gammaCorrectedInterferograms, bitMask );
            PlaneApproximator planeApproximator = new PlaneApproximator();
            PlaneDescriptor planeDescriptor = planeApproximator.Approximate( points );

            double[] misalignments = planeDescriptor.GetPointsMisalignments( points );
            double averageMisalignment = misalignments.Average();
            double targetFunctionValue =
                this.CalculateTargetFunctionValue( averageMisalignment, misalignments );
            return targetFunctionValue;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
    }
}
