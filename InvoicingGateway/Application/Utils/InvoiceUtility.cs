using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class InvoiceUtility
    {

        public static string GeneratingInvoiceNo()
        {
            string merchantPrefix = ConfigHelper.Configuration.GetSection("NumberOfDigits")["GeneralInvoicesPrefix"];
            string date = DateTime.Now.ToString("yyMMdd");
            //string time = DateTime.Now.ToString("HHmmss");
            string randomNumber = Generate_Digits(4).ToString();
            string invoiceNumber = merchantPrefix + date + randomNumber;

            return invoiceNumber;
        }
        public static string GenerateOrderReference()
        {
            int length = int.Parse(ConfigHelper.Configuration.GetSection("NumberOfDigits")["OrderReference"]);

            const string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = characters[random.Next(characters.Length)];
            }

            return new string(result);
        }

        public static long GeneratePayToken()
        {
            int digitNo = int.Parse(ConfigHelper.Configuration.GetSection("NumberOfDigits")["PaymentToken"]);
            return Generate_Digits(digitNo);
        }
        public static string GeneratePayReference()
        {
            int digitNo = int.Parse(ConfigHelper.Configuration.GetSection("NumberOfDigits")["PaymentReference"]);

            string date = DateTime.Now.ToString("yyMMdd");
            string time = DateTime.Now.ToString("HHmmss");
            string randomNumber = Generate_Digits(4).ToString();
            string payReference = time + randomNumber + date;

            return payReference;
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
