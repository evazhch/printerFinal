using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace printerFinal
{
    /// <summary>
    /// PrintSource.xaml 的交互逻辑
    /// </summary>
    public partial class PrintSource : Window
    {
        BLL.ConfigBLL configbll = new BLL.ConfigBLL();

        public PrintSource()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Start start = new Start();
            start.Show();
            this.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            imgxg.Visibility = Visibility.Visible;
            imgpg.Visibility = Visibility.Hidden;
            xgbtn.Visibility = Visibility.Visible;
            pageadgrid.Visibility = Visibility.Hidden;
            //pgbtn.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            imgxg.Visibility = Visibility.Hidden;
            imgpg.Visibility = Visibility.Visible;
            xgbtn.Visibility = Visibility.Hidden;
            pageadgrid.Visibility = Visibility.Visible;
            //pgbtn.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" 成功");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            configbll.SaveConfig("remainPageNum", slider.Value.ToString());
            App.set = new Models.SettingModel();
            MessageBox.Show("纸张记录成功", "当前打印机剩余纸张:" + ConfigurationManager.AppSettings["remainPageNum"]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            slider.Value = int.Parse(ConfigurationManager.AppSettings["remainPageNum"]);
        }
    }
}
