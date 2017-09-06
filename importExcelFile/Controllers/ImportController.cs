using DevExpress.Web.Mvc;
using TestProject.Areas.Import.Models;
using TestProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
 
namespace TestProject.Areas.Import.Controllers
{
    public class ImportController : Controller
    {
        TestProjectEntities db = new TestProjectEntities();
        public ActionResult importTable()
        {

            return View();
        }
        public ActionResult Index()
        {
            //if (Session["DataTableModel"] == null)
            //    Session["DataTableModel"] = InMemoryModel.OpenExcelFile();

            //return View(Session["DataTableModel"]);
            return View();
        }

        [HttpPost]

        public ActionResult Index(HttpPostedFileBase file)
        {
            DataSet ds = new DataSet();
            IEnumerable<importTable> list1;
            string fileLocation = "";
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {

                    fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();
                    DataTable data = new DataTable();
                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    excelConnection.Close();
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    // string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    string query = "Select * from [Sheet1$]";
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(data);// dataAdapter.Fill(ds);
                    }
                    //excelConnection.Close();
                    Session["DataTableModel"] = data;


                    //list1 = BindData.DataTableToList<importTable>(data);

                    //List<importTable> list = binder.BindDataList<importTable>(data);
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }
                //if (System.IO.File.Exists(fileLocation))
                //{
                //    System.IO.File.Delete(fileLocation);
                //}

                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    string conn = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
                //    SqlConnection con = new SqlConnection(conn);
                //    string query = "Insert into Person(Name,Email,Mobile) Values('" +
                //    ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() +
                //    "','" + ds.Tables[0].Rows[i][2].ToString() + "')";
                //    con.Open();
                //    SqlCommand cmd = new SqlCommand(query, con);
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //}

            }
            //return View();
            return View("ViewData");
        }

        public ActionResult GridViewPartial()
        {
            DataTable dt = (DataTable)Session["DataTableModel"];
            return PartialView("_GridViewPartial", dt);
        }

        // public ActionResult SaveData()
        // {
        //     DataTable dt = (DataTable)Session["DataTableModel"];
        //     if (dt != null)
        //     {


        //         //DataTableBinder binder = new DataTableBinder();
        //         List<importTable> list = new List<TestProject.Models.importTable>(); //binder.BindDataList<importTable>(dt);
        //         //list= AutoMapper.Mapper.DynamicMap<IDataReader, List<importTable>>(dt.CreateDataReader());
        //         //BindDataList<Foo>(dt);
        //         importTable acc = null;
        //         foreach (DataRow row in dt.Rows)
        //         {
        //             acc = new importTable();
        //             //acc.importTableId = Convert.ToInt32(row["importTableId"].ToString());
        //             //acc.accountBusinessType
        //             acc.importTableId = Convert.ToInt32(row["importTableId"].ToString());
        //             acc.Id = row["Id"].ToString();
        //             acc.accountType = row["accountType"].ToString();
        //             acc.accountSalesRepId = row["accountSalesRepId"].ToString();
        //             acc.accountSalesRepname = row["accountSalesRepname"].ToString();
        //             acc.accountTerritory = row["accountTerritory"].ToString();
        //             acc.accountFrequency = row["accountFrequency"].ToString();
        //             acc.accountBusinessType = row["accountBusinessType"].ToString();
        //             acc.accountStatus = row["accountStatus"].ToString();
        //             acc.accountSource = row["accountSource"].ToString();
        //             acc.accountNumber = row["accountNumber"].ToString();
        //             acc.accountTermId = row["accountTermId"].ToString();
        //             acc.accountTermName = row["accountTermName"].ToString();
        //             acc.accountTermDays = row["accountTermDays"].ToString();
        //             acc.accountPaymentTypeId = row["accountPaymentTypeId"].ToString();
        //             acc.accountPaymentTypeName = row["accountPaymentTypeName"].ToString();
        //             acc.refNumber = row["refNumber"].ToString();
        //             acc.companyName = row["companyName"].ToString();
        //             acc.companyName2 = row["companyName2"].ToString();
        //             acc.accountContact = row["accountContact"].ToString();
        //             acc.accountContactTitle = row["accountContactTitle"].ToString();
        //             acc.accountContactDept = row["accountContactDept"].ToString();
        //             acc.addressLine = row["addressLine"].ToString();
        //             acc.addressLine2 = row["addressLine2"].ToString();
        //             acc.addressLine3 = row["addressLine3"].ToString();
        //             acc.city = row["city"].ToString();
        //             acc.state = row["state"].ToString();
        //             acc.zipCode = row["zipCode"].ToString();
        //             acc.country = row["country"].ToString();
        //             acc.longitude = Convert.ToDouble(row["longitude"].ToString());
        //             acc.latitude = Convert.ToDouble(row["latitude"].ToString());
        //             acc.mainPhone = row["mainPhone"].ToString();
        //             acc.fax = row["fax"].ToString();
        //             acc.mobile = row["mobile"].ToString();
        //             acc.otherPhone = row["otherPhone"].ToString();
        //             acc.email = row["email"].ToString();
        //             acc.emailAlt = row["emailAlt"].ToString();
        //             acc.website = row["website"].ToString();
        //             acc.creditLimit = Convert.ToDecimal(row["creditLimit"].ToString());
        //             acc.balance = Convert.ToDecimal(row["balance"].ToString());
        //             acc.parentId = row["parentId"].ToString();
        //             acc.parentAccountNumber = row["parentAccountNumber"].ToString();
        //             acc.priceLevelId = row["priceLevelId"].ToString();
        //             acc.priceLevelName = row["priceLevelName"].ToString();
        //             acc.defaultWarehouse = row["defaultWarehouse"].ToString();
        //             var tx1 = row["tax1Exempt"];
        //             string txstr1 = row["tax1Exempt"].ToString();
        //             acc.tax1Exempt = Convert.ToBoolean(row["tax1Exempt"].ToString().ToLower());
        //             acc.tax2Exempt = Convert.ToBoolean(row["tax2Exempt"].ToString().ToLower());
        //             acc.taxCode = row["taxCode"].ToString();
        //             acc.currency = row["currency"].ToString();
        //             acc.notes = row["notes"].ToString();
        //             acc.misc1 = row["misc1"].ToString();
        //             acc.misc2 = row["misc2"].ToString();
        //             acc.misc3 = row["misc3"].ToString();
        //             acc.misc4 = row["misc4"].ToString();
        //             acc.misc5 = row["misc5"].ToString();
        //             acc.misc6 = row["misc6"].ToString();
        //             acc.misc7 = row["misc7"].ToString();
        //             acc.misc8 = row["misc8"].ToString();
        //             acc.misc9 = row["misc9"].ToString();
        //             acc.misc10 = row["misc10"].ToString();
        //             acc.misc11 = row["misc11"].ToString();
        //             acc.misc12 = row["misc12"].ToString();
        //             acc.timeModified = Convert.ToDateTime(row["timeModified"].ToString());
        //             acc.timeCreated = Convert.ToDateTime(row["timeCreated"].ToString());
        //             acc.genERPId = Convert.ToInt32(row["genERPId"].ToString());
        //             acc.impStatusId = Convert.ToInt32(row["impStatusId"].ToString());
        //             acc.impStatusLog = row["impStatusLog"].ToString();
        //             acc.active = Convert.ToBoolean(row["active"].ToString().ToLower());
        //             acc.toDeactivate = Convert.ToBoolean(row["toDeactivate"].ToString());
        //             acc.accountId = Convert.ToInt32(row["accountId"].ToString());
        //             acc.tenantId = Convert.ToInt32(row["tenantId"].ToString());
        //             acc.updatedById = Convert.ToInt32(row["updatedById"].ToString());
        //             acc.createdById = Convert.ToInt32(row["createdById"].ToString());
        //             acc.modifyDateTime = Convert.ToDateTime(row["modifyDateTime"].ToString());
        //             acc.insertDateTime = Convert.ToDateTime(row["insertDateTime"].ToString());

        //             list.Add(acc);
        //         }


        //         db.importTable.AddRange(list);
        //         db.SaveChanges();




        //         //Session["DataTableModel"] = null;

        //     }
        //     else
        //     {
        //         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //     }

        //     return View();
        // }
        public ActionResult chooseFields()
        {
            DataTable dt = (DataTable)Session["DataTableModel"];
            // List<string> columnas = new List<string>();
            List<SelectListItem> columns = new List<SelectListItem>();
            //{
            //    new SelectListItem { Text = "Homeowner", Value = ((int)UserType.Homeowner).ToString()},
            //    new SelectListItem { Text = "Contractor", Value = ((int)UserType.Contractor).ToString()},
            //};
            Type tipo = typeof(importTable);
            List<string> requeridos = new List<string>(new string[] {
                                                                        "genERPId",
                                                                        "impStatusId",
                                                                        "tenantId",
                                                                        "createdById",
                                                                        "insertDateTime"
                                                                    });

            FieldListVM fields = new FieldListVM();
            fields.fieldList = new List<FieldsVM>();
            List<PropertyInfo> requiredList = new List<PropertyInfo>(tipo.GetProperties().Where(
                prop => Attribute.IsDefined(
                    prop, typeof(RequiredAttribute)
                    )));
            //foreach (var prop in tipo.GetProperties())
            //{
            //    FieldsVM field = new FieldsVM();
            //    field.field = prop.Name;
            //    field.column = prop.Name;
            //    fields.fieldList.Add(field);
            //    props[].Name;
            //}
            var propNames = tipo.GetProperties();
            for (int i = 0; i < tipo.GetProperties().Count(); i++)
            {
                FieldsVM field = new FieldsVM();
                field.field = propNames[i].Name;
                field.column = "";
                field.required = requeridos.Contains(propNames[i].Name);
                fields.fieldList.Add(field);

            }
            columns.Add(new SelectListItem { Text = "-- None --", Value = "" });
            foreach (DataColumn c in dt.Columns)
            {
                columns.Add(new SelectListItem { Text = c.ColumnName, Value = c.ColumnName });
            }
            fields.fileColumns = columns;
            ViewBag.columnList = new SelectList(columns, "Value", "Text");
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult chooseFields(List<FieldsVM> fieldList)
        {
            DataTable dt = (DataTable)Session["DataTableModel"];
            List<importTable> lst = ModelDataTableBinder.dataTableToTypeList<importTable>(dt, fieldList);

            db.importTable.AddRange(lst);
            db.SaveChanges();


            return View("SaveData");
        }

        // [NonAction]
        // private List<T> dataTableToTypeList<T>(DataTable dt, List<FieldsVM> fieldList)
        // {
        //     Type tipo = Activator.CreateInstance<T>().GetType();// crea una instancia de tipo T y obtine el tipo de dato
        //     //typeof(T).GetType(); ** 
        //     var fields = tipo.GetFields(); //obtiene los campos de la clase EX:  int count
        //     var properties = tipo.GetProperties();// obtiene las propiedades de la clase EX: int count {get; set;}
        //     List<T> lst = new List<T>();
        //     List<string> columns = new List<string>();
        //     foreach (FieldsVM dc in fieldList)
        //     {
        //         columns.Add(dc.column); //agrega los nombres de columnas relacionadas con las propiedades de la clase en la vista chooseFields.cshtml
        //     }



        //     foreach (DataRow dr in dt.Rows) // para cada fila del data table
        //     {
        //         var ob = Activator.CreateInstance<T>(); // instancia la clase

        //         foreach (var propertyInfo in properties)
        //         {
        //             if (columns.Contains(propertyInfo.Name))
        //             {
        //                 Type ptype = propertyInfo.PropertyType; // obtiene tipo de la propiedad

        //                 if (ptype.Name.Contains("Nullable"))
        //                 {
        //                     ptype = Nullable.GetUnderlyingType(propertyInfo.PropertyType); // obtiene tipo original EX: Nullable.GetUnderlyingType(Nullable<double>) --> devuelve double
        //                 }

        //                 FieldsVM fvm = fieldList.Where(x => x.field == propertyInfo.Name).FirstOrDefault();
        //                 var dataCol = dr[fvm.column];

        //                 if (String.IsNullOrWhiteSpace(fvm.column))
        //                 {
        //                     if (ptype.IsValueType)
        //                     {
        //                         Activator.CreateInstance(ptype);
        //                         propertyInfo.SetValue(ob, Activator.CreateInstance(ptype)); //setea el valor a la propiedad del objeto ob
        //                     }
        //                     else
        //                     {
        //                         propertyInfo.SetValue(ob, null);
        //                     }

        //                 }
        //                 else
        //                 {
        //                     if (propertyInfo.GetType().Name.ToLower().Equals("string"))
        //                     {
        //                         propertyInfo.SetValue(ob, dataCol.ToString());
        //                     }
        //                     else
        //                     {
        //                         dynamic changedObj;
        //                         if (ptype.Name.ToLower().Equals("boolean") || ptype.Name.ToLower().Equals("bool"))
        //                         {
        //                             string strval = dataCol.ToString();
        //                             if (strval == "1" || strval.ToLower() == "true")
        //                             {
        //                                 strval = "true";
        //                             }
        //                             else
        //                             {
        //                                 strval = "false";
        //                             }
        //                             changedObj = Convert.ChangeType(strval, ptype);
        //                         }
        //                         else
        //                         {
        //                             changedObj = Convert.ChangeType(dataCol.ToString(), ptype);
        //                         }
        //                         // cambia tipo de dato , de la columna, al tipo de dato de la propiedad
        //                         propertyInfo.SetValue(ob, changedObj);
        //                     }

        //                 }

        //             }
        //         }

        //         lst.Add(ob); // agrega instancia a lista
        //     }

        //     return lst;
        // }



    }
}