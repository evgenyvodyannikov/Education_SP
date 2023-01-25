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


            Console.ReadKey();

        }
    }
}
