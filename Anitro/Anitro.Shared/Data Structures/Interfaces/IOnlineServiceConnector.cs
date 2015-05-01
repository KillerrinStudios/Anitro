using Anitro.APIs.Events;
using Anitro.Data_Structures.API_Classes;
using Anitro.Data_Structures.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Data_Structures.Interfaces
{
    public interface IOnineServiceConnector
    {
        OnlineService OnlineService { get; }

        event APIFeedbackEventHandler FeedbackEventHandler;
        event APICompletedEventHandler APICompletedEventHandler;

        Task GetAnime(string serviceID);
        Task Search(string searchTerms, SearchFilter filter);

        Task GetLibrary(LibraryType library, LibrarySelection selection = LibrarySelection.All);
        Task GetFavourites(LibraryType library);

        Task Login(string userMail, string password, bool onlyObtainAuthToken = false);
        Task LibraryUpdate(LibraryObject libraryObject);
        Task LibraryRemove(string serviceID);
    }
}
