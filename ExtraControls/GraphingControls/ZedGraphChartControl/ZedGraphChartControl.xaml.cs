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
using ExtraWPF;

using ZedGraph;

namespace ExtraControls {
    /// <summary>
    /// Логика взаимодействия для ZedGraphLineChart.xaml
    /// </summary>
    public partial class ZedGraphChartControl : UserControl {
        //-------------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty GraphInfoCollectionProperty;
        public static readonly DependencyProperty AxesInfoProperty;
        //-------------------------------------------------------------------------------------------------------
        public ZedGraphChartControl() {
            InitializeComponent();
            this.SetDefaultSettings();
        }
        //-------------------------------------------------------------------------------------------------------
        public AxesInfo GraphAxesInfo {
            get {
                return ( AxesInfo )this.GetValue( ZedGraphChartControl.AxesInfoProperty );
            }
            set {
                this.SetValue( ZedGraphChartControl.AxesInfoProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public IList<ZedGraphInfo> GraphInfoCollection {
            get {
                return ( IList<ZedGraphInfo> )this.GetValue( ZedGraphChartControl.GraphInfoCollectionProperty );
            }
            set {
                this.SetValue( ZedGraphChartControl.GraphInfoCollectionProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        static ZedGraphChartControl() {
            DependencyPropertyInfo info;
            
            info = new DependencyPropertyInfo();
            info.PropertyName = "GraphInfoCollection";
            info.DataType = typeof( IList<ZedGraphInfo> );
            info.OwnerDataType = typeof( ZedGraphChartControl );
            info.PropertyChangedHandler =
                new PropertyChangedCallback( ZedGraphChartControl.GraphInfoCollectionChanged );
            ZedGraphChartControl.GraphInfoCollectionProperty = ExtraHelperWPF.RegisterDependencyProperty( info );
            
            info = new DependencyPropertyInfo();
            info.PropertyName = "GraphAxesInfo";
            info.DataType = typeof( AxesInfo );
            info.OwnerDataType = typeof( ZedGraphChartControl );
            info.PropertyChangedHandler =
                new PropertyChangedCallback( ZedGraphChartControl.AxesInfoChanged );
            ZedGraphChartControl.AxesInfoProperty = ExtraHelperWPF.RegisterDependencyProperty( info );
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
        private static void SetDefaultGraphPaneSettings( GraphPane graphPane ) {
            graphPane.XAxis.Cross = 0.0;
            graphPane.YAxis.Cross = 0.0;
            
            int fontSize = 8;
            graphPane.XAxis.Title.FontSpec.Size = fontSize;
            graphPane.YAxis.Title.FontSpec.Size = fontSize;

            bool isVisibleBorder = false;
            System.Drawing.Color borderColor = System.Drawing.Color.Black;
            ZedGraph.Border axisTitleBorder = new ZedGraph.Border(isVisibleBorder, borderColor, 1);
            graphPane.YAxis.Title.FontSpec.Border = axisTitleBorder;

        }
        //-------------------------------------------------------------------------------------------------------
        private static void GraphInfoCollectionChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ZedGraphChartControl zedGraphChart = d as ZedGraphChartControl;
            
            GraphPane graphPane = zedGraphChart.zedGraphControl.GraphPane;
            graphPane.CurveList.Clear();
            ZedGraphChartControl.SetDefaultGraphPaneSettings( graphPane );
            
            IList<ZedGraphInfo> graphInfoCollection = ( IList<ZedGraphInfo> )e.NewValue;
            if ( graphInfoCollection == null ) {
                return;
            }
                                   
            for ( int index = 0; index < graphInfoCollection.Count; index++ ) {
                ZedGraphInfo graphInfo = graphInfoCollection[ index ];
                ZedGraphChartControl.DrawChart( graphInfo, graphPane );
            }

            zedGraphChart.zedGraphControl.AxisChange();
        }
        //-------------------------------------------------------------------------------------------------------
        //Нарисовать график
        private static void DrawChart( ZedGraphInfo graphInfo, GraphPane graphPane ) {
            Point2D[] points = graphInfo.GraphPoints;
            PointPairList pointPairList = new PointPairList();
            for ( int pointIndex = 0; pointIndex < points.Length; pointIndex++ ) {
                Point2D point = points[ pointIndex ];
                pointPairList.Add( point.X, point.Y );
            }
            Color graphColor = graphInfo.GraphColor;
            System.Drawing.Color color =
                System.Drawing.Color.FromArgb( graphColor.A, graphColor.R, graphColor.G, graphColor.B );
            SymbolType symbolType = graphInfo.ZedGraphSymbolType;
            LineItem curve = graphPane.AddCurve( graphInfo.GraphName, pointPairList, color, symbolType );
            curve.Line.IsVisible = graphInfo.LineVisibility;
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Symbol.Size = graphInfo.ZedGraphSymbolSize;
        }
        //-------------------------------------------------------------------------------------------------------
        public static void AxesInfoChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ZedGraphChartControl zedGraphChart = d as ZedGraphChartControl;
            GraphPane graphPane = zedGraphChart.zedGraphControl.GraphPane;
            
            string fontFamily = "Arial";
            bool isBold = false;
            bool isItalic = false;
            bool isUnderLine = false;
            int fontSize = 10;
            System.Drawing.Color color = System.Drawing.Color.Black;

            AxesInfo axesInfo = ( AxesInfo )e.NewValue;
            graphPane.XAxis.Title = 
                new AxisLabel( axesInfo.AxisTitleX, fontFamily, fontSize, color, isBold, isItalic, isUnderLine );
            graphPane.YAxis.Title =
                new AxisLabel( axesInfo.AxisTitleY, fontFamily, fontSize, color, isBold, isItalic, isUnderLine );

            graphPane.XAxis.Scale.FontSpec.Size = 8;
            graphPane.YAxis.Scale.FontSpec.Size = 8;
            
        }
        //-------------------------------------------------------------------------------------------------------
        //Получить изображение
        public System.Drawing.Bitmap GetBitmap( int width, int height, float dpi ) {
            GraphPane graphPane = this.zedGraphControl.GraphPane;
            System.Drawing.Bitmap bitmap = graphPane.GetImage( width, height, dpi );
            return bitmap;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
    }
}
