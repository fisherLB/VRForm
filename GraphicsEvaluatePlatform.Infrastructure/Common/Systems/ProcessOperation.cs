using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Systems
{
  public  class ProcessOperation
    {
        /// <summary>
        /// 进程操作
        /// </summary>
        public static void ProcessKill(string processName)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName.ToLower() == processName.ToLower())
                {
                    item.Kill();
                }
            }

        }

    }
}
