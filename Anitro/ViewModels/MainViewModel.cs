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

        private User m_user;
        public User CurrentUser
        {
            get { return m_user; }
            set
            {
                m_user = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private HummingbirdUser m_hummingbirdUser = new HummingbirdUser();
        public HummingbirdUser HummingbirdUser
        {
            get { return m_hummingbirdUser; }
            set
            {
                if (m_hummingbirdUser == value) return;
                m_hummingbirdUser = value;
                RaisePropertyChanged(nameof(HummingbirdUser));
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
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                HummingbirdUser = new HummingbirdUser();
                HummingbirdUser.UserInfo.Username = "Design View";
                HummingbirdUser.UserInfo.AvatarUrl = new System.Uri("https://static.hummingbird.me/users/avatars/000/007/415/thumb/TyrilCropped1.png?1401236074", System.UriKind.Absolute);
                HummingbirdUser.Selected = true;
            }
            else
            {
                // Code runs "for real"
            }

            ResetViewModel();
        }

        public override void OnNavigatedTo()
        {

        }

        public override void ResetViewModel()
        {
            HummingbirdUser = new HummingbirdUser();
            CurrentNavigationLocation = NavigationLocation.Default;
        }


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

            if (CurrentUser is HummingbirdUser)
            {
                NavigationService.Navigate(typeof(HummingbirdDashboardPage), CurrentUser);
            }
        }
        #endregion

        #region Navigate Anime Library Command
        public RelayCommand NavigateAnimeLibraryCommand
        {
            get
            {
                return new RelayCommand(() =>
                  {
                      NavigateAnimeLibrary();
                  });
            }
        }

        public void NavigateAnimeLibrary()
        {
            Debug.WriteLine("Navigate Anime Library");
            if (!CanNavigate)
                return;

            if (CurrentUser is HummingbirdUser)
            {
                NavigationService.Navigate(typeof(HummingbirdAnimeLibraryPage), CurrentUser);
            }
        }
        #endregion

        #region Navigate Manga Library Command
        public RelayCommand NavigateMangaLibraryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavigateMangaLibrary();
                });
            }
        }

        public void NavigateMangaLibrary()
        {
            Debug.WriteLine("Navigate Manga Library");
            if (!CanNavigate)
                return;

            if (CurrentUser is HummingbirdUser)
            {

            }
        }
        #endregion

        #region Navigate Calendar Command
        public RelayCommand NavigateCalendarCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavigateCalendar();
                });
            }
        }

        public void NavigateCalendar()
        {
            Debug.WriteLine("Navigate Calendar");
            if (!CanNavigate)
                return;
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
                return;

            SetSelectedUser(service, user);

            if (CurrentNavigationLocation == NavigationLocation.Login)
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

            if (service == ServiceName.Hummingbird)
            {
                HummingbirdUser = new HummingbirdUser();
            }

            if (!CanNavigate)
                return;

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
                    CurrentUser = HummingbirdUser;
            }
            else CurrentUser = user;

            CurrentUser.Selected = true;
        }
        #endregion
    }
}