using GalaSoft.MvvmLight;
using LayUp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LayUp.ViewModel
{
   public class IOTableViewModel:ViewModelBase
    {

        private ObservableCollection<IO> _IOList;
        /// <summary>
        /// 数据列表
        /// </summary>
        public ObservableCollection<IO> IOList
        {
            get { return _IOList; }
            set { Set(ref _IOList,value); }
        }
        public IOTableViewModel()
        {
            
        }
    }
}
