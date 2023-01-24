using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace Exercise1.GreetingWebPart
{
    public partial class VisualWebPart1UserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = SPContext.Current.Web.CurrentUser;
            var greeting = String.Format("Welcome to the Contoso intranet portal, {0}!", user.Name);
            lblGreeting.Text = greeting;

        }
    }
}
