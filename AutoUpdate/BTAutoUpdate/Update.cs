using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AutoUpdate
{
   public class Update
    {

        private static string _timerServiceName = "BT_Test_Timer1";//本地服务名称
        private static string _serviceUrl = "http://192.168.1.113:2019/"; //远程服务地址
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            Utils.LogHelper.WriteLog("------------初始化配置开始---------------");
            //初始化
            //Utils.FTPHelper.ftpip = System.Configuration.ConfigurationManager.AppSettings["FTPServiceUrl"];
            //Utils.FTPHelper.username = System.Configuration.ConfigurationManager.AppSettings["FTPUserName"];
            //Utils.FTPHelper.password = System.Configuration.ConfigurationManager.AppSettings["FTPPassword"];
            //_timerServiceName = System.Configuration.ConfigurationManager.AppSettings["TimerServiceName"];
            _serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceDownUrl"];
            Utils.LogHelper.WriteLog("远程文件下载地址："+ _serviceUrl);
            Utils.LogHelper.WriteLog("------------初始化配置结束---------------");
            // StopTimerService();
        }

        /// <summary>
        /// 执行更新
        /// </summary>
        /// <param name="updateInfo"></param>
        public static UpdateMessage ExecuteUpdate(ReceiveUpdateInfo updateInfo)
        {
            UpdateMessage msg=null;
            if (updateInfo==null)
            {
                Utils.LogHelper.WriteLog("------------没有可更新的文件---------------");
                return msg=new UpdateMessage() { Msg= "没有可更新的文件",UpdateResult=MyEnum.UpdateResultEnum.更新失败 };
            }

            switch (updateInfo.FileType)
            {
                case MyEnum.UpdateFileTypeEnum.站点:
                    //更新站点
                    Utils.LogHelper.WriteLog("------------更新站点[" + updateInfo.Name + "]文件开始---------------");
                    msg = ExecuteUpdateSiteTask(updateInfo.Name, updateInfo.DownFileName, updateInfo.LocalFilePath);
                    Utils.LogHelper.WriteLog("------------更新站点[" + updateInfo.Name + "]文件结束---------------");
                    break;
                case MyEnum.UpdateFileTypeEnum.数据库:
                    break;
                case MyEnum.UpdateFileTypeEnum.定时器:
                    break;
                case MyEnum.UpdateFileTypeEnum.其他:
                    break;
                default:
                    break;
            }

           // Utils.LogHelper.WriteLog("------------停掉定时器---------------");
            //停掉定时器
           // StopTimerService();
            //foreach (var item in updateInfo)
            //{
            //    if(string.IsNullOrEmpty(item.SiteName)|| item.SiteName=="Timer")
            //    {
            //        //更新定时器
            //        Utils.LogHelper.WriteLog("------------更新定时器文件开始---------------");
            //        ExecuteUpdateTimerTask(item.LocalTimerPath, item.DownFileName);
            //        Utils.LogHelper.WriteLog("------------更新定时器文件结束---------------");
            //    }
            //    else
            //    {
            //        //更新站点
            //        Utils.LogHelper.WriteLog("------------更新站点[" + item.SiteName + "]文件开始---------------");
            //        ExecuteUpdateSiteTask(item.SiteName, item.DownFileName);
            //        Utils.LogHelper.WriteLog("------------更新站点[" + item.SiteName + "]文件结束---------------");
            //    }
            //}
            //Utils.LogHelper.WriteLog("------------开启定时器---------------");
            ////开启定时器
            //StartTimerService();
            //Utils.LogHelper.WriteLog("------------任务完成---------------");
            return msg;
        }

        #region 执行需要更新的任务
        /// <summary>
        /// 执行更新站点的任务
        /// </summary>
        /// <param name="siteName">传过来的更新站点名称</param>
        /// <param name="downFileName">要下载的文件名(注:最好跟上后缀)</param>
        private static UpdateMessage ExecuteUpdateSiteTask(string siteName, string downFileName, string filePath)
        {
            var msg = new UpdateMessage();
            try
            {
                //停掉站点
                Utils.IISHelper.StopWebSite(siteName);
                //下载远程文件
                DownUpdateFileForSite(downFileName, filePath);
                //开启站点
                Utils.IISHelper.StartWebSite(siteName);
                msg.Msg = "";
                msg.UpdateResult = MyEnum.UpdateResultEnum.更新成功;
            }
            catch (Exception ex)
            {
                //日志
                Utils.LogHelper.WriteLog("更新站点文件失败:" + ex.ToString());
                msg.Msg = "更新站点文件失败";
                msg.UpdateResult = MyEnum.UpdateResultEnum.更新失败;
            }
            return msg;
        }

        /// <summary>
        /// 执行更新定时器的任务
        /// </summary>
        /// <param name="localTimerPath">本地定时器的路径</param>
        /// <param name="downFileName">要下载的文件名(注:最好跟上后缀)</param>
        private static void ExecuteUpdateTimerTask(string localTimerPath, string downFileName)
        {
            try
            {
                //下载远程文件
                DownUpdateFileForFTP(downFileName, localTimerPath);
            }
            catch (Exception ex)
            {
                //日志
                Utils.LogHelper.WriteLog("更新定时器文件失败:" + ex.ToString());
            }

        }
        #endregion



        #region  下载文件并更新

        /// <summary>
        /// 下载更新文件(注:下载站点上的文件)
        /// </summary>
        /// <param name="downFileName">要下载的文件名(注:最好跟上后缀)</param>
        /// <param name="siteFilePath">站点文件物理路径</param>
        private static  void DownUpdateFileForSite(string downFileName,string siteFilePath)
        {
            try
            {
                var directory = AppDomain.CurrentDomain.BaseDirectory + "Update";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                WebClient client = new WebClient();
                //下载文件
                client.DownloadFile(_serviceUrl + downFileName, directory + "\\" + downFileName);
                Thread.Sleep(500);

                //解压压缩包文件
                Utils.CompressHelper.UnRAR(directory + "\\" + downFileName, directory);      
                //删除压缩包文件
                File.Delete(directory + "\\" + downFileName);
                //递归复制文件
                CopyFolder(directory, siteFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("更新文件失败："+ex.ToString());
            }
        }


        /// <summary>
        /// 下载更新文件(注:下载FTP文件)
        /// </summary>
        /// <param name="downFileName">要下载的文件名(注:最好跟上后缀)</param>
        /// <param name="siteFilePath">站点文件物理路径</param>
        private static void DownUpdateFileForFTP(string downFileName, string siteFilePath)
        {
            try
            {
                //保存的文件路径
                var directory = AppDomain.CurrentDomain.BaseDirectory + "Update";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                Utils.FTPHelper.Download(downFileName, directory);
                Thread.Sleep(500);

                //解压压缩包文件
                Utils.CompressHelper.UnRAR(directory + "\\" + downFileName, directory);
                //删除压缩包文件
                File.Delete(directory + "\\" + downFileName);
                //递归复制文件
                CopyFolder(directory+ "\\" + downFileName.Split('.')[0], siteFilePath);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// 更新文件(复制文件夹中的所有文件夹与文件到另一个文件夹)
        /// </summary>
        /// <param name="sourcePath">源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        private static void CopyFolder(string sourcePath, string destPath)
        {
            if (!Directory.Exists(sourcePath))
                throw new Exception("源目录不存在！");
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                //获得源文件下所有文件
                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    File.Copy(c, destFile, true);//覆盖模式
                });
                //获得源文件下所有目录文件
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //采用递归的方法实现
                    CopyFolder(c, destDir);
                });
            }
            catch (Exception ex)
            {
                throw new Exception("更新文件失败:"+ex.ToString());
            }            
        }
        #endregion

        #region 开启或停止定时器服务
        /// <summary>
        /// 停止定时器服务
        /// </summary>
        private static void StopTimerService()
        {

            try
            {
                System.ServiceProcess.ServiceController control = new System.ServiceProcess.ServiceController(_timerServiceName);
                if (control.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    control.Stop();
                    control.Refresh();
                }
            }
            catch (Exception ex)
            {
                Utils.LogHelper.WriteLog("停止定时器失败:"+ex.ToString());
            }
        }

        /// <summary>
        /// 开启定时器服务
        /// </summary>
        private static void StartTimerService()
        {
            try
            {
                System.ServiceProcess.ServiceController control = new System.ServiceProcess.ServiceController(_timerServiceName);
                if (control.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    control.Start();
                    control.Refresh();
                }
            }
            catch (Exception ex)
            {
                Utils.LogHelper.WriteLog("开启定时器失败:" + ex.ToString());
            }
        }
        #endregion

    }


}
