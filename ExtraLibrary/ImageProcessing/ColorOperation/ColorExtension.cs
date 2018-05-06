using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Drawing;

namespace ExtraLibrary.ImageProcessing
{
    public class ColorWrapper {
        //-----------------------------------------------------------------------------
        //Интенсивность серого
        public static int GetGrayIntensity( System.Windows.Media.Color color ) {
            int grayIntensity = ( color.R + color.G + color.B ) / 3;
            return grayIntensity;
        }
        //-----------------------------------------------------------------------------
        //Интенсивность серого
        public static int GetGrayIntensity( System.Drawing.Color color ) {
            int grayIntensity = ( color.R + color.G + color.B ) / 3;
            return grayIntensity;
        }
        //-----------------------------------------------------------------------------
    }
}
