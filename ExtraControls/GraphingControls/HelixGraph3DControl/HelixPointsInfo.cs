using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;

namespace ExtraControls {
    public class HelixPointsInfo {
        //------------------------------------------------------------------------------------------------
        public Point3D[] Points {
            get;
            set;
        }
        //------------------------------------------------------------------------------------------------
        public Color PointsColor {
            get;
            set;
        }
        //------------------------------------------------------------------------------------------------
        public double PointsSize {
            get;
            set;
        }
        //------------------------------------------------------------------------------------------------
        public HelixPointsInfo( Point3D[] points, Color pointsColor, double pointsSize ) {
            this.Points = points;
            this.PointsColor = pointsColor;
            this.PointsSize = pointsSize;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
