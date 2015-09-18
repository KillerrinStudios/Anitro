using Anitro.Helpers;
using Anitro.Models.Page_Parameters;
using Anitro.Models.Enums;
using Anitro.Pages;
using GalaSoft.MvvmLight.Ioc;
using Killerrin_Studios_Toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Anitro
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            UnhandledException += App_UnhandledException;

            // Now, Initialize the App
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null)
            {
                Exception exception = e.Exception;
                if (exception is NullReferenceException && exception.ToString().ToUpper().Contains("SOMA"))
                {
                    Debug.WriteLine("Handled Smaato null reference exception {0}", exception);
                    e.Handled = true;
                    return;
                }
            }
            // APP SPECIFIC HANDLING HERE

            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            #region Debugger Code
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            #endregion

            AnitroLaunchArgs args = new AnitroLaunchArgs();
            args.ParseSecondaryTile(e.Arguments);
            Frame rootFrame = CreateRootFrame(e.PreviousExecutionState, args);

            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            AnitroLaunchArgs launchArgs = new AnitroLaunchArgs();

            try {
                if (args.Kind == ActivationKind.VoiceCommand)
                {
                    var commandArgs = args as Windows.ApplicationModel.Activation.VoiceCommandActivatedEventArgs;
                    var speechRecognitionResult = commandArgs.Result;

                    var rulePath = speechRecognitionResult.RulePath[0];
                    launchArgs.Activation = AnitroLaunchActivation.VoiceCommand;
                    launchArgs.LaunchReason = AnitroLaunchArgs.ParseLaunchReason(rulePath);

                    // Staying because Speech is a bitch and this will debug
                    //foreach (var key in speechRecognitionResult.SemanticInterpretation.Properties.Keys)
                    //{
                    //    foreach (var param in speechRecognitionResult.SemanticInterpretation.Properties[key])
                    //    {
                    //        Debug.WriteLine(key + ", " + param);
                    //    }
                    //}
                    //Debug.WriteLine(speechRecognitionResult.SemanticInterpretation.Properties["DictatedTerms"][0]);

                    launchArgs.Parameter = speechRecognitionResult.SemanticInterpretation.Properties["DictatedTerms"][0];
                }
                else if (args.Kind == ActivationKind.Protocol)
                {
                    var commandArgs = args as Windows.ApplicationModel.Activation.ProtocolActivatedEventArgs;
                    Debug.WriteLine(commandArgs.Uri.OriginalString);

                    launchArgs.Activation = AnitroLaunchActivation.Protocol;
                    launchArgs.ParseProtocol(commandArgs.Uri);
                }
                else if (args.Kind == ActivationKind.Search)
                {
                    var commandArgs = args as Windows.ApplicationModel.Activation.SearchActivatedEventArgs;
                    launchArgs.Activation = AnitroLaunchActivation.Search;
                    launchArgs.LaunchReason = AnitroLaunchReason.Search;
                    launchArgs.Parameter = AnitroLaunchArgs.CreateSearchParameter(commandArgs.QueryText);
                }
                else if (args.Kind == ActivationKind.LockScreen)
                {
                    var commandArgs = args as Windows.ApplicationModel.Activation.LockScreenActivatedEventArgs;
                    launchArgs.Activation = AnitroLaunchActivation.Lockscreen;
                    launchArgs.LaunchReason = AnitroLaunchReason.Lockscreen;
                }
                else
                {
                    launchArgs = new AnitroLaunchArgs();
                }
            }
            catch (Exception) { launchArgs = new AnitroLaunchArgs(); }

            Frame rootFrame = CreateRootFrame(args.PreviousExecutionState, launchArgs);

            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            base.OnSearchActivated(args);
        }

        public Frame CreateRootFrame(ApplicationExecutionState PreviousExecutionState, AnitroLaunchArgs launchArgs)
        {
            Debug.WriteLine(launchArgs.ToString());

            // Create the Root Frame
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), launchArgs);
            }

            return rootFrame;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
