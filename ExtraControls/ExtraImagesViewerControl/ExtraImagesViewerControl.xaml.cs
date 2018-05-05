using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections;

using ExtraWPF;

namespace ExtraControls {
    /// <summary>
    /// Логика взаимодействия для HayImagesViewer.xaml
    /// </summary>
    public partial class ExtraImagesViewerControl : UserControl {

        public static readonly DependencyProperty ImageInfoCollectionProperty;
        public static readonly DependencyProperty SelectedImageInfoCollectionProperty;
        public static readonly DependencyProperty SelectedIndexProperty;
        public static readonly DependencyProperty SelectedIndicesProperty;
        public static readonly DependencyProperty ImagesListOrientationProperty;
        public static readonly RoutedEvent SelectedImageChangedEvent;
        //-------------------------------------------------------------------------------------------------------
        public IList<ImageInfo> SelectedImageInfoCollection {
            get {
                return ( IList<ImageInfo> )this.GetValue( ExtraImagesViewerControl.SelectedImageInfoCollectionProperty );
            }
            set {
                this.SetValue( ExtraImagesViewerControl.SelectedImageInfoCollectionProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public Orientation ImagesListOrientation {
            get {
                return ( Orientation )this.GetValue( ExtraImagesViewerControl.ImagesListOrientationProperty );
            }
            set {
                this.SetValue( ExtraImagesViewerControl.ImagesListOrientationProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public IList<ImageInfo> ImageInfoCollection {
            get {
                return ( IList<ImageInfo> )this.GetValue( ExtraImagesViewerControl.ImageInfoCollectionProperty );
            }
            set {
                this.SetValue( ExtraImagesViewerControl.ImageInfoCollectionProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public int SelectedIndex {
            get {
                return ( int )this.GetValue( ExtraImagesViewerControl.SelectedIndexProperty );
            }
            set {
                this.SetValue( ExtraImagesViewerControl.SelectedIndexProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public event SelectedImageChangedEventHandler SelectedImageChanged {
            add {
                base.AddHandler( ExtraImagesViewerControl.SelectedImageChangedEvent, value );
            }
            remove {
                base.RemoveHandler( ExtraImagesViewerControl.SelectedImageChangedEvent, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        static ExtraImagesViewerControl() {
            DependencyPropertyInfo info;

            info = new DependencyPropertyInfo();
            info.PropertyName = "ImageInfoCollection";
            info.DataType = typeof( IList<ImageInfo> );
            info.OwnerDataType = typeof( ExtraImagesViewerControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImagesViewerControl.ImageInfoCollectionChanged );
            ExtraImagesViewerControl.ImageInfoCollectionProperty = ExtraHelperWPF.RegisterDependencyProperty( info );
            
            info = new DependencyPropertyInfo();
            info.PropertyName = "SelectedIndex";
            info.DataType = typeof( int );
            info.OwnerDataType = typeof( ExtraImagesViewerControl );
            info.PropertyChangedHandler = 
                new PropertyChangedCallback( ExtraImagesViewerControl.SelectedIndexChanged );
            ExtraImagesViewerControl.SelectedIndexProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "ImagesListOrientation";
            info.DataType = typeof( Orientation );
            info.OwnerDataType = typeof( ExtraImagesViewerControl );
            info.PropertyChangedHandler = 
                new PropertyChangedCallback( ExtraImagesViewerControl.ImagesListOrientationChanged );
            ExtraImagesViewerControl.ImagesListOrientationProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "SelectedImageInfoCollection";
            info.DataType = typeof( IList<ImageInfo> );
            info.OwnerDataType = typeof( ExtraImagesViewerControl );
            info.PropertyChangedHandler =
                new PropertyChangedCallback( ExtraImagesViewerControl.SelectedImageInfoCollectionChanged );
            ExtraImagesViewerControl.SelectedImageInfoCollectionProperty = ExtraHelperWPF.RegisterDependencyProperty( info );
            
            ExtraImagesViewerControl.SelectedImageChangedEvent = EventManager.RegisterRoutedEvent
                ( 
                    "SelectedImageChanged", RoutingStrategy.Bubble,
                    typeof( SelectedImageChangedEventHandler ), typeof( ExtraImagesViewerControl )
                );
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public ExtraImagesViewerControl() {
            InitializeComponent();
            this.Initialize();
        }
        //-------------------------------------------------------------------------------------------------------
        private void Initialize() {
            this.imagesListBox.MouseLeftButtonDown +=
                new MouseButtonEventHandler( ExtraImagesViewerControl.ImagesListBox_MouseLeftButtonDown );
        }
        //-------------------------------------------------------------------------------------------------------
        private static void ImageInfoCollectionChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImagesViewerControl extraImagesViewerControl = d as ExtraImagesViewerControl;
            IList<ImageInfo> imagesInfo = ( IList<ImageInfo> )e.NewValue;
            extraImagesViewerControl.imagesListBox.ItemsSource = imagesInfo;
        }
        //-------------------------------------------------------------------------------------------------------
        private static void ImagesListOrientationChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImagesViewerControl extraImagesViewerControl = d as ExtraImagesViewerControl;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        private static void ImagesListBox_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            
        }
        //-------------------------------------------------------------------------------------------------------
        private static void SelectedIndexChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImagesViewerControl extraImagesViewerControl = d as ExtraImagesViewerControl;
            extraImagesViewerControl.imagesListBox.SelectedIndex = ( int )e.NewValue;
        }
        //-------------------------------------------------------------------------------------------------------
        private void imagesListBoxSelectionChanged( object sender, SelectionChangedEventArgs e ) {
            ListBox imagesListBox = sender as ListBox;
            
            IList<ImageInfo> selectedImagesInfo = new List<ImageInfo>();
            IList selectedItems = imagesListBox.SelectedItems;
            for ( int index = 0; index < selectedItems.Count; index++ ) {
                object item = selectedItems[ index ];
                ImageInfo imageInfo = item as ImageInfo;
                selectedImagesInfo.Add( imageInfo );
            }
            this.SetValue( ExtraImagesViewerControl.SelectedImageInfoCollectionProperty, selectedImagesInfo );

            this.SetValue( ExtraImagesViewerControl.SelectedIndexProperty, imagesListBox.SelectedIndex );
            
            SelectedImageChangedEventArgs selectedImageChangedEventArgs = new SelectedImageChangedEventArgs();
            selectedImageChangedEventArgs.RoutedEvent = ExtraImagesViewerControl.SelectedImageChangedEvent;
            base.RaiseEvent( selectedImageChangedEventArgs );
        }
        //--------------------------------------------------------------------------------------------------------
        private static void SelectedImageInfoCollectionChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e
        ) {
            //--            
        }
        //--------------------------------------------------------------------------------------------------------
        private void ImagesListBox_KeyUp( object sender, KeyEventArgs e ) {
            
        }
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
    }
}
