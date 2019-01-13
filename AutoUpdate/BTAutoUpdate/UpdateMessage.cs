using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    /// <summary>
    /// 更新的消息
    /// </summary>
   public class UpdateMessage
    {
        /// <summary>
        /// 返回的消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 更新的结果
        /// </summary>
        public MyEnum.UpdateResultEnum UpdateResult { get; set; }
    }
}
