using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LayUp.ViewModel
{
   public class PasswordInputViewModel : ViewModelBase
    {
        
        private string _password;
        public string PassWord
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }
        private string _hint;
        public string Hint
        {
            get { return _hint; }
            set { Set(ref _hint, value); }
        }
        public ICommand SendPasswordCommand{get;set;}
        public ICommand CancelCommand{get;set; }
        public PasswordInputViewModel()
        {
            Messenger.Default.Register<string>(this, "error" ,msg => { _hint = msg; });
            SendPasswordCommand = new RelayCommand(SendPassword);
            CancelCommand = new RelayCommand(SendNothing);
        }

        private void SendNothing()
        {
            Messenger.Default.Send<string>("", "Message");
        }

        private void SendPassword()
        {
            Messenger.Default.Send<string>(_password, "Message");
        }
    }
}
