﻿

#pragma checksum "C:\Users\Andrew\Documents\GitHub\Anitro\Anitro 8.1\Anitro\Anitro.WindowsPhone\LibraryPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9E50FE6E7FC9D4E93D642EF3D432F20F"
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
    partial class LibraryPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 10 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.LibraryPage_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 15 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Search_Clicked;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 16 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Refresh_Clicked;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 19 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.About_Clicked;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 20 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ClearRecent_Clicked;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 74 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.Favourites_GridView_Clicked;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 68 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Library_Tapped;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 61 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Library_Tapped;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 54 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Library_Tapped;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 47 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Library_Tapped;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 40 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Library_Tapped;
                 #line default
                 #line hidden
                break;
            case 12:
                #line 31 "..\..\LibraryPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.Recent_GridView_Clicked;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


