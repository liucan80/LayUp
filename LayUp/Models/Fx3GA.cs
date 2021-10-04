using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace LayUp.Models
{
    /// <summary>
    /// 该类暴露数据给界面
    /// </summary>
  public  class Fx3GA:ObservableObject
    {
        public Fx3GA()
        {
            for (int i = 0; i < 1024; i++)
            {
                Output.Add(false);
            }
            for (int i = 0; i < 1024; i++)
            {
                Input.Add(false);
            }
            for (int i = 0; i < 7680; i++)
            {
                M.Add(false);
            }
            for (int i = 0; i < 2048; i++)
            {
                SM.Add(false);
            }
            for (int i = 0; i < 8000; i++)
            {
                Data.Add(0);
            }
            for (int i = 0; i < 10000; i++)
            {
                SpecicalData.Add(0);
            }

        }
        public List<string> ConnectionMethods = new List<string>
        {
             "MXComponent","ModbusTCP",
        };
        //PLC连接方式
        private String _connectionMethod;
        public String ConnectionMethod
        {
            get { return _connectionMethod; }
            set { Set(ref _connectionMethod, value); }
        }

        //PLC站号 用来连接MX Component
        private int? _stationNumber;
        public int? StationNumber
        { 
            get { return _stationNumber; }
            set { Set(ref _stationNumber, value); } 
        }

        //PLC的IP地址 用来连接Modbus
        private String _ipAddress="127.0.0.1";
        public String IpAddress
        {
            get { return _ipAddress; }
            set { Set(ref _ipAddress, value); }
        }
        //PLC的端口 用来连接Modbus
        private int? _port=502;
        public int? Port
        {
            get { return _port; }
            set { Set(ref _port, value); }
        }
        //PLC的连接状态
        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
               // _isConnected = value;
                Set(ref _isConnected, value);
              //  _isDisConnected = !value;
               // Set(ref _isDisConnected, !value);


            }
        }

        private bool? _isInManualMode  = null;
        public bool? IsInManualMode
        {
            get { return _isInManualMode; }
            set
            {
                
                Set(ref _isInManualMode, value);

            }
        }
        //PLC CPU type
        private string _CPUName;
        public string CPUName
        {
            get { return _CPUName; }
            set
            {
                Set(ref _CPUName, value);
            }
        }
        //ReturnCode
        private string _returnCode="--";
        public string RetrunCode
        {
            get { return _returnCode; }
            set
            {
                Set(ref _returnCode, value);
            }
        }

        //ErrorCode
        private string _errorCode;
        public string ErrorCode
        {
            get { return _errorCode; }
            set
            {
                Set(ref _errorCode, value);
            }
        }

        #region 输入信号
        private ObservableCollection<bool> _input= new ObservableCollection<bool> { };

        public ObservableCollection<bool> Input
        {
            get { return _input; }
            set { Set(ref _input, value); }
        }
        #endregion

        #region 输出信号

        private ObservableCollection<bool> _output= new ObservableCollection<bool> { };

        public ObservableCollection<bool> Output
        {
            get { return _output; }
            set { Set(ref _output, value); }
        }

        
        #endregion

        #region D寄存器
        private ObservableCollection<int> _data = new ObservableCollection<int> { };

        public ObservableCollection<int> Data
        {
            get { return _data; }
            set { Set(ref _data, value); }
        }
        #endregion

        #region M寄存器
        private ObservableCollection<bool> _M = new ObservableCollection<bool> {  };

        public ObservableCollection<bool> M
        {
            get { return _M; }
            set { Set(ref _M, value); }
        }
        #endregion

        #region SD寄存器

        private ObservableCollection<int> _specialData = new ObservableCollection<int> { };

        public ObservableCollection<int> SpecicalData
        {
            get { return _specialData; }
            set { Set(ref _specialData, value); }
        }

        #endregion
        #region SM寄存器
        //最新自诊断出错(包含报警器ON)
        private ObservableCollection<bool> _SM = new ObservableCollection<bool> { };

        public ObservableCollection<bool> SM
        {
            get { return _SM; }
            set { Set(ref _SM, value); }
        }
        #endregion
    }
}
