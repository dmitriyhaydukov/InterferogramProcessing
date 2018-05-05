using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Geometry3D;
using ExtraLibrary.Mathematics.Vectors;

namespace ExtraControls {
    public class HelixGridLinesInfo {
        //------------------------------------------------------------------------------
        public double Width {
            get;
            set;
        }
        //------------------------------------------------------------------------------
        public double Length {
            get;
            set;
        }
        //------------------------------------------------------------------------------
        public double MinorDistance {
            get;
            set;
        }
        //------------------------------------------------------------------------------
        public double MajorDistance {
            get;
            set;
        }
        //------------------------------------------------------------------------------
        public double Thickness {
            get;
            set;
        }
        //------------------------------------------------------------------------------
        public Point3D Center {
            get;
            set;
        }
        //------------------------------------------------------------------------------
        public RealVector LengthDirection {
            get;
            set;
        }
        //------------------------------------------------------------------------------
        public RealVector Normal {
            get;
            set;
        }
        //------------------------------------------------------------------------------
    }
}
