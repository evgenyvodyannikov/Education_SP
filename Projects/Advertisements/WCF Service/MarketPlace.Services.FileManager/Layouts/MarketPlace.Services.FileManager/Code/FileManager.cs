using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.Office.Server.ReputationModel;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using Microsoft.Web.Hosting.Administration;
using MarketPlace.Services.FileManager.Model;
using System.IO;
using System.Runtime.Serialization;
using System.Net;

namespace MarketPlace.Services.FileManager.Code
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public sealed class FileManager : IFileManager
    {

        public ResponseJSON<List<AdvAttachment>> GetFilesByItemId(string itemId)
        {
            
            List<AdvAttachment> Images = new List<AdvAttachment>();
            ResponseJSON<List<AdvAttachment>> Response = new ResponseJSON<List<AdvAttachment>>(Images);
            SPListItem Item = null;

            using (SPSite site = new SPSite("http://sharepoint"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    try
                    {
                        ToggleUnsafeOperations(web, site, true);
                        SPList list = web.Lists["Advertisements"];
                        Item = list.Items.GetItemById(Int32.Parse(itemId));
                        SPAttachmentCollection attachments = Item.Attachments;

                        for (int i = 0; i < attachments.Count; i++)
                        {
                            string attachmentname = attachments[i];
                            string attachmentAbsoluteURL = attachments.UrlPrefix + attachmentname;
                            AdvAttachment attachmentItem = new AdvAttachment(i + 1, attachmentname, attachmentAbsoluteURL);
                            Images.Add(attachmentItem);
                        }
                        ToggleUnsafeOperations(web, site, false);
                    }
                    catch (Exception e)
                    {
                        if (Item == null)
                        {
                            string errorMessage = "There was an error: Item with id " + itemId + " not found.";
                            throw new WebFaultException<string>(errorMessage, HttpStatusCode.NotFound);
                        }
                        else
                        {
                            throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                        }
                    }

                }
            }

            return Response;
        }

        public ResponseJSON<List<AdvAttachment>> AddFiles(AdvAttachment[] Attachments)
        {
            List<AdvAttachment> ResponseData = new List<AdvAttachment>();
            ResponseJSON<List<AdvAttachment>> Response = new ResponseJSON<List<AdvAttachment>>(ResponseData);
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite("http://sharepoint"))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        try
                        {
                            ToggleUnsafeOperations(web, site, true);
                            SPList advertisementList = web.Lists["Advertisements"];
                            SPListItem item = advertisementList.GetItemById(Attachments[0].AdvertisementId);
                            int Index = item.Attachments.Count;

                            foreach (AdvAttachment attachment in Attachments)
                            {
                                byte[] fileContent = Base64Decode(attachment.Data);
                                item.Attachments.Add(attachment.Name, fileContent);
                                string attachmentAbsoluteURL = item.Attachments.UrlPrefix + attachment.Name;

                                Index += 1;
                                attachment.Id = Index;
                                attachment.Data = "Parsed";
                                attachment.Url = attachmentAbsoluteURL;
                                ResponseData.Add(attachment);
                            }

                            item.Update();
                            ToggleUnsafeOperations(web, site, false);
                        }

                        catch (InvalidCastException e)
                        {
                            throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                        }
                    }

                }

            });

            return Response;
        }

        public ResponseJSON<Advertisement> CreateAdvertisement(Advertisement DataJson)
        {
            Advertisement AdvertisementData = new Advertisement(DataJson);
            ResponseJSON<Advertisement> Response = new ResponseJSON<Advertisement>(AdvertisementData);
            SPListItem category = null;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite("http://sharepoint"))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        try
                        {
                            ToggleUnsafeOperations(web, site, true);
                            SPList advertisementList = web.Lists["Advertisements"];
                            SPListItem item = advertisementList.AddItem();
                            SPUserCollection Users = web.AllUsers;
                            SPUser Author = Users.GetByID(DataJson.AuthorId);

                            item["Title"] = DataJson.Title;
                            item["Description"] = DataJson.Description;
                            item["Status"] = DataJson.Status;
                            item["Author"] = Author;
                            item["Created"] = DateTime.Now;

                            SPList categoryList = web.Lists["Categories"];
                            category = categoryList.GetItemById(DataJson.CategoryId);

                            item["Category"] = new SPFieldLookupValue(category.ID, category.Title);
                            item.Update();

                            AdvertisementData.Id = item.ID;

                            if (DataJson.Attachments.Length > 0)
                            {
                                int index = 0;

                                foreach (AdvAttachment attachment in DataJson.Attachments)
                                {
                                    try
                                    {
                                        byte[] fileContent = Base64Decode(attachment.Data);
                                        item.Attachments.Add(attachment.Name, fileContent);

                                        string attachmentAbsoluteURL = item.Attachments.UrlPrefix + attachment.Name;
                                        index += 1;

                                        attachment.Id = index;
                                        attachment.AdvertisementId = item.ID;
                                        attachment.Data = "Parsed";
                                        attachment.Url = attachmentAbsoluteURL;
                                    }
                                    catch (Exception e)
                                    {
                                        throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                                    }
                                }
                            }

                            item.Update();
                            ToggleUnsafeOperations(web, site, false); 
                        }

                        catch (Exception e)
                        {
                            if (category == null)
                            {
                                string errorMessage = "Category with Id " + DataJson.CategoryId + " not found. Item cannot be created.";
                                throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                            }
                            throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                        }
                    }
                }
            });

            return Response;
        }

        public ResponseJSON<Advertisement> EditAdvertisement(Advertisement DataJson)
        {
            ResponseJSON<Advertisement> Response = new ResponseJSON<Advertisement>(DataJson);
            SPListItem category = null;
            SPListItem item = null;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite("http://sharepoint"))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        try
                        {
                            ToggleUnsafeOperations(web, site, true);
                            SPList advertisementList = web.Lists["Advertisements"];
                            item = advertisementList.GetItemById(DataJson.Id);

                            item["Title"] = DataJson.Title;
                            item["Description"] = DataJson.Description;
                            item["Status"] = DataJson.Status;

                            SPList categoryList = web.Lists["Categories"];
                            category = categoryList.GetItemById(DataJson.CategoryId);

                            item["Category"] = new SPFieldLookupValue(category.ID, category.Title);
                            item.Update();

                            if (DataJson.Attachments.Length > 0)
                            {
                                int index = item.Attachments.Count;

                                foreach (AdvAttachment attachment in DataJson.Attachments)
                                {
                                    try
                                    {
                                        byte[] fileContent = Base64Decode(attachment.Data);
                                        item.Attachments.Add(attachment.Name, fileContent);

                                        string attachmentAbsoluteURL = item.Attachments.UrlPrefix + attachment.Name;
                                        index += 1;

                                        attachment.Id = index;
                                        attachment.AdvertisementId = item.ID;
                                        attachment.Data = "Parsed";
                                        attachment.Url = attachmentAbsoluteURL;
                                    }
                                    catch (Exception e)
                                    {
                                        throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                                    }
                                }
                            }

                            item.Update();
                            ToggleUnsafeOperations(web, site, false);
                        }

                        catch (Exception e)
                        {
                            if (category == null)
                            {
                                string errorMessage = "Category with Id " + DataJson.CategoryId + " not found. Item cannot be edited.";
                                throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                            }
                            else if (item == null)
                            {
                                string errorMessage = "Item with Id " + DataJson.CategoryId + " not found.";
                                throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                            }
                            throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                        }
                    }
                }
            });

            return Response;

        }

        public void DeleteFiles(AdvAttachment[] attachments)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite("http://sharepoint"))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        try
                        {
                            ToggleUnsafeOperations(web, site, true);
                            SPList list = web.Lists["Advertisements"];
                            SPListItem item = list.GetItemById(attachments[0].AdvertisementId);
                            SPAttachmentCollection ExistingAttachments = item.Attachments;

                            for (int i = 0; i < ExistingAttachments.Count; i++)
                            {
                                string attachmentName = ExistingAttachments[i];
                                if (attachments.Where(x => x.Name == attachmentName).ToArray().Length > 0)
                                {
                                    ExistingAttachments.DeleteNow(attachmentName);
                                }
                            }
                            item.Update();
                            ToggleUnsafeOperations(web, site, false);
                        }
                        catch (InvalidCastException e)
                        {
                            throw new WebFaultException<string>(e.ToString(), HttpStatusCode.InternalServerError);
                        }
                    }
                }
            });

        }

        private static byte[] Base64Decode(string base64EncodedData)
        {
            return Convert.FromBase64String(base64EncodedData);
        }

        private void ToggleUnsafeOperations (SPWeb web, SPSite site, bool flag)
        {
            web.AllowUnsafeUpdates = flag;
            site.AllowUnsafeUpdates = flag;
        }

    }

}