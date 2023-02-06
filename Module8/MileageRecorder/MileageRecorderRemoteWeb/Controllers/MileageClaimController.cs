using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MileageRecorderRemoteWeb.Models;
using Microsoft.SharePoint.Client;

namespace MileageRecorderRemoteWeb.Controllers
{
    public class MileageClaimController : Controller
    {

        public PartialViewResult Index()
        {
            //Obtain all the mileage claims here

            return PartialView("Index");
        }

        public ActionResult Create()
        {
            MileageClaim newClaim = new MileageClaim();
            return View("Create", newClaim);
        }

        [HttpPost]
        public ActionResult Create(MileageClaim claim)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", claim);
            }
            else
            {
                //Insert mileage claim creation code here
                
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
