// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Xamarin.InAppPurchase;
using InAppPurchaseTest.Extensions;

namespace InAppPurchaseTest
{
    public partial class StoreTableViewController : UITableViewController
    {
        private IDisposable _progressSubscription;
        private IDisposable _availableSubscription;

        #region Computed Properties
        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public StoreTableSource DataSource
        {
            get { return (StoreTableSource)TableView.Source; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="InAppPurchaseTest.PurchaseTableViewController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public StoreTableViewController(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Public Override Methods
        /// <Docs>Called when the system is running low on memory.</Docs>
        /// <summary>
        /// Dids the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Adjust for iOS 7
            TableView.ContentInset = new UIEdgeInsets(20, 0, 0, 0);
            TableView.ContentOffset = new System.Drawing.PointF(0, -20);

            // Register the TableView's data source
            TableView.Source = new StoreTableSource();

            _progressSubscription = InAppPurchaseHandler.DownloadProgress
                .Subscribe((string obj) =>
	            {
	                StoreTab.BadgeValue = obj;
	            });

            _availableSubscription = InAppPurchaseHandler.AvailableProducts
                 .Subscribe((InAppProduct[] obj) =>
                 {
                    DataSource.LoadData(obj);
                    TableView.ReloadData();
                 });

        }
        #endregion
    }
}
