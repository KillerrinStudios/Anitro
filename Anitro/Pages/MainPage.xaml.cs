using Anitro.Controls;
using Anitro.Helpers;
using Anitro.Models.Enums;
using Anitro.Models.Page_Parameters;
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
using Microsoft.AdMediator.Universal;
using Microsoft.AdMediator.Core;
using Microsoft.AdMediator.Core.Models;

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Loaded();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LaunchArgs = e.Parameter as AnitroLaunchArgs;
            ViewModel.OnNavigatedTo();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.OnNavigatedFrom();
            base.OnNavigatedFrom(e);
        }

        #region Unavoidable Control Events 
        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ViewModel.CurrentVisualState = e.NewState;
        }
        #endregion

        #region Loaded
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

        private void MainMediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            if (!SimpleIoc.Default.IsRegistered<MediaService>())
            {
                SimpleIoc.Default.Register<MediaService>(() => { return new MediaService(MainMediaElement); });
            }
        }
        #endregion

        #region AdMediator
        private void AdMediator_Loaded(object sender, RoutedEventArgs e)
        {
            AdMediatorControl AdMediator = sender as AdMediatorControl;
            //AdMediator.AdSdkOptionalParameters[AdSdkNames.MicrosoftAdvertising]["Width"] = 728;
            //AdMediator.AdSdkOptionalParameters[AdSdkNames.MicrosoftAdvertising]["Height"] = 90;
        }

        private void AdMediator_AdSdkError(object sender, Microsoft.AdMediator.Core.Events.AdFailedEventArgs e)
        {
            Debug.WriteLine("AdSdkError by {0} ErrorCode: {1} ErrorDescription: {2} Error: {3}", e.Name, e.ErrorCode, e.ErrorDescription, e.Error);
        }

        private void AdMediator_AdSdkEvent(object sender, Microsoft.AdMediator.Core.Events.AdSdkEventArgs e)
        {
            Debug.WriteLine("AdSdk event {0} by {1}", e.EventName, e.Name);
        }

        private void AdMediator_AdMediatorFilled(object sender, Microsoft.AdMediator.Core.Events.AdSdkEventArgs e)
        {
            Debug.WriteLine("AdFilled:" + e.Name);
        }

        private void AdMediator_AdMediatorError(object sender, Microsoft.AdMediator.Core.Events.AdMediatorFailedEventArgs e)
        {
            Debug.WriteLine("AdMediatorError:" + e.Error + " " + e.ErrorCode);
            // if (e.ErrorCode == AdMediatorErrorCode.NoAdAvailable)
            // AdMediator will not show an ad for this mediation cycle
        }
        #endregion
    }
}
