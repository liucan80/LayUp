using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace LayUp.Models
{
    /// <summary>
    /// 该类暴露数据给界面
    /// </summary>
  public  class Fx3GA:ObservableObject
    {
        public List<string> ConnectionMethods = new List<string>
        {
             "MXComponent","ModbusTCP",
        };
       
        
        //PLC连接方式
        private String _connectionMethod="ModbusTCP";
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
        private bool? _isConnected=false;
        public bool? IsConnected
        {
            get { return _isConnected; }
            set
            {
               // _isConnected = value;
                Set(ref _isConnected, value);
                _isDisConnected = !value;
               // Set(ref _isDisConnected, !value);


            }
        }
        //PLC的连接状态  断开时为真
        private bool? _isDisConnected = true;

        public bool? IsDisConnected
        {
            get { return !_isConnected; }
            set
            {
                // _isConnected = value;
                Set(ref _isDisConnected, value);

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
        private string _errorCode="无";
        public string ErrorCode
        {
            get { return _errorCode; }
            set
            {
                Set(ref _errorCode, value);
            }
        }

        #region 输入信号


        private int? _input000;
        public int? Input000
        { get { return _input000; } set
            { Set(ref _input000,value); } }

        private int? _input001;

        public int? Input001
        {
            get { return _input001; }
            set 
            { 
               // _input001 = value;
                Set(ref _input001, value);
            }
        }
        private int? _input002;

        public int? Input002
        {
            get { return _input002; }
            set {  Set(ref _input002, value); }
        }
        private int? _input003;

        public int? Input003
        {
            get { return _input003; }
            set 
            {
                //_input001 = value;
                Set(ref _input003, value);
            }
        }
        private int? _input004;

        public int? Input004
        {
            get { return _input004; }
            set
            {
                //_input001 = value;
                Set(ref _input004, value);
            }
        }
        private int? _input005;

        public int? Input005
        {
            get { return _input005; }
            set
            {
                //_input001 = value;
                Set(ref _input005, value);
            }
        }
        private int? _input006;

        public int? Input006
        {
            get { return _input006; }
            set
            {
                //_input001 = value;
                Set(ref _input006, value);
            }
        }
        private int? _input007;

        public int? Input007
        {
            get { return _input007; }
            set
            {
                //_input001 = value;
                Set(ref _input007, value);
            }
        }
        private int? _input010;
        public int? Input010
        {
            get { return _input010; }
            set
            { Set(ref _input010, value); }
        }

        private int? _input011;

        public int? Input011
        {
            get { return _input011; }
            set
            {
               
                Set(ref _input011, value);
            }
        }
        private int? _input012;

        public int? Input012
        {
            get { return _input012; }
            set {  Set(ref _input012, value); }
        }
        private int? _input013;

        public int? Input013
        {
            get { return _input013; }
            set
            {
               
                Set(ref _input013, value);
            }
        }
        private int? _input014;

        public int? Input014
        {
            get { return _input014; }
            set
            {
                
                Set(ref _input014, value);
            }
        }
        private int? _input015;

        public int? Input015
        {
            get { return _input015; }
            set
            {
               
                Set(ref _input015, value);
            }
        }
        private int? _input016;

        public int? Input016
        {
            get { return _input016; }
            set
            {
                
                Set(ref _input016, value);
            }
        }
        private int? _input017;

        public int? Input017
        {
            get { return _input017; }
            set
            {
               
                Set(ref _input017, value);
            }
        }
        private int? _input020;
        public int? Input020
        {
            get { return _input020; }
            set
            { Set(ref _input020, value); }
        }

        private int? _input021;

        public int? Input021
        {
            get { return _input021; }
            set
            {

                Set(ref _input021, value);
            }
        }
        private int? _input022;

        public int? Input022
        {
            get { return _input022; }
            set { Set(ref _input022, value); }
        }
        private int? _input023;

        public int? Input023
        {
            get { return _input023; }
            set
            {

                Set(ref _input023, value);
            }
        }
        private int? _input024;

        public int? Input024
        {
            get { return _input024; }
            set
            {

                Set(ref _input024, value);
            }
        }
        private int? _input025;

        public int? Input025
        {
            get { return _input025; }
            set
            {

                Set(ref _input025, value);
            }
        }
        private int? _input026;

        public int? Input026
        {
            get { return _input026; }
            set
            {

                Set(ref _input026, value);
            }
        }
        private int? _input027;

        public int? Input027
        {
            get { return _input027; }
            set
            {

                Set(ref _input027, value);
            }
        }
        #endregion

        #region 输出信号

        private int? _output000;    

        public int? Output000
        {
            get { return _output000; }
            set 
            {
                Set(ref _output000, value);
            }
        }
        private int? _output001;

        public int? Output001
        {
            get { return _output001; }
            set
            {
                Set(ref _output001, value);
            }
        }
        private int? _output002;
        public int? Output002
        {
            get { return _output002; }
            set
            {
                Set(ref _output002, value);
            }
        }
        private int? _output003;
        public int? Output003
        {
            get { return _output003; }
            set
            {
                Set(ref _output003, value);
            }
        }
        private int? _output004;
        public int? Output004
        {
            get { return _output004; }
            set
            {
                Set(ref _output004, value);
            }
        }
        private int? _output005;
        public int? Output005
        {
            get { return _output005; }
            set
            {
                Set(ref _output005, value);
            }
        }
        private int? _output006;
        public int? Output006
        {
            get { return _output006; }
            set
            {
                Set(ref _output006, value);
            }
        }
        private int? _output007;
        public int? Output007
        {
            get { return _output007; }
            set
            {
                Set(ref _output007, value);
            }
        }
        private int? _output010;

        public int? Output010
        {
            get { return _output010; }
            set
            {
                Set(ref _output010, value);
            }
        }
        private int? _output011;

        public int? Output011
        {
            get { return _output011; }
            set
            {
                Set(ref _output011, value);
            }
        }
        private int? _output012;
        public int? Output012
        {
            get { return _output012; }
            set
            {
                Set(ref _output012, value);
            }
        }
        private int? _output013;
        public int? Output013
        {
            get { return _output013; }
            set
            {
                Set(ref _output013, value);
            }
        }
        private int? _output014;
        public int? Output014
        {
            get { return _output014; }
            set
            {
                Set(ref _output014, value);
            }
        }
        private int? _output015;
        public int? Output015
        {
            get { return _output015; }
            set
            {
                Set(ref _output015, value);
            }
        }
        private int? _output016;
        public int? Output016
        {
            get { return _output016; }
            set
            {
                Set(ref _output016, value);
            }
        }
        private int? _output017;
        public int? Output017
        {
            get { return _output017; }
            set
            {
                Set(ref _output017, value);
            }
        }
        #endregion

        #region D寄存器
        private int? _data200;

        public int?Data200
        {
            get { return _data200; }
            set
            {
                Set(ref _data200, value);
            }
        }
        private int? _data201;

        public int? Data201
        {
            get { return _data201; }
            set
            {
                Set(ref _data201, value);
            }
        }
        private int? _data202;

        public int? Data202
        {
            get { return _data202; }
            set
            {
                Set(ref _data202, value);
            }
        }
        private int? _data210;

        public int? Data210
        {
            get { return _data210; }
            set
            {
                Set(ref _data210, value);
            }
        }
        private int? _data211;

        public int? Data211
        {
            get { return _data211; }
            set
            {
                Set(ref _data211, value);
            }
        }
        private int? _data212;

        public int? Data212
        {
            get { return _data212; }
            set
            {
                Set(ref _data212, value);
            }
        }
        #endregion

        #region M寄存器
        private int? _M231;

        public int? M231
        {
            get { return _M231; }
            set
            {
                Set(ref _M231, value);
            }
        }
        private int? _M232;

        public int? M232
        {
            get { return _M232; }
            set
            {
                Set(ref _M232, value);
            }
        }
        private int? _M233;

        public int? M233
        {
            get { return _M233; }
            set
            {
                Set(ref _M233, value);
            }
        }
        private int? _M234;

        public int? M234
        {
            get { return _M234; }
            set
            {
                Set(ref _M234, value);
            }
        }
        private int? _M235;

        public int? M235
        {
            get { return _M235; }
            set
            {
                Set(ref _M235, value);
            }
        }
        #endregion
    }
}
