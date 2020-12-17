using System;
using System.Text;

using System.Text.RegularExpressions;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Strings
{
    public class Validate
    {
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        public Validate()
        {
        }
        #region 数字字符串检查		

        /// <summary>
        /// 检测是否有符合时间格式
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsDateTime(string inputData)
        {
            try
            {
                DateTime m = Convert.ToDateTime(inputData);
                return true;
            }
            catch
            {
                if (inputData.Length != 8)
                    return false;
                string dt = inputData.Insert(4, "-");
                dt = dt.Insert(7, "-");

                return IsDateTime(dt);
            }
        }

        public static bool IsDateTimeCommon(string inputData)
        {
            try
            {
                DateTime m = Convert.ToDateTime(inputData);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            //Match m = RegNumber.Match(inputData);
            //return m.Success;

            try
            {
                int m = Convert.ToInt32(inputData);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }
        #endregion
    }
}
