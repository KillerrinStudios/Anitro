﻿#pragma checksum "C:\Users\Andrew\Documents\GitHub\Anitro\Anitro Companion\Anitro Lockscreen\SettingsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "49375C03E2B65F672228BE86AFD063A1"
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
    
    
    public partial class SettingsPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar ApplicationProgressBar;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.Pivot MainPivot;
        
        internal Microsoft.Phone.Controls.PivotItem generalPivot;
        
        internal System.Windows.Controls.Grid LoggedIn;
        
        internal System.Windows.Controls.TextBlock logoutText;
        
        internal System.Windows.Controls.TextBlock logoutUsernameText;
        
        internal System.Windows.Controls.Button logoutButton;
        
        internal System.Windows.Controls.TextBlock scheduledTaskRunner;
        
        internal System.Windows.Controls.Grid LoggedOut;
        
        internal System.Windows.Controls.Button LoginButton;
        
        internal Microsoft.Phone.Controls.PivotItem lockscreenPivot;
        
        internal Microsoft.Phone.Controls.ToggleSwitch lockscreenSwitch;
        
        internal System.Windows.Controls.Button lockscreenUpdate;
        
        internal System.Windows.Controls.Button lockscreenSettingsButton;
        
        internal Microsoft.Phone.Controls.ListPicker backgroundTaskSelector;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Anitro%20Lockscreen;component/SettingsPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ApplicationProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("ApplicationProgressBar")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.MainPivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("MainPivot")));
            this.generalPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("generalPivot")));
            this.LoggedIn = ((System.Windows.Controls.Grid)(this.FindName("LoggedIn")));
            this.logoutText = ((System.Windows.Controls.TextBlock)(this.FindName("logoutText")));
            this.logoutUsernameText = ((System.Windows.Controls.TextBlock)(this.FindName("logoutUsernameText")));
            this.logoutButton = ((System.Windows.Controls.Button)(this.FindName("logoutButton")));
            this.scheduledTaskRunner = ((System.Windows.Controls.TextBlock)(this.FindName("scheduledTaskRunner")));
            this.LoggedOut = ((System.Windows.Controls.Grid)(this.FindName("LoggedOut")));
            this.LoginButton = ((System.Windows.Controls.Button)(this.FindName("LoginButton")));
            this.lockscreenPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("lockscreenPivot")));
            this.lockscreenSwitch = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("lockscreenSwitch")));
            this.lockscreenUpdate = ((System.Windows.Controls.Button)(this.FindName("lockscreenUpdate")));
            this.lockscreenSettingsButton = ((System.Windows.Controls.Button)(this.FindName("lockscreenSettingsButton")));
            this.backgroundTaskSelector = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("backgroundTaskSelector")));
        }
    }
}

