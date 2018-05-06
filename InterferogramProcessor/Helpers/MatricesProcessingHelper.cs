#region namespeces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;

using ExtraControls;
using ExtraMVVM;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.OS;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Arraying.ArrayCreation;
using ExtraLibrary.Converting;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Progressions;
using ExtraLibrary.Randomness;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.Geometry2D;

using Interferometry.InterferogramProcessing;
using Interferometry.InterferogramCreation;
using Interferometry.InterferogramDecoding;
using Interferometry.DeviceControllers;
using Interferometry.Helpers;
#endregion


namespace InterferogramProcessing {
    public class MatricesProcessingHelper {
        //------------------------------------------------------------------------------------------------------
        public static RealMatrix GetGrayScaledMatrixFromMinusPI( RealMatrix matrix ) {
            //RealMatrix scaledMatrix =
            //    InterferometryHelper.TrnsformPhaseMatrixToGrayScaleMatrix( matrix );

            Interval<double> startInterval = new Interval<double>( -Math.PI, Math.PI );
            Interval<double> finishInterval = new Interval<double>( 0, 255 );
            RealIntervalTransform intervalTransform = new RealIntervalTransform( startInterval, finishInterval );
            RealMatrix scaledMatrix =
                RealMatrixValuesTransform.TransformMatrixValues( matrix, intervalTransform );
            
            return scaledMatrix;
        }
        //------------------------------------------------------------------------------------------------------
        //Приведенная к диапазону 0..255 матрица
        public static RealMatrix GetGrayScaledMatrix( RealMatrix matrix, BitMask2D bitMask ) {
            double prickedValue = 0;
            RealMatrix scaledMatrix =
                InterferometryHelper.TrnsformPhaseMatrixToGrayScaleMatrix( matrix, bitMask, prickedValue );
            return scaledMatrix;
        }
        //------------------------------------------------------------------------------------------------------
        //Матрица с диапазоном значений 0..255
        public static RealMatrix GetExtendedRangeMatrix( RealMatrix matrix ) {
            double minValue = matrix.GetMinValue();
            double maxValue = matrix.GetMaxValue();

            Interval<double> startInterval = new Interval<double>( minValue, maxValue );
            Interval<double> finishInterval = new Interval<double>( 0, 255 );
            RealIntervalTransform intervalTransform = 
                new RealIntervalTransform( startInterval, finishInterval );

            RealMatrix newMatrix = 
                RealMatrixValuesTransform.TransformMatrixValues( matrix, intervalTransform );
            return newMatrix;
        }
        //------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------
    }
}
