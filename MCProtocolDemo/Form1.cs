
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyMCProtocol;
using MCProtocol;
using static MCProtocol.Mitsubishi;

namespace MCProtocolDemo
{

    public partial class Form1 : Form
    {
       public McProtocolTcp mcProtocolTcp = new McProtocolTcp("127.0.0.1", 6000, McFrame.MC3E);

        public Form1()
        {
            InitializeComponent();
            // Mitsubishi mitsubishi = new Mitsubishi();
            EasyMCProtocolClient easyMCProtocolClient = new EasyMCProtocolClient("127.0.0.1", 6000);
            easyMCProtocolClient.Connect();

        }


        private void button1_ClickAsync(object sender, EventArgs e)
        {
            //Task.Run(new Action(()=> {  mcProtocolTcp.Open(); }));
            // mcProtocolTcp.Open();
            //testAsync();
            //int[] data=new int[10];
            //data[0] = 1;
            //mcProtocolTcp.GetDevice("X0");
            //Debug.Print(data[0].ToString());
        }

        public async Task<int> testAsync()
        {
            //await  mcProtocolTcp.Open();
            return await mcProtocolTcp.Open();
        }
    }
}
