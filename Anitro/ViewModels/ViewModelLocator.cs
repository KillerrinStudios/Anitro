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
            SimpleIoc.Default.Register<HummingbirdDashboardViewModel>();
            SimpleIoc.Default.Register<HummingbirdAnimeLibraryViewModel>();

            // Set the Instance
            Instance = this;
        }

        public MainViewModel vm_MainViewModel { get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }

        #region Hummingbird
        public HummingbirdLoginViewModel vm_HummingbirdLoginViewModel { get { return new HummingbirdLoginViewModel(); } }
        public HummingbirdAnimeDetailsViewModel vm_HummingbirdAnimeDetailsViewModel { get { return new HummingbirdAnimeDetailsViewModel(); } }

        public HummingbirdDashboardViewModel vm_HummingbirdDashboardViewModel { get { return ServiceLocator.Current.GetInstance<HummingbirdDashboardViewModel>(); } }
        public HummingbirdAnimeLibraryViewModel vm_HummingbirdAnimeLibraryViewModel { get { return ServiceLocator.Current.GetInstance<HummingbirdAnimeLibraryViewModel>(); } }
        #endregion

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
