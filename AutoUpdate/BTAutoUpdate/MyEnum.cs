using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    /// <summary>
    /// 我的枚举
    /// </summary>
   public class MyEnum
    {

        /// <summary>
        /// 更新文件的类型，特殊还可以特殊处理
        /// </summary>
        public enum  UpdateFileTypeEnum
        {
            站点 = 1,
            数据库 = 2,
            定时器 = 3,
            其他 = 4
        }

        public enum UpdateResultEnum
        {
            更新成功=10,
            更新失败=20
        }
    }
}
