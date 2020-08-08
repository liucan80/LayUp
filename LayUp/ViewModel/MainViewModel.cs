using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LayUp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EasyModbus;
using System.Windows.Controls;
using LayUp.API;
using GalaSoft.MvvmLight.Command;

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
    public class MainViewModel: ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        ModbusClient ModbusClient1 ;

        private FrmPLC _frmPLC;
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

        private Page _connectionType;
        public Page ConnectionType
        {
            get { return _connectionType; }
            set { Set(ref _connectionType, value); }
        }

        private ObservableCollection<string> _returnCode ;
        public ObservableCollection<string> ReturnCode
        {
            get { return _returnCode;}
            set{Set(ref _returnCode,value);
            } }

        private string _currentTime;

        public string CurrentTime
        {
            get { return _currentTime; }
            set { Set(ref _currentTime, value); }
        }
        //输出的状态
        private string _output;
        public string Output
        {
            get { return _output; }
            set { Set(ref _output, value); }
        }
        

        //定时器
        private readonly DispatcherTimer DispatcherTimer1;

        public ICommand ConnectCommand { get; set; }
        public ICommand SwitchOutputCommand{ get; set; }
        public ICommand StopMonitorCommand { get; set; }
       

        public ICommand ShowViewCommand { private set; get; }
        public ICommand SwitchLangugeCommand { get; set; }
        public ICommand WriteDataCommand { get; set; }
        public ICommand ChangeConnectionTypeCommand { get; set; }

        public MainViewModel()
        {
            
         
            ConnectCommand = new RelayCommand<bool>(Connect,CanConnectExcute);
            SwitchOutputCommand = new RelayCommand<string>(SwitchOutput);
            StopMonitorCommand = new RelayCommand<bool>(StopMonitor, CanStopMonitorExcute);
            ShowViewCommand = new RelayCommand<string>(ShowViewCommandExecute);
            SwitchLangugeCommand = new RelayCommand<string>(SwitchLanguage);
            WriteDataCommand = new RelayCommand<object>(WriteData);
            ChangeConnectionTypeCommand = new RelayCommand<string>(ChangeConnectionType);

            _frmPLC = new FrmPLC();
            _layupPLC = new Fx3GA();

            ReturnCode=new ObservableCollection<string>();
            //初始化定时器，定时读取PLC状态
            DispatcherTimer1 = new DispatcherTimer();
            DispatcherTimer1.Interval = new System.TimeSpan(500);
            DispatcherTimer1.Tick += GetOutputStatus;
            DispatcherTimer1.Tick += GetInputStatus;
            DispatcherTimer1.Tick += GetDataStatus;
            DispatcherTimer1.Tick += GetMStatus;




        }
        /// <summary>
        /// 如果是自动运行状态则禁用Command
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanSwitchOutput(string arg)
        {
            if (_layupPLC.IsInManualMode == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 如果已经连接到PLC则禁用Command
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanConnectExcute(bool arg)
        {
            if (_layupPLC.IsDisConnected ==true)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }

        private void GetCurrentTime(object sender, EventArgs e)
        {
            //
          _currentTime=  System.DateTime.Now.ToString();
        }


        //连接PLC
        private void Connect(bool isConnected)
        {
            //设置站号
            switch (LayupPLC.ConnectionMethod)
            {
                case "ModbusTCP":
                    #region 使用ModbusTcp连接
                   
                    if (_layupPLC.IsConnected == true)
                    {
                        DispatcherTimer1.Stop();
                       

                        LayupPLC.IsConnected = false;
                    }
                    else
                    {
                        ModbusClient1 = new ModbusClient(LayupPLC.IpAddress, (int)LayupPLC.Port);
                        try
                        {
                            ModbusClient1.Connect();
                            LayupPLC.IsConnected = true;
                          
                            DispatcherTimer1.Start();

                        }
                        catch (Exception e)
                        {

                            System.Windows.Forms.MessageBox.Show(e.Message);
                        }
                       
                    }
                    break;
                #endregion

                case "MXComponent":
                    #region 使用MX component连接
                    _frmPLC.axActUtlType1.ActLogicalStationNumber = (int)LayupPLC.StationNumber;

                    if (_layupPLC.IsConnected == true)
                    {
                        
                        DispatcherTimer1.Stop();
                        _frmPLC.axActUtlType1.Close();

                        LayupPLC.IsConnected = false;


                    }
                    else
                    {
                        int b = _frmPLC.axActUtlType1.Open();
                        LayupPLC.RetrunCode = b.ToString();
                        if (b == 0)
                        {
                            GetPLCInfo();
                            DispatcherTimer1.Start();
                            LayupPLC.IsConnected = true;
                        }
                        else
                        {
                            DispatcherTimer1.Stop();
                            LayupPLC.IsConnected = false;

                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
           
            
            
           
        }
        private bool CanStopMonitorExcute(bool arg)
        {
            if (_layupPLC.IsConnected == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        //停止监控
        private void StopMonitor(bool isConnected)
        {
            DispatcherTimer1.Stop();
            switch (LayupPLC.ConnectionMethod)
            {
                case "ModbusTCP":
                    ModbusClient1.Disconnect();
                    break;
                case "MXComponent":
                    _frmPLC.axActUtlType1.Close();
                    break;
                default:
                    break;
            }
           
           
           
            _layupPLC.IsConnected = false;
        }
        //打开/关闭输出
        private void SwitchOutput(string str)
        {
            int a;
            
            int b = _frmPLC.axActUtlType1.GetDevice(str, out a);
            if (a==0)
            {
                _frmPLC.axActUtlType1.SetDevice(str, 1);
            }
            else
            {
                _frmPLC.axActUtlType1.SetDevice(str, 0);

            }
          
            //_frmPLC.axActUtlType1.GetDevice(str, out a);
            //_layupPLC.StationNumber = b;
            //_layupPLC.Output000 = a;

        }
        private delegate int? MyDel(bool parameter);
        MyDel del = (parameter) =>
        {
            if (parameter != true)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        };
        //获取输出状态
        private void GetOutputStatus(object sender, System.EventArgs e)
        {
            switch (LayupPLC.ConnectionMethod)
            {
                case "ModbusTCP":
                    if (true)
                    {

                        bool[] Result;
                        Result = ModbusClient1.ReadCoils(0000, 16);
                        _layupPLC.Output000 = del(Result[0]);
                        _layupPLC.Output001 = del(Result[1]);
                        _layupPLC.Output002 = del(Result[2]);
                        _layupPLC.Output003 = del(Result[3]);
                        _layupPLC.Output004 = del(Result[4]);
                        _layupPLC.Output005 = del(Result[5]);
                        _layupPLC.Output006 = del(Result[6]);
                        _layupPLC.Output007 = del(Result[7]);
                        _layupPLC.Output010 = del(Result[8]);
                        _layupPLC.Output011 = del(Result[9]);
                        _layupPLC.Output012 = del(Result[10]);
                        _layupPLC.Output013 = del(Result[11]);
                        _layupPLC.Output014 = del(Result[12]);
                        _layupPLC.Output015 = del(Result[13]);
                        _layupPLC.Output016 = del(Result[14]);
                        _layupPLC.Output017 = del(Result[15]);
                       
                    }
                    break;
                case "MXComponent":

                    int[] a = new int[2];
                    int b = _frmPLC.axActUtlType1.ReadDeviceBlock("y000", 1, out a[0]);
                    if (b != 0)
                    {
                        StopMonitor(true);
                        return;
                    }
                    string str = Convert.ToString(a[0], 2).PadLeft(16, '0');
                    int[] array1 = new int[16];
                    for (int i = 0; i < 16; i++)
                    {
                        array1[i] = int.Parse(str.Substring(i, 1));
                    }

                    // _returnCode.Add(String.Format("0x{0:x8} [HEX]", b));
                    _layupPLC.Output000 = array1[15];
                    _layupPLC.Output001 = array1[14];
                    _layupPLC.Output002 = array1[13];
                    _layupPLC.Output003 = array1[12];
                    _layupPLC.Output004 = array1[11];
                    _layupPLC.Output005 = array1[10];
                    _layupPLC.Output006 = array1[9];
                    _layupPLC.Output007 = array1[8];
                    _layupPLC.Output010 = array1[7];
                    _layupPLC.Output011 = array1[6];
                    _layupPLC.Output012 = array1[5];
                    _layupPLC.Output013 = array1[4];
                    _layupPLC.Output014 = array1[3];
                    _layupPLC.Output015 = array1[2];
                    _layupPLC.Output016 = array1[1];
                    _layupPLC.Output017 = array1[0];
                    break;
                default:
                    break;
            }
           
           

        }
        //获取输入状态

        private void GetInputStatus(object sender, System.EventArgs e)
        {
            if (LayupPLC.ConnectionMethod == "ModbusTCP")
            {
                bool[] Result;
                Result = ModbusClient1.ReadDiscreteInputs(0000, 24);

               
                _layupPLC.Input000 = del(Result[0]);
                _layupPLC.Input001 = del(Result[1]);
                _layupPLC.Input002 = del(Result[2]);
                _layupPLC.Input003 = del(Result[3]);
                _layupPLC.Input004 = del(Result[4]);
                _layupPLC.Input005 = del(Result[5]);
                _layupPLC.Input006 = del(Result[6]);
                _layupPLC.Input007 = del(Result[7]);
                _layupPLC.Input010 = del(Result[8]);
                _layupPLC.Input011 = del(Result[9]);
                _layupPLC.Input012 = del(Result[10]);
                _layupPLC.Input013 = del(Result[11]);
                _layupPLC.Input014 = del(Result[12]);
                _layupPLC.Input015 = del(Result[13]);
                _layupPLC.Input016 = del(Result[14]);
                _layupPLC.Input017 = del(Result[15]);
                _layupPLC.Input020 = del(Result[16]);
                _layupPLC.Input021 = del(Result[17]);
                _layupPLC.Input022 = del(Result[18]);
                _layupPLC.Input023 = del(Result[19]);
                _layupPLC.Input024 = del(Result[20]);
                _layupPLC.Input025 = del(Result[21]);
                _layupPLC.Input026 = del(Result[22]);
                _layupPLC.Input027 = del(Result[23]);
            }
            else
            {
                int[] a = new int[2];
                int b = _frmPLC.axActUtlType1.ReadDeviceBlock("X000", 2, out a[0]);
                string str = Convert.ToString(a[0], 2).PadLeft(16, '0');

                int[] array1 = new int[16];
                int[] array2 = new int[16];
                for (int i = 0; i < 16; i++)
                {
                    array1[i] = int.Parse(str.Substring(i, 1));
                }
                str = Convert.ToString(a[1], 2).PadLeft(16, '0');
                for (int i = 0; i < 16; i++)
                {
                    array2[i] = int.Parse(str.Substring(i, 1));
                }
                _layupPLC.Input000 = array1[15];
                _layupPLC.Input001 = array1[14];
                _layupPLC.Input002 = array1[13];
                _layupPLC.Input003 = array1[12];
                _layupPLC.Input004 = array1[11];
                _layupPLC.Input005 = array1[10];
                _layupPLC.Input006 = array1[9];
                _layupPLC.Input007 = array1[8];
                _layupPLC.Input010 = array1[7];
                _layupPLC.Input011 = array1[6];
                _layupPLC.Input012 = array1[5];
                _layupPLC.Input013 = array1[4];
                _layupPLC.Input014 = array1[3];
                _layupPLC.Input015 = array1[2];
                _layupPLC.Input016 = array1[1];
                _layupPLC.Input017 = array1[0];
                _layupPLC.Input020 = array2[15];
                _layupPLC.Input021 = array2[14];
                _layupPLC.Input022 = array2[13];
                _layupPLC.Input023 = array2[12];
                _layupPLC.Input024 = array2[11];
                _layupPLC.Input025 = array2[10];
                _layupPLC.Input026 = array2[9];
                _layupPLC.Input027 = array2[8];
                // _layupPLC.Input004 = array1[0];
                //Console.WriteLine(str);
                //   BitConverter

            }


        }
        //获取D寄存器状态
        private void GetDataStatus(object sender, System.EventArgs e)
        {
            if (LayupPLC.ConnectionMethod == "ModbusTCP")
            {
                int[] Result;
                Result = ModbusClient1.ReadHoldingRegisters(192, 24);
                _layupPLC.Data200 = Result[8];
                _layupPLC.Data201 = Result[9];
                _layupPLC.Data202 = Result[10];
                _layupPLC.Data210 = Result[18];
                _layupPLC.Data211 = Result[19];
                _layupPLC.Data212 = Result[20];
            }
            else
            {

                int[] a = new int[16];
                int b = _frmPLC.axActUtlType1.ReadDeviceBlock("D200", 16, out a[0]);
                foreach (var item in a)
                {

                    Console.WriteLine(item.ToString());
                }
                _layupPLC.Data200 = a[0];
                _layupPLC.Data201 = a[1];
                _layupPLC.Data202 = a[2];
                _layupPLC.Data210 = a[10];
                _layupPLC.Data211 = a[11];
                _layupPLC.Data212 = a[12];
            }
        }
        //获取M寄存器状态
        private void GetMStatus(object sender, System.EventArgs e)
        {
            if (LayupPLC.ConnectionMethod == "ModbusTCP")
            {

                bool[] Result;
                //获取M224-M239的值 

                Result = ModbusClient1.ReadCoils(8416, 16);

                _layupPLC.M231 = del(Result[7]);
                _layupPLC.M232 = del(Result[8]);
                _layupPLC.M233 = del(Result[9]);
                _layupPLC.M234 = del(Result[10]);
                _layupPLC.M235 = del(Result[11]);

                Result = ModbusClient1.ReadCoils(8416, 16);

                return;
            }
            else
            {
                int[] a = new int[2];
                int[] b = new int[2];
                //获取M224-M239的值
                int c = _frmPLC.axActUtlType1.ReadDeviceBlock("M224", 1, out a[0]);
                //获取M128-M143的值
                c = _frmPLC.axActUtlType1.ReadDeviceBlock("M128", 1, out b[0]);

                int[] array1 = Convert1(a[0]);
                int[] array2 = Convert1(b[0]);


                _layupPLC.M231 = array1[8];
                _layupPLC.M232 = array1[7];
                _layupPLC.M233 = array1[6];
                _layupPLC.M234 = array1[5];
                _layupPLC.M235 = array1[4];

                _layupPLC.M128 = array2[15];
                _layupPLC.M129 = array2[14];
                _layupPLC.M130 = array2[13];
                _layupPLC.M131 = array2[12];
                _layupPLC.M132 = array2[11];
                _layupPLC.M133 = array2[10];
                _layupPLC.M134 = array2[9];
                _layupPLC.M135 = array2[8];
                _layupPLC.M136 = array2[7];
                _layupPLC.M137 = array2[6];
                _layupPLC.M138 = array2[5];
                _layupPLC.M139 = array2[4];
                _layupPLC.M140 = array2[3];
                _layupPLC.M141 = array2[2];
                _layupPLC.M142 = array2[1];
                _layupPLC.M143 = array2[0];



            }

        }

        private static int[] Convert1(int a)
        {
            string str = Convert.ToString(a, 2).PadLeft(16, '0');
            int[] array1 = new int[16];
            for (int i = 0; i < 16; i++)
            {
                array1[i] = int.Parse(str.Substring(i, 1));
            }

            return array1;
        }

        //获取PLC基本信息
        private void GetPLCInfo()
        {
            int cpuType;
            string cpuName;
            int returnCode = _frmPLC.axActUtlType1.GetCpuType(out cpuName, out cpuType);
            _layupPLC.CPUName = cpuName;
        }

        //写入D寄存器

        private void WriteData(object parameter)
        {

            int[] a = new int[3];
            var values = (object[])parameter;
            if (LayupPLC.ConnectionMethod=="ModbusTCP")
            {
                ModbusClient1.WriteSingleRegister(10000,Convert.ToInt32(values[0]));
                ModbusClient1.WriteSingleRegister(10001,Convert.ToInt32(values[1]));
                ModbusClient1.WriteSingleRegister(10002,Convert.ToInt32(values[2]));
                ModbusClient1.WriteSingleRegister(10010,Convert.ToInt32(values[3]));
                ModbusClient1.WriteSingleRegister(10011,Convert.ToInt32(values[4]));
                ModbusClient1.WriteSingleRegister(10012,Convert.ToInt32(values[5]));
            }
            else
            {
                int b = _frmPLC.axActUtlType1.SetDevice("D200", Convert.ToInt32(values[0]));
                b = _frmPLC.axActUtlType1.SetDevice("D201", Convert.ToInt32(values[1]));
                b = _frmPLC.axActUtlType1.SetDevice("D202", Convert.ToInt32(values[2]));
                b = _frmPLC.axActUtlType1.SetDevice("D210", Convert.ToInt32(values[3]));
                b = _frmPLC.axActUtlType1.SetDevice("D211", Convert.ToInt32(values[4]));
                b = _frmPLC.axActUtlType1.SetDevice("D212", Convert.ToInt32(values[5]));
            }
              
            
           // int b = _frmPLC.axActUtlType1.WriteDeviceBlock("D200", 3, ref a[0]);
        }
        //切换手动自动
        private void Switch()
        {
            int tempReturnCode =_frmPLC.axActUtlType1.SetDevice("m0081", 0);

        }
        public void ShowViewCommandExecute(string viewName)
        {
            Messenger.Default.Send(new NotificationMessage(viewName));
        }
        //切换界面语言
        public void SwitchLanguage(string str)
        {
            // TODO: 切换系统资源文件
            ResourceDictionary dict = new ResourceDictionary();
            try
            {
                switch (str)
                {
                    case "zh-cn":
                        dict.Source = new Uri(@"Languages\zh-cn.xaml", UriKind.Relative);
                        break;
                    case "en":
                        dict.Source = new Uri(@"Languages\en.xaml", UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri(@"Languages\zh-cn.xaml", UriKind.Relative);
                        break;
                }
                Application.Current.Resources.MergedDictionaries[0] = dict;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message+"未找到语言文件");

            }
           

            

        }


        #region 页面切换
        
        #endregion
        //改变连接方式
        private void ChangeConnectionType(string connectionType)
        {
           
            switch (connectionType)
            {
                case "MXComponent":
                   // ConnectionType = PageManager.PageMXComponent;
                    LayupPLC.ConnectionMethod = "MXComponent";
                    // ConnectionMethod = "MXComponent";
                    break;
                case "ModbusTCP":
                   // ConnectionType = PageManager.PageMXComponent;
                    LayupPLC.ConnectionMethod = "ModbusTCP";
                    //ConnectionMethod = "ModbusTcp";

                    break;
                default:
                    break;
            }
        }
    }
}