using AnimeTrackingServiceWrapper;
using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1.Models;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.Models;
using Anitro.Models.Enums;
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
    public class HummingbirdDashboardViewModel : AnitroViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HummingbirdDashboardViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }
        }

        public override void ResetViewModel()
        {

        }
    }
}