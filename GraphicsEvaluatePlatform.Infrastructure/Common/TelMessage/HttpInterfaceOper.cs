using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.TelMessage
{
    public class HttpInterfaceOper
    {
        public static string GetSendResult(string url)
        {
            var result = "";
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "text/html;charset=gb2312";
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                result = streamReader.ReadToEnd();
                httpWebResponse.Close();
            }
            catch (Exception e)
            {
                result = e.ToString();
            }
            return result;
        }
    }
}
