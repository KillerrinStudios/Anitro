using AnimeTrackingServiceWrapper;
using Anitro.Helpers;
using Anitro.Models;
using Anitro.Models.Page_Parameters;
using Anitro.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Anitro.ViewModels
{
    public abstract class AnitroViewModelBase : ViewModelBase
    {
        #region Services
        public bool CanNavigate { get { return SimpleIoc.Default.IsRegistered<NavigationService>(); } }
        public NavigationService NavigationService
        {
            get
            {
                return SimpleIoc.Default.GetInstance<NavigationService>(); ;
            }
        }

        public bool HasProgressService { get { return SimpleIoc.Default.IsRegistered<ProgressService>(); } }
        public ProgressService ProgressService
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ProgressService>(); ;
            }
        }

        public bool HasMediaService { get { return SimpleIoc.Default.IsRegistered<MediaService>(); } }
        public MediaService MediaService
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MediaService>(); ;
            }
        }

        public static string m_topNavBarText = "";
        public string TopNavBarText
        {
            get { return m_topNavBarText; }
            set
            {
                m_topNavBarText = value;
                RaisePropertyChanged(nameof(TopNavBarText));
            }
        }

        #endregion

        #region All View Models
        private static AnitroLaunchArgs m_anitroLaunchArgs = null;
        public AnitroLaunchArgs LaunchArgs
        {
            get { return m_anitroLaunchArgs; }
            set
            {
                if (m_anitroLaunchArgs == value) return;
                m_anitroLaunchArgs = value;
                RaisePropertyChanged(nameof(LaunchArgs));
            }
        }

        private static LicencesOwned m_anitroLicense = new LicencesOwned();
        public LicencesOwned AnitroLicense
        {
            get { return m_anitroLicense; }
            set
            {
                m_anitroLicense = value;

                Debug.WriteLine("Updating Anitro License");
                RaisePropertyChanged(nameof(AnitroLicense));
            }
        }

        public APIServiceCollection APIServiceCollections { get { return APIServiceCollection.Instance; } }
        #endregion

        #region Users
        private static User m_currentUser = null;
        public User CurrentUser
        {
            get { return m_currentUser; }
            set
            {
                m_currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private static HummingbirdUser m_hummingbirdUser_loggedIn = new HummingbirdUser();
        public HummingbirdUser HummingbirdUser_LoggedIn
        {
            get { return m_hummingbirdUser_loggedIn; }
            set
            {
                //if (m_hummingbirdUser_loggedIn == value) return;
                m_hummingbirdUser_loggedIn = value;
                RaisePropertyChanged(nameof(HummingbirdUser_LoggedIn));
            }
        }
        #endregion

        public abstract void Loaded();
        public abstract void OnNavigatedTo();
        public abstract void OnNavigatedFrom();
        public abstract void ResetViewModel();

        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) return;

            try
            {
                base.RaisePropertyChanged(propertyName);
            }
            catch (Exception) { }
        }
    }
}
