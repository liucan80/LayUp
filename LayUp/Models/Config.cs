using System;
using System.Collections.Generic;
using LayUp.Common;
using LayUp.Base;

namespace LayUp.Models
{
    /// <summary>
    /// 本软件配置文件实体类
    /// </summary>
    [Serializable]
    public class Config
    {
       
        /// <summary>
        /// 允许日志
        /// </summary>
        //public bool logEnabled
        //{
        //    get; set;
        //}

     

        /// <summary>
        /// IP地址
        /// </summary>
        public string Address
        {
            get; set;
        }
        /// <summary>
        /// 端口  
        /// </summary>
        public int? Port
        {
            get; set;
        }
        /// <summary>
        /// 自动连接
        /// </summary>
        public bool AutoConnect { get;  set; }
    }


}
