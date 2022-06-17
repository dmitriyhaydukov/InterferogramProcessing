#region namespeces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Numerics;

using ExtraControls;
using ExtraMVVM;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.OS;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Arraying.ArrayCreation;
using ExtraLibrary.Converting;
using ExtraLibrary.Mathematics;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Progressions;
using ExtraLibrary.Mathematics.Approximation;
using ExtraLibrary.Mathematics.Numbers;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.Mathematics.Filtering;
using ExtraLibrary.Randomness;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;
using ExtraLibrary.Collections;
using ExtraLibrary.Algorithms;

using Interferometry.InterferogramProcessing;
using Interferometry.InterferogramCreation;
using Interferometry.InterferogramDecoding;
using Interferometry.DeviceControllers;
using Interferometry.Helpers;
using Interferometry.PhaseUnwrapping;

using UserInterfaceHelping;

using ZedGraph;

using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using InterferogramProcessing.Helpers;

//using MathWorks.MATLAB.NET.Arrays;
using WPFColorPickerLib;

#endregion

namespace InterferogramProcessing {
    //--------------------------------------------------------------------------------------------------
    public class MainViewModel : NotificationObject {
        #region data
        ImagesViewModel imagesViewModel = new ImagesViewModel();
        
        private BitmapSource activeImage = null;
        
        private BitmapSource mainLeftImage;
        private BitmapSource mainRightImage;

        public RealMatrix mainRightMatrix;
        public RealMatrix mainLeftMatrix;

        private BitmapSource stencilImage;
        private bool? useStencilImage;
        BitMask2D stencilBitMask;       

        private bool? comparisonMode;       //Режим сравнения (графики)

        private IList<GraphInfo> graphInfoCollection;

        private GraphShowingMode graphShowingMode = GraphShowingMode.GrayScale;
        private ImagesViewingMode imagesViewingMode = ImagesViewingMode.Undefined;
        private int imageGraphRow;

        private DecodingAlgorithm decodingAlgorithm = DecodingAlgorithm.ThreePointAlgorithm;

        //-----------------------------------------------------------------------------------
        private DecodingManager decodingManager = new DecodingManager();
        private ProcessingManager processingManager = new ProcessingManager();
        //-----------------------------------------------------------------------------------

        RestrictedCapacityList<ExtraLibrary.Geometry3D.Point3D[]> lastSpatialPoints =
            new RestrictedCapacityList<ExtraLibrary.Geometry3D.Point3D[]>( 2 );
        
        //-----------------------------------------------------------------------------------
        ComplexMatrix lastFourierTransform2D;   //Последнее преобразование Фурье
        ComplexMatrix[] fourierTransforms2D;    //Преобразования Фурье

        Complex[] fourierTransformValuesForGraph;
        //-----------------------------------------------------------------------------------        
        RestrictedCapacityList<System.Drawing.Point> fourierTransformFilterPoints =
            new RestrictedCapacityList<System.Drawing.Point>( 2 );
        //-----------------------------------------------------------------------------------
        
        private string rootMeanSquareErrorText;
        //-----------------------------------------------------------------------------------
        private Point2D[] EllipsePoints2D;
        private Point2D[] TrajectoryPoints;
        private double[] IntensitiesForPointOne;

        //-----------------------------------------------------------------------------------
        private System.Drawing.Point lastClickedPoint;
        private System.Windows.Media.Color lastClickedColor;
        //-----------------------------------------------------------------------------------
        private Dictionary<System.Windows.Media.Color, int> colorNumbers =
            new Dictionary<System.Windows.Media.Color,int>();
        
        private WriteableBitmap unwrappingTemplateImage;
        
        #endregion

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public MainViewModel() {
            this.InitializeData();
        }
        //--------------------------------------------------------------------------------------------------
        public void InitializeData() {
            this.imagesViewModel.InitializeData();

            this.useStencilImage = false;
            this.comparisonMode = false;
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        #region Commands
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> decodeCommand;
        public ICommand DecodeCommand {
            get {
                if ( this.decodeCommand == null ) {
                    this.decodeCommand = new DelegateCommand<object>
                        ( this.Decode, this.CanAlwaysPerformOperation );
                }
                return this.decodeCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getImageFromCameraCommand;
        public ICommand GetImageFromCameraCommand {
            get {
                if ( this.getImageFromCameraCommand == null ) {
                    this.getImageFromCameraCommand = new DelegateCommand<object>
                        ( this.GetImageFromCamera, this.CanAlwaysPerformOperation );
                }
                return this.getImageFromCameraCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> cameraManagementCommand;
        public ICommand CameraManagementCommand {
            get {
                if ( this.cameraManagementCommand == null ) {
                    this.cameraManagementCommand = new DelegateCommand<object>
                        ( this.CameraManagement, this.CanAlwaysPerformOperation );
                }
                return this.cameraManagementCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> motorManagementCommand;
        public ICommand MotorManagementCommand {
            get {
                if ( this.motorManagementCommand == null ) {
                    this.motorManagementCommand = new DelegateCommand<object>
                        ( this.MotorManagement, this.CanAlwaysPerformOperation );
                }
                return this.motorManagementCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> addRightImageToImagesListCommand;
        public ICommand AddRightImageToImagesListCommand {
            get {
                if ( this.addRightImageToImagesListCommand == null ) {
                    this.addRightImageToImagesListCommand = new DelegateCommand<object>
                        ( this.AddResultImageToImagesList, this.CanAlwaysPerformOperation );
                }
                return this.addRightImageToImagesListCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> leftImageToRightImageCommand;
        public ICommand LeftImageToRightImageCommand {
            get {
                if ( this.leftImageToRightImageCommand == null ) {
                    this.leftImageToRightImageCommand = new DelegateCommand<object>
                        ( this.LeftImageToRightImage, this.CanAlwaysPerformOperation );
                }
                return this.leftImageToRightImageCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showMainImageInWindowCommand;
        public ICommand ShowMainImageInWindowCommand {
            get {
                if ( this.showMainImageInWindowCommand == null ) {
                    this.showMainImageInWindowCommand = new DelegateCommand<object>
                        ( this.ShowMainImageInWindow, this.CanAlwaysPerformOperation );
                }
                return this.showMainImageInWindowCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> loadStencilImageCommand;
        public ICommand LoadStencilImageCommand {
            get {
                if ( this.loadStencilImageCommand == null ) {
                    this.loadStencilImageCommand = new DelegateCommand<object>
                        ( this.LoadStencilImage, this.CanAlwaysPerformOperation );
                }
                return this.loadStencilImageCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showNumeratorsDenominatorsGraphCommand;
        public ICommand ShowNumeratorsDenominatorsGraphCommand {
            get {
                if ( this.showNumeratorsDenominatorsGraphCommand == null ) {
                    this.showNumeratorsDenominatorsGraphCommand = new DelegateCommand<object>
                        ( this.ShowNumeratorsDenominatorsGraph, this.CanAlwaysPerformOperation );
                }
                return this.showNumeratorsDenominatorsGraphCommand;
            }
        }

        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> generateImagesCommand;
        public ICommand GenerateImagesCommand {
            get {
                if ( this.generateImagesCommand == null ) {
                    this.generateImagesCommand = new DelegateCommand<object>
                        ( this.GenerateInterferograms, this.CanAlwaysPerformOperation );
                }
                return this.generateImagesCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> gammaCorrectionCommand;
        public ICommand GammaCorrectionCommand {
            get {
                if ( this.gammaCorrectionCommand == null ) {
                    this.gammaCorrectionCommand = new DelegateCommand<object>
                        ( this.GammaCorrection, this.CanAlwaysPerformOperation );
                }
                return this.gammaCorrectionCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showAutoGammaCorrectionGraphCommand;
        public ICommand ShowAutoGammaCorrectionGraphCommand {
            get {
                if ( this.showAutoGammaCorrectionGraphCommand == null ) {
                    this.showAutoGammaCorrectionGraphCommand = new DelegateCommand<object>
                        ( this.ShowAutoGammaCorrectionGraph, this.CanAlwaysPerformOperation );
                }
                return this.showAutoGammaCorrectionGraphCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showIntensitiesTrajectoryInSpaceCommand;
        public ICommand ShowIntensitiesTrajectoryInSpaceCommand {
            get {
                if ( this.showIntensitiesTrajectoryInSpaceCommand == null ) {
                    this.showIntensitiesTrajectoryInSpaceCommand = new DelegateCommand<object>
                        ( this.ShowIntensitiesTrajectoryInSpace, this.CanAlwaysPerformOperation );
                }
                return this.showIntensitiesTrajectoryInSpaceCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showRowIntensitiesTrajectoryInSpaceCommand;
        public ICommand ShowRowIntensitiesTrajectoryInSpaceCommand {
            get {
                if ( this.showRowIntensitiesTrajectoryInSpaceCommand == null ) {
                    this.showRowIntensitiesTrajectoryInSpaceCommand = new DelegateCommand<object>
                        ( this.ShowRowIntensitiesTrajectoryInSpace, this.CanAlwaysPerformOperation );
                }
                return this.showRowIntensitiesTrajectoryInSpaceCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> transformImagesToGrayScaleImagesCommand;
        public ICommand TransformImagesToGrayScaleImagesCommand {
            get {
                if ( this.transformImagesToGrayScaleImagesCommand == null ) {
                    this.transformImagesToGrayScaleImagesCommand = new DelegateCommand<object>
                        ( this.TransformImagesToGrayScaleImages, this.CanAlwaysPerformOperation );
                }
                return this.transformImagesToGrayScaleImagesCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> normalizeImagesCommand;
        public ICommand NormalizeImagesCommand {
            get {
                if ( this.normalizeImagesCommand == null ) {
                    this.normalizeImagesCommand = new DelegateCommand<object>
                        ( this.NormalizeImages, this.CanAlwaysPerformOperation );
                }
                return this.normalizeImagesCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> saveGraphControlImageToFileCommand;
        public ICommand SaveGraphControlImageToFileCommand {
            get {
                if ( this.saveGraphControlImageToFileCommand == null ) {
                    this.saveGraphControlImageToFileCommand = new DelegateCommand<object>
                        ( this.SaveGraphControlImageToFile, this.CanAlwaysPerformOperation );
                }
                return this.saveGraphControlImageToFileCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showPairLastPointsInSpaceCommand;
        public ICommand ShowPairLastPointsInSpaceCommand {
            get {
                if ( this.showPairLastPointsInSpaceCommand == null ) {
                    this.showPairLastPointsInSpaceCommand = new DelegateCommand<object>
                        ( this.ShowPairLastPointsInSpace, this.CanAlwaysPerformOperation );
                }
                return this.showPairLastPointsInSpaceCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showCylinderDecodingAlgorithmInfoCommand;
        public ICommand ShowCylinderDecodingAlgorithmInfoCommand {
            get {
                if ( this.showCylinderDecodingAlgorithmInfoCommand == null ) {
                    this.showCylinderDecodingAlgorithmInfoCommand = new DelegateCommand<object>
                        ( this.ShowCylinderDecodingAlgorithmInfo, this.CanAlwaysPerformOperation );
                }
                return this.showCylinderDecodingAlgorithmInfoCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> testCommand;
        public ICommand TestCommand {
            get {
                if ( this.testCommand == null ) {
                    this.testCommand = new DelegateCommand<object>
                        ( this.Test, this.CanAlwaysPerformOperation );
                }
                return this.testCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showIntensitiesTrajectoryWithApproximatedPlaneInSpaceCommand;
        public ICommand ShowIntensitiesTrajectoryWithApproximatedPlaneInSpaceCommand {
            get {
                if ( this.showIntensitiesTrajectoryWithApproximatedPlaneInSpaceCommand == null ) {
                    this.showIntensitiesTrajectoryWithApproximatedPlaneInSpaceCommand = new DelegateCommand<object>
                        ( this.ShowIntensitiesTrajectoryWithApproximatedPlaneInSpace, this.CanAlwaysPerformOperation );
                }
                return this.showIntensitiesTrajectoryWithApproximatedPlaneInSpaceCommand;
            }
        }
        
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getFelteredAverageIntensitiesInterferogramsCommand;
        public ICommand GetFelteredAverageIntensitiesInterferogramsCommand {
            get {
                if ( this.getFelteredAverageIntensitiesInterferogramsCommand == null ) {
                    this.getFelteredAverageIntensitiesInterferogramsCommand = new DelegateCommand<object>
                        ( this.GetFelteredAverageIntensitiesInterferograms, this.CanAlwaysPerformOperation );
                }
                return this.getFelteredAverageIntensitiesInterferogramsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getPhaseImageCommand;
        public ICommand GetPhaseImageCommand {
            get {
                if ( this.getPhaseImageCommand == null ) {
                    this.getPhaseImageCommand = new DelegateCommand<object>
                        ( this.GetPhaseImage, this.CanAlwaysPerformOperation );
                }
                return this.getPhaseImageCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> cropImagesBoundsCommand;
        public ICommand CropImagesBoundsCommand {
            get {
                if ( this.cropImagesBoundsCommand == null ) {
                    this.cropImagesBoundsCommand = new DelegateCommand<object>
                        ( this.CropImagesBounds, this.CanAlwaysPerformOperation );
                }
                return this.cropImagesBoundsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> filterImagesByMaskCommand;
        public ICommand FilterImagesByMaskCommand {
            get {
                if ( this.filterImagesByMaskCommand == null ) {
                    this.filterImagesByMaskCommand = new DelegateCommand<object>
                        ( this.FilterImagesByMask, this.CanAlwaysPerformOperation );
                }
                return this.filterImagesByMaskCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> enchanceEdgesCommand;
        public ICommand EnchanceEdgesCommand
        {
            get
            {
                if (this.enchanceEdgesCommand == null)
                {
                    this.enchanceEdgesCommand = new DelegateCommand<object>
                        (this.EnchanceEdges, this.CanAlwaysPerformOperation);
                }
                return this.enchanceEdgesCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> substractImagesCommand;
        public ICommand SubstractImagesCommand {
            get {
                if ( this.substractImagesCommand == null ) {
                    this.substractImagesCommand = new DelegateCommand<object>
                        ( this.SubstractImages, this.CanAlwaysPerformOperation );
                }
                return this.substractImagesCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showSubstractGraphCommand;
        public ICommand ShowSubstractGraphCommand {
            get {
                if ( this.showSubstractGraphCommand == null ) {
                    this.showSubstractGraphCommand = new DelegateCommand<object>
                        ( this.ShowSubstractGraph, this.CanAlwaysPerformOperation );
                }
                return this.showSubstractGraphCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showGraphsForSelectedImagesCommand;
        public ICommand ShowGraphsForSelectedImagesCommand {
            get {
                if ( this.showGraphsForSelectedImagesCommand == null ) {
                    this.showGraphsForSelectedImagesCommand = new DelegateCommand<object>
                        ( this.ShowGraphsForSelectedImages, this.CanAlwaysPerformOperation );
                }
                return this.showGraphsForSelectedImagesCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> advancedImagesSubstractingCommand;
        public ICommand AdvancedImagesSubstractingCommand {
            get {
                if ( this.advancedImagesSubstractingCommand == null ) {
                    this.advancedImagesSubstractingCommand = new DelegateCommand<object>
                        ( this.AdvancedImagesSubstracting, this.CanAlwaysPerformOperation );
                }
                return this.advancedImagesSubstractingCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getFourierTransformSpectrumCommand;
        public ICommand GetFourierTransformSpectrumCommand {
            get {
                if ( this.getFourierTransformSpectrumCommand == null ) {
                    this.getFourierTransformSpectrumCommand = new DelegateCommand<object>
                        ( this.GetFourierTransformSpectrum, this.CanAlwaysPerformOperation );
                }
                return this.getFourierTransformSpectrumCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getInverseFourierTransformCommand;
        public ICommand GetInverseFourierTransformCommand {
            get {
                if ( this.getInverseFourierTransformCommand == null ) {
                    this.getInverseFourierTransformCommand = new DelegateCommand<object>
                        ( this.GetInverseFourierTransform, this.CanAlwaysPerformOperation );
                }
                return this.getInverseFourierTransformCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> cropImagesToSizeAtPowerOfTwoCommand;
        public ICommand CropImagesToSizeAtPowerOfTwoCommand {
            get {
                if ( this.cropImagesToSizeAtPowerOfTwoCommand == null ) {
                    this.cropImagesToSizeAtPowerOfTwoCommand = new DelegateCommand<object>
                        ( this.CropImagesToSizeAtPowerOfTwo, this.CanAlwaysPerformOperation );
                }
                return this.cropImagesToSizeAtPowerOfTwoCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> filterFourierTransformByRemovingCommand;
        public ICommand FilterFourierTransformByRemovingCommand {
            get {
                if ( this.filterFourierTransformByRemovingCommand == null ) {
                    this.filterFourierTransformByRemovingCommand = new DelegateCommand<object>
                        ( this.FilterFourierTransformByRemoving, this.CanAlwaysPerformOperation );
                }
                return this.filterFourierTransformByRemovingCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> filterFourierTransformBySelectingCommand;
        public ICommand FilterFourierTransformBySelectingCommand {
            get {
                if ( this.filterFourierTransformBySelectingCommand == null ) {
                    this.filterFourierTransformBySelectingCommand = new DelegateCommand<object>
                        ( this.FilterFourierTransformBySelecting, this.CanAlwaysPerformOperation );
                }
                return this.filterFourierTransformBySelectingCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getFourierTransformSpectrumsCommand;
        public ICommand GetFourierTransformSpectrumsCommand {
            get {
                if ( this.getFourierTransformSpectrumsCommand == null ) {
                    this.getFourierTransformSpectrumsCommand = new DelegateCommand<object>
                        ( this.GetFourierTransformSpectrums, this.CanAlwaysPerformOperation );
                }
                return this.getFourierTransformSpectrumsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getFourierTransformForCurrentGraphCommand;
        public ICommand GetFourierTransformForCurrentGraphCommand {
            get {
                if ( this.getFourierTransformForCurrentGraphCommand == null ) {
                    this.getFourierTransformForCurrentGraphCommand = new DelegateCommand<object>
                        ( this.GetFourierTransformForCurrentGraph, this.CanAlwaysPerformOperation );
                }
                return this.getFourierTransformForCurrentGraphCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> unwrapLastPhaseMatrixByRowsCommand;
        public ICommand UnwrapLastPhaseMatrixByRowsCommand {
            get {
                if ( this.unwrapLastPhaseMatrixByRowsCommand == null ) {
                    this.unwrapLastPhaseMatrixByRowsCommand = new DelegateCommand<object>
                        ( this.UnwrapLastPhaseMatrixByRows, this.CanAlwaysPerformOperation );
                }
                return this.unwrapLastPhaseMatrixByRowsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> filterImagesByStepCommand;
        public ICommand FilterImagesByStepCommand {
            get {
                if ( this.filterImagesByStepCommand == null ) {
                    this.filterImagesByStepCommand = new DelegateCommand<object>
                        ( this.FilterImagesByStep, this.CanAlwaysPerformOperation );
                }
                return this.filterImagesByStepCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> filterImagesByMedianCommand;
        public ICommand FilterImagesByMedianCommand
        {
            get
            {
                if (this.filterImagesByMedianCommand == null)
                {
                    this.filterImagesByMedianCommand = new DelegateCommand<object>
                        (this.FilterImagesByMedian, this.CanAlwaysPerformOperation);
                }
                return this.filterImagesByMedianCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> filterFourierTransformsBySelectingCommand;
        public ICommand FilterFourierTransformsBySelectingCommand {
            get {
                if ( this.filterFourierTransformsBySelectingCommand == null ) {
                    this.filterFourierTransformsBySelectingCommand = new DelegateCommand<object>
                        ( this.FilterFourierTransformsBySelecting, this.CanAlwaysPerformOperation );
                }
                return this.filterFourierTransformsBySelectingCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        
        private DelegateCommand<object> getInverseFourierTransformsCommand;
        public ICommand GetInverseFourierTransformsCommand {
            get {
                if ( this.getInverseFourierTransformsCommand == null ) {
                    this.getInverseFourierTransformsCommand = new DelegateCommand<object>
                        ( this.GetInverseFourierTransforms, this.CanAlwaysPerformOperation );
                }
                return this.getInverseFourierTransformsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> unwrapPhaseMatrixByRowsCommand;
        public ICommand UnwrapPhaseMatrixByRowsCommand {
            get {
                if ( this.unwrapPhaseMatrixByRowsCommand == null ) {
                    this.unwrapPhaseMatrixByRowsCommand = new DelegateCommand<object>
                        ( this.UnwrapPhaseMatrixByRows, this.CanAlwaysPerformOperation );
                }
                return this.unwrapPhaseMatrixByRowsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> unwrapPhaseMatrixByColumnsCommand;
        public ICommand UnwrapPhaseMatrixByColumnsCommand {
            get {
                if ( this.unwrapPhaseMatrixByColumnsCommand == null ) {
                    this.unwrapPhaseMatrixByColumnsCommand = new DelegateCommand<object>
                        ( this.UnwrapPhaseMatrixByColumns, this.CanAlwaysPerformOperation );
                }
                return this.unwrapPhaseMatrixByColumnsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> unwrapPhaseMatrixByTemplateImageCommand;
        public ICommand UnwrapPhaseMatrixByTemplateImageCommand {
            get {
                if ( this.unwrapPhaseMatrixByTemplateImageCommand == null ) {
                    this.unwrapPhaseMatrixByTemplateImageCommand = new DelegateCommand<object>
                        ( this.UnwrapPhaseMatrixByTemplateImage, this.CanAlwaysPerformOperation );
                }
                return this.unwrapPhaseMatrixByTemplateImageCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
                
        private DelegateCommand<object> showMatlabGraph3DCommand;
        public ICommand ShowMatlabGraph3DCommand {
            get {
                if ( this.showMatlabGraph3DCommand == null ) {
                    this.showMatlabGraph3DCommand = new DelegateCommand<object>
                        ( this.ShowMatlabGraph3D, this.CanAlwaysPerformOperation );
                }
                return this.showMatlabGraph3DCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
               
        private DelegateCommand<object> substractPhasesCommand;
        public ICommand SubstractPhasesCommand {
            get {
                if ( this.substractPhasesCommand == null ) {
                    this.substractPhasesCommand = new DelegateCommand<object>
                        ( this.SubstractPhases, this.CanAlwaysPerformOperation );
                }
                return this.substractPhasesCommand;
            }
        }    
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showPhaseMatrixMatlabGraph3DCommand;
        public ICommand ShowPhaseMatrixMatlabGraph3DCommand {
            get {
                if ( this.showPhaseMatrixMatlabGraph3DCommand == null ) {
                    this.showPhaseMatrixMatlabGraph3DCommand = new DelegateCommand<object>
                        ( this.ShowPhaseMatrixMatlabGraph3D, this.CanAlwaysPerformOperation );
                }
                return this.showPhaseMatrixMatlabGraph3DCommand;
            }
        }    
        
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> calculateRootMeanSquareErrorForGraphsCommand;
        public ICommand CalculateRootMeanSquareErrorForGraphsCommand {
            get {
                if ( this.calculateRootMeanSquareErrorForGraphsCommand == null ) {
                    this.calculateRootMeanSquareErrorForGraphsCommand = new DelegateCommand<object>
                        ( this.CalculateRootMeanSquareErrorForGraphs, this.CanAlwaysPerformOperation );
                }
                return this.calculateRootMeanSquareErrorForGraphsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getInversePhaseCommand;
        public ICommand GetInversePhaseCommand {
            get {
                if ( this.getInversePhaseCommand == null ) {
                    this.getInversePhaseCommand = new DelegateCommand<object>
                        ( this.GetInversePhase, this.CanAlwaysPerformOperation );
                }
                return this.getInversePhaseCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> calculateRootMeanSquareErrorForMatricesCommand;
        public ICommand CalculateRootMeanSquareErrorForMatricesCommand {
            get {
                if ( this.calculateRootMeanSquareErrorForMatricesCommand == null ) {
                    this.calculateRootMeanSquareErrorForMatricesCommand = new DelegateCommand<object>
                        ( this.CalculateRootMeanSquareErrorForMatrices, this.CanAlwaysPerformOperation );
                }
                return this.calculateRootMeanSquareErrorForMatricesCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> calculatePhaseShiftsCommand;
        public ICommand CalculatePhaseShiftsCommand {
            get {
                if ( this.calculatePhaseShiftsCommand == null ) {
                    this.calculatePhaseShiftsCommand = new DelegateCommand<object>
                        ( this.CalculatePhaseShifts, this.CanAlwaysPerformOperation );
                }
                return this.calculatePhaseShiftsCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showPhaseShiftsCalculatingEllipsePointsCommand;
        public ICommand ShowPhaseShiftsCalculatingEllipsePointsCommand {
            get {
                if ( this.showPhaseShiftsCalculatingEllipsePointsCommand == null ) {
                    this.showPhaseShiftsCalculatingEllipsePointsCommand = new DelegateCommand<object>
                        ( this.ShowPhaseShiftsCalculatingEllipsePoints, this.CanAlwaysPerformOperation );
                }
                return this.showPhaseShiftsCalculatingEllipsePointsCommand;
            }
        }
                
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> showIntensitiesForPointCommand;
        public ICommand ShowIntensitiesForPointCommand {
            get {
                if ( this.showIntensitiesForPointCommand == null ) {
                    this.showIntensitiesForPointCommand = new DelegateCommand<object>
                        ( this.ShowIntensitiesForPoint, this.CanAlwaysPerformOperation );
                }
                return this.showIntensitiesForPointCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> getInverseFourierTransformForCurrentGraphCommand;
        public ICommand GetInverseFourierTransformForCurrentGraphCommand {
            get {
                if ( this.getInverseFourierTransformForCurrentGraphCommand == null ) {
                    this.getInverseFourierTransformForCurrentGraphCommand = new DelegateCommand<object>
                        ( this.GetInverseFourierTransformForCurrentGraph, this.CanAlwaysPerformOperation );
                }
                return this.getInverseFourierTransformForCurrentGraphCommand;
            }
        }

        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> gammaCorrectionByCyclingShiftCommand;
        public ICommand GammaCorrectionByCyclingShiftCommand {
            get {
                if ( this.gammaCorrectionByCyclingShiftCommand == null ) {
                    this.gammaCorrectionByCyclingShiftCommand = new DelegateCommand<object>
                        ( this.GammaCorrectionByCyclingShift, this.CanAlwaysPerformOperation );
                }
                return this.gammaCorrectionByCyclingShiftCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> filterByMaxSpectrumValueCommand;
        public ICommand FilterByMaxSpectrumValueCommand {
            get {
                if ( this.filterByMaxSpectrumValueCommand == null ) {
                    this.filterByMaxSpectrumValueCommand = new DelegateCommand<object>
                        ( this.FilterByMaxSpectrumValue, this.CanAlwaysPerformOperation );
                }
                return this.filterByMaxSpectrumValueCommand;
            }
        }

        //--------------------------------------------------------------------------------------------------
            
        private DelegateCommand<object> correctImagesToSinusoidalFunctionCommand;
        public ICommand CorrectImagesToSinusoidalFunctionCommand {
            get {
                if ( this.correctImagesToSinusoidalFunctionCommand == null ) {
                    this.correctImagesToSinusoidalFunctionCommand = new DelegateCommand<object>
                        ( this.CorrectImagesToSinusoidalFunction, this.CanAlwaysPerformOperation );
                }
                return this.correctImagesToSinusoidalFunctionCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> fillRegionCommand;
        public ICommand FillRegionCommand {
            get {
                if ( this.fillRegionCommand == null ) {
                    this.fillRegionCommand = new DelegateCommand<object>
                        ( this.FillRegion, this.CanAlwaysPerformOperation );
                }
                return this.fillRegionCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> setColorNumberCommand;
        public ICommand SetColorNumberCommand {
            get {
                if ( this.setColorNumberCommand == null ) {
                    this.setColorNumberCommand = new DelegateCommand<object>
                        ( this.SetColorNumber, this.CanAlwaysPerformOperation );
                }
                return this.setColorNumberCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> setLeftMainImageAsUnwrappingTemplateImageCommand;
        public ICommand SetLeftMainImageAsUnwrappingTemplateImageCommand {
            get {
                if ( this.setLeftMainImageAsUnwrappingTemplateImageCommand == null ) {
                    this.setLeftMainImageAsUnwrappingTemplateImageCommand = new DelegateCommand<object>
                        ( this.SetLeftMainImageAsUnwrappingTemplateImage, this.CanAlwaysPerformOperation );
                }
                return this.setLeftMainImageAsUnwrappingTemplateImageCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> extendImageCommand;
        public ICommand ExtendImageCommand {
            get {
                if ( this.extendImageCommand == null ) {
                    this.extendImageCommand = new DelegateCommand<object>
                        ( this.ExtendImage, this.CanAlwaysPerformOperation );
                }
                return this.extendImageCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> dilatateImageCommand;
        public ICommand DilatateImageCommand {
            get {
                if ( this.dilatateImageCommand == null ) {
                    this.dilatateImageCommand = new DelegateCommand<object>
                        ( this.DilatateImage, this.CanAlwaysPerformOperation );
                }
                return this.dilatateImageCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> customNormalizeImageCommand;
        public ICommand CustomNormalizeImageCommand
        {
            get
            {
                if (this.customNormalizeImageCommand == null)
                {
                    this.customNormalizeImageCommand = new DelegateCommand<object>
                        (this.CustomNormalizeImage, this.CanAlwaysPerformOperation);
                }
                return this.customNormalizeImageCommand;
            }
        }


        //--------------------------------------------------------------------------------------------------
        private DelegateCommand<object> generateRectanglePulsesCommand;
        public ICommand GenerateRectanglePulsesCommand {
            get {
                if ( this.generateRectanglePulsesCommand == null ) {
                    this.generateRectanglePulsesCommand = new DelegateCommand<object>
                        ( this.GenerateRectanglePulses, this.CanAlwaysPerformOperation );
                }
                return this.generateRectanglePulsesCommand;
            }
        }
        //--------------------------------------------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------------------------------------------
        #region Operations
        //--------------------------------------------------------------------------------------------------
        public bool CanAlwaysPerformOperation( object parameter ) {
            return true;
        }
        //--------------------------------------------------------------------------------------------------
        //Автоматическая гамма-коррекция
        public void GammaCorrection( object parameter ) {
            IList<RealMatrix> selectedInterferograms = this.GetSelectedGrayScaleMatrices();
            RealMatrix firstInterferogram = selectedInterferograms[ 0 ];
            BitMask2D bitMask = null;
            if ( this.UseStencilImage == true ) {
                bitMask = this.stencilBitMask;
            }
            else {
                bitMask = new BitMask2D( firstInterferogram.RowCount, firstInterferogram.ColumnCount, true );
            }

            RealMatrix[] gammaCorrectedInterferograms =
                this.processingManager.GammaCorrection( selectedInterferograms.ToArray(), bitMask );

            Interval<double> finishInterval = new Interval<double>( 0, 255 );
            RealMatrix[] scaledInterferograms = 
                MatricesManager.TransformMatricesValuesToFinishIntervalValues
                ( finishInterval, gammaCorrectedInterferograms );
            WriteableBitmap[] images =
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, scaledInterferograms.ToArray() );
            
            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, selectedInterferograms.Count );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Gamma Correction" );

            this.AddImagesToImagesList( images, imagesNames, gammaCorrectedInterferograms );
        }
        //--------------------------------------------------------------------------------------------------
        //Нормализация изображений
        public void NormalizeImages( object parameter ) {
            IList<RealMatrix> selectedGrayScaleMatrices = this.GetSelectedGrayScaleMatrices();
            Interval<double> finishInterval = new Interval<double>( 0, 255 );
            
            RealMatrix[] resultImagesMatrices =
                MatricesManager.TransformMatricesValuesToFinishIntervalValues
                ( finishInterval, selectedGrayScaleMatrices.ToArray() );
            
            WriteableBitmap[] resultImages = 
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, resultImagesMatrices );

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, resultImages.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Normalized" );

            this.AddImagesToImagesList( resultImages, imagesNames, resultImagesMatrices );
        }
        //--------------------------------------------------------------------------------------------------
        //Изображение с камеры
        public void GetImageFromCamera( object parameter ) {
            
        }
        //--------------------------------------------------------------------------------------------------
        public void CameraManagement( object parameter ) {
            
            CameraWindow cameraWindow = new CameraWindow();
            cameraWindow.Show();
        }
        //--------------------------------------------------------------------------------------------------
        public void MotorManagement( object parameter ) {

            MotorWindow motorWindow = new MotorWindow();
            motorWindow.Show();
        }
        //--------------------------------------------------------------------------------------------------
        //Расшифровать
        public void Decode( object parameter ) {
            IList<RealMatrix> interferograms = this.GetSelectedGrayScaleMatrices();
            if ( interferograms != null && interferograms.Count == 0 ) {
                return;
            }

            RealMatrix firstInterferogram = interferograms[ 0 ];
            BitMask2D bitMask;

            if ( this.UseStencilImage == true ) {
                bitMask = this.stencilBitMask;
            }
            else {
                bitMask = new BitMask2D( firstInterferogram.RowCount, firstInterferogram.ColumnCount, true );
            }

            RealMatrix grayScaledPhaseMatrix = null;
            
            /*
            if ( this.decodingAlgorithm == DecodingAlgorithm.NumeratorsDenominatorsPatteringAlgorithm ) {
                grayScaledPhaseMatrix =
                    this.decodingManager.DecodeImageByNumeratorsDenominatorsPatteringAlgorithm
                    ( interferograms.ToArray(), bitMask );
            }
            */

            if ( this.decodingAlgorithm == DecodingAlgorithm.TrajectoryDescriptionByCylinderAlgorithm ) {
                grayScaledPhaseMatrix =
                    this.decodingManager.DecodeImageByTrajectoryDescriptionByCylinderAlgorithm
                    ( interferograms.ToArray(), bitMask );
            }

            else if ( this.decodingAlgorithm == DecodingAlgorithm.FirstHarmonicRotationAlgorithm ) {
                grayScaledPhaseMatrix =
                    this.decodingManager.DecodeImageByFirstHarmonicRotationAlgorithm( interferograms.ToArray() );
            }

            else if ( this.decodingAlgorithm == DecodingAlgorithm.ThreePointAlgorithm ) {
                PhaseShiftInfoWindow phaseShiftInfoWindow = new PhaseShiftInfoWindow();
                phaseShiftInfoWindow.ShowDialog();
                if ( phaseShiftInfoWindow.DialogResult == true ) {
                    double[] phaseShifts = phaseShiftInfoWindow.PhaseShifts;
                    grayScaledPhaseMatrix =
                        this.decodingManager.DecodeImageByThreePhaseShiftsAlgorithm
                        ( interferograms.ToArray(), phaseShifts, bitMask );
                }
                else {
                    return;
                }

                /*
                //double noisePercent = 30;
                //double[] phaseShifts = InterferogramProcessingHelper.GetPhaseShifts();
                //double[] phaseShifts = InterferogramProcessingHelper.GetNoisyPhaseShifts( noisePercent );
                grayScaledPhaseMatrix =
                    this.decodingManager.DecodeImageByThreePhaseShiftsAlgorithm
                    ( interferograms.ToArray(), phaseShifts, bitMask );
                */
            }

            /*
            else if ( this.decodingAlgorithm == DecodingAlgorithm.OrthogonalVectorsAlgorithm ) {
                grayScaledPhaseMatrix =
                    this.decodingManager.DecodeImageByOrthogonalVectorsAlgorithm
                    ( interferograms.ToArray(), bitMask );
            }
            */

            else if ( this.decodingAlgorithm == DecodingAlgorithm.GenericAlgorithm ) {

                //double[] phaseShifts = InterferogramProcessingHelper.GetPhaseShiftsForGenericAlgorithm();

                GenericPhaseShiftInfoWindow genericPhaseShiftInfoWindow =
                    new GenericPhaseShiftInfoWindow( interferograms.Count );

                genericPhaseShiftInfoWindow.ShowDialog();
                if ( genericPhaseShiftInfoWindow.DialogResult == true ) {
                    double[] phaseShifts = genericPhaseShiftInfoWindow.PhaseShifts;
                    grayScaledPhaseMatrix =
                        this.decodingManager.DecodeImageByGenericAlgorithm
                        ( interferograms.ToArray(), phaseShifts, bitMask );
                }
                else {
                    return;
                }
            }
            
            else if ( this.decodingAlgorithm == DecodingAlgorithm.EstimatedAnglesAlgorithm ) {
                grayScaledPhaseMatrix =
                    this.decodingManager.DecodeImageByEstimatedAnglesAlgorithm( interferograms.ToArray(), bitMask );
            }
            
            if ( grayScaledPhaseMatrix != null ) {
                BitmapSource resultImage = InterferogramProcessingHelper.GetGrayScaleImage( grayScaledPhaseMatrix );
                this.MainRightImage = resultImage;
                this.mainRightMatrix = this.decodingManager.LastPhaseMatrix;
            }
            else {
                System.Windows.MessageBox.Show( "Error" );
            }
        }
        //--------------------------------------------------------------------------------------------------
        public void AddResultImageToImagesList( object parameter ) {
            ImageInfo imageInfo = new ImageInfo( "Result Image", this.MainRightImage, this.mainRightMatrix );
            this.imagesViewModel.ImageInfoCollection.Insert( 0, imageInfo );
        }
        //--------------------------------------------------------------------------------------------------
        //Отобразить изображение в окне
        public void ShowMainImageInWindow( object parameter ) {
            UserInterfaceHelper.ShowImageInWindow( this.MainLeftImage );
        }
        //--------------------------------------------------------------------------------------------------
        //Загрузка шаблона
        public void LoadStencilImage( object parameter ) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if ( dialogResult == System.Windows.Forms.DialogResult.OK ) {
                string fileName = openFileDialog.FileName;
                 ExtraImageInfo extraImageInfo = InterferogramProcessingHelper.CreateImageFromFile( fileName );
                //WriteableBitmap image = InterferogramProcessingHelper.CreateImageFromFile( fileName );
                this.stencilBitMask = BitMaskCreator.CreateBitMaskFormWriteableBitmap( extraImageInfo.Image );
                this.StencilImage = extraImageInfo.Image;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //График числители - знаменатели формулы расшифровки
        public void ShowNumeratorsDenominatorsGraph( object parameter ) {
            RealMatrix[] interferograms = this.GetSelectedGrayScaleMatrices().ToArray();
            RealMatrix fiirstInterferogram = interferograms[0];
            BitMask2D bitMask = null;
            
            if ( this.UseStencilImage == true ) {
                bitMask = this.stencilBitMask;
            }
            else {
                new BitMask2D( fiirstInterferogram.RowCount, fiirstInterferogram.ColumnCount, true );
            }
                
            double[] phaseShifts = InterferometryHelper.GetRandomPhaseShifts( interferograms.Length );
            GenericInterferogramDecoder interferogramDecoder = new GenericInterferogramDecoder();
            double[] denominators = interferogramDecoder.GetDecodingFormulaDenominators
                ( interferograms, phaseShifts,bitMask );
            double[] numerators = interferogramDecoder.GetDecodingFormulaNumerators
                ( interferograms, phaseShifts, bitMask );
            
            Point2D[] graphPoints = PlaneManager.CreatePoints2D( numerators, denominators );
            string graphName = "Nummerators - Denominators";
            System.Windows.Media.Color color = System.Windows.Media.Colors.Red;
            bool lineVisibility = false;
            ZedGraph.SymbolType symbolType = SymbolType.Diamond;
            int symbolSize = 1;

            ZedGraphInfo zedGraphInfo = new ZedGraphInfo
                ( graphName, color, graphPoints, lineVisibility, symbolType, symbolSize );
            IList<ZedGraphInfo> graphInfoCollection = new List<ZedGraphInfo>() { zedGraphInfo };
            string axisTitleX = "Numerators";
            string axisTitleY = "Denominators";
            AxesInfo axesInfo = new AxesInfo( axisTitleX, axisTitleY );
            UserInterfaceHelper.ShowZedGraphInWindow( graphInfoCollection, axesInfo );
                        
            /*
            ExtraLibrary.Geometry3D.Point3D[] points3D = 
                SpaceManager.CreatePoints3DFromPoints2D( graphPoints, 0 );

            HelixPointsInfo pointsInfo = new HelixPointsInfo( points3D, Colors.Red, 2 );
            IList<HelixPointsInfo> pointsInfoCollection = new List<HelixPointsInfo>() { pointsInfo };
            UserInterfaceHelper.ShowGraph3DInWindow( pointsInfoCollection );
            */ 
             
        }
        //--------------------------------------------------------------------------------------------------
        //Генерация интерферограмм
        public void GenerateInterferograms( object parameter ) {

            InterferogramInfoWindow interferogramInfoWindow = new InterferogramInfoWindow();
            interferogramInfoWindow.InterferogramWidth = GenerableInterferogramInfo.Width;
            interferogramInfoWindow.InterferogramHeight = GenerableInterferogramInfo.Height;
            interferogramInfoWindow.IntensityNoisePercent = GenerableInterferogramInfo.NoisePercent;
            interferogramInfoWindow.PhaseShifts = InterferogramProcessingHelper.GetPhaseShifts();
            interferogramInfoWindow.FringeCount = GenerableInterferogramInfo.FringeCount;
            
            bool? dialogResult = interferogramInfoWindow.ShowDialog();
            if ( dialogResult == false || dialogResult == null ) {
                return;
            }

            int width = interferogramInfoWindow.InterferogramWidth;
            int height = interferogramInfoWindow.InterferogramHeight;
            double percentNoise = interferogramInfoWindow.IntensityNoisePercent;
            int fringeCount = interferogramInfoWindow.FringeCount;
            //double phaseShiftNoisePercent = 30;
            double[] phaseShifts = interferogramInfoWindow.PhaseShifts;
            //double[] phaseShifts = InterferogramProcessingHelper.GetPhaseShiftsForTwoPointAlgorithmPhaseShiftEstimation();

            //double[] phaseShifts = InterferogramProcessingHelper.GetNoisyPhaseShifts( phaseShiftNoisePercent );
            
            /*
            int width = GenerableInterferogramInfo.Width;
            int height = GenerableInterferogramInfo.Height;
            double percentNoise = 3;
            double phaseShiftNoisePercent = 30;
            double[] phaseShifts = InterferogramProcessingHelper.GetPhaseShifts();
            //double[] phaseShifts = InterferogramProcessingHelper.GetNoisyPhaseShifts( phaseShiftNoisePercent );
            */ 
            InterferogramInfo interferogramInfo = new InterferogramInfo( width, height, percentNoise );
            
            //InterferogramCreator interferogramCreator =
            //    new LinearFringeInterferogramCreator( interferogramInfo, fringeCount );

            //InterferogramCreator interferogramCreator =
            //    new CircleFringeInterferogramCreator( interferogramInfo );

            InterferogramCreator interferogramCreator =
                new LinearFringeInterferogramCreator( interferogramInfo, fringeCount );
            
            InterferogramGenerator generator = new InterferogramGenerator( interferogramCreator );
            
            RealMatrix[] interferograms = generator.GenerateInterferograms( phaseShifts );

            for ( int index = 0; index < interferograms.Length; index++ ) {
                RealMatrix interferogram = interferograms[ index ];
                WriteableBitmap image = InterferogramProcessingHelper.GetGrayScaleImage( interferogram );

                string name = "Generated " + index.ToString();
                ImageInfo imageInfo = new ImageInfo( name, image, interferogram );
                this.imagesViewModel.ImageInfoCollection.Add( imageInfo );
            }
        }
        //--------------------------------------------------------------------------------------------------
        //Изображение фазы
        public void GetPhaseImage( object parameter ) {
            
            int width = GenerableInterferogramInfo.Width;
            int height = GenerableInterferogramInfo.Height;
            double percentNoise = 0;
            InterferogramInfo interferogramInfo = new InterferogramInfo( width, height, percentNoise );
            int fringeCount = GenerableInterferogramInfo.FringeCount;
            
            
            //LinearFringeInterferogramCreator interferogramCreator =
            //    new LinearFringeInterferogramCreator( interferogramInfo, fringeCount );
            
            CircleFringeInterferogramCreator interferogramCreator = 
                new CircleFringeInterferogramCreator( interferogramInfo );


            RealMatrix phaseMatrix = interferogramCreator.GetPhaseMatrix();
            RealMatrix scaledPhaseMatrix = InterferometryHelper.TrnsformPhaseMatrixToGrayScaleMatrix( phaseMatrix );

            WriteableBitmap image = InterferogramProcessingHelper.GetGrayScaleImage( scaledPhaseMatrix );

            string name = "Phase Matrix ";
            ImageInfo imageInfo = new ImageInfo( name, image, phaseMatrix );
            this.imagesViewModel.ImageInfoCollection.Insert( 0, imageInfo );

        }
        //--------------------------------------------------------------------------------------------------
        //График целевой функции автоматической гамма-коррекции
        public void ShowAutoGammaCorrectionGraph( object parameter ) {
            double[] valuesX = this.processingManager.LastGammaValues;
            double[] valuesY = this.processingManager.LastGammaTargetFunctionValues;
            Point2D[] graphPoints = PlaneManager.CreatePoints2D( valuesX, valuesY );

            System.Windows.Media.Color color = Colors.Red;
            GraphInfo graphInfo = new GraphInfo( "Auto Gamma Correction", color, graphPoints, true );
            IList<GraphInfo> graphsInfo = new List<GraphInfo>() { graphInfo };
            UserInterfaceHelper.ShowSwordfishGraphInWindow( graphsInfo );
        }
        //----------------------------------------------------------------------------------------------------------
        //Траектория интенсивностей в пространстве
        public void ShowIntensitiesTrajectoryInSpace( object parameter ) {
            IList<RealMatrix> selectedInterferograms = this.GetSelectedGrayScaleMatrices();
            RealMatrix firstInterferogram = selectedInterferograms[ 0 ];
            BitMask2D bitMask = null;
            if ( this.UseStencilImage == true ) {
                bitMask = this.stencilBitMask;
            }
            else {
                bitMask = new BitMask2D( firstInterferogram.RowCount, firstInterferogram.ColumnCount, true );
            }
            ExtraLibrary.Geometry3D.Point3D[] points = InterferometryHelper.GetSpatialPointsFromInterferograms
                ( selectedInterferograms.ToArray(), bitMask );
                                  
            ExtraLibrary.Geometry3D.Point3D midPoint = SpaceManager.GetMidPoint( points );

            int multipleValue = 20;
            
            ExtraLibrary.Geometry3D.Point3D[] filteredPoints =
                SpaceManager.GetPointsByMultipleIndex( points, multipleValue );
            this.lastSpatialPoints.AddItem( filteredPoints );
            
            double pointsSize = 3;
            double midPointSize = 5;

            HelixPointsInfo pointsInfo = new HelixPointsInfo( filteredPoints, Colors.Red, pointsSize );
            HelixPointsInfo midPointInfo =
                new HelixPointsInfo( new ExtraLibrary.Geometry3D.Point3D[] { midPoint }, Colors.Green, midPointSize );
            IList<HelixPointsInfo> pointsInfoCollection = new List<HelixPointsInfo>() { pointsInfo, midPointInfo };
            UserInterfaceHelper.ShowGraph3DInWindow( pointsInfoCollection );
        }
        //-------------------------------------------------------------------------------------------------------------
        //Траектория интенсивностей с аппроксимирующей плоскостью
        public void ShowIntensitiesTrajectoryWithApproximatedPlaneInSpace( object parameter ) {
            IList<RealMatrix> selectedInterferograms = this.GetSelectedGrayScaleMatrices();
            RealMatrix firstInterferogram = selectedInterferograms[ 0 ];
            BitMask2D bitMask = null;
            if ( this.UseStencilImage == true ) {
                bitMask = this.stencilBitMask;
            }
            else {
                bitMask = new BitMask2D( firstInterferogram.RowCount, firstInterferogram.ColumnCount, true );
            }
            ExtraLibrary.Geometry3D.Point3D[] points = InterferometryHelper.GetSpatialPointsFromInterferograms
                ( selectedInterferograms.ToArray(), bitMask );
            ExtraLibrary.Geometry3D.Point3D midPoint = SpaceManager.GetMidPoint( points );

            PlaneApproximator planeApproximator = new PlaneApproximator();
            PlaneDescriptor planeDescriptor = planeApproximator.Approximate( points );

            int multipleValue = 20;

            ExtraLibrary.Geometry3D.Point3D[] filteredPoints =
                SpaceManager.GetPointsByMultipleIndex( points, multipleValue );
            this.lastSpatialPoints.AddItem( filteredPoints );

            double pointsSize = 3;
            double midPointSize = 5;

            HelixPointsInfo pointsInfo = new HelixPointsInfo( filteredPoints, Colors.Red, pointsSize );
            HelixPointsInfo midPointInfo =
                new HelixPointsInfo( new ExtraLibrary.Geometry3D.Point3D[] { midPoint }, Colors.Green, midPointSize );

            HelixGridLinesInfo gridLinesInfo = new HelixGridLinesInfo() {
                Center = midPoint,
                LengthDirection = planeDescriptor.GetVectorInPlane(),
                Normal = planeDescriptor.GetNormalVector(),
                Length = 250,
                Width = 250,
                MajorDistance = 0,
                MinorDistance = 5,
                Thickness = 1
            };

            IList<HelixPointsInfo> pointsInfoCollection = new List<HelixPointsInfo>() { pointsInfo, midPointInfo };
            IList<HelixGridLinesInfo> gridLinesInfoCollection = new List<HelixGridLinesInfo>() { gridLinesInfo };
            UserInterfaceHelper.ShowGraph3DInWindow( pointsInfoCollection, gridLinesInfoCollection );
            //UserInterfaceHelper.ShowGraph3DInWindow( pointsInfoCollection );
        }

        //-------------------------------------------------------------------------------------------------------------
        //Траектория интенсивностей для строки в пространстве
        public void ShowRowIntensitiesTrajectoryInSpace( object parameter ) {
            IList<RealMatrix> selectedInterferograms = this.GetSelectedGrayScaleMatrices();
            RealMatrix firstInterferogram = selectedInterferograms[ 0 ];
            BitMask2D bitMask = null;
            if ( this.UseStencilImage == true ) {
                bitMask = this.stencilBitMask;
            }
            else {
                bitMask = new BitMask2D( firstInterferogram.RowCount, firstInterferogram.ColumnCount, true );
            }
            
            int row = int.Parse( ( string )parameter );
            ExtraLibrary.Geometry3D.Point3D[] points = InterferometryHelper.GetSpatialPointsFromInterferogramsRow
                ( selectedInterferograms.ToArray(), bitMask, row );
            ExtraLibrary.Geometry3D.Point3D midPoint = SpaceManager.GetMidPoint( points );

            this.lastSpatialPoints.AddItem( points );
            
            double pointsSize = 3;
            double midPointSize = 5;

            HelixPointsInfo pointsInfo = new HelixPointsInfo( points, Colors.Red, pointsSize );
            HelixPointsInfo midPointInfo =
                new HelixPointsInfo( new ExtraLibrary.Geometry3D.Point3D[] { midPoint }, Colors.Green, midPointSize );
            IList<HelixPointsInfo> pointsInfoCollection = new List<HelixPointsInfo>() { pointsInfo, midPointInfo };
            UserInterfaceHelper.ShowGraph3DInWindow( pointsInfoCollection );
        }
        //------------------------------------------------------------------------------------------------------------
        public void ShowPairLastPointsInSpace( object parameter ) {
            double pointsSize = 3;
            
            HelixPointsInfo pointsInfoOne = 
                new HelixPointsInfo( lastSpatialPoints.GetItem( 0 ), Colors.Red, pointsSize );
            HelixPointsInfo pointsInfoTwo =
                new HelixPointsInfo( lastSpatialPoints.GetItem( 1 ), Colors.Red, pointsSize );

            IList<HelixPointsInfo> pointsInfoCollectionOne = new List<HelixPointsInfo>() { pointsInfoOne };
            IList<HelixPointsInfo> pointsInfoCollectionTwo = new List<HelixPointsInfo>() { pointsInfoTwo };

            UserInterfaceHelper.ShowPairGraph3DInWindow( pointsInfoCollectionOne, pointsInfoCollectionTwo );
        }
        //--------------------------------------------------------------------------------------------------
        //Преобразование к полутоноввым изображениям
        public void TransformImagesToGrayScaleImages( object parameter ) {
            IList<RealMatrix> selectedGrayScaleImagesMatrices = this.GetSelectedGrayScaleMatrices();
            IList<WriteableBitmap> grayScaleImages = new List<WriteableBitmap>();
            for ( int index = 0; index < selectedGrayScaleImagesMatrices.Count; index++ ) {
                RealMatrix grayScaleImageMatrix = selectedGrayScaleImagesMatrices[ index ];
                WriteableBitmap writeableBitmap =
                    WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                    ( grayScaleImageMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );
                grayScaleImages.Add( writeableBitmap );
            }

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, grayScaleImages.Count );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Gray Scale" );
            this.AddImagesToImagesList( grayScaleImages.ToArray(), imagesNames, selectedGrayScaleImagesMatrices.ToArray() );
        }
        //--------------------------------------------------------------------------------------------------
        public void LeftImageToRightImage( object parameter ) {
            this.MainRightImage = this.MainLeftImage;
            this.mainRightMatrix = this.mainLeftMatrix;
        }
        //--------------------------------------------------------------------------------------------------
        //Сохранение изображения графика в файл
        public void SaveGraphControlImageToFile( object parameter ) {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".png";
            saveDialog.Filter = "Images (.png)|*.png";

            if ( saveDialog.ShowDialog() == true ) {
                SwordfishXYLineChartControl chartControl = parameter as SwordfishXYLineChartControl;
                int width = ( int )chartControl.ActualWidth;
                int height = ( int )chartControl.ActualHeight;
                string fileName = saveDialog.FileName;
                ExtraWPF.ExtraHelperWPF.SaveControlImageToPngFile( chartControl, width, height, fileName );
            }
        }
        //--------------------------------------------------------------------------------------------------
        public void ShowCylinderDecodingAlgorithmInfo( object parameter ) {
            double pointsSize = 2;

            int multipleValue = 20;
            ExtraLibrary.Geometry3D.Point3D[] trajectoryPoints =
                SpaceManager.GetPointsByMultipleIndex( decodingManager.CylinderPoints, multipleValue );
            
            //ExtraLibrary.Geometry3D.Point3D[] circlePoints =
            //    SpaceManager.GetPointsByMultipleIndex( decodingManager.CylinderCirclePoints, multipleValue );

            HelixPointsInfo trajectoryPointsInfo =
                new HelixPointsInfo( trajectoryPoints, Colors.Blue, pointsSize );
            //HelixPointsInfo circlePontsInfo =
            //    new HelixPointsInfo( circlePoints, Colors.Red, pointsSize );

            //IList<HelixPointsInfo> pointsInfoCollection =
            //    new List<HelixPointsInfo>() { trajectoryPointsInfo, circlePontsInfo };

            IList<HelixPointsInfo> pointsInfoCollection =
                new List<HelixPointsInfo>() { trajectoryPointsInfo };


            //UserInterfaceHelper.ShowGraph3DInWindow( pointsInfoCollection );

            /*
            HelixGridLinesInfo gridLinesInfo = new HelixGridLinesInfo() {
                Center = SpaceManager.GetMidPoint( circlePoints ),
                LengthDirection = decodingManager.CirclePointsPlane.GetVectorInPlane(),
                Normal = decodingManager.CirclePointsPlane.GetNormalVector(),
                Length = 200,
                Width = 200,
                MajorDistance = 0,
                MinorDistance = 5,
                Thickness = 1
            };
            */ 

            //IList<HelixGridLinesInfo> gridLinesInfoCollection = new List<HelixGridLinesInfo>() { gridLinesInfo };
            
            //UserInterfaceHelper.ShowGraph3DInWindow( pointsInfoCollection, gridLinesInfoCollection );
            UserInterfaceHelper.ShowGraph3DInWindow( pointsInfoCollection );

            double maxX = PlaneManager.GetCoordinatesX( this.decodingManager.PlaneXyPoints ).Max() + 50;
            double maxY = PlaneManager.GetCoordinatesY( this.decodingManager.PlaneXyPoints ).Max() + 50;

            Point2D[] points1 = new Point2D[] {
                new Point2D(-this.decodingManager.EigenVector1[0] * maxX, -this.decodingManager.EigenVector1[1] * maxY),
                new Point2D(this.decodingManager.EigenVector1[0] * maxX, this.decodingManager.EigenVector1[1] * maxY)
            };
            System.Windows.Media.Color color1 = Colors.DarkRed;
                       

            Point2D[] points2 = new Point2D[] {
                new Point2D(-this.decodingManager.EigenVector2[0] * maxX, -this.decodingManager.EigenVector2[1] * maxY),
                new Point2D(this.decodingManager.EigenVector2[0] * maxX, this.decodingManager.EigenVector2[1] * maxY)
            };
            System.Windows.Media.Color color2 = Colors.DarkGreen;

            Point2D[] points3 = this.decodingManager.PlaneXyPoints;
            System.Windows.Media.Color color3 = Colors.Blue;

            ZedGraphInfo zedGraphInfo1 = new ZedGraphInfo("Главная компонента 1", color1, points1, true, SymbolType.Circle, 3);
            ZedGraphInfo zedGraphInfo2 = new ZedGraphInfo("Главная компонента 2", color2, points2, true, SymbolType.Circle, 3);
            ZedGraphInfo zedGraphInfo3 = new ZedGraphInfo("Точки", color3, points3, false, SymbolType.Circle, 1 );


            AxesInfo axesInfo = new AxesInfo("", "");
            List<ZedGraphInfo> zedInfo = new List<ZedGraphInfo>() { zedGraphInfo3, zedGraphInfo2, zedGraphInfo1 };

            UserInterfaceHelper.ShowZedGraphInWindow( zedInfo, axesInfo );




        }
        //--------------------------------------------------------------------------------------------------
        public void GetFelteredAverageIntensitiesInterferograms( object parameter ) {
            int windowSize = Convert.ToInt32( parameter.ToString() );

            IList<RealMatrix> selectedGrayScaleMatrices = this.GetSelectedGrayScaleMatrices();
            
            InterferogramsAverageIntensitiesFilter interferogramsAverageIntensitiesFilter = 
                new InterferogramsAverageIntensitiesFilter();
            RealMatrix[] resultInterferograms = interferogramsAverageIntensitiesFilter.GetFilteredInterferograms
                ( windowSize, selectedGrayScaleMatrices.ToArray() );
            
            WriteableBitmap[] resultImages =
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, resultInterferograms );
            
            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, resultImages.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Average Intensity Filter" );

            this.AddImagesToImagesList( resultImages, imagesNames, resultInterferograms );
        }
        //--------------------------------------------------------------------------------------------------
        //Обрезка границ изображений
        public void CropImagesBounds( object parameter ) {
            int cropSize = Convert.ToInt32( parameter );

            //cropSize = cropSize / 2;

            IList<WriteableBitmap> selectedImages = this.GetSelectedImages();
            WriteableBitmap firstBitmap = selectedImages[ 0 ];
            
            int width = firstBitmap.PixelWidth;
            int height = firstBitmap.PixelHeight;
            
            System.Drawing.Point topLeft = new System.Drawing.Point( cropSize, cropSize );
            System.Drawing.Point rightBottom = new System.Drawing.Point( width - cropSize - 1, height - cropSize - 1 );
            WriteableBitmap[] cropedImages = WriteableBitmapsManager.GetSubBitmaps
                ( selectedImages.ToArray(), topLeft, rightBottom );
            
            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, cropedImages.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Croped" );

            this.AddImagesToImagesList( cropedImages, imagesNames, new RealMatrix[ cropedImages.Length ] );
        }
        //--------------------------------------------------------------------------------------------------
        //Фильтрация изображений по маске
        public void FilterImagesByMask( object parameter ) {
            int size = Convert.ToInt32( parameter );
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();

            RealMatrix mask = MatrixHandler.GetFilteredMaskByAverageValue( size );
            RealMatrix[] newMatrices = MatricesManager.FilterMatricesByMask( grayScaleMatrices.ToArray(), mask );
            
            WriteableBitmap[] resultImages =
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, newMatrices );

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, resultImages.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Usual Filter" );

            this.AddImagesToImagesList( resultImages, imagesNames, newMatrices );
        }
        //--------------------------------------------------------------------------------------------------
        //Улучшить границы
        public void EnchanceEdges(object parameter)
        {            
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();

            RealMatrix mask = MatrixHandler.GetFilteredMaskForEdgeEnhancing();
            RealMatrix[] newMatrices = MatricesManager.FilterMatricesByMask(grayScaleMatrices.ToArray(), mask);

            double originMinValue = grayScaleMatrices[0].GetMinValue();
            double originMaxValue = grayScaleMatrices[0].GetMaxValue();

            double minValue = newMatrices[0].GetMinValue();
            double maxValue = newMatrices[0].GetMaxValue();

            Interval<double> startInterval = new Interval<double>(minValue, maxValue);
            Interval<double> finishInterval = new Interval<double>(0, 255);

            newMatrices = MatricesManager.TransformMatrices(startInterval, finishInterval, newMatrices);

            WriteableBitmap[] resultImages =
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                (OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, newMatrices);

            double[] numbers = ArrayCreator.CreateLinearSeriesArray(1, 1, resultImages.Length);
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray(numbers);
            imagesNames = ArrayOperator.AddStringToEachValue(imagesNames, "Enchance edge filter");

            this.AddImagesToImagesList(resultImages, imagesNames, newMatrices);
        }
        //--------------------------------------------------------------------------------------------------
        //Вычитание изображений
        public void SubstractImages( object parameter ) {
            string stringParameter = ( string )parameter;
            
            string[] stringCoefficients = stringParameter.Split( ';' );

            double coefficientOne = double.Parse( stringCoefficients[ 0 ] );
            double coefficientTwo = double.Parse( stringCoefficients[ 1 ] );
            
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();

            RealMatrix matrixOne = grayScaleMatrices[ 0 ] * coefficientOne;
            RealMatrix matrixTwo = grayScaleMatrices[ 1 ] * coefficientTwo;

            RealMatrix resultMatrix = matrixOne - matrixTwo;
            //RealMatrix resultMatrix = matrixOne * coefficientOne - matrixTwo * coefficientTwo;
            
            System.Windows.Media.Color negativeIntensityReplacedColor = Colors.Black;
            WriteableBitmap resultImage =
                WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( resultMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, negativeIntensityReplacedColor );
            string resultImageName = "Substract Image";

            WriteableBitmap[] resultImages = new WriteableBitmap[] { resultImage };
            
            string[] imagesNames = new string[] { resultImageName };

            this.AddImagesToImagesList( resultImages, imagesNames, new RealMatrix[] { resultMatrix } );
        }
        //--------------------------------------------------------------------------------------------------
        public void SubstractPhases( object parameter ) {
            string stringParameter = ( string )parameter;

            //string[] stringCoefficients = stringParameter.Split( ';' );

            //double coefficientOne = double.Parse( stringCoefficients[ 0 ] );
            //double coefficientTwo = double.Parse( stringCoefficients[ 1 ] );

            IList<RealMatrix> matrices = this.GetSelectedMatrices();

            RealMatrix matrixOne = matrices[ 0 ]; //* coefficientOne;
            RealMatrix matrixTwo = matrices[ 1 ]; //* coefficientTwo;

            RealMatrix resultMatrix = matrixOne - matrixTwo;
            //RealMatrix resultMatrix = matrixOne * coefficientOne - matrixTwo * coefficientTwo;

            System.Windows.Media.Color negativeIntensityReplacedColor = Colors.Black;

            RealMatrix scaledResultMatrix =
                RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( resultMatrix, new Interval<double>( 0, 255 ) );
            
            WriteableBitmap resultImage =
                WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledResultMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, negativeIntensityReplacedColor );
            string resultImageName = "Substract Phase";

            WriteableBitmap[] resultImages = new WriteableBitmap[] { resultImage };

            string[] imagesNames = new string[] { resultImageName };

            this.AddImagesToImagesList( resultImages, imagesNames, new RealMatrix[] { resultMatrix } );
        }
        //--------------------------------------------------------------------------------------------------
        public void AdvancedImagesSubstracting( object parameter ) {
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();

            RealMatrix matrixOne = grayScaleMatrices[ 0 ];
            RealMatrix matrixTwo = grayScaleMatrices[ 1 ];

            RealMatrix resultMatrix = new RealMatrix( matrixOne.RowCount, matrixOne.ColumnCount );

            for ( int row = 0; row < matrixTwo.RowCount; row++ ) {
                double[] rowValuesOne = matrixOne.GetRow( row );
                double[] rowValuesTwo = matrixTwo.GetRow( row );
                
                double averageValue = rowValuesTwo.Average();

                for ( int column = 0; column < matrixTwo.ColumnCount; column++ ) {
                    double delta = rowValuesTwo[ column ] - averageValue;
                    double resultValue = rowValuesOne[ column ] - delta;
                    resultMatrix[ row, column ] = resultValue;
                }
            }

            System.Windows.Media.Color negativeIntensityReplacedColor = Colors.Black;
            WriteableBitmap resultImage =
                WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( resultMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, negativeIntensityReplacedColor );
            string resultImageName = "Substract Image";

            WriteableBitmap[] resultImages = new WriteableBitmap[] { resultImage };
            string[] imagesNames = new string[] { resultImageName };

            this.AddImagesToImagesList( resultImages, imagesNames, new RealMatrix[] { resultMatrix } );
        }
        //--------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------
        //Показать график разности
        public void ShowSubstractGraph( object parameter ) {
            double[] valuesX = PlaneManager.GetCoordinatesX( this.GraphInfoCollection[ 0 ].GraphPoints );
                        
            double[] valuesY1 = PlaneManager.GetCoordinatesY( this.GraphInfoCollection[ 0 ].GraphPoints );
            double[] valuesY2 = PlaneManager.GetCoordinatesY( this.GraphInfoCollection[ 1 ].GraphPoints );

            double[] resultValues = new double[ valuesX.Length ];
            for ( int index = 0; index < valuesX.Length; index++ ) {
                resultValues[ index ] = valuesY1[ index ] - valuesY2[ index ];
            }

            Point2D[] graphPoints = PlaneManager.CreatePoints2D( valuesX, resultValues );
            System.Windows.Media.Color graphColor = System.Windows.Media.Colors.Green;
            GraphInfo graphInfo = new GraphInfo( "Разность", graphColor, graphPoints, true );

            IList<GraphInfo> graphInfoCollection = new List<GraphInfo>() { graphInfo };
            UserInterfaceHelper.ShowSwordfishGraphInWindow( graphInfoCollection );
        }
        //--------------------------------------------------------------------------------------------------
        public void CalculateRootMeanSquareErrorForGraphs( object parameter ) {
            
            double[] valuesY1 = PlaneManager.GetCoordinatesY( this.GraphInfoCollection[ 0 ].GraphPoints );
            double[] valuesY2 = PlaneManager.GetCoordinatesY( this.GraphInfoCollection[ 1 ].GraphPoints );

            double rootMeanSquareError = Statistician.GetRootMeanSquareError( valuesY1, valuesY2 );
            this.RootMeanSquareErrorText = rootMeanSquareError.ToString();
        }
        //--------------------------------------------------------------------------------------------------
        public void CalculateRootMeanSquareErrorForMatrices( object parameter ) {
            RealMatrix[] selectedMatrices = this.GetSelectedMatrices().ToArray();

            RealMatrix matrixOne = selectedMatrices[ 0 ];
            RealMatrix matrixTwo = selectedMatrices[ 1 ];

            double rootMeanSquareError = Statistician.GetRootMeanSquareError( matrixOne, matrixTwo );
            this.RootMeanSquareErrorText = rootMeanSquareError.ToString();
        }
        //--------------------------------------------------------------------------------------------------
        //Отобразить графики для выделенных изображений
        public void ShowGraphsForSelectedImages( object parameter ) {
            string stringParameter = parameter.ToString();
            int row = Convert.ToInt32(stringParameter);
            
            ColorsCollection colorsCollection = new ColorsCollection();
            IList<WriteableBitmap> selectedImages = this.GetSelectedImages();
            IList<RealMatrix> selectedMatrices = this.GetSelectedMatrices();

            IList<GraphInfo> graphInfoCollection = new List<GraphInfo>();
            for ( int index = 0; index < selectedImages.Count; index++ ) {
                WriteableBitmap bitmap = selectedImages[ index ];
                RealMatrix matrix = selectedMatrices[ index ];
                int number = index + 1;
                GraphInfo graphInfo = this.GetImageRowGraphInfo( bitmap, matrix, row, colorsCollection[ index ], number );
                graphInfoCollection.Add( graphInfo );
            }

            this.GraphInfoCollection = graphInfoCollection;
        }
        //--------------------------------------------------------------------------------------------------
        //Спектр преобразования Фурье
        public void GetFourierTransformSpectrum( object parameter ) {
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();
            RealMatrix grayScaleMatrix = grayScaleMatrices[ 0 ];
            FastFourierTransform fastFourierTransform = new FastFourierTransform();

            ComplexMatrix fourierTransform2D =
                fastFourierTransform.GetCenteredFourierTransform2D( grayScaleMatrix );
            this.lastFourierTransform2D = fourierTransform2D;
            
            RealMatrix fourierTransformSpectrum2D =
                fastFourierTransform.GetFourierTransformSpectrum2D( fourierTransform2D );

            LogTransform logTransform = new LogTransform(0.5);
            fourierTransformSpectrum2D = 
                RealMatrixValuesTransform.TransformMatrixValues( fourierTransformSpectrum2D, logTransform );

            //double brightness = 10;
            //fourierTransformSpectrum2D = fourierTransformSpectrum2D * brightness;
            
            RealMatrix scaledFourierTransformSpectrum2D =
                RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( fourierTransformSpectrum2D, new Interval<double>( 0, 255 ) );

            WriteableBitmap fourierTransformSpectrumImage =
                WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledFourierTransformSpectrum2D, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );

            this.MainRightImage = fourierTransformSpectrumImage;
            this.mainRightMatrix = fourierTransformSpectrum2D;
        }
        //--------------------------------------------------------------------------------------------------
        //Обратное преобразование Фурье
        public void GetInverseFourierTransform( object parameter ) {
            ComplexMatrix complexMatrix = this.lastFourierTransform2D;
            
            FastFourierTransform fastFourierTransform = new FastFourierTransform();
            
            ComplexMatrix inverseFourierTransform2D =
                fastFourierTransform.GetInverseFourierTransform2D( complexMatrix );
                        
            RealMatrix realInverseFourierTransform2D = inverseFourierTransform2D.GetRealMatrix();
                      
            realInverseFourierTransform2D = 
                fastFourierTransform.GetMatrixForCenteredFourierTransform( realInverseFourierTransform2D );            
            
            RealMatrix scaledRealInverseFourierTransform2D =
                RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( realInverseFourierTransform2D, new Interval<double>( 0, 255 ) );
            
            WriteableBitmap inverseFourierTransformImage =
                WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledRealInverseFourierTransform2D, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );
             
            this.MainRightImage = inverseFourierTransformImage;
            this.mainRightMatrix = realInverseFourierTransform2D;
        }
        //--------------------------------------------------------------------------------------------------
        public void GetInverseFourierTransforms( object parameter ) {
            FastFourierTransform fastFourierTransform = new FastFourierTransform();
            WriteableBitmap[] resultImages = new WriteableBitmap[ this.fourierTransforms2D.Length ];
            RealMatrix[] resultMatrices = new RealMatrix[ this.fourierTransforms2D.Length ];

            for ( int index = 0; index < this.fourierTransforms2D.Length; index++ ) {
                ComplexMatrix fourierTransform2D = this.fourierTransforms2D[ index ];
                ComplexMatrix inverseFourierTransform2D =
                    fastFourierTransform.GetInverseFourierTransform2D( fourierTransform2D );
                RealMatrix realInverseFourierTransform2D = inverseFourierTransform2D.GetRealMatrix();
                realInverseFourierTransform2D =
                    fastFourierTransform.GetMatrixForCenteredFourierTransform( realInverseFourierTransform2D );

                RealMatrix scaledRealInverseFourierTransform2D =
                    RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                    ( realInverseFourierTransform2D, new Interval<double>( 0, 255 ) );
                
                WriteableBitmap inverseFourierTransformImage =
                    WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                    ( scaledRealInverseFourierTransform2D, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );

                resultImages[ index ] = inverseFourierTransformImage;
                resultMatrices[ index ] = realInverseFourierTransform2D;
            }

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, resultImages.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Inverse Fourier Transform" );

            this.AddImagesToImagesList( resultImages, imagesNames, resultMatrices );
        }
        //--------------------------------------------------------------------------------------------------
        //Спектры преобразования Фурье
        public void GetFourierTransformSpectrums( object parameter ) {
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();

            this.fourierTransforms2D =
                FastFourierTransformManager.GetCenteredFourierTransforms2D( grayScaleMatrices.ToArray() );
            RealMatrix[] fourierTransformSpectrums2D =
                FastFourierTransformManager.GetFourierTransformSpectrums2D( this.fourierTransforms2D );
            
            double logTransformCoefficient = 0.5;
            fourierTransformSpectrums2D =
                MatricesManager.GetLogTransforms( fourierTransformSpectrums2D, logTransformCoefficient );

            RealMatrix[] scaledFourierTransformSpectrums2D =
                MatricesManager.TransformMatricesValuesToFinishIntervalValues
                ( new Interval<double>( 0, 255 ), fourierTransformSpectrums2D );

            WriteableBitmap[] fourierTransformSpectrumImages =
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, scaledFourierTransformSpectrums2D );

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, fourierTransformSpectrumImages.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " FFT Spectrum" );

            this.AddImagesToImagesList( fourierTransformSpectrumImages, imagesNames, fourierTransformSpectrums2D );
        } 
        //--------------------------------------------------------------------------------------------------
        //Обрезка изображений
        public void CropImagesToSizeAtPowerOfTwo( object parameter ) {
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();
            RealMatrix grayScaleMatrix = grayScaleMatrices[ 0 ];

            int newRowCount = MathHelper.GetPreviousHighestNumberAtPowerOfTwo( grayScaleMatrix.RowCount );
            int newColumnCount = MathHelper.GetPreviousHighestNumberAtPowerOfTwo( grayScaleMatrix.ColumnCount );

            RealMatrix[] newMatrices = new RealMatrix[grayScaleMatrices.Count];
            for ( int index = 0; index < grayScaleMatrices.Count; index++ ) {
                RealMatrix matrix = grayScaleMatrices[ index ];
                RealMatrix newMatrix = matrix.GetCenteredSubMatrix( newRowCount, newColumnCount );
                newMatrices[ index ] = newMatrix;
            }

            WriteableBitmap[] images =
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, newMatrices );

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, images.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Crop Image" );

            this.AddImagesToImagesList( images, imagesNames, newMatrices );
        }
        //--------------------------------------------------------------------------------------------------
        public void FilterFourierTransformByRemoving( object parameter ) {
            double filterMatrixDefaultValue = 1.0;
            double filterValue = 0.0;
            this.FilterFourierTransform( filterMatrixDefaultValue, filterValue );
        }
        //--------------------------------------------------------------------------------------------------
        public void FilterFourierTransformBySelecting( object parameter ) {
            double filterMatrixDefaultValue = 0.0;
            double filterValue = 1.0;
            this.FilterFourierTransform( filterMatrixDefaultValue, filterValue );
        }
        //--------------------------------------------------------------------------------------------------
        public void FilterFourierTransformsBySelecting( object parameter ) {
            
            double filterMatrixDefaultValue = 0.0;
            double filterValue = 1.0;

            this.FilterFourierTransforms( filterMatrixDefaultValue, filterValue );
        }
        //--------------------------------------------------------------------------------------------------
        //Преобразование Фурье для текущего графика
        public void GetFourierTransformForCurrentGraph( object parameter ) {

            Point2D[] graphPoints = this.GraphInfoCollection[ 0 ].GraphPoints;
            double[] argumentValues = ArrayCreator.CreateLinearSeriesArray( 0, 1, graphPoints.Length );
            double[] functionValues = PlaneManager.GetCoordinatesY( graphPoints );
            double[] frequencyValues = ArrayCreator.CreateLinearSeriesArray( 0, 1, graphPoints.Length );

            FourierTransform fourierTransform = new FourierTransform();

            Complex[] resultValues =
                fourierTransform.GetCenteredFourierTransform( argumentValues, functionValues, frequencyValues );
            this.fourierTransformValuesForGraph = resultValues;
            
            double[] spectrumValues = fourierTransform.GetFourierTransformSpectrum( resultValues );

            //double[] valuesX = ArrayCreator.CreateLinearSeriesArray( 0, 1, spectrumValues.Length );

            Point2D[] resultGraphPoints = PlaneManager.CreatePoints2D( frequencyValues, spectrumValues );

            GraphInfo resultGraphInfo =
                new GraphInfo( "Fourier Transform Spectrum", Colors.Red, resultGraphPoints, true );
            IList<GraphInfo> graphInfoCollection = new List<GraphInfo>() { resultGraphInfo };
            UserInterfaceHelper.ShowSwordfishGraphInWindow( graphInfoCollection );
        }
        //--------------------------------------------------------------------------------------------------
        //Преобразование Фурье для текущего графика
        public void GetInverseFourierTransformForCurrentGraph( object parameter ) {
            Complex[] fourierTransformValues = this.fourierTransformValuesForGraph;
            double[] argumentValues = ArrayCreator.CreateLinearSeriesArray( 0, 1, fourierTransformValues.Length );

            FourierTransform fourierTransform = new FourierTransform();
            //Complex[] inverseValues = fourierTransform.GetInvereFourierTransform( argumentValues, fourierTransformValues );
            double[] inverseValues = fourierTransform.GetInvereFourierTransform( argumentValues, fourierTransformValues );
            inverseValues = fourierTransform.GetFunctionValuesForCenteredFourierTransform( argumentValues, inverseValues );

            //double[] inverseValuesRealParts = inverseValues.Select( c => c.Real).ToArray();

            Point2D[] resultGraphPoints = PlaneManager.CreatePoints2D( argumentValues, inverseValues );
                       
            GraphInfo resultGraphInfo =
                new GraphInfo( "Inverse Fourier Transform", Colors.Red, resultGraphPoints, true );
            IList<GraphInfo> graphInfoCollection = new List<GraphInfo>() { resultGraphInfo };
            UserInterfaceHelper.ShowSwordfishGraphInWindow( graphInfoCollection );
            
            /*
            ZedGraphInfo resultGraphInfo =
                new ZedGraphInfo( "Inverse Fourier Transform", Colors.Red, resultGraphPoints, true, SymbolType.Diamond, 1 );
            IList<ZedGraphInfo> graphInfoCollectionInv = new List<ZedGraphInfo>() { resultGraphInfo };
            UserInterfaceHelper.ShowZedGraphInWindow( graphInfoCollectionInv, new AxesInfo( "X", "Y" ) );
            */ 

        }
        //--------------------------------------------------------------------------------------------------
        //Развертка матрицы фаз по строкам
        public void UnwrapLastPhaseMatrixByRows( object parameter ) {
            RealMatrix phaseMatrix = this.decodingManager.LastPhaseMatrix;
            PhaseUnwrappingAlgorithm phaseUnwrappingAlgorithm = new PhaseUnwrappingAlgorithm( phaseMatrix );
            
            //int extremumIndex = phaseMatrix.ColumnCount / 2;
            int extremumIndex = 0;
            UnwrapDirection unwrapDirection = UnwrapDirection.Up;

            RealMatrix resultMatrix = phaseUnwrappingAlgorithm.UnwrapByRows( extremumIndex, unwrapDirection );

            RealMatrix scaledResultMatrix = 
                RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( resultMatrix, new Interval<double>( 0, 255 ) );
            
            WriteableBitmap resultImage = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledResultMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );

            this.MainRightImage = resultImage;
            this.mainRightMatrix = resultMatrix;
        }
        //--------------------------------------------------------------------------------------------------
        //Развертка матрицы фаз по строкам
        public void UnwrapPhaseMatrixByRows( object parameter ) {
            RealMatrix matrix = this.GetSelectedMatrices()[ 0 ];
            RealMatrix phaseMatrix = matrix;
            PhaseUnwrappingAlgorithm phaseUnwrappingAlgorithm = new PhaseUnwrappingAlgorithm( phaseMatrix );

            //int extremumIndex = phaseMatrix.ColumnCount / 2;
            int extremumIndex = 0;
            //UnwrapDirection unwrapDirection = UnwrapDirection.Up;
            UnwrapDirection unwrapDirection = UnwrapDirection.Down;

            RealMatrix resultMatrix = phaseUnwrappingAlgorithm.UnwrapByRows( extremumIndex, unwrapDirection );

            RealMatrix scaledResultMatrix =
                RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( resultMatrix, new Interval<double>( 0, 255 ) );

            WriteableBitmap resultImage = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledResultMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );

            this.MainRightImage = resultImage;
            this.mainRightMatrix = resultMatrix;
        }
        
        //--------------------------------------------------------------------------------------------------
        
        //Развертка матрицы фаз по строкам
        public void UnwrapPhaseMatrixByColumns( object parameter ) {
            RealMatrix matrix = this.GetSelectedMatrices()[ 0 ];
            RealMatrix phaseMatrix = matrix.GetTransposedMatrix();
            PhaseUnwrappingAlgorithm phaseUnwrappingAlgorithm = new PhaseUnwrappingAlgorithm( phaseMatrix );

            //int extremumIndex = phaseMatrix.ColumnCount / 2;
            int extremumIndex = 0;
            //UnwrapDirection unwrapDirection = UnwrapDirection.Up;
            UnwrapDirection unwrapDirection = UnwrapDirection.Down;

            RealMatrix resultMatrix = phaseUnwrappingAlgorithm.UnwrapByRows( extremumIndex, unwrapDirection );
            resultMatrix = resultMatrix.GetTransposedMatrix();

            RealMatrix scaledResultMatrix =
                RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( resultMatrix, new Interval<double>( 0, 255 ) );

            WriteableBitmap resultImage = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledResultMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );

            this.MainRightImage = resultImage;
            this.mainRightMatrix = resultMatrix;
        }
        
        //--------------------------------------------------------------------------------------------------
        //Фильтрация изображений простым фильтром
        public void FilterImagesByStep( object parameter ) {
            int step = int.Parse( ( string )parameter );
            int stepX, stepY;
            stepX = stepY = step;
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();
            SimpleGrayScaleFilter simpleGrayScaleFilter = new SimpleGrayScaleFilter();

            IList<RealMatrix> resultMatrices = new List<RealMatrix>();
            for ( int index = 0; index < grayScaleMatrices.Count; index++ ) {
                RealMatrix matrix = grayScaleMatrices[ index ];
                RealMatrix filteredMatrix = simpleGrayScaleFilter.ExecuteFiltration( matrix, stepX, stepY );
                resultMatrices.Add( filteredMatrix );
            }
            
            WriteableBitmap[] resultImages = WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, resultMatrices.ToArray() );

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, resultImages.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Filtered Image" );

            this.AddImagesToImagesList( resultImages, imagesNames, resultMatrices.ToArray() );
        }
        //--------------------------------------------------------------------------------------------------
        //Фильтрация изображений медианным фильтром
        public void FilterImagesByMedian(object parameter)
        {
            int windowSize = int.Parse((string)parameter);
            IList<RealMatrix> grayScaleMatrices = this.GetSelectedGrayScaleMatrices();
            MedianByRowsGrayScaleFilter medianByRowsGrayScaleFilter = new MedianByRowsGrayScaleFilter();

            IList<RealMatrix> resultMatrices = new List<RealMatrix>();
            for (int index = 0; index < grayScaleMatrices.Count; index++)
            {
                RealMatrix matrix = grayScaleMatrices[index];
                RealMatrix filteredMatrix = medianByRowsGrayScaleFilter.ExecuteFiltration(matrix, windowSize);
                resultMatrices.Add(filteredMatrix);
            }

            WriteableBitmap[] resultImages = WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                (OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, resultMatrices.ToArray());

            double[] numbers = ArrayCreator.CreateLinearSeriesArray(1, 1, resultImages.Length);
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray(numbers);
            imagesNames = ArrayOperator.AddStringToEachValue(imagesNames, " Filtered Image");

            this.AddImagesToImagesList(resultImages, imagesNames, resultMatrices.ToArray());
        }
        //--------------------------------------------------------------------------------------------------
        public void FilterByMaxSpectrumValue( object parameter ) {
            FourierTransform fourierTransform = new FourierTransform();
            this.fourierTransformValuesForGraph =
                fourierTransform.FilterByMaxSpectrumValue( this.fourierTransformValuesForGraph );
        }
        //--------------------------------------------------------------------------------------------------
        public void GetInversePhase( object parameter ) {
            RealMatrix phaseMatrix = this.GetSelectedMatrices().ToArray()[ 0 ];
              
            RealMatrix newMatrix = new RealMatrix( phaseMatrix.RowCount, phaseMatrix.ColumnCount );
            
            for ( int row = 0; row < phaseMatrix.RowCount; row++ ) {
                for ( int column = 0; column < phaseMatrix.ColumnCount; column++ ) {
                    newMatrix[ row, column ] = 2 * Math.PI - phaseMatrix[ row, column ];    
                }
            }
            
            RealMatrix scaledMatrix = RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                (newMatrix, new Interval<double>(0,  255));

            WriteableBitmap newImage = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );

            this.MainRightImage = newImage;
            this.mainRightMatrix = newMatrix;
        }
        //--------------------------------------------------------------------------------------------------
        public void CalculatePhaseShifts( object parameter ) {
            RealMatrix[] selectedMatrices = this.GetSelectedGrayScaleMatrices().ToArray();
            PhaseShiftComputer phaseShiftComputer = new PhaseShiftComputer( selectedMatrices );
            System.Drawing.Point pointOne = this.fourierTransformFilterPoints.GetItem( 0 );
            System.Drawing.Point pointTwo = this.fourierTransformFilterPoints.GetItem( 1 );
            double[] phaseShifts = phaseShiftComputer.Compute( pointOne, pointTwo );
            this.TrajectoryPoints = phaseShiftComputer.TrajectoryPoints;
            this.EllipsePoints2D = phaseShiftComputer.EllipsePoints;
            this.IntensitiesForPointOne = phaseShiftComputer.IntensitiesForPointOne;

            FileStream fs = new FileStream( "CalculatedPhaseShifts.txt", FileMode.Create );
            StreamWriter sw = new StreamWriter( fs );
            
            StringBuilder sb = new StringBuilder();
            for ( int index = 0; index < phaseShifts.Length; index++ ) {
                double phaseShiftInDegrees = Mathem.GetAngleInDegrees( phaseShifts[ index ] );
                //sb.AppendFormat( "{0};", phaseShifts[ index ] );
                sb.AppendFormat( "{0};", phaseShiftInDegrees );
                sw.WriteLine( string.Format( "{0}\t{1}", phaseShifts[ index ], phaseShiftInDegrees.ToString()) );       
            }
            
            //this.RootMeanSquareErrorText = sb.ToString();
            
            sw.Close();
            fs.Close();
            
            double[] arrayX = ArrayCreator.CreateLinearSeriesArray(1, 1, this.IntensitiesForPointOne.Length); 
            Point2D[] points = PlaneManager.CreatePoints2D(arrayX, this.IntensitiesForPointOne); 

            GraphInfo graphInfo = new GraphInfo("1", Colors.Red, points, true);
            this.GraphInfoCollection = new List<GraphInfo>() { graphInfo }; 
        }
        //--------------------------------------------------------------------------------------------------
        public void ShowIntensitiesForPoint( object parameter ) {
            int x = this.lastClickedPoint.X;
            int y = this.lastClickedPoint.Y;

            RealMatrix[] matrices = this.GetSelectedGrayScaleMatrices().ToArray();
            List<double> intensities = new List<double>();
            for ( int index = 0; index < matrices.Length; index++ ) {
                double intensity = matrices[ index ][ y, x ];
                intensities.Add( intensity );
            }

            double[] resultArray = intensities.ToArray();
            double[] arrayX = ArrayCreator.CreateLinearSeriesArray(1, 1, resultArray.Length);
            Point2D[] graphPoints = PlaneManager.CreatePoints2D( arrayX, resultArray );

            GraphInfo graphInfo = new GraphInfo( "1", Colors.Red, graphPoints, true );

            this.GraphInfoCollection = new List<GraphInfo>() { graphInfo };
        }

        //--------------------------------------------------------------------------------------------------
        public void ShowPhaseShiftsCalculatingEllipsePoints( object parameter ) {
            bool lineVisibility = false;
            ZedGraphInfo zedGraphInfo1 = 
                new ZedGraphInfo("Trajectory points", Colors.Red, this.TrajectoryPoints, lineVisibility, SymbolType.Diamond, 3);
            
            ZedGraphInfo zedGraphInfo2 =
                new ZedGraphInfo("Ellipse points", Colors.Green, this.EllipsePoints2D, false, SymbolType.Diamond, 1 );
                
            List<ZedGraphInfo> zedGrpahInfoCollection = new List<ZedGraphInfo>() {zedGraphInfo1, zedGraphInfo2 };
            AxesInfo axesInfo = new AxesInfo( "Интенсивность в точке 1", "Интенсивность в точке 2" );
            UserInterfaceHelper.ShowZedGraphInWindow( zedGrpahInfoCollection, axesInfo ); 
        }
        //--------------------------------------------------------------------------------------------------
        public void GammaCorrectionByCyclingShift( object parameter ) {
            RealMatrix[] matrices = this.GetSelectedGrayScaleMatrices().ToArray();
            
            double[] gammaValues = ArrayCreator.CreateLinearSeriesArray( 1.2, 0.2, 5 ); 

            InterfrogramsGammaCorrectorByCyclingShift interferogramsCorrector = new InterfrogramsGammaCorrectorByCyclingShift();
            RealMatrix[] resultMatrices = interferogramsCorrector.GetCorrectedInterferograms( matrices, gammaValues );

            RealMatrix[] scaledResultMatrices =
                MatricesManager.TransformMatricesValuesToFinishIntervalValues
                ( new Interval<double>( 0, 255 ), resultMatrices );
            
            WriteableBitmap[] resultImages =
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, scaledResultMatrices );

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, resultImages.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Gamma Corrected Image" );
            
            this.AddImagesToImagesList( resultImages, imagesNames, resultMatrices );
        }
        //--------------------------------------------------------------------------------------------------
        public void CorrectImagesToSinusoidalFunction( object parameter ) {
            RealMatrix[] grayScaleMatrices = this.GetSelectedGrayScaleMatrices().ToArray();
            
            int rowCount = grayScaleMatrices[ 0 ].RowCount;
            int columnCount = grayScaleMatrices[ 0 ].ColumnCount;
            
            RealMatrix[] resultMatrices = new RealMatrix[ grayScaleMatrices.Length ];
            for ( int index = 0; index < resultMatrices.Length; index++ ) {
                resultMatrices[ index ] = new RealMatrix( rowCount, columnCount );
            }
            
            double[] argumentValues = ArrayCreator.CreateLinearSeriesArray(0, 1, grayScaleMatrices.Length);
            double[] frequencyValues = ArrayCreator.CreateLinearSeriesArray(0, 1, grayScaleMatrices.Length);

            FourierTransform fourierTransform = new FourierTransform();
            for ( int row = 0; row < rowCount; row++ ) {
                for ( int column = 0; column < columnCount; column++ ) {
                    double[] intensities = MatricesManager.GeValuesFromMatrices( row, column, grayScaleMatrices );
                    Complex[] fourierTransformValues =
                        fourierTransform.GetCenteredFourierTransform( argumentValues, intensities, frequencyValues );
                    Complex[] filteredValues = fourierTransform.FilterByMaxSpectrumValue( fourierTransformValues );
                    double[] inverseValues = fourierTransform.GetInvereFourierTransform( argumentValues, filteredValues );
                    double[] resultValues = 
                        fourierTransform.GetFunctionValuesForCenteredFourierTransform( argumentValues, inverseValues );
                    MatricesManager.SetValuesInMatrices( resultMatrices, resultValues, row, column );
                }
            }
                        
            RealMatrix[] scaledMatrices =
                MatricesManager.TransformMatricesValuesToFinishIntervalValues
                ( new Interval<double>( 0, 255 ), resultMatrices );
            
            WriteableBitmap[] images =
                WriteableBitmapsManager.CreateGrayScaleWriteableBitmapsFromMatrices
                ( OS.IntegerSystemDpiX, OS.IntegerSystemDpiY, scaledMatrices );

            double[] numbers = ArrayCreator.CreateLinearSeriesArray( 1, 1, images.Length );
            string[] imagesNames = ExtraLibrary.Converting.ArrayConverter.ToStringArray( numbers );
            imagesNames = ArrayOperator.AddStringToEachValue( imagesNames, " Corrected Sinusoidal Image" );

            this.AddImagesToImagesList( images, imagesNames, resultMatrices );
            
            //int row = 100;
            //int column = 100;
            //double[] resValues = MatricesManager.GeValuesFromMatrices( row, column, resultMatrices );
            

        }
        //--------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------
        public void GenerateRectanglePulses(object parameter) {

            RectanglePulses rectanglePulses = new RectanglePulses();

            bool? dilogResult = rectanglePulses.ShowDialog();
            if ( dilogResult == null || dilogResult == false ) {
                return;
            }
            else {

                int arrayLength = rectanglePulses.ArrayLength;
                int rectangleWidth = rectanglePulses.RectangleWidth;
                int rectangleHeight = rectanglePulses.RectangleHeight;
                int rectangleInterval = rectanglePulses.RectangleInterval;

                int startIndex = 1;
                int index = startIndex;
                int step = rectangleWidth + rectangleInterval;

                double[] values = new double[ arrayLength ];


                while ( index < arrayLength ) {
                    int m = index;
                    while ( m < arrayLength && m < index + rectangleWidth ) {
                        values[ m ] = rectangleHeight;
                        m = m + 1;
                    }

                    index = index + step;
                }

                double[] coordinatesX = ArithmeticProgression.GetValues( 0, 1, arrayLength );
                double[] coordinatesY = values;
                Point2D[] graphPoints = PlaneManager.CreatePoints2D( coordinatesX, coordinatesY );
                GraphInfo graphInfo = new GraphInfo( "RectanglePulse", Colors.Red, graphPoints, true );

                this.GraphInfoCollection = new List<GraphInfo>() { graphInfo };
            }
        }

        //--------------------------------------------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------------------------------------------
        #region Auxiliary Operation
        //--------------------------------------------------------------------------------------------------
        //Фильтрация преобразования Фурье
        public void FilterFourierTransform( double filterMatrixDefaultValue, double filterValue ) {
            System.Drawing.Point pointOne = this.fourierTransformFilterPoints.GetItem( 0 );
            System.Drawing.Point pointTwo = this.fourierTransformFilterPoints.GetItem( 1 );

            RealMatrix filterMatrix = new RealMatrix
                ( this.lastFourierTransform2D.RowCount, this.lastFourierTransform2D.ColumnCount, filterMatrixDefaultValue );

            int width = pointTwo.X - pointOne.X;
            int height = pointTwo.Y - pointOne.Y;

            Rect rectangle = new Rect( pointOne.X, pointOne.Y, width, height );
            PlaneCoordinateSystem planeCoordinateSystem = new PlaneCoordinateSystem
                ( this.lastFourierTransform2D.ColumnCount, this.lastFourierTransform2D.RowCount );
            Rect symmetricRectangleInScreenCoordinateSystem =
                planeCoordinateSystem.GetSymmetricRectangleInScreenCoordinateSystem( rectangle );

            int rowTopLeftOne = ( int )rectangle.Y;
            int columnTopLeftOne = ( int )rectangle.X;

            int rowTopLeftTwo = ( int )symmetricRectangleInScreenCoordinateSystem.Y;
            int columnTopLeftTwo = ( int )symmetricRectangleInScreenCoordinateSystem.X;

            RealMatrix region = new RealMatrix( height, width, filterValue );

            filterMatrix.SetSubMatrix( region, rowTopLeftOne, columnTopLeftOne );
            filterMatrix.SetSubMatrix( region, rowTopLeftTwo, columnTopLeftTwo );

            FourierFilter fourierFilter = new FourierFilter();
            ComplexMatrix filteredFourierTransform2D =
                fourierFilter.FilterFourierTransform( this.lastFourierTransform2D, filterMatrix );

            this.lastFourierTransform2D = filteredFourierTransform2D;
        }
        //--------------------------------------------------------------------------------------------------
        //Фильтрация преобразований Фурье
        public void FilterFourierTransforms( double filterMatrixDefaultValue, double filterValue ) {
            System.Drawing.Point pointOne = this.fourierTransformFilterPoints.GetItem( 0 );
            System.Drawing.Point pointTwo = this.fourierTransformFilterPoints.GetItem( 1 );

            RealMatrix filterMatrix = new RealMatrix
                ( this.fourierTransforms2D[0].RowCount, this.fourierTransforms2D[0].ColumnCount, filterMatrixDefaultValue );

            int width = pointTwo.X - pointOne.X;
            int height = pointTwo.Y - pointOne.Y;

            Rect rectangle = new Rect( pointOne.X, pointOne.Y, width, height );
            PlaneCoordinateSystem planeCoordinateSystem = new PlaneCoordinateSystem
                ( this.fourierTransforms2D[ 0 ].ColumnCount, this.fourierTransforms2D[ 0 ].RowCount );
            Rect symmetricRectangleInScreenCoordinateSystem =
                planeCoordinateSystem.GetSymmetricRectangleInScreenCoordinateSystem( rectangle );

            int rowTopLeftOne = ( int )rectangle.Y;
            int columnTopLeftOne = ( int )rectangle.X;

            int rowTopLeftTwo = ( int )symmetricRectangleInScreenCoordinateSystem.Y;
            int columnTopLeftTwo = ( int )symmetricRectangleInScreenCoordinateSystem.X;

            RealMatrix region = new RealMatrix( height, width, filterValue );

            filterMatrix.SetSubMatrix( region, rowTopLeftOne, columnTopLeftOne );
            filterMatrix.SetSubMatrix( region, rowTopLeftTwo, columnTopLeftTwo );

            FourierFilter fourierFilter = new FourierFilter();

            for ( int index = 0; index < this.fourierTransforms2D.Length; index++ ) {
                ComplexMatrix filteredFourierTransform2D = 
                    fourierFilter.FilterFourierTransform( this.fourierTransforms2D[ index ], filterMatrix );
                this.fourierTransforms2D[ index ] = filteredFourierTransform2D;
            }
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        public void AddPointToFourierTransformFilterPoints( System.Drawing.Point point ) {
            this.fourierTransformFilterPoints.AddItem( point );
        }
        //--------------------------------------------------------------------------------------------------
        public void SetLastClickedPoint( System.Drawing.Point point ) {
            this.lastClickedPoint = point;
            this.SetLastClickedColor();
        }
        //--------------------------------------------------------------------------------------------------
        public void SetLastClickedColor() {
            WriteableBitmap bitmap = this.MainLeftImage as WriteableBitmap;
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
            
            int x = lastClickedPoint.X;
            int y = lastClickedPoint.Y;

            this.lastClickedColor = wrapper.GetPixelColor( x, y );
        }

        //--------------------------------------------------------------------------------------------------
        public void ShowMatlabGraph3D( object parameter ) {
            RealMatrix selectedMatrix = this.GetSelectedGrayScaleMatrices().ToArray()[ 0 ];
            //RealMatrix selectedMatrix = this.GetSelectedMatrices().ToArray()[ 0 ];
            
            /*
            MWNumericArray xArray =
                new MWNumericArray( ArrayCreator.CreateLinearSeriesArray( 0, 1, selectedMatrix.ColumnCount ) );
            MWNumericArray yArray =
                new MWNumericArray( ArrayCreator.CreateLinearSeriesArray( 0, 1, selectedMatrix.RowCount ) );
            MWNumericArray zArray = new MWNumericArray( selectedMatrix.GetDataArray() );
                        
            Graph3D.Graph3D g = new Graph3D.Graph3D();
            g.MatlabGraph3D( xArray, yArray, zArray );
            */ 
        }
        //--------------------------------------------------------------------------------------------------
        public void ShowPhaseMatrixMatlabGraph3D( object parameter ) {
            //RealMatrix selectedMatrix = this.GetSelectedGrayScaleMatrices().ToArray()[ 0 ];
            
            RealMatrix selectedMatrix = this.GetSelectedMatrices().ToArray()[ 0 ];

            /*
            MWNumericArray xArray =
                new MWNumericArray( ArrayCreator.CreateLinearSeriesArray( 0, 1, selectedMatrix.ColumnCount ) );
            MWNumericArray yArray =
                new MWNumericArray( ArrayCreator.CreateLinearSeriesArray( 0, 1, selectedMatrix.RowCount ) );
            MWNumericArray zArray = new MWNumericArray( selectedMatrix.GetDataArray() );

            Graph3D.Graph3D g = new Graph3D.Graph3D();
            g.MatlabGraph3D( xArray, yArray, zArray );
            */
 
        }
        //--------------------------------------------------------------------------------------------------
        public void SetGraphInfoCollection( WriteableBitmap leftBitmap, WriteableBitmap rightBitmap, int row ) {
            this.imageGraphRow = row;
            
            IList<GraphInfo> graphInfoCollection = new List<GraphInfo>();
            System.Windows.Media.Color color = Colors.Red;
            RealMatrix matrix;
            
            if ( this.ActiveImage == this.MainRightImage && this.ImagesViewingMode == ImagesViewingMode.ImagesComparison ) {
                matrix = this.mainLeftMatrix;
            }
            else {
                matrix = this.ActiveImage == this.MainRightImage ? this.mainRightMatrix : this.mainLeftMatrix;
            }
            
            GraphInfo graphInfo = this.GetImageRowGraphInfo( leftBitmap, matrix, row, color, 1 );
            graphInfoCollection.Add( graphInfo );

            if ( this.imagesViewingMode == ImagesViewingMode.ImagesComparison ) {
                color = Colors.Blue;
                graphInfo = this.GetImageRowGraphInfo( rightBitmap, this.mainRightMatrix, row, color, 2 );
                graphInfoCollection.Add( graphInfo );
            }

            this.GraphInfoCollection = graphInfoCollection;
        }
        //------------------------------------------------------------------------------------------------------
        private GraphInfo GetImageRowGraphInfo(
            WriteableBitmap bitmap, RealMatrix matrix, int row, System.Windows.Media.Color graphColor, int numberInName
        ) {
            double[] valuesY = null;

            if ( matrix != null ) {
                valuesY = matrix.GetRow( row );
            }
            else {

                switch ( this.GraphShowingMode ) {
                    case GraphShowingMode.GrayScale: {
                            valuesY = ImagesProcessingHelper.GetRowGrayScaleValues( bitmap, row );
                            break;
                        }
                    case GraphShowingMode.Red: {
                            valuesY = ImagesProcessingHelper.GetRowRedValues( bitmap, row );
                            break;
                        }
                    case GraphShowingMode.Green: {
                            valuesY = ImagesProcessingHelper.GetRowGreenValues( bitmap, row );
                            break;
                        }
                    case GraphShowingMode.Blue: {
                            valuesY = ImagesProcessingHelper.GetRowBlueValues( bitmap, row );
                            break;
                        }
                    case GraphShowingMode.Phase: {
                            valuesY = matrix.GetRow( row );
                            break;
                        }
                }
            }

            double[] valuesX = ArithmeticProgression.GetValues( 0, 1, bitmap.PixelWidth );
            Point2D[] points = PlaneManager.CreatePoints2D( valuesX, valuesY );

            string graphName = numberInName.ToString();
            //GraphInfo graphInfo = new GraphInfo( "Фаза", graphColor, points, true );
            GraphInfo graphInfo = new GraphInfo( graphName, graphColor, points, true );
            return graphInfo;
        }
        //------------------------------------------------------------------------------------------------------
        //C камеры  получено изображние
        /*
        private void CameraDeviceController_ImageReceived( ImageReceiveEventArgs imageReceivedEventArgs ) {
            BitmapSource imageSource = imageReceivedEventArgs.ReceivedImage;
            string imageName = DateTime.Now.ToString();
            ImageInfo imageInfo = new ImageInfo( imageName, imageSource, null );
            this.ImagesViewModel.ImageInfoCollection.Insert( 0, imageInfo );
            this.MainLeftImage = imageSource;
        }
        */ 
        //--------------------------------------------------------------------------------------------------
        //Выбранные интерферограммы
        private IList<RealMatrix> GetSelectedGrayScaleMatrices() {
            IList<RealMatrix> interferograms = new List<RealMatrix>();
            if ( this.ImagesViewModel.SelectedImageInfoCollection != null ) {
                
                for ( int index = 0; index < this.ImagesViewModel.SelectedImageInfoCollection.Count; index++ ) {
                    ImageInfo imageInfo = this.ImagesViewModel.SelectedImageInfoCollection[ index ];
                    WriteableBitmap writeableBitmap = imageInfo.ImageSource as WriteableBitmap;
                    WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( writeableBitmap );
                    RealMatrix interferogram = wrapper.GetGrayScaleMatrix();
                    interferograms.Add( interferogram );
                }
            }

            return interferograms;
        }
        //--------------------------------------------------------------------------------------------------
        //Выбранные изображения
        private IList<WriteableBitmap> GetSelectedImages() {
            IList<WriteableBitmap> selectedBitmaps = new List<WriteableBitmap>();
            for ( int index = 0; index < this.ImagesViewModel.SelectedImageInfoCollection.Count; index++ ) {
                ImageInfo imageInfo = this.ImagesViewModel.SelectedImageInfoCollection[ index ];
                WriteableBitmap writeableBitmap = imageInfo.ImageSource as WriteableBitmap;
                selectedBitmaps.Add( writeableBitmap );
            }
            return selectedBitmaps;
        }
        //--------------------------------------------------------------------------------------------------
        private IList<RealMatrix> GetSelectedMatrices() {
            IList<RealMatrix> selectedMatrices = new List<RealMatrix>();
            for ( int index = 0; index < this.ImagesViewModel.SelectedImageInfoCollection.Count; index++ ) {
                ImageInfo imageInfo = this.ImagesViewModel.SelectedImageInfoCollection[ index ];
                RealMatrix matrix = imageInfo.Matrix;
                selectedMatrices.Add( matrix );
            }
            return selectedMatrices;
        }
        //--------------------------------------------------------------------------------------------------
        //Добавление изображений к списку
        private void AddImagesToImagesList( WriteableBitmap[] images, string[] imagesNames, RealMatrix[] matrices) {
            for ( int index = 0; index < images.Length; index++ ) {
                WriteableBitmap image = images[ index ];
                string imageName = imagesNames[ index ];
                RealMatrix matrix = matrices[ index ];
                ImageInfo imageInfo = new ImageInfo( imageName, image, matrix );
                this.ImagesViewModel.ImageInfoCollection.Add( imageInfo );
            }
        }
        //--------------------------------------------------------------------------------------------------
        
        //--------------------------------------------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------------------------------------------
        public void Test( object parameter ) {
            Test5();
        }
        //--------------------------------------------------------------------------------------------------
        private void Test1() {
            ExtraLibrary.Geometry3D.Point3D point1 = new ExtraLibrary.Geometry3D.Point3D( 1, 3, 10 );
            ExtraLibrary.Geometry3D.Point3D point2 = new ExtraLibrary.Geometry3D.Point3D( 0, 2, 23 );
            ExtraLibrary.Geometry3D.Point3D point3 = new ExtraLibrary.Geometry3D.Point3D( -4, 9, 6 );

            ExtraLibrary.Geometry3D.Point3D[] points =
                new ExtraLibrary.Geometry3D.Point3D[] { point1, point2, point3 };

            ExtraLibrary.Geometry3D.Point3D midPoint = SpaceManager.GetMidPoint( points );
            
            PlaneApproximator planeApproximator = new PlaneApproximator();
            PlaneDescriptor planeDescriptor = SpaceManager.GetPlaneByThreePoints( point1, point2, point3 );

            HelixGridLinesInfo gridLinesInfo = new HelixGridLinesInfo() {
                Center = midPoint,
                LengthDirection = planeDescriptor.GetVectorInPlane(),
                Normal = planeDescriptor.GetNormalVector(),
                Length = 30,
                Width = 30,
                MajorDistance = 0,
                MinorDistance = 5,
                Thickness = 1
            };

            HelixPointsInfo pointsInfo = new HelixPointsInfo( points, Colors.Red, 6 );
            HelixGraph3DControl control = new HelixGraph3DControl();
            control.AddPointsInfo( pointsInfo );
            control.AddGridLinesInfo( gridLinesInfo );

            Window window = new Window();
            window.Content = control;
            //window.Show();
            window.Activate();


            ExtraLibrary.Geometry3D.Point3D point_1 = new ExtraLibrary.Geometry3D.Point3D( 1, 7, 7 );
            ExtraLibrary.Geometry3D.Point3D point_2 = new ExtraLibrary.Geometry3D.Point3D( 2, 10, 8 );
            ExtraLibrary.Geometry3D.Point3D point_3 = new ExtraLibrary.Geometry3D.Point3D( 3, 6, 9 );

            ExtraLibrary.Geometry3D.Point3D[] pointsTest =
                new ExtraLibrary.Geometry3D.Point3D[] { point_1, point_2, point_3 };
            PlaneApproximator approximator = new PlaneApproximator();
            PlaneDescriptor descriptor = approximator.Approximate( pointsTest );
        }
        //--------------------------------------------------------------------------------------------------
        private void Test2() {
            int count = 1024;

            double[] frequencyValues = ArrayCreator.CreateLinearSeriesArray( 0, 1, count );
            double[] valuesX = ArrayCreator.CreateLinearSeriesArray( 0, 1, count );
            
            double[] valuesY = new double[ count ];
            for ( int index = 0; index < 16; index++ ) {
                valuesY[ index ] = 1;
            }
                       
            FourierTransform fourierTransform = new FourierTransform();
            Complex[] transformValues = fourierTransform.GetCenteredFourierTransform( valuesX, valuesY, frequencyValues );

            double[] spectrumValues = fourierTransform.GetFourierTransformSpectrum( transformValues );
            Point2D[] graphPoints = PlaneManager.CreatePoints2D( frequencyValues, spectrumValues );
            
            GraphInfo graphInfo = new GraphInfo( "1", Colors.Red, graphPoints, true );
            IList<GraphInfo> graphInfoCollection = new List<GraphInfo>() { graphInfo };

            UserInterfaceHelper.ShowSwordfishGraphInWindow( graphInfoCollection );
                        
            /*
            Complex[] inverseValues = fourierTransform.GetInvereFourierTransform( valuesX, transformValues );

            double[] inverseValuesRealParts = inverseValues.Select( c => c.Magnitude ).ToArray();
            inverseValuesRealParts = 
                fourierTransform.GetFunctionValuesForCenteredFourierTransform( valuesX, inverseValuesRealParts );

            Point2D[] resultGraphPoints = PlaneManager.CreatePoints2D( valuesX, inverseValuesRealParts );

            ZedGraphInfo resultGraphInfo =
                new ZedGraphInfo( "Inverse Fourier Transform", Colors.Red, resultGraphPoints, true, SymbolType.Diamond, 1 );
            IList<ZedGraphInfo> graphInfoCollectionInv = new List<ZedGraphInfo>() { resultGraphInfo };
            UserInterfaceHelper.ShowZedGraphInWindow( graphInfoCollectionInv, new AxesInfo( "X", "Y" ) );
            */

        }
        //--------------------------------------------------------------------------------------------------
        private void Test3() {
            int w = 512;
            int h = 512;
            int powerOfTwo = 9;
            RealMatrix matrix = new RealMatrix( h, w );

            int ww = 20;
            int hh = 10;

            for ( int x = w / 2 - ww; x < w / 2 + ww; x++ ) {
                for ( int y = h / 2 - hh; y < h / 2 + hh; y++ )
                    matrix[ y, x ] = 255;
            }
            WriteableBitmap bitmap = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( matrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );
            UserInterfaceHelper.ShowImageInWindow( bitmap );
            
            FastFourierTransform fourierTransform = new FastFourierTransform();
            //ComplexMatrix fourierTransformResult = fourierTransform.GetCenteredFourierTransform2D( matrix );
            ComplexMatrix fourierTransformResult = fourierTransform.GetCenteredFourierTransform2D( matrix );
            RealMatrix fourierTransformSpectrum2D = 
                fourierTransform.GetFourierTransformSpectrum2D( fourierTransformResult );
            LogTransform logTransform = new LogTransform( 0.5 );
            
            RealMatrix resultMatrix = 
                RealMatrixValuesTransform.TransformMatrixValues( fourierTransformSpectrum2D, logTransform );
            resultMatrix = RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( resultMatrix, new Interval<double>( 0, 255 ) );
            WriteableBitmap resultBitmap = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( resultMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );
            UserInterfaceHelper.ShowImageInWindow( resultBitmap );
                        
            ComplexMatrix recoveredMatrix = fourierTransform.GetInverseFourierTransform2D( fourierTransformResult );
            RealMatrix recoveredMatrixSpectrum = fourierTransform.GetFourierTransformSpectrum2D( recoveredMatrix );
            
            RealMatrix scaledRecoveredMatrix =
                RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( recoveredMatrixSpectrum, new Interval<double>( 0, 255 ) );
            WriteableBitmap recoveredBitmap = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledRecoveredMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );
            UserInterfaceHelper.ShowImageInWindow( recoveredBitmap );
                
        }
        //--------------------------------------------------------------------------------------------------
        private void Test4() {
            RealMatrix[] matrices = this.GetSelectedGrayScaleMatrices().ToArray();

            double result = Statistician.GetMatricesPearsonsCorrelationCcoefficient( matrices[ 0 ], matrices[ 1 ] );
        }
        //------------------------------------------------------------------------------------------------------
        private void Test5()
        {
            FastFourierTransform fft = new FastFourierTransform();
            
            /*
            RealMatrix m = new RealMatrix(8, 8);
            int k = 0;
            for (int row = 0; row < m.RowCount; row++)
            {
                for (int col = 0; col < m.ColumnCount; col++)
                { 
                    m[row, col] = k + 1;
                    k++;
                }
            }

            ComplexMatrix res = fft.GetCudaFourierTransform2D(m);
            */

            int width = 32;
            int heght = 32;

            RealMatrix m = new RealMatrix(heght, width);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < heght; y++)
                {
                    m[y, x] = y + x;
                }
            }


            ComplexMatrix res = fft.GetCenteredFourierTransform2D(m);
            
        }

        //------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------
        private void FillRegion(object parameter)
        {
            WriteableBitmap bitmap = this.MainLeftImage as WriteableBitmap;
            if ( bitmap != null ) {
                System.Windows.Media.Color selectedColor;

                WPFColorPickerLib.ColorDialog colorDialog = new WPFColorPickerLib.ColorDialog();
                bool? dialogResult = colorDialog.ShowDialog();
                if ( dialogResult == true ) {
                    selectedColor = colorDialog.SelectedColor;
                }
                else {
                    return;
                }
                
                WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
                int x = this.lastClickedPoint.X;
                int y = this.lastClickedPoint.Y;

                wrapper.FillRegion( x, y, System.Windows.Media.Colors.Black, selectedColor );
                this.MainLeftImage = bitmap;

            }
        }
        //------------------------------------------------------------------------------------------------------
        private void SetColorNumber( object parameter ) {
            ColorNumberWindow window = new ColorNumberWindow();

            int number;
            bool isExistNumber = this.colorNumbers.TryGetValue( this.lastClickedColor, out number );
            if ( isExistNumber ) {
                window.LabelColor = this.lastClickedColor;
                window.Number = number;
            }
            else {
                window.LabelColor = this.lastClickedColor;
            }

            bool? dialogResult = window.ShowDialog();
            if ( dialogResult == true ) {
                colorNumbers[ this.lastClickedColor ] = window.Number;
            }
        }
        //------------------------------------------------------------------------------------------------------
        private void SetLeftMainImageAsUnwrappingTemplateImage( object sender ) {
            this.unwrappingTemplateImage = this.MainLeftImage as WriteableBitmap;
            WriteableBitmapWrapper templateWrapper = WriteableBitmapWrapper.Create( this.unwrappingTemplateImage );
            
            //templateWrapper.DeleteBlackPoints();
            //this.MainLeftImage = templateWrapper.Image;
        }
        //------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------
        private void ExtendImage( object parameter ) {
            WriteableBitmap image = this.GetSelectedImages()[ 0 ];
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( image );

            WriteableBitmap newImage = wrapper.GetExtendedWriteableBitmapByOnePixelToBorder();
            this.MainRightImage = newImage;
        }
        //------------------------------------------------------------------------------------------------------
        
        //------------------------------------------------------------------------------------------------------
        private void UnwrapPhaseMatrixByTemplateImage(object parameter) {
            SetColorNumbers( 0 );

            RealMatrix phaseMatrix = this.GetSelectedMatrices()[ 0 ];

            RealMatrix unwrappedPhaseMatrix = new RealMatrix( phaseMatrix );
            WriteableBitmapWrapper templateWrapper = WriteableBitmapWrapper.Create( this.unwrappingTemplateImage );
            //templateWrapper.DeleteBlackPoints();

            foreach ( KeyValuePair<System.Windows.Media.Color, int> kvp in this.colorNumbers ) {
                List<System.Drawing.Point> colorPoints = templateWrapper.GetSpecificColorPoints( kvp.Key );
                int number = kvp.Value;

                foreach ( System.Drawing.Point point in colorPoints ) {
                    double phaseValue = phaseMatrix[ point.Y, point.X ];
                    double newPhaseValue = phaseValue + Math.PI * 2 * number;
                    unwrappedPhaseMatrix[ point.Y, point.X ] = newPhaseValue;
                }
            }
                        
            double prevValue;
            double value;
            double originValue;
            
            /*
            for ( int y = 0; y < unwrappedPhaseMatrix.RowCount; y++ ) {
                prevValue = unwrappedPhaseMatrix[ y, 0 ];
                for ( int x = 1; x < unwrappedPhaseMatrix.ColumnCount - 3; x++ ) {
                    value = unwrappedPhaseMatrix[ y, x ];
                    if ( Math.Abs( value - prevValue ) > Math.PI ) {
                        int n;

                        if ( colorNumbers.TryGetValue( templateWrapper.GetPixelColor( x - 1, y ), out n ) ) {
                            originValue = phaseMatrix[ y, x ];
                            unwrappedPhaseMatrix[ y, x ] = originValue + n * Math.PI * 2;
                        }

                        if ( colorNumbers.TryGetValue( templateWrapper.GetPixelColor( x + 2, y ), out n ) ) {
                            originValue = phaseMatrix[ y, x + 1 ];
                            unwrappedPhaseMatrix[ y, x + 1 ] = originValue + n * Math.PI * 2;
                        }
                                          
                    }
                    prevValue = value;
                }
            }
            
            for ( int y = 0; y < unwrappedPhaseMatrix.RowCount; y++ ) {
                prevValue = unwrappedPhaseMatrix[ y, 0 ];
                for ( int x = 1; x < unwrappedPhaseMatrix.ColumnCount - 3; x++ ) {
                    value = unwrappedPhaseMatrix[ y, x ];
                    if ( Math.Abs( value - prevValue ) > Math.PI ) {
                        int n;

                        if ( colorNumbers.TryGetValue( templateWrapper.GetPixelColor( x - 1, y ), out n ) ) {
                            originValue = phaseMatrix[ y, x ];
                            unwrappedPhaseMatrix[ y, x ] = originValue + n * Math.PI * 2;
                        }

                        if ( colorNumbers.TryGetValue( templateWrapper.GetPixelColor( x + 2, y ), out n ) ) {
                            originValue = phaseMatrix[ y, x + 1 ];
                            unwrappedPhaseMatrix[ y, x + 1 ] = originValue + n * Math.PI * 2;
                        }

                    }
                    prevValue = value;
                }
            }
            */
            
            /*
            for ( int y = 0; y < unwrappedPhaseMatrix.RowCount; y++ ) {
                prevValue = unwrappedPhaseMatrix[ y, 0 ];
                for ( int x = 1; x < unwrappedPhaseMatrix.ColumnCount - 3; x++ ) {
                    value = unwrappedPhaseMatrix[ y, x ];
                    if ( Math.Abs( value - prevValue ) > Math.PI ) {
                        int n;

                        if ( colorNumbers.TryGetValue( templateWrapper.GetPixelColor( x + 1, y ), out n ) ) {
                            originValue = phaseMatrix[ y, x ];
                            unwrappedPhaseMatrix[ y, x ] = originValue + n * Math.PI * 2;
                        }

                        if ( colorNumbers.TryGetValue( templateWrapper.GetPixelColor( x + 2, y ), out n ) ) {
                            originValue = phaseMatrix[ y, x + 1 ];
                            unwrappedPhaseMatrix[ y, x + 1 ] = originValue + n * Math.PI * 2;
                        }

                    }
                    prevValue = value;
                }
            }
            */

            for ( int y = 0; y < unwrappedPhaseMatrix.RowCount; y++ ) {
                prevValue = unwrappedPhaseMatrix[ y, 0 ];
                for ( int x = 1; x < unwrappedPhaseMatrix.ColumnCount - 1; x++ ) {
                    value = unwrappedPhaseMatrix[ y, x ];
                    if ( Math.Abs(prevValue - value) > Math.PI ) {
                        unwrappedPhaseMatrix[ y, x ] = ( prevValue + unwrappedPhaseMatrix[ y, x + 1 ] ) / 2;
                        //unwrappedPhaseMatrix[ y, x ] = prevValue;
                    }

                    prevValue = unwrappedPhaseMatrix[ y, x ];
                }
            }
            
            /*
            for ( int y = 0; y < unwrappedPhaseMatrix.RowCount; y++ ) {
                prevValue = unwrappedPhaseMatrix[ y, 0 ];
                for ( int x = 1; x < unwrappedPhaseMatrix.ColumnCount; x++ ) {
                    value = unwrappedPhaseMatrix[ y, x ];
                    if ( ( prevValue - value ) > Math.PI / 2 ) {
                        unwrappedPhaseMatrix[ y, x ] += Math.PI * 2;
                    }
                    else if ( prevValue - value < -Math.PI / 2 ) {
                        unwrappedPhaseMatrix[ y, x ] -= Math.PI * 2;
                    }

                    prevValue = value;
                }
            }
            */ 



            RealMatrix scaledResultMatrix =
                RealMatrixValuesTransform.TransformMatrixValuesToFinishIntervalValues
                ( unwrappedPhaseMatrix, new Interval<double>( 0, 255 ) );

            WriteableBitmap resultImage = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix
                ( scaledResultMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY );

            this.MainRightImage = resultImage;
            this.mainRightMatrix = unwrappedPhaseMatrix;

        }
        //------------------------------------------------------------------------------------------------------
        private void CustomizeUnwrappedTemplateImage() {
            //WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( this.unwrappingTemplateImage );
            //List<System.Drawing.Point> points = wrapper.GetSpecificColorPoints( Colors.Black );
        }
        
        //------------------------------------------------------------------------------------------------------
        private void SetColorNumbers( object sender ) {
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( this.unwrappingTemplateImage );

            int startX = this.lastClickedPoint.X;
            int startY = this.lastClickedPoint.Y;

            int colorNumber = 0;
            System.Windows.Media.Color prevColor = wrapper.GetPixelColor( startX, startY );
            System.Windows.Media.Color currentColor;
            this.colorNumbers[ prevColor ] = colorNumber;

            for ( int x = startX + 1; x < this.MainLeftImage.PixelWidth; x++ ) {
                currentColor = wrapper.GetPixelColor( x, startY );
                if ( currentColor == prevColor && currentColor != Colors.Black && currentColor != Colors.White ) {
                    continue;
                }

                if ( currentColor != prevColor && currentColor != Colors.Black && currentColor != Colors.White ) {
                    colorNumber--;
                    this.colorNumbers[ currentColor ] = colorNumber;
                    prevColor = currentColor;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------
        public void DilatateImage( object parameter ) {
            WriteableBitmap image = this.MainLeftImage as WriteableBitmap;
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( image );
            wrapper.Dilatate( this.lastClickedColor );

            this.MainLeftImage = wrapper.Image;
        }
        //------------------------------------------------------------------------------------------------------
        public void CustomNormalizeImage( object parameter )
        {
            WriteableBitmap image = this.MainLeftImage as WriteableBitmap;
            
            WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create(image);
            RealMatrix matrix = wrapper.GetGrayScaleMatrix();

            Interval<double> startInterval = new Interval<double>(0, 255.0);
            Interval<double> finishInterval = new Interval<double>(0, 255.0 * 167.0 / 211.0);
            RealIntervalTransform transform = new RealIntervalTransform(startInterval, finishInterval);

            RealMatrix resMatrix = RealMatrixValuesTransform.TransformMatrixValues(matrix, transform);
            WriteableBitmap resImage = WriteableBitmapCreator.CreateGrayScaleWriteableBitmapFromMatrix(resMatrix, OS.IntegerSystemDpiX, OS.IntegerSystemDpiY);

            this.MainRightImage = resImage;
        }

        public BitmapSource MainLeftImage {
            get {
                return this.mainLeftImage;
            }
            set {
                this.mainLeftImage = value;
                this.RaisePropertyChanged( () => this.MainLeftImage );
            }
        }
        
        public BitmapSource MainRightImage {
            get {
                return this.mainRightImage;
            }
            set {
                this.mainRightImage = value;
                this.RaisePropertyChanged( () => this.MainRightImage );
            }
        }
        
        public BitmapSource ActiveImage {
            get {
                return this.activeImage;
            }
            set {
                this.activeImage = value;
                this.RaisePropertyChanged( () => this.ActiveImage );
            }
        }

        public BitmapSource StencilImage {
            get {
                return this.stencilImage;
            }
            set {
                this.stencilImage = value;
                this.RaisePropertyChanged( () => this.StencilImage );
            }
        }
        

        public bool? UseStencilImage {
            get {
                return this.useStencilImage;
            }
            set {
                this.useStencilImage = value;
                this.RaisePropertyChanged( () => this.UseStencilImage );
            }
        }
        
        public IList<GraphInfo> GraphInfoCollection {
            get {
                return this.graphInfoCollection;
            }
            set {
                this.graphInfoCollection = value;
                this.RaisePropertyChanged( () => this.GraphInfoCollection);
            }
        }

        //ImagesViewModel imagesViewModel = new ImagesViewModel();

        public ImagesViewModel ImagesViewModel {
            get {
                return this.imagesViewModel;
            }
            set {
                this.imagesViewModel = value;
                this.RaisePropertyChanged( () => this.ImagesViewModel );
            }
        }

        public ImagesViewingMode ImagesViewingMode {
            get {
                return this.imagesViewingMode;
            }
            set {
                this.imagesViewingMode = value;
                this.RaisePropertyChanged( () => this.ImagesViewingMode );
            }
        }
        
        public GraphShowingMode GraphShowingMode {
            get {
                return this.graphShowingMode;
            }
            set {
                this.graphShowingMode = value;
                this.RaisePropertyChanged( () => this.GraphShowingMode );
            }
        }

        public string RootMeanSquareErrorText {
            get {
                return this.rootMeanSquareErrorText;
            }
            set {
                this.rootMeanSquareErrorText = value;
                this.RaisePropertyChanged( () => this.RootMeanSquareErrorText );
            }
        }
        
        public DecodingAlgorithm DecodingAlgorithm {
            get {
                return this.decodingAlgorithm;
            }
            set {
                this.decodingAlgorithm = value;
                this.RaisePropertyChanged( () => this.DecodingAlgorithm );
            }
        }
      
    }
    //------------------------------------------------------------------------------------------------------
}
