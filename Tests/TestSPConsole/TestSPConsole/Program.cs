using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace TestSPConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            string url = "http://sharepoint:2013/";

            SPSite siteCollection = new SPSite(url);
            SPWeb site = siteCollection.RootWeb;

            foreach(SPList list in site.Lists)
            {
                Console.WriteLine(list.Title);
            }

            Console.ReadLine();
        }
    }
}
