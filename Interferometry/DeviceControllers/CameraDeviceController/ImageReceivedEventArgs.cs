using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Interferometry.DeviceControllers {
    public class ImageReceiveEventArgs : EventArgs {
        //---------------------------------------------------------------------------------------
        public BitmapSource ReceivedImage {
            get;
            set;
        }
        //---------------------------------------------------------------------------------------
        public ImageReceiveEventArgs( BitmapSource image ) {
            this.ReceivedImage = image;
        }
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
    }
}
