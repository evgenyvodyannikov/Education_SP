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
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
        }

        /// <summary>
        /// Обновлен элемент.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
        }

        /// <summary>
        /// Удален элемент.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }


    }
}