using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace EasyMCProtocol
{
    public class EasyMCProtocolClient
    {
        private bool debug = false;
        private TcpClient tcpClient;
        private string ipAddress = "127.0.0.1";
        private int port = 5000;
        private bool udpFlag = false;
        private int connectTimeout = 1000;
        NetworkStream stream;
        private bool connected = false;
        public delegate void ConnectedChangedHandler(object sender);

        public event ConnectedChangedHandler ConnectedChanged;

        /// <summary>
		/// Constructor which determines the Master ip-address and the Master Port.
		/// </summary>
		/// <param name="ipAddress">IP-Address of the Master device</param>
		/// <param name="port">Listening port of the Master device</param>
        public EasyMCProtocolClient(string ipAddress, int port)
        {
            if (debug) StoreLogData.Instance.Store("EasyModbus library initialized for Modbus-TCP, IPAddress: " + ipAddress + ", Port: " + port, System.DateTime.Now);
#if (!COMMERCIAL)
            Console.WriteLine("EasyModbus Client Library Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.WriteLine("Copyright (c) Stefan Rossmann Engineering Solutions");
            Console.WriteLine();
#endif
            this.ipAddress = ipAddress;
            this.port = port;
        }

        /// <summary>
        /// Establish connection to Master device in case of Modbus TCP.
        /// </summary>
        public void Connect()
        {
            if (!udpFlag)
            {
                if (debug) StoreLogData.Instance.Store("Open TCP-Socket, IP-Address: " + ipAddress + ", Port: " + port, System.DateTime.Now);
                tcpClient = new TcpClient();
                var result = tcpClient.BeginConnect(ipAddress, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(connectTimeout);
                if (!success)
                {
                    throw new EasyModbus.Exceptions.ConnectionException("connection timed out");
                }
                tcpClient.EndConnect(result);

                //tcpClient = new TcpClient(ipAddress, port);
                stream = tcpClient.GetStream();
                stream.ReadTimeout = connectTimeout;
                connected = true;
            }
            else
            {
                tcpClient = new TcpClient();
                connected = true;
            }
            if (ConnectedChanged != null)
                try
                {
                    ConnectedChanged(this);
                }
                catch
                {

                }
        }
        
        /// <summary>
        /// Close connection to Master Device.
        /// </summary>
        public void Disconnect()
        {
            
            if (stream != null)
                stream.Close();
            if (tcpClient != null)
                tcpClient.Close();
            connected = false;
            if (ConnectedChanged != null)
                ConnectedChanged(this);

        }
        
        ~EasyMCProtocolClient()
        {
           
            if (tcpClient != null & !udpFlag)
            {
                if (stream != null)
                    stream.Close();
                tcpClient.Close();
            }
        }

        /// <summary>
        /// Gets or Sets the IP-Address of the Server.
        /// </summary>
		public string IPAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }

        /// <summary>
        /// Gets or Sets the Port were the Modbus-TCP Server is reachable (Standard is 502).
        /// </summary>
		public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }
    }
}
