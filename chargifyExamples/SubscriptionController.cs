using STestProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PWDTK_DOTNET451;
using TestProject.Validations;
using ChargifyNET;
using System.Data;
using System.Net;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Security;
using TestProject.clases;
using TestProject.TestProjectSecurity;
using System.Text;
using System.Data.Entity.Validation;


namespace TestProject.Controllers
{
    [AllowAnonymous]
    public class SubscriptionController : Controller
    {
        private TestProjectEntities db = new TestProjectEntities();
        // GET: Subscription

        public ActionResult ReactivationInstructions()
        {
            tenant tenant = (tenant)TempData["tenant"];
            ViewBag.Message = (string)TempData["Message"];
            bool hasPaymentInfo = false;
            if (tenant == null)
            {
                //TempData["Message"] = "Your user is not a customer";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            ChargifyConnect chargify = ChargifyTools.Chargify;
            ISubscription subscription = ChargifyTools.getSubscription(tenant.billingRefNumber);
            if (subscription == null)
            {
                TempData["Message"] = "Your user is not a customer";
                return RedirectToAction("Login", "Home", new { area = "" });
            }


            if (subscription.PaymentProfile != null)
            {
                hasPaymentInfo = true;
            }
            ViewBag.hasPaymentInfo = hasPaymentInfo;
            ViewBag.tenantId = tenant.tenantId;
            ViewBag.paymentUrl = chargify.GetPrettySubscriptionUpdateURL("", "", subscription.SubscriptionID);
            return View();
        }
        [HttpPost]
        public ActionResult Reactivation(int tenantId)
        {
            if (tenantId <= 0)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            tenant tenant = db.tenant.Find(tenantId);

            if (tenant == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            ChargifyConnect chargify = ChargifyTools.Chargify;
            ISubscription currentSubscription = ChargifyTools.getSubscription(tenant.billingRefNumber);
            if (currentSubscription == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            if (currentSubscription.PaymentProfile != null)
            {
                try
                {
                    ISubscription reactivatedSubscription = chargify.ReactivateSubscription(currentSubscription.SubscriptionID);
                    if (reactivatedSubscription.State == SubscriptionState.Active)
                    {
                        TempData["Message"] = "Your subscription has been successfully reactivated.\n please login again";
                        return RedirectToAction("Login", "Home", new { area = "" });
                    }
                    else
                    {
                        TempData["Message"] = "Your subscription couldn't be activated.\n Follow the instructions and try again";
                        TempData["tenant"] = tenant;
                        //return RedirectToAction("Login", "Home", new { area = "" });
                        return RedirectToAction("ReactivationInstructions");
                    }
                }
                catch (Exception)
                {

                    TempData["Message"] = "Your subscription couldn't be activated.\n Follow the instructions and try again";
                    TempData["tenant"] = tenant;
                    //return RedirectToAction("Login", "Home", new { area = "" });
                    return RedirectToAction("ReactivationInstructions");
                }
                

            }
            else
            {
                TempData["Message"] = "You haven't set your payment information.\n Follow the instructions and try again";
                TempData["tenant"] = tenant;
                return RedirectToAction("ReactivationInstructions", tenant);
            }

            return View();
        }
    }
}