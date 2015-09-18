using AnimeTrackingServiceWrapper;
using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1.Models;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using AnimeTrackingServiceWrapper.UniversalServiceModels.ActivityFeed;
using Anitro.Helpers;
using Anitro.Models;
using Anitro.Models.Enums;
using Anitro.Models.Page_Parameters;
using Anitro.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Killerrin_Studios_Toolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.StartScreen;

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
    public class HummingbirdAnimeDetailsViewModel : AnitroViewModelBase
    {
        private LibraryObject m_libraryObject = new LibraryObject();
        public LibraryObject LibraryObject
        {
            get { return m_libraryObject; }
            set
            {
                if (m_libraryObject == value) return;

                m_libraryObject.PropertyChanged -= M_libraryObject_PropertyChanged;
                m_libraryObject = value;
                m_libraryObject.PropertyChanged += M_libraryObject_PropertyChanged;

                try
                {
                    RaisePropertyChanged(nameof(LibraryObject));
                    RaisePropertyChanged(nameof(IsFavourited));
                }
                catch (Exception) { Debug.WriteLine("Exception Raising Property Changed: LibraryObject or IsFavourited"); }

            }
        }

        public bool IsFavourited
        {
            get
            {
                try
                {
                    if (!(HummingbirdUser_LoggedIn.LoginInfo.IsUserLoggedIn)) return false;
                    if (HummingbirdUser_LoggedIn.AnimeLibrary.IsFavourited(LibraryObject.Anime))
                        return true;
                }
                catch (Exception) { }

                return false;
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HummingbirdAnimeDetailsViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                HummingbirdUser_LoggedIn = new HummingbirdUser();
                HummingbirdUser_LoggedIn.UserInfo.Username = "Design Time";
                HummingbirdUser_LoggedIn.UserInfo.AvatarUrl = new System.Uri("https://static.hummingbird.me/users/avatars/000/007/415/thumb/TyrilCropped1.png?1401236074", System.UriKind.Absolute);
                HummingbirdUser_LoggedIn.HummingbirdUserInfo.cover_image = "https://static.hummingbird.me/users/cover_images/000/007/415/thumb/Zamma_resiz.jpg?1401237213";

                HummingbirdUser_LoggedIn.LoginInfo.Username = "Design Time";
                HummingbirdUser_LoggedIn.LoginInfo.AuthToken = "AuthToken";

                AnimeObject anime = new AnimeObject();
                anime.RomanjiTitle = "Gate: Jieitai Kanochi nite, Kaku Tatakaeri";
                anime.EnglishTitle = "GATE";
                anime.CoverImageUrl = new Uri("https://static.hummingbird.me/anime/poster_images/000/010/085/large/85a5d8cc2972ae422158be7069076be41435868848_full.jpg?1435924413", UriKind.Absolute);
                anime.WebUrl = new Uri("https://hummingbird.me/anime/gate-jieitai-kanochi-nite-kaku-tatakaeri", UriKind.Absolute);
                anime.EpisodeCount = 24;
                anime.Service = ServiceName.Hummingbird;
                anime.AgeRating = AgeRating.R17;
                anime.MediaType = MediaType.TV;
                anime.AiringStatus = AiringStatus.CurrentlyAiring;
                anime.Genres.Add(MediaGenre.Action);
                anime.Genres.Add(MediaGenre.Adventure);
                anime.Genres.Add(MediaGenre.Fantasy);
                anime.Genres.Add(MediaGenre.Military);
                anime.Synopsis = "In August of 20XX, a portal to a parallel world, known as the \"Gate,\" suddenly appeared in Ginza, Tokyo. Monsters and troops poured out of the portal, turning the shopping district into a bloody inferno. The Japan Ground - Self Defence Force immediately took action and pushed the fantasy creatures back to the \"Gate.\" To facilitate negotiations and prepare for future fights, the JGSDF dispatched the Third Reconnaissance Team to the \"Special Region\" at the other side of the Gate. Youji Itami, a JSDF officer as well as a 33 - year - old otaku, was appointed as the leader of the Team.Amid attacks from enemy troops the team visited a variety of places and learnt a lot about the local culture and geography. Thanks to their efforts in humanitarian relief, although with some difficulties they were gradually able to reach out to the locals.They even had a cute elf, a sorceress and a demigoddess in their circle of new friends.On the other hand, the major powers outside the Gate such as the United States, China, and Russia were extremely interested in the abundant resources available in the Special Region.They began to exert diplomatic pressure over Japan. A suddenly appearing portal to an unknown world—to the major powers it may be no more than a mere asset for toppling the international order.But to our protagonists it is an invaluable opportunity to broaden knowledge, friendship, and ultimately their perspective towards the world. (Source: Baka - Tsuki)";

                LibraryObject libraryObject = new LibraryObject();
                libraryObject.Anime = anime;
                libraryObject.EpisodesWatched = 6;
                libraryObject.Rating = 4.0;
                libraryObject.LastWatched = DateTime.Now;
                libraryObject.RewatchedTimes = 1;
                libraryObject.Private = PrivacySettings.Public;
                libraryObject.Section = LibrarySection.CurrentlyWatching;
                libraryObject.Notes = "AMAZING!";

                LibraryObject = libraryObject;
            }
            else
            {
                // Code runs "for real"
            }
        }

        public override void Loaded()
        {
            Debug.WriteLine("HummingbirdAnimeDetailsViewModel: Loaded");
        }

        public override void OnNavigatedTo()
        {
            Debug.WriteLine("HummingbirdAnimeDetailsViewModel: OnNavigatedTo");
            try
            {
                MainViewModel.Instance.CurrentNavigationLocation = NavigationLocation.AnimeDetails;
            }
            catch (Exception) { }
        }

        public override void OnNavigatedFrom()
        {
            Debug.WriteLine("HummingbirdAnimeDetailsViewModel: OnNavigatedFrom");

            //UpdateAnime();
            if (HummingbirdUser_LoggedIn.LoginInfo.IsUserLoggedIn)
                HummingbirdUser.Save(HummingbirdUser_LoggedIn);
        }

        public override void ResetViewModel()
        {
            Debug.WriteLine("HummingbirdAnimeDetailsViewModel: ResetViewModel");
        }

        #region Update/Remove Anime
        private void M_libraryObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LibraryObject.Section))
            {
                if (LibraryObject.Section == LibrarySection.None)
                {
                    RemoveAnime();
                    return;
                }
            }
            UpdateAnime();
        }

        #region Update Anime In Library
        public RelayCommand UpdateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UpdateAnime();
                });
            }
        }

        private bool ___autoSetCompletion = false;
        public void UpdateAnime()
        {
            Debug.WriteLine("Updating Anime");
            #region Early Exit
            if (!HummingbirdUser_LoggedIn.LoginInfo.IsUserLoggedIn) return;
            if (LibraryObject == null) return;
            if (___autoSetCompletion)
            {
                ___autoSetCompletion = false;
                return;
            }
            #endregion

            #region Library Section Changes
            if (LibraryObject.AreAllEpisodesWatched)
            {
                LibraryObject.Section = LibrarySection.Completed;
                ___autoSetCompletion = true;
            }
            if (LibraryObject.Section == LibrarySection.None) { RemoveAnime(); return; }
            #endregion

            Progress<APIProgressReport> updateAnimeProgress = new Progress<APIProgressReport>();
            updateAnimeProgress.ProgressChanged += UpdateAnimeProgress_ProgressChanged;
            APIServiceCollection.Instance.HummingbirdV1API.AnimeAPI.UpdateAnimeInLibrary(HummingbirdUser_LoggedIn.LoginInfo, LibraryObject, updateAnimeProgress);
        }

        private void UpdateAnimeProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();

                if (!HummingbirdUser_LoggedIn.AnimeLibrary.IsInLibrary(LibraryObject.Anime.ID))
                {
                    HummingbirdUser_LoggedIn.AnimeLibrary.AddToLibrary(LibraryObject);
                }
            }
            else if (APIResponseHelpers.IsAPIResponseFailed(e.CurrentAPIResonse))
            {
                ProgressService.Reset();
            }
        }
        #endregion

        #region Remove Anime From Library
        public void RemoveAnime()
        {
            Debug.WriteLine("Removing Anime");
            if (!HummingbirdUser_LoggedIn.LoginInfo.IsUserLoggedIn) return;
            if (LibraryObject == null) return;
            if (LibraryObject.Section != LibrarySection.None) return;

            Progress<APIProgressReport> removeAnimeProgress = new Progress<APIProgressReport>();
            removeAnimeProgress.ProgressChanged += RemoveAnimeProgress_ProgressChanged;
            APIServiceCollection.Instance.HummingbirdV1API.AnimeAPI.RemoveAnimeFromLibrary(HummingbirdUser_LoggedIn.LoginInfo, LibraryObject.Anime.ID.ID, removeAnimeProgress);
        }

        private void RemoveAnimeProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage);
            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();
                HummingbirdUser_LoggedIn.AnimeLibrary.RemoveFromLibrary(LibraryObject.Anime.ID);
            }
            else if (APIResponseHelpers.IsAPIResponseFailed(e.CurrentAPIResonse))
            {
                ProgressService.Reset();
            }
        }
        #endregion
        #endregion

        #region Get Anime
        public RelayCommand<ServiceID> GetAnimeCommand
        {
            get
            {
                return new RelayCommand<ServiceID>((serviceID) =>
                {
                    GetAnime(serviceID);
                });
            }
        }

        public void GetAnime(ServiceID serviceID)
        {
            if (string.IsNullOrWhiteSpace(serviceID.ID))
                return;

            Debug.WriteLine("Getting Anime");
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
                LibraryObject = HummingbirdAnimeLibraryViewModel.GetLibraryObject((AnimeObject)e.Parameter.Converted);
            }
            else if (APIResponseHelpers.IsAPIResponseFailed(e.CurrentAPIResonse))
            {
                ProgressService.Reset();
            }
        }
        #endregion

        #region Increment/Decrement Commands
        public RelayCommand IncrementRewatchingCommand { get { return new RelayCommand(() => { LibraryObject.RewatchedTimes++; }); } }
        public RelayCommand DecrementRewatchingCommand
        {
            get
            {
                return new RelayCommand(() => 
                    {
                        if (LibraryObject.RewatchedTimes > 0)
                            LibraryObject.RewatchedTimes--;
                    });
            }
        }
        #endregion

        #region Favourite
        public RelayCommand FavouriteCommand { get { return new RelayCommand(() => { Favourite(); }); } }
        public async void Favourite()
        {
            if (IsFavourited)
            {
                Debug.WriteLine("Unfavouriting");

                return;
            }

            Debug.WriteLine("Favouriting");
        }
        #endregion

        #region Set as Lock screen
        public RelayCommand SetAsLockscreenCommand { get { return new RelayCommand(() => { SetAsLockscreen(); }); } }
        public async void SetAsLockscreen()
        {
            Debug.WriteLine("Setting as Lockscreen");
            await LockscreenTools.Instance.SetLockscreenImage(LibraryObject.Anime.CoverImageUrl, LibraryObject.Anime.ID.ID);
        }
        #endregion

        #region Pin to Start
        public RelayCommand PinToStartCommand { get { return new RelayCommand(() => { PinToStart(); }); } }
        public async void PinToStart()
        {
            Debug.WriteLine("Pinning to Start");
            AnitroLaunchArgs launchArgs = new AnitroLaunchArgs();
            launchArgs.LaunchReason = AnitroLaunchReason.GoToDetails;
            launchArgs.Parameter = AnitroLaunchArgs.CreateAnimeParameter(LibraryObject.Anime.ID.ID);

            SecondaryTile tile = await TileManager.Instance.CreateTile(LibraryObject.Anime.ID.ID,
                                                                       LibraryObject.Anime.RomanjiTitle,
                                                                       launchArgs.CreateProtocol(),
                                                                       new TileImages(LibraryObject.Anime.CoverImageUrl),
                                                                       false);
            await TileManager.Instance.Pin(tile, Windows.UI.Popups.Placement.Default);
        }
        #endregion
    }
}