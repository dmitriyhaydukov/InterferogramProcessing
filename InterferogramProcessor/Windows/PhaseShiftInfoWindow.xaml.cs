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
    /// Interaction logic for PhaseShiftInfoWindow.xaml
    /// </summary>
    public partial class PhaseShiftInfoWindow : Window {

        private List<double[]> phaseShiftsList;
        private int currentIndex = -1;

        public PhaseShiftInfoWindow() {
            InitializeComponent();

            this.phaseShiftsList = new List<double[]>();
            double[] phaseShifts;

            phaseShifts = new double[] { 0, Math.PI / 2, 2 * Math.PI / 3 };
            phaseShiftsList.Add( phaseShifts );

            phaseShifts = new double[] { 0, 2 * Math.PI / 3, 4 * Math.PI / 3 };
            phaseShiftsList.Add( phaseShifts );

            phaseShifts = new double[] { 0, Math.PI / 3, 2 * Math.PI / 2 };
            phaseShiftsList.Add( phaseShifts );

            phaseShifts = new double[] { 0, Math.PI / 3, 2 * Math.PI / 3 };
            phaseShiftsList.Add( phaseShifts );
        }

        private double[] phaseShifts;

        public double[] PhaseShifts {
            get {
                return this.phaseShifts;
            }
            set {
                this.phaseShifts = value;
            }
        }
        
        private void okButton_Click( object sender, RoutedEventArgs e ) {
            
            double phaseShift1 = double.Parse( this.shift1TextBox.Text );
            double phaseShift2 = double.Parse( this.shift2TextBox.Text );
            double phaseShift3 = double.Parse( this.shift3TextBox.Text );

            this.phaseShifts = new double[] { phaseShift1, phaseShift2, phaseShift3 };

            this.DialogResult = true;
        }

        private void cancelButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = false;
        }

        private void nextPhaseShiftsSetButton_Click( object sender, RoutedEventArgs e ) {
            currentIndex++;
            if ( currentIndex == phaseShiftsList.Count ) {
                currentIndex = 0;
            }
            
            double[] phaseShifts = phaseShiftsList[ currentIndex ];

            this.shift1TextBox.Text = phaseShifts[ 0 ].ToString();
            this.shift2TextBox.Text = phaseShifts[ 1 ].ToString();
            this.shift3TextBox.Text = phaseShifts[ 2 ].ToString();

        }
    }
}
