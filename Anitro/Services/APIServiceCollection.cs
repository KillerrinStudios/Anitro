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
        public static APIServiceCollection Instance { get; } = new APIServiceCollection();

        #region Services
        public HummingbirdV1Service HummingbirdV1API { get; } = new HummingbirdV1Service("");
        #endregion

        // Setup and Load the API Services
        private APIServiceCollection()
        {

        }
    }
}
