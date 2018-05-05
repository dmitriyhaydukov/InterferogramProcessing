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
    /// Interaction logic for GenericPhaseShiftInfoWindow.xaml
    /// </summary>
    public partial class GenericPhaseShiftInfoWindow : Window {
        int phaseShiftsCount;

        public GenericPhaseShiftInfoWindow(int phaseShiftsCount) {
            InitializeComponent();
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            this.phaseShiftsCount = phaseShiftsCount;

            this.FillPhaseShiftsGrid();
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

            List<double> shifts = new List<double>();
            
            for ( int index = 0; index < this.phaseShiftsGrid.RowDefinitions.Count; index++ ) {
               
                TextBox phaseShiftTextBox = 
                   (TextBox)this.phaseShiftsGrid.Children.Cast<UIElement>().
                   First(cntrl => Grid.GetRow(cntrl) == index && Grid.GetColumn(cntrl) == 1);

                shifts.Add( int.Parse( phaseShiftTextBox.Text ) );
            }

            this.phaseShifts = shifts.ToArray();
            
            this.DialogResult = true;
        }

        private void cancelButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = false;
        }

        private void phaseShiftsCountOkButton_Click( object sender, RoutedEventArgs e ) {

            //int phaseShiftsCount = int.Parse( this.phaseShiftsCountTextBox.Text );

            this.phaseShiftsGrid.Children.Clear();
            this.phaseShiftsGrid.RowDefinitions.Clear();
            this.phaseShiftsGrid.ColumnDefinitions.Clear();

            phaseShiftsGrid.ColumnDefinitions.Add( new ColumnDefinition() );
            phaseShiftsGrid.ColumnDefinitions.Add( new ColumnDefinition() );

            for ( int index = 0; index < phaseShiftsCount; index++ ) {
                phaseShiftsGrid.RowDefinitions.Add( new RowDefinition() );
                int phaseShiftNumber = index + 1;

                int height = 30;
                Label phaseShiftLabel = new Label();
                phaseShiftLabel.Content = "Фазовый сдвиг " + phaseShiftNumber.ToString();
                phaseShiftLabel.Margin = new Thickness( 5 );
                phaseShiftLabel.Height = height;
                phaseShiftLabel.SetValue( Grid.RowProperty, index );
                phaseShiftLabel.SetValue( Grid.ColumnProperty, 0 );

                TextBox phaseShiftTextBox = new TextBox();
                phaseShiftTextBox.Width = 50;
                phaseShiftTextBox.Height = height;
                phaseShiftTextBox.Margin = new Thickness( 5 );
                phaseShiftTextBox.SetValue( Grid.RowProperty, index );
                phaseShiftTextBox.SetValue( Grid.ColumnProperty, 1 );

                this.phaseShiftsGrid.Children.Add( phaseShiftLabel );
                this.phaseShiftsGrid.Children.Add( phaseShiftTextBox );
            }

            this.UpdateLayout();
        }

        private void Window_Closed( object sender, EventArgs e ) {
            
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            
        }


        private void FillPhaseShiftsGrid() {
            this.phaseShiftsGrid.Children.Clear();
            this.phaseShiftsGrid.RowDefinitions.Clear();
            this.phaseShiftsGrid.ColumnDefinitions.Clear();

            phaseShiftsGrid.ColumnDefinitions.Add( new ColumnDefinition() );
            phaseShiftsGrid.ColumnDefinitions.Add( new ColumnDefinition() );

            for ( int index = 0; index < this.phaseShiftsCount; index++ ) {
                phaseShiftsGrid.RowDefinitions.Add( new RowDefinition() );
                int phaseShiftNumber = index + 1;

                int height = 30;
                Label phaseShiftLabel = new Label();
                phaseShiftLabel.Content = "Фазовый сдвиг " + phaseShiftNumber.ToString();
                phaseShiftLabel.Margin = new Thickness( 5 );
                phaseShiftLabel.Height = height;
                phaseShiftLabel.SetValue( Grid.RowProperty, index );
                phaseShiftLabel.SetValue( Grid.ColumnProperty, 0 );

                TextBox phaseShiftTextBox = new TextBox();
                phaseShiftTextBox.Width = 50;
                phaseShiftTextBox.Height = height;
                phaseShiftTextBox.Margin = new Thickness( 5 );
                phaseShiftTextBox.SetValue( Grid.RowProperty, index );
                phaseShiftTextBox.SetValue( Grid.ColumnProperty, 1 );

                this.phaseShiftsGrid.Children.Add( phaseShiftLabel );
                this.phaseShiftsGrid.Children.Add( phaseShiftTextBox );
            }

            this.UpdateLayout();
        }
    }
}
