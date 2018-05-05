using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace InterferogramProcessing.Extensions {
    public static class EnumExtensions {
        //------------------------------------------------------------------------------------------------------
        public static string GetEnumDescription( Enum value ) {
            FieldInfo fi = value.GetType().GetField( value.ToString() );

            DescriptionAttribute[] attributes =
                ( DescriptionAttribute[] )fi.GetCustomAttributes( typeof( DescriptionAttribute ), false );

            if ( attributes != null && attributes.Length > 0 )
                return attributes[ 0 ].Description;
            else
                return value.ToString();
        }
        //------------------------------------------------------------------------------------------------------
        public static string[] GetEnumDescriptions(Type enumType) {
            Array enumValues = enumType.GetEnumValues();
            string[] enumDescriptions = new string[ enumValues.Length ];
            for ( int index = 0; index < enumValues.Length; index++ ) {

            }
            return enumDescriptions;
        }
        //------------------------------------------------------------------------------------------------------
        public static IEnumerable<T> EnumToList<T>() {
            Type enumType = typeof( T );

            if ( enumType.BaseType != typeof( Enum ) )
                throw new ArgumentException( "T must be of type System.Enum" );

            Array enumValArray = Enum.GetValues( enumType );
            List<T> enumValList = new List<T>( enumValArray.Length );

            foreach ( int val in enumValArray ) {
                enumValList.Add( ( T )Enum.Parse( enumType, val.ToString() ) );
            }

            return enumValList;
        }
        //------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------
    }
}
