using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace ExpensesWebPart.ExpensesInformationWebPart
{
    [ToolboxItemAttribute(false)]
    public class ExpensesInformationWebPart : WebPart
    {
        protected override void CreateChildControls()
        {
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            SPWeb web = SPContext.Current.Web;
            string keyName = "ContosoDepartmentalExpenseTotal";
            if (web.Properties[keyName] != null)
            {
                string expenseTotal = web.Properties[keyName];
                writer.Write("<p>Estimated departmental expense total is: " + expenseTotal + "</p>");
            }
            else
            {
                writer.Write("<p>Estimated departmental expense was never changed</p>");
            }
            

        }
    }
}
