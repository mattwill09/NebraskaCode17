using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using UIKit;
using Xamarin.InAppPurchase;

namespace InAppPurchaseTest.Extensions
{
    public static class InAppPurchaseHandler
    {
        public static InAppPurchaseManager PurchaseManager;

        static InAppPurchaseHandler()
        {
            PurchaseManager = new InAppPurchaseManager();
        }


        public static IObservable<string[]> AllProductsObservble;
        private static IObserver<string[]> observer;

        public static IObservable<InAppProduct[]> PurchasedProducts;
        private static IObserver<InAppProduct[]> purchasedObserver;

        public static IObservable<InAppProduct[]> AvailableProducts;
        private static IObserver<InAppProduct[]> availableObserver;

        public static IObservable<string> DownloadProgress;
        private static IObserver<string> downloadProgressObserver;

        public static void InitializePurchaseManager()
        {

            var observable = Observable.Create((IObserver<string[]> arg) =>
            {
                observer = arg;
                return Disposable.Empty;
            }).Replay(1);

            observable.Connect();
            AllProductsObservble = observable;


            var purchasedObservable = Observable.Create((IObserver<InAppProduct[]> arg) =>
            {
                purchasedObserver = arg;
                return Disposable.Empty;
            }).Replay(1);

            purchasedObservable.Connect();
            PurchasedProducts = purchasedObservable;

            var availableObservable = Observable.Create((IObserver<InAppProduct[]> arg) =>
            {
                availableObserver = arg;
                return Disposable.Empty;
            }).Replay(1);

            availableObservable.Connect();
            AvailableProducts = availableObservable;

			var downloadProgressObservable = Observable.Create((IObserver<string> arg) =>
			{
				downloadProgressObserver = arg;
				return Disposable.Empty;
			}).Replay(1);

			downloadProgressObservable.Connect();
			DownloadProgress = downloadProgressObservable;

            // Assembly public key
            string value = Xamarin.InAppPurchase.Utilities.Security.Unify(
                new string[] { "1322f985c2",
                    "a34166b24",
                    "ab2b367",
                    "851cc6" },
                new int[] { 0, 1, 2, 3 });

            // Initialize the In App Purchase Manager
#if SIMULATED
            PurchaseManager.SimulateiTunesAppStore = true;
#else
            PurchaseManager.SimulateiTunesAppStore = false;
#endif
            PurchaseManager.PublicKey = value;
            PurchaseManager.ApplicationUserName = "KMullins";

            // Warn user that the store is not available
            if (PurchaseManager.CanMakePayments)
            {
                Console.WriteLine("Xamarin.InAppBilling: User can make payments to iTunes App Store.");
            }
            else
            {
                //Display Alert Dialog Box
                using (var alert = new UIAlertView("Xamarin.InAppBilling", "Sorry but you cannot make purchases from the In App Billing store. Please try again later.", null, "OK", null))
                {
                    alert.Show();
                }

            }


            // Warn user if the Purchase Manager is unable to connect to
            // the network.
            PurchaseManager.NoInternetConnectionAvailable += () =>
            {
                //Display Alert Dialog Box
                using (var alert = new UIAlertView("Xamarin.InAppBilling", "No open internet connection is available.", null, "OK", null))
                {
                    alert.Show();
                }
            };

            // Show any invalid product queries
            PurchaseManager.ReceivedInvalidProducts += (productIDs) =>
            {
                // Display any invalid product IDs to the console
                Console.WriteLine("The following IDs were rejected by the iTunes App Store:");
                foreach (string ID in productIDs)
                {
                    Console.WriteLine(ID);
                }
                Console.WriteLine(" ");
            };

            PurchaseManager.ReceivedValidProducts += (System.Collections.Generic.List<InAppProduct> products) =>
            {
                var productArray = products.Select(p => p.ProductIdentifier).ToArray();
                observer.OnNext(productArray);

                UpdatePurchases();
            };

            PurchaseManager.InAppProductPurchased += (transaction, product) => {
                UpdatePurchases();
			};

            // Report the results of the user restoring previous purchases
            PurchaseManager.InAppPurchasesRestored += (count) =>
            {
                // Anything restored?
                if (count == 0)
                {
                    // No, inform user
                    using (var alert = new UIAlertView("Xamarin.InAppPurchase", "No products were available to be restored from the iTunes App Store.", null, "OK", null))
                    {
                        alert.Show();
                    }
                }
                else
                {
                    // Yes, inform user
                    using (var alert = new UIAlertView("Xamarin.InAppPurchase", String.Format("{0} {1} restored from the iTunes App Store.", count, (count > 1) ? "products were" : "product was"), null, "OK", null))
                    {
                        alert.Show();
                    }
                }

                UpdatePurchases();
            };

			PurchaseManager.InAppProductPurchaseFailed += (transaction, product) =>
			{
				// Inform caller that the purchase of the requested product failed.
				// NOTE: The transaction will normally encode the reason for the failure but since
				// we are running in the simulated iTune App Store mode, no transaction will be returned.
				//Display Alert Dialog Box
				using (var alert = new UIAlertView("Xamarin.InAppPurchase", String.Format("Attempt to purchase {0} has failed: {1}", product.Title, transaction.Error.ToString()), null, "OK", null))
				{
					alert.Show();
				}

				// Force a reload to clear any locked items
				UpdatePurchases();
			};

            // Report miscellanous processing errors
            PurchaseManager.InAppPurchaseProcessingError += (message) =>
            {
                //Display Alert Dialog Box
                using (var alert = new UIAlertView("Xamarin.InAppPurchase", message, null, "OK", null))
                {
                    alert.Show();
                }
            };

            // Report any issues with persistence
            PurchaseManager.InAppProductPersistenceError += (message) =>
            {
                using (var alert = new UIAlertView("Xamarin.InAppPurchase", message, null, "OK", null))
                {
                    alert.Show();
                }
            };

            PurchaseManager.InAppPurchaseProductQuantityConsumed += (string identifier) =>  {
                UpdatePurchases();
            };

			PurchaseManager.TransactionObserver.InAppPurchaseContentDownloadInProgress += (download) =>
			{
				// Update the table to display the status of any downloads of hosted content
				// that we currently have in progress so we are forcing a table reload on the
				// download progress update. Since the final message will be the raising of the
				// InAppProductPurchased event, we'll just trap it to clear any completed
				// downloads instead of listening to the InAppPurchaseContentDownloadCompleted on the
				// purchase managers transaction observer.
				UpdatePurchases();

                // Display download percent in the badge
                downloadProgressObserver.OnNext(string.Format("{0:###}%", PurchaseManager.ActiveDownloadPercent * 100.0f));
			};

			PurchaseManager.TransactionObserver.InAppPurchaseContentDownloadCompleted += (download) =>
			{
				// Clear badge
                downloadProgressObserver.OnNext(null);
			};

			PurchaseManager.TransactionObserver.InAppPurchaseContentDownloadCanceled += (download) =>
			{
				// Clear badge
				downloadProgressObserver.OnNext(null);
			};

			PurchaseManager.TransactionObserver.InAppPurchaseContentDownloadFailed += (download) =>
			{
				// Clear badge
				downloadProgressObserver.OnNext(null);
			};

			PurchaseManager.TransactionObserver.InAppPurchaseContentDownloadFailed += (download) =>
			{
				// Inform the user that the download has failed. Normally download would contain
				// information about the failure that you would want to display to the user, since
				// we are running in simulation mode download will be null, so just display a 
				// generic failure message.
				using (var alert = new UIAlertView("Download Failed", "Unable to complete the downloading of content for the product being purchased. Please try again later.", null, "OK", null))
				{
					alert.Show();
				}

				// Force the table to reload to remove current download message
				UpdatePurchases();
			};

            // Setup automatic purchase persistance and load any previous purchases
            PurchaseManager.AutomaticPersistenceType = InAppPurchasePersistenceType.LocalFile;
            PurchaseManager.PersistenceFilename = "AtomicData";
            PurchaseManager.ShuffleProductsOnPersistence = false;
            PurchaseManager.RestoreProducts();

#if SIMULATED
            // Ask the iTunes App Store to return information about available In App Products for sale
            PurchaseManager.QueryInventory(new string[] {
                "product.nonconsumable",
                "feature.nonconsumable",
                "feature.nonconsumable.fail",
                "gold.coins.consumable_x25",
                "gold.coins.consumable_x50",
                "gold.coins.consumable_x100",
                "newsletter.freesubscription",
                "magazine.subscription.duration1month",
                "antivirus.nonrenewingsubscription.duration6months",
                "antivirus.nonrenewingsubscription.duration1year",
                "product.nonconsumable.invalid",
                "content.nonconsumable.downloadable",
                "content.nonconsumable.downloadfail",
                "content.nonconsumable.downloadupdate"
            });

            // Setup the list of simulated purchases to restore when doing a simulated restore of pruchases
            // from the iTunes App Store
            PurchaseManager.SimulatedRestoredPurchaseProducts = "product.nonconsumable,antivirus.nonrenewingsubscription.duration6months,content.nonconsumable.downloadable";
#else
            // Ask the iTunes App Store to return information about available In App Products for sale
            PurchaseManager.QueryInventory (new string[] { 
                "xam.iap.nonconsumable.widget",
                "xam.iap.subscription.duration1month",
                "xam.iap.subscription.duration1year",
                "xam.iap.subscription.duration3months"
            });
#endif

            UpdatePurchases();

        }

        private static void UpdatePurchases()
        {
            var availableProducts = new List<InAppProduct>();
            var purchasedProducts = new List<InAppProduct>();

            foreach (InAppProduct product in PurchaseManager)
            {
                if (product.Purchased)
                {
                    // Yes, add to list
                    purchasedProducts.Add(product);
                }

                // Take action based on the product type
                switch (product.ProductType)
                {
                    case InAppProductType.Consumable:
                        // Consumable products can always be purchased again
                        availableProducts.Add(product);
                        break;
                    case InAppProductType.AutoRenewableSubscription:
                    case InAppProductType.NonRenewingSubscription:
                        // Only display if the subscription has expired
                        if (product.SubscriptionExpired)
                        {
                            availableProducts.Add(product);
                        }
                        break;
                    default:
                        // Only display if the product hasn't been purchased
                        if (!product.Purchased)
                        {
                            availableProducts.Add(product);
                        }
                        break;
                }
            }

            purchasedObserver.OnNext(purchasedProducts.ToArray());
            availableObserver.OnNext(availableProducts.ToArray());
        }

    }
}
