using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure
{
    public class HubMessageConfig
    {

        public HubMessageConfig(HubMessageShowType type)
        {
            ok = "确定";
            cancel = "取消";
            title = "消息";
            icon = "info";
            this.type = type;
        }

        /// <summary>
        /// 显示类型
        /// </summary>
        public HubMessageShowType type { get;private set; }

        /// <summary>
        /// ok的文字显示
        /// </summary>
        public string ok { get; set; }

        /// <summary>
        /// cancel的文字显示
        /// </summary>
        public string cancel { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 仅type为Alert有效,显示的图标图像,默认info。可用值有：error,question,info,warning。
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 仅type为Show有效,消息窗口多少毫秒自动关闭,0表示不会自动关闭
        /// </summary>
        public int timeout { get; set; }

        /// <summary>
        /// 仅type为Show有效,窗口显示类型
        /// </summary>
        public string showType { get; set; }

        /// <summary>
        /// 仅type为Show有效,窗口显示过度时间
        /// </summary>
        public int showSpeed { get; set; }
        /// <summary>
        /// 仅type为Confirm有效,点击ok执行的js脚本
        /// </summary>
        public string onOkExeScript { get; set; }

        /// <summary>
        /// 仅type为Confirm有效,点击cancel执行的js脚本
        /// </summary>
        public string onCancelScript { get; set; }

        
    }
}
