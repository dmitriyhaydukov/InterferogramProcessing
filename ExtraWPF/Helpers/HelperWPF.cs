using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ExtraLibrary.OS;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;

namespace ExtraWPF {
    public class ExtraHelperWPF {
        //------------------------------------------------------------------------------------------------------------
        //Регистрация свойства зависимости
        public static DependencyProperty RegisterDependencyProperty( DependencyPropertyInfo info ) {
            FrameworkPropertyMetadata metaData = new FrameworkPropertyMetadata();
            metaData.PropertyChangedCallback = info.PropertyChangedHandler;
            DependencyProperty dependencyProperty =
                DependencyProperty.Register( info.PropertyName, info.DataType, info.OwnerDataType, metaData );
            return dependencyProperty;
        }
        //------------------------------------------------------------------------------------------------------------
        //Сохранение изображения контрола в файл
        public static void SaveControlImageToPngFile(
            Visual baseElement,
            int imageWidth, int imageHeight,
            string pathToOutputFile
        ) {
            double dpiX = OS.SystemDpiX;
            double dpiY = OS.SystemDpiY;
            RenderTargetBitmap elementBitmap =
                new RenderTargetBitmap( imageWidth, imageHeight, dpiX, dpiY, PixelFormats.Default );
            
            DrawingVisual drawingVisual = new DrawingVisual();
            using ( DrawingContext drawingContext = drawingVisual.RenderOpen() ) {
                VisualBrush visualBrush = new VisualBrush( baseElement );
                drawingContext.DrawRectangle
                    ( visualBrush, null, new Rect( new Point( 0, 0 ), new Size( imageWidth, imageHeight ) ) );
            }
            
            elementBitmap.Render( drawingVisual );
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add( BitmapFrame.Create( elementBitmap ) );

            using ( FileStream imageFile = new FileStream( pathToOutputFile, FileMode.Create, FileAccess.Write ) ) {
                encoder.Save( imageFile );
                imageFile.Flush();
                imageFile.Close();
            }
        }
        //------------------------------------------------------------------------------------------------------------
        //Размер изображения в пикселях
        public static Size GetImageSourceSizeInPixels( ImageSource imageSource ) {
            double dpiX = OS.SystemDpiX;
            double dpiY = OS.SystemDpiY;

            int pixelWidth = Convert.ToInt32( imageSource.Width / 96.0 * dpiX );
            int pixelHeight = Convert.ToInt32( imageSource.Height / 96.0 * dpiY );
            
            Size size = new Size( pixelWidth, pixelHeight );
            return size;
        }
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
    }
}
