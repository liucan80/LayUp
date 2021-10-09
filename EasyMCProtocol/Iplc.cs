using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMCProtocol
{
  public  interface Iplc:IDisposable
    {
        bool Connected { get; }
        Task<int> Open();
        int Close();
        Task<int> SetBitDevice(string iDeviceName, int iSize, int[] iData);
        Task<int> SetBitDevice(Common.PlcDeviceType iType, int iAddress, int iSize, int[] iData);
        Task<int> GetBitDevice(string iDeviceName, int iSize, int[] oData);
        Task<int> GetBitDevice(Common.PlcDeviceType iType, int iAddress, int iSize, int[] oData);
        Task<int> WriteDeviceBlock(string iDeviceName, int iSize, int[] iData);
        Task<int> WriteDeviceBlock(Common.PlcDeviceType iType, int iAddress, int iSize, int[] iData);
        Task<int> WriteDeviceBlock(Common.PlcDeviceType iType, int iAddress, int iSize, byte[] bData);
        Task<byte[]> ReadDeviceBlock(string iDeviceName, int iSize, int[] oData);
        Task<byte[]> ReadDeviceBlock(Common.PlcDeviceType iType, int iAddress, int iSize, int[] oData);
        Task<byte[]> ReadDeviceBlock(Common.PlcDeviceType iType, int iAddress, int iSize);
        Task<int> SetDevice(string iDeviceName, int iData);
        Task<int> SetDevice(Common.PlcDeviceType iType, int iAddress, int iData);
        Task<int> GetDevice(string iDeviceName);
        Task<int> GetDevice(Common.PlcDeviceType iType, int iAddress);
    }
}
