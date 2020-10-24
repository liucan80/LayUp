using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LayUp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LayUp.ViewModel
{
  public  class SettingViewModel:ViewModelBase
    {
        public SettingViewModel()
        {
            //注册消息，接收主窗体发送的信息
            Messenger.Default.Register<SettingModel>(this,"ms", msg =>
            {
                // MessageBox.Show("he");
                Subdivision = msg.Subdivision.ToString();
                Pitch = msg.Pitch.ToString();
               Debug.Print(msg.Subdivision.ToString());

            });
            //Command 绑定
            SetParameterCommand = new RelayCommand(ExcuteSendCommand);
          
        }
        //细分
        private String _subdivision;
        public String Subdivision
        {
            get { return _subdivision; }
            set { Set(ref _subdivision, value); }
        }
        //导程
        private String _pitch;
        public String Pitch
        {
            get { return _pitch; }
            set { Set(ref _pitch, value); }
        }
        public ICommand SetParameterCommand { get; set; }
     
        private SettingModel _settingModel=new SettingModel();
        public SettingModel SettingModel
        {
            get { return _settingModel; }
            set { Set(ref _settingModel, value); }
        }

        private void ExcuteSendCommand()
        {
            // Debug.WriteLine("hello");
            //MessageBox.Show("test");
            SettingModel.Subdivision = int.Parse(Subdivision);
            SettingModel.Pitch = int.Parse(Pitch);
            Messenger.Default.Send<SettingModel>(SettingModel);
            // Messenger.Default.Send<SettingModel>(SettingModel , "Message");
        }

    }
}
