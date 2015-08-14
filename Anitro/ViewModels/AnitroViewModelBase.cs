using AnimeTrackingServiceWrapper;
using Anitro.Helpers;
using Anitro.Models;
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

        public abstract void ResetViewModel();
    }
}
