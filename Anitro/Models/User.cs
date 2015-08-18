using AnimeTrackingServiceWrapper;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public abstract class User : ModelBase
    {
        private bool m_selected = false;
        public bool Selected
        {
            get { return m_selected; }
            set
            {
                if (m_selected == value) return;
                m_selected = value;
                RaisePropertyChanged(nameof(Selected));
            }
        }

        private ServiceName m_service = ServiceName.Unknown;
        public ServiceName Service
        {
            get { return m_service; }
            set
            {
                if (m_service == value) return;
                m_service = value;
                RaisePropertyChanged(nameof(Service));
            }
        }

        private UserLoginInfo m_loginInfo = new UserLoginInfo();
        public UserLoginInfo LoginInfo
        {
            get { return m_loginInfo; }
            set
            {
                if (m_loginInfo == value) return;
                m_loginInfo = value;
                RaisePropertyChanged(nameof(LoginInfo));
            }
        }

        private UserInfo m_userInfo = new UserInfo();
        public UserInfo UserInfo
        {
            get { return m_userInfo; }
            set
            {
                if (m_userInfo == value) return;
                m_userInfo = value;
                RaisePropertyChanged(nameof(UserInfo));
            }
        }

        private Library m_animelibrary = new Library();
        public Library AnimeLibrary
        {
            get { return m_animelibrary; }
            set
            {
                if (m_animelibrary == value) return;
                m_animelibrary = value;
                RaisePropertyChanged(nameof(AnimeLibrary));
            }
        }

        private Library m_mangaLibrary = new Library();
        public Library MangaLibrary
        {
            get { return m_mangaLibrary; }
            set
            {
                if (m_mangaLibrary == value) return;
                m_mangaLibrary = value;
                RaisePropertyChanged(nameof(MangaLibrary));
            }
        }
    }
}
