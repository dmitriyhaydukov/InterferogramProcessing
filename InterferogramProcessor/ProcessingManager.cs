using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

using ExtraControls;
using ExtraMVVM;
using ExtraLibrary.Imaging;
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
using ExtraLibrary.Imaging.ImageProcessing;
using ExtraLibrary.Geometry2D;

using Interferometry.InterferogramProcessing;
using Interferometry.InterferogramCreation;
using Interferometry.InterferogramDecoding;
using Interferometry.DeviceControllers;
using Interferometry.Helpers;

using UserInterfaceHelping;
using ZedGraph;

namespace InterferogramProcessing {
    public class ProcessingManager {
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public double[] LastGammaValues {
            get;
            private set;
        }
        //--------------------------------------------------------------------------------------------------
        public double[] LastGammaTargetFunctionValues {
            get;
            private set;
        }
        //--------------------------------------------------------------------------------------------------
        //Автоматическая гамма-коррекция
        public RealMatrix[] GammaCorrection( RealMatrix[] interferograms, BitMask2D bitMask ) {
            InterferogramGammaCorrector interferogramGammaCorrector = new InterferogramGammaCorrector();
            
            RealMatrix[] gammaCorrectedInterferograms =
                interferogramGammaCorrector.GetGammaCorrectedInterferograms( interferograms, bitMask );
                
            this.LastGammaValues = interferogramGammaCorrector.GammaValues;
            this.LastGammaTargetFunctionValues = interferogramGammaCorrector.TargetFunctionValues;
            
            return gammaCorrectedInterferograms;
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}
