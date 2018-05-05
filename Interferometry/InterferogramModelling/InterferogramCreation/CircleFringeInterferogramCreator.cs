using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics;
using ExtraLibrary.Mathematics.Matrices;

namespace Interferometry.InterferogramCreation {
    //Формирователь картин круговых интерференционных полос
    public class CircleFringeInterferogramCreator : InterferogramCreator {
        //-------------------------------------------------------------------------------
        public CircleFringeInterferogramCreator(
            InterferogramInfo InterferogramInfo
        ) : base( InterferogramInfo ) {
        }
        //-------------------------------------------------------------------------------
        public override RealMatrix CreateInterferogram( double phaseShift ) {
            RealMatrix interferogram = new RealMatrix
                ( this.interferogramInfo.Height, this.interferogramInfo.Width );
            for ( int x = 0; x < interferogram.ColumnCount; x++ ) {
                for ( int y = 0; y < interferogram.RowCount; y++ ) {
                    double intensity = this.CalculateIntensity( x, y, phaseShift );
                    interferogram[ y, x ] = intensity;
                }
            }
            return interferogram;
        }
        //-------------------------------------------------------------------------------
        public RealMatrix GetPhaseMatrix() {
            RealMatrix phaseMatrix = new RealMatrix( this.interferogramInfo.Height, this.interferogramInfo.Width );
            for ( int x = 0; x < phaseMatrix.ColumnCount; x++ ) {
                for ( int y = 0; y < phaseMatrix.RowCount; y++ ) {
                    double phase = this.CalculatePhase( x, y ); //% ( 2 * Math.PI );
                    phaseMatrix[ y, x ] = phase;
                }
            }
            return phaseMatrix;
        }
        //-------------------------------------------------------------------------------
        private double CalculatePhase( double x, double y ) {
            double phase;

            double multiplier = 0.1;

            double centreX1 = this.interferogramInfo.Width / 2;
            double centreY1 = this.interferogramInfo.Height / 2;

            double xx = x - centreX1;
            double yy = y - centreY1;
            
            phase = multiplier * Math.Sqrt( xx * xx + yy * yy );
            return phase;
        }
        
        //Вычисление фазы в точке
        /*
        private double CalculatePhase( double x, double y ) {
            double phase;
                        
            double multiplier1 = 0.2;
            double multiplier2 = 0.2;
            double multiplier3 = 0.2;
            double multiplier4 = 0.2;


            double centreX1 = this.interferogramInfo.Width / 4;
            //double centreY1 = this.interferogramInfo.Height * 2;
            double centreY1 = 0;
            double correctedX1 = ( x - centreX1 ) - this.interferogramInfo.Width / 2;
            double correctedY1 = this.interferogramInfo.Height / 2 - ( y - centreY1 );

            double phase1 =
                multiplier1 *
                Math.Sqrt( correctedX1 * correctedX1 + correctedY1 * correctedY1 );
            
            
            double centreX2 = this.interferogramInfo.Width / 2;
            //double centreY2 = -this.interferogramInfo.Height * 2;
            double centreY2 = 0;
            double correctedX2 = ( x - centreX2 ) - this.interferogramInfo.Width / 2;
            double correctedY2 = this.interferogramInfo.Height / 2 - ( y - centreY2 );

            double phase2 =
                multiplier2 *
                Math.Sqrt( correctedX2 * correctedX2 + correctedY2 * correctedY2 );
                        
            double centreX3 = -this.interferogramInfo.Width / 3; //3 
            double centreY3 = -this.interferogramInfo.Height * 2; //2
                        
            
            //double centreX3 = -this.interferogramInfo.Width / 10;
            //double centreY3 = -this.interferogramInfo.Height * 4;
            

            //double centreY3 = 0;
            double correctedX3 = ( x - centreX3 ) - this.interferogramInfo.Width / 2;
            double correctedY3 = this.interferogramInfo.Height / 2 - ( y - centreY3 );

            double phase3 =
                multiplier3 *
                Math.Sqrt( correctedX3 * correctedX3 + correctedY3 * correctedY3 );

            
            double centreX4 = -this.interferogramInfo.Width / 3;
            //double centreY4 = -2 * this.interferogramInfo.Height;
            //double centreY4 = -this.interferogramInfo.Height * 3;
            double centreY4 = 0;
            double correctedX4 = ( x - centreX4 ) - this.interferogramInfo.Width / 2;
            double correctedY4 = this.interferogramInfo.Height / 2 - ( y - centreY4 );

            double phase4 =
                multiplier4 *
                Math.Sqrt( correctedX4 * correctedX4 + correctedY4 * correctedY4 );
                        
                        
            //phase2 = 0;
            //phase3 = 0;
            //phase4 = 0;
            
            double phaseOffset = 0;
            
            phase = Math.Abs(  phase1 - phase2 + phase3 - phase4 + phaseOffset ) % ( 2 * Math.PI ); 
            //phase = Math.Abs( phase1 + phase2 - phase3 + phase4 + phaseOffset ) % ( 2 * Math.PI );


            //return ( phase1 - phase2 + phaseOffset ) % ( 2 * Math.PI );

            //return ( phase1 + phase2 + phase3 + phaseOffset ) % ( 2 * Math.PI );
            
            //return Math.Abs( ( phase1 - phase2 ) - ( phase3 + phase4 ) + phaseOffset ) % ( 2 * Math.PI );
            //return Math.Abs( ( phase1 - phase3 ) - ( phase2 - phase4 ) + phaseOffset ) % ( 2 * Math.PI );
            
            //return Math.Abs(  phase1 - phase2 + phase3 + phaseOffset ) % ( 2 * Math.PI );
            //return Math.Abs( phase1 - phase2 - phase3 + phaseOffset ) % ( 2 * Math.PI );

            return phase;
        }
        */
        //-------------------------------------------------------------------------------
        //Вычисление интенсивности в точке
        private double CalculateIntensity(
            double x,
            double y,
            double phaseShift   //Фазовый сдвиг
        ) {
            double phase = this.CalculatePhase( x, y ) + phaseShift;
            double noise =
                ( this.randomNumberGenerator.GetNextDouble() - 0.5 ) * 2 *
                this.interferogramInfo.MaxNoise;
            double intensity =
                this.interferogramInfo.MeanIntensity +
                this.interferogramInfo.MeanIntensity *
                this.interferogramInfo.IntensityModulation *
                Math.Cos( phase ) +
                noise;
            intensity = this.GetCorrectedIntensity( intensity );
            return intensity;
        }
        //-------------------------------------------------------------------------------
    }
}
