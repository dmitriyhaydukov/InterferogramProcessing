using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ExtraLibrary.Mathematics.Matrices;

namespace ExtraLibrary.ImageProcessing {
    public class ExtraImageInfo {
        public WriteableBitmap Image {
            get;
            set;
        }

        public RealMatrix RedMatrix {
            get;
            set;
        }

        public RealMatrix GreenMatrix {
            get;
            set;
        }

        public RealMatrix BlueRedMatrix {
            get;
            set;
        }
    }
}
