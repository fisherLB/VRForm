using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEvaluatePlatform.Infrastructure.Common.Files
{
    public class FileUtil
    {
        //储存所有文件夹名
        private ArrayList dirs;

        public FileUtil()
        {
            dirs = new ArrayList();
        }
        /// <summary>
        /// 根据web路径保存图片
        /// </summary>
        /// <param name="url">图片路径</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="method">post,get</param>
        /// <returns></returns>
        public static bool SaveImageForWebUrl(string url, string savePath, string method)
        {
            try
            {
                string strImageURL = url;
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strImageURL);
                webRequest.Method = method;
                System.Net.HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                System.IO.Stream s = webResponse.GetResponseStream();
                List<byte> list = new List<byte>();
                while (true)
                {
                    int data = s.ReadByte();
                    if (data == -1)
                        break;
                    else
                    {
                        byte b = (byte)data;
                        list.Add(b);
                    }
                }
                byte[] bb = list.ToArray();
                System.IO.File.WriteAllBytes(savePath, bb);
                s.Close();
                return true;
            }
            catch
            {
            }
            return false;
        }
        /// <summary>
        /// 根据web路径保存图片
        /// </summary>
        /// <param name="url">图片路径</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="method">post,get</param>
        /// <returns></returns>
        public static byte[] GetImageForWebUrl(string url, string method)
        {
            try
            {
                string strImageURL = url;
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strImageURL);
                webRequest.Method = method;
                System.Net.HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                System.IO.Stream s = webResponse.GetResponseStream();
                List<byte> list = new List<byte>();
                while (true)
                {
                    int data = s.ReadByte();
                    if (data == -1)
                        break;
                    else
                    {
                        byte b = (byte)data;
                        list.Add(b);
                    }
                }
                byte[] bb = list.ToArray();
                return bb;
            }
            catch
            {
            }

            return null; ;
        }
        /// <summary>
        /// 修改文件的属性
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="fileAttributes"></param>
        public static void UpdateAttributes(string filepath, FileAttributes fileAttributes)
        {

            System.IO.File.SetAttributes(filepath, fileAttributes);

        }
        //获取所有文件名
        private ArrayList GetFileName(string dirPath)
        {
            ArrayList list = new ArrayList();

            if (Directory.Exists(dirPath))
            {
                list.AddRange(Directory.GetFiles(dirPath));
            }
            return list;
        }

        //获取所有文件夹及子文件夹
        private void GetDirs(string dirPath)
        {
            if (Directory.GetDirectories(dirPath).Length > 0)
            {
                foreach (string path in Directory.GetDirectories(dirPath))
                {
                    dirs.Add(path);
                    GetDirs(path);
                }
            }
        }

        //获取所有文件夹及子文件夹
        private void GetDirs(string dirPath, ArrayList d)
        {
            if (Directory.GetDirectories(dirPath).Length > 0)
            {
                foreach (string path in Directory.GetDirectories(dirPath))
                {
                    d.Add(path);
                    GetDirs(path);
                }
            }
        }

        //获取所有文件夹及子文件夹
        public ArrayList GetFolders(string dirPath)
        {
            ArrayList dirs = new ArrayList();
            GetDirs(dirPath, dirs);
            return dirs;

        }

        /// <summary>
        /// 获取给出文件夹及其子文件夹下的所有文件名
        /// （文件名为路径加文件名及后缀,
        /// 使用的时候GetAllFileName().ToArray()方法可以转换成object数组
        /// 之后再ToString()分别得到文件名）
        /// </summary>
        /// <param name="rootPath">文件夹根目录</param>
        /// <returns></returns>
        public ArrayList GetAllFileName(string rootPath)
        {
            dirs.Add(rootPath);
            GetDirs(rootPath);
            object[] allDir = dirs.ToArray();

            ArrayList list = new ArrayList();

            foreach (object o in allDir)
            {
                list.AddRange(GetFileName(o.ToString()));
            }

            return list;
        }

        /// <summary>
        /// 获取给出文件夹及其子文件夹下的所有文件名
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public List<string> FileName(string rootPath)
        {
            List<string> list = new List<string>();

            foreach (object o in GetAllFileName(rootPath).ToArray())
            {
                list.Add(o.ToString());
            }
            return list;
        }


        /// <summary>
        /// 获取文件夹下的文件
        /// </summary>
        /// <param name="rootPath">文件夹路劲</param>
        /// <param name="isInFolder">是否包含子文件夹</param>
        /// <returns></returns>
        public List<string> GetFileNames(string rootPath, bool isInFolder)
        {
            if (isInFolder)
                return FileName(rootPath);

            DirectoryInfo mydir = new DirectoryInfo(rootPath);
            FileInfo[] infos = mydir.GetFiles();

            //FileSystemInfo[] infos = mydir.GetFileSystemInfos();
            List<string> list = new List<string>();

            foreach (FileInfo fsi in infos)
            {
                list.Add(fsi.FullName);
            }

            return list;
        }

        /// <summary>
        /// 获取目录下文件列表，并按照创建时间排序
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileInfo[] GetFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles();
            FileComparer fc = new FileComparer();
            Array.Sort(files, fc);
            return files;
        }

        /// <summary>
        /// 获取目录，并按目录创建时间排序
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DirectoryInfo[] GetDirsInfoSort(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirComparer fc = new DirComparer();
            DirectoryInfo[] files = di.GetDirectories();
            Array.Sort(files, fc);
            return files;
        }

        public List<string> GetDirsSort(string path)
        {
            DirectoryInfo[] dirs = GetDirsInfoSort(path);
            List<string> list = new List<string>();
            foreach (DirectoryInfo d in dirs)
            {
                list.Add(d.Name);
            }

            return list;
        }

        /// <summary>
        /// 获取文件的MD5特征码
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetMD5(string file)
        {
            string result = string.Empty;
            if (!File.Exists(file)) return result;

            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                HashAlgorithm algorithm = MD5.Create();
                byte[] hashBytes = algorithm.ComputeHash(fs);
                result = BitConverter.ToString(hashBytes).Replace("-", "");
            }
            return result;
        }
        /// <summary>
        /// 获取多个文件的MD5特征码
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<string> GetMD5(List<string> files)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < files.Count; i++)
            {
                result.Add(GetMD5(files[i]));
            }
            return result;
        }

        /*******************梁世任-文件操作类-开始****************************/
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="strServerPath"></param>
        /// <param name="strFolderPath"></param>
        /// <param name="strFolder"></param>
        /// <returns></returns>
        public static OperationResult CreateFolder(string strServerPath, string strFolderPath, string strFolder)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            if (!Directory.Exists(strFolderPath + strFolder))
            {
                try
                {
                    Directory.CreateDirectory(strFolderPath + strFolder);
                }
                catch (Exception e)
                {
                    ret.ResultType = OperationResultType.Error;
                    ret.Message = e.Message;
                    return ret;
                }
            }
            return ret;
        }
        /// <summary>
        /// 上传保存附件
        /// </summary>
        /// <returns>返回的值包含：filepath,filename</returns>
        public static OperationResult SaveFiles(System.Web.HttpFileCollectionBase files, string strFolderPath, string strFolder)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            for (int i = 0; i < files.Count; i++)
            {
                System.Web.HttpPostedFileBase file = files[i];
                //重新组合文件名
                if (string.IsNullOrEmpty(file.FileName) || file.FileName == "")
                {
                    ret.ResultType = OperationResultType.Error;
                    ret.Message = "请选择要导入的文件。";
                    break;
                }
                string strExtension = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                string fileName = DateTime.Now.ToString("yyMMddhhmmssfff") + Convert.ToString((new Random()).Next(100, 999)) + strExtension;
                try
                {
                    string filePath = strFolderPath + strFolder + "/" + fileName;
                    file.SaveAs(filePath);
                    ret.Message = filePath;
                    ret.AppendData = file.FileName;
                    ret.ResultType = OperationResultType.Success;
                    //  FILENAME = file.FileName;
                }
                catch (Exception e)
                {
                    ret.ResultType = OperationResultType.Error;
                    ret.Message += e.Message;
                    break;
                }
            }
            return ret;
        }

        public static OperationResult SaveFiles(System.Web.HttpFileCollectionBase files, string filepath)
        {
            OperationResult ret = new OperationResult(OperationResultType.Success);
            for (int i = 0; i < files.Count; i++)
            {
                System.Web.HttpPostedFileBase file = files[i];
                //重新组合文件名
                if (string.IsNullOrEmpty(file.FileName) || file.FileName == "")
                {
                    ret.ResultType = OperationResultType.Error;
                    ret.Message = "请选择要导入的文件。";
                    break;
                }
                string strExtension = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                string fileName =Guid.NewGuid()+ strExtension;
                try
                {
                    string FilePath = filepath + "/" + fileName;
                    file.SaveAs(FilePath);
                    ret.Message = FilePath;
                    ret.AppendData = file.FileName;
                    ret.ResultType = OperationResultType.Success;               
                }
                catch (Exception e)
                {
                    ret.ResultType = OperationResultType.Error;
                    ret.Message += e.Message;
                    break;
                }
            }
            return ret;
        }


        /*******************梁世任-文件操作类-结束****************************/

    }

    public class FileComparer : IComparer
    {
        int IComparer.Compare(Object o1, Object o2)
        {
            FileInfo fi1 = o1 as FileInfo;
            FileInfo fi2 = o2 as FileInfo;
            return fi1.CreationTime.CompareTo(fi2.CreationTime);
        }
    }

    public class DirComparer : IComparer
    {
        int IComparer.Compare(Object o1, Object o2)
        {
            DirectoryInfo fi1 = o1 as DirectoryInfo;
            DirectoryInfo fi2 = o2 as DirectoryInfo;
            return fi1.CreationTime.CompareTo(fi2.CreationTime);
        }
    }


}

