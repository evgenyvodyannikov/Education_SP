﻿using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace FullAdvertisement.FullAdvertisementWebPart
{
    [ToolboxItemAttribute(false)]
    public class FullAdvertisementWebPart : WebPart
    {
        // Visual Studio может автоматически обновлять этот путь при изменении элемента проекта веб-части Visual.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/FullAdvertisement/FullAdvertisementWebPart/FullAdvertisementWebPartUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
