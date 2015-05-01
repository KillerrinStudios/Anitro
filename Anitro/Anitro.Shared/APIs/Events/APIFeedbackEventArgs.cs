using System;
using System.Collections.Generic;
using System.Text;

using Anitro.Data_Structures;

namespace Anitro.APIs.Events
{
    public delegate void APIFeedbackEventHandler(object sender, APIFeedbackEventArgs e);

    public class APIFeedbackEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        public APIResponse Result { get; private set; }
        public APIType Type { get; private set; }
        public String FeedbackMessage { get; private set; }

        public APIFeedbackEventArgs(string _message = "")
            : base(new Exception(), false, null)
        {
            Result = APIResponse.None;
            Type = APIType.None;
            FeedbackMessage = _message;
        }
        public APIFeedbackEventArgs(APIResponse _result, APIType _type, string _message)
            : base(new Exception(), false, null)
        {
            Result = _result;
            Type = _type;
            FeedbackMessage = _message;
        }
        public APIFeedbackEventArgs(APIResponse _result, APIType _type, string _message, Exception e, bool canceled, Object state)
            : base(e, canceled, state)
        {
            Result = _result;
            Type = _type;
            FeedbackMessage = _message;
        }
    }
}
