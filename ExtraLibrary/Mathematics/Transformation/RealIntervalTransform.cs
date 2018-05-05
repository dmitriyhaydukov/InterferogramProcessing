using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Sets;

namespace ExtraLibrary.Mathematics.Transformation {
    //Приведение значений одного интервала к другому
    public class RealIntervalTransform {

        //Исходный интервал
        Interval<double> startInterval;           
        //Разность между максимальным и минимальным значением исходного интервала
        double startDifference;

        //Конечный интервал
        Interval<double> finishInterval;

        //Разность между максимальным и минимальным значением конечного интервала
        double finishDifference;
          
        //-------------------------------------------------------------------------------------
        public RealIntervalTransform(
            Interval<double> startInterval,
            Interval<double> finishInterval
        ) {
            this.startInterval = startInterval;
            this.startDifference =
                this.startInterval.MaxValue - this.startInterval.MinValue;

            this.finishInterval = finishInterval;
            this.finishDifference=
                this.finishInterval.MaxValue - this.finishInterval.MinValue;
        }
        //--------------------------------------------------------------------------------------
        //Выполнить приведение заданного значения value
        public double TransformToFinishIntervalValue( double value ) {
            double result =
                ( value - this.startInterval.MinValue ) *
                this.finishDifference / this.startDifference +
                this.finishInterval.MinValue;
            return result;
        }
        //--------------------------------------------------------------------------------------

    }
}
