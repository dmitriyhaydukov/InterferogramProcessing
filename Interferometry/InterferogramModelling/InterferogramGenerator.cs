using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using ExtraLibrary.Arraying.ArrayCreation;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Mathematics.Matrices;

namespace Interferometry.InterferogramCreation {
    //Генератор интерферограмм
    public class InterferogramGenerator {
        InterferogramCreator interferogramCreator;
        //----------------------------------------------------------------------------------------
        public InterferogramGenerator( InterferogramCreator interferogramCreator ) {
            this.interferogramCreator = interferogramCreator;
        }
        //----------------------------------------------------------------------------------------
        //Генерация интерферограмм
        public RealMatrix[] GenerateInterferograms( params double[] phaseShifts ) {
            int interferogramsCount = phaseShifts.Length;
            RealMatrix[] interferograms = new RealMatrix[ interferogramsCount ];
            for ( int index = 0; index < interferogramsCount; index++ ) {
                double phaseShift = phaseShifts[ index ];
                RealMatrix interferogram =
                    this.interferogramCreator.CreateInterferogram( phaseShift );
                interferograms[ index ] = interferogram;
            }
            return interferograms;
        }
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
    }
}
