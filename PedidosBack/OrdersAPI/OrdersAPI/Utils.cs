using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersAPI
{
    public class Utils
    {
        public static List<T> loadObjects<T>(DataTable dt) where T: class {
            List<T> listObject = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                object obj = Activator.CreateInstance(typeof(T));

                foreach (DataColumn col in dt.Columns)
                {
                    if (row[col.ColumnName].ToString() != string.Empty)
                    {
                        setValues(ref obj, col.ColumnName, row[col.ColumnName]);
                    }
                }
                listObject.Add((T)obj);
            }
            return listObject;
        }

        public static T loadObject<T>(DataTable dt) where T : class
        {
            object obj = Activator.CreateInstance(typeof(T));
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (row[col.ColumnName].ToString() != string.Empty)
                    {
                        setValues(ref obj, col.ColumnName, row[col.ColumnName]);
                    }
                }
            }
            return (T) obj;
        }
        public static void setValues(ref object obj,string property,object value) {
            var prop = obj.GetType().GetProperty(property).PropertyType.FullName;
            if (prop.Contains("Int32"))
            {
                obj.GetType().GetProperty(property).SetValue(obj, (int)value);
            }
            else if (prop.Contains("String"))
            {
                obj.GetType().GetProperty(property).SetValue(obj, (string)value);
            }
            else if (prop.Contains("Boolean"))
            {
                obj.GetType().GetProperty(property).SetValue(obj, (bool)value);
            }
            else if (prop.Contains("DateTime"))
            {
                obj.GetType().GetProperty(property).SetValue(obj, (DateTime)value);
            }
            else if (prop.Contains("Double"))
            {
                obj.GetType().GetProperty(property).SetValue(obj, (double)value);
            }
            else if (prop.Contains("Decimal"))
            {
                obj.GetType().GetProperty(property).SetValue(obj, (decimal)value);
            }
            else if (prop.Contains("Int16"))
            {
                obj.GetType().GetProperty(property).SetValue(obj, (short)value);
            }
            else if (prop.Contains("Int64"))
            {
                obj.GetType().GetProperty(property).SetValue(obj, (long)value);
            }
            else {
                obj.GetType().GetProperty(property).SetValue(obj, null);
            }
        }
    }
}
