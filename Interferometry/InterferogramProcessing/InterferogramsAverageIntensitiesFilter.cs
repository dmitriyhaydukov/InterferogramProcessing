using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Interferometry;
using Interferometry.Helpers;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Geometry3D;

namespace Interferometry.InterferogramProcessing {
    public class InterferogramsAverageIntensitiesFilter {
        //----------------------------------------------------------------------------------------------------------
        public RealMatrix[] GetFilteredInterferograms( 
            int windowSize, params RealMatrix[] interferograms
        ) {
            RealMatrix firstInterferogram = interferograms[ 0 ];
            int width = firstInterferogram.ColumnCount;
            int height = firstInterferogram.RowCount;

            int newWidth = width - windowSize + 1;
            int newHeight = height - windowSize + 1;
            RealMatrix[] newMatrices = MatricesManager.CreateMatrices( newHeight, newWidth, 3 );

            int halfSize = windowSize / 2;
            
            int startX = halfSize;
            int startY = halfSize;

            int finishX = width - halfSize - 1;
            int finishY = height - halfSize - 1;

            for ( int x = startX, newX = 0; x <= finishX; x++, newX++ ) {
                for ( int y = startY, newY = 0; y <= finishY; y++, newY++ ) {
                    Rectangle rectangle = new Rectangle( x - halfSize, y - halfSize, windowSize, windowSize );
                    Point3D[] points =
                        InterferometryHelper.GetSpatialPointsFromInterferograms( interferograms, rectangle );
                    Point3D filteredPoint = SpaceManager.GetMidPoint( points );
                    newMatrices[ 0 ][ newY, newX ] = filteredPoint.X;
                    newMatrices[ 1 ][ newY, newX ] = filteredPoint.Y;
                    newMatrices[ 2 ][ newY, newX ] = filteredPoint.Z;
                }
            }
            return newMatrices;
        }
        //----------------------------------------------------------------------------------------------------------
        private Point3D GetPointByMinimalDistancesToPoints( Point3D[] points ) {
            Point3D point = new Point3D();
            return point;
        }
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------
    }
}
