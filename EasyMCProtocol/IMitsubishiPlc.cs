using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMCProtocol
{
   public interface IMitsubishiPlc:IDisposable
    {
        bool Connected { get; }
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <returns>连接成功返回0 连接失败非0</returns>
        int Connect();

        /// <summary>
        /// 断开设备连接
        /// </summary>
        void DisConnect();
        bool SetBitDevice(string iDeviceName, int iSize, int[] iData);
        
        void GetBitDevice(string iDeviceName, int iSize, int[] oData);
        void WriteBitDeviceBlock(string iDeviceName, int iSize, int[] iData);
        
        bool[] ReadBitDeviceBlock(string iDeviceName, int iSize, int[] oData);
        void WriteDeviceBlock(string iDeviceName, int iSize, int[] iData);

        bool[] ReadDeviceBlock(string iDeviceName, int iSize, int[] oData);

        void SetDevice(string iDeviceName, int iData);
        int GetDevice(string iDeviceName);
        //Task<int> GetDevice(PlcDeviceType iType, int iAddress);
        //Task<int> SetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] iData);
        //Task<int> GetBitDevice(PlcDeviceType iType, int iAddress, int iSize, int[] oData);
        //Task<int> WriteDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] iData);
        //Task<int> WriteDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, byte[] bData);
        //Task<byte[]> ReadDeviceBlock(PlcDeviceType iType, int iAddress, int iSize, int[] oData);
        //Task<byte[]> ReadDeviceBlock(PlcDeviceType iType, int iAddress, int iSize);
        //Task<int> SetDevice(PlcDeviceType iType, int iAddress, int iData);
    }
}
