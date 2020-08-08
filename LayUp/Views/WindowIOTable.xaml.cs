using LayUp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LayUp.Views
{
    /// <summary>
    /// WindowIOTable.xaml 的交互逻辑
    /// </summary>
    public partial class WindowIOTable : Window
    {
        public WindowIOTable()
        {
            InitializeComponent();
            var IOlist1 = new List<IO>() { };

            try
            {
                using (StreamReader sr = new StreamReader("IO.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] elements = line.Split(',');

                        IOlist1.Add(new IO() { Register = elements[0], Description = elements[1] });



                        // dataGrid1.Rows.Add(elements);
                    }
                    dg1.ItemsSource = IOlist1;

                }
            }
            catch (FileNotFoundException exception)
            {
                MessageBox.Show(exception.FileName + "未找到");

                //throw;
            }
            

        }
    }
}
