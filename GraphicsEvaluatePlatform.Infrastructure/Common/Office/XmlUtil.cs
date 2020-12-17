using System.Data;
using System.IO;
using System.Text;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Office
{
    public class XmlUtil
    {
        public static byte[] GetByteForDataSet(DataSet ds)
        {
            string xml = ds.GetXml();
            byte[] b = Encoding.UTF8.GetBytes(xml);
            return b;
        }

        public static OperationResult GetByteForDataSet(DataTable dt, string filepath)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            using (FileStream sw = File.Create(filepath))
            {

                string xml = dt.DataSet.GetXml();
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                //bytes.(sw);
                sw.Write(bytes, 0, bytes.Length); ;
                //sw.Close(); //在服务端生成文件
            }
            return ret;
        }
    }
}
