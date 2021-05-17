using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using LayUp.Models;
using LayUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace LayUp.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<IO> _IOList;
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<IO> IOList
        {
            get { return _IOList; }
            set { _IOList = value; }
        }
        private readonly DispatcherTimer DispatcherTimer2;
        private string _currentTime;
        public MainWindow()
        {
            InitializeComponent();
           
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
            //初始化定时器，显示当前时间
            DispatcherTimer2 = new DispatcherTimer();
            DispatcherTimer2.Interval = new System.TimeSpan(500);
            DispatcherTimer2.Tick += GetCurrentTime;
            DispatcherTimer2.Start();
            //获取软件版本号
          txtAssemblyVersion.Text=  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void GetCurrentTime(object sender, EventArgs e)
        {
            _currentTime = System.DateTime.Now.ToString();
            TxtCurrentTime .Text= _currentTime;
            
        }
        private void NotificationMessageReceived(NotificationMessage msg)
        {
            if (msg.Notification == "View2")
            {
                var view2 = new Views.SettingView();
                view2.ShowDialog();
            }
            if (msg.Notification == "IOTable")
            {
                
                var IOTable = new Views.WindowIOTable();
                IOTable.ShowDialog();
            }

        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)e.OriginalSource).Tag.ToString())
            {
                case ("MenuNormal"):
                    this.WindowState = WindowState.Normal;
                    break;
                case ("MenuMin"):
                    this.WindowState = WindowState.Minimized;
                    break;
                case ("MenuClose"):
                    this.Close();
                    break;
                default:
                    break;
            }    ;
        }
    }
}
