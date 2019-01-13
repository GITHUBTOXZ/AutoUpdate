using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Down.Tool
{
    /// <summary>
    ///日志帮助类
    /// </summary>
   public class LogHelper
    {
       //日志文件夹路径
       private static string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Log";
       //日志文件路径
       private static string filePath = folderPath+"//"+DateTime.Now.ToString("yyyy-MM-dd")+".txt";

       private static object obj=new object();

       /// <summary>
       /// 添加日志
       /// </summary>
       /// <param name="msg">日志消息</param>
       public static void AppendLog(string msg)
       {
           if (!Directory.Exists(folderPath))//如果不存在就创建file文件夹 
           {
               Directory.CreateDirectory(folderPath);
           }
           StringBuilder builder = new StringBuilder();
           builder.Append(DateTime.Now);
           builder.Append("\r\n");
           builder.Append(msg);
           builder.Append("\r\n");
           builder.Append("\r\n");
           builder.Append("-------------------------------------------");
          
           lock (obj)
           {
               if (!File.Exists(filePath))
               {
                   using (FileStream sr = new FileStream(filePath, FileMode.Create))
                   {
                       using (StreamWriter wr = new StreamWriter(sr))
                       {
                           wr.WriteLine(builder.ToString());
                       }
                   }
                   
               }
               else
               {
                   using (FileStream sr = new FileStream(filePath, FileMode.Append))
                   {
                       using (StreamWriter wr = new StreamWriter(sr))
                       {
                           wr.WriteLine(builder.ToString());
                       }
                   }

               }
           }
       }
    }
}
