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

using ExtraWPF;
using ExtraLibrary.OS;

namespace ExtraControls {
    /// <summary>
    /// Логика взаимодействия для HayImage.xaml
    /// </summary>
    public partial class ExtraImageControl : UserControl {

        public static readonly DependencyProperty SourceProperty;
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty TextVisibilityProperty;
        public static readonly DependencyProperty ScrollVisibilityProperty;
        public static readonly DependencyProperty CheckedProperty;
        public static readonly DependencyProperty CheckedVisibilityProperty;
        public static readonly DependencyProperty ImageSizeVisibilityProperty;
        public static readonly DependencyProperty MousePositionTrackingProperty;
        
        //Events
        public static readonly RoutedEvent ImageMouseMoveEvent;

        //-------------------------------------------------------------------------------------------------------
        public ExtraImageControl() {
            InitializeComponent();
            this.Initialize();
        }
        //-------------------------------------------------------------------------------------------------------
        private void Initialize() {
            this.image.SizeChanged += new SizeChangedEventHandler( Image_SizeChanged );
        }
        //-------------------------------------------------------------------------------------------------------
        private void Image_SizeChanged( object sender, SizeChangedEventArgs e ) {
            this.textLabel.MaxWidth = e.NewSize.Width;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public Visibility ImageSizeVisibility {
            get {
                return ( Visibility )this.GetValue( ExtraImageControl.ImageSizeVisibilityProperty );
            }
            set {
                this.SetValue( ExtraImageControl.ImageSizeVisibilityProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public Visibility TextVisibility {
            get {
                return ( Visibility )this.GetValue( ExtraImageControl.TextVisibilityProperty );
            }
            set {
                this.SetValue( ExtraImageControl.TextVisibilityProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public Visibility CheckedVisibility {
            get {
                return ( Visibility )this.GetValue( ExtraImageControl.CheckedVisibilityProperty );
            }
            set {
                this.SetValue( ExtraImageControl.CheckedVisibilityProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public bool Checked {
            get {
                return ( bool )this.GetValue( ExtraImageControl.CheckedProperty );
            }
            set {
                this.SetValue( ExtraImageControl.CheckedProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public ImageSource Source {
            get {
                return ( ImageSource )this.GetValue( ExtraImageControl.SourceProperty );
            }
            set {
                this.SetValue( ExtraImageControl.SourceProperty, value );
            }
        }

        //-------------------------------------------------------------------------------------------------------
        public string Text {
            get {
                return ( string )this.GetValue( ExtraImageControl.TextProperty );
            }
            set {
                this.SetValue( ExtraImageControl.TextProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public ScrollBarVisibility ScrollVisibility{
            get {
                return ( ScrollBarVisibility )this.GetValue( ExtraImageControl.ScrollVisibilityProperty );
            }
            set {
                this.SetValue( ExtraImageControl.ScrollVisibilityProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public bool MousePositionTracking {
            get {
                return ( bool )this.GetValue( ExtraImageControl.MousePositionTrackingProperty );
            }
            set {
                this.SetValue( ExtraImageControl.MousePositionTrackingProperty, value );
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public event MouseEventHandler ImageMouseMove {
            add {
                this.image.MouseMove += value;
            }
            remove {
                this.image.MouseMove -= value;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        static ExtraImageControl() {
            DependencyPropertyInfo info;
            
            info = new DependencyPropertyInfo();
            info.PropertyName = "Text";
            info.DataType = typeof( string );
            info.OwnerDataType = typeof( ExtraImageControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImageControl.TextChanged );
            ExtraImageControl.TextProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "Source";
            info.DataType = typeof( BitmapSource );
            info.OwnerDataType = typeof( ExtraImageControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImageControl.SourceChanged );
            ExtraImageControl.SourceProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "ScrollVisibility";
            info.DataType = typeof( ScrollBarVisibility );
            info.OwnerDataType = typeof( ExtraImageControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImageControl.ScrollVisibilityChanged );
            ExtraImageControl.ScrollVisibilityProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "Checked";
            info.DataType = typeof( bool );
            info.OwnerDataType = typeof( ExtraImageControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImageControl.CheckedChanged );
            ExtraImageControl.CheckedProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "CheckedVisibility";
            info.DataType = typeof( Visibility );
            info.OwnerDataType = typeof( ExtraImageControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImageControl.CheckedVisibilityChanged );
            ExtraImageControl.CheckedVisibilityProperty = ExtraHelperWPF.RegisterDependencyProperty( info );
            
            info = new DependencyPropertyInfo();
            info.PropertyName = "ImageSizeVisibility";
            info.DataType = typeof( Visibility );
            info.OwnerDataType = typeof( ExtraImageControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImageControl.ImageSizeVisibilityChanged );
            ExtraImageControl.ImageSizeVisibilityProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "TextVisibility";
            info.DataType = typeof( Visibility );
            info.OwnerDataType = typeof( ExtraImageControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImageControl.TextVisibilityChanged );
            ExtraImageControl.TextVisibilityProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            info = new DependencyPropertyInfo();
            info.PropertyName = "MousePositionTracking";
            info.DataType = typeof( Visibility );
            info.OwnerDataType = typeof( ExtraImageControl );
            info.PropertyChangedHandler = new PropertyChangedCallback( ExtraImageControl.MousePositionTrackingChanged );
            ExtraImageControl.MousePositionTrackingProperty = ExtraHelperWPF.RegisterDependencyProperty( info );

            ExtraImageControl.ImageMouseMoveEvent = EventManager.RegisterRoutedEvent
                (
                    "ImageMouseMove", RoutingStrategy.Bubble,
                    typeof( MouseEventHandler ), typeof( ExtraImageControl )
                );
        }
        //--------------------------------------------------------------------------------------------------------
        private static void CheckedVisibilityChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImageControl extraImageControl = d as ExtraImageControl;
            extraImageControl.checkBox.Visibility = ( Visibility )e.NewValue;
        }
        //--------------------------------------------------------------------------------------------------------
        private static void TextChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImageControl extraImageControl = d as ExtraImageControl;
            extraImageControl.textLabel.Content = ( string )e.NewValue;
        }
        //-------------------------------------------------------------------------------------------------------
        private static void SourceChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImageControl extraImageControl = d as ExtraImageControl;
            BitmapSource imageSource = ( BitmapSource )e.NewValue;
            extraImageControl.image.Source = imageSource;
            
            extraImageControl.imageSizeText.Content =
                imageSource.PixelWidth.ToString() + " x " + imageSource.PixelHeight.ToString();
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        private static void ScrollVisibilityChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImageControl extraImageControl = d as ExtraImageControl;
            extraImageControl.scrollViewer.VerticalScrollBarVisibility = ( ScrollBarVisibility )e.NewValue;
            extraImageControl.scrollViewer.HorizontalScrollBarVisibility = ( ScrollBarVisibility )e.NewValue;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        private static void CheckedChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImageControl extraImageControl = d as ExtraImageControl;
            extraImageControl.checkBox.IsChecked = ( bool? )e.NewValue;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        private static void ImageSizeVisibilityChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImageControl extraImageControl = d as ExtraImageControl;
            extraImageControl.imageSizeText.Visibility = ( Visibility )e.NewValue;
        }
        //-------------------------------------------------------------------------------------------------------
        private static void TextVisibilityChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImageControl extraImageControl = d as ExtraImageControl;
            extraImageControl.textLabel.Visibility = ( Visibility )e.NewValue;
        }
        //-------------------------------------------------------------------------------------------------------
        private static void MousePositionTrackingChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            ExtraImageControl extraImageControl = d as ExtraImageControl;
        }
        //-------------------------------------------------------------------------------------------------------
        private void CheckBox_Checked( object sender, RoutedEventArgs e ) {
            this.Checked = true;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        private void CheckBox_Unchecked( object sender, RoutedEventArgs e ) {
            this.Checked = false;
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
    }
}
