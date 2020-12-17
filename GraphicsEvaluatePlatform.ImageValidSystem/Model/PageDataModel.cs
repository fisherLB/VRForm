using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.ImageValidSystem.Model
{
    public class PageDataModel
    {
        public int total { get; set; }
        public int current { get; set; }
        public DataSet rows { get; set; } 
    }
}
