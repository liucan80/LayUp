using GalaSoft.MvvmLight;

using GalaSoft.MvvmLight.Messaging;
using LayUp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EasyModbus;
using System.Windows.Controls;
using LayUp.API;
using GalaSoft.MvvmLight.CommandWpf;
using System.IO.Ports;
using System.Drawing.Printing;
using static System.Drawing.Printing.PrinterSettings;
using System.Windows.Forms;
using PrintDialog = System.Windows.Forms.PrintDialog;
using System.Drawing;
using Newtonsoft.Json;
using LayUp.Common;
using System.Threading;
using System.IO;
using System.Management;
using SystemFonts = System.Drawing.SystemFonts;
using LayUp.Views;

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
        ModbusClient ModbusClient1;

        private FrmPLC _frmPLC;

        public FrmPLC FrmPLC
        {
            get { return _frmPLC; }
            set { Set(ref _frmPLC, value); }
        }

        private int _quantityOfDone=0;
        public int QuantityOfDone
        {
            get { return _quantityOfDone; }
            set { Set(ref _quantityOfDone, value); }
        }
        //计算机上所有串口
        private String[] _serialPorts;
        public String[] SerialPorts
        {
            get { return _serialPorts; }
            set { Set(ref _serialPorts, value); }
        }
        //用户选择的串口名称
        private String _serialPortName;
        public String SerialPortName
        {
            get { return _serialPortName; }
            set { Set(ref _serialPortName, value); }
        }
        //串口是否连接
        private bool _isSerialPortConnected;
        public bool IsSerialPortConnected
        {
            get { return _isSerialPortConnected; }
            set { Set(ref _isSerialPortConnected, value); }
        }
        //串口接收到的数据
        private string _serialReceviedValue;
        public string SerialReceviedValue
        {
            get { return _serialReceviedValue; }
            set { Set(ref _serialReceviedValue, value); }
        }
        //保存的文件名
        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { Set(ref _fileName, value); }
        }
        SerialPort serialPort = new SerialPort();
        private ObservableCollection<Result> _results=new ObservableCollection<Result>() { };

        public ObservableCollection<Result> Results
        {
            get
            {
                return _results;

            }
            set { Set(ref _results, value); }
        }
        enum enumPrinterStatus
        {
            其他状态 = 1,
            未知,
            空闲,
            正在打印,
            预热,
            停止打印,
            打印中,
            离线
        }
        //所有已安装打印机
        private StringCollection _printers;
        public StringCollection Printers
        {
            get { return _printers; }
            set { Set(ref _printers, value); }
        }
        //默认打印机
        private String _defalutPrinterName;
        public String DefaultPrinterName
        {
            get { return _defalutPrinterName; }
            set { Set(ref _defalutPrinterName, value); }
        }
        //用户选择的打印机
        private String _selectedPrinterName;
        public String SelectedPrinterName
        {
            get { return _selectedPrinterName; }
            set { Set(ref _selectedPrinterName, value); }
        }
        private String _printerStatus;
        public String PrinterStatus
        {
            get { return _printerStatus; }
            set { Set(ref _printerStatus, value); }
        }

        private System.Windows.Forms.PrintDialog printDialog1=new PrintDialog();
        private System.Drawing.Printing.PrintDocument printDocument1 =new PrintDocument();
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1=new PageSetupDialog();
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1=new PrintPreviewDialog();
        private int linesPrinted;
        private string[] lines ;

        //是否自动打印
        private bool _isAutoPrint;
        public bool IsAutoPrint
        {
            get { return _isAutoPrint; }
            set { Set(ref _isAutoPrint, value); }
        }
        string[] baudRate = { "9600", "19200", "38400", "57600", "115200" };
        string[] dataBit = { "5", "6", "7", "8" };
        private Fx3GA _layupPLC = new Fx3GA();
        
        public Fx3GA LayupPLC
        {
            get { return _layupPLC; }
            set { Set(ref _layupPLC, value); }
        }

        private Page _connectionType;

        public Page ConnectionType
        {
            get { return _connectionType; }
            set { Set(ref _connectionType, value); }
        }

        private ObservableCollection<string> _returnCode;

        public ObservableCollection<string> ReturnCode
        {
            get { return _returnCode; }
            set { Set(ref _returnCode, value); }
        }

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

        //错误状态
        private string _errorCode;

        public string ErrorCode
        {
            get { return _errorCode; }
            set { Set(ref _errorCode, value); }
        }
        private SettingModel _settingModel;
        public SettingModel SettingModel
        {
            get { return _settingModel; }
            set { Set(ref _settingModel, value); }
        }
        //原点到拍照点距离
        private float _photoDistance ;
        public float PhotoDistance
        {
            get { return _photoDistance; }
            set { Set(ref _photoDistance, value); }
        }
        //工件间隔
        private float _workpieceInterval;
        public float WorkpieceInterval
        {
            get { return _workpieceInterval; }
            set { Set(ref _workpieceInterval, value); }
        }
        //前进速度
        private float _forwardVelocity;
        public float ForwardVelocity
        {
            get { return _forwardVelocity; }
            set { Set(ref _forwardVelocity, value); }
        }
        //复位速度
        private float _returnVelocity;
        public float ReturnVelocity
        {
            get { return _returnVelocity; }
            set { Set(ref _returnVelocity, value); }
        }
        //爬行速度
        private float _jogVelocity;
        public float JogVelocity
        {
            get { return _jogVelocity; }
            set { Set(ref _jogVelocity, value); }
        }
        //原点回归开始速度
        private float _startVelocity;
        public float StartVelocity
        {
            get { return _startVelocity; }
            set { Set(ref _startVelocity, value); }
        }
        //工件数量
        private int _workpieceQuantity=12;
        public int WorkpieceQuantity
        {
            get { return _workpieceQuantity; }
            set { Set(ref _workpieceQuantity, value); }
        }
        //公差上限
        private double _tolUpperLimit=0;
        public double TolUpperLimit
        {
            get { return _tolUpperLimit; }
            set { Set(ref _tolUpperLimit, value); }
        }
        //公差下限
        private double _tolLowerLimit=0;
        public double TolLowerLimit
        {
            get { return _tolLowerLimit; }
            set { Set(ref _tolLowerLimit, value); }
        }
        //定时器
        private readonly DispatcherTimer DispatcherTimer1;

        public ICommand ConnectCommand { get; set; }
        public ICommand SwitchOutputCommand { get; set; }
        public ICommand StopMonitorCommand { get; set; }


        public ICommand ShowViewCommand { private set; get; }
        public ICommand SwitchLangugeCommand { get; set; }
        public ICommand WriteDataCommand { get; set; }
        public ICommand ChangeConnectionTypeCommand { get; set; }

        public ICommand OpenSerialPortCommand { get; set; }
        public ICommand CloseSerialPortCommand { get; set; }
        public ICommand ChangeSerialPortCommand { get; set; }
        public ICommand ClearReceviedDataCommand { get; set; }

        public ICommand OpenPrintSetupCommand { get; set; }
        public ICommand ChangePrinterCommand { get; set; }

        public ICommand OpenPrintPreviewCommand { get; set; }
        public  ICommand PrintCommand { get; set; }

        public ICommand ShowSettingWindowCommand { get; set; }
        public ICommand ShowIOWindowCommand { get; set; }


        public MainViewModel()
        {


            ConnectCommand = new RelayCommand<bool>(Connect, CanConnectExcute);
            SwitchOutputCommand = new RelayCommand<string>(SwitchOutput);
            StopMonitorCommand = new RelayCommand<bool>(StopMonitor, CanStopMonitorExcute);
            ShowViewCommand = new RelayCommand<string>(ShowSettingWindowCommandExecute);
            SwitchLangugeCommand = new RelayCommand<string>(SwitchLanguage);
            WriteDataCommand = new RelayCommand<object>(WriteData);
            ChangeConnectionTypeCommand = new RelayCommand<string>(ChangeConnectionType);

            OpenSerialPortCommand = new RelayCommand<bool>( OpenSerialPort,CanConnectSerialExcute);
            CloseSerialPortCommand = new RelayCommand(CloserSerialPort, CanCloserSerialPortExcute);
            ChangeSerialPortCommand = new RelayCommand<string>(ChangeSerialPort);
            ClearReceviedDataCommand = new RelayCommand(ClearReceviedData);

            ChangePrinterCommand = new RelayCommand<string>(ChangePrinter);
            OpenPrintSetupCommand=new RelayCommand(ShowPrintSetup);
            OpenPrintPreviewCommand=new RelayCommand(ShowPrintPreview);
            PrintCommand=new RelayCommand(Print);

            ShowSettingWindowCommand = new RelayCommand<string>(ShowSettingWindowCommandExecute);
            ShowIOWindowCommand = new RelayCommand(ShowIOWindowCommandExecute);

            _frmPLC = new FrmPLC();
            _layupPLC = new Fx3GA();
            _serialPorts = SerialPort.GetPortNames();
            _printers = PrinterSettings.InstalledPrinters;
            ReturnCode = new ObservableCollection<string>();
            //初始化定时器，定时读取PLC状态
            DispatcherTimer1 = new DispatcherTimer();
            DispatcherTimer1.Interval = new System.TimeSpan(500);
           // DispatcherTimer1.Tick += GetOutputStatus;
           // DispatcherTimer1.Tick += GetInputStatus;
            //DispatcherTimer1.Tick += GetDataStatus;
            
          //  DispatcherTimer1.Tick +={ () => { _serialPorts = SerialPort.GetPortNames(); } };
            // DispatcherTimer1.Tick += GetMStatus;

            //初始化打印
            DefaultPrinterName = printDocument1.PrinterSettings.PrinterName;
            SelectedPrinterName = DefaultPrinterName;
            //指定打印机
            printDocument1.PrinterSettings.PrinterName =SelectedPrinterName;
            //设置页边距
            printDocument1.PrinterSettings.DefaultPageSettings.Margins.Left = 0;
            printDocument1.PrinterSettings.DefaultPageSettings.Margins.Top = 0;
            printDocument1.PrinterSettings.DefaultPageSettings.Margins.Right = 0;
            printDocument1.PrinterSettings.DefaultPageSettings.Margins.Bottom = 0;
            //设置尺寸大小，如不设置默认是A4纸
            //A4纸的尺寸是210mm×297mm，
            //当你设定的分辨率是72像素/英寸时，A4纸的尺寸的图像的像素是595×842
            //当你设定的分辨率是150像素/英寸时，A4纸的尺寸的图像的像素是1240×1754
            //当你设定的分辨率是300像素/英寸时，A4纸的尺寸的图像的像素是2479×3508，
           // printDocument1.DefaultPageSettings.PaperSize = new PaperSize("A4", 2479, 3508);
            printDialog1.AllowSelection = true;
            printDocument1.DefaultPageSettings.PaperSize = printDialog1.PrinterSettings.DefaultPageSettings.PaperSize;
            //printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.OnBeginPrint);
            printDocument1.BeginPrint += OnBeginPrint;
            printDocument1.EndPrint += PrintDocument1_EndPrint;

            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.OnPrintPage);


            pageSetupDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;


            //将配置文件反序列化为对象，如果反序列化失败就使用默认值初始化SettingModel
            try
            {
                SettingModel = JsonConvert.DeserializeObject<SettingModel>(File.ReadAllText(Environment.CurrentDirectory + "\\config" + ".json"));

            }
            catch (Exception e)
            {

                //System.Windows.MessageBox.Show(e.Message);
                SettingModel = new SettingModel { Subdivision = 1600, Pitch = 5 };

            }

            //初始化串口设置


            //初始化序列化
            JsonSerializer serializer=new JsonSerializer();
           
            //注册消息接收设置窗口传来的对象，接收到后进行序列化操作保存成配置文件
            Messenger.Default.Register<SettingModel>(this, msg =>
            {
                SettingModel.Pitch = msg.Pitch;
                SettingModel.Subdivision = msg.Subdivision;
                //Debug.Print(SettingModel.Pitch.ToString());
                Debug.Print(SettingModel.Subdivision.ToString());
                string Json = JsonConvert.SerializeObject(SettingModel, Formatting.Indented);
                File.WriteAllText(Environment.CurrentDirectory +  "\\config" + ".json", Json);
                
            });

        }

        private void PrintDocument1_EndPrint(object sender, PrintEventArgs e)
        {
            PrinterStatus = "完成打印";
        }

        private void ChangePrinter(string obj)
        {
            SelectedPrinterName = obj;
            printDocument1.PrinterSettings.PrinterName = SelectedPrinterName;
        }

        private void ClearReceviedData()
        {
            Results.Clear();
            QuantityOfDone = 0;
        }

        private void ChangeSerialPort(string obj)
        {
          
            SerialPortName = obj;
            serialPort.PortName = obj;
        }

        private bool CanCloserSerialPortExcute()
        {
            if (IsSerialPortConnected==true)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        private void CloserSerialPort()
        {
            if (IsSerialPortConnected==true)
            {
                serialPort.DataReceived-= dataReceived;
               // serialPort.DataReceived = null;
                serialPort.Close();

                IsSerialPortConnected = false;
            }
           
        }

        private bool CanConnectSerialExcute(bool arg)
        {
            if (IsSerialPortConnected==true)
            {
                return false;
            }
            else
            {
                return true;
            }
           
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
            if (LayupPLC.IsConnected == true)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void GetCurrentTime(object sender, EventArgs e)
        {
            //
            _currentTime = System.DateTime.Now.ToString();
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
                        ModbusClient1 = new ModbusClient(LayupPLC.IpAddress, (int) LayupPLC.Port);
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

                    _frmPLC.axActUtlType1.ActLogicalStationNumber = (int) LayupPLC.StationNumber;

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
                            EventArgs e=null;
                            object o=null;
                            GetDataStatus( o, e);
                            lines = new string[WorkpieceQuantity];

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
            if (LayupPLC.IsConnected == true)
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
                    LayupPLC.ErrorCode = "";
                    break;
                case "MXComponent":
                    _frmPLC.axActUtlType1.Close();
                    LayupPLC.ErrorCode = "";

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
            if (a == 0)
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

        Func<bool,int> BoolToIntFunc=new Func<bool,int>(((parameter) =>
        {
            if (parameter != true)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }));
        
        
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
                        _layupPLC.Output000 = BoolToIntFunc(Result[0]);
                        _layupPLC.Output001 = BoolToIntFunc(Result[1]);
                        _layupPLC.Output002 = BoolToIntFunc(Result[2]);
                        _layupPLC.Output003 = BoolToIntFunc(Result[3]);
                        _layupPLC.Output004 = BoolToIntFunc(Result[4]);
                        _layupPLC.Output005 = BoolToIntFunc(Result[5]);
                        _layupPLC.Output006 = BoolToIntFunc(Result[6]);
                        _layupPLC.Output007 = BoolToIntFunc(Result[7]);
                        _layupPLC.Output010 = BoolToIntFunc(Result[8]);
                        _layupPLC.Output011 = BoolToIntFunc(Result[9]);
                        _layupPLC.Output012 = BoolToIntFunc(Result[10]);
                        _layupPLC.Output013 = BoolToIntFunc(Result[11]);
                        _layupPLC.Output014 = BoolToIntFunc(Result[12]);
                        _layupPLC.Output015 = BoolToIntFunc(Result[13]);
                        _layupPLC.Output016 = BoolToIntFunc(Result[14]);
                        _layupPLC.Output017 = BoolToIntFunc(Result[15]);
                       
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

               
                _layupPLC.Input000 = BoolToIntFunc(Result[0]);
                _layupPLC.Input001 = BoolToIntFunc(Result[1]);
                _layupPLC.Input002 = BoolToIntFunc(Result[2]);
                _layupPLC.Input003 = BoolToIntFunc(Result[3]);
                _layupPLC.Input004 = BoolToIntFunc(Result[4]);
                _layupPLC.Input005 = BoolToIntFunc(Result[5]);
                _layupPLC.Input006 = BoolToIntFunc(Result[6]);
                _layupPLC.Input007 = BoolToIntFunc(Result[7]);
                _layupPLC.Input010 = BoolToIntFunc(Result[8]);
                _layupPLC.Input011 = BoolToIntFunc(Result[9]);
                _layupPLC.Input012 = BoolToIntFunc(Result[10]);
                _layupPLC.Input013 = BoolToIntFunc(Result[11]);
                _layupPLC.Input014 = BoolToIntFunc(Result[12]);
                _layupPLC.Input015 = BoolToIntFunc(Result[13]);
                _layupPLC.Input016 = BoolToIntFunc(Result[14]);
                _layupPLC.Input017 = BoolToIntFunc(Result[15]);
                _layupPLC.Input020 = BoolToIntFunc(Result[16]);
                _layupPLC.Input021 = BoolToIntFunc(Result[17]);
                _layupPLC.Input022 = BoolToIntFunc(Result[18]);
                _layupPLC.Input023 = BoolToIntFunc(Result[19]);
                _layupPLC.Input024 = BoolToIntFunc(Result[20]);
                _layupPLC.Input025 = BoolToIntFunc(Result[21]);
                _layupPLC.Input026 = BoolToIntFunc(Result[22]);
                _layupPLC.Input027 = BoolToIntFunc(Result[23]);
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
                //Result = ModbusClient1.ReadHoldingRegisters(192, 24);
                Result = ModbusClient1.ReadHoldingRegisters(216, 24);

                //_layupPLC.Data200 = Result[8];
                //_layupPLC.Data201 = Result[9];
                //_layupPLC.Data202 = Result[10];
                //_layupPLC.Data210 = Result[18];
                //_layupPLC.Data211 = Result[19];
                //_layupPLC.Data212 = Result[20];
                _layupPLC.Data224 = Result[8];
                _layupPLC.Data226 = Result[10];
                _layupPLC.Data228 = Result[12];
                _layupPLC.Data230 = Result[14];
                _layupPLC.Data232 = Result[16];
                _layupPLC.Data234 = Result[18];


            }
            else
            {

                int[] a = new int[32];
                //int b = _frmPLC.axActUtlType1.ReadDeviceBlock("D200", 16, out a[0]);
                int b = _frmPLC.axActUtlType1.ReadDeviceBlock("D220", 32, out a[0]);

                foreach (var item in a)
                {

                    Console.WriteLine(item.ToString());
                }

                //_layupPLC.Data200 = a[0];
                //_layupPLC.Data201 = a[1];
                //_layupPLC.Data202 = a[2];
                //_layupPLC.Data210 = a[10];
                //_layupPLC.Data211 = a[11];
                //_layupPLC.Data212 = a[12];
                _layupPLC.Data222 = a[2];

                _layupPLC.Data224 = a[4];
                PhotoDistance = ((float)_layupPLC.Data224) / SettingModel.Subdivision * SettingModel.Pitch;
                _layupPLC.Data226 = a[6];
                JogVelocity = ((float)_layupPLC.Data226) / SettingModel.Subdivision * SettingModel.Pitch;

                _layupPLC.Data228 = a[8];
                ReturnVelocity= ((float)_layupPLC.Data228) / SettingModel.Subdivision * SettingModel.Pitch;

                _layupPLC.Data230 = a[10];
                StartVelocity = ((float)_layupPLC.Data230) / SettingModel.Subdivision * SettingModel.Pitch;

                _layupPLC.Data232 = a[12];
                WorkpieceInterval = ((float)_layupPLC.Data232) / SettingModel.Subdivision * SettingModel.Pitch;

                _layupPLC.Data234 = a[14];
                ForwardVelocity = ((float)_layupPLC.Data234) / SettingModel.Subdivision * SettingModel.Pitch;

                _layupPLC.Data236 = a[16];
                WorkpieceQuantity =(int)_layupPLC.Data236;


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

                _layupPLC.M231 = BoolToIntFunc(Result[7]);
                _layupPLC.M232 = BoolToIntFunc(Result[8]);
                _layupPLC.M233 = BoolToIntFunc(Result[9]);
                _layupPLC.M234 = BoolToIntFunc(Result[10]);
                _layupPLC.M235 = BoolToIntFunc(Result[11]);
                //获取M128-M143的值 

                Result = ModbusClient1.ReadCoils(8320, 16);

                ErrorCode = "";
                
                foreach (var item in Result)
                {
                    if (item==true)
                    {
                        ErrorCode += "1";

                        
                    }
                    else
                    {
                        ErrorCode += "0";
                    }
                }
              
                
                _layupPLC.M128 =BoolToIntFunc(Result[0]);
                _layupPLC.M129 =BoolToIntFunc(Result[1]);
                _layupPLC.M130 =BoolToIntFunc(Result[2]);
                _layupPLC.M131 =BoolToIntFunc(Result[3]);
                _layupPLC.M132 =BoolToIntFunc(Result[4]);
                _layupPLC.M133 =BoolToIntFunc(Result[5]);
                _layupPLC.M134 =BoolToIntFunc(Result[6]);
                _layupPLC.M135 =BoolToIntFunc(Result[7]);
                _layupPLC.M136 =BoolToIntFunc(Result[8]);
                _layupPLC.M137 =BoolToIntFunc(Result[9]);
                _layupPLC.M138 =BoolToIntFunc(Result[10]);
                _layupPLC.M139 =BoolToIntFunc(Result[11]);
                _layupPLC.M140 =BoolToIntFunc(Result[12]);
                _layupPLC.M141 =BoolToIntFunc(Result[13]);
                _layupPLC.M142 =BoolToIntFunc(Result[14]);
                _layupPLC.M143 =BoolToIntFunc(Result[15]);
             //  Debug.Print(_layupPLC.M128.ToString());
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

                ErrorCode = "";

                foreach (var item in array2)
                {
                    ErrorCode += item.ToString();
                }


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
                //ModbusClient1.WriteSingleRegister(10000,Convert.ToInt32(values[0]));
                //ModbusClient1.WriteSingleRegister(10001,Convert.ToInt32(values[1]));
                //ModbusClient1.WriteSingleRegister(10002,Convert.ToInt32(values[2]));
                //ModbusClient1.WriteSingleRegister(10010,Convert.ToInt32(values[3]));
                //ModbusClient1.WriteSingleRegister(10011,Convert.ToInt32(values[4]));
                //ModbusClient1.WriteSingleRegister(10012,Convert.ToInt32(values[5]));
            }
            else
            {
                int b = _frmPLC.axActUtlType1.SetDevice("D224",Convert.ToInt32( PhotoDistance * SettingModel.Subdivision / SettingModel.Pitch));
                b = _frmPLC.axActUtlType1.SetDevice("D226", Convert.ToInt32(JogVelocity* SettingModel.Subdivision / SettingModel.Pitch));
                b = _frmPLC.axActUtlType1.SetDevice("D228", Convert.ToInt32(ReturnVelocity * SettingModel.Subdivision / SettingModel.Pitch));
                b = _frmPLC.axActUtlType1.SetDevice("D230", Convert.ToInt32(StartVelocity * SettingModel.Subdivision / SettingModel.Pitch));
                b = _frmPLC.axActUtlType1.SetDevice("D232", Convert.ToInt32(WorkpieceInterval * SettingModel.Subdivision / SettingModel.Pitch));
                b = _frmPLC.axActUtlType1.SetDevice("D234", Convert.ToInt32(ForwardVelocity * SettingModel.Subdivision / SettingModel.Pitch));
                b = _frmPLC.axActUtlType1.SetDevice("D236", Convert.ToInt32(WorkpieceQuantity));
                Results.Clear();

            }


            // int b = _frmPLC.axActUtlType1.WriteDeviceBlock("D200", 3, ref a[0]);
        }
        //切换手动自动
        private void Switch()
        {
            int tempReturnCode =_frmPLC.axActUtlType1.SetDevice("m0081", 0);

        }
        public void ShowSettingWindowCommandExecute(string viewName)
        {
            SettingView settingView = new SettingView();
            
            Messenger.Default.Send<SettingModel>(SettingModel,"ms");
            settingView.ShowDialog();

            
        }
        public void ShowIOWindowCommandExecute()
        {
            WindowIOTable windowIOTable = new WindowIOTable();

          //  Messenger.Default.Send<SettingModel>(SettingModel, "ms");
            windowIOTable.ShowDialog();


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
                System.Windows.Application.Current.Resources.MergedDictionaries[0] = dict;
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

        //打开串口
        private void OpenSerialPort(bool isOpen )
        {
            if (!serialPort.IsOpen)//串口处于关闭状态
            {

                try
                {

                    serialPort.PortName = SerialPortName;//串口号
                    serialPort.BaudRate = 9600;//波特率
                    serialPort.DataBits = 8;//数据位

                    //打开串口
                    serialPort.Open();
                    try
                    {
                        // serialPort.Write("R0\r");
                       // serialPort.DiscardInBuffer();
                       //设置TM3000为RS232通讯模式
                        serialPort.Write("Q0\r");
                        SerialReceviedValue = serialPort.ReadTo("\r");
                        serialPort.DiscardInBuffer();
                        //读取TM3000的out1的公差设置
                        serialPort.Write("SR,LM,01\r");
                        SerialReceviedValue = serialPort.ReadTo("\r");
                        string[] ReceviedValue = SerialReceviedValue.Split(',');
                        if (ReceviedValue[0] == "SR")
                            {
                                string temp1 = ReceviedValue[3].Substring(1);
                                TolUpperLimit= Convert.ToDouble(temp1);
                                string temp2 = ReceviedValue[4].Substring(1);
                                TolLowerLimit = Convert.ToDouble(temp2);
                            //设置TM3000为正常拍照模式
                            serialPort.Write("R0\r");
                            SerialReceviedValue = serialPort.ReadTo("\r");
                            serialPort.DataReceived += dataReceived;
                            serialPort.DiscardInBuffer();
                            IsSerialPortConnected = true;

                            // Debug.Print(Convert.ToDouble("000.512").ToString());
                            // 510 > Convert.ToInt32(temp2[1]) && Convert.ToInt32(temp2[1]) > 490;
                        }
                        else
                            {


                            }
                            

                        

                        }
                    catch (Exception e)
                    {

                        System.Windows.MessageBox.Show(e.Message);
                    }

                }
                catch (System.Exception ex)
                {
                   System.Windows.MessageBox.Show("Error:" + ex.Message, "Error");
                    IsSerialPortConnected = false;
                    return;
                }
            }
       

        }
        //串口接收数据
        private void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
               {
                Debug.Print("接收到串口数据");
                string input=serialPort.ReadTo("\r");
                serialPort.DiscardInBuffer();
                //数据格式:TG,01,+000.485
               
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    if (Results.Count==WorkpieceQuantity)
                    {
                        Results.Clear();
                    }
                   
                    SerialReceviedValue = input;
                    QuantityOfDone++;
                   string[] ReceviedValue= SerialReceviedValue.Split(',');
                    string ResultValue = null;
                    if (ReceviedValue[2]== "＋FFFFFFF"|| ReceviedValue[2] == "-FFFFFFF")
                    {
                        ResultValue = "超量程";
                    }
                    else
                    {
                        string temp = ReceviedValue[2].Substring(1);
                      //string[] temp2=  temp.Split('.');

                        if (TolUpperLimit> Convert.ToDouble(temp)&& Convert.ToDouble(temp)>TolLowerLimit)
                        {
                            ResultValue = "OK";
                        }
                        else
                        {
                            ResultValue = "NG";
                        }
                    }
                    // Thread.Sleep(100);
                    //获取当前时间
                    DateTime TempTime = DateTime.Now;
                    //把获取的结果保存到Results中
                    Results.Add(new Result() { Index =QuantityOfDone.ToString(), IsOk =ResultValue, dateTime = TempTime, ResultValue = ReceviedValue[2] });
                    //把获取的数据添加到Line中等待打印
                    //lines[QuantityOfDone-1] = QuantityOfDone.ToString() + "," + TempTime.ToString() + "," + ReceviedValue[2] + ","+ ResultValue;
                    lines[QuantityOfDone-1] = QuantityOfDone.ToString() + ", "  + ReceviedValue[2] + ", "+ ResultValue;


                    if (QuantityOfDone==WorkpieceQuantity)
                    {
                        string Json = JsonConvert.SerializeObject(Results, Formatting.Indented);
                        FileName = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        File.WriteAllText(Environment.CurrentDirectory + "\\Logs\\"+FileName+".json", Json);
                        QuantityOfDone = 0;
                        if (IsAutoPrint)
                        {
                            Print();
                        }
                    }
                    

                });
                


            }
                    catch (System.Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Error");
                        
                    }
                
            }

        //显示打印设置窗口

        private void ShowPrintSetup()
        {
            try
            {
                pageSetupDialog1.PageSettings.Margins = new Margins(0, 0, 0, 0);
                pageSetupDialog1.ShowDialog();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                System.Windows.MessageBox.Show(e.Message, "Error");
                //throw;
            }
        }
        //显示打印预览窗口
        private void ShowPrintPreview()
        {
            try
            {
                printPreviewDialog1.ShowDialog();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                System.Windows.MessageBox.Show(e.Message, "Error");

                //throw;
            }
        }

        //打印
        private void Print()
        {
            try
            {
                
                printDocument1.Print();
               
            }
            catch (Exception ex)
            {
                //打印失败时输出
                System.Windows.MessageBox.Show(ex.Message, "Error");

            }

        }
        private void OnBeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            PrinterStatus = "正在打印";
            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }
        }
        // OnPrintPage
        private void OnPrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //int x = e.MarginBounds.Width / 2;
            int x = e.PageBounds.Width / 2;
            int y = e.PageBounds.Top + 40;
            int lineHight = 25;
            Brush textBrush = new SolidBrush(Color.Black);
            Font textFont= new Font("Arial Narrow", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PointF titlePointF=new PointF(e.PageBounds.Width/2,e.PageBounds.Top+10);
            StringFormat textFormat = new StringFormat() { LineAlignment = StringAlignment.Center ,Alignment = StringAlignment.Center};

            Brush titleBrush = new SolidBrush(Color.Black);
            Font titleFont=new Font("Arial Narrow",12F);
            StringFormat titleFormat = new StringFormat() {LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
            e.Graphics.DrawString(FileName,titleFont,titleBrush,titlePointF,titleFormat);
   
            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++], textFont, textBrush, x, y,textFormat);
                y += lineHight;
                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;



                }
            }

            linesPrinted = 0;
            e.HasMorePages = false;
        }
     
        private  enumPrinterStatus GetPrinterStat(string PrinterDevice)
        {
            enumPrinterStatus ret = 0;
            string path = @"win32_printer.DeviceId='" + PrinterDevice + "'";
            ManagementObject printer = new ManagementObject(path);
            printer.Get();
            ret = (enumPrinterStatus)Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
            return ret;
        }
    }
    }
