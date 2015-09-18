using AnimeTrackingServiceWrapper;
using Anitro.Models;
using Anitro.Models.Enums;
using Anitro.Pages;
using Anitro.Pages.Hummingbird;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using System.Diagnostics;
using System;
using Anitro.Models.Page_Parameters;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Windows.UI.Xaml;
using Anitro.Helpers;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Killerrin_Studios_Toolkit;

namespace Anitro.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : AnitroViewModelBase
    {
        public static MainViewModel Instance {  get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }
        public VisualState CurrentVisualState;

        #region Properties
        public RelayCommand TogglePaneCommand { get { return new RelayCommand(() => { IsPaneOpen = !IsPaneOpen; }); } }
        private bool m_isPaneOpen = true;
        public bool IsPaneOpen
        {
            get { return m_isPaneOpen; }
            set
            {
                if (m_isPaneOpen == value) return;
                m_isPaneOpen = value;
                RaisePropertyChanged(nameof(IsPaneOpen));
            }
        }

        private NavigationLocation m_currentNavigationLocation = NavigationLocation.Default;
        public NavigationLocation CurrentNavigationLocation
        {
            get { return m_currentNavigationLocation; }
            set
            {
                if (m_currentNavigationLocation == value) return;
                m_currentNavigationLocation = value;
                RaisePropertyChanged(nameof(CurrentNavigationLocation));

                TopNavBarText = Helpers.StringHelpers.AddSpacesToSentence(m_currentNavigationLocation.ToString(), true);
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                HummingbirdUser_LoggedIn = new HummingbirdUser();
                HummingbirdUser_LoggedIn.UserInfo.Username = "Design View";
                HummingbirdUser_LoggedIn.UserInfo.AvatarUrl = new System.Uri("https://static.hummingbird.me/users/avatars/000/007/415/thumb/TyrilCropped1.png?1401236074", System.UriKind.Absolute);
                HummingbirdUser_LoggedIn.Selected = true;
            }
            else
            {
                // Code runs "for real"
            }

            ResetViewModel();
        }

        public override void Loaded()
        {
            CortanaTools.InstallCortanaVDFile(new Uri("ms-appx:///AnitroVoiceCommandDefinition.xml"));
            InitialSetup();

            // Handle Launch Args
            if (!LaunchArgs.Handled)
            {
                if (LaunchArgs.LaunchReason == AnitroLaunchReason.Normal)
                    LaunchArgs.Handled = true;
                else if (LaunchArgs.LaunchReason == AnitroLaunchReason.Lockscreen)
                {
                    NavigationService.Navigate(typeof(SettingsPage), null);
                    LaunchArgs.Handled = true;
                }
            }
        }

        public override void OnNavigatedTo()
        {
        }

        public override void OnNavigatedFrom()
        {

        }

        public override void ResetViewModel()
        {
            //CurrentUser = null;
            //HummingbirdUser = new HummingbirdUser();
            //CurrentNavigationLocation = NavigationLocation.Default;
        }

        #region Startup Methods
        private async Task InitialSetup()
        {
            HummingbirdUser_LoggedIn = await HummingbirdUser.Load();
            await CheckProductLicense();
            NavigateToDefault();
        }

        public async Task CheckProductLicense()
        {
            Progress<APIProgressReport> m_productLicense = new Progress<APIProgressReport>();
            m_productLicense.ProgressChanged += M_productLicense_ProgressChanged;
            InAppPurchaseHelper.CheckProductInformation(m_productLicense);
        }
        private void M_productLicense_ProgressChanged(object sender, APIProgressReport e)
        {
            if (e.CurrentAPIResonse == APIResponse.Successful)
                Debug.WriteLine("Product License retrieved successfully");
            else
                Debug.WriteLine("Product License couldn't be retrieved: Setting to Default");
            Debug.WriteLine(AnitroLicense.ToString());
        }

        private void NavigateToDefault()
        {
            if (HummingbirdUser_LoggedIn.LoginInfo.IsUserLoggedIn)
            {
                SwitchUser(ServiceName.Hummingbird, HummingbirdUser_LoggedIn);
                NavigationService.RemoveLastPage();
            }
        }
        #endregion

        #region Navigation Commands
        #region Navigate Dashboard Command
        public RelayCommand NavigateDashboardCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavigateDashboard();
                });
            }
        }

        public void NavigateDashboard()
        {
            Debug.WriteLine("Navigate Dashboard");
            if (!CanNavigate)
                return;

            if (CurrentVisualState?.Name != AdaptiveTriggerConsts.DesktopMinimumWidthName)
                IsPaneOpen = false;

            if (CurrentUser is HummingbirdUser)
            {
                HummingbirdDashboardParameter parameter = new HummingbirdDashboardParameter();
                parameter.User = HummingbirdUser_LoggedIn;
                NavigationService.Navigate(typeof(HummingbirdDashboardPage), parameter);
            }
        }
        #endregion

        #region Navigate Settings Command
        public RelayCommand NavigateSettingsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavigateSettings();
                });
            }
        }

        public void NavigateSettings()
        {
            Debug.WriteLine("Navigate Settings");
            if (!CanNavigate)
                return;

            if (CurrentVisualState?.Name != AdaptiveTriggerConsts.DesktopMinimumWidthName)
                IsPaneOpen = false;

            NavigationService.Navigate(typeof(SettingsPage), null);
        }
        #endregion

        #region Navigate About Command
        public RelayCommand NavigateAboutCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavigateAbout();
                });
            }
        }

        public void NavigateAbout()
        {
            Debug.WriteLine("Navigate About");
            if (!CanNavigate)
                return;

            if (CurrentVisualState?.Name != AdaptiveTriggerConsts.DesktopMinimumWidthName)
                IsPaneOpen = false;

            NavigationService.Navigate(typeof(AboutPage), null);
        }
        #endregion

        #region Rate App Command
        public RelayCommand RateAppCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RateApp();
                });
            }
        }

        public void RateApp()
        {
            Debug.WriteLine("Rating App");
            if (!CanNavigate)
                return;

            ViewModelLocator.Instance.vm_AboutViewModel.ApplicationData.LaunchReview();
        }
        #endregion
        #endregion

        #region User Interaction Commands
        #region Switch User Command
        public RelayCommand<string> SwitchUserCommand
        {
            get
            {
                return new RelayCommand<string>((serviceNameString) =>
                {
                    SwitchUser(AnimeTrackingServiceWrapper.Converters.ServiceNameConverter.StringToServiceName(serviceNameString));
                });
            }
        }
        public void SwitchUser(ServiceName service, User user = null)
        {
            Debug.WriteLine("Switch To User On Service {0}", service);
            if (!CanNavigate)
            {
                Debug.WriteLine("Can't Navigate");
                return;
            }

            SetSelectedUser(service, user);

            if (CurrentNavigationLocation == NavigationLocation.Default ||
                CurrentNavigationLocation == NavigationLocation.Login)
            {
                NavigateDashboard();
            }
        }
        #endregion

        #region Login Command
        public RelayCommand<string> LoginCommand
        {
            get
            {
                return new RelayCommand<string>((serviceNameString) =>
                {
                    Login(AnimeTrackingServiceWrapper.Converters.ServiceNameConverter.StringToServiceName(serviceNameString));
                });
            }
        }
        public void Login(ServiceName service)
        {
            Debug.WriteLine("Login on Service {0}", service);
            if (!CanNavigate)
                return;

            if (service == ServiceName.Hummingbird)
                NavigationService.Navigate(typeof(HummingbirdLoginPage), null);

            if (CurrentVisualState?.Name != AdaptiveTriggerConsts.DesktopMinimumWidthName)
                IsPaneOpen = false;
        }

        #endregion

        #region Logout Command
        public RelayCommand<string> LogoutCommand
        {
            get
            {
                return new RelayCommand<string>((serviceNameString) =>
                {
                    Logout(AnimeTrackingServiceWrapper.Converters.ServiceNameConverter.StringToServiceName(serviceNameString));
                });
            }
        }

        public void Logout(ServiceName service)
        {
            Debug.WriteLine("Logout of Service {0}", service);
            if (!CanNavigate)
                return;

            if (service == ServiceName.Hummingbird)
            {
                if (CurrentUser is HummingbirdUser) CurrentUser = null;
                HummingbirdUser_LoggedIn = new HummingbirdUser();
                HummingbirdUser.DeleteUser();
            }


            NavigationService.Navigate(typeof(DefaultNoUserPage), null);
            CurrentNavigationLocation = NavigationLocation.Default;
        }
        #endregion

        private void SetSelectedUser(ServiceName service, User user)
        {
            if (CurrentUser != null)
                CurrentUser.Selected = false;

            if (user == null)
            {
                if (service == ServiceName.Hummingbird)
                    CurrentUser = HummingbirdUser_LoggedIn;
            }
            else CurrentUser = user;

            if (CurrentUser == null) return;
            CurrentUser.Selected = true;
        }
        #endregion
    }
}