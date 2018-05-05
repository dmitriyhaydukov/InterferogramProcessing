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

using Interferometry.Helpers;

namespace InterferogramProcessing {
    /// <summary>
    /// Interaction logic for InterferogramInfoWindow.xaml
    /// </summary>
    public partial class InterferogramInfoWindow : Window {
        public InterferogramInfoWindow() {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------------------------
        public int InterferogramWidth {
            get {
                int width = Convert.ToInt32( this.interferogramWidthTextBox.Text );
                return width;
            }
            set {
                this.interferogramWidthTextBox.Text = value.ToString();
            }
        }
        //-------------------------------------------------------------------------------------------------
        public int InterferogramHeight {
            get {
                int height = Convert.ToInt32( this.interferogramHeightTextBox.Text );
                return height;
            }
            set {
                this.interferogramHeightTextBox.Text = value.ToString();
            }
        }
        //-------------------------------------------------------------------------------------------------
        public int FringeCount {
            get {
                int fringeCount = Convert.ToInt32( this.interferogramFringeCountTextBox.Text );
                return fringeCount;
            }
            set {
                this.interferogramFringeCountTextBox.Text = value.ToString();
            }
        }

        //-------------------------------------------------------------------------------------------------
        public double IntensityNoisePercent {
            get {
                double intensityNoisePercent = Convert.ToInt32( this.intensityNoisePercentTextBox.Text );
                return intensityNoisePercent;
            }
            set {
                this.intensityNoisePercentTextBox.Text = value.ToString();
            }
        }
        //-------------------------------------------------------------------------------------------------
        public double[] PhaseShifts {
            get {
                double[] phaseShifts = this.phasShiftsListBox.ItemsSource as double[];
                return phaseShifts;
            }
            set {
                this.phasShiftsListBox.ItemsSource = value;
            }
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        private void OkButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = true;
            this.Close();
        }
        //-------------------------------------------------------------------------------------------------
        private void CancelButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = false;
            this.Close();
        }
        //-------------------------------------------------------------------------------------------------
        private void GeneratePhaseShiftsButton_Click( object sender, RoutedEventArgs e ) {
            double[] phaseShifts = InterferogramProcessingHelper.GetPhaseShifts();
            this.phasShiftsListBox.ItemsSource = phaseShifts;
        }
        //-------------------------------------------------------------------------------------------------
        private void GenerateRandomPhaseShiftsButton_Click( object sender, RoutedEventArgs e ) {
            int count = 3;
            double[] phaseShifts = InterferometryHelper.GetRandomPhaseShifts( count );
            this.phasShiftsListBox.ItemsSource = phaseShifts;
        }
        //-------------------------------------------------------------------------------------------------
        private void GenerateNoisyPhaseShiftsButton_Click( object sender, RoutedEventArgs e ) {
            double noisePercent = double.Parse( this.phaseShiftNoisePercentTextBox.Text );
            double[] phaseShifts = InterferogramProcessingHelper.GetNoisyPhaseShifts( noisePercent );
            this.phasShiftsListBox.ItemsSource = phaseShifts;
        }
        //------------------------------------------------------------------------------------------------- 
        private void GenerateManyPhaseShiftsButton_Click( object sender, RoutedEventArgs e ) {
            double[] phaseShifts = 
                InterferogramProcessingHelper.GetPhaseShiftsForTwoPointAlgorithmPhaseShiftEstimation();
            this.phasShiftsListBox.ItemsSource = phaseShifts;
        }
        //-------------------------------------------------------------------------------------------------
    }
}
