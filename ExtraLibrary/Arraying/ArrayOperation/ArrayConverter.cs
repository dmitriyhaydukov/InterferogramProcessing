using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraLibrary.Converting {
    public class ArrayConverter {
        //------------------------------------------------------------------------------
        //Конвертирование в массив строк
        public static string[] ToStringArray( double[] array ) {
            string[] arrayString = new string[ array.Length ];
            for ( int index = 0; index < array.Length; index++ ) {
                string valueString = array[ index ].ToString();
                arrayString[ index ] = valueString;
            }
            return arrayString;
        }
        //------------------------------------------------------------------------------
        //Конвертирование в массив строк
        public static string[] ToStringArray( int[] array ) {
            string[] arrayString = new string[ array.Length ];
            for ( int index = 0; index < array.Length; index++ ) {
                string valueString = array[ index ].ToString();
                arrayString[ index ] = valueString;
            }
            return arrayString;
        } 
        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
    }
}
