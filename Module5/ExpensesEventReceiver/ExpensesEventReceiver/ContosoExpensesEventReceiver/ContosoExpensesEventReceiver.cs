using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace ExpensesEventReceiver.ContosoExpensesEventReceiver
{
    /// <summary>
    /// События элемента списка
    /// </summary>
    public class ContosoExpensesEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// Добавлен элемент.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            double invoiceValue = 0;
            double.TryParse(properties.AfterProperties["InvoiceTotal"].ToString(), out invoiceValue);
            UpdatePropertyBag(properties.Web, invoiceValue);
        }


        /// <summary>
        /// Обновлен элемент.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            double previousInvoiceValue = (double)properties.ListItem["InvoiceTotal"];
            double newInvoiceValue = 0;
            double.TryParse(properties.AfterProperties["InvoiceTotal"].ToString(), out newInvoiceValue);
            double change = newInvoiceValue - previousInvoiceValue;
            UpdatePropertyBag(properties.Web, change);
        }

        /// <summary>
        /// Удален элемент.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            // ERROR: if use "as string" get Null
            double invoiceValue = 0;
            double.TryParse(properties.BeforeProperties["InvoiceTotal"].ToString(), out invoiceValue);
            UpdatePropertyBag(properties.Web, -invoiceValue);
        }

        private void UpdatePropertyBag(SPWeb web, double change)
        {
            string keyName = "ContosoDepartmentalExpenseTotal";
            double currentValue = 0;
            if (web.Properties[keyName] != null)
            {
                currentValue = double.Parse(web.Properties[keyName]);
            }
            else
            {
                web.Properties.Add(keyName, "");
            }
            currentValue += change;
            web.Properties[keyName] = currentValue.ToString();
            web.Properties.Update();
        }

    }
}