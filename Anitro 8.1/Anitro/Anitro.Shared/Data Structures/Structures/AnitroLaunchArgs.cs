using System;
using System.Collections.Generic;
using System.Text;
using Anitro.Data_Structures.Enumerators;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;

#if WINDOWS_PHONE_APP
using Windows.Media.SpeechRecognition;
#endif

namespace Anitro.Data_Structures.Structures
{
    public class AnitroLaunchArgs
    {
        public AnitroLaunchType type;
        public AppLaunchType launch;
        public string args;

        // Store Launch Arguments for further reference
#if WINDOWS_PHONE_APP
        public VoiceCommandActivatedEventArgs voiceCommandActivatedEventArgs;
#endif
        public ProtocolActivatedEventArgs protocolActivatedEventArgs;

#if WINDOWS_PHONE_APP
        // Store the Speech Recognition Result for further refrence
        SpeechRecognitionResult speechRecognitionResult;
#endif

        public AnitroLaunchArgs(AnitroLaunchType _type, string _args, AppLaunchType _status = AppLaunchType.UriAssociation)
        {
            type = _type;
            args = _args;
            launch = _status;
        }

        public AnitroLaunchArgs(string _parameters, bool cortanaActivated = false)
        {
            Debug.WriteLine("AnitroLaunchArgs(" + _parameters + ")");

            try
            {
                if (cortanaActivated)
                {
                    VoiceCommandParse(_parameters);
                }
                else
                {
                    NormalParse(_parameters);
                }
            }
            catch(Exception) { Failed(); }
        }

        public AnitroLaunchArgs(ProtocolActivatedEventArgs _protocolActivatedEventArgs)
        {
            try
            {
                protocolActivatedEventArgs = _protocolActivatedEventArgs;

                NormalParse(_protocolActivatedEventArgs.Uri.Segments[0].ToString());
            }
            catch (Exception) { Failed(); }
        }

#if WINDOWS_PHONE_APP
        public AnitroLaunchArgs(VoiceCommandActivatedEventArgs _voiceCommandActivatedEventArgs)
        {
            try
            {
                voiceCommandActivatedEventArgs = _voiceCommandActivatedEventArgs;
                speechRecognitionResult = (_voiceCommandActivatedEventArgs.Result as Windows.Media.SpeechRecognition.SpeechRecognitionResult);

                //speechRecognitionResult.SemanticInterpretation

                VoiceCommandParse(speechRecognitionResult.Text);
            }
            catch (Exception) { Failed(); }
        }
#endif

        public void Failed()
        {
            type = AnitroLaunchType.Failed;
            launch = AppLaunchType.None;
            args = "";
        }

        public override string ToString()
        {
            string sArgs;

            try
            {
                sArgs = "type=" + type.ToString() + "&" +
                               "args=" + args + "&" +
                               "status=" + launch.ToString();// AppLaunchType.UriAssociation.ToString();
            }
            catch (Exception) { 
                Failed();
                
                sArgs = "type=" + type.ToString() + "&" +
                        "args=" + args + "&" +
                        "status=" + launch.ToString();
            }

            Debug.WriteLine(sArgs);
            return sArgs;
        }

        #region Parses
        private void VoiceCommandParse(string _parameters)
        {
            string command;
            string query;
            SplitSpeechCommandFromQuery(_parameters, out command, out query);

            if (command == "" || query == "") {
                Failed();

                return;
            }

            type = GetAnitroLaunchTypeFromString(command);
            args = query;

            launch = AppLaunchType.CortanaVoice;
            launch = AppLaunchType.CortanaText;
        }

        private void NormalParse(string _parameters)
        {
            // Split the strings off of the & symbol
            string[] param = _parameters.Split('&');

            // Break each one down further to get the individual parts
            string[] paramType = param[0].Split('=');
            type = GetAnitroLaunchTypeFromString(paramType[1]);


            // Check to see if it is an Url, if it is, parse for what we want, if not, grab it normally
            string[] paramArgs = param[1].Split('=');
            if (type == AnitroLaunchType.Url)
            {
                args = GetArgsFromUrl(paramArgs[1]);
            }
            else
            {
                args = paramArgs[1];
            }

            //// Set the launch type to an Uri Association for future purposes
            //launch = AppLaunchType.UriAssociation;

            // 3 items is not always guarenteed, so check if there is 3 or more then grab the third.
            if (type != AnitroLaunchType.Url && param.Length >= 3)
            {
                string[] paramStatus = param[2].Split('=');
                launch = GetAppLaunchTypeFromString(paramStatus[1]);
            }
            else
            {
                launch = AppLaunchType.UriAssociation;
            }
        }
        #endregion

        public static AnitroLaunchType GetAnitroLaunchTypeFromString(string _str)
        {
            string str = _str.ToLower();
            switch (str)
            {
                case "anime":
                case "Anime":
                    return AnitroLaunchType.Anime;

                case "manga":
                case "Manga":
                    return AnitroLaunchType.Manga;

                case "search":
                case "Search": 
                    return AnitroLaunchType.Search;

                case "url":
                case "Url":
                    return AnitroLaunchType.Url;

                case "none":
                case "None":
                default:
                    return AnitroLaunchType.None;
            }
        }

        public static AppLaunchType GetAppLaunchTypeFromString(string _str)
        {
            string str = _str.ToLower();
            switch(str)
            {
                case "uriassociation":
                case "UriAssociation":
                    return AppLaunchType.UriAssociation;

                case "cortanavoice":
                case "CortanaVoice":
                    return AppLaunchType.CortanaVoice;

                case "cortanatext":
                case "CortanaText":
                    return AppLaunchType.CortanaText;
                
                case "none":
                case "None":
                default:
                    return AppLaunchType.None;
            }
        }

        private string GetArgsFromUrl(string _url)
        {
            Debug.WriteLine("GetArgsFromUrl(" + _url + ")");

            if (_url.Contains("search")) { type = AnitroLaunchType.Search; }
            else if (_url.Contains("anime")) { type = AnitroLaunchType.Anime; }
            else if (_url.Contains("manga")) { type = AnitroLaunchType.Manga; }
            else { 
                Debug.WriteLine("Setting to None");
                type = AnitroLaunchType.None;
                return _url;
            }

            int indexOfMe = _url.IndexOf(".me");

            string parsedString = "";
            if (_url.Contains("search"))
            {
                indexOfMe += 17;
                parsedString = _url.Substring(indexOfMe);
                parsedString = Anitro.APIs.Hummingbird.Helpers.ConvertToAPIConpliantString(parsedString, '+', ' ');

                string[] parsedStrings = parsedString.Split('&');
                parsedString = parsedStrings[0];
            }
            else
            {
                indexOfMe += 10;
                parsedString = _url.Substring(indexOfMe);

                string[] parsedStrings = parsedString.Split('/');
                parsedString = parsedStrings[0];
            }

            return parsedString;
        }

        private void SplitSpeechCommandFromQuery(string _parameters, out string command, out string query)
        {
            if (_parameters.Contains("Search for"))
            {
                command = "Search";
                query = _parameters.Substring(_parameters.IndexOf("for") + 3 + 1);
            }
            else if (_parameters.Contains("Search"))
            {
                command = "Search";
                query = _parameters.Substring(6 + 1);
            }

            else if (_parameters.Contains("Find me"))
            {
                command = "Search";
                query = _parameters.Substring(_parameters.IndexOf("me") + 2 + 1);
            }
            else if (_parameters.Contains("Find"))
            {
                command = "Search";
                query = _parameters.Substring(4 + 1);
            }

            // We couldn't find anything, so we set to nothing;
            else
            {
                command = "";
                query = "";
            }
        }
    }
}
