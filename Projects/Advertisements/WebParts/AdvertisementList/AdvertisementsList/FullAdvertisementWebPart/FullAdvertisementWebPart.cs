﻿using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AdvertisementsList.FullAdvertisements
{
    [ToolboxItemAttribute(false)]
    public class FullAdvertisement : WebPart
    {
        // Visual Studio может автоматически обновлять этот путь при изменении элемента проекта веб-части Visual.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/AdvertisementsList/FullAdvertisements/FullAdvertisementsUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
