using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace LayUp.Models
{
  public  class Result:ObservableObject
    {
        public  String Index { get; set; }
        public DateTime dateTime { get; set; }
        public string ResultValue
        {
            get; set;

        }

        public String  IsOk { get;  set; }

       
    }
}
