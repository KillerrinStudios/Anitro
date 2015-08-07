using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Anitro.Controls
{
    public sealed class UserAccountButton : Control
    {
        public UserAccountButton()
        {
            this.DefaultStyleKey = typeof(UserAccountButton);
        }

        #region Header
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(string),
                typeof(UserAccountButton),
                new PropertyMetadata("", new PropertyChangedCallback(OnHeaderChanged)));

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region Username
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register(
                nameof(Username),
                typeof(string),
                typeof(UserAccountButton),
                new PropertyMetadata("", new PropertyChangedCallback(OnUsernameChanged)));

        private static void OnUsernameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region Avatar
        public ImageSource Avatar
        {
            get { return (ImageSource)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register(
                nameof(Avatar),
                typeof(ImageSource),
                typeof(UserAccountButton),
                new PropertyMetadata(null, new PropertyChangedCallback(OnAvatarChanged)));

        private static void OnAvatarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
    }
}
