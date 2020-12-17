using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using System.IO.Compression;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Strings
{
   public class StringUtil
    {

        #region 汉字转拼音缩写
        /// <summary> 
        /// 汉字转拼音缩写 
        /// 2004-11-30 
        /// </summary> 
        /// <param name="Input">要转换的汉字字符串</param> 
        /// <returns>拼音缩写</returns> 
        public static string GetPYString(string Input)
        {
            string ret = "";
            foreach (char c in Input)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {//字母和符号原样保留 
                    ret += c.ToString();
                }
                else
                {//累加拼音声母 
                    ret += GetPYChar(c.ToString());
                }
            }
            return ret;
        }

        /// <summary> 
        /// 取单个字符的拼音声母 
        /// 2004-11-30 
        /// </summary> 
        /// <param name="c">要转换的单个汉字</param> 
        /// <returns>拼音声母</returns> 
        private static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "A";
            if (i < 0xB2C1) return "B";
            if (i < 0xB4EE) return "C";
            if (i < 0xB6EA) return "D";
            if (i < 0xB7A2) return "E";
            if (i < 0xB8C1) return "F";
            if (i < 0xB9FE) return "G";
            if (i < 0xBBF7) return "H";
            if (i < 0xBFA6) return "G";
            if (i < 0xC0AC) return "K";
            if (i < 0xC2E8) return "L";
            if (i < 0xC4C3) return "M";
            if (i < 0xC5B6) return "N";
            if (i < 0xC5BE) return "O";
            if (i < 0xC6DA) return "P";
            if (i < 0xC8BB) return "Q";
            if (i < 0xC8F6) return "R";
            if (i < 0xCBFA) return "S";
            if (i < 0xCDDA) return "T";
            if (i < 0xCEF4) return "W";
            if (i < 0xD1B9) return "X";
            if (i < 0xD4D1) return "Y";
            if (i < 0xD7FA) return "Z";
            return "*";
        }
        #endregion 汉字转拼音缩写
        #region 把汉字转化成全拼音
        private static int[] pyValue = new int[]
        {
            -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
            -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
            -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
            -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
            -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
            -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
            -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
            -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
            -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
            -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
            -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
            -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
            -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
            -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
            -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
            -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
            -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
            -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
            -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
            -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
            -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
            -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
            -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
            -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
            -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
            -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
            -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
            -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
            -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
            -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
            -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
            -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
            -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
        };

        private static string[] pyName = new string[]
        {
        "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
        "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
        "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
        "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
        "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
        "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
        "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
        "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
        "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
        "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
        "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
        "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
        "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
        "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
        "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
        "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
        "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
        "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
        "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
        "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
        "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
        "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
        "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
        "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
        "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
        "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
        "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
        "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
        "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
        "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
        "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
        "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
        "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
        };

        /// <summary>
        /// 把汉字转换成拼音(全拼)
        /// </summary>
        /// <param name="hzString">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string ConvertE(string hzString)
        {
            // 匹配中文字符
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();

            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString += noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254)  // 修正“圳”字
                            pyString += "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString += pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString;
        }
        #endregion
        #region 根据数组格式成a,b,c,d形式
        /// <summary>
        /// 根据数组格式成a,b,c,d形式
        /// </summary>
        /// <param name="Arr"></param>
        /// <returns></returns>
        public static string StrListByArr(string[] Arr)
        {
            string Str = "";
            if (Arr.Length > 0)
            {
                for (int i = 0; i < Arr.Length; i++)
                {
                    Str += "'" + Arr[i] + "',";
                }
                Str = Str.Substring(0, Str.Length - 1);
            }
            return Str;
        }
        #endregion
        #region 格式首字母大写
        /// <summary>
        /// 格式首字母大写
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string GetFistUpper(string Str)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(Str);
        }
        #endregion
        #region 取得带%%格式字符
        /// <summary>
        /// 取得带%%格式字符
        /// </summary>
        /// <param name="origText">输入字符</param>
        /// <param name="bt">在之间，比如%%，填写%</param>
        /// <param name="repValue"></param>
        /// <param name="props"></param>
        /// <param name="StrVal"></param>
        /// <returns></returns>
        public static ArrayList GetHTMLStr(string origText, string bt)
        {
            ArrayList parm = new ArrayList();
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(bt) >= 0)
            {

                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + bt + "(?<pname>\\w{1,})" + bt + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    parm.Add(m.Groups["pname"].Value.ToLower());
                }

            }
            return parm;
        }
        /// <summary>
        /// 取得带%%格式字符
        /// </summary>
        /// <param name="origText">输入字符</param>
        /// <param name="bt">在之间，比如%%，填写%</param>
        /// <param name="repValue"></param>
        /// <param name="props"></param>
        /// <param name="StrVal"></param>
        /// <returns></returns>
        public static ArrayList GetHTMLStr(string origText, char bt)
        {
            ArrayList parm = new ArrayList();
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(bt) >= 0)
            {

                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + bt.ToString() + "(?<pname>\\w{1,})" + bt.ToString() + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    parm.Add(m.Groups["pname"].Value.ToLower());
                }

            }
            return parm;
        }
        #endregion
        #region 替换带分割符号格式字符
        /// <summary>
        /// 替换带分割符号格式字符
        /// </summary>
        /// <param name="origText"></param>
        /// <param name="props"></param>
        /// <param name="StrVal"></param>
        /// <returns></returns>
        public static string GetHTML(string origText, string[] props, string[] StrVal, char Val)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(Val) >= 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + Val + "(?<pname>\\w{2,})" + Val + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.ToLower();
                    if (Array.IndexOf(props, p) >= 0)
                    {
                        string repValue = null;
                        for (int i = 0; i < props.Length; i++)
                        {
                            string Str = props[i].ToLower();
                            if (p == Str)
                            {
                                repValue = StrVal[i];
                            }
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        /// <summary>
        /// 替换带分割符号格式字符
        /// </summary>
        /// <param name="origText"></param>
        /// <param name="props"></param>
        /// <param name="StrVal"></param>
        /// <returns></returns>
        public static string GetHTML(string origText, string props, string rep, char Val)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(Val) >= 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + Val + "(?<pname>\\w{1,})" + Val + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string repValue = null;
                    string p = m.Groups["pname"].Value.ToLower();
                    string Str = props.ToLower();
                    if (p == Str)
                    {
                        repValue = rep;
                    }
                    if (repValue != null)
                        sb.Replace(m.Groups["ph"].Value, repValue);
                }
                return sb.ToString();
            }
            return origText;
        }
        /// <summary>
        /// 替换带分割符号格式字符
        /// </summary>
        /// <param name="origText"></param>
        /// <param name="props"></param>
        /// <param name="StrVal"></param>
        /// <returns></returns>
        public static string GetHTML(string origText, string props, string rep, string Val)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(Val) >= 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + Val + "(?<pname>\\w{1,})" + Val + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string repValue = null;
                    string p = m.Groups["pname"].Value.ToLower();
                    string Str = props.ToLower();
                    if (p == Str)
                    {
                        repValue = rep;
                    }
                    if (repValue != null)
                    {
                        sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        #endregion
        #region 替换带%%格式字符
        /// <summary>
        /// 替换带%%格式字符
        /// </summary>
        /// <param name="origText"></param>
        /// <param name="props"></param>
        /// <param name="StrVal"></param>
        /// <returns></returns>
        public static string GetHTML(string origText, string[] props, string[] StrVal)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf('%') >= 0)
            {

                //r = new Regex(@"(\[b\])([ \S\t]*?)(\[\/b\])", RegexOptions.IgnoreCase);
                //for (m = r.Match(sDetail); m.Success; m = m.NextMatch())
                //{
                //    sDetail = sDetail.Replace(m.Groups[0].ToString(), "<B>" + m.Groups[2].ToString() + "</B>");
                //}

                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>%(?<pname>\\w{2,})%)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.ToLower();
                    if (Array.IndexOf(props, p) >= 0)
                    {
                        string repValue = null;
                        for (int i = 0; i < props.Length; i++)
                        {
                            string Str = props[i].ToLower();
                            if (p == Str)
                            {
                                repValue = StrVal[i];
                            }
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        #endregion
        #region 格式化字符串，控制字符宽度
        /// <summary>
        /// 格式化字符串，控制字符宽度
        /// </summary>
        /// <param name="inchar"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string limichar(string inchar, int num)
        {
            if (inchar.Length > num)
            {
                inchar = inchar.Substring(0, num);
            }
            return inchar;
        }
        #endregion
        #region 取数组
        /// <summary>
        /// 根据数组长度分隔符取数组
        /// </summary>
        /// <param name="Str">输入字符</param>
        /// <param name="ArrStrLength">数组长度</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static string[] ArrStrList(string Str, int ArrStrLength, char val)
        {
            string[] Arr = new string[ArrStrLength];
            if (Str.Length > 0 && Str.IndexOf(val) >= 0)
            {
                string[] ArrStr = Str.Split(val);
                for (int i = 0; i < ArrStrLength; i++)
                {
                    if (i < ArrStr.Length)
                    {
                        if (ArrStr[i].Length > 0)
                        {
                            Arr[i] = ArrStr[i].ToLower();
                        }
                        else
                        {
                            Arr[i] = "";
                        }
                    }
                    else
                    {
                        Arr[i] = "";
                    }
                }
            }
            else
            {
                for (int i = 0; i < ArrStrLength; i++)
                {
                    Arr[i] = "";
                }
            }
            return Arr;
        }
        public static string[] ArrStrList(string Str, int ArrStrLength, string val)
        {
            string[] Arr = new string[ArrStrLength];
            if (Str.Length > 0 && Str.IndexOf(val) >= 0)
            {
                string[] ArrStr = Str.Split(val.ToCharArray());
                for (int i = 0; i < ArrStrLength; i++)
                {
                    if (i < ArrStr.Length)
                    {
                        if (ArrStr[i].Length > 0)
                        {
                            Arr[i] = ArrStr[i].ToLower();
                        }
                        else
                        {
                            Arr[i] = "";
                        }
                    }
                    else
                    {
                        Arr[i] = "";
                    }
                }
            }
            else
            {
                for (int i = 0; i < ArrStrLength; i++)
                {
                    Arr[i] = "";
                }
            }
            return Arr;
        }
        /// <summary>
        /// 根据分隔符取数组
        /// </summary>
        /// <param name="Str">输入字符</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static string[] ArrStrList(string Str, char val)
        {

            if (Str.Length > 0 && Str.IndexOf(val) >= 0)
            {
                string[] ArrStr = Str.Split(val);
                for (int i = 0; i < ArrStr.Length; i++)
                {
                    if (ArrStr[i].Length > 0)
                    {
                        ArrStr[i] = ArrStr[i].ToLower();
                    }
                    else
                    {
                        ArrStr[i] = "";
                    }
                }
                return ArrStr;
            }
            else
            {
                if (Str.Length > 0)
                {
                    string[] ArrStr = new string[1];
                    ArrStr[0] = Str.ToLower();
                    return ArrStr;
                }
                else
                {
                    return null;
                }
            }
        }
        public static string[] ArrStrList(string Str, string val)
        {

            if (Str.Length > 0 && Str.IndexOf(val) >= 0)
            {
                string[] ArrStr = Str.Split(val.ToCharArray());
                for (int i = 0; i < ArrStr.Length; i++)
                {
                    if (ArrStr[i].Length > 0)
                    {
                        ArrStr[i] = ArrStr[i].ToLower();
                    }
                    else
                    {
                        ArrStr[i] = "";
                    }
                }
                return ArrStr;
            }
            else
            {
                if (Str.Length > 0)
                {
                    string[] ArrStr = new string[1];
                    ArrStr[0] = Str.ToLower();
                    return ArrStr;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 根据数组长度分隔符取数组
        /// </summary>
        /// <param name="Str">输入字符</param>
        /// <param name="ArrStrLength">数组长度</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static int[] ArrIntList(string Str, int ArrStrLength, char val)
        {
            int[] Arr = new int[ArrStrLength];
            if (Str.Length > 0 && Str.IndexOf(val) >= 0)
            {
                string[] ArrInt = Str.Split(val);
                for (int i = 0; i < ArrStrLength; i++)
                {
                    if (i < ArrInt.Length)
                    {
                        if (ArrInt[i].Length > 0)
                        {
                            Arr[i] = int.Parse(ArrInt[i]);
                        }
                        else
                        {
                            Arr[i] = 0;
                        }
                    }
                    else
                    {
                        Arr[i] = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < ArrStrLength; i++)
                {
                    Arr[i] = 0;
                }
            }
            return Arr;
        }
        public static int[] ArrIntList(string Str, int ArrStrLength, string val)
        {
            int[] Arr = new int[ArrStrLength];
            if (Str.Length > 0 && Str.IndexOf(val) >= 0)
            {
                string[] ArrInt = Str.Split(val.ToCharArray());
                for (int i = 0; i < ArrStrLength; i++)
                {
                    if (i < ArrInt.Length)
                    {
                        if (ArrInt[i].Length > 0)
                        {
                            Arr[i] = int.Parse(ArrInt[i]);
                        }
                        else
                        {
                            Arr[i] = 0;
                        }
                    }
                    else
                    {
                        Arr[i] = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < ArrStrLength; i++)
                {
                    Arr[i] = 0;
                }
            }
            return Arr;
        }
        /// <summary>
        /// 根据分隔符取数组
        /// </summary>
        /// <param name="Str">输入字符</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static int[] ArrIntList(string Str, char val)
        {

            if (Str.Length > 0 && Str.IndexOf(val) >= 0)
            {
                string[] ArrInt = Str.Split(val);
                int[] Arr = new int[ArrInt.Length];
                for (int i = 0; i < ArrInt.Length; i++)
                {
                    if (ArrInt[i].Length > 0)
                    {
                        Arr[i] = int.Parse(ArrInt[i].Trim());
                    }
                    else
                    {
                        Arr[i] = 0;
                    }
                }
                return Arr;
            }
            else
            {
                if (Str.Length > 0)
                {
                    int[] Arr = new int[1];
                    Arr[0] = int.Parse(Str.Trim());
                    return Arr;
                }
                else
                {
                    return null;
                }
            }
        }
        public static int[] ArrIntList(string Str, string val)
        {

            if (Str.Length > 0 && Str.IndexOf(val) >= 0)
            {
                string[] ArrInt = Str.Split(val.ToCharArray());
                int[] Arr = new int[ArrInt.Length];
                for (int i = 0; i < ArrInt.Length; i++)
                {
                    if (ArrInt[i].Length > 0)
                    {
                        Arr[i] = int.Parse(ArrInt[i]);
                    }
                    else
                    {
                        Arr[i] = 0;
                    }
                }
                return Arr;
            }
            else
            {
                if (Str.Length > 0)
                {
                    int[] Arr = new int[1];
                    Arr[0] = int.Parse(Str.Trim());
                    return Arr;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region 取数组格式化为字符
        /// <summary>
        /// 根据数组长度分隔符取数组格式化为字符
        /// </summary>
        /// <param name="ArrList">输入数组</param>
        /// <param name="ArrStrLength">数组长度</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static string StrArrList(string[] ArrList, int ArrStrLength, char val)
        {
            string Str = "";
            if (ArrStrLength > 0)
            {
                for (int i = 0; i < ArrStrLength; i++)
                {
                    if (i < ArrList.Length)
                    {
                        Str += ArrList[i] + val.ToString();
                    }
                    else
                    {
                        Str += val.ToString();
                    }
                }
                Str = Str.Substring(0, Str.Length - val.ToString().Length);
            }
            return Str;
        }
        public static string StrArrList(string[] ArrList, int ArrStrLength, string val)
        {
            string Str = "";
            if (ArrStrLength > 0)
            {
                for (int i = 0; i < ArrStrLength; i++)
                {
                    if (i < ArrList.Length)
                    {
                        Str += ArrList[i] + val.ToString();
                    }
                    else
                    {
                        Str += val.ToString();
                    }
                }
                Str = Str.Substring(0, Str.Length - val.ToString().Length);
            }
            return Str;
        }
        /// <summary>
        /// 根据分隔符取数组格式化为字符
        /// </summary>
        /// <param name="ArrList">输入数组</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static string StrArrList(string[] ArrList, char val)
        {

            string Str = "";
            if (ArrList.Length > 0)
            {
                for (int i = 0; i < ArrList.Length; i++)
                {
                    Str += ArrList[i] + val.ToString();
                }
                Str = Str.Substring(0, Str.Length - val.ToString().Length);
            }
            return Str;
        }
        public static string StrArrList(string[] ArrList, string val)
        {

            string Str = "";
            if (ArrList.Length > 0)
            {
                for (int i = 0; i < ArrList.Length; i++)
                {
                    Str += ArrList[i] + val.ToString();
                }
                Str = Str.Substring(0, Str.Length - val.ToString().Length);
            }
            return Str;
        }
        /// <summary>
        /// 根据数组长度分隔符取数组格式化为字符
        /// </summary>
        /// <param name="ArrList">输入数组</param>
        /// <param name="ArrStrLength">数组长度</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static string StrArrList(int[] ArrList, int ArrStrLength, char val)
        {
            string Str = "";
            if (ArrStrLength > 0)
            {
                for (int i = 0; i < ArrStrLength; i++)
                {
                    if (i < ArrList.Length)
                    {
                        Str += ArrList[i].ToString() + val.ToString();
                    }
                    else
                    {
                        Str += val.ToString();
                    }
                }
                Str = Str.Substring(0, Str.Length - val.ToString().Length);
            }
            return Str;
        }
        public static string StrArrList(int[] ArrList, int ArrStrLength, string val)
        {
            string Str = "";
            if (ArrStrLength > 0)
            {
                for (int i = 0; i < ArrStrLength; i++)
                {
                    if (i < ArrList.Length)
                    {
                        Str += ArrList[i].ToString() + val.ToString();
                    }
                    else
                    {
                        Str += val.ToString();
                    }
                }
                Str = Str.Substring(0, Str.Length - val.ToString().Length);
            }
            return Str;
        }
        /// <summary>
        /// 根据分隔符取数组格式化为字符
        /// </summary>
        /// <param name="ArrList">输入数组</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static string StrArrList(int[] ArrList, char val)
        {

            string Str = "";
            if (ArrList.Length > 0)
            {
                for (int i = 0; i < ArrList.Length; i++)
                {
                    Str += ArrList[i].ToString() + val.ToString();
                }
                Str = Str.Substring(0, Str.Length - val.ToString().Length);
            }
            return Str;
        }
        public static string StrArrList(int[] ArrList, string val)
        {

            string Str = "";
            if (ArrList.Length > 0)
            {
                for (int i = 0; i < ArrList.Length; i++)
                {
                    Str += ArrList[i].ToString() + val.ToString();
                }
                Str = Str.Substring(0, Str.Length - val.ToString().Length);
            }
            return Str;
        }
        #endregion
        #region 替换带分割符号格式字符
        /// <summary>
        /// 替换带分割符号格式字符
        /// </summary>
        /// <param name="origText"></param>
        /// <param name="props"></param>
        /// <param name="StrVal"></param>
        /// <param name="EndVal"></param>
        /// <returns></returns>
        public static string GetSearchStrL(string origText, string[] props, char StrVal, char EndVal)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(StrVal) >= 0 && origText.IndexOf(EndVal) > 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + StrVal + "(?<pname>\\w{1,})" + EndVal + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        for (int i = 0; i < props.Length; i++)
                        {
                            string Str = props[i].ToLower();
                            if (ValNum == i)
                            {
                                repValue = Str;
                            }
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string[] props, string StrVal, string EndVal)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(StrVal) >= 0 && origText.IndexOf(EndVal) > 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + StrVal + "(?<pname>\\w{1,})" + EndVal + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        for (int i = 0; i < props.Length; i++)
                        {
                            string Str = props[i].ToLower();
                            if (ValNum == i)
                            {
                                repValue = Str;
                            }
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string props, char StrVal, char EndVal)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(StrVal) >= 0 && origText.IndexOf(EndVal) > 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + StrVal + "(?<pname>\\w{1,})" + EndVal + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        string Str = props.ToLower();
                        if (ValNum == 0)
                        {
                            repValue = Str;
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string props, string StrVal, string EndVal)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(StrVal) >= 0 && origText.IndexOf(EndVal) > 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + StrVal + "(?<pname>\\w{1,})" + EndVal + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        string Str = props.ToLower();
                        if (ValNum == 0)
                        {
                            repValue = Str;
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string[] props, char Val)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(Val) >= 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + Val + "(?<pname>\\w{1,})" + Val + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        for (int i = 0; i < props.Length; i++)
                        {
                            string Str = props[i].ToLower();
                            if (ValNum == i)
                            {
                                repValue = Str;
                            }
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string[] props, string Val)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(Val) >= 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + Val + "(?<pname>\\w{1,})" + Val + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        for (int i = 0; i < props.Length; i++)
                        {
                            string Str = props[i].ToLower();
                            if (ValNum == i)
                            {
                                repValue = Str;
                            }
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string props, char Val)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(Val) >= 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + Val + "(?<pname>\\w{1,})" + Val + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        string Str = props.ToLower();
                        if (ValNum == 0)
                        {
                            repValue = Str;
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string props, string Val)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(Val) >= 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + Val + "(?<pname>\\w{1,})" + Val + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        string Str = props.ToLower();
                        if (ValNum == 0)
                        {
                            repValue = Str;
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string[] props)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf("#") >= 0 && origText.IndexOf("#") > 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph># V(?<pname>\\w{1,})#)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        for (int i = 0; i < props.Length; i++)
                        {
                            string Str = props[i].ToLower();
                            if (ValNum == i)
                            {
                                repValue = Str;
                            }
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        public static string GetSearchStrL(string origText, string props)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf("#") >= 0 && origText.IndexOf("#") > 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph># V(?<pname>\\w{1,})#)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.Trim();
                    if (p.Length > 0)
                    {
                        int ValNum = int.Parse(p);
                        string repValue = null;
                        for (int i = 0; i < props.Length; i++)
                        {
                            string Str = props.ToLower();
                            if (ValNum == 0)
                            {
                                repValue = Str;
                            }
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        #endregion
        #region 取得约定搜索条件字符
        public static string GetSQLSearchCondition(string SearchCondition)
        {
            string StrCondition = "";
            string[] ArrCondition = StringUtil.ArrStrList(SearchCondition, "{@}");
            int length = 0;
            if (ArrCondition.Length > 0)
            {
                length = ArrCondition.Length;
            }
            for (int i = 0; i < length; i++)
            {
                string[] Condition = ArrStrList(ArrCondition[i], ',');
                if (Condition != null)
                {
                    string YunSauan = "And";
                    if (Condition.Length == 4)
                    {
                        YunSauan = Condition[3];
                    }
                    if (Condition.Length > 2)
                    {
                        switch (Condition[0])
                        {
                            case "=":
                                StrCondition += " " + YunSauan + " " + Condition[1] + "='" + Condition[2] + "'";
                                break;
                            case ">":
                                StrCondition += " " + YunSauan + " " + Condition[1] + ">'" + Condition[2] + "'";
                                break;
                            case "<":
                                StrCondition += " " + YunSauan + " " + Condition[1] + "<'" + Condition[2] + "'";
                                break;
                            case ">=":
                                StrCondition += " " + YunSauan + " " + Condition[1] + ">='" + Condition[2] + "'";
                                break;
                            case "<=":
                                StrCondition += " " + YunSauan + " " + Condition[1] + "<='" + Condition[2] + "'";
                                break;
                            case "like":
                                StrCondition += " " + YunSauan + " " + Condition[1] + " like '%" + Condition[2] + "%'";
                                break;
                        }
                    }
                }
            }
            return StrCondition;
        }
        #endregion
        #region 替换格式化字符
        public static string ReplaceStrL(string origText, DataTable tb, int RowNum)
        {
            return ReplaceStrL(origText, "{", "}", tb, RowNum);
        }
        public static string ReplaceStrL(string origText, string StrVal, string EndVal, DataTable tb, int RowNum)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(StrVal) >= 0 && origText.IndexOf(EndVal) > 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + StrVal + "(?<pname>\\w{1,})" + EndVal + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string ph = m.Groups["ph"].Value;
                    string pname = m.Groups["pname"].Value.Trim();
                    if (pname.Length > 0)
                    {
                        string repValue = tb.Rows[RowNum][pname].ToString();
                        if (repValue != null)
                            sb.Replace(ph, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }
        #endregion
        #region 根据行取得字符
        public static string GetStrByDataTable(DataTable tb, int RowNum, string Str)
        {
            string StrCondition = "";
            return StrCondition;
        }
        #endregion
        public static string Append_Id(IList alId)
        {
            StringBuilder idStr = new StringBuilder();
            int length = ((ArrayList)alId).Count;
            for (int i = 0; i < length; i++)
            {
                if (i == 0) idStr.Append(alId[i].ToString());
                else idStr.Append("," + alId[i].ToString());
            }
            return idStr.ToString().Trim();
        }
        public static string Join(int[] IdArr, string splitChar)
        {
            StringBuilder idStr = new StringBuilder();
            int length = IdArr.Length;
            for (int i = 0; i < length; i++)
            {
                if (i == 0) idStr.Append(IdArr[i].ToString());
                else idStr.Append(splitChar + IdArr[i].ToString());
            }
            return idStr.ToString().Trim();
        }

        public static string SubstringLength(string str, int length)
        {

            if (str.Length <= length)
                return str;
            string substr = str.Substring(0, length - 3) + "...";

            return substr;

        }

        /// <summary> 
        /// 获取文中图片地址 
        /// </summary> 
        /// <param name="content">内容</param> 
        /// <returns>地址字符串</returns> 
        public static string GetImageUrl(string content)
        {
            int mouse = 0;
            int cat = 0;
            string imageLabel = "";
            string imgSrc = "";
            string[] Attributes;

            do //得到第一张图片的连接作为主要图片 
            {
                cat = content.IndexOf("<img", mouse);
                if (cat == -1)
                    return "";
                mouse = content.IndexOf('>', cat);
                imageLabel = content.Substring(cat, mouse - cat); //图像标签 

                Attributes = imageLabel.Split(' '); //将图片属性分开 

                foreach (string temp_Attributes in Attributes) //得到图片地址属性 
                    if (temp_Attributes.IndexOf("src") >= 0)
                    {
                        imgSrc = temp_Attributes.ToString();
                        break;
                    }
                imgSrc = imgSrc.Substring(imgSrc.IndexOf('"') + 1, imgSrc.LastIndexOf('"') - imgSrc.IndexOf('"') - 1); //丛地址属性中提取地址 

            } while (imgSrc == "" && cat > 0);

            return (imgSrc);
        }

        /// <summary>
        /// 移除字符串末尾指定字符
        /// </summary>
        /// <param name="str">需要移除的字符串</param>
        /// <param name="value">指定字符</param>
        /// <returns>移除后的字符串</returns>
        public static string RemoveLastChar(string str, string value)
        {
            int _finded = str.LastIndexOf(value);
            if (_finded != -1)
            {
                return str.Substring(0, _finded);
            }
            return str;
        }

        /// <summary>
        /// 过滤ＨＴＭＬ
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string CheckHTMLStr(string html)
        {
            Regex regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" no[\s\S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex6 = new Regex(@"\<img[^\>]+\>", RegexOptions.IgnoreCase);
            Regex regex7 = new Regex(@"</p>", RegexOptions.IgnoreCase);
            Regex regex8 = new Regex(@"<p>", RegexOptions.IgnoreCase);
            Regex regex9 = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }

        public static string GetHtm(string origText, string StarStr, string EndStr)
        {
            string Str = origText;
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(StarStr) >= 0)
            {
                if (StarStr.Length > 0)
                {
                    Str = Str.Substring(origText.IndexOf(StarStr) + StarStr.Length, Str.Length - origText.IndexOf(StarStr) - StarStr.Length);
                }
            }

            if (!string.IsNullOrEmpty(Str) && Str.IndexOf(EndStr) >= 0)
            {
                if (EndStr.Length > 0)
                {
                    Str = Str.Substring(0, Str.IndexOf(EndStr));
                }
            }
            return Str;
        }

        /// <summary>
        /// 根据分隔符取数组
        /// </summary>
        /// <param name="Str">输入字符</param>
        /// <param name="val">分隔符</param>
        /// <returns></returns>
        public static List<string> ArrStrToList(string Str, string Chr)
        {
            List<string> list = new List<string>();
            if (Str == null || Str == "")
            {
                return list;
            }
            int chrlen = Chr.Length - 1;
            int sindex = 0;
            int eindex = 0;
            bool isfinded = false;
            while (!isfinded)
            {
                eindex = Str.IndexOf(Chr, sindex);
                if (eindex == -1)
                {//找到最后

                    list.Add(Str.Substring(sindex, Str.Length - sindex));
                    isfinded = true;
                    continue;
                }
                string s = Str.Substring(sindex, eindex - sindex);
                if (s == Chr)
                    s = "";
                list.Add(s);
                sindex = eindex + 1 + chrlen;

            }

            return list;
        }

        /// <summary>
        /// 根据分隔符取数组
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="Chr"></param>
        /// <returns></returns>
        //public static List<string> ArrStrToList(string Str, string[] Chr, string[] length)
        //{
        //    List<string> list = new List<string>();
        //    if (Chr == null)
        //        return list;

        //    if (Chr.Length < 2)
        //    {
        //        list.Add(Str);
        //        return list;
        //    }
        //    List<string> listFirst = ArrStrToList(Str, Chr[1]);
        //    list.Add(listFirst[0]);
        //    string tempStr = Str.Substring((listFirst[0].Length + Chr[1].Length));
        //    for (int i = 2; i < Chr.Length; i++)
        //    {
        //        listFirst = ArrStrToList(tempStr, Chr[i]);
        //        list.Add(listFirst[0]);
        //        tempStr = tempStr.Substring((listFirst[0].Length + Chr[i].Length));
        //    }
        //    list.Add(tempStr);
        //    return list;

        //    if (Chr.Length == 1)
        //    {
        //        return ArrStrToList(Str, Chr[0]);
        //    }
        //    string Sv = Str;
        //    int sindex = 0;
        //    for (int i = 0; i < Chr.Length; i++)
        //    {
        //        string s = i == 0 ? "" : Chr[i - 1];
        //        string l = length[i];
        //        int lg = 0;
        //        if (Common.Strings.Validate.IsNumber(l))
        //        {
        //            lg = Convert.ToInt32(l);
        //        }
               

        //        if (i == Chr.Length - 1)
        //        {
        //            if (s != "")
        //                list.Add(Sv.Replace(s, ""));
        //            else
        //                list.Add(Sv);
        //            break;
        //        }

        //        if (s == "")
        //        {//分隔符为空
        //            if (lg == 0)
        //            {
        //                return null;
        //            }
        //            string v = Str.Substring(sindex, lg);
        //            list.Add(v);
        //            sindex = sindex + lg;
        //            Sv = Str.Substring(sindex);
        //            continue;
        //        }

        //        int d = Str.IndexOf(s, sindex);
        //        if (d < 0)
        //            return null;
        //        try
        //        {
        //            int nextIndex = Str.IndexOf(s, sindex + 1);
        //            string val = Str.Substring(sindex, nextIndex - sindex);
        //            list.Add(val.Replace(s, ""));
        //            sindex = nextIndex;

        //            Sv = Str.Substring(sindex);
        //        }
        //        catch
        //        {
        //            return null;
        //        }

        //    }

        //    return list;

        //}

        private static int IndexOfStr(string Str, string[] Chr)
        {
            for (int i = 0; i < Str.Length; i++)
            {
                foreach (string s in Chr)
                {
                    string v = Str.Substring(i, s.Length);
                    if (s == v)
                    {
                        return i;
                    }

                }
            }

            return 0;
        }

        public static string ReplaceStrL1(string origText)
        {
            //return ReplaceStrL(origText, "[", "]");

            origText = origText.Replace('[', ' ');

            origText = origText.Replace(']', ' ');

            return origText.Trim();

        }
        public static bool IsVail(string str)
        {
            return IsVail(str, "{", "}");
        }
        public static bool IsVail1(string str)
        {
            return IsVail(str, "[", "]");
        }

        public static bool IsVail(string str, string schr, string echar)
        {

            if (str.Length < 1)
                return false;

            if (str.Substring(0, 1) == schr)
            {
                if (str.Substring(str.Length - 1, 1) == echar)
                    return true;
            }

            return false;
        }

        public static string ReplaceStrL(string origText)
        {
            return ReplaceStrL(origText, "{", "}");
        }

        public static string ReplaceStrL(string origText, string StrVal, string EndVal)
        {
            string str = "";
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf(StrVal) >= 0 && origText.IndexOf(EndVal) > 0)
            {
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>" + StrVal + "(?<pname>\\w{1,})" + EndVal + ")", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);

                str = mts[0].Groups["pname"].Value.Trim();

            }

            return str;
        }



        public static string GetFileName(string filename)
        {
            string fullfilename = "";
            //文件名
            string file = System.IO.Path.GetFileNameWithoutExtension(filename);
            //后缀
            string exp = System.IO.Path.GetExtension(filename);
            string path = System.IO.Path.GetDirectoryName(filename);

            fullfilename = filename;
            int index = 1;
            while (true)
            {
                if (System.IO.File.Exists(fullfilename))
                {
                    fullfilename = path + "\\" + file + index.ToString() + exp;
                    index++;
                }
                else
                    return fullfilename;
            }
        }

        /// <summary>
        /// 转换为日期格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ConvertToDateFormat(string val)
        {
            if (Validate.IsDateTimeCommon(val))
                return val;
            if (val.Length == 8)
            {
                string dt = val.Insert(4, "-");
                dt = dt.Insert(7, "-");
                return dt;
            }
            else
            {
                val = val.Replace(".", "-");
                if (Validate.IsDateTime(val))
                    return val;
                else
                    return "";
            }
        }

        public static string ConverToDatetime(string val)
        {
            string[] arr = val.Split('/');
            if (arr.Length == 0 || arr.Length == 1)
            {
                arr = val.Split('-');
            }
            if (arr.Length == 3)
            {
                return val;
            }
            if (arr.Length == 4)
            {
                string[] arr1 = arr[3].Split(' ');
                val = arr[0] + "-" + arr[1] + "-" + arr[2] + " " + arr1[1];

            }

            return val;
        }


        public static string GetMaxStr(List<string> list)
        {
            if (list.Count == 0)
                return "";
            if (list.Count == 1)
                return list[0];

            string temp = list[0];
            foreach (string s in list)
            {
                if (temp == s)
                    continue;
                if (s.Length < temp.Length)
                    continue;
                if (s.Length > temp.Length)
                {
                    temp = s;
                    continue;
                }
                if (s.Length == temp.Length)
                {
                    if (s.CompareTo(temp) == 1)
                    {
                        temp = s;
                    }
                }

            }

            return temp;

        }

        public static string GetLengthStr(string str, int length)
        {
            if (str.Length > length)
            {
                return str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int l = length - str.Length;
                string s = "";
                for (int i = 0; i < l; i++)
                {
                    s += "0";
                }
                return s + str;
            }
            else
            {
                return str;
            }
        }


        public static string GetLengthStr(string str)
        {
            return GetLengthStr(str, 2);
        }


        public static string GetNowTime()
        {
            string time = "";
            time = DateTime.Now.Year.ToString() + "-" + GetLengthStr(DateTime.Now.Month.ToString()) + "-" + GetLengthStr(DateTime.Now.Day.ToString()) + " " + GetLengthStr(DateTime.Now.Hour.ToString()) + ":" + GetLengthStr(DateTime.Now.Minute.ToString()) + ":" + GetLengthStr(DateTime.Now.Second.ToString());
            return time;
        }

        public static string GetNowTime(DateTime dt)
        {
            string time = "";
            time = dt.Year.ToString() + "-" + GetLengthStr(dt.Month.ToString()) + "-" + GetLengthStr(dt.Day.ToString()) + " " + GetLengthStr(dt.Hour.ToString()) + ":" + GetLengthStr(dt.Minute.ToString()) + ":" + GetLengthStr(dt.Second.ToString());
            return time;
        }

        public static string GetNowTimeDayEnd(DateTime dt)
        {
            string time = "";
            time = dt.Year.ToString() + "-" + GetLengthStr(dt.Month.ToString()) + "-" + GetLengthStr(dt.Day.ToString()) + " 23:59:59";
            return time;
        }

        public static string GetNowTimeDayStart(DateTime dt)
        {
            string time = "";
            time = dt.Year.ToString() + "-" + GetLengthStr(dt.Month.ToString()) + "-" + GetLengthStr(dt.Day.ToString()) + " 00:00:00";
            return time;
        }

        /// <summary>
        /// 是否图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsPic(string filename)
        {
            string exname = System.IO.Path.GetExtension(filename);
            if (exname.ToLower() == ".jpg" || exname.ToLower() == ".jpge" || exname.ToLower() == ".png" || exname.ToLower() == ".bmp" || exname.ToLower() == ".tif" || exname.ToLower() == ".tiff" || exname.ToLower() == ".gif" || exname.ToLower() == ".bmp")
                return true;
            return false;
        }


        /// <summary>
        /// 是否视频
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsVedio(string filename)
        {
            string exname = System.IO.Path.GetExtension(filename);
            if (exname.ToLower() == ".wmv" || exname.ToLower() == ".wvx" || exname.ToLower() == ".asf" || exname.ToLower() == ".asx" || exname.ToLower() == ".wm" || exname.ToLower() == ".wmx" || exname.ToLower() == ".wmd" || exname.ToLower() == ".vob" || exname.ToLower() == ".avi" || exname.ToLower() == ".mpeg" || exname.ToLower() == ".mpg" || exname.ToLower() == ".mp2" || exname.ToLower() == ".mp3" || exname.ToLower() == ".mp4" || exname.ToLower() == ".mp5" || exname.ToLower() == ".mov" || exname.ToLower() == ".mkv" || exname.ToLower() == ".rmvb")
                return true;
            return false;
        }


        public static string DelWeek(string time)
        {
            return time.Replace("/星期一", "").Replace("/星期二", "").Replace("/星期三", "").Replace("/星期四", "").Replace("/星期五", "").Replace("/星期六", "").Replace("/星期日", "");
        }

        /// <summary>
        /// 转换日期格式(yyyyMMdd格式)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetDateString(string val)
        {
            if (!Common.Strings.Validate.IsDateTimeCommon(val))
            {
                return val;
            }

            DateTime dateStr = Convert.ToDateTime(val);
            return dateStr.ToString("yyyyMMdd");

        }

        /// <summary>
        /// 计算文件大小（字节单位，转化方便记录的KB，MB，GB）
        /// </summary>
        /// <param name="b">字节数</param>
        /// <returns></returns>
        public static string GetFileSize(int b)
        {
            int kb = b;
            if (kb < 1024)
                return kb.ToString() + "B";
            else if (1024 < kb && kb < (1024 * 1024))
            {
                decimal f = Convert.ToDecimal(kb) / 1024;
                f = decimal.Round(f, 2);
                return f.ToString() + "KB";
            }
            else if ((1024 * 1024) < kb && kb < (1024 * 1024 * 1024))
            {
                decimal f = Convert.ToDecimal(kb) / 1024 / 1024;
                f = decimal.Round(f, 2);
                return f.ToString() + "MB";
            }
            else if (kb > (1024 * 1024 * 1024))
            {
                decimal f = Convert.ToDecimal(kb) / 1024 / 1024 / 1024;
                f = decimal.Round(f, 2);
                return f.ToString() + "GB";
            }

            return kb.ToString() + "KB";
        }


        public static string GetFileSizeForInt(int b)
        {
            Int32 kb = b;
            if (kb < 1024)
                return kb.ToString() + "B";
            else if (1024 < kb && kb < (1024 * 1024))
            {
                decimal f = Convert.ToDecimal(kb) / 1024;
                f = decimal.Round(f, 2);
                return f.ToString() + "KB";
            }
            else if ((1024 * 1024) < kb && kb < (1024 * 1024 * 1024))
            {
                decimal f = Convert.ToDecimal(kb) / 1024 / 1024;
                f = decimal.Round(f, 2);
                return f.ToString() + "MB";
            }
            else if (kb > (1024 * 1024 * 1024))
            {
                decimal f = Convert.ToDecimal(kb) / 1024 / 1024 / 1024;
                f = decimal.Round(f, 2);
                return f.ToString() + "GB";
            }

            return kb.ToString() + "KB";
        }

        public static string GetFileSize(string skb)
        {
            if (skb.IndexOf("KB") == -1)
            {
                return GetFileSize(Convert.ToInt32(skb));
            }
            int kb = Convert.ToInt32(skb.Replace("KB", ""));

            return GetFileSize(kb);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static string ReadHtmlFile(string temp)
        {
            Encoding code = Encoding.GetEncoding("UTF-8");//定义文字编码 UTF-8
            StreamReader sr = null;
            string str = "";
            try
            {
                if (!File.Exists(temp))
                    return "";
                sr = new StreamReader(temp, code);
                str = sr.ReadToEnd(); // 读取文件 
            }
            catch
            {
                return "";
            }
            finally
            {
                sr.Dispose();
                sr.Close();

            }
            return str;
        }

        /// <summary>
        /// 过滤单引号
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string FilterSpecialChr1(string val)
        {

            string sf = "";

            sf = val.Replace("'", "''");


            return sf;

        }
        /// <summary>
        /// 过滤特殊符号
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static string FilterSpecialChr(string chr)
        {
            return chr.Replace("[", "[[]").Replace("%", "[%]").Replace("?", "[?]").Replace("'", "''");
        }

        /// <summary>
        /// 获取下一个编号
        /// </summary>
        /// <param name="nostr"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static string GetNextNO(string nostr, int bit)
        {
            if (Common.Strings.Validate.IsNumber(nostr))
            {
                string result = Convert.ToString(Convert.ToInt32(nostr) + bit);
                if (result.Length < nostr.Length)
                {
                    int length = nostr.Length - result.Length;
                    for (int i = 0; i < length; i++)
                    {
                        result = "0" + result;
                    }
                }
                return result;
            }
            else if (Common.Strings.Validate.IsDecimal(nostr))
            {
                int h = nostr.IndexOf(".");
                return Convert.ToString(Convert.ToDouble(nostr) + GetZero(nostr.Length - h, bit));

            }
            string head = "";
            object back = "";
            int index = 1;
            int id = 0;
            for (int i = nostr.Length - 1; i >= 0; i--)
            {
                string sl = nostr.Substring(i, index);
                string s = nostr.Substring(i, 1);

                if (s == ".")
                    id = i;


                if (!Common.Strings.Validate.IsNumber(s) && s != ".")
                {
                    if (index == 1)
                    {
                        return nostr + bit.ToString();
                    }

                    string hsl = nostr.Substring(i + 1, index - 1);
                    if (Common.Strings.Validate.IsNumber(hsl))
                    {
                        object t = Convert.ToInt32(hsl) + bit;
                        int length = hsl.Length - t.ToString().Length;
                        if (t.ToString().Length < hsl.Length)
                        {
                            for (int k = 0; k < length; k++)
                            {
                                t = "0" + t.ToString();
                            }
                        }

                        head = nostr.Substring(0, i + 1);
                        back = t;
                        break;
                    }
                    else if (Common.Strings.Validate.IsDecimal(hsl))
                    {
                        head = nostr.Substring(0, i + 1);
                        back = Convert.ToDouble(hsl) + GetZero(nostr.Length - id, bit);
                        break;
                    }
                    else
                    {
                        return nostr + bit.ToString();
                    }

                }
                else if (i == 0)
                {

                }



                index++;
            }

            return head.ToString() + back.ToString(); ;
        }
        static double GetZero(int length, int bit)
        {
            string str = "";
            if (length < 1)
                return 0;
            for (int i = 0; i < length - 2; i++)
            {

                str += "0";
            }
            str = "0." + str + "1";


            double t = 0;
            for (int i = 0; i < bit; i++)
            {
                t += Convert.ToDouble(str);
            }

            return t;

        }

        /// <summary>   
        /// 截取文本，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="length">需计算长度的字符串</param>
        /// <returns>string</returns>
        public static string GetSubString(string str, int length)
        {
            string temp = str;
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j > length)
                {
                    return temp.Substring(0, k) + "";
                }
            }
            return temp;
        }

        /// <summary>
        /// 混合文本长度，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetByteCount(str);
        }

        //将秒转成十分秒
        public static String SendAotoHDS(int second)
        {
            int h = 0;
            int d = 0;
            int s = 0;
            int temp = second % 3600;
            if (second > 3600)
            {
                h = second / 3600;
                if (temp != 0)
                {
                    if (temp > 60)
                    {
                        d = temp / 60;
                        if (temp % 60 != 0)
                        {
                            s = temp % 60;
                        }
                    }
                    else
                    {
                        s = temp;
                    }
                }
            }
            else
            {
                d = second / 60;
                if (second % 60 != 0)
                {
                    s = second % 60;
                }
            }
            if (h > 0)
            {
                return h + "时" + d + "分" + s + "秒";
            }
            else if (d > 0)
            {
                return d + "分" + s + "秒";
            }
            else
            {
                return second + "秒";
            }
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public static string GenerateCheckCode()
        {
            int number;
            //char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                number = random.Next(0, 10);

                //if (number % 2 == 0)
                //    code = (char)('0' + (char)(number % 10));
                //else
                //    code = (char)('A' + (char)(number % 26));

                //if (code == '0' || code == 'o' || code == 'O')
                //    code = 'H';

                checkCode += number.ToString();
            }

            return checkCode;
        }



        /// <summary>
        /// 将传入字符串以GZip算法压缩后，返回Base64编码字符
        /// </summary>
        /// <param name="rawString">需要压缩的字符串</param>
        /// <returns>压缩后的Base64编码的字符串</returns>
        public static string GZipCompressString(string rawString)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(rawString.ToString());
                byte[] zippedData = Compress(rawData);
                return (string)(Convert.ToBase64String(zippedData));
            }

        }
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// 将传入的二进制字符串资料以GZip算法解压缩
        /// </summary>
        /// <param name="zippedString">经GZip压缩后的二进制字符串</param>
        /// <returns>原始未压缩字符串</returns>
        public static string GZipDecompressString(string zippedString)
        {
            if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
                return (string)(System.Text.Encoding.UTF8.GetString(Decompress(zippedData)));
            }
        }
        /// <summary>
        /// ZIP解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }
        /// <summary>
        /// 获取排序字符串
        /// </summary>
        /// <param name="sortStrs"></param>
        /// <returns></returns>
        public static string GetSortString(string sortStrs)
        {
            sortStrs = sortStrs.Trim();
            if (sortStrs == "" || sortStrs == null)
                return "";
            string realSortStrs = "";
            string[] arrSorts = Common.Strings.StringUtil.ArrStrList(sortStrs, ',');
            foreach (string str in arrSorts)
            {
                string sorts = str.Trim();
                string[] arrSort = Common.Strings.StringUtil.ArrStrList(sorts, ' ');
                if (arrSort.Length == 2)
                {
                    realSortStrs += arrSort[0] + " " + arrSort[1] + ",";

                }
                else if (arrSort.Length == 3)
                {
                    if (arrSort[2].ToLower() == "txt")
                    {//按照文本排序
                        realSortStrs += arrSort[0] + " " + arrSort[1] + ",";
                    }
                    else
                    { //按照数字排序
                        realSortStrs += "case when  ISNUMERIC(" + arrSort[0] + ")<>0 then cast(" + arrSort[0] + " as float) else dbo.GetFloatAndAscii(" + arrSort[0] + ") end " + arrSort[1] + ",";
                    }
                }
            }
            realSortStrs = realSortStrs.Substring(0, realSortStrs.Length - 1);

            return realSortStrs;
        }
        public static string GetSpecialStr(string val)
        {
            return val.Replace("specialzhongdian1", "·");
        }
        /// <summary>
        /// 获取配置文件的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettiongs(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
            catch (Exception)
            {

                return "";
            }
        }
        
    }
}
