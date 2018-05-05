using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ExtraLibrary.OS {
    public class OS {
        //------------------------------------------------------------------------------------------
        //DpiX
        public static double GetSystemDpiX() {
            Graphics graphics = Graphics.FromHwnd( IntPtr.Zero );
            double dpiX = graphics.DpiX;
            graphics.Dispose();
            return dpiX;
        }
        //------------------------------------------------------------------------------------------
        //DpiY
        public static double GetSystemDpiY() {
            Graphics graphics = Graphics.FromHwnd( IntPtr.Zero );
            double dpiY = graphics.DpiY;
            graphics.Dispose();
            return dpiY;
        }
        //------------------------------------------------------------------------------------------
        public static double SystemDpiX {
            get {
                return OS.GetSystemDpiX();
            }
        }
        //------------------------------------------------------------------------------------------
        public static double SystemDpiY {
            get {
                return OS.GetSystemDpiY();
            }
        }
        //------------------------------------------------------------------------------------------
        public static int IntegerSystemDpiX {
            get {
                return Convert.ToInt32( OS.SystemDpiX );
            }
        }
        //------------------------------------------------------------------------------------------
        public static int IntegerSystemDpiY {
            get {
                return Convert.ToInt32( OS.SystemDpiY );
            }
        }
        //------------------------------------------------------------------------------------------
    }
}
