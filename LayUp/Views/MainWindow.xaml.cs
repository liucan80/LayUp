using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace LayUp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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
            if (msg.Notification == "ShowView2")
            {
                var view2 = new Views.SettingView();
                view2.ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Test");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            //  var container=ViewModel.ViewModelLocator.
            
        }
    }
}
