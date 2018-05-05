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
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ExtraWPF;
using ExtraLibrary.Mathematics.Vectors;
using ExtraLibrary.Geometry3D;

using HelixToolkit.Wpf;

namespace ExtraControls {
    /// <summary>
    /// Логика взаимодействия для HelixPoints3D.xaml
    /// </summary>
    public partial class HelixGraph3DControl : UserControl {

        //-----------------------------------------------------------------------------------------------------------
        private Dictionary<HelixPointsInfo, PointsVisual3D> pointsInfoDictionary;
        private Dictionary<HelixGridLinesInfo, GridLinesVisual3D> gridLinesInfoDictionary;
        //-----------------------------------------------------------------------------------------------------------
        public HelixGraph3DControl() {

            InitializeComponent();
            this.InitalizeControl();
        }
        //-----------------------------------------------------------------------------------------------------------
        public void InitalizeControl() {
            this.pointsInfoDictionary = new Dictionary<HelixPointsInfo, PointsVisual3D>();
            this.gridLinesInfoDictionary = new Dictionary<HelixGridLinesInfo, GridLinesVisual3D>();
        }
        //-----------------------------------------------------------------------------------------------------------
        public HelixViewport3D ViewPort3D {
            get {
                return this.helixViewPort3D;
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        static HelixGraph3DControl() {
          
        }
        //-----------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------
        public void AddPointsInfo( HelixPointsInfo pointsInfo ) {
            PointsVisual3D pointsVisual3D = new PointsVisual3D();
            pointsVisual3D.Points = HelixGraph3DControl.ConvertPoints( pointsInfo.Points );
            pointsVisual3D.Size = pointsInfo.PointsSize;
            pointsVisual3D.Color = pointsInfo.PointsColor;
            helixViewPort3D.Children.Add( pointsVisual3D );

            this.pointsInfoDictionary.Add( pointsInfo, pointsVisual3D );
        }
        //-----------------------------------------------------------------------------------------------------------
        public void RemovePointsInfo( HelixPointsInfo pointsInfo ) {
            PointsVisual3D pointsVisual3D = this.pointsInfoDictionary[ pointsInfo ];
            this.helixViewPort3D.Children.Remove( pointsVisual3D );
            this.pointsInfoDictionary.Remove( pointsInfo );
        }
        //-----------------------------------------------------------------------------------------------------------
        public void AddGridLinesInfo( HelixGridLinesInfo gridLinesInfo ) {
            
            GridLinesVisual3D gridLinesVisual3D = new GridLinesVisual3D();
            gridLinesVisual3D.Width = gridLinesInfo.Width;
            gridLinesVisual3D.Length = gridLinesInfo.Length;
            gridLinesVisual3D.MajorDistance = gridLinesInfo.MajorDistance;
            gridLinesVisual3D.MinorDistance = gridLinesInfo.MinorDistance;
            gridLinesVisual3D.Thickness = gridLinesInfo.Thickness;
            
            System.Windows.Media.Media3D.Point3D center = new System.Windows.Media.Media3D.Point3D
                ( gridLinesInfo.Center.X, gridLinesInfo.Center.Y, gridLinesInfo.Center.Z );
            gridLinesVisual3D.Center = center;

            Vector3D normal = new Vector3D
                ( gridLinesInfo.Normal[ 0 ], gridLinesInfo.Normal[ 1 ], gridLinesInfo.Normal[ 2 ] );
            gridLinesVisual3D.Normal = normal;

            Vector3D lengthDirection = new Vector3D
                (
                    gridLinesInfo.LengthDirection[ 0 ],
                    gridLinesInfo.LengthDirection[ 1 ],
                    gridLinesInfo.LengthDirection[ 2 ]
                );
            gridLinesVisual3D.LengthDirection = lengthDirection;

            helixViewPort3D.Children.Add( gridLinesVisual3D );

            this.gridLinesInfoDictionary.Add( gridLinesInfo, gridLinesVisual3D );
        }
        //-----------------------------------------------------------------------------------------------------------
        public void RemoveGridLinesInfo( HelixGridLinesInfo gridLinesInfo ) {
            GridLinesVisual3D gridLinesVisual3D = this.gridLinesInfoDictionary[ gridLinesInfo ];
            this.helixViewPort3D.Children.Remove( gridLinesVisual3D );
            this.gridLinesInfoDictionary.Remove( gridLinesInfo );
        }
        //-----------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------
        private static IList<System.Windows.Media.Media3D.Point3D> ConvertPoints(
            IList<ExtraLibrary.Geometry3D.Point3D> points
        ) {
            List<System.Windows.Media.Media3D.Point3D> newPoints = new List<System.Windows.Media.Media3D.Point3D>();
            foreach ( ExtraLibrary.Geometry3D.Point3D point in points ) {
                System.Windows.Media.Media3D.Point3D newPoint =
                    new System.Windows.Media.Media3D.Point3D( point.X, point.Y, point.Z );
                newPoints.Add( newPoint );
            }
            return newPoints;
        }
        //-----------------------------------------------------------------------------------------------------------
        public void ExecuteAutotuning() {
            double mainMinX = 0, mainMinY = 0, mainMinZ = 0;
            double mainMaxX = 0, mainMaxY = 0, mainMaxZ = 0;
            
            for ( int index = 0; index < this.pointsInfoDictionary.Count; index++ ) {
                HelixPointsInfo pointsInfo = this.pointsInfoDictionary.Keys.Skip( index ).First();
                double[] minimalCoordinates = SpaceManager.GetMinimalCoordinates( pointsInfo.Points );
                double[] maximalCoordinates = SpaceManager.GetMaximalCoordinates( pointsInfo.Points );

                double minX = minimalCoordinates[ 0 ];
                double maxX = maximalCoordinates[ 0 ];

                double minY = minimalCoordinates[ 1 ];
                double maxY = maximalCoordinates[ 1 ];

                double minZ = minimalCoordinates[ 2 ];
                double maxZ = maximalCoordinates[ 2 ];

                if ( minX < mainMinX ) {
                    mainMinX = minX;
                }
                if ( minY < mainMinY ) {
                    mainMinY = minY;
                }
                if ( minZ < mainMinZ ) {
                    mainMinZ = minZ;
                }
                if ( mainMaxX < maxX ) {
                    mainMaxX = maxX;
                }
                if ( mainMaxY < maxY ) {
                    mainMaxY = maxY;
                }
                if ( mainMaxZ < maxZ ) {
                    mainMaxZ = maxZ;
                }
            }

            double mainMin = ( new double[] { mainMinX, mainMinY, mainMinZ } ).Min();
            double mainMax = ( new double[] { mainMaxX, mainMaxY, mainMaxZ } ).Max();

            this.SetArrowAxesSize( mainMin, mainMax );
            double step = 20;

            double roundMainMax = mainMax < 0 ? Math.Floor( mainMax ) : Math.Ceiling( mainMax );
            double roundMainMin = mainMin < 0 ? Math.Floor( mainMin ) : Math.Ceiling( mainMin );

            this.DrawAxesMarks( step, roundMainMin + step, roundMainMax - step );
            this.DrawAxesLabels( step, roundMainMin + step, roundMainMax - step );
                    
        }
        //-----------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------
        //Установка размеров координатных осей
        public void SetArrowAxesSize( double minCoordinateValue, double maxCoordinateValue ) {
            this.arrowAxisX.Point1 = new System.Windows.Media.Media3D.Point3D( minCoordinateValue, 0, 0 );
            this.arrowAxisX.Point2 = new System.Windows.Media.Media3D.Point3D( maxCoordinateValue, 0, 0 );

            this.arrowAxisY.Point1 = new System.Windows.Media.Media3D.Point3D( 0, minCoordinateValue, 0 );
            this.arrowAxisY.Point2 = new System.Windows.Media.Media3D.Point3D( 0, maxCoordinateValue, 0 );

            this.arrowAxisZ.Point1 = new System.Windows.Media.Media3D.Point3D( 0, 0, minCoordinateValue );
            this.arrowAxisZ.Point2 = new System.Windows.Media.Media3D.Point3D( 0, 0, maxCoordinateValue );

            this.axisNameX.Position = new System.Windows.Media.Media3D.Point3D( maxCoordinateValue, 0, 0 );
            this.axisNameY.Position = new System.Windows.Media.Media3D.Point3D( 0, maxCoordinateValue, 0 );
            this.axisNameZ.Position = new System.Windows.Media.Media3D.Point3D( 0, 0, maxCoordinateValue );
        }
        //-----------------------------------------------------------------------------------------------------------
        public void DrawAxesLabels( double step, double minValue, double maxValue ) {
            double fontSize = 12;
            double margin = 10;
            double height = 40;
            for ( double value = minValue + step; value < maxValue; value += step ) {
                TextVisual3D valueLabelX = new TextVisual3D();
                TextVisual3D valueLabelY = new TextVisual3D();
                TextVisual3D valueLabelZ = new TextVisual3D();
                
                //valueLabelX.FontSize = valueLabelY.FontSize = valueLabelZ.FontSize = fontSize;
                //valueLabelX.Height = valueLabelY.Height = valueLabelZ.Height = 5;
                valueLabelX.Text = valueLabelY.Text = valueLabelZ.Text = value.ToString();
                
                valueLabelX.TextDirection = new Vector3D( 1, 0, 0 );
                valueLabelY.TextDirection = new Vector3D( 0, 1, 0 );
                valueLabelZ.TextDirection = new Vector3D( 0, 1, 0 );

                valueLabelX.Position = new System.Windows.Media.Media3D.Point3D( value, 0, -margin );
                valueLabelY.Position = new System.Windows.Media.Media3D.Point3D( 0, value, -margin );
                valueLabelZ.Position = new System.Windows.Media.Media3D.Point3D( 0, margin, value );

                this.helixViewPort3D.Children.Add( valueLabelX );
                if ( value != 0 ) {
                    this.helixViewPort3D.Children.Add( valueLabelY );
                    this.helixViewPort3D.Children.Add( valueLabelZ );
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        private void DrawAxesMarks( double step, double minValue, double maxValue ) {
            double markLength = 6;
            double halfMarkLength = markLength / 2;

            for ( double value = minValue + step; value < maxValue; value += step ) {
                LinesVisual3D valueMarkX = new LinesVisual3D();
                valueMarkX.Points = new List<System.Windows.Media.Media3D.Point3D>() {
                    new System.Windows.Media.Media3D.Point3D( value, 0, halfMarkLength ),
                    new System.Windows.Media.Media3D.Point3D( value, 0, -halfMarkLength ),
                };

                LinesVisual3D valueMarkY = new LinesVisual3D();
                valueMarkY.Points = new List<System.Windows.Media.Media3D.Point3D>() {
                    new System.Windows.Media.Media3D.Point3D( 0, value, halfMarkLength ),
                    new System.Windows.Media.Media3D.Point3D( 0, value, -halfMarkLength ),
                };

                LinesVisual3D valueMarkZ = new LinesVisual3D();
                valueMarkZ.Points = new List<System.Windows.Media.Media3D.Point3D>() {
                    new System.Windows.Media.Media3D.Point3D( 0, halfMarkLength, value ),
                    new System.Windows.Media.Media3D.Point3D( 0, -halfMarkLength, value ),
                };

                this.helixViewPort3D.Children.Add( valueMarkX );
                this.helixViewPort3D.Children.Add( valueMarkY );
                this.helixViewPort3D.Children.Add( valueMarkZ );
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        private void HelixViewPort3D_LookAtChanged( object sender, RoutedEventArgs e ) {
                
        }
        //-----------------------------------------------------------------------------------------------------------
    }
}
