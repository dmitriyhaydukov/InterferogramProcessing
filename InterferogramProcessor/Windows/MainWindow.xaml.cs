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

using WPFExtensions.Controls;
using ExtraControls;

namespace InterferogramProcessing {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        //--------------------------------------------------------------------------------------------------
        private MainViewModel mainViewModel;
        //--------------------------------------------------------------------------------------------------
        private Point lastClickedImagePoint;
        //--------------------------------------------------------------------------------------------------
        public MainWindow() {
            InitializeComponent();
            InitializeControls();
        }
        //--------------------------------------------------------------------------------------------------
        public void InitializeControls() {
            string axisTitleX = "Координата X изображения";
            string axisTitleY = "Интенсивность";
            //string axisTitleY = "Фаза (в радианах)";
            AxesInfo axesInfo = new AxesInfo( axisTitleX, axisTitleY );
            this.mainChartConrol.GraphAxesInfo = axesInfo;
        }
        //--------------------------------------------------------------------------------------------------
        public MainViewModel ViewModel {
            set {
                this.mainViewModel = value;
                this.DataContext = this.mainViewModel;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private void mainLeftImage_MouseMove( object sender, MouseEventArgs e ) {
            Image imageControl = sender as Image;
            Point point = e.GetPosition( imageControl );
            int x = Convert.ToInt32( point.X );
            int y = Convert.ToInt32( point.Y );
            
            this.imageCoordinateX.Content = x.ToString();
            this.imageCoordinateY.Content = y.ToString();
        }
        //--------------------------------------------------------------------------------------------------
        private void mainRightImage_MouseMove( object sender, MouseEventArgs e ) {

        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private void mainImagesViewerControl_SelectedImageChanged(
            object sender, ExtraControls.SelectedImageChangedEventArgs args
        ) {
            ExtraImagesViewerControl extraImagesViewControl = sender as ExtraImagesViewerControl;
            int index = extraImagesViewControl.SelectedIndex;
            if ( index == -1 ) {
                return;
            }
            this.mainViewModel.MainLeftImage = 
                this.mainViewModel.ImagesViewModel.ImageInfoCollection[ index ].ImageSource;
            this.mainViewModel.mainLeftMatrix =
                this.mainViewModel.ImagesViewModel.ImageInfoCollection[ index ].Matrix;
        }
        //--------------------------------------------------------------------------------------------------
        private void mainLeftImage_MouseDown( object sender, MouseButtonEventArgs e ) {
            if ( e.ClickCount == 1 ) {
                Image imageControl = sender as Image;
                Point point = e.GetPosition( imageControl );
                int row = ( int )point.Y;
                this.lastClickedImagePoint = point;
                //this.mainViewModel.SetLastClickedPoint( new System.Drawing.Point( ( int )point.X, ( int )point.Y ) );
                
                this.mainViewModel.AddPointToFourierTransformFilterPoints
                    ( new System.Drawing.Point( ( int )point.X, ( int )point.Y ) );
                
                WriteableBitmap leftBitmap = this.mainViewModel.MainLeftImage as WriteableBitmap;
                WriteableBitmap rightBitmap = this.mainViewModel.MainRightImage as WriteableBitmap;
                this.mainViewModel.ActiveImage = leftBitmap;
                
                this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void mainRightImage_MouseDown( object sender, MouseButtonEventArgs e ) {
            if ( e.ClickCount == 1 ) {
                Image imageControl = sender as Image;
                Point point = e.GetPosition( imageControl );
                int row = ( int )point.Y;
                this.lastClickedImagePoint = point;
                //this.mainViewModel.SetLastClickedPoint( new System.Drawing.Point( ( int )point.X, ( int )point.Y ) );

                this.mainViewModel.AddPointToFourierTransformFilterPoints
                    ( new System.Drawing.Point( ( int )point.X, ( int )point.Y ) );

                WriteableBitmap leftBitmap = this.mainViewModel.MainLeftImage as WriteableBitmap;
                WriteableBitmap rightBitmap = this.mainViewModel.MainRightImage as WriteableBitmap;
                this.mainViewModel.ActiveImage = rightBitmap;

                if ( this.mainViewModel.ImagesViewingMode == ImagesViewingMode.ImagesComparison ) {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }
                else if ( this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined ) {
                    this.mainViewModel.SetGraphInfoCollection( rightBitmap, leftBitmap, row );
                }
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void GraphGrayScaleMode_Checked( object sender, RoutedEventArgs e ) {
            if ( this.mainViewModel.ActiveImage != null ) {
                int row = ( int )this.lastClickedImagePoint.Y;
                WriteableBitmap leftBitmap = this.mainViewModel.MainLeftImage as WriteableBitmap;
                WriteableBitmap rightBitmap = this.mainViewModel.MainRightImage as WriteableBitmap;

                if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainLeftImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }
                else if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainRightImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( rightBitmap, leftBitmap, row );
                }
                else {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }

                this.SetGraphIntensityTitle();
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void GraphRedMode_Checked( object sender, RoutedEventArgs e ) {
            if ( this.mainViewModel.ActiveImage != null ) {
                int row = ( int )this.lastClickedImagePoint.Y;
                WriteableBitmap leftBitmap = this.mainViewModel.MainLeftImage as WriteableBitmap;
                WriteableBitmap rightBitmap = this.mainViewModel.MainRightImage as WriteableBitmap;

                if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainLeftImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }
                else if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainRightImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( rightBitmap, leftBitmap, row );
                }
                else {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }

                this.SetGraphIntensityTitle();
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void GraphGreenMode_Checked( object sender, RoutedEventArgs e ) {
            if ( this.mainViewModel.ActiveImage != null ) {
                int row = ( int )this.lastClickedImagePoint.Y;
                WriteableBitmap leftBitmap = this.mainViewModel.MainLeftImage as WriteableBitmap;
                WriteableBitmap rightBitmap = this.mainViewModel.MainRightImage as WriteableBitmap;

                if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainLeftImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }
                else if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainRightImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( rightBitmap, leftBitmap, row );
                }
                else {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }

                this.SetGraphIntensityTitle();
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void GraphBlueMode_Checked( object sender, RoutedEventArgs e ) {
            if ( this.mainViewModel.ActiveImage != null ) {
                int row = ( int )this.lastClickedImagePoint.Y;
                WriteableBitmap leftBitmap = this.mainViewModel.MainLeftImage as WriteableBitmap;
                WriteableBitmap rightBitmap = this.mainViewModel.MainRightImage as WriteableBitmap;

                if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainLeftImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }
                else if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainRightImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( rightBitmap, leftBitmap, row );
                }
                else {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }

                this.SetGraphIntensityTitle();
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void GraphPhaseMode_Checked( object sender, RoutedEventArgs e ) {
            if ( this.mainViewModel.ActiveImage != null ) {
                int row = ( int )this.lastClickedImagePoint.Y;
                WriteableBitmap leftBitmap = this.mainViewModel.MainLeftImage as WriteableBitmap;
                WriteableBitmap rightBitmap = this.mainViewModel.MainRightImage as WriteableBitmap;

                if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainLeftImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }
                else if (
                    this.mainViewModel.ImagesViewingMode == ImagesViewingMode.Undefined &&
                    this.mainViewModel.ActiveImage == this.mainViewModel.MainRightImage
                ) {
                    this.mainViewModel.SetGraphInfoCollection( rightBitmap, leftBitmap, row );
                }
                else {
                    this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
                }

                this.SetGraphPhaseTitle();
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void RowNumberButtonOK_Click( object sender, RoutedEventArgs e ) {
            int row = int.Parse( this.rowNumberTextBox.Text );
            
            WriteableBitmap leftBitmap = this.mainViewModel.MainLeftImage as WriteableBitmap;
            WriteableBitmap rightBitmap = this.mainViewModel.MainRightImage as WriteableBitmap;
            
            this.mainViewModel.SetGraphInfoCollection( leftBitmap, rightBitmap, row );
        }
        //--------------------------------------------------------------------------------------------------
        private void MainImagesViewerControl_KeyUp( object sender, KeyEventArgs e ) {
            if ( e.Key == Key.Delete ) {
                this.mainViewModel.ImagesViewModel.DeleteImagesCommand.Execute( sender );
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void SetAxisY_Name_Intensity_Click( object sender, RoutedEventArgs e ) {
            this.SetGraphIntensityTitle();
        }
        //--------------------------------------------------------------------------------------------------
        private void SetAxisY_Name_Phase_Click( object sender, RoutedEventArgs e ) {
            this.SetGraphPhaseTitle();
            
        }

        private void coefficientsTextBox_TextChanged( object sender, TextChangedEventArgs e ) {
                    
        }
        //--------------------------------------------------------------------------------------------------
        private void SetGraphPhaseTitle() {
            string axisTitleX = "Координата X изображения";
            string axisTitleY = "Фаза (в радианах)";
            AxesInfo axesInfo = new AxesInfo( axisTitleX, axisTitleY );
            this.mainChartConrol.GraphAxesInfo = axesInfo;
        }
        //--------------------------------------------------------------------------------------------------
        private void SetGraphIntensityTitle() {
            string axisTitleX = "Координата X изображения";
            string axisTitleY = "Интенсивность";
            AxesInfo axesInfo = new AxesInfo( axisTitleX, axisTitleY );
            this.mainChartConrol.GraphAxesInfo = axesInfo;
        }

        //--------------------------------------------------------------------------------------------------
    }
}
