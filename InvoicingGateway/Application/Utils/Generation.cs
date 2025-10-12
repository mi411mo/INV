using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class Generation
    {
        // TODO: check for the uniquness of generated Id
        public static long GenerateExceptionId()
        {
            return Generate_Digits(5);
        }
        public static long GenerateReferenceNumber()
        {
            return Generate_Digits(9);
        }
        static string num;
        private static long Generate_Digits(int length)
        {
            var rndDigits = new StringBuilder().Insert(0, "0123456789", length).ToString().ToCharArray();
            num = "";
            do
            {
                num = string.Join("", rndDigits.OrderBy(o => Guid.NewGuid()).Take(length));
            } while (long.Parse(num).ToString().Length != length);
            return long.Parse(num);
        }
    }
}
