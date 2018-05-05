using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Arraying.ArrayOperation;

namespace Interferometry.InterferogramDecoding {
    public class GenericInterferogramDecoder : InterferogramDecoder {
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------
        //Вычисление фазы
        private double CalculatePhase(
            RealVector orthogonalIntensitiesVector,
            RealVector sinPhaseShiftsVector,
            RealVector cosPhaseShiftsVector
        ) {
            double numerator = orthogonalIntensitiesVector * sinPhaseShiftsVector;
            double denominator = orthogonalIntensitiesVector * cosPhaseShiftsVector;
            double phase = Math.PI + Math.Atan2( denominator, numerator );
            return phase;
        }
        //-------------------------------------------------------------------------------------------
        //Числитель формулы расшифровки
        public double GetDecodingFormulaNumerator( double[] intensities, double[] phaseShifts ) {
            RealVector sinPhaseShiftsVector = this.GetSinVector( phaseShifts );
            RealVector intensitiesVector = new RealVector( intensities );
            RealVector orthogonalIntensitiesVector = intensitiesVector.GetOrthogonalVector();
            double numerator = orthogonalIntensitiesVector * sinPhaseShiftsVector;
            return numerator;
        }
        //--------------------------------------------------------------------------------------------------
        //Знаменатель формулы расшифровки
        private double GetDecodingFormulaDenominator( double[] intensities, double[] phaseShifts ) {
            RealVector cosPhaseShiftsVector = this.GetCosVector( phaseShifts );
            RealVector intensitiesVector = new RealVector( intensities );
            RealVector orthogonalIntensitiesVector = intensitiesVector.GetOrthogonalVector();
            double denominator = orthogonalIntensitiesVector * cosPhaseShiftsVector;
            return denominator;
        }
        //---------------------------------------------------------------------------------------------------
        //Числители формулы расшифровки
        public double[] GetDecodingFormulaNumerators(
            RealMatrix[] interferograms, double[] phaseShifts, BitMask2D bitMask
        ) {
            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;
            List<double> numerators = new List<double>();
            if ( bitMask == null ) {
                for ( int x = 0; x < width; x++ ) {
                    for ( int y = 0; y < height; y++ ) {
                        double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                        double numerator = this.GetDecodingFormulaNumerator( intensities, phaseShifts );
                        numerators.Add( numerator );
                    }
                }
            }
            else {
                for ( int x = 0; x < width; x++ ) {
                    for ( int y = 0; y < height; y++ ) {
                        if ( bitMask[ y, x ] ) {
                            double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                            double numerator = this.GetDecodingFormulaNumerator( intensities, phaseShifts );
                            numerators.Add( numerator );
                        }
                    }
                }
            }
            return numerators.ToArray();
        }
        //---------------------------------------------------------------------------------------------------
        //Знаменатели формулы расшифровки
        public double[] GetDecodingFormulaDenominators( 
            RealMatrix[] interferograms, double[] phaseShifts, BitMask2D bitMask
        ) {
            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;
            List<double> denominators = new List<double>();
            if ( bitMask == null ) {
                for ( int x = 0; x < width; x++ ) {
                    for ( int y = 0; y < height; y++ ) {
                        double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                        double denominator = this.GetDecodingFormulaDenominator( intensities, phaseShifts );
                        denominators.Add( denominator );
                    }
                }
            }
            else {
                for ( int x = 0; x < width; x++ ) {
                    for ( int y = 0; y < height; y++ ) {
                        if ( bitMask[ y, x ] ) {
                            double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                            double denominator = this.GetDecodingFormulaDenominator( intensities, phaseShifts );
                            denominators.Add( denominator );
                        }
                    }
                }
            }
            return denominators.ToArray();
        }
        //--------------------------------------------------------------------------------------------------
        //Расшифровка в точке
        public double Decode( double[] intensities, double[] phaseShifts ) {
            RealVector cosVector = this.GetCosVector( phaseShifts );
            RealVector sinVector = this.GetSinVector( phaseShifts );

            RealVector intensitiesVector = new RealVector( intensities );
            RealVector orthogonalIntensitiesVector = intensitiesVector.GetOrthogonalVector();
                        
            double phase = this.CalculatePhase( orthogonalIntensitiesVector, sinVector, cosVector );
            return phase;
        }
        //--------------------------------------------------------------------------------------
        //Вектор синусов значений
        private RealVector GetSinVector( double[] values ) {
            double[] sinValues = ArrayOperator.ComputeFunction( values, Math.Sin );
            RealVector sinVector = new RealVector( sinValues );
            return sinVector;
        }
        //--------------------------------------------------------------------------------------
        //Вектор косинусов значений
        private RealVector GetCosVector( double[] values ) {
            double[] cosValues = ArrayOperator.ComputeFunction( values, Math.Cos );
            RealVector cosVector = new RealVector( cosValues );
            return cosVector;
        }
        //-----------------------------------------------------------------------------------------------
        //Расшифровка интерферограммы
        public InterferogramDecodingResult DecodeInterferogram(
            RealMatrix[] interferograms, double[] phaseShifts, BitMask2D bitMask
        ) {
            RealVector cosVector = this.GetCosVector( phaseShifts );
            RealVector sinVector = this.GetSinVector( phaseShifts );

            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;
            RealMatrix phaseMatrix = new RealMatrix( height, width );

            for ( int x = 0; x < width; x++ ) {
                for ( int y = 0; y < height; y++ ) {
                    if ( bitMask[ y, x ] ) {
                        double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                        RealVector intensitiesVector = new RealVector( intensities );
                        RealVector orthogonalIntensitiesVector = intensitiesVector.GetOrthogonalVector();
                        double phase =
                            this.CalculatePhase( orthogonalIntensitiesVector, sinVector, cosVector );
                        phaseMatrix[ y, x ] = phase;
                    }
                    else {
                        phaseMatrix[ y, x ] = this.DefaultPhaseValue;
                    }
                }
            }
            InterferogramDecodingResult decodingResult = new InterferogramDecodingResult( phaseMatrix );
            return decodingResult;
        }
        //------------------------------------------------------------------------------------------------
        
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
