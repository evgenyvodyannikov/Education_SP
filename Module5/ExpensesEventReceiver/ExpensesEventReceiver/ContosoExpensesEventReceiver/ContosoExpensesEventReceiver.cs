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
            base.ItemAdded(properties);
        }

        /// <summary>
        /// Обновлен элемент.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
        }

        /// <summary>
        /// Удален элемент.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
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