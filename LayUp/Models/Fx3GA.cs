using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace LayUp.Models
{
    /// <summary>
    /// 该类暴露数据给界面
    /// </summary>
  public  class Fx3GA:ObservableObject
    {
        public int? StationNumber { get; set; }
        private int? _input000;
        public int? Input000 { get { return _input000; } set
            { Set(ref _input000,value); } }
        public int? Output000 { get; set; }
    }
}
