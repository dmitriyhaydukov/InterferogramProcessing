using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using ExtraLibrary.Mathematics.Equations;
using ExtraLibrary.Mathematics.Numbers;

namespace ExtraLibrary.Geometry2D {
    //Окружность
    [Serializable]
    public class CircleDescriptor {
        private Point2D centre;
        private double radius;
        //---------------------------------------------------------------------------------------------
        //Конструктор
        public CircleDescriptor( Point2D centre, double radius ) {
            this.centre = centre;
            this.radius = radius;
        }
        //---------------------------------------------------------------------------------------------
        //Координаты x, соответствующие координате y
        public double[] GetCoordinatesX( double y ) {
            double a = 1;
            double b = -2 * this.centre.X;
            double c =
                ( y - this.centre.Y ) * ( y - this.centre.Y ) +
                this.centre.X * this.centre.X -
                radius * radius;
            QuadraticEquation quadraticEquation = new QuadraticEquation( a, b, c );
            Complex[] complexRoots = quadraticEquation.GetRoots();
            double[] realRoots = NumbersManager.GetRealParts( complexRoots );
            return realRoots;
        }
        //---------------------------------------------------------------------------------------------
        //Координаты y, соответствующие координате x
        public double[] GetCoordinatesY( double x ) {
            double a = 1;
            double b = -2 * this.centre.Y;
            double c =
                ( x - this.centre.X ) * ( x - this.centre.X ) +
                this.centre.Y * this.centre.Y -
                radius * radius;
            
            //Complex[] complexRoots = QuadraticEquation.GetRoots( a, b, c );
            Complex[] complexRoots = ComplexQuadraticEquation.GetRoots( a, b, c );
                        
            double[] realRoots = NumbersManager.GetRealParts( complexRoots );
            return realRoots;
        }
        //---------------------------------------------------------------------------------------------
        //Центр
        public Point2D Centre {
            get {
                return this.centre;
            }
            set {
                this.centre = value;
            }
        }
        //---------------------------------------------------------------------------------------------
        //Радиус
        public double Radius {
            get {
                return this.radius;
            }
            set {
                this.radius = value;        
            }
        }
        //---------------------------------------------------------------------------------------------
    }
}
