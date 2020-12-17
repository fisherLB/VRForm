using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Model
{
    /// <summary>
    /// easyui 分页参数
    /// </summary>
    public class GridPager
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 当前页是第几页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        private string _filter = "";

        public string filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

    }
}
