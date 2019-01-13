using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate.Utils
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
   public static class LogHelper
    {
        private static object lockObj = new object();

        /// <summary>
        /// 创建txt日志
        /// </summary>
        /// <param name="content">内容</param>
        public static void WriteLog(string content)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\UpdateLog";
            string fileNameUrl = string.Format("{0}\\{1}.txt", path, DateTime.Now.ToString("yyyyMMdd"));
            string Log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + content + "\r\n";
            // 创建文件夹，如果不存在就创建file文件夹
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            lock (lockObj)
            {
                // 判断文件是否存在，不存在则创建
                if (!System.IO.File.Exists(fileNameUrl))
                {
                    // 创建写入文件
                    using (FileStream fs = new FileStream(fileNameUrl, FileMode.Create, FileAccess.Write))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(Log);// 开始写入值
                        sw.Close();
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(fileNameUrl, FileMode.Append, FileAccess.Write))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(Log);// 开始写入值
                        sw.Close();
                    }
                }
            }
        }
    }
}
