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
    public class DefaultNoUserViewModel : AnitroViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public DefaultNoUserViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
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
    }
}