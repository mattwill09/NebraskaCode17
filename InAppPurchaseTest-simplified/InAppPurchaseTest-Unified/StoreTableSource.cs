using System;
using System.Drawing;
using Foundation;
using UIKit;
using Xamarin.InAppPurchase;
using System.Collections.Generic;

namespace InAppPurchaseTest
{
    public class StoreTableSource : UITableViewSource
    {
        #region Private Variables
        private InAppProduct[] products = new InAppProduct[0];
        #endregion

        #region Public Methods
        /// <summary>
        /// Scans the Purchase Manage for products that have not already been purchased
        /// </summary>
        public void LoadData(InAppProduct[] prods)
        {
            products = prods;
        }
        #endregion

        #region Public Override Methods
        /// <Docs>Table view displaying the sections.</Docs>
        /// <returns>Number of sections required to display the data. The default is 1 (a table must have at least one section).</returns>
        /// <para>Declared in [UITableViewDataSource]</para>
        /// <summary>
        /// Numbers the of sections.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        public override nint NumberOfSections(UITableView tableView)
        {
            // Are we currently downloading content?
            if (Extensions.InAppPurchaseHandler.PurchaseManager.DownloadInProgress)
            {
                // Add a third section for downloads
                return 3;
            }
            else
            {
                // Hard coded two sections: Available Products & My Account
                return 2;
            }
        }

        /// <Docs>Table view displaying the rows.</Docs>
        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {

            // Set value based on the section
            switch (section)
            {
                case 0:
                    // Return the number of products
                    return products.Length;
            }

            // Default to one
            return 1;
        }

        /// <Docs>Table view containing the section.</Docs>
        /// <summary>
        /// Called to populate the header for the specified section.
        /// </summary>
        /// <see langword="null"></see>
        /// <returns>The for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override string TitleForHeader(UITableView tableView, nint section)
        {
            // Set value based on the section
            switch (section)
            {
                case 0:
                    // Products Header
                    return "Available Products";
                case 1:
                    // Advanced features
                    return "My Account";
                case 2:
                    // Downloads
                    return "Downloading Content";
            }

            // Default to nothing
            return "";
        }

        /// <Docs>Table view containing the section.</Docs>
        /// <summary>
        /// Called to populate the footer for the specified section.
        /// </summary>
        /// <see langword="null"></see>
        /// <returns>The for footer.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override string TitleForFooter(UITableView tableView, nint section)
        {
            // Not displaying a footer
            return "";
        }

        /// <Docs>Table view requesting the cell.</Docs>
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // Get table cell to hold data
            var cell = tableView.DequeueReusableCell(StoreTableCell.Key) as StoreTableCell;

            // Take action based on the section
            switch (indexPath.Section)
            {
                case 0:
                    // Get product for given cell
                    InAppProduct product = products[indexPath.Row];

                    // Populate cell with product information
                    cell.DisplayProduct(Extensions.InAppPurchaseHandler.PurchaseManager, product);
                    break;
                case 1:
                    // Display the link to restore purchases
                    cell.DisplayRestore(Extensions.InAppPurchaseHandler.PurchaseManager);
                    break;
                case 2:
                    // Display the currently downloading item
                    cell.DisplayDownload(Extensions.InAppPurchaseHandler.PurchaseManager);
                    break;
            }

            return cell;
        }
        #endregion
    }
}

