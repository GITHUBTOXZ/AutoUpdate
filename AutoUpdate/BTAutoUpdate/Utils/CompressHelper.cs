using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate.Utils
{
    /// <summary>
    /// 压缩文件帮助类
    /// </summary>
   public class CompressHelper
    {
        #region  压缩,解压ZIP文件
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="fileName">创建的zip文件名</param>
        /// <param name="sourcePath">源目录路径</param>
        public static void Compress(string fileName, string sourcePath)
        {
            //生成的压缩文件为test.zip
            using (FileStream fsOut = File.Create(fileName + ".zip"))
            {
                //ZipOutputStream类的构造函数需要一个流，文件流、内存流都可以，压缩后的内容会写入到这个流中。
                using (ZipOutputStream zipStream = new ZipOutputStream(fsOut))
                {
                    //准备把sourcePath文件添加到压缩包中。
                    FileInfo fi = new FileInfo(sourcePath);
                    //entryName就是压缩包中文件的名称。
                    string entryName = "update";
                    //ZipEntry类代表了一个压缩包中的一个项，可以是一个文件，也可以是一个目录。
                    ZipEntry newEntry = new ZipEntry(entryName);
                    newEntry.DateTime = fi.LastWriteTime;
                    newEntry.Size = fi.Length;
                    //把压缩项的信息添加到ZipOutputStream中。
                    zipStream.PutNextEntry(newEntry);
                    byte[] buffer = new byte[4096];
                    //把需要压缩文件以文件流的方式复制到ZipOutputStream中。

                    using (FileStream streamReader = File.OpenRead(fileName))
                    {
                        StreamUtils.Copy(streamReader, zipStream, buffer);
                    }
                    zipStream.CloseEntry();
                    //添加多个文件
                    //如果要压缩一个文件夹，就是通过遍历添加文件夹下所有的文件
                    string fileName2 = @"G:\share\web.dll";
                    FileInfo fi2 = new FileInfo(fileName2);

                    //文件在压缩包中的路径
                    string entryName2 = "share\\web.dll";
                    ZipEntry newEntry2 = new ZipEntry(entryName2);
                    newEntry2.DateTime = fi2.LastWriteTime;
                    newEntry2.Size = fi2.Length;
                    zipStream.PutNextEntry(newEntry2);
                    byte[] buffer2 = new byte[4096];
                    using (FileStream streamReader = File.OpenRead(fileName2))
                    {
                        StreamUtils.Copy(streamReader, zipStream, buffer2);
                    }
                    zipStream.CloseEntry();
                    //使用流操作时一定要设置IsStreamOwner为false。否则很容易发生在文件流关闭后的异常。
                    zipStream.IsStreamOwner = false;
                    zipStream.Finish();
                    zipStream.Close();
                }
            }
        }


        /// <summary>
        /// ZIP:解压一个zip文件
        /// add yuangang by 2016-06-13
        /// </summary>
        /// <param name="ZipFile">需要解压的Zip文件（绝对路径）</param>
        /// <param name="TargetDirectory">解压到的目录</param>
        /// <param name="Password">解压密码</param>
        /// <param name="OverWrite">是否覆盖已存在的文件</param>
        public static void Decompress(string ZipFile, string TargetDirectory, bool OverWrite = true, string Password="")
        {
            //如果解压到的目录不存在，则报错
            if (!System.IO.Directory.Exists(TargetDirectory))
            {
                throw new System.IO.FileNotFoundException("指定的目录: " + TargetDirectory + " 不存在!");
            }
            //目录结尾
            if (!TargetDirectory.EndsWith("\\")) { TargetDirectory = TargetDirectory + "\\"; }

            using (ZipInputStream zipfiles = new ZipInputStream(File.OpenRead(ZipFile)))
            {
                if (!string.IsNullOrEmpty(Password))
                    zipfiles.Password = Password;
                ZipEntry theEntry;

                while ((theEntry = zipfiles.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(TargetDirectory + directoryName);

                    if (fileName != "")
                    {
                        if ((File.Exists(TargetDirectory + directoryName + fileName) && OverWrite) || (!File.Exists(TargetDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = File.Create(TargetDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = zipfiles.Read(data, 0, data.Length);

                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }

                zipfiles.Close();
            }
        }

        #endregion

        #region 压缩,解压rar文件
        /// <summary>  
        /// 获取WinRAR.exe路径  
        /// </summary>  
        /// <returns>为空则表示未安装WinRAR</returns>  
        public static string ExistsRAR()
        {
            RegistryKey regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
            //RegistryKey regkey = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRAR.exe\shell\open\command");  
            string strkey = regkey.GetValue("").ToString();
            regkey.Close();
            //return strkey.Substring(1, strkey.Length - 7);  
            return strkey;
        }

        /// <summary>  
        /// 解压RAR文件  
        /// </summary>  
        /// <param name="rarFilePath">要解压的文件路径</param>  
        /// <param name="unrarDestPath">解压路径（绝对路径）</param>  
        public static void UnRAR(string rarFilePath, string unrarDestPath)
        {
            string rarexe = ExistsRAR();
            if (String.IsNullOrEmpty(rarexe))
            {
                throw new Exception("未安装WinRAR程序");
            }
            try
            {
                //组合出需要shell的完整格式  
                string shellArguments = string.Format("x -o+ \"{0}\" \"{1}\\\"", rarFilePath, unrarDestPath);

                //用Process调用  
                using (Process unrar = new Process())
                {
                    ProcessStartInfo startinfo = new ProcessStartInfo();
                    startinfo.FileName = rarexe;
                    startinfo.Arguments = shellArguments;               //设置命令参数  
                    startinfo.WindowStyle = ProcessWindowStyle.Hidden;  //隐藏 WinRAR 窗口  

                    unrar.StartInfo = startinfo;
                    unrar.Start();
                    unrar.WaitForExit();//等待解压完成  

                    unrar.Close();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>  
        ///  压缩为RAR文件  
        /// </summary>  
        /// <param name="filePath">要压缩的文件路径（绝对路径）</param>  
        /// <param name="rarfilePath">压缩到的路径（绝对路径）</param>  
        public static void RAR(string filePath, string rarfilePath, string otherPara = "")
        {
            RAR(filePath, rarfilePath, "", "", otherPara);
        }

        /// <summary>  
        ///  压缩为RAR文件  
        /// </summary>  
        /// <param name="filePath">要压缩的文件路径（绝对路径）</param>  
        /// <param name="rarfilePath">压缩到的路径（绝对路径）</param>  
        /// <param name="rarName">压缩后压缩包名称</param>  
        public static void RAR(string filePath, string rarfilePath, string rarName, string otherPara = "")
        {
            RAR(filePath, rarfilePath, "", rarName, otherPara);
        }

        /// <summary>  
        ///  压缩为RAR文件  
        /// </summary>  
        /// <param name="filePath">要压缩的文件路径（绝对路径）</param>  
        /// <param name="rarfilePath">压缩到的路径（绝对路径）</param>  
        /// <param name="rarName">压缩后压缩包名称</param>  
        /// <param name="password">解压密钥</param>  
        public static void RAR(string filePath, string rarfilePath, string password, string rarName, string otherPara = "")
        {
            string rarexe = ExistsRAR();
            if (String.IsNullOrEmpty(rarexe))
            {
                throw new Exception("未安装WinRAR程序。");
            }

            if (!Directory.Exists(filePath))
            {
                //throw new Exception("文件不存在！");  
            }

            if (String.IsNullOrEmpty(rarName))
            {
                rarName = Path.GetFileNameWithoutExtension(filePath) + ".rar";
            }
            else
            {
                if (Path.GetExtension(rarName).ToLower() != ".rar")
                {
                    rarName += ".rar";
                }
            }

            try
            {
                //Directory.CreateDirectory(rarfilePath);  
                //压缩命令，相当于在要压缩的文件夹(path)上点右键->WinRAR->添加到压缩文件->输入压缩文件名(rarName)  
                string shellArguments;
                if (String.IsNullOrEmpty(password))
                {
                    shellArguments = string.Format("a -ep1 \"{0}\" \"{1}\" -r", rarName, filePath);
                }
                else
                {
                    shellArguments = string.Format("a -ep1 \"{0}\" \"{1}\" -r -p\"{2}\"", rarName, filePath, password);
                }
                if (!string.IsNullOrEmpty(otherPara))
                {
                    shellArguments = shellArguments + " " + otherPara;
                }

                using (Process rar = new Process())
                {
                    ProcessStartInfo startinfo = new ProcessStartInfo();
                    startinfo.FileName = rarexe;
                    startinfo.Arguments = shellArguments;               //设置命令参数  
                    startinfo.WindowStyle = ProcessWindowStyle.Hidden;  //隐藏 WinRAR 窗口  
                    startinfo.WorkingDirectory = rarfilePath;

                    rar.StartInfo = startinfo;
                    rar.Start();
                    rar.WaitForExit(); //无限期等待进程 winrar.exe 退出  
                    rar.Close();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
