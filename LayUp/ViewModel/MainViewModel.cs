using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LayUp.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using LayUp.Handler;
using Dapper;
using System.Collections.Generic;
using EasyModbus;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
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
        
        EasyModbus.ModbusClient ModbusClient1;
        
        protected static Config config;
        private Fx3GA _layupPLC = new Fx3GA();
        public Fx3GA LayupPLC
        {
            get { return _layupPLC; }
            set { Set(ref _layupPLC, value); }
        }
        /// <summary>
        /// 是否自动连接PLC
        /// </summary>
        private bool _autoConnect;
        public bool AutoConnect
        {
            get { return _autoConnect; }
            set { Set(ref _autoConnect, value); }

        }
        //错误状态
        private string _errorCode;

        private bool _errorHappened;
        public bool ErrorHappened { get { return _errorHappened; }  set {
                Set(ref _errorHappened, value);
                 } }

        public string ErrorCode
        {
            get { return _errorCode; }
            set { Set(ref _errorCode, value); }
        }
        
        private ObservableCollection<ErrorRecord> _currentLog=new ObservableCollection<ErrorRecord> { 

        };
        public ObservableCollection<ErrorRecord> CurrentLog
        {
            get { return _currentLog; }
            set
            {
                Set(ref _currentLog, value);
            }
        }
        private bool[] previousRegister = new bool[100];
        public ObservableCollection<Error> DefinedError = new ObservableCollection<Error>();
        public ObservableCollection<string> ErrorRegisters = new ObservableCollection<string>();

        /// <summary>
        /// 是否开机启动
        /// </summary>
        private bool _isAutoRun;
        public bool IsAutoRun
        { get => Utils.IsAutoRun();
            set { Set(ref _isAutoRun, value); }
        }

        /// <summary>
        /// 查询的起始时间
        /// </summary>
        private DateTime _startDateTime=DateTime.Now;
        public DateTime StartDateTime
        { get { return _startDateTime; }
            set { Set(ref _startDateTime,value); }
        }
        /// <summary>
        /// 查询的截至时间
        /// </summary>
        private DateTime _endDateTime=DateTime.Now;
        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set { Set(ref _endDateTime, value); }
        }
        /// <summary>
        /// 要查询的记录等级
        /// </summary>
        private string _level;
        public string Level
        {
            get { return _level; }
            set { Set(ref _level,value); }
        }
        /// <summary>
        /// 要查询的记录ID
        /// </summary>
        private string _ID;
        public string ID
        {
            get { return _ID; }
            set { Set(ref _ID, value); }
        }

        /// <summary>
        /// 是否允许查询
        /// </summary>
        private bool _isAllowQuery;
        public bool IsAllowQuery
        {
            get { return _isAllowQuery; }
            set { Set(ref _isAllowQuery, value); }
        }
        //定时器
        private readonly DispatcherTimer DispatcherTimer1;
        private readonly DispatcherTimer DispatcherTimer2;
        public ICommand ConnectCommand { get; set; }
        public ICommand SwitchOutputCommand { get; set; }
        public ICommand StopMonitorCommand { get; set; }
        public ICommand ShowViewCommand { private set; get; }
        public ICommand SwitchLangugeCommand { get; set; }
        public ICommand WriteDataCommand { get; set; }
        public ICommand WriteRadomDataCommand { get; set; }
        public ICommand ReadRadomDataCommand { get; set; }
        public ICommand ChangeConnectionTypeCommand { get; set; }
        public ICommand ButtonDownCommand { get; set; }
        public ICommand ButtonUpCommand { get; set; }
        public ICommand SetAutoRunCommand { get; set; }
        public ICommand SetAutoConnectCommand { get; set; }
        public ICommand ResetErrorCommand { get; set; }
        public ICommand ResetPLCCommand { get; set; }
        public ICommand ClearErrorLogCommand { get; set; }
        public ICommand QueryCommand { get; set; }
        public ICommand AllowQueryCommand { get; set; }
        public ICommand ClearDateLogCommand { get; set; }
        public ICommand ShowWindow { get; set; }
        DataAccess dataAccess = new DataAccess();
        PasswordInput passwordInput;
        public MainViewModel()
        {
           
            Messenger.Default.Register<string>(this, "Message", msg =>
            {
                if (msg=="6666")
                {
                    dataAccess.ClearRecords();
                    EasyModbus.StoreLogData.Instance.Store("ClearErrorLog", DateTime.Now);
                    passwordInput.Close();
                }
                else
                {
                    passwordInput.Close();
                }

            });
            CurrentLog = dataAccess.GetAllRecords();
            ConnectCommand = new RelayCommand(Connect, ()=> { return !_layupPLC.IsConnected; });
            SwitchOutputCommand = new RelayCommand<string>(SwitchDelay);
         //   StopMonitorCommand = new RelayCommand<bool>(CloseConnection, CanStopMonitorExcute);
            StopMonitorCommand = new RelayCommand(CloseConnection, () =>{ return _layupPLC.IsConnected; });
            ShowViewCommand = new RelayCommand<string>(ShowViewCommandExecute);
            SwitchLangugeCommand = new RelayCommand<string>(SwitchLanguage);
            WriteDataCommand = new RelayCommand<object>(WriteData);
            WriteRadomDataCommand = new RelayCommand(WriteRadomData,()=> { return _layupPLC.IsConnected; });
            ReadRadomDataCommand = new RelayCommand(ReadRadomData);
            ChangeConnectionTypeCommand = new RelayCommand<string>(ChangeConnectionType);
            ButtonDownCommand = new RelayCommand<string>(OnButtonDown);
            ButtonUpCommand = new RelayCommand<string>(OnButtonUp   );
            SetAutoRunCommand = new RelayCommand<bool>(setAutoRun);
            SetAutoConnectCommand = new RelayCommand<bool>(setAutoConnect);
            ResetErrorCommand = new RelayCommand(ResetError, ()=> { return ErrorHappened; });
            ResetPLCCommand = new RelayCommand(ResetPLC, () => { return _layupPLC.SM[0]; });
            ClearErrorLogCommand = new RelayCommand(ClearErrorLog, () => {return CurrentLog.Count == 0 ? false : true; });
            QueryCommand = new RelayCommand(QueryErrorLog);
            AllowQueryCommand = new RelayCommand(AllowQuery);
            ClearDateLogCommand = new RelayCommand(ClearRecords);


            _layupPLC = new Fx3GA(){ ConnectionMethod="ModbusTCP" };

            //载入配置文件
            ConfigHandler.LoadConfig(ref config);
            _layupPLC.IpAddress = config.Address;
            _layupPLC.Port = config.Port;
            AutoConnect = config.AutoConnect;    
            //载入错误定义文件
            using (StreamReader sr = new StreamReader("CustomError.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] elements = line.Split(',');
                    DefinedError.Add(new Error { ID = Convert.ToInt32(elements[0]), ErrorRegister = elements[1], ErrorDescription = elements[2], Level = elements[3] });
                    //dataGridView1.Rows.Add(elements);
                }

                foreach (var item in DefinedError)
                {
                    ErrorRegisters.Add(item.ErrorRegister);
                }
            } 
           
            //初始化定时器，定时读取PLC状态
            DispatcherTimer1 = new DispatcherTimer();
            DispatcherTimer1.Interval = new System.TimeSpan(0,0,1);
            DispatcherTimer1.Tick += GetOutputStatus;
            DispatcherTimer1.Tick += GetInputStatus;
            DispatcherTimer1.Tick += GetDataStatus;
            DispatcherTimer1.Tick += GetMStatus;
            DispatcherTimer1.Tick += GetSpecialDataStatus;
            DispatcherTimer1.Tick += GetSpecialMStatus;
            //初始化定时器，读取记录数据库
            DispatcherTimer2 = new DispatcherTimer { Interval = new System.TimeSpan(0, 0, 1) };
            DispatcherTimer2.Tick += DispatcherTimer2_Tick;
            DispatcherTimer2.Start();
            //如果配置是自动连接PLC 则软件开启后就连接PLC
            if (AutoConnect)
            {
                Connect();
                
            }
            EasyModbus.StoreLogData.Instance.Store("Open HMI", DateTime.Now);

        }

        private void ClearRecords()
        {

            passwordInput = new PasswordInput { WindowStartupLocation=WindowStartupLocation.CenterScreen};
            passwordInput.ShowDialog();
            
        } 

        private void DispatcherTimer2_Tick(object sender, EventArgs e)
        {
            Debug.Print(Thread.CurrentThread.Name);
            Task.Run(() => { CurrentLog = dataAccess.GetAllRecords(); });
            Debug.Print(Thread.CurrentThread.Name);

        }

        private void AllowQuery()
        {
            if (IsAllowQuery)
            {
                DispatcherTimer2.Stop();
            }
            else
            {
                DispatcherTimer2.Start();
                
            }
        }

        private void QueryErrorLog()
        {
            var sql = "SELECT * FROM ErrorRecord Where" ;
            var param = new
            {
                StartTime = StartDateTime,
                EndTime = EndDateTime,
                ErrorLevel = Level,
                ErrorID = ID
            };
            if (StartDateTime!=null)
            {
                sql = sql + " ErrorTime>=@StartTime";
            }
            if (EndDateTime != null)
            {
                sql = sql + " and ErrorTime<=@EndTime";
            }
            if (Level != null && Level!="")
            {
                sql = sql + " and Level==@ErrorLevel";
            }
            if (ID != null && ID!="")
            {
                sql = sql + " and ID==@ErrorID";
            }
            // var sql = "SELECT * FROM ErrorRecord Where ErrorTime >= @ErrorTime and ErrorTime >= @ErrorTime ";
            CurrentLog = dataAccess.GetRecords(sql, param);
        }

        private void ClearErrorLog()
        {
            //System.Windows.Forms.MessageBox.Show("Test");
            CurrentLog.Clear();
        }

        private void ResetPLC()
        {
            //将SM50置位，
            ModbusClient1.WriteSingleCoil(20480, true);//UNDONE(需要测试是否可以清除错误)
        }

      

        private void ReadRadomData()
        {
            var result = ModbusClient1.ReadHoldingRegisters(150, 100);
            for (int i = 0; i < 100; i++)
            {
                _layupPLC.Data[i+150] = result[i];
            }
        }

        private void setAutoConnect(bool obj)
        {
            config.AutoConnect = _autoConnect;
            ConfigHandler.SaveConfig(ref config);
        }

        private void OnButtonUp(string str)
        {
            int MRelayAddress = Convert.ToInt32(str.Substring(1)) + 8192;
            try
            {
                ModbusClient1.WriteSingleCoil(MRelayAddress, false);
            }
            catch (Exception)
            {
                throw;
            }
    
        }

        private void OnButtonDown(string str)
        {
            int MRelayAddress = Convert.ToInt32(str.Substring(1)) + 8192;
            try
            {
                    ModbusClient1.WriteSingleCoil(MRelayAddress, true);
            }
            catch (Exception)
            {
                throw;
            }
          
        }

        //打开连接
        private void Connect()
        {
            //设置站号

            switch (_layupPLC.ConnectionMethod)
            {
                case "ModbusTCP":
                    config.Address = _layupPLC.IpAddress;
                    config.Port = _layupPLC.Port;
                    ConfigHandler.SaveConfig(ref config);
                    #region 使用ModbusTcp连接

                    if (_layupPLC.IsConnected)
                    {
                        DispatcherTimer1.Stop();
                        _layupPLC.IsConnected = false;
                    }
                    else
                    {
                        ModbusClient1 = new ModbusClient(_layupPLC.IpAddress, (int) _layupPLC.Port);
                        try
                        {
                            ModbusClient1.Connect();
                            DispatcherTimer1.Start();
                            _layupPLC.IsConnected = true;
                            //读取界面需要的参数
                            ReadRadomData();
                            //记录连接信息
                            EasyModbus.StoreLogData.Instance.Store("PLC Connected for Modbus-TCP,IPAddress: " + _layupPLC.IpAddress + ", Port: " + _layupPLC.Port, DateTime.Now);
                        }
                        catch (Exception e)
                        {

                            System.Windows.Forms.MessageBox.Show(e.Message);
                        }

                    }

                    break;

                #endregion

                

               
                default:
                    
                    break;
            }
            



        }

        //关闭连接
        private void CloseConnection()
        {
            DispatcherTimer1.Stop();
            switch (_layupPLC.ConnectionMethod)
            {
                case "ModbusTCP":
                    ModbusClient1.Disconnect();
                    EasyModbus.StoreLogData.Instance.Store("PLC DisConnected for Modbus-TCP,IPAddress: " + _layupPLC.IpAddress + ", Port: " + _layupPLC.Port, DateTime.Now);
                    break;
                default:
                    break;
            }
            _layupPLC.IsConnected = false;
        }
        //打开/关闭继电器
        private void SwitchDelay(string str)
        {
           
            //用M继电器驱动输出继电器,fx5UPLC M0~7679的Modbus地址是2000H,所以先获取对应M继电器状态再翻转
            int MRelayAddress = Convert.ToInt32(str.Substring(1))+8192; 
            bool[] MState;
            try
            {
               MState= ModbusClient1.ReadCoils(MRelayAddress, 1);
                if (MState[0])
                {
                    ModbusClient1.WriteSingleCoil(MRelayAddress, false);
                    EasyModbus.StoreLogData.Instance.Store("Switch Delay \t" + str +"\tFalse", DateTime.Now);

                }
                else
                {
                    ModbusClient1.WriteSingleCoil(MRelayAddress, true);
                    EasyModbus.StoreLogData.Instance.Store("Switch Delay \t" + str+"\tTrue", DateTime.Now);

                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        //获取Y输出状态
        private void GetOutputStatus(object sender, System.EventArgs e)
        {
            try
            {
                switch (_layupPLC.ConnectionMethod)
                {
                    case "ModbusTCP":
                        if (true)
                        {

                            bool[] Result;

                            Result = ModbusClient1.ReadCoils(0000, 32);

                            for (int i = 0; i < 32; i++)
                            {
                                _layupPLC.Output[i] = Result[i];
                            }

                          

                        }
                        break;
                    
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
           
           
           

        }
        //获取X输入状态
        private void GetInputStatus(object sender, System.EventArgs e)
        {
            if (_layupPLC.ConnectionMethod == "ModbusTCP")
            {
                bool[] Result;
                Result = ModbusClient1.ReadDiscreteInputs(0000, 32);
                for (int i = 0; i < 32; i++)
                {
                    _layupPLC.Input[i] = Result[i];
                }

            }
            


        }
        //获取D寄存器状态
        private void GetDataStatus(object sender, System.EventArgs e)
        {
            if (_layupPLC.ConnectionMethod == "ModbusTCP")
            {
                int[] Result;
                Result = ModbusClient1.ReadHoldingRegisters(130, 100);

                _layupPLC.Data[130] = Result[0];
                _layupPLC.Data[132] = Result[2];
            }
            
        }
        //获取M寄存器状态
        public async void  GetMStatus(object sender, System.EventArgs e)
        {
            if (_layupPLC.ConnectionMethod == "ModbusTCP")
            {
                bool[] Result;
                //获取M0-M320的值 
                Result = ModbusClient1.ReadCoils(8192,320);
                for(int i=0;i<320;i++)
                {
                    _layupPLC.M[i] = Result[i];

                }
                //获取M1000-M1100的值
                Result = ModbusClient1.ReadCoils(9192, 100);
                for (int i = 0; i < 100; i++)
                {
                    _layupPLC.M[i+1000] = Result[i];

                }
               await  Task.Run(new Action(()=> { ErrorHandler(Result); }));
                
                   
            }
           

        }
        //获取SD寄存器状态
        private void GetSpecialDataStatus(object sender, System.EventArgs e)
        {
            if (_layupPLC.ConnectionMethod == "ModbusTCP")
            {
                int[] Result;
                Result = ModbusClient1.ReadHoldingRegisters(20480, 80);
                _layupPLC.SpecicalData[0] = Result[0];
                Result = ModbusClient1.ReadHoldingRegisters(0x657C, 50);
                for (int i = 0; i < 50; i++)
                {
                    _layupPLC.SpecicalData[i+5500] = Result[i];
                }     
            }
           
        }
        //获取SM继电器状态
        private void GetSpecialMStatus(object sender, System.EventArgs e)
        {
            if (_layupPLC.ConnectionMethod == "ModbusTCP")
            {
                bool[] Result;
                Result = ModbusClient1.ReadCoils(20480, 100);

                for (int i = 0; i < 100; i++)
                {
                    _layupPLC.SM[i] = Result[i];
                }

            }

        }
        //写入D寄存器
        private void WriteData(object parameter)
        {

            int[] a = new int[3];
            var values = (object[])parameter;
            if (_layupPLC.ConnectionMethod == "ModbusTCP")
            {
                ModbusClient1.WriteSingleRegister(10000, Convert.ToInt32(values[0]));
                ModbusClient1.WriteSingleRegister(10001, Convert.ToInt32(values[1]));
                ModbusClient1.WriteSingleRegister(10002, Convert.ToInt32(values[2]));
                ModbusClient1.WriteSingleRegister(10010, Convert.ToInt32(values[3]));
                ModbusClient1.WriteSingleRegister(10011, Convert.ToInt32(values[4]));
                ModbusClient1.WriteSingleRegister(10012, Convert.ToInt32(values[5]));
            }
        }
        //写入随机D寄存器
        private void WriteRadomData()
        {
            string str = $"D150={ _layupPLC.Data[150]}\t";
            str = str + $"D200={ _layupPLC.Data[200]}\t";
            str = str + $"D234={ _layupPLC.Data[234]}\t";
            str = str + $"D232={ _layupPLC.Data[232]}\t";
            str = str + $"D170={ _layupPLC.Data[170]}\t";
            str = str + $"D172={ _layupPLC.Data[172]}\t";
            str = str + $"D174={ _layupPLC.Data[174]}\t";
            str = str + $"D176={ _layupPLC.Data[176]}\t";
            str = str + $"D196={ _layupPLC.Data[196]}\t";
            str = str + $"D198={ _layupPLC.Data[198]}\t";

            ModbusClient1.WriteSingleRegister(150, _layupPLC.Data[150]);
            ModbusClient1.WriteSingleRegister(200, _layupPLC.Data[200]);
            ModbusClient1.WriteSingleRegister(234, _layupPLC.Data[234]);
            ModbusClient1.WriteSingleRegister(232, _layupPLC.Data[232]);
            ModbusClient1.WriteSingleRegister(170, _layupPLC.Data[170]);
            ModbusClient1.WriteSingleRegister(172, _layupPLC.Data[172]);
            ModbusClient1.WriteSingleRegister(174, _layupPLC.Data[174]);
            ModbusClient1.WriteSingleRegister(176, _layupPLC.Data[176]);
            ModbusClient1.WriteSingleRegister(196, _layupPLC.Data[196]);
            ModbusClient1.WriteSingleRegister(198, _layupPLC.Data[198]);
            EasyModbus.StoreLogData.Instance.Store("Write Parameters\t"+str, DateTime.Now);


        }

        public void ShowViewCommandExecute(string viewName)
        {
            Messenger.Default.Send(new NotificationMessage(viewName));
        }
        //切换界面语言
        public void SwitchLanguage(string str)
        {
          
            //  切换系统资源文件
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

        //设置自动启动
        public void setAutoRun(bool isautorun)
        {
             Utils.SetAutoRun(!isautorun);
        }
 
        //改变连接方式
        private void ChangeConnectionType(string connectionType)
        {
           
            switch (connectionType)
            {
                case "MXComponent":
                    _layupPLC.ConnectionMethod = "MXComponent";
       
                    break;
                case "ModbusTCP":
                  
                    _layupPLC.ConnectionMethod = "ModbusTCP";
                    break;
                default:
                    break;
            }
        }

        private void ErrorHandler(bool[] Register)
        {
            List<ErrorRecord> er = new List<ErrorRecord>();
            if (Register.Contains(true))
            {
                _errorHappened = true;
                
                for (int i = 0; i < DefinedError.Count; i++)
                {
                    if (Register[i] != previousRegister[i])
                    {
                        if (Register[i])
                        {

                            Error e = DefinedError.Where(p => p.ID == i).First();
                            ErrorRecord errorRecord = new ErrorRecord { ID = e.ID, ErrorDescription = e.ErrorDescription, ErrorRegister = e.ErrorRegister, Level = e.Level, ErrorTime = System.DateTime.Now };
                            //Task.Run(new Action(() => { CurrentLog.Add(errorRecord); er.Add(errorRecord);
                            //    dataAccess.InsertErrorRecord(errorRecord);
                            //}));
                            
                           System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => {
                               //CurrentLog.Add(errorRecord); 
                               er.Add(errorRecord);
                               dataAccess.InsertErrorRecord(errorRecord);
                           }));
                            //Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                            //    CurrentLog.Add(errorRecord); er.Add(errorRecord);
                            //    dataAccess.InsertErrorRecord(errorRecord);
                            //}));
                            //CurrentLog.Add(errorRecord);
                            // er.Add(errorRecord);
                            // dataAccess.InsertErrorRecord(errorRecord);
                        }

                    }


                    previousRegister[i] = Register[i];
                }
                if (er.Count>0)
                {
                    ErrorCode = er.Last().ErrorRegister +"\t"+ er.Last().ErrorDescription;

                }

            }
            else
            {
                _errorHappened = false;
                ErrorCode = "";
            }
            
            

        }

        private void ResetError()
        {
            bool[] errorRegister = new bool[100];

            ModbusClient1.WriteMultipleCoils(9192, errorRegister);
            EasyModbus.StoreLogData.Instance.Store("Reset Error", DateTime.Now);

        }

        internal static void OnWindowsClosing(object sender, CancelEventArgs e)
        {
            Messenger.Default.Send<string>("Close", "Notify");
            EasyModbus.StoreLogData.Instance.Store("Close HMI", DateTime.Now);

        }

        public static implicit operator Window(MainViewModel v)
        {
            throw new NotImplementedException();
        }
    }
    public class DataAccess
    {
       public   DataAccess()
        {
            CreateDatabase();
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + Global.DbFile);
        }
        /// <summary>
        /// 创建数据库和数据表
        /// </summary>
        private static void CreateDatabase()
        {
            if (!System.IO.File.Exists(Global.DbFile))
            {
                SQLiteConnection.CreateFile(Global.DbFile);

            }
            using (var cnn =SimpleDbConnection())
            {
                
                cnn.Execute(
                    @"CREATE TABLE IF NOT EXISTS  ErrorRecord
                      (
                         ParentID                                   INTEGER  primary key AUTOINCREMENT,
                         ID                                         INTEGER  not null,
                         ErrorRegister                              char(100),
                         ErrorDescription                           char(100),
                         Level                                      char(100),
                         ErrorTime                                  datetime 
                      )");
            }
        }
        /// <summary>
        /// 新增单条记录
        /// </summary>
        /// <param name="errorRecord"></param>
        public int InsertErrorRecord(ErrorRecord errorRecord)
        {
            using (IDbConnection connection = SimpleDbConnection())
            {
                const string sql =@"INSERT INTO ErrorRecord(ID,ErrorRegister ,ErrorDescription,Level, ErrorTime) VALUES (@ID, @ErrorRegister, @ErrorDescription, @Level,@ErrorTime)";
             var dapperDemoID= connection.Execute(sql,errorRecord);
                //var dapperDemoID = connection.Query<ErrorRecord>(sql, errorRecord).First();
                return dapperDemoID;
            }
        }
        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ErrorRecord> GetAllRecords()
        {
            // Create a connection to the SQL server
            using (IDbConnection connection = SimpleDbConnection())
            {
                var sql = "SELECT * FROM ErrorRecord";
                //return connection.Query<Movie>("SELECT * FROM Film").ToList(); // straight up SQL not good... use stored procedures.
                return new ObservableCollection<ErrorRecord> (connection.Query<ErrorRecord>(sql));
               
            } // close the connection again, NOTE: Always remember to close connection to the DB when you are done. 
        }
        public ObservableCollection<ErrorRecord> GetRecords(string sql,object param)
        {
            // Create a connection to the SQL server
            using (IDbConnection connection = SimpleDbConnection())
            {
               // var sql = "SELECT * FROM ErrorRecord";
                //return connection.Query<Movie>("SELECT * FROM Film").ToList(); // straight up SQL not good... use stored procedures.
                return new ObservableCollection<ErrorRecord>(connection.Query<ErrorRecord>(sql,param));

            } // close the connection again, NOTE: Always remember to close connection to the DB when you are done. 
        }

        public static int Delete(ErrorRecord errorRecord)
        {
            using (IDbConnection connection = SimpleDbConnection())
            {
                return connection.Execute("delete from ErrorRecord where id=@ID", errorRecord);
            }
        }

        public static int Delete(List<ErrorRecord> errorRecords)
        {
            using (IDbConnection connection = SimpleDbConnection())
            {
                return connection.Execute("delete from ErrorRecord where id=@ID", errorRecords);
            }
        }
        /// <summary>
        /// In操作
        /// </summary>
        public static List<ErrorRecord> QueryIn()
        {
            using (IDbConnection connection = SimpleDbConnection())
            {
                var sql = "select * from ErrorRecord where id in @ids";
                //参数类型是Array的时候，dappper会自动将其转化
                return connection.Query<ErrorRecord>(sql, new { ids = new int[2] { 1, 2 }, }).ToList();
            }
        }

        public static List<ErrorRecord> QueryIn(int[] ids)
        {
            using (IDbConnection connection = SimpleDbConnection())
            {
                var sql = "select * from ErrorRecord where id in @ids";
                //参数类型是Array的时候，dappper会自动将其转化
                return connection.Query<ErrorRecord>(sql, new { ids }).ToList();
            }
        }

        public  void ClearRecords()
        {
            using (IDbConnection connection = SimpleDbConnection())
            {
                 connection.Execute("delete  from ErrorRecord");
            }
        }
    }
}