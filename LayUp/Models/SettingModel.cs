using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayUp.Models
{
   public class SettingModel:ObservableObject

    {
        public SettingModel()
        {

        }
        //细分
        public int Subdivision { get; set; }
        //导程
        public int Pitch { get; set; }

    }
}
