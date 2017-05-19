using System;
using System.Drawing;
using Foundation;
using UIKit;
using Xamarin.InAppPurchase;
using System.Collections.Generic;

namespace InAppPurchaseTest
{
    public class PurchaseTableSource : UITableViewSource
    {
        private UIViewController _controller;
        #region Private Variables
        private InAppProduct[] purchases = new InAppProduct[0];
        #endregion

        public PurchaseTableSource(UIViewController controller) {
            _controller = controller;
        }

        #region Public Methods
        /// <summary>
        /// Scans the Purchase Manage for products that have already been purchased.
        /// </summary>
        public void LoadData(InAppProduct[] purch)
        {
            purchases = purch;
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
            // Only one section in this table
            return 1;
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

            // Return value
            return purchases.Length;
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
            return "Purchased Items";
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
            var cell = tableView.DequeueReusableCell(PurchaseTableCell.Key) as PurchaseTableCell;

            // Get product for given cell
            InAppProduct product = purchases[indexPath.Row];
           
            // Populate cell with product information
            cell.DisplayProduct(_controller, product);

            return cell;
        }
        #endregion
    }
}

