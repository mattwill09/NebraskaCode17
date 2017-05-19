using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using InAppPurchaseTest.Extensions;
using UIKit;
using Xamarin.InAppPurchase;
using Xamarin.InAppPurchase.Utilities;


namespace InAppPurchaseTest
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{

		#region Public Properties
		public static UIStoryboard Storyboard = UIStoryboard.FromName ("MainStoryboard", null);
		public MainViewController mainView;
		public InAppPurchaseManager PurchaseManager = new InAppPurchaseManager ();
		#endregion

		#region Computed Properties
		/// <summary>
		/// Gets or sets the window.
		/// </summary>
		/// <value>The window.</value>
		public override UIWindow Window {
			get;
			set;
		}
		#endregion 

		#region Override Methods
		/// <summary>
		/// Finisheds the launching.
		/// </summary>
		/// <returns><c>true</c>, if launching was finisheded, <c>false</c> otherwise.</returns>
		/// <param name="application">Application.</param>
		/// <param name="launchOptions">Launch options.</param>
		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
            InAppPurchaseHandler.InitializePurchaseManager();

            // Build a window
            Window = new UIWindow (UIScreen.MainScreen.Bounds);

			// Get the defaul view from the storyboard
			mainView = Storyboard.InstantiateInitialViewController () as MainViewController;

			// Display the first view
			Window.RootViewController = mainView;
			Window.MakeKeyAndVisible ();

			return true;
		}

		// This method is invoked when the application is about to move from active to inactive state.
		// OpenGL applications should use this method to pause.
		public override void OnResignActivation (UIApplication application)
		{
		}
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}
		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		// This method is called when the application is about to terminate. Save data, if needed.
		public override void WillTerminate (UIApplication application)
		{
		}
		#endregion
	}
}

