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
    public class ImagesProcessingHelper {
        //-----------------------------------------------------------------------------------------------------
        //Интенсивности серого строки
        public static double[] GetRowGrayScaleValues( WriteableBitmap bitmap, int row ) {
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
            if ( wrapper.IsFormatGrayScale ) {
                double[] grayScaleValues = wrapper.GetRowGrayValues( row );
                return grayScaleValues;
            }
            else {
                Color[] rowColors = wrapper.GetRowColors( row );
                double[] grayScaleValues = new double[ rowColors.Length ];
                for ( int index = 0; index < rowColors.Length; index++ ) {
                    Color color = rowColors[ index ];
                    double grayIntensity = ColorWrapper.GetGrayIntensity( color );
                    grayScaleValues[ index ] = grayIntensity;
                }
                return grayScaleValues;
            }
        }
        //-----------------------------------------------------------------------------------------------------
        //Интенсивности красного канала
        public static double[] GetRowRedValues( WriteableBitmap bitmap, int row ) {
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
            if ( wrapper.IsFormatGrayScale ) {
                double[] grayScaleValues = wrapper.GetRowGrayValues( row );
                return grayScaleValues;
            }
            else {
                Color[] rowColors = wrapper.GetRowColors( row );
                double[] redValues = new double[ rowColors.Length ];
                for ( int index = 0; index < rowColors.Length; index++ ) {
                    Color color = rowColors[ index ];
                    double redValue = color.R;
                    redValues[ index ] = redValue;
                }
                return redValues;
            }
            
        }
        //-----------------------------------------------------------------------------------------------------
        //Интенсивности зеленого канала
        public static double[] GetRowGreenValues( WriteableBitmap bitmap, int row ) {
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
            if ( wrapper.IsFormatGrayScale ) {
                double[] grayScaleValues = wrapper.GetRowGrayValues( row );
                return grayScaleValues;
            }
            else {
                Color[] rowColors = wrapper.GetRowColors( row );
                double[] greenValues = new double[ rowColors.Length ];
                for ( int index = 0; index < rowColors.Length; index++ ) {
                    Color color = rowColors[ index ];
                    double greenValue = color.G;
                    greenValues[ index ] = greenValue;
                }
                return greenValues;
            }
        }
        //-----------------------------------------------------------------------------------------------------
        //Интенсивности синего канала
        public static double[] GetRowBlueValues( WriteableBitmap bitmap, int row ) {
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
            if ( wrapper.IsFormatGrayScale ) {
                double[] grayScaleValues = wrapper.GetRowGrayValues( row );
                return grayScaleValues;
            }
            else {
                Color[] rowColors = wrapper.GetRowColors( row );
                double[] blueValues = new double[ rowColors.Length ];
                for ( int index = 0; index < rowColors.Length; index++ ) {
                    Color color = rowColors[ index ];
                    double blueValue = color.B;
                    blueValues[ index ] = blueValue;
                }
                return blueValues;
            }
        }
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
    }
}
