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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UserInterfaceHelping {
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ProgressBarWindow : Window {
        //--------------------------------------------------------------------------------------------------
        UpdateProgressBarDelegate updateProogressBarDelegate;
        //--------------------------------------------------------------------------------------------------
        public ProgressBarWindow() {
            InitializeComponent();
            this.updateProogressBarDelegate = new UpdateProgressBarDelegate( this.progressBar.SetValue );
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public double ProgressBarValue {
            set {
                object propertyValueArray = new object[] { ProgressBar.ValueProperty, value };
                Dispatcher.Invoke(
                    this.updateProogressBarDelegate, DispatcherPriority.Background, propertyValueArray
                );
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}
