using Anitro.Controls;
using Anitro.Helpers;
using Anitro.Models.Enums;
using Anitro.Services;
using Anitro.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Anitro.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Instance;
        public MainViewModel ViewModel { get { return (MainViewModel)DataContext; } }

        public MainPage()
        {
            Instance = this;
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.OnNavigatedTo();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.OnNavigatedFrom();
            base.OnNavigatedFrom(e);
        }

        private void MainNavigation_HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainNavigation.IsPaneOpen = !MainNavigation.IsPaneOpen;
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DefaultNoUserPage));
            ViewModel.CurrentNavigationLocation = NavigationLocation.Default;

            if (!SimpleIoc.Default.IsRegistered<NavigationService>())
            {
                SimpleIoc.Default.Register<NavigationService>(() => { return new NavigationService(MainFrame); });
            }
        }

        private void MainProgressIndicator_Loaded(object sender, RoutedEventArgs e)
        {
            if (!SimpleIoc.Default.IsRegistered<ProgressService>())
            {
                SimpleIoc.Default.Register<ProgressService>(() => { return new ProgressService(MainProgressIndicator); });
            }
        }
    }
}
