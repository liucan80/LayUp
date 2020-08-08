using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LayUp.ViewModel
{
  public  class SettingViewModel:ViewModelBase
    {
        public SettingViewModel()
        {

        }

        private string _hello = "hello";
        public string Hello
        {
            get {return _hello; }
            set { Set(ref _hello, value); }
        }

    }
}
