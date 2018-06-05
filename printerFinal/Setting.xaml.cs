using printerFinal.Models;
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
    /// Setting2.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        BLL.ConfigBLL configbll = new BLL.ConfigBLL();
        BLL.LogBll logbll = new BLL.LogBll();

        public Setting()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {

            var a = App.set.Id;
            try
            {
                configbll.SaveConfig("id", App.set.Id);
                configbll.SaveConfig("universityCode", App.set.universityCode);

                configbll.SaveConfig("position", App.set.position);

                configbll.SaveConfig("IPposition", App.set.IPposition);
                configbll.SaveConfig("TeamviewerId", App.set.TeamviewerId);

                configbll.SaveConfig("ContactName", App.set.ContactName);
                configbll.SaveConfig("ContactPhoto", App.set.ContactPhone);

                //BLL.ConfigBLL.SaveConfig("code", App.set.code);
                //BLL.ConfigBLL.SaveConfig("nowStatus", App.set.nowStatus);
                //重新加载set
                App.set = new Models.SettingModel();

                MessageBox.Show("本地更新成功","id:" + ConfigurationManager.AppSettings["id"] + "\n"
                                                    + "code" + ConfigurationManager.AppSettings["code"] + "\n");
                Log_m log = new Log_m();
                log.datetime = DateTime.Now;
                log.code = App.set.code;
                log.role = App.user.role;
                log.text = "配置修改";
                log.result = "Y";
                log.node = "id: " + ConfigurationManager.AppSettings["id"] + " "
                       + " universityCode:" + ConfigurationManager.AppSettings["universityCode"] + " "
                       + " position:" + ConfigurationManager.AppSettings["position"] + " "
                       + " IPposition:" + ConfigurationManager.AppSettings["IPposition"] + " "
                       + " TeamviewerId:" + ConfigurationManager.AppSettings["TeamviewerId"] + " "
                       + " ContactName:" + ConfigurationManager.AppSettings["ContactName"] + " "
                       + " ContactPhoto:" + ConfigurationManager.AppSettings["ContactPhoto"] + " ";
                logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"本地更新失败");
            }

            //保存配置信息并上传

        }

        private void closbtn_Click(object sender, RoutedEventArgs e)
        {
            Start start = new Start();
            start.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
