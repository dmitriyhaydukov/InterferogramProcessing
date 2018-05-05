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
using Swordfish.WPF.Charts;

using ExtraLibrary.Geometry2D;
using ExtraLibrary.Mathematics.Sets;
using ExtraWPF;

using ZedGraph;


namespace ExtraControls {
    /// <summary>
    /// Логика взаимодействия для ZedGraphHistogram.xaml
    /// </summary>
    public partial class ZedGraphHistogramControl : UserControl {
        public static readonly DependencyProperty HistogramInfoProperty;
        //-------------------------------------------------------------------------------------------------------
        public ZedGraphHistogramControl() {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public HistogramInfo HistogramInfo {
            get {
                return ( HistogramInfo )this.GetValue( ZedGraphHistogramControl.HistogramInfoProperty );
            }
            set {
                this.SetValue( ZedGraphHistogramControl.HistogramInfoProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        static ZedGraphHistogramControl() {
            DependencyPropertyInfo info = new DependencyPropertyInfo();
            info.PropertyName = "HistogramInfo";
            info.DataType = typeof( HistogramInfo );
            info.OwnerDataType = typeof( ZedGraphHistogramControl );
            info.PropertyChangedHandler =
                new PropertyChangedCallback( ZedGraphHistogramControl.HistogramInfoChanged );

            ZedGraphHistogramControl.HistogramInfoProperty = ExtraHelperWPF.RegisterDependencyProperty( info );
        }
        //-------------------------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Настройки по умолчанию
        private void SetDefaultSettings() {
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        private static void HistogramInfoChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ZedGraphHistogramControl zedGraphHistogramControl = d as ZedGraphHistogramControl;

            GraphPane graphPane = zedGraphHistogramControl.zedGraphControl.GraphPane;
            graphPane.CurveList.Clear();

            HistogramInfo histogramInfo = ( HistogramInfo )e.NewValue;
            if ( histogramInfo == null ) {
                return;
            }

            PointPairList pointPairList = new PointPairList();
            double x, y;
            for ( int index = 0; index < histogramInfo.Intervals.Length; index++ ) {
                Interval<double> interval = histogramInfo.Intervals[ index ];

                x = interval.MinValue;
                y = 0;
                pointPairList.Add( x, y );

                x = interval.MinValue;
                y = histogramInfo.FrequencyValues[ index ];
                pointPairList.Add( x, y );

                x = interval.MaxValue;
                y = histogramInfo.FrequencyValues[ index ];
                pointPairList.Add( x, y );

                x = interval.MaxValue;
                y = 0;
                pointPairList.Add( x, y );
            }

            Color histogramColor = Colors.Red;
            System.Drawing.Color color =
                System.Drawing.Color.FromArgb( histogramColor.A, histogramColor.R, histogramColor.G, histogramColor.B );
            graphPane.AddCurve( histogramInfo.HistogramName, pointPairList, color, SymbolType.None );

            zedGraphHistogramControl.zedGraphControl.AxisChange();
        }
        //-------------------------------------------------------------------------------------------------------
        //Получить изображение
        public System.Drawing.Bitmap GetBitmap(int width, int height, float dpi) {
            GraphPane graphPane = this.zedGraphControl.GraphPane;
            System.Drawing.Bitmap bitmap = graphPane.GetImage( width, height, dpi );
            return bitmap;
        }
        //-------------------------------------------------------------------------------------------------------
    }
}
