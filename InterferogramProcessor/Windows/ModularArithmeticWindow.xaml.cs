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

using ExtraLibrary.Mathematics.ModularArithmetic;

namespace InterferogramProcessing
{
    /// <summary>
    /// Логика взаимодействия для ModularArithmeticWindow.xaml
    /// </summary>
    public partial class ModularArithmeticWindow : Window
    {
        public ModularArithmeticWindow()
        {
            InitializeComponent();
        }

        public void UpdateTable(ModularArithmeticTable table)
        {
            this.tableView.UpdateTable(table);
        }
    }
}
