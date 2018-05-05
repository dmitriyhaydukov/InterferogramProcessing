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

namespace UserInterfaceHelping {
    /// <summary>
    /// Interaction logic for SwordfishGraphWindow.xaml
    /// </summary>
    public partial class HelixGraph3DWindow : Window {
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public HelixGraph3DWindow() {
            InitializeComponent();
        }
        //--------------------------------------------------------------------------------------------------
        /*
        public IList<HelixPointsInfo> PointsInfoCollection {
            set {
                this.helixGraph3DControl.PointsInfoCollection = value;
            }
        }
        */
        //--------------------------------------------------------------------------------------------------
        public void AddPointsInfo( HelixPointsInfo pointsInfo ) {
            this.helixGraph3DControl.AddPointsInfo( pointsInfo );
        }
        //--------------------------------------------------------------------------------------------------
        public void AddGridLinesInfo( HelixGridLinesInfo gridLinesInfo ) {
            this.helixGraph3DControl.AddGridLinesInfo( gridLinesInfo );
        }
        //--------------------------------------------------------------------------------------------------
        public void ExecuteAutotuning() {
            this.helixGraph3DControl.ExecuteAutotuning();        
        }
        //--------------------------------------------------------------------------------------------------
        private void SaveContentMenuItem_Click( object sender, RoutedEventArgs e ) {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".png";
            saveDialog.Filter = "Images (.png)|*.png";

            if ( saveDialog.ShowDialog() == true ) {
                int width = ( int )this.helixGraph3DControl.ActualWidth;
                int height = ( int )this.helixGraph3DControl.ActualHeight;
                string fileName = saveDialog.FileName;
                ExtraHelperWPF.SaveControlImageToPngFile( this.helixGraph3DControl, width, height, fileName );
            }
        }
        //--------------------------------------------------------------------------------------------------
    }
}
