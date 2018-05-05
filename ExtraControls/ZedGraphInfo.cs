using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ExtraLibrary.Geometry2D;
using ZedGraph;

namespace ExtraControls {
    public class ZedGraphInfo : GraphInfo {
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        public SymbolType ZedGraphSymbolType {
            get;
            set;
        }
        //-----------------------------------------------------------------------------------------
        public int ZedGraphSymbolSize {
            get;
            set;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        public ZedGraphInfo(
            string graphName, Color graphColor, Point2D[] graphPoints, bool lineVisibility,
            SymbolType zedGraphSymbolType, int zedGraphSymbolSize
        ) : base ( graphName, graphColor, graphPoints, lineVisibility ) {
            this.ZedGraphSymbolType = zedGraphSymbolType;
            this.ZedGraphSymbolSize = zedGraphSymbolSize;
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
