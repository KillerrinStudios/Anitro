using AnimeTrackingServiceWrapper;
using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1.Models;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using AnimeTrackingServiceWrapper.UniversalServiceModels.ActivityFeed;
using Anitro.Models;
using Anitro.Models.Enums;
using Anitro.Models.Page_Parameters;
using Anitro.Pages.Hummingbird;
using Anitro.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
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
        private HummingbirdUser m_user = new HummingbirdUser();
        public HummingbirdUser User
        {
            get { return m_user; }
            set
            {
                if (m_user == value) return;
                m_user = value;
                RaisePropertyChanged(nameof(User));
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HummingbirdDashboardViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                User = new HummingbirdUser();
                User.UserInfo.Username = "Design Time";
                User.UserInfo.AvatarUrl = new System.Uri("https://static.hummingbird.me/users/avatars/000/007/415/thumb/TyrilCropped1.png?1401236074", System.UriKind.Absolute);
                User.HummingbirdUserInfo.cover_image = "https://static.hummingbird.me/users/cover_images/000/007/415/thumb/Zamma_resiz.jpg?1401237213";

                User.UserInfo.Location = "Toronto, Ontario";
                User.HummingbirdUserInfo.bio = "Developer of Anitro for Windows Phone and Windows!";

                User.HummingbirdUserInfo.waifu = "Iona";
                User.HummingbirdUserInfo.waifu_or_husbando = "Waifu";
                User.HummingbirdUserInfo.waifu_slug = "aoki-hagane-no-arpeggio";
                User.HummingbirdUserInfo.waifu_char_id = "39088";

                // Activity Feed
                ActivityFeedComment activityFeedComment = new ActivityFeedComment(User.UserInfo, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum ut purus sem. Aliquam eget nulla diam. Morbi ipsum erat, convallis porta suscipit quis.", DateTime.Now);
                activityFeedComment.Replies.Add(new ActivityFeedComment(User.UserInfo, "Reply Comment 1", DateTime.Now));
                User.ActivityFeed.Add(activityFeedComment);

                User.ActivityFeed.Add(new ActivityFeedCommentTo(User.UserInfo, User.UserInfo, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum ut purus sem.", DateTime.Now));
                User.ActivityFeed.Add(new ActivityFeedFollowedMessage(User.UserInfo, User.UserInfo, DateTime.Now));
                User.ActivityFeed.Add(new ActivityFeedComment(User.UserInfo, "Comment 2", DateTime.Now));
            }
            else
            {
                // Code runs "for real"
            }
        }

        public override void OnNavigatedTo()
        {
            MainViewModel.Instance.CurrentNavigationLocation = NavigationLocation.Dashboard;
            RefreshActivityFeed();

            HummingbirdAnimeLibraryViewModel libraryVM = ViewModelLocator.Instance.vm_HummingbirdAnimeLibraryViewModel;
            libraryVM.User = User;

            RefreshUserDetails();

            if (User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Count == 0)
                libraryVM.RefreshLibrary();
            if (User.AnimeLibrary.Favourites.Count == 0)
                libraryVM.RefreshFavourites();
        }

        public override void OnNavigatedFrom()
        {

        }

        public override void ResetViewModel()
        {

        }

        #region Commands
        #region Activity Feed
        #region Refresh Activity Feed
        public RelayCommand RefreshActivityFeedCommand { get { return new RelayCommand(() => { RefreshActivityFeed(); }); } }
        public void RefreshActivityFeed(int index = 0)
        {
            Debug.WriteLine("Refreshing Activity Feed");
            Progress<APIProgressReport> m_refreshActivityFeedProgress = new Progress<APIProgressReport>();
            m_refreshActivityFeedProgress.ProgressChanged += M_refreshActivityFeedProgress_ProgressChanged;
            APIServiceCollection.Instance.HummingbirdV1API.SocialAPI.GetActivityFeed(User.LoginInfo.Username, index, m_refreshActivityFeedProgress);
        }

        private void M_refreshActivityFeedProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();
                User.ActivityFeed = e.Parameter.Converted as ObservableCollection<AActivityFeedItem>;
            }
            else if (APIResponseHelpers.IsAPIResponseFailed(e.CurrentAPIResonse))
            {
                ProgressService.Reset();
            }
        }
        #endregion

        #region Post To Activity Feed
        public RelayCommand<string> PostToActivityFeedCommand { get { return new RelayCommand<string>((message) => { PostToActivityFeed(message); }); } }
        public void PostToActivityFeed(string message)
        {
            Debug.WriteLine("Posting to Activity Feed");
            if (string.IsNullOrWhiteSpace(message)) return;

            HummingbirdUser loggedInUser = MainViewModel.Instance.HummingbirdUser;
            if (loggedInUser == null) return;

            Progress<APIProgressReport> m_postToActivityFeedprogress = new Progress<APIProgressReport>();
            m_postToActivityFeedprogress.ProgressChanged += M_postToActivityFeedprogress_ProgressChanged; ;
            APIServiceCollection.Instance.HummingbirdV1API.SocialAPI.PostStatusUpdate(loggedInUser.LoginInfo,
                                                                                      loggedInUser.UserInfo,
                                                                                      User.UserInfo,
                                                                                      message,
                                                                                      m_postToActivityFeedprogress);
        }

        private void M_postToActivityFeedprogress_ProgressChanged(object sender, APIProgressReport e)
        {
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();
                User.ActivityFeed.Insert(0, (AActivityFeedItem)e.Parameter.Converted);
            }
            else if (APIResponseHelpers.IsAPIResponseFailed(e.CurrentAPIResonse))
            {
                ProgressService.Reset();
            }
        }
        #endregion

        #region Navigate Anime Details Page
        public RelayCommand<ServiceID> GetAnimeAndNavigateCommand { get { return new RelayCommand<ServiceID>((serviceID) => { GetAnimeAndNavigate(serviceID); }); } }
        public void GetAnimeAndNavigate(ServiceID serviceID)
        {
            Debug.WriteLine("Getting Anime");
            if (string.IsNullOrWhiteSpace(serviceID.ID))
                return;
            if (User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Count == 0)
                return;

            Progress<APIProgressReport> m_getAnimeDetailsProgress = new Progress<APIProgressReport>();
            m_getAnimeDetailsProgress.ProgressChanged += M_getAnimeDetailsProgress_ProgressChanged;
            APIServiceCollection.Instance.HummingbirdV1API.AnimeAPI.GetAnime(serviceID.ID, m_getAnimeDetailsProgress);
        }

        private void M_getAnimeDetailsProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();
                ViewModelLocator.Instance.vm_HummingbirdAnimeLibraryViewModel.NavigateAnimeDetailsPage((AnimeObject)e.Parameter.Converted);
            }
            else if (APIResponseHelpers.IsAPIResponseFailed(e.CurrentAPIResonse))
            {
                ProgressService.Reset();
            }
        }
        #endregion
        #endregion

        #region Refresh User Details Command
        public RelayCommand RefreshUserDetailsCommand { get { return new RelayCommand(() => { RefreshUserDetails(); }); } }
        public void RefreshUserDetails()
        {
            Progress<APIProgressReport> userDetailsProgress = new Progress<APIProgressReport>();
            userDetailsProgress.ProgressChanged += RefreshUserDetailsProgress_ProgressChanged;
            APIServiceCollection.Instance.HummingbirdV1API.GetUserInfo(User.LoginInfo.Username, userDetailsProgress);
        }
        private void RefreshUserDetailsProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            Debug.WriteLine("GetUserDetailsProgress_ProgressChanged: {0}", e);
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);

            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();
                User.UserInfo = e.Parameter.Converted as UserInfo;
                User.HummingbirdUserInfo = e.Parameter.Raw as UserObjectHummingbirdV1;

                if (User.LoginInfo.IsUserLoggedIn)
                    HummingbirdUser.Save(User);
            }
            else if (APIResponseHelpers.IsAPIResponseFailed(e.CurrentAPIResonse))
            {
                ProgressService.Reset();
            }
        }
        #endregion
        #endregion

        #region Navigation Commands
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
            Debug.WriteLine("Navigating Hummingbird Anime Library");
            if (!CanNavigate)
                return;

            NavigationService.Navigate(typeof(HummingbirdAnimeLibraryPage), User);
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
            Debug.WriteLine("Navigating Hummingbird Calendar");
            if (!CanNavigate)
                return;

            HummingbirdCalendarParameter parameter = new HummingbirdCalendarParameter();
            parameter.User = User;

            NavigationService.Navigate(typeof(HummingbirdCalendarPage), parameter);
        }
        #endregion

        #region Navigate Search Command
        public RelayCommand NavigateSearchCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavigateSearch();
                });
            }
        }

        public void NavigateSearch()
        {
            Debug.WriteLine("Navigating Hummingbird Search");
            if (!CanNavigate)
                return;

            SearchParameter parameter = new SearchParameter();
            parameter.SearchTerms = "";
            parameter.Filter = SearchFilter.Everything;
            parameter.User = User;
            NavigationService.Navigate(typeof(HummingbirdSearchPage), parameter);
        }
        #endregion
        #endregion
    }
}