// This file has been autogenerated from a class added in the UI designer.
using System;
using Foundation;
using UIKit;
using Xamarin.InAppPurchase;

namespace InAppPurchaseTest
{
	public partial class MainViewController : UITabBarController
	{
		#region Private Variables
		private PurchaseTableViewController _purchaseTable;
		private StoreTableViewController _storeTable;
		private FeaturesController _featuresController;
		private SettingsController _settingsController;
		#endregion

		#region Computed Properties
		/// <summary>
		/// Gets the purchase table.
		/// </summary>
		/// <value>The purchase table.</value>
		public PurchaseTableViewController purchaseTable {
			get { return _purchaseTable; }
		}

		/// <summary>
		/// Gets the store table.
		/// </summary>
		/// <value>The store table.</value>
		public StoreTableViewController storeTable {
			get { return _storeTable; }
		}
		#endregion

		#region Constructors
		public MainViewController (IntPtr handle) : base (handle)
		{
		}
		#endregion

	}
}
