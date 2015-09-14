/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Anitro"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Anitro.ViewModels.Hummingbird;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Anitro.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public static ViewModelLocator Instance;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
            //    // Create run time view services and models
            //    SimpleIoc.Default.Register<IDataService, DataService>();
            //}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<HummingbirdDashboardViewModel>();
            SimpleIoc.Default.Register<HummingbirdAnimeLibraryViewModel>();

            // Set the Instance
            Instance = this;
        }

        public DefaultNoUserViewModel vm_DefaultNoUserViewModel { get { return new DefaultNoUserViewModel(); } }

        public MainViewModel vm_MainViewModel { get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }
        public AboutViewModel vm_AboutViewModel { get { return ServiceLocator.Current.GetInstance<AboutViewModel>(); } }
        public SettingsViewModel vm_SettingsViewModel { get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); } }

        #region Hummingbird
        public HummingbirdLoginViewModel vm_HummingbirdLoginViewModel { get { return new HummingbirdLoginViewModel(); } }
        public HummingbirdAnimeDetailsViewModel vm_HummingbirdAnimeDetailsViewModel { get { return new HummingbirdAnimeDetailsViewModel(); } }
        public HummingbirdSearchViewModel vm_HummingbirdSearchViewModel { get { return new HummingbirdSearchViewModel(); } }
        public HummingbirdCalendarViewModel vm_HummingbirdCalendarViewModel { get { return new HummingbirdCalendarViewModel(); } }

        public HummingbirdDashboardViewModel vm_HummingbirdDashboardViewModel { get { return ServiceLocator.Current.GetInstance<HummingbirdDashboardViewModel>(); } }
        public HummingbirdAnimeLibraryViewModel vm_HummingbirdAnimeLibraryViewModel { get { return ServiceLocator.Current.GetInstance<HummingbirdAnimeLibraryViewModel>(); } }
        #endregion

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
