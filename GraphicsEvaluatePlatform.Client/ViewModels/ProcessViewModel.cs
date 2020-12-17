using GraphicsEvaluatePlatform.Client.Repository;
using GraphicsEvaluatePlatform.Client.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;
using GraphicsEvaluatePlatform.Client.Models;
using System.Windows.Forms;
using GraphicsEvaluatePlatform.Client.Common;
using System.Windows.Media.Imaging;
using WPFBinarizationLib;
using System.IO;
using GraphicsEvaluatePlatform.Infrastructure.Common;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.ViewModels
{
    public class ProcessViewModel : NotificationObject
    {
        private Process _model;

        public Process Model
        {
            get { return _model; }
            set
            {
                _model = value;
                this.RaisePropertyChanged("Model");
            }
        }
        private BitmapSource _imagesource;
        public BitmapSource ImageSource
        {
            get
            {
                return _imagesource;
            }
            set
            {
                _imagesource = value;
                this.RaisePropertyChanged("ImageSource");
            }
        }
        private BitmapSource _imagetarget;
        public BitmapSource ImageTarget
        {
            get
            {
                return _imagetarget;
            }
            set
            {
                _imagetarget = value;
                this.RaisePropertyChanged("ImageTarget");
            }
        }
        String[] Images = null;
        private int CurIndex = 0;
        private string _name;
        private string _path;
        private string _size;
        private string _newSize;
        private string _type;
        private string _resolution;
        private string _rotation;
        private string _water;
        private string _height;
        private string _width;
        private string _newName;
        private string _newType;
        private string _newWater;
        private string _skew;
        private string _newSkew;
        private string _constrast;
        private string _bright;
        /// <summary>
        /// 图像名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value; RaisePropertyChanged("Name");
            }
        }
        /// <summary>
        /// 图像路径
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                RaisePropertyChanged("Path");
            }
        }
        /// <summary>
        /// 图像大小
        /// </summary>
        public string Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value; RaisePropertyChanged("Size");
            }
        }
        public string NewSize
        {
            get
            {
                return _newSize;
            }
            set
            {
                _newSize = value; RaisePropertyChanged("NewSize");
            }
        }
        /// <summary>
        /// 图像格式
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value; RaisePropertyChanged("Type");
            }
        }
        /// <summary>
        /// 图像分辨率
        /// </summary>
        public string Resolution
        {
            get
            {
                return _resolution;
            }
            set
            {
                _resolution = value; RaisePropertyChanged("Resolution");
            }
        }
        /// <summary>
        /// 图像偏斜角
        /// </summary>
        public string Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value; RaisePropertyChanged("Rotation");
            }
        }
        /// <summary>
        /// 水印内容
        /// </summary>
        public string Water
        {
            get { return _water; }
            set
            {
                _water = value; RaisePropertyChanged("Water");
            }
        }
        public string NewWater
        {
            get { return _newWater; }
            set
            {
                _newWater = value; RaisePropertyChanged("NewWater");
            }
        }
        public string Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaisePropertyChanged("Height");
            }
        }
        public string Width
        {
            get { return _width; }
            set
            {
                _width = value;
                RaisePropertyChanged("Width");
            }
        }
        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                RaisePropertyChanged("NewName");
            }
        }
        public string NewType
        {
            get { return _newType; }
            set
            {
                _newType = value;
                RaisePropertyChanged("NewType");
            }
        }
        public string Skew
        {
            get { return _skew; }
            set
            {
                _skew = value;
                RaisePropertyChanged("Skew");
            }
        }
        public string NewSkew
        {
            get { return _newSkew; }
            set
            {
                _newSkew = value;
                RaisePropertyChanged("NewSkew");
            }
        }
        public string Constrast
        {
            get { return _constrast; }
            set
            {
                _constrast = value;
                RaisePropertyChanged("Constrast");
            }
        }
        public string Bright
        {
            get { return _bright; }
            set
            {
                _bright = value;
                RaisePropertyChanged("Bright");
            }
        }
        private readonly IProcessService ProcessService;
        private Page Page { get; set; }

        /// <summary>
        ///构造函数
        /// </summary>
        public ProcessViewModel(object page)
        {
            ProcessService = new ProcessService();
            this.FileOpenCommand = new DelegateCommand(new Action(this.FileOpen));//打开目录
            this.SearchCommand = new DelegateCommand(new Action(this.List));//查询
            this.SaveCommand = new DelegateCommand(new Action(this.Add));//新增
            this.UpdateCommand = new DelegateCommand(new Action(this.Edit));//修改
            this.DelCommand = new DelegateCommand(new Action(this.Del));//删除
            this.PreCommand = new DelegateCommand(new Action(this.Pre));//删除
            this.NextCommand = new DelegateCommand(new Action(this.Next));//删除
            this.BrightCommand = new DelegateCommand(new Action(this.SetBright));
            this.ConstrastCommand = new DelegateCommand(new Action(this.SetContrast));
            this.Page = (Page)page;

        }

        private void FileOpen()
        {
            // 文件夹选择对话框  
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {

                // 不显示创建新文件夹按钮  
                ShowNewFolderButton = false
            };

            // 设置初始目录  
            //dialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;

            // 获取用户选择文件夹  
            if (dialog.ShowDialog() == DialogResult.OK)
            {   // 显示用户选择文件夹  

                // 获取所有图像文件列表  
                Images = Folder.GetImages(dialog.SelectedPath, SearchOption.TopDirectoryOnly);
                if (Images != null)
                {
                    InitImage(Images[0]);
                }
            }
        }
        private void InitImage(string filePath)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(filePath));

            FileInfo file = new FileInfo(filePath);
            Name = file.Name;
            NewName= file.Name;
            Path = filePath;
            Type = file.Extension;
            NewType = file.Extension.ToUpper().Trim();
            Size = (file.Length / 1024) + " KB";
            NewSize=(file.Length / 1024) + " KB";
            Width = bitmap.PixelWidth.ToString();
            Height = bitmap.PixelHeight.ToString();
            Resolution = Width + " * " + Height + " 像素";
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(filePath);
            Rotation = bmp.GetSkewAngle().ToString();
            NewSkew=bmp.GetSkewAngle().ToString();
            Water = "";
            NewWater = "";
            Constrast = "0";
            Bright = "0";
            /**
             * 水印
             * 
             * **/
            //取水印内容
            string str_pathtxt = Application.StartupPath + "\\W" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            if (!File.Exists(str_pathtxt))
            {
                FileStream fs1 = new FileStream(str_pathtxt, FileMode.Create, FileAccess.Write);//创建写入文件 
                fs1.Close();
            }
            LSB_Decrypt decrypt = new LSB_Decrypt(Path, str_pathtxt);
            if (decrypt.ExecuteDecrypt())
            {
                StreamReader sr = new StreamReader(str_pathtxt, Encoding.UTF8);
                Water= sr.ReadLine().ToString();
                NewWater = Water;
                sr.Close();
                File.Delete(str_pathtxt);//删除txt
            }


            ImageSource = bitmap;
            ImageTarget = ImageSource.ToGrayBitmap();
        }

        public DelegateCommand FileOpenCommand { get; set; }

        /// <summary>
        ///  取数据集合
        /// </summary>
        public DelegateCommand GetListCommand { get; set; }

        /// <summary>
        ///  取数据集合
        /// </summary>
        public DelegateCommand DetailCommand { get; set; }

        /// <summary>
        /// 保存命令
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// 修改命令
        /// </summary>
        public DelegateCommand UpdateCommand { get; set; }

        /// <summary>
        /// 删除命令
        /// </summary>
        public DelegateCommand DelCommand { get; set; }

        /// <summary>
        /// 查询命令
        /// </summary>
        public DelegateCommand SearchCommand { get; set; }

        /// <summary>
        ///  取数据明细
        /// </summary>
        public DelegateCommand PreCommand { get; set; }
        public DelegateCommand NextCommand { get; set; }

        public DelegateCommand ConstrastCommand { get; set; }

    public DelegateCommand BrightCommand { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        private void List()
        {
            MessageBox.Show("List....");
        }

        /// <summary>
        /// 明细
        /// </summary>
        private void Detail()
        {
            MessageBox.Show("Detail....");
            return;
        }

        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            MessageBox.Show("Add....");
            return;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        private void Edit()
        {
            MessageBox.Show("Edit....");
            return;
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Del()
        {
            MessageBox.Show("Del....");
            return;
        }
        /// <summary>
        /// 切换到上一张图像
        /// </summary>
        private void Pre()
        {
            int index = CurIndex - 1;
            if (index>=0&& Images[index] != null)
            {
                InitImage(Images[index]);
                CurIndex--;
            }
            else
            {
                MessageBox.Show("没有图片了！");
            }
        }
        /// <summary>
        /// 切换到下一张图像
        /// </summary>
        private void Next( )
        {
            int index = CurIndex + 1;
            if (Images[index] != null)
            {
                InitImage(Images[index]);
                CurIndex++;
            }
            else
            {
                MessageBox.Show("没有图片了！");
            }
        }
        /// <summary>
        /// 调整偏斜角
        /// </summary>
        private void SetSkew()
        {
            MessageBox.Show("正在调整偏斜角！");
        }
        private void SetSize()
        {
            MessageBox.Show("正在调整大小！");
        }
        private void SetBright()
        {

            MessageBox.Show("正在调整亮度！");
        }
        private void SetContrast()
        {
            this.Constrast = Constrast;
            MessageBox.Show("正在调整对比度！");
        }
    }
}
