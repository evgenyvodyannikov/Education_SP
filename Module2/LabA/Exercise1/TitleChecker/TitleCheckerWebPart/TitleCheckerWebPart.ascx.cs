using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace TitleChecker
{
    [ToolboxItemAttribute(false)]
    public partial class TitleCheckerWebPart : WebPart
    {
        Guid selectedSiteGuid = Guid.Empty;
        bool siteUpdated = false;

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling using
        // the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public TitleCheckerWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected override void OnPreRender(EventArgs e)
        {
            pnlUpdateControls.Visible = false;
            pnlResult.Visible = false;

            var site = SPContext.Current.Site;
            lstWebs.Items.Clear();
            foreach (SPWeb web in site.AllWebs)
            {
                try
                {
                    lstWebs.Items.Add(new ListItem(web.Title, web.ID.ToString()));
                }
                finally
                {
                    web.Dispose();
                }
            }

            if (siteUpdated)
            {
                lstWebs.SelectedIndex = -1;
                selectedSiteGuid = Guid.Empty;                
                pnlResult.Visible = true;
            }
            else
            {
                pnlResult.Visible = false;
            }            

            if (!selectedSiteGuid.Equals(Guid.Empty))
            {
                lstWebs.Items.FindByValue(selectedSiteGuid.ToString()).Selected = true;
                pnlUpdateControls.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        protected void lstWebs_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSiteGuid = new Guid(lstWebs.SelectedValue);
            txtTitle.Text = lstWebs.SelectedItem.Text;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            selectedSiteGuid = new Guid(lstWebs.SelectedValue);
            string newTitle = txtTitle.Text;
            
            if (!String.IsNullOrEmpty(newTitle) && !selectedSiteGuid.Equals(Guid.Empty))
            {
                using (SPWeb web = SPContext.Current.Site.OpenWeb(selectedSiteGuid))
                {
                    web.Title = newTitle;
                    web.Update();
                    litResult.Text = String.Format("The title of the site at <i>{0}</i> has been changed to <i>{1}</i>.", web.Url, newTitle);
                }
                siteUpdated = true;                
            }
        }
    }
}
