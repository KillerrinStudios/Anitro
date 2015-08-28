using AnimeTrackingServiceWrapper;
using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1.Models;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.Models;
using Anitro.Models.Enums;
using Anitro.Pages.Hummingbird;
using Anitro.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Diagnostics;

namespace Anitro.ViewModels.Hummingbird
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
    public class HummingbirdLoginViewModel : AnitroViewModelBase
    {
        private UserLoginInfo m_userLoginInfo = new UserLoginInfo();
        public UserLoginInfo UserLoginInfo
        {
            get { return m_userLoginInfo; }
            set
            {
                if (m_userLoginInfo == value) return;
                m_userLoginInfo = value;
                RaisePropertyChanged(nameof(UserLoginInfo));
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HummingbirdLoginViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                UserLoginInfo.Username = "killerrin";
                UserLoginInfo.Password = "password";
            }
            else
            {
                // Code runs "for real"
            }
        }

        public override void OnNavigatedTo()
        {
            MainViewModel.Instance.CurrentNavigationLocation = NavigationLocation.Login;
        }

        public override void OnNavigatedFrom()
        {

        }

        public override void ResetViewModel()
        {
            UserLoginInfo = new UserLoginInfo();
        }


        public RelayCommand LoginCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Login();
                });
            }
        }

        
        bool m_currentlyLoggingIn = false;
        public void Login()
        {
            Debug.WriteLine("Attempting to Login");
            if (m_currentlyLoggingIn)
            {
                Debug.WriteLine("Already Attempting");
                return;
            }

            Progress<APIProgressReport> m_loginProgress = new Progress<APIProgressReport>();
            m_loginProgress.ProgressChanged += LoginProgress_ProgressChanged;

            APIServiceCollection.Instance.HummingbirdV1API.Login(UserLoginInfo.Username, UserLoginInfo.Password, m_loginProgress);
            m_currentlyLoggingIn = true;
        }

        Progress<APIProgressReport> m_userDetailsProgress;
        private void LoginProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            Debug.WriteLine("LoginProgress_ProgressChanged: {0}", e);
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            APIFeedback = e;

            if (e.CurrentAPIResonse != APIResponse.ContinuingExecution)
            {
                ProgressService.DisableRing();
                ProgressService.PercentageVisibility = Windows.UI.Xaml.Visibility.Collapsed;

                if (e.CurrentAPIResonse == APIResponse.Successful)
                {
                    UserLoginInfo = e.Parameter.Converted as UserLoginInfo;

                    m_userDetailsProgress = new Progress<APIProgressReport>();
                    m_userDetailsProgress.ProgressChanged += GetUserDetailsProgress_ProgressChanged;
                    APIServiceCollection.Instance.HummingbirdV1API.GetUserInfo(UserLoginInfo.Username, m_userDetailsProgress);
                }
                else
                {
                    m_currentlyLoggingIn = false;
                }
            }
        }

        UserInfo convertedUserInfo;
        UserObjectHummingbirdV1 rawUserInfo;
        private void GetUserDetailsProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            Debug.WriteLine("GetUserDetailsProgress_ProgressChanged: {0}", e);
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            APIFeedback = e;

            if (e.CurrentAPIResonse != APIResponse.ContinuingExecution)
            {
                if (e.CurrentAPIResonse == APIResponse.Successful)
                {
                    ProgressService.Hide();
                    ProgressService.Reset();

                    convertedUserInfo = e.Parameter.Converted as UserInfo;
                    rawUserInfo = e.Parameter.Raw as UserObjectHummingbirdV1;

                    HummingbirdUser hummingbirdUser = MainViewModel.Instance.HummingbirdUser;
                    hummingbirdUser.LoginInfo = UserLoginInfo;
                    hummingbirdUser.UserInfo = convertedUserInfo;
                    hummingbirdUser.HummingbirdUserInfo = rawUserInfo;

                    MainViewModel.Instance.SwitchUser(ServiceName.Hummingbird);
                    NavigationService.RemoveLastPage();
                }
                else
                {
                    m_currentlyLoggingIn = false;
                }
            }
        }
    }
}