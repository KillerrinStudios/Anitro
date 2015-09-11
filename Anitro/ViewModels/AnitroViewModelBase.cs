using AnimeTrackingServiceWrapper;
using Anitro.Helpers;
using Anitro.Models;
using Anitro.Models.Page_Parameters;
using Anitro.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.ViewModels
{
    public abstract class AnitroViewModelBase : ViewModelBase
    {
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

        private APIProgressReport m_APIFeedback = new APIProgressReport();
        public APIProgressReport APIFeedback
        {
            get { return m_APIFeedback; }
            set
            {
                m_APIFeedback = value;
                RaisePropertyChanged(nameof(APIFeedback));
            }
        }

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

        public APIServiceCollection APIServiceCollections { get { return APIServiceCollection.Instance; } }

        public abstract void Loaded();
        public abstract void OnNavigatedTo();
        public abstract void OnNavigatedFrom();
        public abstract void ResetViewModel();
    }
}
