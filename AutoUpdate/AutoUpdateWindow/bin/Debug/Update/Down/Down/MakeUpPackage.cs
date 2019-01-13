using Down.Tool;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Down
{
    public partial class MakeUpPackage : Form
    {
        /// <summary>
        /// 服务请求地址
        /// </summary>
        private static string _ServiceHost = System.Configuration.ConfigurationManager.AppSettings["ServiceHost"];
        /// <summary>
        /// 文件保存地址
        /// </summary>
        private static string _FileSavePath = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"];
        /// <summary>
        /// 文件名
        /// </summary>
        private static string _FileName = System.Configuration.ConfigurationManager.AppSettings["FileName"];

        //ZIP密钥
        private static string _Secretkey = System.Configuration.ConfigurationManager.AppSettings["FileName"];
        public MakeUpPackage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 制作更新包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void make_btn_Click(object sender, EventArgs e)
        {       
            try
            {
                ZipHelper.Compress(_ServiceHost + _FileName, _ServiceHost, _FileName, _Secretkey);
                MessageBox.Show("制作更新包成功");
            }
            catch (Exception ex)
            {
                LogHelper.AppendLog("制作更新包失败：" + ex.ToString());
                MessageBox.Show("制作更新包失败");
            }
          
         
        }

        /// <summary>
        ///下载服务上的更新文件
        /// </summary>
        private void  downServiceFile()
        {
            ClientHelper.DownloadFile();
        }

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browse_btn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //_makePath = dialog.SelectedPath;
                //makePath.Text = _makePath;
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
