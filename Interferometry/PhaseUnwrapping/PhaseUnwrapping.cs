using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Algorithms;

namespace Interferometry.PhaseUnwrapping {
    //-------------------------------------------------------------------------------------
    //Алгоритм развертки фазы
    public class PhaseUnwrappingAlgorithm {
        RealMatrix phaseMatrix;
        UnwrappingAlgorithm unwrappingAlgorithm;
        //---------------------------------------------------------------------------------
        public PhaseUnwrappingAlgorithm( RealMatrix phaseMatrix ) {
            this.phaseMatrix = phaseMatrix;
            this.unwrappingAlgorithm = new UnwrappingAlgorithm();    
        }
        //---------------------------------------------------------------------------------
        //Развертка фазы по строкам
        public RealMatrix UnwrapByRows( int extremumIndex, UnwrapDirection unwrapDirection ) {
            double thresholdDifferenceValue = Math.PI;
            double increasingStep = 2 * Math.PI;
            RealMatrix resultMatrix = this.unwrappingAlgorithm.UnwrapMatrixByRows
                ( this.phaseMatrix, extremumIndex, unwrapDirection, thresholdDifferenceValue, increasingStep );
            return resultMatrix;
        }
        //---------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------
        //Развертка по строкам
        public static RealMatrix UnwrapByRows(
            RealMatrix phaseMatrix, int extremumIndx, UnwrapDirection unwrapDirection
        ) {
            PhaseUnwrappingAlgorithm phaseUnwrappingAlgorithm = new PhaseUnwrappingAlgorithm( phaseMatrix );
            RealMatrix resultMatrix = phaseUnwrappingAlgorithm.UnwrapByRows( extremumIndx, unwrapDirection );
            return resultMatrix;
        }
        //---------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------
    }
}
