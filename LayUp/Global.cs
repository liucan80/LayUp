
using System;
using System.Collections.Generic;

namespace LayUp
{
    class Global
    {
        #region 常量

        public const string DownloadFileName = "v2ray-windows.zip";
        public const string v2rayWebsiteUrl = @"https://www.v2fly.org/";
        public const string AboutUrl = @"https://github.com/2dust/v2rayN";
        public const string UpdateUrl = AboutUrl + @"/releases";
        public const string v2flyCoreUrl = "https://github.com/v2fly/v2ray-core/releases";
        public const string xrayCoreUrl = "https://github.com/XTLS/Xray-core/releases";
        public const string NUrl = @"https://github.com/2dust/v2rayN/releases";
        #endregion

        /// <summary>
        /// 本软件配置文件名
        /// </summary>
        public const string ConfigFileName = "GUI-Config.json";

       

        #region 全局变量

        /// <summary>
        /// 是否需要重启服务V2ray
        /// </summary>
        public static bool reloadV2ray
        {
            get; set;
        }

        /// <summary>
        /// 是否开启全局代理(http)
        /// </summary>
        public static bool sysAgent
        {
            get; set;
        }

        /// <summary>
        /// socks端口
        /// </summary>
        public static int socksPort
        {
            get; set;
        }

        /// <summary>
        /// http端口
        /// </summary>
        public static int httpPort
        {
            get; set;
        }


        /// <summary>
        ///  
        /// </summary>
        public static int statePort
        {
            get; set;
        }


        public static System.Threading.Mutex mutexObj
        {
            get; set;
        }

        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\SimpleDb.sqlite"; }
        }



        #endregion
    }
}
