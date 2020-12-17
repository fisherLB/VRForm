using System;
using System.Security.Cryptography;
using System.Text;

namespace GraphicsEvaluatePlatform.Infrastructure.Encrypt
{
    public class MD5Algorithm : IEncryptPasswordStrategy
    {
        public string EncryptFor(string source)
        {
            string result = string.Empty;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5_input = UTF8Encoding.Default.GetBytes(source);
            byte[] md5_output = md5.ComputeHash(md5_input);
            result = BitConverter.ToString(md5_output);
            return result;
        }
    }
}
