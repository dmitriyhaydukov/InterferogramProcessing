using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.Mathematics.Transformation {
    public class LogTransform {
        //----------------------------------------------------------------------------------------------
        public double Coefficient {
            get;
            set;
        }
        //----------------------------------------------------------------------------------------------
        public LogTransform( double coefficient ) {
            this.Coefficient = coefficient;
        }
        //----------------------------------------------------------------------------------------------
        public double GetLogTransform( double value ) {
            double newValue = this.Coefficient * Math.Log( 1 + value );
            return newValue;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
}
