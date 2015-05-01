using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
using Anitro.Data_Structures.Enumerators;

namespace Anitro.Data_Structures.Structures
{
    public class ActivityFeed
    {
        public ObservableCollection<API_Classes.ActivityFeedObject> Posts;
        public ObservableCollection<API_Classes.ActivityFeedObject> LibraryUpdates;
        public ObservableCollection<API_Classes.ActivityFeedObject> Miscellaneous;
        
        public ActivityFeed()
        {
            ClearActivityFeed(ActivityFeedSelection.All);
        }

        public void ClearActivityFeed(ActivityFeedSelection s)
        {
            switch (s)
            {
                case ActivityFeedSelection.Posts:
                    Posts = new ObservableCollection<API_Classes.ActivityFeedObject>() { };
                    break;
                case ActivityFeedSelection.LibraryUpdates:
                    LibraryUpdates = new ObservableCollection<API_Classes.ActivityFeedObject>() { };
                    break;
                case ActivityFeedSelection.Miscellaneous:
                    Miscellaneous = new ObservableCollection<API_Classes.ActivityFeedObject>() { };
                    break;
                case ActivityFeedSelection.All:
                    ClearActivityFeed(ActivityFeedSelection.Posts);
                    ClearActivityFeed(ActivityFeedSelection.LibraryUpdates);
                    ClearActivityFeed(ActivityFeedSelection.Miscellaneous);
                    break;
            }
        }

        public ObservableCollection<API_Classes.ActivityFeedObject> SortIntoSingleList()
        {
            ObservableCollection<API_Classes.ActivityFeedObject> selection = new ObservableCollection<API_Classes.ActivityFeedObject>() { };

            foreach (API_Classes.ActivityFeedObject afo in Posts) { selection.Add(afo); }
            foreach (API_Classes.ActivityFeedObject afo in LibraryUpdates) { selection.Add(afo); }
            foreach (API_Classes.ActivityFeedObject afo in Miscellaneous) { selection.Add(afo); }

            return selection;
        }

    }
}
