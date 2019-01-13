using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate.Utils
{
    /// <summary>
    /// IIS服务帮助类 add by xz 20190105
    /// </summary>
   public static class IISHelper
    {
        private static Microsoft.Web.Administration.ServerManager webManager = new Microsoft.Web.Administration.ServerManager();

        /// <summary>
        /// 获取当前IIS站点列表
        /// </summary>
        /// <returns></returns>
        public static List<IISStationInfo> GetLocalIISStationsList()
        {
           
            List<IISStationInfo> re = new List<IISStationInfo>();
            string entPath = "IIS://localhost/w3svc";
            DirectoryEntry ent = new DirectoryEntry(entPath);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName != "IIsWebServer") continue;

                IISStationInfo item = new IISStationInfo();
                if(child.Properties["ServerBindings"].Value == null)
                {
                    item.IP = "";
                }else
                {
                    var ip = child.Properties["ServerBindings"].Value.ToString().Split(':');
                    item.IP = ip[0] + ":" + ip[1];
                }              
                item.Name = child.Properties["ServerComment"].Value.ToString();
                item.PoolName = child.Properties["AppPoolId"].Value.ToString();
                DirectoryEntry root = new DirectoryEntry(entPath + "/" + child.Name + "/ROOT");
                item.Path = root.Properties["Path"].Value.ToString();            
                re.Add(item);
            }
            return re;
        }


        /// <summary>
        /// 根据站点名称获取站点信息
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static IISStationInfo GetLocalIISStations(string siteName)
        {
            try
            {
                string entPath = "IIS://localhost/w3svc";
                DirectoryEntry ent = new DirectoryEntry(entPath);
                IISStationInfo item = null;
                foreach (DirectoryEntry child in ent.Children)
                {
                    if (child.SchemaClassName == "IIsWebServer" && child.Properties["ServerComment"].Value.ToString() == siteName)
                    {
                        item = new IISStationInfo();
                        item.Name = child.Properties["ServerComment"].Value.ToString();
                        DirectoryEntry root = new DirectoryEntry(entPath + "/" + child.Name + "/ROOT");
                        item.Path = root.Properties["Path"].Value.ToString();
                       
                        break;
                    }
                }
                return item;

            }
            catch (Exception ex)
            {
                throw new Exception("查找本地站点失败:"+ex.ToString());
            }
          
        }

        /// <summary>
        /// 开启iis站点
        /// </summary>
        public static void StartWebSite(string siteName)
        {
            try
            {
                var site = webManager.Sites[siteName];
                if (site == null)
                    return;
                site.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 停止iis站点
        /// </summary>
        public static void StopWebSite(string siteName)
        {
            try
            {
                var site = webManager.Sites[siteName];
                if (site == null)
                    return;
                site.Stop();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// IIS站点数据封装
        /// </summary>
        public class IISStationInfo
        {

            /// <summary>
            /// 站点IP
            /// </summary>
            public string IP { get; set; }

            /// <summary>
            /// 站点名
            /// </summary>
            public string Name { get; set; }


            /// <summary>
            /// 站点指定路径
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// 程序池
            /// </summary>
            public string PoolName { get; set; }
        }
    }
}
