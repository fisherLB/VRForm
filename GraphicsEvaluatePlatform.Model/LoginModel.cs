/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2017. All rights reserved.
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.Model
 * 项目描述: 
 * 类 名 称: LoginModel
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: WorkServer
 * 命名空间: GraphicsEvaluatePlatform.Model
 * 文件名称: LoginModel
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2017/4/20 16:53:00
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.Model
{
    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 获取或设置 登录账号
        /// </summary>
        [Required]
        [Display(Name = "登录账号")]
        public string LoginName { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "登录密码")]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 是否记住登录
        /// </summary>
        [Display(Name = "记住登录")]
        public bool IsRememberLogin { get; set; }

        /// <summary>
        /// 获取或设置 登录验证码
        /// </summary>
        [Required(ErrorMessage = "*")]
        [Display(Name = "验证码")]
        public string ValidateCode { get; set; }

        /// <summary>
        /// 获取或设置 登录成功后返回地址
        /// </summary>
        //public string ReturnUrl { get; set; }
    }
}
