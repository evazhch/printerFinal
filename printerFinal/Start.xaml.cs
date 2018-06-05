using MahApps.Metro.Controls;
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
using System.Windows.Threading;

namespace printerFinal
{
    /// <summary>
    /// Start.xaml 的交互逻辑
    /// </summary>
    public partial class Start : MetroWindow
    {
        private static bool IsStart = Convert.ToBoolean(ConfigurationManager.AppSettings["IsStart"]);
        public DispatcherTimer dtimer;



        public Start()
        {
            this.UseNoneWindowStyle = true;
            this.IgnoreTaskbarOnMaximize = true;
            InitializeComponent();

            //开机自启设置代码
            //if (!IsExistKey("PrinterThird") && IsStart)
            //{
            //    SelfRunning(IsStart, "PrinterThird", @"C:\Users\FZC\Desktop\PrinterThird\PrinterThird\bin\Debug\PrinterThird.exe");
            //}
            //else if (IsExistKey("PrinterThird") && !IsStart)
            //{
            //    SelfRunning(!IsStart, "PrinterThird", @"C:\Users\FZC\Desktop\PrinterThird\PrinterThird\bin\Debug\PrinterThird.exe");
            //}
        }


        /// <summary>
        /// 配置查看按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            dtimer.Stop();
            land2 ad = new land2();
            ad.state = 0;
            ad.ShowDialog();
            this.Close();
        }
        /// <summary>
        /// 最大化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            dtimer.Stop();
            double x1 = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
            double y1 = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度
            if (x1 / y1 == 16.0 / 9.0)
            {
                MainWindow mv = new MainWindow();
                this.Close();
                mv.WindowState = WindowState.Maximized;
                mv.Show();
            }
            else if (x1 / y1 == 4.0 / 3.0)
            {
                //main43 mv = new main43();
                //this.Close();
                //mv.WindowState = WindowState.Maximized;
                //mv.Show();
            }
            else
            {
                MainWindow mv = new MainWindow();
                this.Close();
                mv.WindowState = WindowState.Maximized;
                mv.Show();
                MessageBox.Show("不是程序的最佳显示效果，建议设置为16：9 或 4：3 屏幕分辨率");
            }

        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dtimer.Stop();
            this.Close();
        }
        /// <summary>
        /// 资源管理按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {
            dtimer.Stop();
            land2 ad = new land2();
            ad.state = 1;
            ad.ShowDialog();
            this.Close();
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //开机自启
            //BLL.Core.SelfRunning(true, "自助打印", @"C: \Users\FZC\Desktop\PrinterThird\PrinterThird\bin\Debug\PrinterThird.exe");

            //初始化一些全局参数
            App.set = new Models.SettingModel();
            App.set.code = ConfigurationManager.AppSettings["code"];
            App.set.universityCode = ConfigurationManager.AppSettings["universityCode"];

            //加载键盘
            string str1 = App.set.code;
            string str2 = App.set.universityCode;

            textBox_GotFocus(sender, e);
            //land ld1 = new land();
            //fram.Content = ld1;

            //加载时钟
            dtimer = new System.Windows.Threading.DispatcherTimer();

            //2分钟过后跳转main
            dtimer.Interval = TimeSpan.FromMinutes(2);
            dtimer.Tick += dtimer_Tick;
            dtimer.Start();

            //键盘响应事件
            this.KeyDown += ModifyPrice_KeyDown;
        }
        /// <summary>
        /// 跳转到main
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtimer_Tick(object sender, EventArgs e)
        {
            dtimer.Stop();
            double x1 = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
            double y1 = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度
            if (x1 / y1 == 16.0 / 9.0)
            {
                MainWindow mv = new MainWindow();
                this.Close();
                mv.WindowState = WindowState.Maximized;
                mv.Show();
            }
            else if (x1 / y1 == 4.0 / 3.0)
            {
                main43 mv = new main43();
                this.Close();
                mv.WindowState = WindowState.Maximized;
                mv.Show();
            }
            else
            {
                MainWindow mv = new MainWindow();
                this.Close();
                mv.WindowState = WindowState.Maximized;
                mv.Show();
                MessageBox.Show("不是程序的最佳显示效果，建议设置为16：9 或 4：3 屏幕分辨率");
            }
        }
        /// <summary>
        /// 超级管理员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
                dtimer.Stop();
                SuperAdmin sa = new SuperAdmin();
                sa.Owner = this;
                sa.ShowDialog();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == ConfigurationManager.AppSettings["adminUser"] && textBox1.Text == ConfigurationManager.AppSettings["adminPass"])
            {
                this.dtimer.Stop();
                SuperAdmin sa = new SuperAdmin();
                sa.Owner = this;
                sa.Show();
            }
            else
            {
                MessageBox.Show("账户密码错误");
            }

            //PrintingPage ptpg = new PrintingPage();
            //ptpg.ShowDialog();
        }
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NumKeyBoard nkb = new NumKeyBoard();
            nkb.tb = textBox;
            keyFram.Content = nkb;
        }
        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            NumKeyBoard nkb = new NumKeyBoard();
            nkb.tb = textBox1;
            keyFram.Content = nkb;
        }
        /// <summary>
        /// 判断注册表键值对是否存在
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        //private bool IsExistKey(string keyName)
        //{
        //    try
        //    {
        //        bool _exist = false;
        //        RegistryKey local = Registry.LocalMachine;
        //        RegistryKey runs = local.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        //        if (runs == null)
        //        {
        //            RegistryKey key2 = local.CreateSubKey("SOFTWARE");
        //            RegistryKey key3 = key2.CreateSubKey("Microsoft");
        //            RegistryKey key4 = key3.CreateSubKey("Windows");
        //            RegistryKey key5 = key4.CreateSubKey("CurrentVersion");
        //            RegistryKey key6 = key5.CreateSubKey("Run");
        //            runs = key6;
        //        }
        //        string[] runsName = runs.GetValueNames();
        //        foreach (string strName in runsName)
        //        {
        //            if (strName.ToUpper() == keyName.ToUpper())
        //            {
        //                _exist = true;
        //                return _exist;
        //            }
        //        }
        //        return _exist;

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return false;
        //    }
        //}
        ///isStart--是否开机自启动
        ///exeName--应用程序名
        ///path--应用程序路径
        //private bool SelfRunning(bool isStart, string exeName, string path)
        //{
        //    try
        //    {
        //        RegistryKey local = Registry.LocalMachine;
        //        RegistryKey key = local.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        //        if (key == null)
        //        {
        //            local.CreateSubKey("SOFTWARE//Microsoft//Windows//CurrentVersion//Run");
        //        }
        //        if (isStart)//若开机自启动则添加键值对
        //        {
        //            key.SetValue(exeName, path);
        //            key.Close();
        //        }
        //        else//否则删除键值对
        //        {
        //            string[] keyNames = key.GetValueNames();
        //            foreach (string keyName in keyNames)
        //            {
        //                if (keyName.ToUpper() == exeName.ToUpper())
        //                {
        //                    key.DeleteValue(exeName);
        //                    key.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        //throw;
        //    }

        //    return true;
        //}
    }
}
