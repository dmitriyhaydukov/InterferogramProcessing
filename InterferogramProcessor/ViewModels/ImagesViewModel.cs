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

using ExtraControls;
using ExtraMVVM;
using ExtraLibrary.ImageProcessing;
using ExtraLibrary.OS;
using ExtraLibrary.Arraying.ArrayOperation;
using ExtraLibrary.Arraying.ArrayCreation;
using ExtraLibrary.Converting;
using ExtraLibrary.Mathematics.Matrices;
using ExtraLibrary.Mathematics.Statistics;
using ExtraLibrary.Mathematics.Sets;
using ExtraLibrary.Mathematics.Progressions;
using ExtraLibrary.Randomness;
using ExtraLibrary.Mathematics.Transformation;
using ExtraLibrary.Geometry2D;
using ExtraLibrary.Geometry3D;
using ExtraLibrary.Collections;

using Interferometry.InterferogramProcessing;
using Interferometry.InterferogramCreation;
using Interferometry.InterferogramDecoding;
using Interferometry.DeviceControllers;
using Interferometry.Helpers;

using UserInterfaceHelping;

using ZedGraph;

using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;


#endregion

namespace InterferogramProcessing {
    public class ImagesViewModel : NotificationObject {
        
        private IList<ImageInfo> imageInfoCollection;
        private IList<ImageInfo> selectedImageInfoCollection;
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        public IList<ImageInfo> ImageInfoCollection {
            get {
                return this.imageInfoCollection;
            }
            set {
                this.imageInfoCollection = value;
                this.RaisePropertyChanged( () => this.ImageInfoCollection );
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public IList<ImageInfo> SelectedImageInfoCollection {
            get {
                return this.selectedImageInfoCollection;
            }
            set {
                this.selectedImageInfoCollection = value;
                this.RaisePropertyChanged( () => this.SelectedImageInfoCollection );
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void InitializeData() {
            this.imageInfoCollection = new BindingList<ImageInfo>();
        }
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        private DelegateCommand<object> loadImagesCommand;
        public ICommand LoadImagesCommand {
            get {
                if ( this.loadImagesCommand == null ) {
                    this.loadImagesCommand = new DelegateCommand<object>
                        ( this.LoadImages, this.CanAlwaysPerformOperation );
                }
                return this.loadImagesCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private DelegateCommand<object> saveImagesCommand;
        public ICommand SaveImagesCommand {
            get {
                if ( this.saveImagesCommand == null ) {
                    this.saveImagesCommand = new DelegateCommand<object>
                        ( this.SaveImages, this.CanAlwaysPerformOperation );
                }
                return this.saveImagesCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private DelegateCommand<object> deleteImagesCommand;
        public ICommand DeleteImagesCommand {
            get {
                if ( this.deleteImagesCommand == null ) {
                    this.deleteImagesCommand = new DelegateCommand<object>
                        ( this.DeleteImages, this.CanAlwaysPerformOperation );
                }
                return this.deleteImagesCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //Загрузить изображения
        public void LoadImages( object parameter ) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if ( dialogResult == System.Windows.Forms.DialogResult.OK ) {
                string[] fileNames = openFileDialog.FileNames;
                for ( int index = 0; index < fileNames.Length; index++ ) {
                    string fileName = fileNames[ index ];
                    ExtraImageInfo extraImageInfo = InterferogramProcessingHelper.CreateImageFromFile( fileName );
                    //WriteableBitmap image = InterferogramProcessingHelper.CreateImageFromFile( fileName );
                    string imageName = Path.GetFileName( fileName );
                    RealMatrix matrix = extraImageInfo.Matrix;
                    ImageInfo imageInfo = new ImageInfo( imageName, extraImageInfo.Image, matrix );
                    this.ImageInfoCollection.Add( imageInfo );
                }
            }
        }
        //------------------------------------------------------------------------------------------------------
        //Сохранение изображений
        public void SaveImages( object parameter ) {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = ".png";
            saveDialog.Filter = "Images (.png)|*.png";

            for ( int index = 0; index < this.SelectedImageInfoCollection.Count; index++ ) {
                ImageInfo imageInfo = this.SelectedImageInfoCollection[ index ];
                if ( saveDialog.ShowDialog() == true ) {
                    string fileName = saveDialog.FileName;
                    WriteableBitmap bitmap = imageInfo.ImageSource as WriteableBitmap;
                    WriteableBitmapWrapper wrapper = WriteableBitmapWrapper.Create( bitmap );
                    wrapper.SaveToPngFile( fileName );
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //Удаление изображений
        public void DeleteImages( object parameter ) {
            while ( this.SelectedImageInfoCollection.Count > 0 ) {
                ImageInfo imageInfo = this.SelectedImageInfoCollection[ 0 ];
                this.ImageInfoCollection.Remove( imageInfo );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public bool CanAlwaysPerformOperation( object parameter ) {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public bool CanPerformOperation( object parameter ) {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
    }
}
