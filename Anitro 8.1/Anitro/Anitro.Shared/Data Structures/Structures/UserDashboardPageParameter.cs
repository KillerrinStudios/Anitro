using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.Structures
{
    public class UserDashboardPageParameter
    {
        public User user;
        public bool initalLoad;

        public UserDashboardPageParameter(User _user, bool _initalLoad = false)
        {
            user = _user;
            initalLoad = _initalLoad;
        }
    }
}
