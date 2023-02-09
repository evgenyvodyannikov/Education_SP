using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace SiteArchiveFeature.SalesWebDeletingReceiver
{
    /// <summary>
    /// Веб-события
    /// </summary>
    public class SalesWebDeletingReceiver : SPWebEventReceiver
    {
        /// <summary>
        /// Удаляется сайт.
        /// </summary>
		public override void WebDeleting(SPWebEventProperties properties)
        {
            using (var site = new SPSite(properties.SiteId))
            {
                string backupLocation = @"F:\Module13\backups\Backup.backup";
                site.WebApplication.Sites.Backup(site.Url, backupLocation, true);
            }
        }
    }
}