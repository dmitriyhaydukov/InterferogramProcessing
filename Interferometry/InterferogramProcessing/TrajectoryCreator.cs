using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Geometry2D;

namespace Interferometry.InterferogramProcessing {
    //Создатель траектории интерференционных сигналов в двух
    //произвольных точках интерферограмм
    class TrajectoryCreator {
        RealMatrix[] interferograms;
        //------------------------------------------------------------------------------------------
        public TrajectoryCreator( params RealMatrix[] interferograms ) {
            this.interferograms = interferograms;
        }
        //------------------------------------------------------------------------------------------
        //Получить траекторию
        public Curve2D GetTrajectory( Point pointOne, Point pointTwo) {
            int interferogramsCount = this.interferograms.Length;

            int pointOneCoordinateX = pointOne.X;
            int pointOneCoordinateY = pointOne.Y;

            int pointTwoCoordinateX = pointTwo.X;
            int pointTwoCoordinateY = pointTwo.Y;

            double[] intensitiesAtPointOne = new double[ interferogramsCount ];
            double[] intensitiesAtPointTwo = new double[ interferogramsCount ];

            for ( int index = 0; index < interferogramsCount; index++ ) {
                RealMatrix interferogram = interferograms[ index ];
                double intensityAtPointOne =
                    interferogram[ pointOneCoordinateY, pointOneCoordinateX ];
                double intensityAtPointTwo =
                    interferogram[ pointTwoCoordinateY, pointTwoCoordinateX ];
                intensitiesAtPointOne[ index ] = intensityAtPointOne;
                intensitiesAtPointTwo[ index ] = intensityAtPointTwo;
            }

            Curve2D trajectory = new Curve2D( intensitiesAtPointOne, intensitiesAtPointTwo );
            return trajectory;
        }
        //------------------------------------------------------------------------------------------
        //Получить траекторию
        public static Curve2D GetTrajectory(
            Point pointOne, Point pointTwo,
            params RealMatrix[] interferograms
        ) {
            TrajectoryCreator trajectoryCreator = new TrajectoryCreator( interferograms );
            Curve2D trajectory = trajectoryCreator.GetTrajectory( pointOne, pointTwo );
            return trajectory;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

    }
}
