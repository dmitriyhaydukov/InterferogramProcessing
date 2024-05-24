using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ExtraControls;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

namespace UserInterfaceHelping {
    
    public class UserInterfaceHelper {
        //---------------------------------------------------------------------------------------------------------
        //Отображение изображения в окне
        public static void ShowImageInWindow( ImageSource imageSource ) {
            ImageWindow imageWindow = new ImageWindow();
            imageWindow.ImageSource = imageSource;
            imageWindow.Show();
            imageWindow.Activate();
        }
        //---------------------------------------------------------------------------------------------------------
        //Отображение графиков в окне
        public static void ShowSwordfishGraphInWindow( IList<GraphInfo> graphInfoCollection ) {
            SwordfishGraphWindow swordfishGraphWindow = new SwordfishGraphWindow();
            swordfishGraphWindow.GraphInfoCollection = graphInfoCollection;
            swordfishGraphWindow.Show();
            swordfishGraphWindow.Activate();
        }
        //---------------------------------------------------------------------------------------------------------
        //Отображение графиков в окне
        public static void ShowZedGraphInWindow( IList<ZedGraphInfo> zedGraphsInfo, AxesInfo axesInfo ) {
            ZedGraphWindow zedGraphWindow = new ZedGraphWindow();
            zedGraphWindow.ZedGraphsInfo = zedGraphsInfo;
            zedGraphWindow.AxesInfo = axesInfo;
            zedGraphWindow.Show();
            zedGraphWindow.Activate();
        }
        //---------------------------------------------------------------------------------------------------------
        //Отображение 3D-графика в окне
        public static void ShowGraph3DInWindow( IList<HelixPointsInfo> pointsInfoCollection, IList<HelixGridLinesInfo> gridLinesInfoCollection) {
            HelixGraph3DWindow helixGraph3DWindow = new HelixGraph3DWindow();
                       
            for (int index = 0; index < pointsInfoCollection.Count; index++)
            {
                HelixPointsInfo pointsInfo = pointsInfoCollection[index];
                helixGraph3DWindow.AddPointsInfo(pointsInfo);
            }
            //helixGraph3DWindow.ExecuteAutotuning();
            
            if (gridLinesInfoCollection != null)
            {
                for (int index = 0; index < gridLinesInfoCollection.Count; index++)
                {
                    HelixGridLinesInfo gridLinesInfo = gridLinesInfoCollection[index];
                    helixGraph3DWindow.AddGridLinesInfo(gridLinesInfo);
                }
            }

            helixGraph3DWindow.ExecuteAutotuning();

            helixGraph3DWindow.Show();
            helixGraph3DWindow.Activate();
        }
        //---------------------------------------------------------------------------------------------------------
        /*
        public static void ShowGraph3DInWindow(
            IList<HelixPointsInfo> pointsInfoCollection,
            IList<HelixGridLinesInfo> gridLinesInfoCollection
        ) {
            HelixGraph3DWindow helixGraph3DWindow = new HelixGraph3DWindow();

            for ( int index = 0; index < pointsInfoCollection.Count; index++ ) {
                HelixPointsInfo pointsInfo = pointsInfoCollection[ index ];
                helixGraph3DWindow.AddPointsInfo( pointsInfo );
            }

            for ( int index = 0; index < gridLinesInfoCollection.Count; index++ ) {
                HelixGridLinesInfo gridLinesInfo = gridLinesInfoCollection[ index ];
                helixGraph3DWindow.AddGridLinesInfo( gridLinesInfo );
            }
            
            helixGraph3DWindow.ExecuteAutotuning();
            helixGraph3DWindow.Show();
            helixGraph3DWindow.Activate();
        }
        */
        //---------------------------------------------------------------------------------------------------------
        public static void ShowPairGraph3DInWindow(
            IList<HelixPointsInfo> pointsInfoCollectionOne,
            IList<HelixPointsInfo> pointsInfoCollectionTwo
        ) {
            PairHelixGraph3DWindow pairHelixGraph3DWindow = new PairHelixGraph3DWindow();

            for ( int index = 0; index < pointsInfoCollectionOne.Count; index++ ) {
                HelixPointsInfo pointsInfo = pointsInfoCollectionOne[ index ];
                pairHelixGraph3DWindow.AddPointsInfoToLeftGraph( pointsInfo );
            }

            for ( int index = 0; index < pointsInfoCollectionTwo.Count; index++ ) {
                HelixPointsInfo pointsInfo = pointsInfoCollectionTwo[ index ];
                pairHelixGraph3DWindow.AddPointsInfoToRightGraph( pointsInfo );
            }
                        
            pairHelixGraph3DWindow.ExecuteAutotuning();
            pairHelixGraph3DWindow.Show();
            pairHelixGraph3DWindow.Activate();
        }
        //---------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------
    }
    
}
