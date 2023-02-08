using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace VacationRequestsEventReceiver.Features.VacationEntitlementChecker
{
    /// <summary>
    /// Этот класс обрабатывает события, возникающие в ходе активации, деактивации, установки, удаления и обновления компонентов.
    /// </summary>
    /// <remarks>
    /// GUID, присоединенный к этому классу, может использоваться при создании пакета и не должен изменяться.
    /// </remarks>

    [Guid("196b06b2-fe0b-4cac-bbee-39b742ce0793")]
    public class VacationEntitlementCheckerEventReceiver : SPFeatureReceiver
    {
        // Раскомментируйте ниже метод для обработки события, возникающего после активации компонента.
    
        const string itemAddingName = "Vacation Request ItemAdding";
        const string itemUpdatedName = "Vacation Request ItemUpdated";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            AddItemAddingReceiver(properties);
            AddItemUpdatedReceiver(properties);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            RemoveItemAddingReceiver(properties);
            RemoveItemUpdatedReceiver(properties);
        }

        private static void AddItemAddingReceiver(SPFeatureReceiverProperties properties)
        {
            using (var site = properties.Feature.Parent as SPSite)
            {
                using (var web = site.RootWeb)
                {
                    // Get the Vacation Request content type.
                    var vacationRequestCT = web.ContentTypes["Vacation Request"];
                    if (vacationRequestCT != null)
                    {
                        // Check for existing event receivers.
                        Guid existingEventReceiverId = Guid.Empty;
                        foreach (SPEventReceiverDefinition eventReceiver in vacationRequestCT.EventReceivers)
                        {
                            if (String.Equals(eventReceiver.Name, itemAddingName))
                            {
                                existingEventReceiverId = eventReceiver.Id;
                            }
                        }
                        if (!existingEventReceiverId.Equals(Guid.Empty))
                        {
                            vacationRequestCT.EventReceivers[existingEventReceiverId].Delete();
                            vacationRequestCT.Update(true);
                        }

                        SPEventReceiverDefinition erd = vacationRequestCT.EventReceivers.Add();
                        erd.Assembly = "VacationRequestsEventReceiver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b8b5cfca861bb4cc";
                        erd.Class = "VacationRequestsEventReceiver.VacationRequestEventReceiver";
                        erd.Type = SPEventReceiverType.ItemAdding;
                        erd.Name = itemAddingName;
                        erd.Synchronization = SPEventReceiverSynchronization.Synchronous;
                        erd.SequenceNumber = 10050;
                        erd.Update();
                        vacationRequestCT.Update(true);
                    }
                }
            }
        }

        private static void AddItemUpdatedReceiver(SPFeatureReceiverProperties properties)
        {
            using (var site = properties.Feature.Parent as SPSite)
            {
                using (var web = site.RootWeb)
                {
                    // Get the Vacation Request content type.
                    var vacationRequestCT = web.ContentTypes["Vacation Request"];
                    if (vacationRequestCT != null)
                    {
                        // Check for existing event receivers.
                        Guid existingEventReceiverId = Guid.Empty;
                        foreach (SPEventReceiverDefinition eventReceiver in vacationRequestCT.EventReceivers)
                        {
                            if (String.Equals(eventReceiver.Name, itemUpdatedName))
                            {
                                existingEventReceiverId = eventReceiver.Id;
                            }
                        }
                        if (!existingEventReceiverId.Equals(Guid.Empty))
                        {
                            vacationRequestCT.EventReceivers[existingEventReceiverId].Delete();
                            vacationRequestCT.Update(true);
                        }

                        SPEventReceiverDefinition erd = vacationRequestCT.EventReceivers.Add();
                        erd.Assembly = "VacationRequestsEventReceiver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b8b5cfca861bb4cc";
                        erd.Class = "VacationRequestsEventReceiver.VacationRequestEventReceiver";
                        erd.Type = SPEventReceiverType.ItemUpdated;
                        erd.Name = itemUpdatedName;
                        erd.Synchronization = SPEventReceiverSynchronization.Synchronous;
                        erd.SequenceNumber = 10050;
                        erd.Update();
                        vacationRequestCT.Update(true);
                    }
                }
            }
        }

        private static void RemoveItemAddingReceiver(SPFeatureReceiverProperties properties)
        {
            using (var site = properties.Feature.Parent as SPSite)
            {
                using (var web = site.RootWeb)
                {
                    var vacationRequestCT = web.ContentTypes["Vacation Request"];
                    if (vacationRequestCT != null)
                    {
                        Guid existingEventReceiverId = Guid.Empty;
                        foreach (SPEventReceiverDefinition eventReceiver in vacationRequestCT.EventReceivers)
                        {
                            if (String.Equals(eventReceiver.Name, itemAddingName))
                            {
                                existingEventReceiverId = eventReceiver.Id;
                            }
                        }
                        if (!existingEventReceiverId.Equals(Guid.Empty))
                        {
                            vacationRequestCT.EventReceivers[existingEventReceiverId].Delete();
                            vacationRequestCT.Update(true);
                        }
                    }
                }
            }
        }

        private static void RemoveItemUpdatedReceiver(SPFeatureReceiverProperties properties)
        {
            using (var site = properties.Feature.Parent as SPSite)
            {
                using (var web = site.RootWeb)
                {
                    var vacationRequestCT = web.ContentTypes["Vacation Request"];
                    if (vacationRequestCT != null)
                    {
                        Guid existingEventReceiverId = Guid.Empty;
                        foreach (SPEventReceiverDefinition eventReceiver in vacationRequestCT.EventReceivers)
                        {
                            if (String.Equals(eventReceiver.Name, itemUpdatedName))
                            {
                                existingEventReceiverId = eventReceiver.Id;
                            }
                        }
                        if (!existingEventReceiverId.Equals(Guid.Empty))
                        {
                            vacationRequestCT.EventReceivers[existingEventReceiverId].Delete();
                            vacationRequestCT.Update(true);
                        }
                    }
                }
            }
        }


        // Раскомментируйте ниже метод для обработки события, возникающего после установки компонента.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Раскомментируйте ниже метод для обработки события, возникающего перед удалением компонента.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Раскомментируйте ниже метод для обработки события, возникающего при обновлении компонента.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
