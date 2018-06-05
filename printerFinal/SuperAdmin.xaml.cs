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
using System.Windows.Shapes;

namespace printerFinal
{
    /// <summary>
    /// SuperAdmin.xaml 的交互逻辑
    /// </summary>
    public partial class SuperAdmin : Window
    {
        public SuperAdmin()
        {
            InitializeComponent();
        }
        BLL.ConfigBLL configbll = new BLL.ConfigBLL();
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Models.Config_m> list = new List<Models.Config_m>();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                Models.Config_m m = new Models.Config_m();
                m.key = key;
                m.value = ConfigurationManager.AppSettings[key];
                list.Add(m);
            }
            dataGrid.ItemsSource = list;
            dataGrid.IsReadOnly = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            List<Models.Config_m> list = dataGrid.ItemsSource as List<Models.Config_m>;
            configbll.SaveConfigSuper(list);
            

            BLL.LogBll logbll = new BLL.LogBll();
            Models.Log_m log = new Models.Log_m("更改设置","Y","");
            

            list.Clear();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                Models.Config_m m = new Models.Config_m();
                m.key = key;
                m.value = ConfigurationManager.AppSettings[key];
                list.Add(m);
                log.node += "[" + m.key + "," + m.value + "]";
            }
            logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);
            dataGrid.ItemsSource = list;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Start st=this.Owner as Start;
            st.dtimer.Start();
        }
    }
}
