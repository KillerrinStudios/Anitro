﻿

#pragma checksum "C:\Users\Andrew\Documents\GitHub\Anitro\Anitro 8.1\Anitro\Anitro.Windows\LoginSettingsPane.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D47E6128B3C5527FD14C509E437173B1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Anitro
{
    partial class LoginSettingsPane : global::Windows.UI.Xaml.Controls.SettingsFlyout
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock loginErrors; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBox usernameBox; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.PasswordBox passwordBox; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel buttonStackPanel; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button LoginButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressRing ApplicationProgressBar; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///LoginSettingsPane.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            loginErrors = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("loginErrors");
            usernameBox = (global::Windows.UI.Xaml.Controls.TextBox)this.FindName("usernameBox");
            passwordBox = (global::Windows.UI.Xaml.Controls.PasswordBox)this.FindName("passwordBox");
            buttonStackPanel = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("buttonStackPanel");
            LoginButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("LoginButton");
            ApplicationProgressBar = (global::Windows.UI.Xaml.Controls.ProgressRing)this.FindName("ApplicationProgressBar");
        }
    }
}



