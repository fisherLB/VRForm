/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.Infrastructure.Common.Strings;
using GraphicsEvaluatePlatform.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Transactions;




/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 项目描述: 
 * 类 名 称: SynchronousData
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 文件名称: SynchronousData
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/9 12:01:20
 ***********************************************************************/
namespace VRForm.Lib
{
    /// <summary>
    /// 同步数据类
    /// </summary>
    public static class SynchronousData
    {

        /// <summary>
        /// 同步用户表数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string SynchronousUsersData(DataTable data, string tableName)
        {
            using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                var del = DataBll.Query("delete from " + tableName);
                bool re = DataBll.SqlBulkCopyByDatatable(tableName, data);
            }
            return "";

        }

        /// <summary>
        /// 发送client请求
        /// </summary>
        /// <param name="MetodUrl"></param>
        /// <returns></returns>
        public static string SendClient(string MetodUrl)
        {
            string url = ConfigurationManager.AppSettings["WEBAPIURL"];

            var result = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    string Ip = "192.168.1.136";
                    var requestJson = JsonUtil.ToJson(Ip);
                    HttpContent httpContent = new StringContent(requestJson);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    result = client.PostAsync(MetodUrl, httpContent).Result.Content.ReadAsStringAsync().Result;
                    result = result.Replace("\"{", "{");
                    result = result.Replace("}\"", "}");
                    result = result.Replace("\\", "");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                //Application.Exit();
            }

            return result;
        }
    }
}
