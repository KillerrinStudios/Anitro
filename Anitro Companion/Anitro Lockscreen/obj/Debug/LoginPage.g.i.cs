﻿#pragma checksum "C:\Users\Andrew\Documents\GitHub\Anitro\Anitro Companion\Anitro Lockscreen\LoginPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "645A6A98CC8BED5834EAB1C6AB883D07"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Anitro_Lockscreen {
    
    
    public partial class LoginPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar ApplicationProgressBar;
        
        internal System.Windows.Controls.Grid LoginLayout;
        
        internal System.Windows.Controls.TextBlock loginErrors;
        
        internal Microsoft.Phone.Controls.PhoneTextBox usernameBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox passwordBox;
        
        internal System.Windows.Controls.Button LoginButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Anitro%20Lockscreen;component/LoginPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ApplicationProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("ApplicationProgressBar")));
            this.LoginLayout = ((System.Windows.Controls.Grid)(this.FindName("LoginLayout")));
            this.loginErrors = ((System.Windows.Controls.TextBlock)(this.FindName("loginErrors")));
            this.usernameBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("usernameBox")));
            this.passwordBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("passwordBox")));
            this.LoginButton = ((System.Windows.Controls.Button)(this.FindName("LoginButton")));
        }
    }
}

