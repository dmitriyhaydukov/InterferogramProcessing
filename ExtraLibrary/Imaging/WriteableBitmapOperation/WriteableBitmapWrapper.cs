using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using media = System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using drawing = System.Drawing;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Geometry2D;

namespace ExtraLibrary.Imaging {
    public abstract class WriteableBitmapWrapper {
        protected WriteableBitmap writeableBitmap;
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        //Количество байтов на строку изображения
        protected int GetStride() {
            int stride = this.writeableBitmap.PixelWidth * this.BytesPerPixel;
            return stride;
        }
        //----------------------------------------------------------------------------------------
        //Количество байтов на пиксел изображения
        protected int BytesPerPixel {
            get {
                return this.writeableBitmap.Format.BitsPerPixel / 8;
            }
        }
        //----------------------------------------------------------------------------------------
        //Смещение для пикселя (x, y) в массиве байтов
        protected int GetOffset( int x, int y ) {
            int offset = ( y * this.writeableBitmap.PixelWidth + x ) * this.BytesPerPixel;
            return offset;
        }
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        //Размер в байтах
        public int SizeInBytes {
            get {
                int sizeInBytes =
                    this.Image.PixelWidth *
                    this.Image.PixelHeight *
                    this.BytesPerPixel;
                return sizeInBytes;
            }
        }
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------
        //Изображение
        public WriteableBitmap Image {
            get {
                return this.writeableBitmap;        
            }
        }
        //----------------------------------------------------------------------------------------
        //Сохранение в файл
        private void SaveToFile( string fileName, BitmapEncoder encoder ) {
            if ( fileName == string.Empty ) {
                throw new Exception();
            }
            FileStream fileStream = new FileStream( fileName, FileMode.Create );
            BitmapFrame frame = BitmapFrame.Create( this.Image );
            encoder.Frames.Add( frame );
            encoder.Save( fileStream );
            fileStream.Close();
        }
        //-----------------------------------------------------------------------------------------------
        //Сохранение в файл Png
        public void SaveToPngFile( string fileName ) {
            BitmapEncoder encoder = new PngBitmapEncoder();
            this.SaveToFile( fileName, encoder );
        }
        //-----------------------------------------------------------------------------------------------
        //Сохранение в файл Bmp
        public void SaveToBmpFile( string fileName ) {
            BitmapEncoder encoder = new BmpBitmapEncoder();
            this.SaveToFile( fileName, encoder );
        }
        //-----------------------------------------------------------------------------------------------
        //Сохранение в файл Jpeg
        public void SaveToJpegFile( string fileName ) {
            BitmapEncoder encoder = new JpegBitmapEncoder();
            this.SaveToFile( fileName, encoder );
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Создание WriteableBitmap
        public static WriteableBitmapWrapper Create( WriteableBitmap writeableBitmap ) {
            media.PixelFormat format = writeableBitmap.Format;
            
            if ( format == media.PixelFormats.Rgb48 ) {
                return new WriteableBitmapWrapperRGB48( writeableBitmap );
            }

            if ( format == media.PixelFormats.Rgba64 ) {
                return new WriteableBitmapWrapperRGBA64( writeableBitmap );
            }
            if ( format == media.PixelFormats.Bgra32 ) {
                return new WriteableBitmapWrapperBGRA32( writeableBitmap );
            }
            if ( format == media.PixelFormats.Gray16 ) {
                return new WriteableBitmapWrapperGray16( writeableBitmap );
            }
            if ( format == media.PixelFormats.Indexed8 ) {
                return new WriteableBitmapWrapperIndexed8( writeableBitmap );
            }
            else {
                throw new ImageException();
            }   
        }
        //-----------------------------------------------------------------------------------------------
        //Получить цвет пикселя
        public abstract media.Color GetPixelColor( int x, int y );
        //-----------------------------------------------------------------------------------------------
        //Установить цвет пикселя
        public abstract void SetPixelColor( int x, int y, media.Color color );
        //-----------------------------------------------------------------------------------------------
        public abstract double GetGrayValue( int x, int y );
        public abstract double SetGrayValue( int x, int y, double grayValue );
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Установить цвет пикселей
        public void SetPixelsColor( IList<System.Drawing.Point> points, media.Color color ) {
            for ( int index = 0; index < points.Count; index++ ) {
                System.Drawing.Point point = points[ index ];
                this.SetPixelColor( point.X, point.Y, color );
            }
        }
        //-----------------------------------------------------------------------------------------------
        //Матрица цветов
        public media.Color[ , ] GetColorMatrix() {
            media.Color[ , ] colorMatrix = new media.Color[ this.Image.PixelHeight, this.Image.PixelWidth ];
            for ( int x = 0; x < this.Image.PixelWidth; x++ ) {
                for ( int y = 0; y < this.Image.PixelHeight; y++ ) {
                    media.Color color = this.GetPixelColor( x, y );
                    colorMatrix[ y, x ] = color;
                }
            }
            return colorMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        //Цвета строки
        public media.Color[] GetRowColors( int rowIndex ) {
            media.Color[] rowColors = new media.Color[ this.Image.PixelWidth ];
            for ( int x = 0; x < this.Image.PixelWidth; x++ ) {
                media.Color color = this.GetPixelColor( x, rowIndex );
                rowColors[ x ] = color;
            }
            return rowColors;
        }
        //-----------------------------------------------------------------------------------------------
        public double[] GetRowGrayValues( int rowIndex ) {
            double[] grayValues = new double[ this.Image.PixelWidth ];
            for ( int x = 0; x < this.Image.PixelWidth; x++ ) {
                double grayValue = this.GetGrayValue( x, rowIndex );
                grayValues[ x ] = grayValue;
            }
            return grayValues;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Цвета столбца
        public media.Color[] GetColumnColors( int columnIndex ) {
            media.Color[] columnColors = new media.Color[ this.Image.PixelHeight ];
            for ( int y = 0; y < this.Image.PixelHeight; y++ ) {
                media.Color color = this.GetPixelColor( columnIndex, y );
                columnColors[ y ] = color;
            }
            return columnColors;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        public RealMatrix GetRedMatrix() {
            if ( !this.IsFormatRGB ) {
                throw new ImageException();
            }

            int rowCount = this.Image.PixelHeight;
            int columnCount = this.Image.PixelWidth;
            RealMatrix redMatrix = new RealMatrix( rowCount, columnCount );
            for ( int y = 0; y < rowCount; y++ ) {
                for ( int x = 0; x < columnCount; x++ ) {
                    media.Color pixelColor = this.GetPixelColor( x, y );
                    int red = pixelColor.R;
                    redMatrix[ y, x ] = red;
                }
            }
            return redMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        public RealMatrix GetGreenMatrix() {
            if ( !this.IsFormatRGB ) {
                throw new ImageException();
            }

            int rowCount = this.Image.PixelHeight;
            int columnCount = this.Image.PixelWidth;
            RealMatrix greenMatrix = new RealMatrix( rowCount, columnCount );
            for ( int y = 0; y < rowCount; y++ ) {
                for ( int x = 0; x < columnCount; x++ ) {
                    media.Color pixelColor = this.GetPixelColor( x, y );
                    int green = pixelColor.G;
                    greenMatrix[ y, x ] = green;
                }
            }
            return greenMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        public RealMatrix GetBlueMatrix() {
            if ( !this.IsFormatRGB ) {
                throw new ImageException();
            }

            int rowCount = this.Image.PixelHeight;
            int columnCount = this.Image.PixelWidth;
            RealMatrix blueMatrix = new RealMatrix( rowCount, columnCount );
            for ( int y = 0; y < rowCount; y++ ) {
                for ( int x = 0; x < columnCount; x++ ) {
                    media.Color pixelColor = this.GetPixelColor( x, y );
                    int blue = pixelColor.B;
                    blueMatrix[ y, x ] = blue;
                }
            }
            return blueMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        //Матрица интенсивностей серого
        public RealMatrix GetGrayScaleMatrix() {
            int rowCount = this.Image.PixelHeight;
            int columnCount = this.Image.PixelWidth;
            RealMatrix grayScaleMatrix = new RealMatrix( rowCount, columnCount );
            for ( int y = 0; y < rowCount; y++ ) {
                for ( int x = 0; x < columnCount; x++ ) {
                    media.Color pixelColor = this.GetPixelColor( x, y );
                    int grayIntensity = ColorWrapper.GetGrayIntensity( pixelColor );
                    grayScaleMatrix[ y, x ] = grayIntensity;
                }
            }
            return grayScaleMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        //Новое изображение c заданными dpiX, dpiY
        public WriteableBitmap GetNewWriteableBitmap( int dpiX, int dpiY ) {
            int pixelWidth = this.Image.PixelWidth;
            int pixelHeight = this.Image.PixelHeight;
            int stride = this.GetStride();

            Int32Rect rect = new Int32Rect( 0, 0, pixelWidth, pixelHeight );
            byte[] pixelsBytes = new byte[ this.SizeInBytes ];
            this.Image.CopyPixels( rect, pixelsBytes, stride, 0 );

            media.PixelFormat pixelFormat = this.Image.Format;
            WriteableBitmap newImage = WriteableBitmapCreator.CreateWriteableBitmap
                ( pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat );
            newImage.WritePixels( rect, pixelsBytes, stride, 0 );
            return newImage;
        }
        //-----------------------------------------------------------------------------------------------
        public WriteableBitmap GetExtendedWriteableBitmapByOnePixelToBorder() {
            int pixelWidth = this.Image.PixelWidth;
            int pixelHeight = this.Image.PixelHeight;
            int stride = this.GetStride();

            int dpiX = OS.OS.IntegerSystemDpiX;
            int dpiY = OS.OS.IntegerSystemDpiY;

            Int32Rect rect = new Int32Rect( 0, 0, pixelWidth, pixelHeight );
            byte[] pixelsBytes = new byte[ this.SizeInBytes ];
            this.Image.CopyPixels( rect, pixelsBytes, stride, 0 );
            
            media.PixelFormat pixelFormat = this.Image.Format;

            WriteableBitmap newImage = WriteableBitmapCreator.CreateWriteableBitmap
                ( pixelWidth + 2, pixelHeight + 2, dpiX, dpiY, pixelFormat );

            Int32Rect newRect = new Int32Rect( 1, 1, pixelWidth, pixelHeight );
            newImage.WritePixels( newRect, pixelsBytes, stride, 0 );
            return newImage;
        }
        //-----------------------------------------------------------------------------------------------
        //Матрица интенсивностей серого
        public static RealMatrix GetGrayScaleMatrixFromWriteableBitmap( WriteableBitmap bitmap ) {
            WriteableBitmapWrapper bitmapWrapper = WriteableBitmapWrapper.Create( bitmap );
            RealMatrix grayScaleMatrix = bitmapWrapper.GetGrayScaleMatrix();
            return grayScaleMatrix;
        }
        //-----------------------------------------------------------------------------------------------
        public bool IsFormatRGB {
            get {
                return WriteableBitmapWrapper.IsImageFormatRGB( this.writeableBitmap );
            }
        }
        //-----------------------------------------------------------------------------------------------------
        public bool IsFormatGrayScale {
            get {
                return WriteableBitmapWrapper.IsImageFormatGrayScale( this.writeableBitmap );
            }
        }
        //-----------------------------------------------------------------------------------------------------
        public static bool IsImageFormatRGB( WriteableBitmap image ) {
            bool isRGB =
               image.Format == media.PixelFormats.Bgra32 ||
               image.Format == media.PixelFormats.Bgr32;
            return isRGB;
        }
        //-----------------------------------------------------------------------------------------------------
        public static bool IsImageFormatGrayScale(WriteableBitmap image) {
            bool isForamtGrayScale =
                image.Format == media.PixelFormats.Gray16 ||
                image.Format == media.PixelFormats.Gray8 ||
                image.Format == media.PixelFormats.Gray4 ||
                image.Format == media.PixelFormats.Gray2 ||
                image.Format == media.PixelFormats.Gray32Float;
            return isForamtGrayScale;
        }
        //-----------------------------------------------------------------------------------------------------


        //-----------------------------------------------------------------------------------------------------
        //Обрезка изображения
        public WriteableBitmap GetSubBitmap( System.Drawing.Point leftTop, System.Drawing.Point rightBottom ) {
            int newImageWidth = Convert.ToInt32( Math.Abs( rightBottom.X - leftTop.X ) ) + 1;
            int newImageHeight = Convert.ToInt32( Math.Abs( rightBottom.Y - leftTop.Y ) ) + 1;
            media.PixelFormat pixelFormat = this.Image.Format;
            int dpiX = Convert.ToInt32( OS.OS.SystemDpiX );
            int dpiY = Convert.ToInt32( OS.OS.SystemDpiY );
            WriteableBitmap newBitmap = WriteableBitmapCreator.CreateWriteableBitmap
                ( newImageWidth, newImageHeight, dpiX, dpiY, pixelFormat );
            WriteableBitmapWrapper newImageWrapper = WriteableBitmapWrapper.Create( newBitmap );

            for ( int y = leftTop.Y, newY = 0; y <= rightBottom.Y; y++, newY++ ) {
                for ( int x = leftTop.X, newX = 0; x <= rightBottom.X; x++, newX++ ) {
                    media.Color color = this.GetPixelColor( x, y );
                    newImageWrapper.SetPixelColor( newX, newY, color );
                }
            }

            return newBitmap;
        }
        //-----------------------------------------------------------------------------------------------------
        public void DrawFillRectangle( Rect rectangle, media.Color color ) {
            int startX = ( int )rectangle.X;
            int startY = ( int )rectangle.Y;

            int endX = ( int )rectangle.BottomRight.X;
            int endY = ( int )rectangle.BottomRight.Y;

            for ( int x = startX; x <= endX; x++ ) {
                for ( int y = startY; y < endY; y++ ) {
                    this.SetPixelColor( x, y, color );
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------
        public void FillRegion( int startX, int startY, media.Color borderColor, media.Color newColor ) {
            Stack<drawing.Point> points = new Stack<drawing.Point>();
            points.Push( new drawing.Point( startX, startY ) );

            while ( points.Count > 0 ) {
                drawing.Point point = points.Pop();
                int x = point.X;
                int y = point.Y;
                                
                if ( this.GetPixelColor( x, y ) != borderColor ) {
                                        
                    this.SetPixelColor( x, y, newColor );
                    int newX, newY;
                    media.Color color;

                    newX = x;
                    newY = y - 1;
                    if ( this.ValidateCoordinates( newX, newY ) ) {
                        color = this.GetPixelColor( newX, newY );
                        if ( color != borderColor && color != newColor ) {
                            points.Push( new drawing.Point( newX, newY ) );
                        }
                    }

                    newX = x;
                    newY = y + 1;
                    if ( this.ValidateCoordinates( newX, newY ) ) {
                        color = this.GetPixelColor( newX, newY );
                        if ( color != borderColor && color != newColor ) {
                            points.Push( new drawing.Point( newX, newY ) );
                        }
                    }
                    
                    newX = x - 1;
                    newY = y;
                    if ( this.ValidateCoordinates( newX, newY ) ) {
                        color = this.GetPixelColor( newX, newY );
                        if ( color != borderColor && color != newColor ) {
                            points.Push( new drawing.Point( newX, newY ) );
                        }
                    }
                    
                    newX = x + 1;
                    newY = y;
                    if ( this.ValidateCoordinates( newX, newY ) ) {
                        color = this.GetPixelColor( newX, newY );
                        if ( color != borderColor && color != newColor ) {
                            points.Push( new drawing.Point( newX, newY ) );
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------
        private bool ValidateCoordinates( int x, int y ) {
            if ( x > 0 && x < this.writeableBitmap.PixelWidth && y > 0 && y < this.writeableBitmap.PixelHeight ) {
                return true;
            }
            else {
                return false;
            }
        }
        //-----------------------------------------------------------------------------------------------------
        public List<drawing.Point> GetSpecificColorPoints( media.Color color ) {
            List<drawing.Point> points = new List<drawing.Point>();
            
            int rowCount = this.Image.PixelHeight;
            int columnCount = this.Image.PixelWidth;
            RealMatrix greenMatrix = new RealMatrix( rowCount, columnCount );
            for ( int y = 0; y < rowCount; y++ ) {
                for ( int x = 0; x < columnCount; x++ ) {
                    if ( this.GetPixelColor( x, y ) == color ) {
                        points.Add( new drawing.Point( x, y ) );
                    }
                }
            }

            return points;
        }
        //-----------------------------------------------------------------------------------------------------
        public void Dilatate( media.Color color ) {
            List<drawing.Point> points = this.GetSpecificColorPoints( color );

            int newX, newY;
            foreach ( drawing.Point point in points ) {
                
                newX = point.X;
                newY = point.Y - 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    this.SetPixelColor( newX, newY, color );
                }

                newX = point.X;
                newY = point.Y + 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    this.SetPixelColor( newX, newY, color );
                }


                newX = point.X - 1;
                newY = point.Y;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    this.SetPixelColor( newX, newY, color );
                }

                newX = point.X + 1;
                newY = point.Y - 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    this.SetPixelColor( newX, newY, color );
                }

                newX = point.X + 1;
                newY = point.Y + 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    this.SetPixelColor( newX, newY, color );
                }

                newX = point.X - 1;
                newY = point.Y + 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    this.SetPixelColor( newX, newY, color );
                }


                newX = point.X + 1;
                newY = point.Y - 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    this.SetPixelColor( newX, newY, color );
                }

                newX = point.X - 1;
                newY = point.Y - 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    this.SetPixelColor( newX, newY, color );
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------
        public void DeleteBlackPoints() {
              
            List<System.Drawing.Point> points = this.GetSpecificColorPoints( media.Colors.Black );
            
            foreach ( System.Drawing.Point point in points ) {
                Dictionary<media.Color, int> colorsCount = new Dictionary<media.Color, int>();
                int newX, newY;
                media.Color color;
                int count;

                newX = point.X;
                newY = point.Y - 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    color = this.GetPixelColor( newX, newY );
                    if ( color != media.Colors.Black ) {
                        if ( colorsCount.TryGetValue( color, out count ) ) {
                            colorsCount[ color ] = count + 1;
                        }
                        else {
                            colorsCount[ color ] = 1;
                        }
                    }
                }
                
                newX = point.X;
                newY = point.Y + 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    color = this.GetPixelColor( newX, newY );
                    if ( color != media.Colors.Black ) {
                        if ( colorsCount.TryGetValue( color, out count ) ) {
                            colorsCount[ color ] = count + 1;
                        }
                        else {
                            colorsCount[ color ] = 1;
                        }
                    }
                }

                newX = point.X - 1;
                newY = point.Y;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    color = this.GetPixelColor( newX, newY );
                    if ( color != media.Colors.Black ) {
                        if ( colorsCount.TryGetValue( color, out count ) ) {
                            colorsCount[ color ] = count + 1;
                        }
                        else {
                            colorsCount[ color ] = 1;
                        }
                    }
                }

                newX = point.X + 1;
                newY = point.Y;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    color = this.GetPixelColor( newX, newY );
                    if ( color != media.Colors.Black ) {
                        if ( colorsCount.TryGetValue( color, out count ) ) {
                            colorsCount[ color ] = count + 1;
                        }
                        else {
                            colorsCount[ color ] = 1;
                        }
                    }
                }

                newX = point.X + 1;
                newY = point.Y + 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    color = this.GetPixelColor( newX, newY );
                    if ( color != media.Colors.Black ) {
                        if ( colorsCount.TryGetValue( color, out count ) ) {
                            colorsCount[ color ] = count + 1;
                        }
                        else {
                            colorsCount[ color ] = 1;
                        }
                    }
                }


                newX = point.X + 1;
                newY = point.Y - 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    color = this.GetPixelColor( newX, newY );
                    if ( color != media.Colors.Black ) {
                        if ( colorsCount.TryGetValue( color, out count ) ) {
                            colorsCount[ color ] = count + 1;
                        }
                        else {
                            colorsCount[ color ] = 1;
                        }
                    }
                }

                newX = point.X - 1;
                newY = point.Y + 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    color = this.GetPixelColor( newX, newY );
                    if ( color != media.Colors.Black ) {
                        if ( colorsCount.TryGetValue( color, out count ) ) {
                            colorsCount[ color ] = count + 1;
                        }
                        else {
                            colorsCount[ color ] = 1;
                        }
                    }
                }

                newX = point.X - 1;
                newY = point.Y - 1;
                if ( this.ValidateCoordinates( newX, newY ) ) {
                    color = this.GetPixelColor( newX, newY );
                    if ( color != media.Colors.Black ) {
                        if ( colorsCount.TryGetValue( color, out count ) ) {
                            colorsCount[ color ] = count + 1;
                        }
                        else {
                            colorsCount[ color ] = 1;
                        }
                    }
                }

                media.Color bestColor = media.Colors.Black;
                int max = 0;
                foreach ( KeyValuePair<media.Color, int> kvp in colorsCount ) {
                    if ( kvp.Value > max ) {
                        max = kvp.Value;
                        bestColor = kvp.Key;
                    }
                }

                this.SetPixelColor( point.X, point.Y, bestColor );
            }

        }
        //-----------------------------------------------------------------------------------------------------
    }
}
