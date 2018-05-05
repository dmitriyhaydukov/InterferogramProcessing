using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Geometry2D;

namespace Interferometry.InterferogramDecoding {
    public class InterferogramDecodingResult {
        //-------------------------------------------------------------------------------------
        public RealMatrix ResultMatrix {
            get;
            private set;
        }
        //-------------------------------------------------------------------------------------
        public Point2D[] EllipsePoints {
            get;
            set;
        }
        //-------------------------------------------------------------------------------------
        public InterferogramDecodingResult( RealMatrix resultMatrix ) {
            this.ResultMatrix = resultMatrix;    
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
    }
}
