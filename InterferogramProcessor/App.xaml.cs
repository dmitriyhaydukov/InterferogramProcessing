using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace InterferogramProcessing {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        protected override void OnStartup( StartupEventArgs e ) {
            base.OnStartup( e );

            MainViewModel mainViewModel = new MainViewModel();
            MainWindow mainWindow = new MainWindow();
            mainWindow.ViewModel = mainViewModel;
            mainWindow.Show();
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}
