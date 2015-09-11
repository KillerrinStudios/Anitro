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
using Windows.ApplicationModel;
using Killerrin_Studios_Toolkit;
using Windows.ApplicationModel.Email;

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
    public class AboutViewModel : AnitroViewModelBase
    {
        public static AboutViewModel Instance { get { return ServiceLocator.Current.GetInstance<AboutViewModel>(); } }

        KillerrinApplicationData m_applicationData = new KillerrinApplicationData("http://www.hummingbird.me");
        public KillerrinApplicationData ApplicationData
        {
            get { return m_applicationData; }
            protected set
            {
                m_applicationData = value;
                RaisePropertyChanged(nameof(ApplicationData));
            }
        }

        public string EmailFeedBackContent { get { return "Feedback - " + ApplicationData.FeedbackUrl; } }
        public string EmailSupportContent { get { return "Support - " + ApplicationData.FeedbackUrl; } }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public AboutViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                ApplicationData.Name = "Anitro";
                ApplicationData.PublisherName = "Killerrin Studios";
                ApplicationData.Description = "Description Goes Here";
            }
            else
            {
                // Code runs "for real"
            }

            ResetViewModel();
        }

        public override void Loaded()
        {

        }

        public override void OnNavigatedTo()
        {
        }

        public override void OnNavigatedFrom()
        {

        }

        public override void ResetViewModel()
        {
        }

        #region Email Feedback
        public RelayCommand EmailFeedbackCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EmailFeedback();
                });
            }
        }

        public async Task EmailFeedback()
        {
            Debug.WriteLine("Emailing Feedback");
            await KillerrinApplicationData.SendEmail(ApplicationData.FeedbackUrl, ApplicationData.FeedbackSubject, "");
        }
        #endregion

        #region Email Support
        public RelayCommand EmailSupportCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EmailSupport();
                });
            }
        }

        public async Task EmailSupport()
        {
            Debug.WriteLine("Emailing Support");
            await KillerrinApplicationData.SendEmail(ApplicationData.SupportUrl, ApplicationData.SupportSubject, "");
        }
        #endregion
    }
}