using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories.Impl.V1.DRY
{
    public class ConvertToModel<R>
    {
        public static IList<R> DataTableToModels(DataTable dt)
        {
            return JsonConvert.DeserializeObject<IList<R>>(DataTableToJSONWithJSONNet(dt));
        }
        public static R DataToModel(DataTable dt)
        {
            return JsonConvert.DeserializeObject<R>(DataTableToJSONWithJSONNet(dt));
        }
        private static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            var ignore = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore};
            JSONString = JsonConvert.SerializeObject(table, ignore);
            return JSONString;
        }
    }
}
