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
using ExtraWPF;

namespace UserInterfaceHelping {
    /// <summary>
    /// Interaction logic for SwordfishGraphWindow.xaml
    /// </summary>
    public partial class SwordfishGraphWindow : Window {
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public SwordfishGraphWindow() {
            InitializeComponent();
        }
        //--------------------------------------------------------------------------------------------------
        public IList<GraphInfo> GraphInfoCollection {
            set {
                this.chartControl.GraphInfoCollection = value;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private void SaveContentMenuItem_Click( object sender, RoutedEventArgs e ) {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".png";
            saveDialog.Filter = "Images (.png)|*.png";

            if ( saveDialog.ShowDialog() == true ) {
                int width = ( int )this.chartControl.ActualWidth;
                int height = ( int )this.chartControl.ActualHeight;
                string fileName = saveDialog.FileName;
                ExtraHelperWPF.SaveControlImageToPngFile( this.chartControl, width, height, fileName );
            }
        }
        //--------------------------------------------------------------------------------------------------
    }
}
