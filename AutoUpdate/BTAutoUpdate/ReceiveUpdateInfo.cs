using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    /// <summary>
    /// 接收更新文件的信息
    /// </summary>
   public class ReceiveUpdateInfo
    {

        /// <summary>
        /// 需要更新名称(注：站点名，数据库名，定时器名)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 要下载的文件名称
        /// </summary>
        public string DownFileName { get; set; }

        /// <summary>
        /// 本地物理路径
        /// </summary>
        public string LocalFilePath { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public MyEnum.UpdateFileTypeEnum FileType { get; set; }
    }
}
