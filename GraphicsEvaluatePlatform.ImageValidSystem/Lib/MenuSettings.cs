/***********************************************************************
 * Copyright @ Taitan Soft Corporation 2018. All rights reserved.
 ***********************************************************************/
using GraphicsEvaluatePlatform.ImageValidSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;


/***********************************************************************
 * 项目名称: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 项目描述: 
 * 类 名 称: MenuSettings
 * 说    明: 
 * 版 本 号: v1.0.0
 * 作    者: admin
 * 命名空间: GraphicsEvaluatePlatform.ImageValidSystem.Lib
 * 文件名称: MenuSettings
 * CLR 版本: 4.0.30319.42000
 * 创建时间: 2018/5/11 9:50:25
 ***********************************************************************/
namespace GraphicsEvaluatePlatform.ImageValidSystem.Lib
{
     public  class MenuSettings
    {
        /// <summary>
          /// 设置MenuStrip控件
          /// </summary>
          /// <param name="address"></param>
        public void SetMainMenu(MenuStrip menuStrip1, string address)
        {
            menuStrip1.Items.Clear();
            //读取XML文件
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"..\..\XML/Menu.xml");
            XmlNode root = xmlDoc.SelectSingleNode("MenuStrip");
            SetMenu(menuStrip1, root);
        }
        /// <summary>
          /// 设置MenuStrip控件第一层菜单及其子菜单
          /// </summary>
          /// <param name="ms">MenuStrip控件</param>
          /// <param name="xnode">XML结点</param>
        private void SetMenu(MenuStrip ms, XmlNode xnode)
        {
            foreach (XmlNode xn in xnode.ChildNodes)
            {
                XmlElement xe = (XmlElement)xn;
                ToolStripMenuItem newtsmi = new ToolStripMenuItem(xe.GetAttribute("Text"));
                //设置快捷键
                if (string.IsNullOrWhiteSpace(xe.GetAttribute("ShortKey")))
                {
                    SetShortKey(newtsmi, xe.GetAttribute("ShortKey"));
                }
                //设置调用函数
                if (!string.IsNullOrWhiteSpace(xe.GetAttribute("FunctionAttribute")))
                {
                    newtsmi.Tag = xe.GetAttribute("FunctionAttribute");
                    newtsmi.Click += ToolStripMenuItem_Click;
                }
                //设置子菜单
                if (xn.ChildNodes.Count != 0)
                {
                    SetItem(newtsmi, xn);
                }
                ms.Items.Add(newtsmi);
            }
        }
        /// <summary>
          /// 设置 ToolStripMenuItem 控件及其子菜单（递归设置）
          /// </summary>
          /// <param name="tsmi">ToolStripMenuItem控件</param>
          /// <param name="xnode">XML结点</param>
        private void SetItem(ToolStripMenuItem tsmi, XmlNode xnode)
        {
            foreach (XmlNode xn in xnode.ChildNodes)
            {
                XmlElement xe = (XmlElement)xn;
                ToolStripMenuItem newtsmi = new ToolStripMenuItem(xe.GetAttribute("Text"));
                switch (xe.GetAttribute("Type"))
                {
                    case "MenuItem":
                        {
                            //设置快捷键
                            if (!string.IsNullOrWhiteSpace(xe.GetAttribute("ShortKey")))
                            {
                                SetShortKey(newtsmi, xe.GetAttribute("ShortKey"));
                            }
                            //设置调用函数
                            if (!string.IsNullOrWhiteSpace(
                     xe.GetAttribute("FunctionAttribute")))
                            {
                                newtsmi.Tag = xe.GetAttribute("FunctionAttribute");
                                newtsmi.Click += ToolStripMenuItem_Click;
                            }
                            //设置子菜单
                            if (xn.ChildNodes.Count != 0)
                            {
                                SetItem(newtsmi, xn);
                            }
                            tsmi.DropDownItems.Add(newtsmi as ToolStripItem);
                        }
                        break;
                    case "Seperator":
                        {
                            tsmi.DropDownItems.Add(new ToolStripSeparator());
                        }
                        break;
                    default: break;
                }
            }
        }
        /// <summary>
          /// 通过字符串（如"Ctrl+Alt+A"）判断按键信息
          /// </summary>
          /// <param name="key"></param>
          /// <returns></returns>
        private void SetShortKey(ToolStripMenuItem tsmi, string key)
        {
            System.Windows.Forms.Keys result = default(System.Windows.Forms.Keys);
            string[] keys = key.Split('+');
            if (keys.Contains("Ctrl")) { result |= Keys.Control; }
            if (keys.Contains("Shift")) { result |= Keys.Shift; }
            if (keys.Contains("Alt")) { result |= Keys.Alt; }
            if (keys.Contains("A")) { result |= Keys.A; }
            if (keys.Contains("B")) { result |= Keys.B; }
            if (keys.Contains("C")) { result |= Keys.C; }
            if (keys.Contains("D")) { result |= Keys.D; }
            if (keys.Contains("E")) { result |= Keys.E; }
            if (keys.Contains("F")) { result |= Keys.F; }
            if (keys.Contains("G")) { result |= Keys.G; }
            if (keys.Contains("H")) { result |= Keys.H; }
            if (keys.Contains("I")) { result |= Keys.I; }
            if (keys.Contains("J")) { result |= Keys.J; }
            if (keys.Contains("K")) { result |= Keys.K; }
            if (keys.Contains("L")) { result |= Keys.L; }
            if (keys.Contains("M")) { result |= Keys.M; }
            if (keys.Contains("N")) { result |= Keys.N; }
            if (keys.Contains("O")) { result |= Keys.O; }
            if (keys.Contains("P")) { result |= Keys.P; }
            if (keys.Contains("Q")) { result |= Keys.Q; }
            if (keys.Contains("R")) { result |= Keys.R; }
            if (keys.Contains("S")) { result |= Keys.S; }
            if (keys.Contains("T")) { result |= Keys.T; }
            if (keys.Contains("U")) { result |= Keys.U; }
            if (keys.Contains("V")) { result |= Keys.V; }
            if (keys.Contains("W")) { result |= Keys.W; }
            if (keys.Contains("X")) { result |= Keys.X; }
            if (keys.Contains("Y")) { result |= Keys.Y; }
            if (keys.Contains("Z")) { result |= Keys.Z; }
            if (keys.Contains("0")) { result |= Keys.D0; }
            if (keys.Contains("1")) { result |= Keys.D1; }
            if (keys.Contains("2")) { result |= Keys.D2; }
            if (keys.Contains("3")) { result |= Keys.D3; }
            if (keys.Contains("4")) { result |= Keys.D4; }
            if (keys.Contains("5")) { result |= Keys.D5; }
            if (keys.Contains("6")) { result |= Keys.D6; }
            if (keys.Contains("7")) { result |= Keys.D7; }
            if (keys.Contains("8")) { result |= Keys.D8; }
            if (keys.Contains("9")) { result |= Keys.D9; }
            if (keys.Contains("F1")) { result |= Keys.F1; }
            if (keys.Contains("F2")) { result |= Keys.F2; }
            if (keys.Contains("F3")) { result |= Keys.F3; }
            if (keys.Contains("F4")) { result |= Keys.F4; }
            if (keys.Contains("F5")) { result |= Keys.F5; }
            if (keys.Contains("F6")) { result |= Keys.F6; }
            if (keys.Contains("F7")) { result |= Keys.F7; }
            if (keys.Contains("F8")) { result |= Keys.F8; }
            if (keys.Contains("F9")) { result |= Keys.F9; }
            if (keys.Contains("F10")) { result |= Keys.F10; }
            if (keys.Contains("F11")) { result |= Keys.F11; }
            if (keys.Contains("F12")) { result |= Keys.F12; }
            tsmi.ShortcutKeys = result;
        }
        /// <summary>
          /// 特性 RemarkAttribute，用在函数上，其 Remark 属性决定了
          /// 积分函数 Integration 应该选择程序中的哪个函数进行计算
          /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class RemarkAttribute : Attribute
        {
            string remark;
            public string Remark
            {
                get { return remark; }
            }
            //构造函数
            public RemarkAttribute(string comment)
            {
                remark = comment;
            }
        }
        /// <summary>
          /// 单击一个ToolStripMenuItem后触发的事件
          /// </summary>
          /// <param name="sender"></param>
          /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //如果Tag为空，则不执行任何事件
            if (string.IsNullOrWhiteSpace((string)((ToolStripMenuItem)sender).Tag))
            {
                return;
            }
            //如果Tag存在，则寻找具有和Tag内容一致特性的函数并调用
            FunctionInvoke((string)((ToolStripMenuItem)sender).Tag);
        }
        /// <summary>
          /// 调用具有指定特性名的函数
          /// </summary>
          /// <param name="funcattr">函数的RemarkAttribute特性值</param>
        private void FunctionInvoke(string funcattr)
        {
            //需要 using System.Reflection;
            MethodInfo[] mi = typeof(frmMain).GetMethods(
         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //找到具有指定特性的函数，进行调用
            foreach (MethodInfo m in mi)
            {
                Type t2 = typeof(RemarkAttribute);
                RemarkAttribute ra = (RemarkAttribute)Attribute.GetCustomAttribute(m, t2);
                if (ra != null && ra.Remark == funcattr)
                {
                    m.Invoke(this, new object[] { });
                    break;
                }
            }
        }
      
    }

}
