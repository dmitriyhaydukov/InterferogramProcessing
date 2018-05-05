using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ExtraLibrary.Mathematics.Matrices;

namespace ExtraControls {
    public class ImageInfo {
        //--------------------------------------------------------------------------------
        public string ImageName {
            get;
            set;
        }
        //--------------------------------------------------------------------------------
        public BitmapSource ImageSource {
            get;
            set;
        }
        //--------------------------------------------------------------------------------
        public bool Checked {
            get;
            set;
        }
        //--------------------------------------------------------------------------------
        public RealMatrix Matrix {
            get;
            set;
        }
        //--------------------------------------------------------------------------------
        public ImageInfo( string imageName, BitmapSource imageSource, RealMatrix matrix ) {
            this.ImageName = imageName;
            this.ImageSource = imageSource;
            this.Checked = false;
            this.Matrix = matrix;
        }
        //--------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------
    }
}
