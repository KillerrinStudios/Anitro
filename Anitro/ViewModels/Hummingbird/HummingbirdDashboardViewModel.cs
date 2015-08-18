using AnimeTrackingServiceWrapper;
using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1.Models;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using AnimeTrackingServiceWrapper.UniversalServiceModels.ActivityFeed;
using Anitro.Models;
using Anitro.Models.Enums;
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
        }

        public override void ResetViewModel()
        {

        }

        #region Commands

        #region Refresh Activity Feed
        public RelayCommand RefreshActivityFeedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RefreshActivityFeed();
                });
            }
        }

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
        }

        #endregion
        #endregion
    }
}