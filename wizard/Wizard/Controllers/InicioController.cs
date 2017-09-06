using ChargifyNET;
using SQRely_Admin.Areas.Wizard.Models;
using SQRely_Admin.clases;
using SQRely_Admin.Models;
using SQRely_Admin.SqrelySecurity;
using SQRely_Admin.Validations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQRely_Admin.Areas.Wizard.Controllers
{
    public class InicioController : Controller
    {
        private SQRelyEntities db = new SQRelyEntities();
        // GET: Wizard/Inicio
        string EmailCompany = "team@viasql.com";
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Savewizard(TranWizardConfigJSON data)
        {
            ResponseStatusJSON status = new ResponseStatusJSON();
            int tenantconfigs = clsCommon.getIdentity().tenatId;
            if (data != null)
            {
                List<genConfig> wizzardconfigs = new List<genConfig>();

                foreach (var item in data.tenantconfigs)
                {
                    if (ifExistConfig(item))
                    {

                    }
                    else
                    {
                        wizzardconfigs.Add(BulkConfigs(item));
                    }

                }
                foreach (var item in data.settings)
                {
                    if (ifExistConfig(item))
                    {

                    }
                    else
                    {
                        if (item.parameter != null)
                        {
                            wizzardconfigs.Add(BulkConfigs(item));
                        }
                    }

                }
                try
                {
                    db.genConfig.AddRange(wizzardconfigs);
                    db.SaveChanges();

                    if (data.usuarios != null)
                    {
                        foreach (var user in data.usuarios)
                        {
                            contact contacto = new contact();
                            contacto.name = user.username;
                            contacto.lastName = user.userlastname;
                            contacto.email = user.email;
                            contacto.contactGenderId = 1;
                            contacto.active = true;

                            /****** Valores quemados de campos auditoria*****/
                            contacto.tenantId = clsCommon.getIdentity().tenatId;
                            contacto.updatedById = 0;//por defecto o null
                            contacto.createdById = clsCommon.getIdentity().Id;
                            contacto.modifyDateTime = new DateTime(1900, 1, 1, 0, 0, 0);//por defecto o null
                            contacto.insertDateTime = DateTime.Now;
                            /****** Valores quemados de campos auditoria*****/
                            db.contact.Add(contacto);
                            db.SaveChanges();

                            employee empleado = new employee();
                            empleado.contactId = contacto.contactId;
                            empleado.active = true;
                            empleado.employeeTypeId = 1; //tipo de empleado

                            /****** Valores quemados de campos auditoria*****/
                            empleado.tenantId = clsCommon.getIdentity().tenatId;
                            empleado.updatedById = 0;//por defecto o null
                            empleado.createdById = clsCommon.getIdentity().Id;
                            empleado.modifyDateTime = new DateTime(1900, 1, 1, 0, 0, 0);//por defecto o null
                            empleado.insertDateTime = DateTime.Now;
                            /****** Valores quemados de campos auditoria*****/
                            db.employee.Add(empleado);
                            db.SaveChanges();

                            userLogin usuario = new userLogin();
                            usuario.userName = user.alias;
                            usuario.tenantId = clsCommon.getIdentity().tenatId;
                            usuario.active = true;
                            usuario.secretQuestion = "N/A";
                            usuario.secretAnswer = Guid.NewGuid().ToString();
                            usuario.confirmKey = usuario.secretAnswer;
                            usuario.email = user.email;
                            usuario.password = user.password;
                            usuario.lastLoginDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                            usuario.lastPasswordReset = new DateTime(1900, 1, 1, 0, 0, 0);
                            usuario.contactId = contacto.contactId;
                            usuario.userLoginTypeId = 4; //customer

                            /****** Valores quemados de campos auditoria*****/
                            usuario.tenantId = clsCommon.getIdentity().tenatId;
                            usuario.updatedById = 0;//por defecto o null
                            usuario.createdById = clsCommon.getIdentity().Id;
                            usuario.modifyDateTime = new DateTime(1900, 1, 1, 0, 0, 0);//por defecto o null
                            usuario.insertDateTime = DateTime.Now;
                            /****** Valores quemados de campos auditoria*****/
                            clsCommon.CreateUser(usuario);

                            db.sp_Register_Owner(user.rol.id, usuario.tenantId, usuario.userLoginId, usuario.createdById);

                            string subject = "Welcome to Sqrely";
                            //string body = "Welcome To SQRely!  \n\n" +
                            //    "Thanks For signing up.  \n\n " +
                            //    "User: " + user.username + "\n" +
                            //    "Password: " + user.password + "\n\n" +
                            //    "If clicking this button doesn't work, please copy and paste the URL below:  \n\n" +
                            //   Url.Action("Login", "Home", null, Request.Url.Scheme) + "\n\n";


                            string body = @"<table width='100%' border='0' cellspacing='0' cellpadding='0'> " +
                                        @"<tr> " +
                                            @"<td align='center'> " +
                                                @"<img src='cid:head' width='100%' /> <br/>" +
                                                @"<img src='cid:welcome' width='100%' /> <br/>" +
                                            @"</td> " +
                                        @"</tr> " +
                                    @"</table> <br/>" +
                "<span style='font-size:20px; color:#1b75bb'>Welcome to Sqrely!</span>  <br/>" + "<br/><br/>" +
                @"<span style='font-size:20px'>Thanks For signing up</span><br /><br /><br />" +
                @"<span style='font-size:16px'>User: " + user.alias + "</span><br/>" +
                @"<span style='font-size:16px'>User: " + user.password + "</span><br/>" +
                @"<span style='font-size:16px'>If clicking this button doesn't work, please copy and paste the URL below:</span><br/>" +
                Url.Action("Login", "Home", null, Request.Url.Scheme)+
                @"<br/><div style='height: 200px; width: 100 %; '></div>" +
                @"<br/><img src='cid:foot' width='100%' />";







                //            string body = "<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                //                "<tr>" +
                //                    "<td align='center'>" +
                //                    @"<img src='cid:logo' width='137px' />" +
                //                    "</td>" +
                //                "</tr>" +
                //            "</table>" +
                //@"<span style='font-size:20px; color:#1b75bb'>¡Welcome to the Sqrely family!</span><br />" +
                //@"<span style='font-size:20px'>Thanks For signing up</span><br /><br /><br />" +
                //@"<span style='font-size:16px'>User: " + user.alias + "</span><br/>" +
                //@"<span style='font-size:16px'>User: " + user.password + "</span><br/>" +
                //@"<span style='font-size:16px'>If clicking this button doesn't work, please copy and paste the URL below:</span><br/>" +
                //Url.Action("Login", "Home", null, Request.Url.Scheme);

                            List<string> filepath = new List<string>();
                            List<string> cid = new List<string>();
                            filepath.Add("~/Content/img/Head-34.png");
                            cid.Add("head");
                            filepath.Add("~/Content/img/Welcome.png");
                            cid.Add("welcome");
                            filepath.Add("~/Content/img/Foot-34.png");
                            cid.Add("foot");

                            //'/***** Sirver para mandar email con estructura html al correo ********/
                            //clsCommon.SendEmail(EmailCompany, user.email, subject, body);
                            clsCommon.SendEmail(EmailCompany, user.email, subject, body, filepath, cid, true);
                            //clsCommon.SendEmail(EmailCompany, user.email, subject, body, "~/Content/img/emailLogo.png", "logo", true);
                        }
                    }
                    int totalusers = Convert.ToInt32(data.numberusers);
                    AddusersBilling(totalusers);
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


            }
            status.status = StatusJson.OK;
            status.message = "Wizard Complete";
            status.redirectUrl = "Dashboard/Dashboard";
            var result = Json(status);
            var Cookie = new HttpCookie("Wizflag" + tenantconfigs.ToString(), "true");
            Session["Wizard"] = false;
            HttpContext.Response.Cookies.Set(Cookie);
            return result;
        }

        public JsonResult GetInfoClient()
        {
            var result = Json("", JsonRequestBehavior.AllowGet);
            int currentuser = clsCommon.getIdentity().Id;
            int tenantid = clsCommon.getIdentity().tenatId;
            tenant ctenant = (from t in db.tenant where t.tenantId == tenantid select t).FirstOrDefault();
            ChargifyConnect chargify = clsCommon.Chargify;
            int usercount = 1;
            bool validpaid = false;
            ISubscription currentSubscription = ChargifyTools.getSubscription(ctenant.billingRefNumber);
            if (currentSubscription != null)
            {
                int componentId = clsCommon.getComponentId(currentSubscription.Product.Handle);
                if (currentSubscription.State == SubscriptionState.Active)
                {
                    if (componentId > 0)
                    {
                        IComponentAttributes newInfo = chargify.GetComponentInfoForSubscription(currentSubscription.SubscriptionID, componentId);
                        usercount = Convert.ToInt32(newInfo.AllocatedQuantity + 1);
                    }
                }
                validpaid = true;
            }
            List<userLogin> users = (from u in db.userLogin where u.tenantId == tenantid && u.active == true select u).ToList();
            var tenantinfo = (from obj in db.vw_Tenant
                              where obj.tenantId == tenantid
                              select new
                              {
                                  paid = validpaid,
                                  Companyname = obj.companyName,
                                  AllocatedUsers = obj.allocatedUsers,
                                  address = obj.Address,
                                  owneremail = obj.email,
                                  codigoarea = obj.countryCode,
                                  phone = obj.number,
                                  ownerurl = obj.companyURL,
                                  //userscount = (from u in db.userLogin
                                  //              join ct in db.contact
                                  //              on u.contactId
                                  //              equals ct.contactId
                                  //              where u.tenantId == tenantid && u.active == true
                                  //              select new
                                  //              {
                                  //                  name = ct.name,
                                  //                  alias = u.userName,
                                  //                  email = ct.email
                                  //              }).ToList(),
                                  tenantconfigs = (from obj2 in db.genConfigName
                                                   where obj2.genConfigNameGroupId == 1 && obj2.active == true
                                                   orderby obj2.code ascending
                                                   select new
                                                   {
                                                       configNameid = obj2.genConfigNameId,
                                                       parameter = "",
                                                       active = obj2.active,
                                                       code = obj2.code
                                                   }).ToList(),
                                  settconfigs = (from obj3 in db.genConfigName
                                                 where obj3.genConfigNameGroupId == 2 && obj3.active == true
                                                 orderby obj3.code ascending
                                                 select new
                                                 {
                                                     configNameid = obj3.genConfigNameId,
                                                     parameter = "",
                                                     active = obj3.active,
                                                     code = obj3.code
                                                 }).ToList(),
                                  languanges = (from obj4 in db.genLanguage select new { obj4.name }).ToList(),
                                  currencies = (from obj5 in db.genCurrency select new { obj5.name, code = obj5.code }).ToList(),
                                  regions = (from obj6 in db.genRegion select new { obj6.name }).ToList(),
                                  Timezones = (from obj7 in db.genTimezone select new { obj7.timeZone }).ToList(),
                                  defaultconfigs = (from dcn in db.genConfigName
                                                    join dc in db.genConfig
                                                    on dcn.genConfigNameId equals dc.genConfigNameId
                                                    where dc.tenantId == tenantid && dcn.genConfigNameGroupId == 1 && dcn.active == true
                                                    orderby dcn.code ascending
                                                    select new
                                                    {
                                                        dcn.code,
                                                        dc.parameter
                                                    }).ToList(),
                                  settdefaultconfigs = (from dcn in db.genConfigName
                                                    join dc in db.genConfig
                                                    on dcn.genConfigNameId equals dc.genConfigNameId
                                                    where dc.tenantId == tenantid && dcn.genConfigNameGroupId == 2 && dcn.active == true
                                                    orderby dcn.code ascending
                                                    select new
                                                    {
                                                        dcn.code,
                                                        dc.parameter
                                                    }).ToList(),
                                  userroles = (from urol in db.userLoginRole where (urol.userLoginRoleId != 1 && urol.userLoginRoleId != 2) select new { name = urol.name, id = urol.userLoginRoleId }).ToList()
                              }).FirstOrDefault();

            if (tenantinfo != null)
            {
                result = Json(tenantinfo, JsonRequestBehavior.AllowGet);
            }

            return result;
        }

        public JsonResult GetLogoCompany()
        {
            var result = Json("", JsonRequestBehavior.AllowGet);
            int tenantid = clsCommon.getIdentity().tenatId;
            var CurrentCompanyLogo = (from cl in db.genConfig
                                      join cn in db.genConfigName
                                      on cl.genConfigNameId equals cn.genConfigNameId
                                      where cl.tenantId == tenantid && cn.code == "CCI03"
                                      select new { image = cl.parameter }).FirstOrDefault();
            if (CurrentCompanyLogo != null)
            {
                result = Json(CurrentCompanyLogo, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        //public JsonResult Ifuserexist()
        //{
        //    var existuser = Json("", JsonRequestBehavior.AllowGet);

        //    return existuser;
        //}
        public int Ifuserexist(string username)
        {
            int existuser = 0;
            var result = (from users in db.userLogin where users.userName == username select users).FirstOrDefault();
            if (result != null)
            {
                existuser = 1;
            }
            return existuser;
        }
        public int Ifemailexist(string email)
        {
            int existemail = 0;
            var result = (from users in db.userLogin where users.email == email select users).FirstOrDefault();
            if (result != null)
            {
                existemail = 1;
            }

            return existemail;
        }


        public ActionResult ShowWiz()
        {
            return PartialView("Wizard");
        }

        public genConfig BulkConfigs(ConfiguracionesJSON item)
        {
            genConfig config = new genConfig();

            config.parameter = item.parameter;
            config.active = true;
            config.genConfigNameId = item.configNameid;
            /****** Valores quemados de campos auditoria*****/
            config.tenantId = clsCommon.getIdentity().tenatId;
            config.updatedById = 0;//por defecto o null
            config.createdById = clsCommon.getIdentity().Id;
            config.modifyDateTime = new DateTime(1900, 1, 1, 0, 0, 0);//por defecto o null
            config.insertDateTime = DateTime.Now;
            ///****** Valores quemados de campos auditoria*****/           

            return config;

        }

        public bool ifExistConfig(ConfiguracionesJSON item)
        {
            int tn = clsCommon.getIdentity().tenatId;
            genConfig ifexistconfig = (from c in db.genConfig where c.genConfigNameId == item.configNameid && c.tenantId == tn select c).FirstOrDefault();
            if (ifexistconfig != null)
            {
                ifexistconfig.parameter = item.parameter;
                ifexistconfig.updatedById = clsCommon.getIdentity().Id;
                ifexistconfig.modifyDateTime = new DateTime(1900, 1, 1, 0, 0, 0);//por defecto o null
                db.Entry(ifexistconfig).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public void AddusersBilling(int count)
        {
            Sqrely_Identity identity = clsCommon.getIdentity();

            if (count > 0)
            {
                try
                {
                    ChargifyConnect Chargify = clsCommon.Chargify;
                    // Load the customer information for the current user
                    //ICustomer customer = Chargify.Find<Customer>(clsCommon.getCurrentUserBillingRefNumber());
                    // Alternate syntax
                    // Dim customer As ICustomer = Chargify.LoadCustomer(clsCommon.getCurrentUserBillingRefNumber())
                    tenant currentenant = db.tenant.Find(identity.tenatId);
                    ISubscription currentSubscription = ChargifyTools.getSubscription(currentenant.billingRefNumber);
                    if (currentSubscription != null)
                    {
                       

                        int componentID = clsCommon.getComponentId(currentSubscription.Product.Handle);
                        if (componentID > 0)
                        {
                            //Update the amount allocated
                            //Dim info As IComponentAttributes = Chargify.UpdateComponentAllocationForSubscription(currentSubscription.SubscriptionID, componentID, (CInt(noUsersTb.Text) - 1))
                            //Dim info As IComponentAttributes = Chargify.UpdateComponentAllocationForSubscription(currentSubscription.SubscriptionID, componentID, newQuantity)
                            //Chargify.CreateComponentAllocation(currentSubscription.SubscriptionID, componentID, count, "New Users", ComponentUpgradeProrationScheme.No_Prorate, ComponentDowngradeProrationScheme.No_Prorate);
                            if (currentSubscription.State == SubscriptionState.Trialing)
                            {
                                Chargify.CreateComponentAllocation(currentSubscription.SubscriptionID, componentID, count, "New Users", ComponentUpgradeProrationScheme.No_Prorate, ComponentDowngradeProrationScheme.No_Prorate);

                            }
                            else
                            {
                                Chargify.CreateComponentAllocation(currentSubscription.SubscriptionID, componentID, count, "New Users", ComponentUpgradeProrationScheme.Prorate_Attempt_Capture, ComponentDowngradeProrationScheme.Prorate);

                            }
                        }
                        //Dim sqltext As String = "UPDATE tenant SET modifyDateTime=getdate(), " &
                        //" allocatedUsers = @allocatedUsers " &
                        //" WHERE billingRefNumber = @billingRefNumber"
                        //ApplicationBlocks.SqlHelper.ExecuteNonQuery(clsCommon.connString,
                        //                                        System.Data.CommandType.Text, sqltext,
                        //                                    New System.Data.SqlClient.SqlParameter("@allocatedUsers", noUsersTb.Text),
                        //                                    New System.Data.SqlClient.SqlParameter("@billingRefNumber", currentSubscription.Customer.SystemID))
                        //tenant tenant = default(tenant);
                        //tenant = (from t in db.tenantwhere t.billingRefNumber.Equals(currentSubscription.Customer.SystemID)).FirstOrDefault();

                        currentenant.allocatedUsers = count + 1;
                        //Dim identity As SQRely_Admin.SqrelySecurity.Sqrely_Identity
                        //identity = HttpContext.Current.User.Identity
                        currentenant.updatedById = identity.Id;
                        currentenant.modifyDateTime = DateTime.Now;
                        db.Entry(currentenant).State = EntityState.Modified;
                        db.SaveChanges();
                        //LoadSubscriptionInfo();
                        //usageResultLtr.Text = string.Format("Current Allocated users ({0}).", noUsersTb.Text);
                        //If newAddedUsers > 0 Then
                        //    ' Charge Existing Users on new account
                        //    Dim amount As Decimal = newAddedUsers * currentSubscription.ProductPrice
                        //    Chargify.CreateCharge(currentSubscription.SubscriptionID, amount, String.Format("New Users Added: {0}", newAddedUsers.ToString()), True)
                        //End If

                        //' Get the amount allocated
                        //Dim newInfo As IComponentAttributes = Chargify.GetComponentInfoForSubscription(currentSubscription.SubscriptionID, componentID)

                        //If newInfo IsNot Nothing Then
                        //    ' Charged OK
                        //    Me.usageResultLtr.Text = String.Format("Current additional users ({0}).", noUsersTb.Text)
                        //    ScriptManager.RegisterStartupScript(Me.Page, GetType(Page), "highlight", "HighlightResult();", True)
                        //Else
                        //    ' Not OK
                        //    Me.usageResultLtr.Text = String.Format("Error. ({0})", DateTime.Now.ToString())
                        //    ScriptManager.RegisterStartupScript(Me.Page, GetType(Page), "highlight", "HighlightResult();", True)
                        //End If
                    }
                }
                catch (Exception ex)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "error", "alert('" + ex.Message + "');", true);
                }
            }
        }
    }
}