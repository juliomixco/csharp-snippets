using SQRely_Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQRely_Admin.Areas.Wizard.Models
{

    public class TranWizardConfigJSON
    {
        public List<ConfiguracionesJSON> tenantconfigs { get; set; }
        public List<UsuariosWizardJSON> usuarios { get; set; }
        public List<ConfiguracionesJSON> settings { get; set; }
        public int numberusers { get; set; }

    }
    public class ConfiguracionesJSON
    {
        public int configNameid { get; set; }
        public string parameter { get; set; }
        public bool active { get; set; }
        public string code { get; set; }
    }

    public class UsuariosWizardJSON
    {
        public string username { get; set; }
        public string userlastname { get; set; }
        public string alias { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public Rolusers rol { get; set; }
    }

    public class Rolusers
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}