using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Anitro.Controls.Rating
{
    public sealed partial class Star : UserControl
    {
        public const Int32 DEFAULT_STAR_SIZE = 12;

        public Star()
        {
            this.DataContext = this;
            InitializeComponent();

            gdStar.Width = DEFAULT_STAR_SIZE;
            gdStar.Height = DEFAULT_STAR_SIZE;
            gdStar.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, DEFAULT_STAR_SIZE, DEFAULT_STAR_SIZE)
            };

            mask.Width = DEFAULT_STAR_SIZE;
            mask.Height = DEFAULT_STAR_SIZE;
            this.SizeChanged += new SizeChangedEventHandler(Star_SizeChanged);
        }

        void Star_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        #region Background Color
        /// <summary>
        /// Gets or sets the BackgroundColor property.  
        /// </summary>
        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// BackgroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor),
            typeof(SolidColorBrush),
            typeof(Star),
            new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnBackgroundColorChanged)));

        /// <summary>
        /// Handles changes to the BackgroundColor property.
        /// </summary>
        private static void OnBackgroundColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Star control = (Star)d;
            control.gdStar.Background = (SolidColorBrush)e.NewValue;
            control.mask.Fill = (SolidColorBrush)e.NewValue;
        }
        #endregion

        #region Star Foreground Color
        /// <summary>
        /// Gets or sets the StarForegroundColor property.  
        /// </summary>
        public SolidColorBrush StarForegroundColor
        {
            get { return (SolidColorBrush)GetValue(StarForegroundColorProperty); }
            set { SetValue(StarForegroundColorProperty, value); }
        }

        /// <summary>
        /// StarForegroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty StarForegroundColorProperty =
            DependencyProperty.Register(nameof(StarForegroundColor),
                typeof(SolidColorBrush),
                typeof(Star),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnStarForegroundColorChanged)));

        /// <summary>
        /// Handles changes to the StarForegroundColor property.
        /// </summary>
        private static void OnStarForegroundColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Star control = (Star)d;
            control.FillStarColor();
        }
        #endregion

        #region Star Outline Color
        /// <summary>
        /// Gets or sets the StarOutlineColor property.  
        /// </summary>
        public SolidColorBrush StarOutlineColor
        {
            get { return (SolidColorBrush)GetValue(StarOutlineColorProperty); }
            set { SetValue(StarOutlineColorProperty, value); }
        }

        /// <summary>
        /// StarOutlineColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty StarOutlineColorProperty =
            DependencyProperty.Register(nameof(StarOutlineColor),
                typeof(SolidColorBrush),
                typeof(Star),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnStarOutlineColorChanged)));

        /// <summary>
        /// Handles changes to the StarOutlineColor property.
        /// </summary>
        private static void OnStarOutlineColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Star control = (Star)d;
            control.starOutline.Stroke = (SolidColorBrush)e.NewValue;
        }
        #endregion

        #region Value
        /// <summary>
        /// Gets or sets the Value property.  
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                typeof(double),
                typeof(Star),
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged)));

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Star starControl = (Star)d;

            starControl.FillStarColor();

            Int32 marginLeftOffset = (Int32)(starControl.Value * (double)starControl.StarSize);
            starControl.InvalidateArrange();
            starControl.InvalidateMeasure();
        }
        #endregion

        #region Star Size
        /// <summary>
        /// Gets or sets the Value property.  
        /// </summary>
        public int StarSize
        {
            get { return (int)GetValue(StarSizeProperty); }
            set { SetValue(StarSizeProperty, value); }
        }

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty StarSizeProperty =
            DependencyProperty.Register(nameof(StarSize),
                typeof(int),
                typeof(Star),
                new PropertyMetadata(DEFAULT_STAR_SIZE, new PropertyChangedCallback(OnStarSizeChanged)));

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnStarSizeChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Star starControl = (Star)d;

            starControl.gdStar.Width = starControl.StarSize;
            starControl.gdStar.Height = starControl.StarSize;
            starControl.gdStar.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, starControl.StarSize, starControl.StarSize)
            };
            starControl.mask.Width = starControl.StarSize;
            starControl.mask.Height = starControl.StarSize;

            starControl.FillStarColor();

            Int32 marginLeftOffset = (Int32)(starControl.Value * (double)starControl.StarSize);
            starControl.InvalidateArrange();
            starControl.InvalidateMeasure();
        }
        #endregion

        #region Star Scale Width
        /// <summary>
        /// Gets or sets the Value property.  
        /// </summary>
        public double StarScaleWidth
        {
            get { return (double)GetValue(StarScaleWidthProperty); }
            set { SetValue(StarScaleWidthProperty, value); }
        }

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty StarScaleWidthProperty =
            DependencyProperty.Register(nameof(StarScaleWidth),
                typeof(double),
                typeof(Star),
                new PropertyMetadata(1.0, new PropertyChangedCallback(OnStarScaleWidthChanged)));

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnStarScaleWidthChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Star starControl = (Star)d;
        }
        #endregion

        #region Star Scale Height
        /// <summary>
        /// Gets or sets the Value property.  
        /// </summary>
        public double StarScaleHeight
        {
            get { return (double)GetValue(StarScaleHeightProperty); }
            set { SetValue(StarScaleHeightProperty, value); }
        }

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty StarScaleHeightProperty =
            DependencyProperty.Register(nameof(StarScaleHeight),
                typeof(double),
                typeof(Star),
                new PropertyMetadata(1.0, new PropertyChangedCallback(OnStarScaleHeightChanged)));

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnStarScaleHeightChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Star starControl = (Star)d;
        }
        #endregion

        private void FillStarColor()
        {
            if (Value == 0)
            {
                starForeground.Fill = new SolidColorBrush(Colors.Gray);
            }
            else if (Value > 0 && Value < 1)
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.StartPoint = new Point(0, 0);
                brush.EndPoint = new Point(1, 0);

                GradientStop stop1 = new GradientStop();
                stop1.Offset = Value;
                stop1.Color = StarForegroundColor.Color;

                GradientStop stop2 = new GradientStop();
                stop2.Offset = 1 - Value;
                stop2.Color = Colors.Gray;

                brush.GradientStops.Add(stop1);
                brush.GradientStops.Add(stop2);
                starForeground.Fill = brush;
            }
            else
            {
                starForeground.Fill = StarForegroundColor;
            }
        }
    }
}
