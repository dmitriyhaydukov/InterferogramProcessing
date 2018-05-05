using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace ExtraLibrary.Imaging {
    public class BitmapImageCreator {
        //------------------------------------------------------------------------------------------
        //Создание BitmapImage из файла
        public static BitmapImage CreateBitmapImageFromFile( string fileName ) {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri( fileName );
            bitmapImage.EndInit();
            return bitmapImage;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
    }
}
