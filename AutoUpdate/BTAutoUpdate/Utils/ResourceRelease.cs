using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoUpdate.Utils
{
    /// <summary>
    /// 资源释放处理
    /// </summary>
   public static class ResourceRelease
    {

        [DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private const int OF_READWRITE = 2;
        private const int OF_SHARE_DENY_NONE = 0x40;
        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 文件是否占用
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileUsing(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                return true;
            }
            CloseHandle(vHandle);
            return false;
        }

        /// <summary>
        /// 获取指定文件或目录中存在的(关联的)运行进程信息，以便后面可以解除占用
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static Dictionary<int, string> GetRunProcessInfos(string filePath)
        {

            Dictionary<int, string> runProcInfos = new Dictionary<int, string>();
            string fileName = Path.GetFileName(filePath);
            var fileRunProcs = Process.GetProcessesByName(fileName);
            if (fileRunProcs != null && fileRunProcs.Count() > 0)
            {
                runProcInfos = fileRunProcs.ToDictionary(p => p.Id, p => p.ProcessName);
                return runProcInfos;
            }

            string fileDirName = Path.GetDirectoryName(filePath); //查询指定路径下的运行的进程
            Process startProcess = new Process();
            startProcess.StartInfo.FileName = RelaseAndGetHandleExePath();
            startProcess.StartInfo.Arguments = string.Format("\"{0}\"", fileDirName);
            startProcess.StartInfo.UseShellExecute = false;
            startProcess.StartInfo.RedirectStandardInput = false;
            startProcess.StartInfo.RedirectStandardOutput = true;
            startProcess.StartInfo.CreateNoWindow = true;
            startProcess.StartInfo.StandardOutputEncoding = ASCIIEncoding.UTF8;
            startProcess.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data) && e.Data.IndexOf("pid:", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    //var regex = new System.Text.RegularExpressions.Regex(@"(^[\w\.\?\u4E00-\u9FA5]+)\s+pid:\s*(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    var regex = new System.Text.RegularExpressions.Regex(@"(^.+(?=pid:))\bpid:\s+(\d+)\s+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (regex.IsMatch(e.Data))
                    {
                        var mathedResult = regex.Match(e.Data);

                        int procId = int.Parse(mathedResult.Groups[2].Value);
                        string procFileName = mathedResult.Groups[1].Value.Trim();

                        if ("explorer.exe".Equals(procFileName, StringComparison.OrdinalIgnoreCase))
                        {
                            return;
                        }

                        //var regex2 = new System.Text.RegularExpressions.Regex(string.Format(@"\b{0}.*$", fileDirName.Replace(@"\", @"\\").Replace("?",@"\?")), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        var regex2 = new System.Text.RegularExpressions.Regex(@"\b\w{1}:.+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        string procFilePath = (regex2.Match(e.Data).Value ?? "").Trim();

                        if (filePath.Equals(procFilePath, StringComparison.OrdinalIgnoreCase) || filePath.Equals(PathJoin(procFilePath, procFileName), StringComparison.OrdinalIgnoreCase))
                        {
                            runProcInfos[procId] = procFileName;
                        }
                        else //如果乱码，则进行特殊的比对
                        {
                            if (procFilePath.Contains("?") || procFileName.Contains("?")) //?乱码比对逻辑
                            {
                                var regex3 = new System.Text.RegularExpressions.Regex(procFilePath.Replace(@"\", @"\\").Replace(".", @"\.").Replace("?", ".{1}"), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                if (regex3.IsMatch(filePath))
                                {
                                    runProcInfos[procId] = procFileName;
                                }
                                else
                                {
                                    string tempProcFilePath = PathJoin(procFilePath, procFileName);

                                    regex3 = new System.Text.RegularExpressions.Regex(tempProcFilePath.Replace(@"\", @"\\").Replace(".", @"\.").Replace("?", ".{1}"), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                    if (regex3.IsMatch(filePath))
                                    {
                                        runProcInfos[procId] = procFileName;
                                    }
                                }
                            }
                            else if (procFilePath.Length == filePath.Length || PathJoin(procFilePath, procFileName).Length == filePath.Length) //其它乱码比对逻辑，仅比对长度，如果相同交由用户判断
                            {
                                //发现文件:{可能被一个进程占用,需要强制终止该进程
                                 runProcInfos[procId] = procFileName;
                                
                            }
                        }
                    }
                }
            };

            startProcess.Start();
            startProcess.BeginOutputReadLine();
            startProcess.WaitForExit();

            return runProcInfos;
        }


        private static string RelaseAndGetHandleExePath()
        {
            var handleInfo = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SysUpdate\\handle.exe");
            if (!File.Exists(handleInfo.FullName))
            {
                if (!Directory.Exists(handleInfo.DirectoryName))
                {
                    Directory.CreateDirectory(handleInfo.DirectoryName);
                }

                //byte[] handleExeData = Properties.Resources.handle;
                //File.WriteAllBytes(handleInfo.FullName, handleExeData);

                var handleProc = Process.Start(handleInfo.FullName);//若第一次，则弹出提示框，需要点击agree同意才行
                handleProc.WaitForExit();
            }

            return handleInfo.FullName;
        }

        /// <summary>
        /// 拼接路径（不过滤殊字符）
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        private static string PathJoin(params string[] paths)
        {
            if (paths == null || paths.Length <= 0)
            {
                return string.Empty;
            }

            string newPath = paths[0];

            for (int i = 1; i < paths.Length; i++)
            {
                if (!newPath.EndsWith("\\"))
                {
                    newPath += "\\";
                }

                if (paths[i].StartsWith("\\"))
                {
                    paths[i] = paths[i].Substring(1);
                }

                newPath += paths[i];
            }

            return newPath;
        }

        private static void CloseProcessWithFile(string filePath)
        {
            if (!IsFileUsing(filePath)) return;

            //正在尝试解除占用文件_FilePaths[_FileIndex]
            var runProcInfos = GetRunProcessInfos(filePath); //获取被占用的进程
            var currentProcessId = Process.GetCurrentProcess().Id;
            var localProcesses = Process.GetProcesses();
            bool hasKilled = false;
            foreach (var item in runProcInfos)
            {
                if (item.Key != currentProcessId) //排除当前进程
                {
                    var runProcess = localProcesses.SingleOrDefault(p => p.Id == item.Key);
                    //var runProcess = Process.GetProcessById(item.Key);
                    if (runProcess != null)
                    {
                        try
                        {
                            runProcess.Kill(); //强制关闭被占用的进程
                            hasKilled = true;
                        }
                        catch
                        { }
                    }
                }
            }

            if (hasKilled)
            {
                Thread.Sleep(500);
            }
        }

    }
}
