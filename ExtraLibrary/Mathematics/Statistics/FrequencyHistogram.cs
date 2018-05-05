using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Arraying.ArrayOperation;

namespace ExtraLibrary.Mathematics.Statistics {
    //Частотная гистограмма
    public class FrequencyHistogram {
        int intervalsCount; //Количество интервалов
        double[] values;    //Значения

        Interval<double>[] intervals = null;    //Интервалы
        double[] frequencyValues = null;        //Значения частоты

        //----------------------------------------------------------------------------------------------------------
        //Конструктор
        public FrequencyHistogram(double[] values, int intervalsCount) {
            this.intervalsCount = intervalsCount;
            this.values = values;
        }
        //----------------------------------------------------------------------------------------------------------
        //Рассчитать
        public void Calculate() {
            double minValue = this.values.Min();
            double maxValue = this.values.Max();
            
            this.intervals = this.GetIntervals( minValue, maxValue, this.intervalsCount );
            this.frequencyValues = this.GetFrequencyValues( this.values, this.intervals );
        }
        //----------------------------------------------------------------------------------------------------------
        //Интервалы
        public Interval<double>[] Intervals {
            get {
                return this.intervals;
            }
        }
        //----------------------------------------------------------------------------------------------------------
        //Значения частот
        public double[] FrequencyValues {
            get {
                return this.frequencyValues;
            }
        }
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //Разбить на интервалы
        private Interval<double>[] GetIntervals(double minValue, double maxValue, int intervalsCount) {
            double range = maxValue - minValue;
            double intervalLength = range / this.intervalsCount;

            Interval<double>[] intervals = new Interval<double>[ intervalsCount ];

            double startValue = minValue;
            double finishValue = startValue + intervalLength;

            for ( int index = 0; index < intervalsCount - 1; index++ ) {
                Interval<double> interval = new Interval<double>( startValue, finishValue );
                intervals[ index ] = interval;
                startValue = finishValue;
                finishValue += intervalLength;
            }
            
            startValue = maxValue - intervalLength;
            finishValue = maxValue;
            intervals[ intervalsCount - 1 ] = new Interval<double>( startValue, finishValue );

            return intervals;
        }
        //----------------------------------------------------------------------------------------------------------
        //Значения частот
        private double[] GetFrequencyValues(double[] values, Interval<double>[] intervals) {
            double[] frequencyValues = new double[ intervals.Length ];
            for ( int index = 0; index < values.Length; index++ ) {
                double value = values[ index ];
                int intervalIndex = this.GetIntervalIndex( value, intervals );
                frequencyValues[ intervalIndex ]++;
            }
            return frequencyValues;
        }
        //----------------------------------------------------------------------------------------------------------
        //Номер интервала, соответствующий данному значению
        private int GetIntervalIndex( double value, Interval<double>[] intervals ) {
            for ( int index = 0; index < intervals.Length; index++ ) {
                Interval<double> interval = intervals[ index ];
                if ( interval.Contains( value ) ) {
                    return index;
                }
            }
            return -1;
        }
        //----------------------------------------------------------------------------------------------------------
        //Номер интервала c максимальным значением частоты
        public int GetMaxFreqencyIntervalIndex() {
            int maxFreqencyIntervalIndex = ArrayOperator.GetMaxValueIndex( this.frequencyValues );
            return maxFreqencyIntervalIndex;
        }
        //----------------------------------------------------------------------------------------------------------
        //Индексы интервалов с наибольшими значениями частот
        public int[] GetMaxFrequencyIntervalsIndecies( int count ) {
            int[] intervalsIndecies = new int[count];
            Dictionary<int, double> frequenciesDictionary = ArrayOperator.GetDictionary( this.frequencyValues );
            int index = 0;
            while ( index < count ) {
                double maxFrequencyValue = 0;
                int maxFrequencyIndex = 0;
                foreach ( KeyValuePair<int, double> keyValuePair in frequenciesDictionary ) {
                    double value = keyValuePair.Value;
                    int frequencyIndex = keyValuePair.Key;
                    if ( maxFrequencyValue < value ) {
                        maxFrequencyValue = value;
                        maxFrequencyIndex = frequencyIndex;
                    }
                }
                intervalsIndecies[ index ] = maxFrequencyIndex;
                frequenciesDictionary.Remove( maxFrequencyIndex );
                index++;
            }
            return intervalsIndecies;
        }
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
    }
}
