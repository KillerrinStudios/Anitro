using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Anitro.Controls
{
    public sealed partial class MenuButton : RadioButton
    {
        public MenuButton()
        {
            this.InitializeComponent();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                LayoutRoot.DataContext = this;
            }
        }

        #region Header
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set
            {
                if (value == null) return;
                SetValue(HeaderProperty, value);
            }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(MenuButton), new PropertyMetadata(""));
        #endregion

        #region Avatar
        public Uri HeaderImage
        {
            get { return (Uri)GetValue(HeaderImageProperty); }
            set
            {
                if (value == null) return;
                SetValue(HeaderImageProperty, value);
            }
        }
        public static readonly DependencyProperty HeaderImageProperty =
            DependencyProperty.Register(nameof(HeaderImage), typeof(Uri), typeof(MenuButton), new PropertyMetadata(new Uri("http://www.example.com/", UriKind.Absolute), OnAvatarPropertyChanged));

        private static void OnAvatarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuButton button = (d as MenuButton);
            button.menuHeaderImage.Source = new BitmapImage(button.HeaderImage);
        }
        #endregion

        #region Symbol
        public Symbol Symbol
        {
            get { return (Symbol)GetValue(SymbolProperty); }
            set
            {
                SetValue(SymbolProperty, value);
            }
        }
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(nameof(Symbol), typeof(Symbol), typeof(MenuButton), new PropertyMetadata(Symbol.Emoji, OnSymbolPropertyChanged));

        private static void OnSymbolPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuButton button = (d as MenuButton);
            button.menuSymbolImage.Symbol = button.Symbol;
        }
        #endregion

        #region SymbolVisibility
        public Visibility SymbolIconVisibility
        {
            get { return (Visibility)GetValue(SymbolIconVisibilityProperty); }
            set
            {
                SetValue(SymbolIconVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty SymbolIconVisibilityProperty =
            DependencyProperty.Register(nameof(SymbolIconVisibility), typeof(Visibility), typeof(MenuButton), new PropertyMetadata(Visibility.Collapsed, OnSymbolVisibilityPropertyChanged));

        private static void OnSymbolVisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuButton button = (d as MenuButton);
            button.menuSymbolImage.Visibility = button.SymbolIconVisibility;
        }
        #endregion

        private void parent_Checked(object sender, RoutedEventArgs e)
        {
            SetBackground();
        }

        private void parent_Unchecked(object sender, RoutedEventArgs e)
        {
            SetBackground();
        }

        private void SetBackground()
        {
            if (IsChecked.Value)
                LayoutRoot.Background = (Application.Current.Resources["SystemControlBackgroundAccentBrush"] as SolidColorBrush);
            else
                LayoutRoot.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
        }
    }
}
