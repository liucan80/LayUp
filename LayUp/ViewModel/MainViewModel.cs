using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LayUp.Models;
using System.Windows.Input;

namespace LayUp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        private FrmPLC _frmPLC=new FrmPLC();
       public FrmPLC FrmPLC { 
            get 
            { return _frmPLC; } 
            set
            { Set(ref _frmPLC, value); }
        }

        private Fx3GA _layupPLC=new Fx3GA();

        public Fx3GA LayupPLC
        {
            get { return _layupPLC; }
            set { Set(ref _layupPLC,value); }
        }
        public ICommand ConnectCommand { get; set; }
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///
            ConnectCommand = new RelayCommand(Connect);
          

        }

        private void Connect()
        {
            int a;
            _frmPLC.axActUtlType1.ActLogicalStationNumber = 1;
           int b= _frmPLC.axActUtlType1.Open();
            _frmPLC.axActUtlType1.GetDevice("x000", out a);
            _layupPLC.Input000 = a;
        }
    }
}