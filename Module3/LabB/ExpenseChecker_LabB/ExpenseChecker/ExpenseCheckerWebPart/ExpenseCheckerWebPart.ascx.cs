using Microsoft.Office.Server.Utilities;
using System.Data;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;

namespace ExpenseChecker.ExpenseCheckerWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class ExpenseCheckerWebPart : WebPart
    {
        // TODO: Ex 1 Task 2 Declare a DataTable variable
        DataTable dtable;

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling using
        // the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ExpenseCheckerWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            // TODO: Ex 1 Task 2 Instantiate the DataTable and add columns
            dtable = new DataTable();
            dtable.Columns.Add("UniqueId");
            dtable.Columns.Add("CapExRequestor");
            dtable.Columns.Add("CapExCategory");
            dtable.Columns.Add("CapExDescription");
            dtable.Columns.Add("CapExAmount", typeof(Decimal));

        }

        protected override void OnPreRender(EventArgs e)
        {
            // Create a query object.
            SPQuery query = new SPQuery();
            query.Query = @"
                <Where>
                    <And>
                        <Leq>
                            <FieldRef Name=""CapExAmount""></FieldRef>
                            <Value Type=""Currency"">500.00</Value>
                        </Leq>
                        <Eq>
                            <FieldRef Name=""CapExStatus""></FieldRef>
                            <Value Type=""Choice"">Pending</Value>
                        </Eq>
                    </And>
                </Where>
                ";
            query.ViewFields = @"                
                <FieldRef Name=""UniqueId"" />
                <FieldRef Name=""CapExRequestor"" />
                <FieldRef Name=""CapExCategory"" />
                <FieldRef Name=""CapExDescription"" />
                <FieldRef Name=""CapExAmount"" />                                                
                ";

            // Query the list and bind the results to the lstExpenses control.
            var web = SPContext.Current.Web;
            var list = web.Lists["Expenditure Requests"];
            
            // TODO: Ex 1 Task 5 Create and configure a ContentIterator object
            var items = list.GetItems(query);
            lstExpenses.DataSource = items.GetDataTable();
            lstExpenses.DataBind();
        }

        // TODO: Ex 1 Task 3 Add the ProcessItem method
        private void ProcessItem(SPListItem item)
        {
            string uniqueId = item["UniqueId"].ToString();
            var field = item.Fields["Requested By"] as SPFieldUser;
            var fieldValue = field.GetFieldValue(item["Requested By"].ToString()) as SPFieldUserValue;
            string username = fieldValue.User.Name;
            string category = item["Category"].ToString();
            string description = item["Description"].ToString();
            decimal amount = Decimal.Parse(item["Amount"].ToString());
            dtable.Rows.Add(uniqueId, username, category, description, amount);
        }

        // TODO: Ex 1 Task 4 Add the ProcessError method
        private bool ProcessError(SPListItem item, Exception e)
        {
            throw new Exception("An error occurred during item processing", e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            UpdateItems(true);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            UpdateItems(false);
        }

        private void UpdateItems(bool isApproved)
        {
            string status = isApproved ? "Approved" : "Rejected";

            // Retrieve the selected items from the lstExpenses control.
            var selectedItems = from item in lstExpenses.Items
                                where IsChecked(item)
                                select item;

            var web = SPContext.Current.Web;
            var list = web.Lists["Expenditure Requests"];

            foreach (var selectedItem in selectedItems)
            {
                // Get the unique identifier for each list item.
                var hiddenField = selectedItem.FindControl("hiddenID") as HiddenField;
                Guid itemID;
                if (Guid.TryParse(hiddenField.Value, out itemID))
                {
                    // Retrieve the list item and update the status.
                    SPListItem item = list.GetItemByUniqueId(itemID);
                    item["Request Status"] = status;
                    item.Update();
                }
            }
        }

        private static bool IsChecked(ListViewDataItem item)
        {
            var checkBox = item.FindControl("chkUpdate") as CheckBox;
            return checkBox.Checked;
        }
    }
}
