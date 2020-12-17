using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Encrypt
{
    public class PlaintextAlgorithm:IEncryptPasswordStrategy
    {
        public string EncryptFor(string source)
        {
            return source;
        }
    }
}
