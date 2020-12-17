using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Client.Models
{
    public class PageData
    {
        public int total { get; set; }
        public int current { get; set; }
        public DataSet rows { get; set; }
    }
}
