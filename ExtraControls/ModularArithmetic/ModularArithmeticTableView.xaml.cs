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

using ExtraLibrary.Mathematics.ModularArithmetic;

namespace ExtraControls
{
    /// <summary>
    /// Логика взаимодействия для ModularArithmeticTable.xaml
    /// </summary>
    public partial class ModularArithmeticTableView : UserControl
    {
        public ModularArithmeticTableView()
        {
            InitializeComponent();
        }
        
        public void UpdateTable(ModularArithmeticTable table)
        {
            this.mainGrid.RowDefinitions.Clear();
            this.mainGrid.ColumnDefinitions.Clear();

            int cellWidth = 50;
            int cellHeight = 50;

            for (int m2 = 0; m2 < table.M2; m2++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition()
                {
                    MinWidth = cellWidth,
                    MaxWidth = cellWidth
                };
                this.mainGrid.ColumnDefinitions.Add(columnDefinition);
            }

            for (int m1 = 0; m1 < table.M1; m1++)
            {
                RowDefinition rowDefinition = new RowDefinition()
                {
                    MinHeight = cellHeight,
                    MaxHeight = cellHeight
                };
                this.mainGrid.RowDefinitions.Add(rowDefinition);
            }

            for (int m1 = 0; m1 < table.M1; m1++)
            {
                for (int m2 = 0; m2 < table.M2; m2++)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = table.Table[m1, m2].ToString();
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.Margin = new Thickness(5);
                    Grid.SetRow(tb, m1);
                    Grid.SetColumn(tb, m2);
                    this.mainGrid.Children.Add(tb);
                }
            }

            this.InvalidateVisual();
        }
    }
}
