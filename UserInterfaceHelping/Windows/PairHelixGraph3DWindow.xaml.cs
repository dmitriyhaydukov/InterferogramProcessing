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

using ExtraControls;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

using ExtraWPF;

using HelixToolkit.Wpf;

namespace UserInterfaceHelping {
    /// <summary>
    /// Interaction logic for SwordfishGraphWindow.xaml
    /// </summary>
    public partial class PairHelixGraph3DWindow : Window {
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public PairHelixGraph3DWindow() {
            InitializeComponent();
            Initialize();
        }
        //--------------------------------------------------------------------------------------------------
        public void Initialize() {
            this.helixGraph3DControlOne.ViewPort3D.CameraChanged +=
                new RoutedEventHandler( ViewPort3DOne_CameraChanged );
            this.helixGraph3DControlTwo.ViewPort3D.CameraChanged +=
                new RoutedEventHandler( ViewPort3DTwo_CameraChanged );
        }
        //--------------------------------------------------------------------------------------------------
        public void AddPointsInfoToLeftGraph( HelixPointsInfo pointsInfo ) {
            this.helixGraph3DControlOne.AddPointsInfo( pointsInfo );
        }
        //--------------------------------------------------------------------------------------------------
        public void AddPointsInfoToRightGraph( HelixPointsInfo pointsInfo ) {
            this.helixGraph3DControlTwo.AddPointsInfo( pointsInfo );
        }
        //--------------------------------------------------------------------------------------------------
        /*
        public IList<HelixPointsInfo> PointsInfoCollectionOne {
            set {
                this.helixGraph3DControlOne.PointsInfoCollection = value;
            }
        }
        //--------------------------------------------------------------------------------------------------
        public IList<HelixPointsInfo> PointsInfoCollectionTwo {
            set {
                this.helixGraph3DControlTwo.PointsInfoCollection = value;
            }
        }
        */ 
        //--------------------------------------------------------------------------------------------------
        public void ExecuteAutotuning() {
            this.helixGraph3DControlOne.ExecuteAutotuning();
            this.helixGraph3DControlTwo.ExecuteAutotuning();
        }
        //--------------------------------------------------------------------------------------------------
        private void SaveContentMenuItem_Click( object sender, RoutedEventArgs e ) {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".png";
            saveDialog.Filter = "Images (.png)|*.png";

            if ( saveDialog.ShowDialog() == true ) {
                int width = ( int )this.mainGrid.ActualWidth;
                int height = ( int )this.mainGrid.ActualHeight;
                string fileName = saveDialog.FileName;
                ExtraHelperWPF.SaveControlImageToPngFile( this.mainGrid, width, height, fileName );
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void ViewPort3DOne_CameraChanged( object sender, RoutedEventArgs e ) {
            CameraHelper.Copy(
                this.helixGraph3DControlOne.ViewPort3D.Camera,
                this.helixGraph3DControlTwo.ViewPort3D.Camera
            );
        }
        //--------------------------------------------------------------------------------------------------
        private void ViewPort3DTwo_CameraChanged( object sender, RoutedEventArgs e ) {
            CameraHelper.Copy(
                this.helixGraph3DControlTwo.ViewPort3D.Camera,
                this.helixGraph3DControlOne.ViewPort3D.Camera
            );
        }
        //--------------------------------------------------------------------------------------------------
    }
}
