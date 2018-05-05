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

namespace UserInterfaceHelping {
    /// <summary>
    /// Interaction logic for SwordfishGraphWindow.xaml
    /// </summary>
    public partial class ZedGraphWindow : Window {
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public ZedGraphWindow() {
            InitializeComponent();
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public IList<ZedGraphInfo> ZedGraphsInfo {
            set {
                this.chartControl.GraphInfoCollection = value;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public AxesInfo AxesInfo {
            set {
                this.chartControl.GraphAxesInfo = value;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}
