using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using printerFinal.BLL;
using printerFinal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace printerFinal
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 变量区域
        public System.Windows.Threading.DispatcherTimer dtimer; //主程序刷新定时器
        public System.Windows.Threading.DispatcherTimer adtimer; //广告刷新定时器
        public System.Windows.Threading.DispatcherTimer stadtimer; //机器状态定时器
        JSONBLL jsonbll = new JSONBLL();
        private delegate void DoPrintMethod(PrintDialog pdlg, DocumentPaginator paginator,string name,int num);
        enum Jobs
        {
            成绩单 = 1,
            学籍证明
        }
        struct Appstat
        {
            public bool line; //网络是否连接
            public string stat;
        }
        Appstat stat;//表示机器状态参数
        public struct AdStruct //广告结构体
        {
            public string url { get; set; }
            public int adStopTime { get; set; }
        }
        List<AdStruct> adList = new List<AdStruct>(); //广告列表变量
        int adnum = 0; //广告的数量
       
        #endregion
        #region 广告及状态处理方法
        /// <summary>
        /// 取得广告列表
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<AdStruct> getAdList(string url,Dictionary<string,string> dic)
        {
            HttpBLL httpbll = new HttpBLL();
            List<AdStruct> list = new List<AdStruct>();

            string str = httpbll.GetResponseString(httpbll.CreatePostHttpResponse(url, dic));

            if (str != null)
            {
                JObject jo = JsonConvert.DeserializeObject<JObject>(str);
                

                if (jo["code"].ToString() == "200")
                {
                    var ja = jo["ad"];
                    foreach (var a in ja)
                    {
                        AdStruct adstruct = new AdStruct();
                        adstruct.adStopTime = int.Parse(a["adStopTime"].ToString());
                        adstruct.url = a["adUrl"].ToString();
                        list.Add(adstruct);
                    }
                    return list;
                }
                else
                {
                    return list;
                }
            }
            else
            {
                return list;
            }
        }
        /// <summary>
        ///   定时刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dtimer_Tick(object sender, EventArgs e)
        {
            dtimer.Stop();
            adtimer.Stop();
            stadtimer.Stop();
            MainWindow mv = new MainWindow();
            mv.Show();
            this.Close();
        }
        /// <summary>
        /// 状态检测方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stadtimer_Tick(object sender, EventArgs e)
        {
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
            options.DontFragment = true;
            string data = "Test Data!";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 2000; // Timeout 时间，单位：毫秒

            //检查网络连接
            try
            {
                System.Net.NetworkInformation.PingReply reply = p.Send("baidu.com", timeout, buffer, options);
                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    stat.line = true;
                    netLine.Text = "网络已连接";
                    netLine.Foreground = Brushes.Green;
                }
                else
                {
                    stat.line = false;
                    netLine.Text = "网络连接已断开";
                    netLine.Foreground = Brushes.Red;
                }
            }
            catch
            {
                stat.line = false;
                netLine.Text = "网络异常，请联系管理员";
                netLine.Foreground = Brushes.Red;
            }

            if (stat.line == true)
            {
                //从网络获取机器状态
                HttpBLL httpbll = new HttpBLL();
                JSONBLL jsonbll = new JSONBLL();

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("id", ConfigurationManager.AppSettings["id"]);

                string str = httpbll.GetResponseString(httpbll.CreatePostHttpResponse(ConfigurationManager.AppSettings["machine"], dic));

                if (str != null)
                {
                    JObject jo = JsonConvert.DeserializeObject<JObject>(str);

                    if (jo["code"].ToString() == "200")
                    {
                        JObject jo1;
                        string str1 = jo["machine"].ToString();
                        if (str1 != null)
                        {
                            jsonbll.jsonToJobject(str1, out jo1);
                            stat.stat = jo1["nowStatus"].ToString();
                        }
                        else
                        {
                            stat.stat = "查无此机,若要正常使用，请检查配置文件与服务器";
                        }
                    }
                    else
                    {
                        stat.stat = "意外错误，未能与服务器正常通信";
                    }
                }
                else
                {
                    stat.stat = "意外错误，通信失败";
                }
            }
            else
            {
                stat.stat = "连接失败";
            }

            pageRemain.Text = App.set.remainPageNum.ToString();

            try
            {
                PrintServer ps = new PrintServer();
                PrintQueue queue = ps.GetPrintQueue(ConfigurationManager.AppSettings["printer"]);
                printerStaTxt.Text = queue.QueueStatus.ToString();
            }
            catch
            {
                printerStaTxt.Text = "打印服务未启动";
            }
            
        }
        /// <summary>
        /// 广告刷新方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adtimer_Tick(object sender, EventArgs e)
        {
            if (adList.Count>0)
            {
                    if (adnum >= adList.Count)
                    {
                        adnum = 0;
                        AdStruct adstruct = new AdStruct();
                        adstruct = adList[adnum];
                        adwin.Source = new BitmapImage(new Uri(adstruct.url));
                        adtimer.Interval = TimeSpan.FromSeconds(adstruct.adStopTime);
                        adnum++;
                    }
                    else
                    {
                        AdStruct adstruct = new AdStruct();
                        adstruct = adList[adnum];
                        adwin.Source = new BitmapImage(new Uri(adstruct.url));
                        adtimer.Interval = TimeSpan.FromSeconds(adstruct.adStopTime);
                        adnum++;
                    }
            }
            else
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("code", ConfigurationManager.AppSettings["machineCode"]);
                dic.Add("universityCode", ConfigurationManager.AppSettings["universityCode"]);
                adList = getAdList(ConfigurationManager.AppSettings["ad_list"],dic);
                if (adList == null)
                {
                    adwin.Source = new BitmapImage(new Uri("/Resources/picture/adBg.png", UriKind.Relative));
                }
                adtimer.Interval = TimeSpan.FromSeconds(3);
            }
        }
        #endregion
        #region 主程序通用方法
        public MainWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 退回主程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)//Esc键  
            {
                Start st = new Start();
                st.Show();

                dtimer.Stop();
                adtimer.Stop();
                stadtimer.Stop();
                this.Close();
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
            }
        }
        /// <summary>
        /// 加载附加资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            unName.Text = ConfigurationManager.AppSettings["unName"];
            unEName.Text = ConfigurationManager.AppSettings["unEName"];

            dtimer = new System.Windows.Threading.DispatcherTimer();

            //每100秒刷新一次
            dtimer.Interval = TimeSpan.FromSeconds(100);
            dtimer.Tick += dtimer_Tick;
            dtimer.Start();

            //string str1 = BLL.HttpBLL.GetResponseString(BLL.HttpBLL.CreatePostHttpResponse("", null));
            

            adtimer = new System.Windows.Threading.DispatcherTimer();
            adtimer.Interval = TimeSpan.FromSeconds(1);
            adtimer.Tick += adtimer_Tick;
            adtimer.Start();

            stadtimer = new System.Windows.Threading.DispatcherTimer();
            stadtimer.Interval = TimeSpan.FromSeconds(3);
            stadtimer.Tick += stadtimer_Tick;
            stadtimer.Start();

            this.KeyDown += ModifyPrice_KeyDown;
            fram.Content = null;
            NumKeyBoard nkb = new NumKeyBoard();
            nkb.tb = textBox;
            fram.Content = nkb;
        }
        /// <summary>
        /// 提前检测
        /// </summary>
        /// <returns></returns>
        private bool preExam()
        {
            if (!stat.line)
            {
                MessageBox.Show("网络问题", "对不起网络不可用");
                return false;
            }
            if (stat.stat != "正常")
            {
                MessageBox.Show("机器已被服务器禁止使用", stat.stat);
                return false;
            }

            //稍后再说
            if (printerStaTxt.Text != "None" || ConfigurationManager.AppSettings["printerStatus"]!="正常")
            {
                MessageBox.Show("打印机问题", "对不起打印机不可用");
                return false;
            }

            if (int.Parse(pageRemain.Text) < 30)
            {
                MessageBox.Show("纸张问题", "抱歉剩余纸张过少，请更换机器");
                return false;
            }
            int result = preTextTest(textBox1);
            if (result == 1)
            {
                MessageBox.Show("输入问题", "请正确输入学号和验证码");
                return false;
            }
            return true;
        }
        #endregion
        #region 确定打印按钮的事务处理
        /// <summary>
        /// 确定打印按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rightbtn_Click(object sender, RoutedEventArgs e)
        {
            if (!preExam())
            {
                return;
            }

            PrintBLL printbll = new PrintBLL();
            printbll.ClearJobs();

            dtimer.Stop();
            adtimer.Stop();
            stadtimer.Stop();

            App.user.Name = textBox.Text;
            App.user.PassWord = textBox1.Text;
            App.user.role = 1;

            List<detail_m> details = getPrintJobList();

            if (details != null && details.Count > 0)
            {
                App.psta.Count = 0;
                App.psta.nowCount = 0;
                App.psta.jobstype = "";
                foreach (detail_m detail in details)
                {
                    string a = detail.printTypeName;
                    string url = ConfigurationManager.AppSettings[a];

                    App.psta.Count += detail.printNum;
                    App.psta.jobstype += Enum.GetName(typeof(Jobs), detail.printTypeId)+" ";

                    if (url != null)
                    {
                        JObject jo;

                        jsonbll.jsonToJobject(detail.printContent, out jo);
                        jsonbll.jsonToJobject(jo["data"].ToString(), out jo);
                        int b=getDoc(url, a, jo, Enum.GetName(typeof(Jobs), detail.printTypeId), detail.printNum);
                        if(b==1)
                        {
                            MessageBox.Show("请联系管理员检查配置有无相应模版或其他错误","无法加载模板");
                            dtimer.Start();
                            adtimer.Start();
                            stadtimer.Start();
                            return;
                        }
                    }
                    else
                    {
                        dtimer.Start();
                        adtimer.Start();
                        stadtimer.Start();
                        MessageBox.Show("请联系管理员检查配置有无相应模版或其他错误","无法加载模板");                        
                        return;
                    }
                }
               
                PrintingPage ppg = new PrintingPage();
                ppg.Owner = this;
                ppg.Show();
            }
            else
            {
                MessageBox.Show("您并无打印订单，或学号和验证码有误","订单获取失败");
            }
        }
        /// <summary>
        /// 得到打印文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        private int getDoc(string url, string type, JObject data, string name, int num)
        {
            //PrintDialog pd = new PrintDialog();
            PrintDialog pd = new PrintDialog();
            PrintBLL pl = new PrintBLL();
            pl.SetPrintProperty(pd);

            if (type == "CJD")
            {
                FlowDocument doc = BLL.PrintBLL.loadSorceDocument(url, data);

                if (doc != null)
                {
                    doc.PageHeight = pd.PrintableAreaHeight;
                    doc.PageWidth = pd.PrintableAreaWidth;
                    doc.PagePadding = new Thickness(50);
                    doc.ColumnGap = 0;
                    doc.ColumnWidth = pd.PrintableAreaWidth;

                    Dispatcher.BeginInvoke(new DoPrintMethod(DoPrint), DispatcherPriority.ApplicationIdle, pd, ((IDocumentPaginatorSource)doc).DocumentPaginator, name, num);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else //其他不需要动态生成文档
            {
                FlowDocument doc = BLL.PrintBLL.LoadDocument(url, data);
                if (doc != null)
                {
                    doc.PageHeight = pd.PrintableAreaHeight;
                    doc.PageWidth = pd.PrintableAreaWidth;
                    doc.PagePadding = new Thickness(50);
                    doc.ColumnGap = 0;
                    doc.ColumnWidth = pd.PrintableAreaWidth;
                    Dispatcher.BeginInvoke(new DoPrintMethod(DoPrint), DispatcherPriority.ApplicationIdle, pd, ((IDocumentPaginatorSource)doc).DocumentPaginator, name, num);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="pdlg"></param>
        /// <param name="paginator"></param>
        /// <param name="name"></param>
        /// <param name="num"></param>
        private void DoPrint(PrintDialog pdlg, DocumentPaginator paginator, string name,int num)
        {
            //for(int i=0;i<num;i++)                
                pdlg.PrintDocument(paginator, name);
        }
        /// <summary>
        /// 得到打印信息新
        /// </summary>
        private List<detail_m> getPrintJobList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("code", textBox1.Text);
            dic.Add("universityCode", ConfigurationManager.AppSettings["universityCode"]);
            dic.Add("universityId", textBox.Text);

            HttpBLL httpbll = new HttpBLL();
            JSONBLL jsonbll = new JSONBLL();
            JObject jo;
            //得到Json字符串
            string printstr = httpbll.GetResponseString(httpbll.CreatePostHttpResponse(ConfigurationManager.AppSettings["apply_detail"], dic));

            if (printstr != null)
            {
                jsonbll.jsonToJobject(printstr, out jo);//得到反序列化实例JObject

                if (jo["code"].ToString() == "200")
                {
                    Apply_m apply = new Apply_m();
                    apply = JsonConvert.DeserializeObject<Apply_m>(jo["apply"].ToString());//得到detail信息

                    //App.psta.studentName = apply.userName;

                    List<detail_m> printdetails = new List<detail_m>();

                    List<detail_m> printTypes = JsonConvert.DeserializeObject<List<detail_m>>(jo["detail"].ToString());//得到detail信息
                    return printTypes;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        #endregion
        #region help按钮处理事件组
        /// <summary>
        /// 列出本校的可打印类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private List<print_type_m> getPrintTypeList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("universityCode", ConfigurationManager.AppSettings["universityCode"]);
            dic.Add("universityId", textBox.Text);
            List<print_type_m> printTypes = new List<print_type_m>();

            HttpBLL httpbll = new HttpBLL();
            JSONBLL jsonbll = new JSONBLL();
            JObject jo;

            string printstr = httpbll.GetResponseString(httpbll.CreatePostHttpResponse(ConfigurationManager.AppSettings["type_list"], dic));

            jsonbll.jsonToJobject(printstr, out jo);//得到反序列化实例JObject
            if (jo != null && jo["code"].ToString() == "200")
            {
                printTypes = JsonConvert.DeserializeObject<List<print_type_m>>(jo["types"].ToString());
            }

            return printTypes;
        }
        /// <summary>
        /// help 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<print_type_m> printTypeList = getPrintTypeList();

            if (printTypeList.Count > 0)
            {
                helpPage ppg = new helpPage();
                ppg.DataContext = printTypeList;
                ppg.coutRun.Text = printTypeList.Count.ToString();
                ppg.Owner = this;
                ppg.ShowDialog();
            }
            else
            {
                MessageBox.Show("出错", "没有可打印的文件类型，请检查网络，联系管理员");
            }
        }
        #endregion
        #region 文本框处理
        /// <summary>
        /// text 获取焦点处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.Text = null;
            fram.Content = null;
            NumKeyBoard nkb = new NumKeyBoard();
            nkb.tb = textBox;
            fram.Content = nkb;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox1.Text = null;
            fram.Content = null;
            NumKeyBoard nkb = new NumKeyBoard();
            nkb.tb = textBox1;
            fram.Content = nkb;
        }
        /// <summary>
        /// 文本框内容预检测
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        private int preTextTest(TextBox tb)
        {
            int result;
            bool able;
            able = int.TryParse(tb.Text.ToString(), out result);
            if (!able)
            {
                MessageBox.Show("输入格式错误", "输入格式错误请检查输入为正确数字");
                return 1;
            }
            return 0;
        }
        #endregion
    }
}
