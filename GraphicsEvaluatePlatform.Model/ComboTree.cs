using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Model
{
   public class ComboTree
    {
        public string id { get; set; }
        public string ParentId { get; set; }
        public string text { get; set; }

        public string state { get; set; }

        public List<ComboTree> nodes { get; set; }
    }
}
