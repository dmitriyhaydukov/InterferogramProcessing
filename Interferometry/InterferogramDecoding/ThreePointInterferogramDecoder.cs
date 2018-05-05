using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;

namespace Interferometry.InterferogramDecoding {
    //-------------------------------------------------------------------------------------------
    //Трехточечный расшифровщик
    public class ThreePointInterferogramDecoder : InterferogramDecoder {
        //---------------------------------------------------------------------------------------
        //Расшифровать
        public double Decode( double[] intensities, double[] phaseShifts ) {
            bool validateArraySizes =
                this.ValidateArraySize( intensities ) &&
                this.ValidateArraySize( phaseShifts );

            if ( !validateArraySizes ) {
                throw new InterferogramDecodingException();
            }

            double componentSin = this.GetDecodingFormulaNumerator( intensities, phaseShifts );
            double componentCos = this.GetDecodingFormulaDenominator( intensities, phaseShifts );

            double result = this.CalculatePhase( componentSin, componentCos );
            return result;
        }
        //---------------------------------------------------------------------------------------------------
        public double CalculatePhase( double componentSin, double componentCos ) {
            
            double result = Math.PI - Math.Atan2( componentCos, componentSin );
            //double result = Math.Atan2( componentSin, componentCos );
            //double result = Math.Atan( componentSin / componentCos );
            
            return result;
        }
        //---------------------------------------------------------------------------------------------------
        //Расшифровка интерферограммы
        public InterferogramDecodingResult DecodeInterferogram(
            RealMatrix[] interferograms, double[] phaseShifts, BitMask2D bitMask
        ) {
            bool validateArraySizes =
                this.ValidateArraySize( interferograms ) &&
                this.ValidateArraySize( phaseShifts );

            if ( !validateArraySizes ) {
                throw new InterferogramDecodingException();
            }

            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;

            RealMatrix resultMatrix = new RealMatrix( height, width );

            for ( int x = 0; x < width; x++ ) {
                for ( int y = 0; y < height; y++ ) {
                    if ( bitMask[ y, x ] == true ) {
                        double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                        double phase = this.Decode( intensities, phaseShifts );
                        resultMatrix[ y, x ] = phase;
                    }
                    else {
                        resultMatrix[ y, x ] = this.DefaultPhaseValue;
                    }
                }
            }
            InterferogramDecodingResult decodingResult = new InterferogramDecodingResult( resultMatrix );
            return decodingResult;
        }
        //-----------------------------------------------------------------------------------------------------
        //Числитель формулы расшифровки
        public double GetDecodingFormulaNumerator( double[] intensities, double[] phaseShifts ) {
            double intensity1 = intensities[ 0 ];
            double intensity2 = intensities[ 1 ];
            double intensity3 = intensities[ 2 ];

            double phaseShift1 = phaseShifts[ 0 ];
            double phaseShift2 = phaseShifts[ 1 ];
            double phaseShift3 = phaseShifts[ 2 ];

            double numerator =
               ( intensity2 - intensity3 ) * Math.Sin( phaseShift1 ) +
               ( intensity3 - intensity1 ) * Math.Sin( phaseShift2 ) +
               ( intensity1 - intensity2 ) * Math.Sin( phaseShift3 );

            return numerator;
        }
        //----------------------------------------------------------------------------------------------------
        //Знаменатель формулы расшифровки
        public double GetDecodingFormulaDenominator( double[] intensities, double[] phaseShifts ) {
            double intensity1 = intensities[ 0 ];
            double intensity2 = intensities[ 1 ];
            double intensity3 = intensities[ 2 ];

            double phaseShift1 = phaseShifts[ 0 ];
            double phaseShift2 = phaseShifts[ 1 ];
            double phaseShift3 = phaseShifts[ 2 ];

            double denominator =
                ( intensity3 - intensity2 ) * Math.Cos( phaseShift1 ) +
                ( intensity1 - intensity3 ) * Math.Cos( phaseShift2 ) +
                ( intensity2 - intensity1 ) * Math.Cos( phaseShift3 );

            return denominator;
        }
        //----------------------------------------------------------------------------------------------------
        //Знаменатели формулы расшифровки для всех точек интерферограммы ( по маске )
        public double[] GetDecodingFormulaDenominators( 
            RealMatrix[] interferograms, double[] phaseShifts, BitMask2D bitMask
        ) {
            bool validateArraySizes =
                this.ValidateArraySize( interferograms ) &&
                this.ValidateArraySize( phaseShifts );

            if ( !validateArraySizes ) {
                throw new InterferogramDecodingException();
            }
            
            int width = interferograms[0].ColumnCount;
            int height = interferograms[0].RowCount;

            List<double> denominators = new List<double>();

            for ( int x = 0; x < width; x++ ) {
                for ( int y = 0; y < height; y++ ) {
                    if ( bitMask[ y, x ] ) {
                        double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                        double denominator = this.GetDecodingFormulaDenominator( intensities, phaseShifts );
                        denominators.Add( denominator );
                    }
                }
            }

            return denominators.ToArray();
        }
        //---------------------------------------------------------------------------------------------------
        //Числители формулы расшифровки для всех точек интерферограммы ( по маске )
        public double[] GetDecodingFormulaNumerators(
            RealMatrix[] interferograms, double[] phaseShifts, BitMask2D bitMask
        ) {
            bool validateArraySizes =
                this.ValidateArraySize( interferograms ) &&
                this.ValidateArraySize( phaseShifts );

            if ( !validateArraySizes ) {
                throw new InterferogramDecodingException();
            }

            int width = interferograms[ 0 ].ColumnCount;
            int height = interferograms[ 0 ].RowCount;

            List<double> numerators = new List<double>();
                        
            for ( int x = 0; x < width; x++ ) {
                for ( int y = 0; y < height; y++ ) {
                    if ( bitMask[ y, x ] ) {
                        double[] intensities = this.CreateIntensitiesAtPoint( x, y, interferograms );
                        double numerator = this.GetDecodingFormulaNumerator( intensities, phaseShifts );
                        numerators.Add( numerator );
                    }
                }
            }
            return numerators.ToArray();
        }
        //----------------------------------------------------------------------------------------
        //Проверка размера массива
        private bool ValidateArraySize(Array array) {
            int arraySize = array.Length;
            return arraySize == 3;
        }
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
    }
}
