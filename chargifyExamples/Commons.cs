using System;
using System.Linq;
using System.Web;
using ChargifyNET;
using ChargifyNET.Configuration;
using PWDTK_DOTNET451;
using System.Net.Mail;
using System.Configuration;
using TestProject.Models;
using System.Data;
using System.Text;
using System.Data.Entity;
using TestProject.TestProjectSecurity;
using System.Security.Cryptography;
using System.Web.Security;
using System.Net.Mime;
using System.Collections.Generic;
using System.IO;

namespace TestProject.Validations
{
    public class Commons
    {
        //*********************** Chargify *********************************
        ///******Funciones que sirven para chargify*****/


        public static int getComponentId(string productName)
        {

            string componentName = null;
            switch (productName)
            {
                case "basic":
                    //componentName = "BASIC_USER_COMPONENT";
                    componentName = "CHARGIFY06";
                    break;
                case "basicyearly":
                    //componentName = "BASIC_USERYEARLY_COMPONENT";
                    componentName = "CHARGIFY07";
                    break;
                case "pro":
                    //componentName = "PRO_USER_COMPONENT";
                    componentName = "CHARGIFY08";
                    break;
                case "proyearly":
                    //componentName = "PRO_USERYEARLY_COMPONENT";
                    componentName = "CHARGIFY09";
                    break;
                case "enterprise":
                    //componentName = "ENTERPRISE_USER_COMPONENT";
                    componentName = "CHARGIFY11";
                    break;
                case "componentNameyearly":
                    componentName = "CHARGIFY10";//"ENTERPRISE_USERYEARLY_COMPONENT";
                    break;
                case "free":
                    componentName = "CHARGIFY10";
                    break;
            }

            int componentID = Convert.ToInt32(Commons.getDefaultSetting(componentName));//Convert.ToInt32(ConfigurationManager.AppSettings[componentName]);

            return componentID;

        }


        public static bool SendEmail(string EmFrom, string EmtoAddress, string Emsubject, string Embody, bool EmIsBodyHtml = false)
        {
            //string your_id = "team@exampe.com";
            //string your_password = "1234";
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp-relay.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //Credentials = new System.Net.NetworkCredential(your_id, your_password),
                    Timeout = 10000,
                };
                //MailMessage mm = new MailMessage(From, "recepient@gmail.com", "subject", "body");
                MailMessage mm = new MailMessage(EmFrom, EmtoAddress, Emsubject, Embody);
                client.Send(mm);
                return true;
            }
            catch
            {
                return false;
            }
        }//Cierre SendEmail


        public static bool SendEmail(string EmFrom, string EmtoAddress, string Emsubject, string Embody, string filepath, string cid, bool EmIsBodyHtml = false)
        {
            //string your_id = "team@exampe.com";
            //string your_password = "1234";
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp-relay.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //Credentials = new System.Net.NetworkCredential(your_id, your_password),
                    Timeout = 10000,
                };
                //MailMessage mm = new MailMessage(From, "recepient@gmail.com", "subject", "body");
                //MailMessage mm = new MailMessage(EmFrom, EmtoAddress, Emsubject, Embody);
                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.AlternateViews.Add(getEmbeddedImage(Embody, filepath,cid));
                mail.From = new MailAddress(EmFrom);
                mail.To.Add(EmtoAddress);
                mail.Subject = Emsubject;
                client.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendEmail(string EmFrom, string EmtoAddress, string Emsubject, string Embody, List<string> filepath, List<string> cid, bool EmIsBodyHtml = false)
        {
            //string your_id = "team@exampe.com";
            //string your_password = "1234";
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp-relay.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //Credentials = new System.Net.NetworkCredential(your_id, your_password),
                    Timeout = 10000,
                };
                //MailMessage mm = new MailMessage(From, "recepient@gmail.com", "subject", "body");
                //MailMessage mm = new MailMessage(EmFrom, EmtoAddress, Emsubject, Embody);
                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.AlternateViews.Add(getEmbeddedImage(Embody, filepath, cid));
                mail.From = new MailAddress(EmFrom);
                mail.To.Add(EmtoAddress);
                mail.Subject = Emsubject;
                client.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static AlternateView getEmbeddedImage(string body, string filepath, string cid)
        {
            //string yourFile = "c:/header.png";
            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(filepath)); //MediaTypeNames.Image.Jpeg);//new LinkedResource(filepath);
            inline.ContentId = cid;//Guid.NewGuid().ToString();
            string htmlBody = body;//@"<img src='cid:" + inline.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);
            return alternateView;
        }

        private static AlternateView getEmbeddedImage(string body, List<string> filepath, List<string> cid)
        {
            string htmlBody = body;
            LinkedResource inline;
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            for (int i = 0; i < filepath.Count; i++)
            {
                inline = new LinkedResource(HttpContext.Current.Server.MapPath(filepath[i]));
                inline.ContentId = cid[i];
                alternateView.LinkedResources.Add(inline);
            }
            
            return alternateView;
        }

        public static string EncryptKey(string cadena, string key)
        {
            //arreglo de bytes donde guardaremos la llave
            byte[] keyArray;
            //arreglo de bytes donde guardaremos el texto
            //que vamos a encriptar
            byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(cadena);
            //se utilizan las clases de encriptación
            //provistas por el Framework
            //Algoritmo MD5
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            //se guarda la llave para que se le realice hashing
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            //Algoritmo 3DAS
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            //se empieza con la transformación de la cadena
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //arreglo de bytes donde se guarda la cadena cifrada
            byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);
            tdes.Clear();
            //se regresa el resultado en forma de una cadena
            //return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
            return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
        } //cierre de EncryptKey

        public static string DecryptKey(string clave, string key)
        {
            byte[] keyArray;
            //convierte el texto en una secuencia de bytes

            byte[] Array_a_Descifrar = Convert.FromBase64String(clave);

            //se llama a las clases que tienen los algoritmos
            //de encriptación se le aplica hashing
            //algoritmo MD5
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
            tdes.Clear();
            //se regresa en forma de cadena
            return UTF8Encoding.UTF8.GetString(resultArray);
        }//Cierre DecryptKey

        public static string getDefaultSetting(string code)
        {
            TestProjectEntities db = new TestProjectEntities();

            try
            {

                genConfigName config = db.genConfigName.Where(x => x.code.ToLower() == code.ToLower()).FirstOrDefault();
                //tenant tenant = db.tenant.Find(user.tenantId);
                //billingRefNumber = tenant.billingRefNumber;
                if (config != null)
                {
                    return config.defaultValue;
                }
            }
            catch (Exception e)
            {


            }


            return null;
        }

        public static string getSetting(string code)
        {
            TestProjectEntities db = new TestProjectEntities();

            try
            {
                int currenttenant = getIdentity().tenatId;
                genConfigName config = db.genConfigName.Where(x => x.code.ToLower() == code.ToLower()).FirstOrDefault();
                genConfig configvalue = db.genConfig.Where(c => c.tenantId == currenttenant && c.genConfigNameId == config.genConfigNameId).FirstOrDefault();
                //tenant tenant = db.tenant.Find(user.tenantId);
                //billingRefNumber = tenant.billingRefNumber;
                if (config != null)
                {
                    return configvalue.parameter;
                }
            }
            catch (Exception e)
            {


            }


            return null;
        }

        public static byte[] getBytes(HttpPostedFile file) {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }

    }
}