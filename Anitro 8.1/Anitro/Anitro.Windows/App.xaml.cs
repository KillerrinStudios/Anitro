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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using BugSense;
using BugSense.Model;
using BugSense.Core.Model;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Anitro
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        public static Frame RootFrame;
        public static SettingsPaneCommandsRequestedEventArgs settingsPaneCommands;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            // Initialize BugSense
            BugSenseHandler.Instance.HandleWhileDebugging = DebugTools.HandleBugSenseInDebug;
            BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), Consts.appData.BugSenseKey);
            BugSenseHandler.Instance.AddCrashExtraData(new CrashExtraData("Version", Consts.appData.OS.ToString()));

            // Other Windows Required Initialization
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            //RequestedTheme = ApplicationTheme.Light;
            //RequestedTheme = ApplicationTheme.Dark;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = RootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

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

            CodeToRunBeforeSuspend();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        // Handle protocol activations and continuation activations.
        /// </summary>
        protected override void OnActivated(IActivatedEventArgs e)
        {
            if (e.Kind == ActivationKind.Protocol)
            {
                //ProtocolActivatedEventArgs protocolArgs = e as ProtocolActivatedEventArgs;
                //Frame rootFrame = CreateRootFrame();
                ////RestoreStatus(e.PreviousExecutionState);

                //if (rootFrame.Content == null)
                //{
                //    if (!rootFrame.Navigate(typeof(MainPage)))
                //    {
                //        throw new Exception("Failed to create initial page");
                //    }
                //}

                //var p = rootFrame.Content as MainPage;
                //p.ProtocolEvent = protocolArgs;
                //p.NavigateToProtocolPage();
                ////Debug.WriteLine(protocolArgs.Uri.AbsoluteUri);

                //// Ensure the current window is active
                //Window.Current.Activate();
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    this.DebugSettings.EnableFrameRateCounter = true;
                }
#endif

                Frame rootFrame = Window.Current.Content as Frame;

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();

                    // TODO: change this value to a cache size that is appropriate for your application
                    rootFrame.CacheSize = 1;

                    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        // TODO: Load state from previously suspended application
                    }

                    // Place the frame in the current Window
                    Window.Current.Content = rootFrame;
                }

                if (rootFrame.Content == null)
                {
#if WINDOWS_PHONE_APP
                    // Removes the turnstile navigation for startup.
                    if (rootFrame.ContentTransitions != null)
                    {
                        this.transitions = new TransitionCollection();
                        foreach (var c in rootFrame.ContentTransitions)
                        {
                            this.transitions.Add(c);
                        }
                    }

                    rootFrame.ContentTransitions = null;
                    rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    if (!rootFrame.Navigate(typeof(MainPage), (e as ProtocolActivatedEventArgs).Uri.Segments[0]))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private Frame CreateRootFrame()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = RootFrame = new Frame();

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                rootFrame.NavigationFailed += OnNavigationFailed;

                //SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active.
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and 
                // navigate to the first page.
                rootFrame = new Frame();

                // Place the frame in the current Window.
                Window.Current.Content = rootFrame;
            }

            if (!rootFrame.Navigate(typeof(SearchPage), new Anitro.Data_Structures.Structures.SearchPageParameter(args.QueryText)))
            {
                throw new Exception("Failed to create initial page");
            }

            // Ensure the current window is active.
            Window.Current.Activate();
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            base.OnWindowCreated(args);
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "About", "About", (handler) => ShowAboutSettingsFlyout()));

            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "Options", "Options", (handler) => ShowCustomSettingFlyout()));

            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "Unlock", "Unlock Anitro", (handler) => ShowUnlockAnitroFlyout()));

            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "Privacy", "Privacy", OpenPrivacyPolicy));

            settingsPaneCommands = args;
        }

        public void ShowCustomSettingFlyout()
        {
            OptionsSettingsPane CustomSettingFlyout = new OptionsSettingsPane();
            CustomSettingFlyout.Show();
        }

        public void ShowAboutSettingsFlyout()
        {
            AboutSettingsPane CustomSettingFlyout = new AboutSettingsPane();
            CustomSettingFlyout.Show();
        }

        public void ShowUnlockAnitroFlyout()
        {
            UnlockAnitroFlyout unlockAnitroFlyout = new UnlockAnitroFlyout();
            unlockAnitroFlyout.Show();
        }

        private async void OpenPrivacyPolicy(IUICommand command)
        {
            Uri uri = new Uri("http://privacy.killerrin.com", UriKind.Absolute);
            await Windows.System.Launcher.LaunchUriAsync(uri);
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

        private void CodeToRunBeforeSuspend()
        {

        }
    }
}