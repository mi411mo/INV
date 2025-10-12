using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Domain.Utils
{
    public static class StringExtensions
    {
        public static string ParseObjectToQueryString(object obj, bool lowercase)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select (lowercase ? p.Name.ToLower() : p.Name) + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }
        public static string ParseObjectToSqlString(object obj, bool lowercase)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select (lowercase ? p.Name.ToLower() : p.Name) + "=" + p.GetValue(obj, null).ToString();

            return string.Join("&", properties.ToArray());
        }

        //Get all properties names in the class which are not null
        public static string[] GetObjectPropertiesNames(object obj, bool lowercase)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select (lowercase ? p.Name.ToLower() : p.Name);

            return properties.ToArray();// string.Join(",", properties.ToArray());
        }

        //Get all properties Values in the class which are not null
        public static string[] GetObjectPropertiesValuesAsString(object obj, bool lowercase)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.GetValue(obj, null).ToString();

            return properties.ToArray();// string.Join(",", properties.ToArray());
        }
        //Get all properties Values in the class which are not null
        public static object[] GetObjectPropertiesValues(object obj, bool lowercase)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.GetValue(obj, null);

            return properties.ToArray();// string.Join(",", properties.ToArray());
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}
