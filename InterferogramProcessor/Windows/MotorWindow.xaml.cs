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
using System.IO.Ports;

using OsmStepMotorController;
using OsmStepMotorController.Exceptions;


namespace InterferogramProcessing {
    /// <summary>
    /// Interaction logic for MotorWindow.xaml
    /// </summary>
    
    
    public partial class MotorWindow : Window {

        ModbusCommands modbusCommands;
                
        byte leftDirection = 0;
        byte rightDirection = 1;

        byte downDirection = 0;
        byte upDirection = 1;


        public MotorWindow() {
            InitializeComponent();
            Init();
        }

        private void Init() {

            this.connectButton.IsEnabled = true;
            this.disconnectButton.IsEnabled = false;

            string[] serialPortNames = SerialPort.GetPortNames();
            this.serialPortsComboBox.ItemsSource = serialPortNames;
        }

        private void connectButton_Click( object sender, RoutedEventArgs e ) {

            string selectedSerialPort = (string) this.serialPortsComboBox.SelectedItem;
            this.modbusCommands = new ModbusCommands( selectedSerialPort );
            
            try {
                this.modbusCommands.Connect();

                this.connectButton.IsEnabled = false;
                this.disconnectButton.IsEnabled = true;
            }
            catch ( ConnectionException exception ) {
                
                MessageBox.Show( exception.Message );
            }

        }

        private void executeMove( byte motorAddress, byte direction) {

            try {
                ushort current = ushort.Parse( this.currentTextBox.Text );
                ushort startSpeed = ushort.Parse( this.startSpeedTextBox.Text );
                ushort endSpeed = ushort.Parse( this.endSpeedTextBox.Text );
                ushort speed = ushort.Parse( this.speedTextBox.Text );
                ushort accelaration = ushort.Parse( this.accelarationTextBox.Text );
                ushort decelarationSteps = ushort.Parse( this.decelerationStepsNumberTextBox.Text );
                ushort stepsNumber = ushort.Parse( this.stepsNumberTextBox.Text );


                this.modbusCommands.SetCurrent( motorAddress, current );
                this.modbusCommands.SetStartSpeed( motorAddress, startSpeed );
                this.modbusCommands.SetEndSpeed( motorAddress, endSpeed );
                this.modbusCommands.SetSpeed( motorAddress, speed );
                this.modbusCommands.SetAcceleration( motorAddress, accelaration );
                this.modbusCommands.SetDecelerationSteps( motorAddress, decelarationSteps );
                this.modbusCommands.SetNumberOfSteps( motorAddress, stepsNumber );

                this.modbusCommands.SetDirection( motorAddress, direction );
                this.modbusCommands.MoveNumberOfSteps( motorAddress );
            }
            catch ( Exception exception ) {
                
            }
        }

        private void disconnectButton_Click( object sender, RoutedEventArgs e ) {
            try {

                this.modbusCommands.Disconnect();

                this.connectButton.IsEnabled = true;
                this.disconnectButton.IsEnabled = false;
            }
            catch ( ConnectionException exception ) {

                MessageBox.Show( exception.Message );
            }
        }
        
        private void leftButton_Click( object sender, RoutedEventArgs e ) {
            
            byte motorAddress = byte.Parse(this.motor1AddressTextBox.Text);
            this.executeMove( motorAddress, this.leftDirection );
        }
                            

        private void rightButton_Click( object sender, RoutedEventArgs e ) {

            byte motorAddress = byte.Parse( this.motor1AddressTextBox.Text );
            this.executeMove( motorAddress, this.rightDirection );
        }

        private void upButton_Click( object sender, RoutedEventArgs e ) {
            byte motorAddress = byte.Parse( this.motor2AddressTextBox.Text );
            this.executeMove( motorAddress, this.upDirection );
        }

        private void downButton_Click( object sender, RoutedEventArgs e ) {
            byte motorAddress = byte.Parse( this.motor2AddressTextBox.Text );
            this.executeMove( motorAddress, this.downDirection );
        }

    }
}
