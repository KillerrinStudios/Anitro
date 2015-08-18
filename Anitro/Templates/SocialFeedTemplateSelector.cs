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
    public class SocialFeedTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TemplateComment { get; set; }
        public DataTemplate TemplateCommentTo { get; set; }
        public DataTemplate TemplateFollowedMessage { get; set; }
        public DataTemplate TemplateMediaUpdate { get; set; }

        protected override DataTemplate SelectTemplateCore (object item, DependencyObject container)
        {
            AActivityFeedItem activityFeedItem = item as AActivityFeedItem;

            if (activityFeedItem is ActivityFeedComment)
            {
                return TemplateComment;
            }
            else if (activityFeedItem is ActivityFeedCommentTo)
            {
                return TemplateCommentTo;
            }
            else if (activityFeedItem is ActivityFeedFollowedMessage)
            {
                return TemplateFollowedMessage;
            }
            else if (activityFeedItem is ActivityFeedMediaUpdate)
            {
                return TemplateMediaUpdate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
