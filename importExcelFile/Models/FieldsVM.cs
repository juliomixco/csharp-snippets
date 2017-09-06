using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestProject.Areas.Import.Models
{
    public class FieldsVM
    {
        public string field { get; set; }
        public string column { get; set; }
        public bool required { get; set; }
    }
    public class FieldListVM
    {
        public List<FieldsVM> fieldList { get; set; }
        public IEnumerable<SelectListItem> columnsDropdown
        {
            get { return new SelectList(fileColumns, "Value", "Text"); }
        }
        public List<SelectListItem> fileColumns { get; set; }

    }
}