﻿

#pragma checksum "C:\Users\killer rin\Documents\GitHub\Anitro\Anitro 8.1\Anitro\Anitro.WindowsPhone\UserDashboardPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "95D7339A2F58D007872CE6F838309195"
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
    partial class UserDashboardPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton refreshUserInfo; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressRing ApplicationProgressBar; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock debugTextBlock; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.MediaElement player; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Hub DashboardHub; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.HubSection General_HubSection; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.HubSection UserStats_HubSection; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.HubSection Social_HubSection; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///UserDashboardPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            refreshUserInfo = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("refreshUserInfo");
            ApplicationProgressBar = (global::Windows.UI.Xaml.Controls.ProgressRing)this.FindName("ApplicationProgressBar");
            debugTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("debugTextBlock");
            player = (global::Windows.UI.Xaml.Controls.MediaElement)this.FindName("player");
            DashboardHub = (global::Windows.UI.Xaml.Controls.Hub)this.FindName("DashboardHub");
            General_HubSection = (global::Windows.UI.Xaml.Controls.HubSection)this.FindName("General_HubSection");
            UserStats_HubSection = (global::Windows.UI.Xaml.Controls.HubSection)this.FindName("UserStats_HubSection");
            Social_HubSection = (global::Windows.UI.Xaml.Controls.HubSection)this.FindName("Social_HubSection");
        }
    }
}



