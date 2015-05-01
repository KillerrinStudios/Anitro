using System;
using System.Collections.Generic;
using System.Text;

using Anitro.Data_Structures;

namespace Anitro.APIs.Events
{
    public delegate void LibraryLoadedEventHandler(object sender, LibraryLoadedEventArgs e);

    public class LibraryLoadedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public APIResponse Result { get; private set; }
        public APIType Type { get; private set; }
        public object ResultObject { get; private set; }

        public LibraryLoadedEventArgs()
            : base(new Exception(), false, null)
        {
            Result = APIResponse.None;
            Type = APIType.None;
            ResultObject = null;
        }
        public LibraryLoadedEventArgs(APIResponse _result, APIType _type, object _ResultObject)
            : base(new Exception(), false, null)
        {
            Result = _result;
            Type = _type;
            ResultObject = _ResultObject;
        }
        public LibraryLoadedEventArgs(APIResponse _result, APIType _type, object _ResultObject, Exception e, bool canceled, Object state)
            :base(e, canceled, state)
        {
            Result = _result;
            Type = _type;
            ResultObject = _ResultObject;
        }
    }
}
