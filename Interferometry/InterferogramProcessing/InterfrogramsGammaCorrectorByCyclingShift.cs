using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.GammaCorrection;
using ExtraLibrary.Mathematics.Approximation;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

namespace Interferometry.InterferogramProcessing {
    public class InterfrogramsGammaCorrectorByCyclingShift {
        //---------------------------------------------------------------------------------------------------------------------
        public RealMatrix[] GetCorrectedInterferograms( RealMatrix[] interferograms, double[] gammaValues ) {
            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;
            
            RealMatrix[] gammaCorrectedInterferograms = new RealMatrix[ interferograms.Length ];
            for ( int index = 0; index < gammaCorrectedInterferograms.Length; index++ ) {
                gammaCorrectedInterferograms[ index ] = new RealMatrix( height, width );
            }
                               
            for ( int x = 0; x < width; x++ ) {
                for ( int y = 0; y < height; y++ ) {
                    double[] intensities = MatricesManager.GeValuesFromMatrices( y, x, interferograms );
                    int cyclingShiftsCount = 0;
                    double[] intensitiesByCyclingShift =
                        ArrayOperator.GetArrayByCyclingShiftWithMinimumValueInOrigin( intensities, out cyclingShiftsCount );
                    
                    int optimalGammaValueIndex = this.GetOptimalGammaValueIndex( intensitiesByCyclingShift, gammaValues );
                    double gamma = gammaValues[ optimalGammaValueIndex ];
                    
                    double[] gammaCorrectedIntensities = ArrayOperator.GetValuesInPower( intensities, gamma );

                    double[] resultValues =
                        ArrayOperator.GetArrayByReverseCyclingShift( gammaCorrectedIntensities, cyclingShiftsCount );
                    MatricesManager.SetValuesInMatrices( gammaCorrectedInterferograms, resultValues, y, x ); 
                }
            }

            return gammaCorrectedInterferograms;
        }
        //---------------------------------------------------------------------------------------------------------------------
        //Индекс оптимального значения гаммы
        private int GetOptimalGammaValueIndex( double[] intensitiesByCyclingShift, double[] gammaValues ) {
            Dictionary<int, double> rootMeanSquareErrors = new Dictionary<int, double>();
            for ( int index = 0; index < gammaValues.Length; index++ ) {
                double gamma = gammaValues[ index ];
                double[] correctedIntensities = ArrayOperator.GetValuesInPower( intensitiesByCyclingShift, gamma );
                double rootMeanSquareError = Statistician.GetRootMeanSquareError( intensitiesByCyclingShift, correctedIntensities );
                rootMeanSquareErrors.Add( index, rootMeanSquareError );
            }

            int optimalIndex = 0;
            double optimalError = rootMeanSquareErrors[ 0 ];
            foreach ( KeyValuePair<int, double> pair in rootMeanSquareErrors ) {
                if ( pair.Value < optimalError ) {
                    optimalIndex = pair.Key;
                }
            }

            return optimalIndex;
        }
        //---------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------

    }
}
