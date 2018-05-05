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
    /// Interaction logic for ColorNimberWindow.xaml
    /// </summary>
    public partial class ColorNumberWindow : Window {
        public ColorNumberWindow() {
            InitializeComponent();
        }

        public Color LabelColor {
            get {
                SolidColorBrush brush = this.colorLabel.Background as SolidColorBrush;
                return brush.Color;
            }
            set {
                SolidColorBrush brush = new SolidColorBrush( value );
                this.colorLabel.Background = brush;
            }
        }

        public int Number {
            get {
                return int.Parse( this.numberTextBox.Text );
            }
            set {
                this.numberTextBox.Text = value.ToString();
            }
        }
        
        private void okButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = false;
            this.Close();
        }
    }
}
