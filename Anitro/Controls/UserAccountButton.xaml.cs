using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class UserAccountButton : Button
    {
        public UserAccountButton()
        {
            this.InitializeComponent();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                LayoutRoot.DataContext = this;
            }
        }

        #region Dependency Properties
        #region IsSelected
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(UserAccountButton), new PropertyMetadata(false, OnIsSelectedPropertyChanged));

        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserAccountButton button = (d as UserAccountButton);
            if (button.IsSelected)
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
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(UserAccountButton), new PropertyMetadata(""));
        #endregion

        #region Username
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set
            {
                if (value == null) return;
                SetValue(UsernameProperty, value);
            }
        }
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register(nameof(Username), typeof(string), typeof(UserAccountButton), new PropertyMetadata("", OnUsernamePropertyChanged));

        private static void OnUsernamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserAccountButton button = (d as UserAccountButton);
            if (button.IsLoggedIn)
                button.LoginLogoutMenuFlyoutItem.Text = "Logout";
            else button.LoginLogoutMenuFlyoutItem.Text = "Login";
        }
        #endregion

        #region Avatar
        public Uri Avatar
        {
            get { return (Uri)GetValue(AvatarProperty); }
            set
            {
                if (value == null) return;
                SetValue(AvatarProperty, value);
            }
        }
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register(nameof(Avatar), typeof(Uri), typeof(UserAccountButton), new PropertyMetadata(new Uri("http://www.example.com/", UriKind.Absolute), OnAvatarPropertyChanged));

        private static void OnAvatarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserAccountButton button = (d as UserAccountButton);
            button.avatarImage.Source = new BitmapImage(button.Avatar);
        }
        #endregion
        #endregion

        public bool IsLoggedIn { get { return !string.IsNullOrWhiteSpace(Username); } }

        #region Commands
        #region Login Command
        public RelayCommand<string> LoginCommand
        {
            get { return (RelayCommand<string>)GetValue(LoginCommandProperty); }
            set
            {
                if (value == null) return;
                SetValue(LoginCommandProperty, value);
            }
        }
        public static readonly DependencyProperty LoginCommandProperty =
            DependencyProperty.Register(nameof(LoginCommand), typeof(RelayCommand<string>), typeof(UserAccountButton), new PropertyMetadata(new RelayCommand<string>((s) => { })));
        #endregion

        #region Logout Command
        public RelayCommand<string> LogoutCommand
        {
            get { return (RelayCommand<string>)GetValue(LogoutCommandProperty); }
            set
            {
                if (value == null) return;
                SetValue(LogoutCommandProperty, value);
            }
        }
        public static readonly DependencyProperty LogoutCommandProperty =
            DependencyProperty.Register(nameof(LogoutCommand), typeof(RelayCommand<string>), typeof(UserAccountButton), new PropertyMetadata(new RelayCommand<string>((s) => { })));
        #endregion

        #region Switch User Command
        public RelayCommand<string> SwitchUserCommand
        {
            get { return (RelayCommand<string>)GetValue(SwitchUserCommandProperty); }
            set
            {
                if (value == null) return;
                SetValue(SwitchUserCommandProperty, value);
            }
        }
        public static readonly DependencyProperty SwitchUserCommandProperty =
            DependencyProperty.Register(nameof(SwitchUserCommand), typeof(RelayCommand<string>), typeof(UserAccountButton), new PropertyMetadata(new RelayCommand<string>((s) => { })));
        #endregion
        #endregion


        public event TappedEventHandler LoginTapped;
        public event TappedEventHandler LogoutTapped;
        private void LoginLogoutMenuFlyoutItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsLoggedIn)
            {
                if (LogoutTapped != null)
                    LogoutTapped(this, e);

                if (LogoutCommand.CanExecute(Header))
                    LogoutCommand.Execute(Header);
            }
            else
            {
                if (LoginTapped != null)
                    LoginTapped(this, e);

                if (LoginCommand.CanExecute(Header))
                    LoginCommand.Execute(Header);
            }
        }

        public event TappedEventHandler SwitchUserTapped;
        private void SwitchMenuFlyoutItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsLoggedIn)
            {
                if (IsSelected) return;

                if (SwitchUserTapped != null)
                    SwitchUserTapped(this, e);

                if (SwitchUserCommand.CanExecute(Header))
                    SwitchUserCommand.Execute(Header);
            }
            else
            {
                if (LoginTapped != null)
                    LoginTapped(this, e);

                if (LoginCommand.CanExecute(Header))
                    LoginCommand.Execute(Header);
            }
        }
    }
}
