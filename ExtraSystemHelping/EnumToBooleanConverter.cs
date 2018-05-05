#region namespeces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Data;

#endregion

namespace ExtraSystemHelping {
    public class EnumToBooleanConverter : IValueConverter {
        //---------------------------------------------------------------------------------------------------------
        public object Convert(
            object value, Type targetType, object parameter, System.Globalization.CultureInfo culture
        ) {
            return value.Equals( parameter );
        }
        //---------------------------------------------------------------------------------------------------------
        public object ConvertBack(
            object value, Type targetType, object parameter, System.Globalization.CultureInfo culture
        ) {
            return value.Equals( true ) ? parameter : Binding.DoNothing;
        }
        //---------------------------------------------------------------------------------------------------------
    }
}