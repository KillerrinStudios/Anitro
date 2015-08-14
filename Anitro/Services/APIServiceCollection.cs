using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Services
{
    public class APIServiceCollection
    {
        private static APIServiceCollection m_instance = new APIServiceCollection();
        public static APIServiceCollection Instance
        {
            get
            {
                if (m_instance == null) m_instance = new APIServiceCollection();
                return m_instance;
            }
        }

        // The Services
        public HummingbirdV1Service HummingbirdV1API = new HummingbirdV1Service("");

        // Setup and Load the API Services
        private APIServiceCollection()
        {
        }
    }
}
