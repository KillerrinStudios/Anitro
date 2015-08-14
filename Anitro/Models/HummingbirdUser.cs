using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1.Models;
using AnimeTrackingServiceWrapper.UniversalServiceModels.ActivityFeed;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class HummingbirdUser : User
    {
        private ObservableCollection<AActivityFeedItem> m_activityFeed = new ObservableCollection<AActivityFeedItem>();
        public ObservableCollection<AActivityFeedItem> ActivityFeed
        {
            get { return m_activityFeed; }
            set
            {
                if (m_activityFeed == value) return;
                m_activityFeed = value;
                RaisePropertyChanged(nameof(ActivityFeed));
            }
        }

        private UserObjectHummingbirdV1 m_hummingbirdUserInfo = new UserObjectHummingbirdV1();
        public UserObjectHummingbirdV1 HummingbirdUserInfo
        {
            get { return m_hummingbirdUserInfo; }
            set
            {
                if (m_hummingbirdUserInfo == value) return;
                m_hummingbirdUserInfo = value;
                RaisePropertyChanged(nameof(HummingbirdUserInfo));
            }
        }

        public HummingbirdUser()
        {
            Service = AnimeTrackingServiceWrapper.ServiceName.Hummingbird;
            UserInfo.AvatarUrl = new Uri("http://www.example.com/", UriKind.Absolute);
            UserInfo.Username = "";
        }
    }
}
