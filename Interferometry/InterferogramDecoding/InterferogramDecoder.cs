using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;

namespace Interferometry.InterferogramDecoding {
    public abstract class InterferogramDecoder {
        //----------------------------------------------------------------------------------
        //Значение фазы по умолчанию
        public double DefaultPhaseValue {
            get;
            set;
        }
        //----------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------
        public InterferogramDecoder() {
            //this.DefaultPhaseValue = 0;
            this.DefaultPhaseValue = 2 * Math.PI;
        }
        //----------------------------------------------------------------------------------
        //Значения интенсивностей в конкретоной точке интерферограмм
        protected double[] CreateIntensitiesAtPoint(
            int x,  //Координата X
            int y,  //Координата Y
            params RealMatrix[] interferograms
        ) {
            double[] intensities =
                MatricesManager.GeValuesFromMatrices( y, x, interferograms );
            return intensities;
        }
        //----------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------
    }
}
