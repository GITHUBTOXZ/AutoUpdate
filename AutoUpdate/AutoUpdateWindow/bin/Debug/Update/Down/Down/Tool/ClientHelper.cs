using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Down.Tool
{
    /// <summary>
    /// 通讯帮助类
    /// </summary>
   public class ClientHelper
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

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns>文件名称</returns>
       public static void DownloadFile()
       {

           if (!Directory.Exists(_FileSavePath))
           {
               Directory.CreateDirectory(_FileSavePath);
           }
           using (WebClient webClient = new WebClient())
           { 
               //开始下载        
               var _savePath=_FileSavePath + "\\" + _FileName;
               webClient.DownloadFile(_ServiceHost, _savePath);
           }
       }       
    }
}
