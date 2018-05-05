using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;
using ExtraLibrary.Mathematics;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Mathematics.Approximation;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Mathematics.Transformation;


namespace Interferometry.InterferogramDecoding {
    public class FirstHarmonicRotationInterferogramDecoder : InterferogramDecoder {
        //--------------------------------------------------------------------------------------------------------
        public InterferogramDecodingResult DecodeInterferogram( RealMatrix[] interferograms) {

            RealMatrix firstInterferogram = interferograms[ 0 ];

            int width = firstInterferogram.ColumnCount;
            int height = firstInterferogram.RowCount;

            RealMatrix resultMatrix = new RealMatrix( height, width );
            
            int firstPointCoordinateX = 0;
            int firstPointCoordinateY = 0;

            FourierTransform fourierTransform = new FourierTransform();
            double[] firstPointIntensities =
                this.CreateIntensitiesAtPoint( firstPointCoordinateX, firstPointCoordinateY, interferograms );
            Complex firstPointFirstHarmonic = fourierTransform.GetFirstHarmonic( firstPointIntensities );

            double C1 = firstPointFirstHarmonic.Real;
            double S1 = firstPointFirstHarmonic.Imaginary;

            for ( int x = 0; x < width; x++ ) {
                for ( int y = 0; y < height; y++ ) {
                    double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                    Complex firstHarmonic = fourierTransform.GetFirstHarmonic( intensities );
                    double C2 = firstHarmonic.Real;
                    double S2 = firstHarmonic.Imaginary;

                    resultMatrix[ y, x ] = GetPhase( C1, C2, S1, S2 );
                }
            }

            InterferogramDecodingResult decodingResult = new InterferogramDecodingResult( resultMatrix );
            return decodingResult;
        }
        //--------------------------------------------------------------------------------------------------------
        private double GetPhase( double C1, double C2, double S1, double S2 ) {
            double phase = Math.Atan2( C1 * C2 + S1 * S2, S1 * C2 - C1 * S2 );
            return phase;
        }
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
    }
}
