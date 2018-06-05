using Newtonsoft.Json.Linq;
using printerFinal.BLL;
using printerFinal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Printing;
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
    /// PrintingPage.xaml 的交互逻辑
    /// </summary>
    public partial class PrintingPage : Window
    {

        //public List<detail_m> details;

        int num = 0;
        int wrongnum = 0;
        int timenum = 0;
        int preJobNum = 0;
        int sameJobNum = 0;

        public System.Windows.Threading.DispatcherTimer dtimer;

        HttpBLL httpbll = new HttpBLL();
        JSONBLL jsonbll = new JSONBLL();
        ConfigBLL configbll = new ConfigBLL();
        LogBll logbll = new LogBll();
        public PrintingPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 打印出错状态处理
        /// </summary>
        public void PrintWrong()
        {
            PrintServer ps = new PrintServer();
            PrintQueue queue = ps.GetPrintQueue(ConfigurationManager.AppSettings["printer"]);
            //int sendResult = 0;
            try
            {
                string proResult = "";
                getWrongMsg(ref proResult, queue);
                //messgeBoxBll msgBox = new messgeBoxBll();
                messgeBoxBll.Show("打印机出现问题，已经删除打印任务，本次操作不会计费", "问题排查：\n" + proResult);

                configbll.SaveConfig("printerStatus", "wrong");

                //if (queue.NumberOfJobs == 0 || wrongnum > 6)
                //{
                JObject jo;
                //上传结果
                string result = httpbll.updatePrintStatus("0");

                jsonbll.jsonToJobject(result, out jo);
                if (jo["code"].ToString() == "200")
                {
                    messgeBoxBll.Show("结果上传成功", jo["msg"].ToString());

                    //if (wrongnum > 6)
                    //{
                    //    messgeBoxBll.Show( "无法清理残余任务请联系管理员","问题排查：\n" + proResult);
                    //}

                    if (sameJobNum > 30)
                    {
                        messgeBoxBll.Show("超时", "打印机停留在同一任务时间过长");
                        proResult += "打印机停留在同一任务时间过长";
                    }
                    Log_m log = new Log_m("打印出错上传结果", "Y", "问题：" + proResult);
                    logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);

                }
                else
                {
                    messgeBoxBll.Show("结果上传失败", jo["msg"].ToString());
                    //if (wrongnum > 6)
                    //{
                    //    messgeBoxBll.Show("无法清理残余任务请联系管理员", "问题排查：\n" + proResult);
                    //}
                    Log_m log = new Log_m("打印出错上传结果", "N", "问题：" + proResult+ jo["msg"].ToString());
                    logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);
                }

                //PrintBLL printbll = new PrintBLL();
                //printbll.ClearJobs();
                //}
                //else
                //{
                //  messgeBoxBll.Show("...........","正在还原初始状态");
                //}
            }
            catch (Exception ex)
            {
                messgeBoxBll.Show(ex.Message, "意外错误");
                Log_m log = new Log_m("打印出错上传结果", "N", "问题：" + ex.Message);
                logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);

                //PrintBLL printbll = new PrintBLL();
                //printbll.ClearJobs();
            }
        }
        /// <summary>
        /// 打印成功处理
        /// </summary>
        public int PrintSuccess()
        {
            int resualt = 0;
            try
            {
                //上报纸张减少
                JObject jo;
                string result1 = httpbll.updatePageNum(App.psta.Count);
                jsonbll.jsonToJobject(result1, out jo);
                if (jo["code"].ToString() == "200")
                {
                    BLL.messgeBoxBll.Show("纸张上报成功", jo["msg"].ToString());
                    Log_m log = new Log_m("打印成功上传结果", "Y", "纸张");
                    logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);
                    //上传结果至服务器
                    string result = httpbll.updatePrintStatus("0");
                    jsonbll.jsonToJobject(result, out jo);
                    if (jo["code"].ToString() == "200")
                    {
                        BLL.messgeBoxBll.Show("结果上传成功", jo["msg"].ToString());
                        log = new Log_m("打印成功上传结果", "Y", "结果");
                        logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);

                        //计算并相应减少纸张
                        App.set.remainPageNum -= App.psta.Count;
                        App.set.printePageNum += App.psta.Count;
                        configbll.SaveConfig("remainPageNum", App.set.remainPageNum.ToString());
                        configbll.SaveConfig("printePageNum", App.set.printePageNum.ToString());

                    }
                    else
                    {
                        BLL.messgeBoxBll.Show("结果上传失败", jo["msg"].ToString());
                        log = new Log_m("打印成功上传结果", "N", "结果问题：code" + jo["msg"].ToString());
                        logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);
                        resualt = 1;
                    }
                }
                else
                {
                    BLL.messgeBoxBll.Show("纸张上报失败", jo["msg"].ToString());
                    Log_m log = new Log_m("打印成功上传结果", "N", "纸张问题：code" + jo["msg"].ToString());
                    logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);
                    resualt = 1;
                }

                return resualt;
            }
            catch (Exception ex)
            {
                BLL.messgeBoxBll.Show("意外的错误", ex.Message);
                Log_m log = new Log_m("打印成功上传结果", "N", "问题：" + ex.Message);
                logbll.AddLog(log, ConfigurationManager.AppSettings["logFile"]);

                return 1;
            }
        }
        /// <summary>
        ///   定时刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dtimer_Tick(object sender, EventArgs e)
        {
            timenum++;
            PrintBLL printbll = new PrintBLL();

            PrintServer ps = new PrintServer();
            PrintQueue queue = ps.GetPrintQueue(ConfigurationManager.AppSettings["printer"]);

            printbll.GetJobs(ConfigurationManager.AppSettings["printer"]);

            string str = getStatus();
            if (queue.IsInError || queue.IsPaperJammed || queue.IsServerUnknown || queue.IsOutOfPaper || queue.HasPaperProblem || sameJobNum > 30)//|| str == "2" || str == "8")
            {
                wrongnum++;
                if (wrongnum > 3)
                {
                    PrintWrong();
                    
                    closeThis();
                }
                ps.Dispose();
                queue.Dispose();
                return;
            }

            //界面反馈
            if (queue.NumberOfJobs > 0)
            {

                if (queue.NumberOfJobs == preJobNum)
                {
                    sameJobNum++;
                    progress.Value += 1;
                }
                else
                {
                    preJobNum = queue.NumberOfJobs;
                    sameJobNum = 0;
                    progress.Value = (100 * (App.psta.Count - preJobNum) / App.psta.Count);
                }
                num = 0;
                nowCountText.Text = (App.psta.Count - preJobNum).ToString();
                ps.Dispose();
                queue.Dispose();
                return;
            }
            //打印结束
            if (queue.NumberOfJobs == 0)
            {
                progress.Value = 100;
                num++;
                ps.Dispose();
                queue.Dispose();
                if (num >= 3)
                {
                    //成功结束
                    if (PrintSuccess() == 0 || num > 6)
                    {
                        if (num > 6)
                            messgeBoxBll.Show("与服务器通信失败", "因网络问题通信失败，本次打印不会扣费");
                        closeThis();
                    }
                }
            }
        }
        /// <summary>
        /// 关闭打印过程
        /// </summary>
        private void closeThis()
        {
            dtimer.Stop();
            //PrintBLL printbll = new PrintBLL();
            //printbll.ClearJobs();

            double x1 = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
            double y1 = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度
            if (x1 / y1 == 16.0 / 9.0)
            {
                MainWindow a = Owner as MainWindow;
                a.dtimer.Start();
                a.adtimer.Start();
                a.stadtimer.Start();
                a.Show();
                this.Close();
            }
            else if (x1 / y1 == 4.0 / 3.0)
            {
                main43 a = Owner as main43;
                a.dtimer.Start();
                a.adtimer.Start();
                a.stadtimer.Start();
                a.Show();
                this.Close();
            }
            else
            {
                MainWindow a = Owner as MainWindow;
                a.dtimer.Start();
                a.adtimer.Start();
                a.stadtimer.Start();
                a.Show();
                this.Close();
                messgeBoxBll.Show("不是程序的最佳显示效果", "建议设置为16：9 或 4：3 屏幕分辨率");
            }
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            PrintServer ps = new PrintServer();
            PrintQueue queue = ps.GetPrintQueue(ConfigurationManager.AppSettings["printer"]);
            if (queue.NumberOfJobs == 0)
            {
                ps.Dispose();
                queue.Dispose();
                closeThis();
            }
            else
            {
                ps.Dispose();
                queue.Dispose();
                MessageBox.Show("请耐心等待","打印还未完成");
            }
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            this.DataContext = App.psta;
            dtimer = new System.Windows.Threading.DispatcherTimer();

            //每3秒刷新一次
            dtimer.Interval = TimeSpan.FromSeconds(2);
            dtimer.Tick += dtimer_Tick;
            dtimer.Start();
        }
        /// <summary>  
        /// 获取打印机状态  
        /// </summary>  
        /// <returns></returns>  
        public string getStatus()
        {
            string path = @"win32_printer.DeviceId='" + ConfigurationManager.AppSettings["Printer"] + "'";
            try
            {
                ManagementObject printer = new ManagementObject(path);
                printer.Get();
                string a = printer.Properties["PrinterStatus"].Value.ToString();
                printer.Dispose();
                return a;
            }
            catch (Exception ex)
            {
                return ("出错" + ex.Message);
            }
        }
        /// <summary>
        /// 排查出错信息
        /// </summary>
        /// <returns></returns>
        public void getWrongMsg(ref string statusReport, PrintQueue pq)
        {
            if (pq.HasPaperProblem)
            {
                statusReport = statusReport + "Has a paper problem. ";
            }
            if (!(pq.HasToner))
            {
                statusReport = statusReport + "Is out of toner. ";
            }
            if (pq.IsDoorOpened)
            {
                statusReport = statusReport + "Has an open door. ";
            }
            if (pq.IsInError)
            {
                statusReport = statusReport + "Is in an error state. ";
            }
            if (pq.IsNotAvailable)
            {
                statusReport = statusReport + "Is not available. ";
            }
            if (pq.IsOffline)
            {
                statusReport = statusReport + "Is off line. ";
            }
            if (pq.IsOutOfMemory)
            {
                statusReport = statusReport + "Is out of memory. ";
            }
            if (pq.IsOutOfPaper)
            {
                statusReport = statusReport + "Is out of paper. ";
            }
            if (pq.IsOutputBinFull)
            {
                statusReport = statusReport + "Has a full output bin. ";
            }
            if (pq.IsPaperJammed)
            {
                statusReport = statusReport + "Has a paper jam. ";
            }
            if (pq.IsPaused)
            {
                statusReport = statusReport + "Is paused. ";
            }
            if (pq.IsTonerLow)
            {
                statusReport = statusReport + "Is low on toner. ";
            }
            if (pq.NeedUserIntervention)
            {
                statusReport = statusReport + "Needs user intervention. ";
            }
            if (statusReport == "")
            {
                statusReport = "无法监测到的错误";
            }
        }
    }
}
