using FastEnumUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils
{
    public class EnumValues
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string NameEn { get; private set; }

        public EnumValues(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public EnumValues(int id, string nameEn, string nameAr)
        {
            Id = id;
            NameEn = nameEn;
            Name = nameAr;
        }
    }
    public static class EnumUtils
    {
        public static List<EnumValues> GetEnumValueslist<TEnum>() where TEnum : struct, Enum
        {
            List<EnumValues> enumValues = new();
            foreach (TEnum res in Enum.GetValues<TEnum>())
                enumValues.Add(new EnumValues(res.ToInt32(), res.ToString(), res.GetDescription()));
            return enumValues;
        }
        public static List<TEnum> GetAllEnumslist<TEnum>() where TEnum : struct, Enum
        {
            List<TEnum> enums = new();
            foreach (object res in Enum.GetValues<TEnum>())
                enums.Add((TEnum)res);

            return enums;
        }
        public static StringBuilder GetAllValues<TEnum>() where TEnum : struct, Enum
        {
            //var values = Enum.GetValues<TEnum>();
            //var message = string.Empty;
            //foreach (object res in values)
            //    message += $"{(int)res} to ({res}), ";
            //message = "[" + message.Trim().TrimEnd(',') + "]";
            //return message;

            var message = new StringBuilder().Append("[");
            var OR = "";
            foreach (object res in Enum.GetValues(typeof(TEnum)))
            {
                message.Append($"{OR}{(int)res} to ({res})");
                // message.Append($"{OR}{(int)res}");
                OR = " ,";
                //OR = "\n";
            }
            return message.Append("]");
        }
        public static string GetEnumsNames<TEnum>() where TEnum : struct, Enum
        {
            var values = Enum.GetNames<TEnum>();
            var message = "('" + string.Join("','", values) + "')";
            return message;
        }
        public static string GetEnumsValues<TEnum>() where TEnum : struct, Enum
        {
            var message = "(";
            foreach (object res in Enum.GetValues(typeof(TEnum)))
            {
                message += $"{(int)res},";
            }
            message = message.TrimEnd(',');
            return message += ")";
        }

        public static bool ValidateEnums<TEnum>(object value) where TEnum : struct, Enum
        {
            var preValue = decimal.TryParse(value.ToString(), out decimal Res)
               ? ((int)Res)
               : value;
            var res = FastEnum.TryParse(preValue.ToString(), false, out TEnum enumValue);
            return (res && FastEnum.IsDefined(enumValue));
        }

    }
    public static class EnumUtils<TEnum> where TEnum : struct, Enum
    {
        public static string GetAllValues()
        {
            var values = FastEnum.GetValues<TEnum>();
            var message = string.Empty;
            foreach (object res in values)
                message += $"{(int)res} to ({res}), ";
            message = "[" + message.Trim().TrimEnd(',') + "]";
            return message;
        }

        public static string GetEnumsNames()
        {
            var values = FastEnum.GetNames<TEnum>();
            var message = "('" + string.Join("','", values) + "')";
            return message;
        }

        public static string GetEnumsValues()
        {
            var values = FastEnum.GetValues<TEnum>();
            var message = string.Empty;
            foreach (object res in values)
                message += $"{(int)res},";
            message = "(" + message.Trim().TrimEnd(',') + ")";
            return message;
        }

       
    }
}
