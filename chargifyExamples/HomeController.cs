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
using System.Web.Routing;

namespace TestProject.Controllers
{

    public class HomeController : Controller
    {
        private TestProjectEntities db = new TestProjectEntities();

        string charkey = "jhgashjdgasjyd";
        //LLave para la encriptacion de confimkeys   

        //Permite optener la ip publica
        private string getExternalIp()
        {
            return Request.ServerVariables["REMOTE_ADDR"];
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe(TestProject.Models.Subscribe subscribe, string confirmKey)
        {
            ChargifyConnect chargify = ChargifyTools.Chargify;
            string plan = subscribe.plan;
            bool isCredit = subscribe.hasCreditCard || subscribe.creditcard.requireCredit;//ChargifyTools.RequireCreditCard(subscribe.plan);
            try
            {
                if (ChargifyTools.IsChargifyProduct(subscribe.plan))
                {
                    ViewBag.confirmKey = confirmKey;
                    ViewBag.plan = subscribe.plan;
                    if (ValidatePassword(subscribe.password) == false)
                    {
                        //ViewBag.contactGenderId = new SelectList(db.contactGender.ToList(), "contactGenderId", "name");
                        ViewBag.Message = "Error password, you need a format that contains capital letters and numbers, example: Michael7.";
                        ViewBag.plan = subscribe.plan;

                        return View(subscribe);
                    }

                    userLogin user = new userLogin();
                    contact cont = new contact();
                    tenant tnt = new tenant();
                    //----------------------------------------------------------------------------------------------------------------
                    //----------------------------------------------------------------------------------------------------------------
                    //----------------------------------------------------------------------------------------------------------------

                    tnt.tenantSubscriptionPlanId = (from pl in db.tenantSubscriptionPlan
                                                    where pl.code.ToLower().Equals(plan.ToLower())
                                                    select pl.tenantSubscriptionPlanId).FirstOrDefault();
                    tnt.active = true;
                    tnt.allocatedUsers = 1; //cantidad de usuarios asignados
                    tnt.billingRefNumber = Guid.NewGuid().ToString();
                    tnt.companyName = subscribe.company;
                    tnt.companyURL = "N/A";
                    tnt.database = "TestProject";
                    tnt.tenantStatusId = 2;
                    tnt.tenantSourceId = 2;
                    if (isCredit)
                    {
                        tnt.tenentBillingTypeId = 1;
                    }
                    else
                    {
                        tnt.tenentBillingTypeId = 2;
                    }

                    /****** Valores quemados de campos auditoria*****/
                    tnt.updatedById = 0;
                    tnt.createdById = TntIdTestProject; // Id tenant TestProject
                    tnt.modifyDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                    tnt.insertDateTime = DateTime.Now;
                    /****** Valores quemados de campos auditoria*****/

                    db.tenant.Add(tnt);
                    db.SaveChanges();





                    var city = db.genCity
                        .Include(x => x.genState.genContry)
                        .SingleOrDefault(x => x.genCityId == Convert.ToInt32(subscribe.genCityId));
                    if (isCredit)
                    {

                        contactPhone phone = new contactPhone
                        {
                            active = true,
                            number = subscribe.phoneNumber,
                            contactId = cont.contactId,
                            contactPhoneTypeId = 1,
                            tenantId = tnt.tenantId,
                            updatedById = 0,
                            createdById = TntIdTestProject, // Id tenant TestProject
                            modifyDateTime = new DateTime(1900, 1, 1, 0, 0, 0),
                            insertDateTime = DateTime.Now
                        };

                        db.contactPhone.Add(phone);
                        db.SaveChanges();

                        cont.preferredBillAddressId = address.contactAddressId;
                        cont.preferredPhoneId = phone.contactPhoneId;
                        db.Entry(cont).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                    

                   
                    /*** cosas de chargify!!!*/
                    CustomerAttributes customerInformation = new CustomerAttributes();
                    customerInformation.FirstName = subscribe.firstName;
                    customerInformation.LastName = subscribe.lastName;
                    customerInformation.Organization = subscribe.company;
                    customerInformation.Email = subscribe.email;
                    // Create a new guid, this would be the Membership UserID if we were creating a new user simultaneously
                    customerInformation.SystemID = tnt.billingRefNumber;


                    ISubscription newSubscription = null;
                    string productHandle = plan;

                    if (isCredit)
                    {
                        CreditCardAttributes creditCardInfo = new CreditCardAttributes();

                        creditCardInfo.FullNumber = subscribe.creditcard.creditCardNumber;
                        creditCardInfo.CVV = subscribe.creditcard.cvv;
                        creditCardInfo.ExpirationMonth = subscribe.creditcard.ExpireMonth;
                        creditCardInfo.ExpirationYear = subscribe.creditcard.ExpireYear;

                        creditCardInfo.BillingAddress = subscribe.street;
                        creditCardInfo.BillingCity = city.City;//subscribe.city;
                        creditCardInfo.BillingState = city.genState.State;
                        creditCardInfo.BillingZip = subscribe.postalCode;
                        creditCardInfo.BillingCountry = city.genState.genContry.contry;

                        newSubscription = chargify.CreateSubscription(productHandle, customerInformation, creditCardInfo);
                    }
                    else
                    {
                        newSubscription = chargify.CreateSubscription(productHandle, customerInformation);
                    }                  

                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {

                return View(subscribe);
            }


            return View(subscribe);
        }

    }//Cierre HomeController
}// Cierre TestProject.Controllers
