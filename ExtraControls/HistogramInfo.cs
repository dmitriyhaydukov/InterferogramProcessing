using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Sets;

namespace ExtraControls {
    public class HistogramInfo {
        //----------------------------------------------------------------------------------------------------
        public HistogramInfo( Interval<double>[] intervals, double[] frequencyValues, string histogramName ) {
            this.Intervals = intervals;
            this.FrequencyValues = frequencyValues;
            this.HistogramName = histogramName;
        }
        //----------------------------------------------------------------------------------------------------
        public Interval<double>[] Intervals {
            get;
            set;
        }
        //----------------------------------------------------------------------------------------------------
        public double[] FrequencyValues {
            get;
            set;
        }
        //----------------------------------------------------------------------------------------------------
        public string HistogramName {
            get;
            set;
        }
        //----------------------------------------------------------------------------------------------------
    }
}
