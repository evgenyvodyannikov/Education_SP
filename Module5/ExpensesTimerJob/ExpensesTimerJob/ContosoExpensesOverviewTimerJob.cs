using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;

namespace ExpensesTimerJob
{
    public class ContosoExpensesOverviewTimerJob : SPJobDefinition
    {

        public ContosoExpensesOverviewTimerJob()
        {

        }
  
        public override void Execute(Guid targetInstanceId)
        {
            using (SPSite site = new SPSite("http://sharepoint:2013"))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList overviewList = web.Lists["Contractor Agreements"];

                    string keyName = "ContosoDepartmentalExpenseTotal";
                    double totalValue = 0;

                    foreach (SPListItem item in overviewList.Items)
                    {
                        double currentItemInvoiceValue = 0;
                        double.TryParse(item.Properties["InvoiceTotal"].ToString(), out currentItemInvoiceValue);
                        totalValue += currentItemInvoiceValue;
                    }

                    web.Properties[keyName] = totalValue.ToString();
                    web.Properties.Update();
                }
            }
        }


    }
}
