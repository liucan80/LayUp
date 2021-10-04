using GalaSoft.MvvmLight.Messaging;
using LayUp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace LayUp.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer DispatcherTimer2;
        //private string _currentTime;
        public MainWindow()
        {
            InitializeComponent();

            //  Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
            //初始化定时器，显示当前时间
            DispatcherTimer2 = new DispatcherTimer { Interval = new TimeSpan(0,0,1)};
            //DispatcherTimer2.Interval = new System.TimeSpan(500);
            DispatcherTimer2.Tick += GetCurrentTime;
            DispatcherTimer2.Start();
            //获取软件版本号
            txtAssemblyVersion.Text=  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Closing += ViewModel.MainViewModel.OnWindowsClosing;
           // txtAssemblyVersion.DataContext = new CurrentTimeViewModel();
        }

     

        private void GetCurrentTime(object sender, EventArgs e)
        {
           TxtCurrentTime.Text = System.DateTime.Now.ToString(); 
           

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
    public class IPValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double myValue = 0;
            if (double.TryParse(value.ToString(), out myValue))
            {
                if (myValue >= 0 && myValue <= 100)
                {
                    return new ValidationResult(true, null);
                }
            }
            return new ValidationResult(false, "Input should between 0 and 100");
        }
    }
    public class FillWrapPanel : Panel
    {
        // Using a DependencyProperty as the backing store for MinItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinItemWidthProperty =
            DependencyProperty.Register("MinItemWidth", typeof(double), typeof(FillWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for MaxItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxItemWidthProperty =
            DependencyProperty.Register("MaxItemWidth", typeof(double), typeof(FillWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for ItemMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(double), typeof(FillWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for RowMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowMarginProperty =
            DependencyProperty.Register("RowMargin", typeof(double), typeof(FillWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for FloorItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloorItemWidthProperty =
            DependencyProperty.Register("FloorItemWidth", typeof(bool), typeof(FillWrapPanel), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool FloorItemWidth
        {
            get { return (bool)GetValue(FloorItemWidthProperty); }
            set { SetValue(FloorItemWidthProperty, value); }
        }

        public double MinItemWidth
        {
            get { return (double)GetValue(MinItemWidthProperty); }
            set { SetValue(MinItemWidthProperty, value); }
        }

        public double MaxItemWidth
        {
            get { return (double)GetValue(MaxItemWidthProperty); }
            set { SetValue(MaxItemWidthProperty, value); }
        }

        public double ItemMargin
        {
            get { return (double)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public double RowMargin
        {
            get { return (double)GetValue(RowMarginProperty); }
            set { SetValue(RowMarginProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return new Size(0, 0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double yOffset = 0.0;
            double xOffset = 0.0;
            int itemCountInRow = CalculateItemsCountInOneRow(finalSize);
            //check zero

            if (itemCountInRow == 0)
            {
                return base.ArrangeOverride(finalSize);
            }
            double itemWidth = CalculateItemWidth(finalSize.Width, itemCountInRow);

            for (int i = 0; i < Children.Count;)
            {
                double rowHeight = 0;
                for (int column = 0; column < itemCountInRow && i + column < Children.Count; column++)
                {
                    UIElement child = Children[i + column];
                    child.Arrange(new Rect(xOffset, yOffset, itemWidth, child.DesiredSize.Height));
                    if (child.DesiredSize.Height > rowHeight)
                    {
                        rowHeight = child.DesiredSize.Height;
                    }

                    xOffset += itemWidth + ItemMargin;
                }

                yOffset += rowHeight + RowMargin;
                xOffset = 0.0;
                i += itemCountInRow;
            }

            return base.ArrangeOverride(finalSize);
        }

        private int CalculateItemsCountInOneRow(Size finalSize)
        {
            // Calling Math.Floor is necessory or not?
            return (int)Math.Floor(((finalSize.Width + ItemMargin) / (MinItemWidth + ItemMargin)));
        }

        private double CalculateItemWidth(double totalWidth, int itemCountInRow)
        {
            if (itemCountInRow > Children.Count)
            {
                itemCountInRow = Children.Count;
            }

            double itemWidth = (totalWidth - (itemCountInRow - 1) * ItemMargin) / itemCountInRow;

            if (itemWidth > MaxItemWidth)
            {
                itemWidth = MaxItemWidth;
            }

            return FloorItemWidth ? Math.Floor(itemWidth) : itemWidth;
        }
    }

  
}
