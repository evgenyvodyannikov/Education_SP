using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace VacationRequestsEventReceiver
{
    public class VacationRequestEventReceiver : SPItemEventReceiver
    {
        public override void ItemAdding(SPItemEventProperties properties)
        {
            using (var web = properties.OpenWeb())
            {
                // Get the duration of the vacation in days.
                string strStartDate = properties.AfterProperties["StartDate"].ToString();
                string strEndDate = properties.AfterProperties["_EndDate"].ToString();

                var startDate = DateTime.Parse(strStartDate).Date;
                var endDate = DateTime.Parse(strEndDate).Date;

                TimeSpan span = endDate - startDate;
                int vacationLength = span.Days;

                // Get the SPUser instance for the employee.
                var employeeFieldVal = new SPFieldUserValue(web, properties.AfterProperties["Employee"].ToString());
                SPUser employee = web.EnsureUser(employeeFieldVal.LookupValue);

                // Get the list item for the employee from the Vacation Tracker list.
                var listVT = web.GetList("Lists/VacationTracker");
                var query = new SPQuery();
                query.Query = String.Format(@"
		            <Where>
		                <Eq>
		                    <FieldRef Name=""Employee"" LookupId=""TRUE""></FieldRef>
		                    <Value Type=""Integer"">{0}</Value>
		                </Eq>
		            </Where>", employee.ID);

                var trackerItems = listVT.GetItems(query);

                // If you cannot find the employee in the Vacation Tracker list, fail gracefully.
                if (trackerItems.Count == 0)
                {
                    properties.ErrorMessage = string.Format("User {0} not found in Vacation Tracker list.", employee.Name);
                    properties.Status = SPEventReceiverStatus.CancelWithError;
                }
                else
                {
                    // Calculate whether the user has sufficient vacation entitlement to cover the request.
                    var trackerItem = trackerItems[0];
                    int daysRemaining;
                    int.TryParse(trackerItem["Days Remaining"].ToString(), out daysRemaining);
                    // If the user does not have sufficient vacation entitlement, block the request.
                    if (vacationLength > daysRemaining)
                    {
                        properties.ErrorMessage = string.Format("User {0} only has {1} vacation days remaining.", employee.Name, daysRemaining);
                        properties.Status = SPEventReceiverStatus.CancelWithError;
                    }
                }

            }
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            var status = properties.AfterProperties["VacationRequestStatus"].ToString();
            // No action required unless the status has been set to "Approved".
            if (!status.Equals("Approved"))
                return;

            using (var web = properties.OpenWeb())
            {
                // Get the duration of the vacation in days.
                var item = properties.ListItem;
                string strStartDate = item["StartDate"].ToString();
                string strEndDate = item["_EndDate"].ToString();
                var startDate = DateTime.Parse(strStartDate).Date;
                var endDate = DateTime.Parse(strEndDate).Date;
                TimeSpan span = endDate - startDate;
                int vacationLength = span.Days;

                // Get the SPUser instance for the employee.
                var employeeFieldVal = new SPFieldUserValue(web, properties.AfterProperties["Employee"].ToString());
                var employee = employeeFieldVal.User;

                // Get the list item for the employee from the Vacation Tracker list.
                var listVT = web.GetList("Lists/VacationTracker");
                var query = new SPQuery();
                query.Query = String.Format(@"
		        <Where>
		            <Eq>
		                 <FieldRef Name=""Employee"" LookupId=""TRUE""></FieldRef>
		                 <Value Type=""Integer"">{0}</Value>
		            </Eq>
		        </Where>", employee.ID);

                var trackerItems = listVT.GetItems(query);
                SPListItem trackerItem;
                if (trackerItems.Count > 0)
                {
                    trackerItem = trackerItems[0];
                }
                else return;

                // Update the remaining holiday entitlement in the Vacation Tracker list.
                int daysRemaining;
                int.TryParse(trackerItem["Days Remaining"].ToString(), out daysRemaining);
                int balance = daysRemaining - vacationLength;
                trackerItem["Days Remaining"] = balance;
                trackerItem.Update();

                // Update the vacation request status to "Booked".
                item["Vacation Request Status"] = "Booked";
                item.Update();
            }


        }
    }
}
