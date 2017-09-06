using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TestProject.Areas.Import.Models
{
    public class ModelDataTableBinder
    {
        public static List<T> dataTableToTypeList<T>(DataTable dt, List<FieldsVM> fieldList)
        {
            Type tipo = Activator.CreateInstance<T>().GetType();// crea una instancia de tipo T y obtine el tipo de dato
            //typeof(T).GetType(); ** 
            var fields = tipo.GetFields(); //obtiene los campos de la clase EX:  int count
            var properties = tipo.GetProperties();// obtiene las propiedades de la clase EX: int count {get; set;}
            List<T> lst = new List<T>();
            List<string> columns = new List<string>();
            foreach (FieldsVM dc in fieldList)
            {
                columns.Add(dc.column); //agrega los nombres de columnas relacionadas con las propiedades de la clase en la vista chooseFields.cshtml
            }



            foreach (DataRow dr in dt.Rows) // para cada fila del data table
            {
                var ob = Activator.CreateInstance<T>(); // instancia la clase

                foreach (var propertyInfo in properties)
                {
                    if (columns.Contains(propertyInfo.Name))
                    {
                        Type ptype = propertyInfo.PropertyType; // obtiene tipo de la propiedad

                        if (ptype.Name.Contains("Nullable"))
                        {
                            ptype = Nullable.GetUnderlyingType(propertyInfo.PropertyType); // obtiene tipo original EX: Nullable.GetUnderlyingType(Nullable<double>) --> devuelve double
                        }

                        FieldsVM fvm = fieldList.Where(x => x.field == propertyInfo.Name).FirstOrDefault();
                        var dataCol = dr[fvm.column];

                        if (String.IsNullOrWhiteSpace(fvm.column))
                        {
                            if (ptype.IsValueType)
                            {
                                Activator.CreateInstance(ptype);
                                propertyInfo.SetValue(ob, Activator.CreateInstance(ptype)); //setea el valor a la propiedad del objeto ob
                            }
                            else
                            {
                                propertyInfo.SetValue(ob, null);
                            }

                        }
                        else
                        {
                            if (propertyInfo.GetType().Name.ToLower().Equals("string"))
                            {
                                propertyInfo.SetValue(ob, dataCol.ToString());
                            }
                            else
                            {
                                dynamic changedObj;
                                if (ptype.Name.ToLower().Equals("boolean") || ptype.Name.ToLower().Equals("bool"))
                                {
                                    string strval = dataCol.ToString();
                                    if (strval == "1" || strval.ToLower() == "true")
                                    {
                                        strval = "true";
                                    }
                                    else
                                    {
                                        strval = "false";
                                    }
                                    changedObj = Convert.ChangeType(strval, ptype);
                                }
                                else
                                {
                                    changedObj = Convert.ChangeType(dataCol.ToString(), ptype);
                                }
                                //dynamic changedObj = Convert.ChangeType(dataCol.ToString(), ptype); // cambia tipo de dato , de la columna, al tipo de dato de la propiedad
                                propertyInfo.SetValue(ob, changedObj);
                            }

                        }

                    }
                }

                lst.Add(ob); // agrega instancia a lista
            }

            return lst;
        }
    }
}