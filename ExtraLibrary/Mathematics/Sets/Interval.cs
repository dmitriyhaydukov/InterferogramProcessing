using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Mathematics.Sets {
    //Интервал значений
    public struct Interval<TYPE> where TYPE : IComparable {
        TYPE minValue;    //Минимальное значение
        TYPE maxValue;    //Максимальное значение
        //------------------------------------------------------------------------------------
        public Interval( TYPE minValue, TYPE maxValue ) {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
        //------------------------------------------------------------------------------------
        //Принадлежность значеия интервалу
        public bool Contains(TYPE value) {
            bool isContain = 
                ( 0 <= value.CompareTo( this.minValue ) ) &&
                ( value.CompareTo( this.maxValue ) <= 0 );
            return isContain;
        }
        //------------------------------------------------------------------------------------
        //Минимальное значение
        public TYPE MinValue {
            get {
                return this.minValue;
            }
        }
        //------------------------------------------------------------------------------------
        //Максимальное значение
        public TYPE MaxValue {
            get {
                return this.maxValue;
            }
        }
        //------------------------------------------------------------------------------------
    }
}
