using printerFinal.Models;
using System;
using System.Collections.Generic;
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

namespace printerFinal
{
    /// <summary>
    /// helpPage.xaml 的交互逻辑
    /// </summary>
    public partial class helpPage : Window
    {
        public System.Windows.Threading.DispatcherTimer dtimer;

        void dtimer_Tick(object sender, EventArgs e)
        {
            closThis();
        }


        public helpPage()
        {
            InitializeComponent();
        }
        private void closThis()
        {
            dtimer.Stop();
            var a = this.Owner;
            if (a != null)
            {
                (this.Owner as MainWindow).dtimer.Start();
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            closThis();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            foreach (print_type_m a in this.DataContext as List<print_type_m>)
            {
                jobstr.Text += a.printType + " ";
            }
            dtimer = new System.Windows.Threading.DispatcherTimer();
            //每60秒刷新一次
            dtimer.Interval = TimeSpan.FromSeconds(60);
            dtimer.Tick += dtimer_Tick;
            dtimer.Start();
        }

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    Button bt = sender as Button;
        //    textBlock.Text +=" "+bt.Content;
        //}
    }
}
