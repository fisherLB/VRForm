using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Encrypt
{
    public class SHA1Algorithm:IEncryptPasswordStrategy
    {
        public string EncryptFor(string source)
        {
            string result = string.Empty;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] sha1_input = UTF8Encoding.Default.GetBytes(source);
            byte[] sha1_output = sha1.ComputeHash(sha1_input);
            result = BitConverter.ToString(sha1_output);
            return result;
        }
    }
}
