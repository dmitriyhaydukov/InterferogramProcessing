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

namespace InterferogramProcessing {
    /// <summary>
    /// Interaction logic for RectanglePulses.xaml
    /// </summary>
    public partial class RectanglePulses : Window {
        public RectanglePulses() {
            InitializeComponent();
        }

        private void okButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = true;
        }

        private void cancelButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = false;
            this.Close();
        }


        public int RectangleWidth {
            get {
                return int.Parse( this.rectangleWidth.Text );
            }
        }

        public int RectangleInterval {
            get {
                return int.Parse( this.rectangleInterval.Text );
            }
        }

        public int RectangleHeight {
            get {
                return int.Parse( this.rectangleHeight.Text );
            }
        }

        public int ArrayLength {
            get {
                return int.Parse( this.arrayLength.Text );
            }
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {

        }
    }
}
