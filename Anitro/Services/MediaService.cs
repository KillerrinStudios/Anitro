using Anitro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Anitro.Services
{
    public class MediaService
    {
        public MediaElement m_mediaElement;
        public MediaService(MediaElement mediaElement)
        {
            m_mediaElement = mediaElement;
        }

        public void SetSource(Uri source)
        {
            m_mediaElement.Source = source;
        }

        public void Play()
        {
            m_mediaElement.Play();
        }

        public void Pause()
        {
            m_mediaElement.Pause();
        }

        public void Stop()
        {
            m_mediaElement.Stop();
        }

        public bool CanPause { get { return m_mediaElement.CanPause; } }
    }
}
