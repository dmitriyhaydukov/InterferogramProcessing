using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

namespace ExtraLibrary.Geometry2D {
    //Система координат на плоскости
    public class PlaneCoordinateSystem {
        //-------------------------------------------------------------------------------------------
        public double SizeX {
            get;
            set;
        }
        //-------------------------------------------------------------------------------------------
        public double SizeY {
            get;
            set;
        }
        //-------------------------------------------------------------------------------------------
        public PlaneCoordinateSystem( double sizeX, double sizeY ) {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
        }
        //-------------------------------------------------------------------------------------------
        //Координата X в системе координат экрана
        public double GetScreenCoordinateX( double cartesianCoordinateX ) {
            double screenCoordinateX = this.SizeX / 2 + cartesianCoordinateX;
            return screenCoordinateX;
        }
        //-------------------------------------------------------------------------------------------
        //Координата Y в системе координат экрана
        public double GetScreenCoordinateY( double cartesianCoordinateY ) {
            double screenCoordinateY = this.SizeY / 2 - cartesianCoordinateY;
            return screenCoordinateY;
        }
        //-------------------------------------------------------------------------------------------
        //Координата X в декартовой системе координат
        public double GetCartesianCoordinateX( double screenCoordinateX ) {
            double cartesianCoordinateX = screenCoordinateX - this.SizeX / 2;
            return cartesianCoordinateX;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Координата Y в декартовой системе координат
        public double GetCartesianCoordinateY( double screenCoordinateY ) {
            double cartesianCoordinateY = this.SizeY / 2 - screenCoordinateY;
            return cartesianCoordinateY;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Прямоугольник, симметричный данному, в декартовой системе координат
        public Rect GetSymmetricRectangleInCartesianCoordinateSystem( Rect rectangleInCartesianCoordinateSystem ) {
            double endX = 
                rectangleInCartesianCoordinateSystem.X + rectangleInCartesianCoordinateSystem.Width;
            double endY = 
                rectangleInCartesianCoordinateSystem.Y - rectangleInCartesianCoordinateSystem.Height;
            Rect newRectangle = new Rect(
                -endX, -endY,
                rectangleInCartesianCoordinateSystem.Width,
                rectangleInCartesianCoordinateSystem.Height
            );
            return newRectangle;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Прямоугольник, симметричный данному, в системе координат экрана
        public Rect GetSymmetricRectangleInScreenCoordinateSystem( Rect rectangleInScreenCoordinateSystem ) {
            
            Rect rectangleInCartesianCoordinateSystem = new Rect(
                this.GetCartesianCoordinateX( rectangleInScreenCoordinateSystem.X ),
                this.GetCartesianCoordinateY( rectangleInScreenCoordinateSystem.Y ),
                rectangleInScreenCoordinateSystem.Width,
                rectangleInScreenCoordinateSystem.Height
            );

            Rect symmetricRectangleInCartesianCoordinateSystem =
                this.GetSymmetricRectangleInCartesianCoordinateSystem
                ( rectangleInCartesianCoordinateSystem );
            
            Rect symmetricRectangleInScreenCoordinateSystem = new Rect(
                this.GetScreenCoordinateX( symmetricRectangleInCartesianCoordinateSystem.X ),
                this.GetScreenCoordinateY( symmetricRectangleInCartesianCoordinateSystem.Y ),
                symmetricRectangleInCartesianCoordinateSystem.Width,
                symmetricRectangleInCartesianCoordinateSystem.Height
            );

            return symmetricRectangleInScreenCoordinateSystem;
        }
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
    }
}
