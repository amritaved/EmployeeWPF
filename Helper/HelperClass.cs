using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWPF.Helper
{
    public class HelperClass
    {
        public static string ErrorHandling(HttpResponseMessage responseMessage)
        {
            var json =  responseMessage.Content.ReadAsStringAsync().Result;
            JArray jsonArray = JArray.Parse(json);
            string sb = null;
            foreach (var number in jsonArray)
            {
                sb += String.Concat(number["field"]?.ToString(), " ", number["message"]?.ToString(), "\n");
            }
            return sb;
        }
    }
}
