using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SPClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get the unique identifier for the local farm.
            Guid farmID = SPFarm.Local.Id;
            //Determine whether the current user is a farm administrator.
            bool isAdmin = SPFarm.Local.CurrentUserIsAdministrator();
            Console.WriteLine("FarmID {0}\nisAdmin {1}", farmID, isAdmin);

            // display all Services with Online status.
            //foreach (SPService service in SPFarm.Local.Services)
            //{
            //    foreach (SPServiceInstance instance in service.Instances)
            //    {
            //        if (instance.Status == SPObjectStatus.Online)
            //        {
            //            Console.WriteLine("\nInstance: {0} with type {1} is Online", instance, instance.TypeName);
            //        }
            //    }
            //}

            // Get a reference to the parent SPWebService instance.
            var contentService = SPWebService.ContentService;
            // Use an indexer to retrieve the web application by display name.
            var webApp = contentService.WebApplications["Test"];
            //// Change the maximum file size (in MB) that users can upload.
            webApp.MaximumFileSize = 75;
            //// Persist the changes.
            webApp.Update();
            Console.WriteLine(webApp.ToString());


            // Pass a URL to the SPSite constructor.
            var site1 = new SPSite("http://sharepoint:4444");
            Console.WriteLine("site1: {0}", site1);
            // Retrieve an SPSite from the parent SPWebApplication.
            var contentService1 = SPWebService.ContentService;
            var webApp1 = contentService1.WebApplications["Test"];
            var site2 = webApp.Sites["sharepoint"];
            Console.WriteLine("site2: {0}", site2);
            // Retrieve an SPSite from the current execution context.
            var site3 = SPContext.Current != null ? SPContext.Current.Site.ToString() : "not present";
            Console.WriteLine("site3: {0}", site3);
            // Dispose of SPSite objects where appropriate after use.
            site1?.Dispose();
            site2?.Dispose();

            Console.ReadKey();

        }
    }
}
