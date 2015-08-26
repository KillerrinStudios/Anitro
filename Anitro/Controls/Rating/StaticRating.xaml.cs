using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Anitro.Controls.Rating
{
    public sealed partial class StaticRating : UserControl
    {
        public StaticRating()
        {
            InitializeComponent();
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
                typeof(StaticRating),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnBackgroundColorChanged)));

        /// <summary>
        /// Handles changes to the BackgroundColor property.
        /// </summary>
        private static void OnBackgroundColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            StaticRating control = (StaticRating)d;
            foreach (Star star in control.spStars.Children)
                star.BackgroundColor = (SolidColorBrush)e.NewValue;
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
                typeof(StaticRating),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnStarForegroundColorChanged)));

        /// <summary>
        /// Handles changes to the StarForegroundColor property.
        /// </summary>
        private static void OnStarForegroundColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            StaticRating control = (StaticRating)d;
            foreach (Star star in control.spStars.Children)
                star.StarForegroundColor = (SolidColorBrush)e.NewValue;

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
                typeof(StaticRating),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnStarOutlineColorChanged)));


        /// <summary>
        /// Handles changes to the StarOutlineColor property.
        /// </summary>
        private static void OnStarOutlineColorChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            StaticRating control = (StaticRating)d;
            foreach (Star star in control.spStars.Children)
                star.StarOutlineColor = (SolidColorBrush)e.NewValue;
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
                typeof(StaticRating),
                new PropertyMetadata(Star.DEFAULT_STAR_SIZE, new PropertyChangedCallback(OnStarSizeChanged)));

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnStarSizeChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            StaticRating ratingsControl = (StaticRating)d;
            SetupStars(ratingsControl);
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
                typeof(StaticRating),
                new PropertyMetadata(1.0, new PropertyChangedCallback(OnStarScaleWidthChanged)));

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnStarScaleWidthChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            StaticRating ratingsControl = (StaticRating)d;
            foreach (Star star in ratingsControl.spStars.Children)
                star.StarScaleWidth = ratingsControl.StarScaleWidth;
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
                typeof(StaticRating),
                new PropertyMetadata(1.0, new PropertyChangedCallback(OnStarScaleHeightChanged)));

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnStarScaleHeightChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            StaticRating ratingsControl = (StaticRating)d;
            foreach (Star star in ratingsControl.spStars.Children)
                star.StarScaleHeight = ratingsControl.StarScaleHeight;
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
                typeof(StaticRating),
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged)));

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            StaticRating ratingsControl = (StaticRating)d;
            SetupStars(ratingsControl);
        }
        #endregion

        #region Number of Stars
        /// <summary>
        /// Gets or sets the NumberOfStars property.  
        /// </summary>
        public Int32 NumberOfStars
        {
            get { return (Int32)GetValue(NumberOfStarsProperty); }
            set { SetValue(NumberOfStarsProperty, value); }
        }

        /// <summary>
        /// NumberOfStars Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumberOfStarsProperty =
            DependencyProperty.Register(nameof(NumberOfStars),
                typeof(Int32),
                typeof(StaticRating),
                new PropertyMetadata((Int32)5, new PropertyChangedCallback(OnNumberOfStarsChanged)));

        /// <summary>
        /// Handles changes to the NumberOfStars property.
        /// </summary>
        private static void OnNumberOfStarsChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            StaticRating ratingsControl = (StaticRating)d;
            SetupStars(ratingsControl);
        }
        #endregion

        /// <summary>
        /// Sets up stars when Value or NumberOfStars properties change
        /// Will only show up to the number of stars requested (up to Maximum)
        /// so if Value > NumberOfStars * 1, then Value is clipped to maximum
        /// number of full stars
        /// </summary>
        /// <param name="ratingsControl"></param>
        private static void SetupStars(StaticRating ratingsControl)
        {
            double localValue = ratingsControl.Value;

            ratingsControl.spStars.Children.Clear();
            for (int i = 0; i < ratingsControl.NumberOfStars; i++)
            {
                Star star = new Star();
                star.BackgroundColor = ratingsControl.BackgroundColor;
                star.StarForegroundColor = ratingsControl.StarForegroundColor;
                star.StarOutlineColor = ratingsControl.StarOutlineColor;
                star.StarScaleWidth = ratingsControl.StarScaleWidth;
                star.StarScaleHeight = ratingsControl.StarScaleHeight;
                star.StarSize = ratingsControl.StarSize;

                if (localValue > 1)
                    star.Value = 1.0;
                else if (localValue > 0)
                {
                    star.Value = localValue;
                }
                else
                {
                    star.Value = 0.0;
                }

                localValue -= 1.0;
                ratingsControl.spStars.Children.Insert(i, star);
            }
        }
    }
}
