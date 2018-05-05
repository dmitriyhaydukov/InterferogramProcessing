using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

using ExtraLibrary.Imaging;

namespace Interferometry.DeviceControllers {
    public class CameraDeviceController {
        //-------------------------------------------------------------------------------------------------
        private ImageGetter imageGetter;
        private BitmapSource receivedImage;     //Полученное изображение

        //-------------------------------------------------------------------------------------------------
        public event ImageReceivedEventHandler ImageReceived;
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        public CameraDeviceController() {
            this.imageGetter = new ImageGetter();
            imageGetter.imageReceived += new ImageReceived( this.ImageGetter_ImageReceived );
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //Обработка плучения изображения
        private void ImageGetter_ImageReceived( Image newImage ) {
            Bitmap newBitmap = newImage as Bitmap;
            BitmapSource bitmapSource = ImagesConverter.CreateBitmapSourceFromBitmap( newBitmap );
            this.receivedImage = new WriteableBitmap( bitmapSource );
            ImageReceiveEventArgs imageReceivedEventArgs = new ImageReceiveEventArgs( this.receivedImage );
            this.RaiseImageReceivedEvent( imageReceivedEventArgs );
        }
        //-------------------------------------------------------------------------------------------------
        private void RaiseImageReceivedEvent( ImageReceiveEventArgs imageReceivedEventArgs ) {
            if ( this.ImageReceived != null ) {
                this.ImageReceived( imageReceivedEventArgs );
            }
        }
        //-------------------------------------------------------------------------------------------------
        //Получить изображение
        public void GetImage() {
            this.imageGetter.getImage();
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
    }
}
