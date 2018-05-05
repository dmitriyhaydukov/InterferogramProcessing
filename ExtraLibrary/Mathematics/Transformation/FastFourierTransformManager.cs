using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayCreation;

namespace ExtraLibrary.Mathematics.Transformation {
    public class FastFourierTransformManager {
        //---------------------------------------------------------------------------------------------------------
        //Преобразование Фурье для массива матриц
        public static ComplexMatrix[] GetCenteredFourierTransforms2D( RealMatrix[] matrices ) {
            FastFourierTransform fastFourierTransform = new FastFourierTransform();
            ComplexMatrix[] fourierTransforms2D = new ComplexMatrix[ matrices.Length ];
            for ( int index = 0; index < matrices.Length; index++ ) {
                RealMatrix matrix = matrices[ index ];
                ComplexMatrix fourierTransform2D = fastFourierTransform.GetCenteredFourierTransform2D( matrix );
                fourierTransforms2D[ index ] = fourierTransform2D;    
            }
            return fourierTransforms2D;
        }
        //----------------------------------------------------------------------------------------------------------
        //Обратное преобразование Фурье для массива матриц
        public static ComplexMatrix[] GetInverseFourierTransforms2D( ComplexMatrix[] fourierTransforms2D ) {
            FastFourierTransform fastFourierTransform = new FastFourierTransform();
            ComplexMatrix[] inverseFourierTransforms2D = new ComplexMatrix[ fourierTransforms2D.Length ];
            for ( int index = 0; index < fourierTransforms2D.Length; index++ ) {
                ComplexMatrix fourierTransform2D = fourierTransforms2D[ index ];
                ComplexMatrix inverseFourierTransform2D = 
                    fastFourierTransform.GetInverseFourierTransform2D( fourierTransform2D );
                inverseFourierTransforms2D[ index ] = inverseFourierTransform2D;
            }
            return inverseFourierTransforms2D;
        }
        //----------------------------------------------------------------------------------------------------------
        //Двумерное преобразование Фурье для массива матриц
        public static RealMatrix[] GetFourierTransformSpectrums2D( ComplexMatrix[] fourierTransforms2D ) {
            FastFourierTransform fastFourierTransform = new FastFourierTransform();
            RealMatrix[] fourierTransformSpectrums2D = new RealMatrix[ fourierTransforms2D.Length ];
            for ( int index = 0; index < fourierTransforms2D.Length; index++ ) {
                ComplexMatrix fourierTransform2D = fourierTransforms2D[ index ];
                RealMatrix fourierTransformSpectrum2D =
                    fastFourierTransform.GetFourierTransformSpectrum2D( fourierTransform2D );
                fourierTransformSpectrums2D[ index ] = fourierTransformSpectrum2D;
            }
            return fourierTransformSpectrums2D;
        }
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
    }
}
