using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ExtraLibrary.ImageProcessing
{
    public class ImagesConverter {
        //-------------------------------------------------------------------------------------------------------
        [DllImport( "gdi32.dll" )]
        private static extern bool DeleteObject( IntPtr hObject );
        //-------------------------------------------------------------------------------------------------------
        //Конвертирование Bitmap в BitmapSource
        public static BitmapSource CreateBitmapSourceFromBitmap( Bitmap bitmap ) {
            if ( bitmap == null ) {
                throw new ArgumentNullException( "bitmap" );
            }

            lock ( bitmap ) {
                IntPtr hBitmap = bitmap.GetHbitmap();

                try {
                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                        ( hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions() );
                    return bitmapSource;
                }
                finally {
                    DeleteObject( hBitmap );
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
    }
}
