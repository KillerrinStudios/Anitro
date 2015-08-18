using AnimeTrackingServiceWrapper.UniversalServiceModels;
using AnimeTrackingServiceWrapper.UniversalServiceModels.ActivityFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Anitro.Templates
{
    public class LibraryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TemplateCurrentlyWatching { get; set; }
        public DataTemplate TemplatePlanToWatch { get; set; }
        public DataTemplate TemplateCompleted { get; set; }
        public DataTemplate TemplateOnHold { get; set; }
        public DataTemplate TemplateDropped { get; set; }

        protected override DataTemplate SelectTemplateCore (object item, DependencyObject container)
        {
            LibraryObject libraryObject = item as LibraryObject;

            if (libraryObject.Section == LibrarySection.CurrentlyWatching ||
                libraryObject.Section == LibrarySection.CurrentlyReading)
            {
                return TemplateCurrentlyWatching;
            }
            else if (libraryObject.Section == LibrarySection.PlanToWatch ||
                     libraryObject.Section == LibrarySection.PlanToRead)
            {
                return TemplatePlanToWatch;
            }
            else if (libraryObject.Section == LibrarySection.Completed)
            {
                return TemplateCompleted;
            }
            else if (libraryObject.Section == LibrarySection.OnHold)
            {
                return TemplateOnHold;
            }
            else if (libraryObject.Section == LibrarySection.Dropped)
            {
                return TemplateDropped;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
