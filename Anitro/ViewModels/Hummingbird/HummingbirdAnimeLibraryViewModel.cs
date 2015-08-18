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
using System.Collections.Generic;
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
    public class HummingbirdAnimeLibraryViewModel : AnitroViewModelBase
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
        public HummingbirdAnimeLibraryViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                User = new HummingbirdUser();
                User.UserInfo.Username = "Design Time";
                User.UserInfo.AvatarUrl = new System.Uri("https://static.hummingbird.me/users/avatars/000/007/415/thumb/TyrilCropped1.png?1401236074", System.UriKind.Absolute);
                User.HummingbirdUserInfo.cover_image = "https://static.hummingbird.me/users/cover_images/000/007/415/thumb/Zamma_resiz.jpg?1401237213";

                AnimeObject anime = new AnimeObject();
                anime.RomanjiTitle = "Gate: Jieitai Kanochi nite, Kaku Tatakaeri";
                anime.EnglishTitle = "GATE";
                anime.CoverImageUrl = new Uri("https://static.hummingbird.me/anime/poster_images/000/010/085/large/85a5d8cc2972ae422158be7069076be41435868848_full.jpg?1435924413", UriKind.Absolute);

                // Recent
                User.AnimeLibrary.Recent.Add(anime);
                User.AnimeLibrary.Recent.Add(anime);
                User.AnimeLibrary.Recent.Add(anime);
                User.AnimeLibrary.Recent.Add(anime);
                User.AnimeLibrary.Recent.Add(anime);
                User.AnimeLibrary.Recent.Add(anime);

                // Library
                //User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Add(anime);
                //User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Add(anime);
                //User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Add(anime);
                //User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Add(anime);
                //User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Add(anime);
                //User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Add(anime);

                // Favourites
                User.AnimeLibrary.Favourites.Add(anime);
                User.AnimeLibrary.Favourites.Add(anime);
                User.AnimeLibrary.Favourites.Add(anime);
                User.AnimeLibrary.Favourites.Add(anime);
                User.AnimeLibrary.Favourites.Add(anime);
                User.AnimeLibrary.Favourites.Add(anime);
            }
            else
            {
                // Code runs "for real"
            }
        }

        public override void OnNavigatedTo()
        {
            MainViewModel.Instance.CurrentNavigationLocation = NavigationLocation.AnimeLibrary;

            if (User.AnimeLibrary.LibraryCollection.UnfilteredCollection.Count == 0)
                RefreshLibrary();
            if (User.AnimeLibrary.Favourites.Count == 0)
                RefreshFavourites();
        }

        public override void ResetViewModel()
        {

        }

        #region Commands
        #region Refresh Library
        public RelayCommand ClearRecentCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ClearRecent();
                });
            }
        }

        public void ClearRecent()
        {
            Debug.WriteLine("Clear Recent");
            User.AnimeLibrary.Recent.Clear();
        }
        #endregion

        #region Refresh Library
        public RelayCommand RefreshLibraryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RefreshLibrary();
                });
            }
        }

        public void RefreshLibrary()
        {
            Debug.WriteLine("Refreshing Library");
            Progress<APIProgressReport> m_refreshLibraryProgress = new Progress<APIProgressReport>();
            m_refreshLibraryProgress.ProgressChanged += M_refreshLibraryProgress_ProgressChanged;
            APIServiceCollection.Instance.HummingbirdV1API.AnimeAPI.GetAnimeLibrary(User.UserInfo.Username, LibrarySection.All, m_refreshLibraryProgress);
        }

        private void M_refreshLibraryProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();
                List<LibraryObject> libraryObjects = e.Parameter.Converted as List<LibraryObject>;
                User.AnimeLibrary.LibraryCollection.UnfilteredCollection = new ObservableCollection<LibraryObject>(libraryObjects);
            }
        }
        #endregion

        #region Refresh Favourites
        public RelayCommand RefreshFavouritesCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RefreshFavourites();
                });
            }
        }

        public void RefreshFavourites()
        {
            Debug.WriteLine("Refreshing Favourites");
            Progress<APIProgressReport> m_refreshFavouritesProgress = new Progress<APIProgressReport>();
            m_refreshFavouritesProgress.ProgressChanged += M_refreshFavouritesProgress_ProgressChanged;
            APIServiceCollection.Instance.HummingbirdV1API.AnimeAPI.GetAnimeFavourites(User.UserInfo.Username, m_refreshFavouritesProgress);
        }

        private void M_refreshFavouritesProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();
                List<AnimeObject> animeObjects = e.Parameter.Converted as List<AnimeObject>;
                User.AnimeLibrary.Favourites = new ObservableCollection<AnimeObject>(animeObjects);
            }
        }

        #endregion
        #endregion
    }
}