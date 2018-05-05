using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Randomness;

namespace Interferometry.InterferogramCreation {
    //Формирователь интерферограмм
    public abstract class InterferogramCreator {
        protected InterferogramInfo interferogramInfo;
        protected RandomNumberGenerator randomNumberGenerator;
        //--------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------
        public InterferogramCreator( InterferogramInfo interferogramInfo ) {
            this.interferogramInfo = interferogramInfo;
            this.randomNumberGenerator = new RandomNumberGenerator();
        }
        //--------------------------------------------------------------------------------------
        public abstract RealMatrix CreateInterferogram( double phaseShift );
        //--------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------
        //Коррекция интенсивности
        protected double GetCorrectedIntensity( double intensity ) {
            if ( intensity < 0 ) {
                return 0;
            }
            if ( this.interferogramInfo.MaxIntensity < intensity ) {
                return this.interferogramInfo.MaxIntensity;
            }
            return intensity;
        }
        //--------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------
    }
}
