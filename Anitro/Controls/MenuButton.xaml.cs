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
    public sealed partial class MenuButton : Button
    {
        public MenuButton()
        {
            this.InitializeComponent();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                LayoutRoot.DataContext = this;
            }
        }

        #region IsSelected
        public bool? IsSelected
        {
            get { return (bool?)GetValue(IsSelectedProperty); }
            set
            {
                if (value == null) return;
                SetValue(IsSelectedProperty, value);
            }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool?), typeof(MenuButton), new PropertyMetadata(false, OnIsSelectedPropertyChanged));

        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuButton button = (d as MenuButton);
            if (button.IsSelected.Value)
                button.Background = (Application.Current.Resources["SystemControlBackgroundAccentBrush"] as SolidColorBrush);
            else
                button.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
        }
        #endregion

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

    }
}
