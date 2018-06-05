using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace printerFinal.BLL
{
    class PrintBLL
    {
        JSONBLL jsonbll = new JSONBLL();

        
        #region 加载流文档方法族
        /// <summary>
        /// 加载流文档
        /// </summary>
        /// <param name="strTmplName">流文档的路径</param>
        /// <param name="data">流文档的数据</param>
        /// <returns></returns>
        public static FlowDocument LoadDocument(string strTmplName, JObject data)
        {
            try
            {
                FileStream xamlFile = new FileStream(strTmplName, FileMode.Open, FileAccess.Read);
                FlowDocument doc = XamlReader.Load(xamlFile) as FlowDocument;
                xamlFile.Dispose();

                doc.PagePadding = new Thickness(50);
                //data.Add("autograph", App.set.code);

                doc.DataContext = data;
                
                return doc;
            }
            catch(Exception ex)
            {
                MessageBox.Show("模板加载出错", ex.Message);
                return null;
            }
            
        }
        /// <summary>
        /// 成绩单加载
        /// </summary>
        /// <param name="strTmplName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static FlowDocument loadSorceDocument(string strTmplName, JObject data)
        {
            int num=0;
            JArray ja;
            try
            {
                FileStream xamlFile = new FileStream(strTmplName, FileMode.Open, FileAccess.Read);

                FlowDocument doc = XamlReader.Load(xamlFile) as FlowDocument;

                xamlFile.Dispose();

                //doc.PageWidth = 816;
                //doc.PageHeight = 1150;
                //doc.PagePadding = new Thickness(50);
                JSONBLL jsonbll = new JSONBLL();

                string birth = data["birthday"].ToString();
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                DateTime dt = startTime.AddMilliseconds(long.Parse(birth));
                data["birthday"] = dt.ToString("yyyy/MM/dd");
                jsonbll.jobjectDisassem(data, "scores", out ja);//去一下Json层


                DateTime now = DateTime.Now;

                data.Add("now", now.ToString("yyyy/MM/dd HH:mm:ss"));
               
                TableRowGroup group = doc.FindName("rowsDetails") as TableRowGroup;
                Style styleCell = doc.Resources["BorderedCell"] as Style;
                Style styleCell1 = doc.Resources["BorderedCell1"] as Style;

                foreach (var item in ja)
                {
                    if (num >= 45 && num < 90)
                    {
                        group = doc.FindName("rowsDetails1") as TableRowGroup;
                    }
                    else if (num >= 90 && num < 135)
                    {
                        group = doc.FindName("rowsDetails2") as TableRowGroup;
                    }
                    else if (num < 45)
                    {
                        group = doc.FindName("rowsDetails") as TableRowGroup;
                    }

                    string a = item.ToString();
                    JObject aa;
                    jsonbll.jsonToJobject(a, out aa);

                    TableRow row = new TableRow();

                    TableCell cell = new TableCell(new Paragraph(new Run(" ")));
                    cell.Style = styleCell1;
                    row.Cells.Add(cell);
                    cell = new TableCell(new Paragraph(new Run(aa["year_name"].ToString())));
                    cell.Style = styleCell1;
                    row.Cells.Add(cell);

                    group.Rows.Add(row);

                    num++;

                    JArray ja1;
                    jsonbll.jobjectDisassem(aa, "year_score", out ja1);//去一下Json层
                    for (int i = 0; i < ja1.Count;)
                    {
                        JObject jo1, jo2;
                        string str1 = ja1[i].ToString();
                        jsonbll.jsonToJobject(str1, out jo1);

                        TableRow row1 = new TableRow();

                        cell = new TableCell(new Paragraph(new Run("  ")));
                        cell.Style = styleCell1;
                        row1.Cells.Add(cell);
                        cell = new TableCell(new Paragraph(new Run(jo1["course_name"].ToString())));
                        cell.Style = styleCell1;
                        row1.Cells.Add(cell);

                        cell = new TableCell(new Paragraph(new Run(jo1["mark"].ToString())));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);

                        cell = new TableCell(new Paragraph(new Run(jo1["credit_hour"].ToString())));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);

                        cell = new TableCell(new Paragraph(new Run(jo1["course_type"].ToString())));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);

                        if (i + 1 < ja1.Count)
                        {
                            string str2 = ja1[i + 1].ToString();
                            jsonbll.jsonToJobject(str2, out jo2);

                            cell = new TableCell(new Paragraph(new Run("  ")));
                            cell.Style = styleCell1;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run(jo2["course_name"].ToString())));
                            cell.Style = styleCell1;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run(jo2["mark"].ToString())));
                            cell.Style = styleCell;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run(jo2["credit_hour"].ToString())));
                            cell.Style = styleCell;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run(jo2["course_type"].ToString())));
                            cell.Style = styleCell;
                            row1.Cells.Add(cell);
                        }
                        group.Rows.Add(row1);
                        num++;

                        if (num > 45 && num < 90)
                        {
                            group = doc.FindName("rowsDetails1") as TableRowGroup;
                        }
                        else if (num >= 90 && num < 135)
                        {
                            group = doc.FindName("rowsDetails2") as TableRowGroup;
                        }
                        else if (num <= 45)
                        {
                            group = doc.FindName("rowsDetails") as TableRowGroup;
                        }

                        i = i + 2;
                    }
                }
                //分页管理

                if (num > 45)
                {
                    data.Add("pagenum", num / 45 + 1);
                }
                else
                {
                    data.Add("pagenum", 1);
                }

                if (num / 45 == 1 && num > 45)
                {
                    doc.Blocks.Remove(doc.FindName("table2") as Section);
                }
                else if (num / 45 == 0 || num == 45)
                {
                    doc.Blocks.Remove(doc.FindName("table1") as Section);
                    doc.Blocks.Remove(doc.FindName("table2") as Section);
                }

                doc.DataContext = data;

                return doc;
            }
            catch(Exception ex)
            {
                MessageBox.Show("模板加载出错", ex.Message);
                return null;
            }
        }
        #endregion

        #region 打印方法
        /// <summary>
        /// 打印方法直接打印
        /// </summary>
        /// <param name="pdlg"></param>
        /// <param name="paginator"></param>
        /// <param name="describe"></param>
        public void DoPrint(PrintDialog pdlg, DocumentPaginator paginator, string describe)
        {
            pdlg.PrintDocument(paginator, describe);
        }
        #endregion

        #region 检测Job相关
        /// <summary>
        /// 得到打印机里的打印job
        /// </summary>
        /// <param name="printerName"></param>
        public void GetJobs(string printerName)
        {
            //if (App.psta == null)
            //{
            //    App.psta = new Models.PageSta();
            //}

            PrintServer ps = new PrintServer();
            PrintQueue queue = ps.GetPrintQueue(printerName);
            App.psta.nowCount = queue.NumberOfJobs;
            ps.Dispose();      
            queue.Dispose();
        }
        /// <summary>
        /// 清理打印机队列中的JOb
        /// </summary>
        public void ClearJobs()
        {
            //清空打印机里残留的队列
            PrintServer ps = new PrintServer();
            PrintQueue queue = ps.GetPrintQueue(ConfigurationManager.AppSettings["printer"]);
            queue.Refresh();
            PrintJobInfoCollection allPrintJobs = queue.GetPrintJobInfoCollection();
            foreach (PrintSystemJobInfo printJob in allPrintJobs)
            {
                printJob.Cancel();
            }
            //释放资源
            ps.Dispose();
            queue.Dispose();
            allPrintJobs.Dispose();
        }
        /// <summary>
        /// 检错
        /// </summary>
        /// <param name="statusReport"></param>
        /// <param name="pq"></param>
        public static void SpotTroubleUsingQueueAttributes(ref String statusReport, PrintQueue pq)
        {
            if ((pq.QueueStatus & PrintQueueStatus.PaperProblem) == PrintQueueStatus.PaperProblem)
            {
                statusReport = statusReport + "Has a paper problem. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.NoToner) == PrintQueueStatus.NoToner)
            {
                statusReport = statusReport + "Is out of toner. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.DoorOpen) == PrintQueueStatus.DoorOpen)
            {
                statusReport = statusReport + "Has an open door. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.Error) == PrintQueueStatus.Error)
            {
                statusReport = statusReport + "Is in an error state. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.NotAvailable) == PrintQueueStatus.NotAvailable)
            {
                statusReport = statusReport + "Is not available. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.Offline) == PrintQueueStatus.Offline)
            {
                statusReport = statusReport + "Is off line. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.OutOfMemory) == PrintQueueStatus.OutOfMemory)
            {
                statusReport = statusReport + "Is out of memory. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.PaperOut) == PrintQueueStatus.PaperOut)
            {
                statusReport = statusReport + "Is out of paper. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.OutputBinFull) == PrintQueueStatus.OutputBinFull)
            {
                statusReport = statusReport + "Has a full output bin. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.PaperJam) == PrintQueueStatus.PaperJam)
            {
                statusReport = statusReport + "Has a paper jam. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.Paused) == PrintQueueStatus.Paused)
            {
                statusReport = statusReport + "Is paused. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.TonerLow) == PrintQueueStatus.TonerLow)
            {
                statusReport = statusReport + "Is low on toner. ";
            }
            if ((pq.QueueStatus & PrintQueueStatus.UserIntervention) == PrintQueueStatus.UserIntervention)
            {
                statusReport = statusReport + "Needs user intervention. ";
            }

            // Check if queue is even available at this time of day
            // The method below is defined in the complete example.
            //ReportAvailabilityAtThisTime(ref statusReport, pq);
        }
        #endregion


        /// <summary>
        /// 设置打印格式
        /// </summary>
        /// <param name="printDialog">打印文档</param>
        /// <param name="pageSize">打印纸张大小 a4</param>
        /// <param name="pageOrientation">打印方向 竖向</param>
        public void SetPrintProperty(PrintDialog printDialog, PageMediaSizeName pageSize = PageMediaSizeName.ISOA4, PageOrientation pageOrientation = PageOrientation.Portrait)
        {
            var printTicket = printDialog.PrintTicket;
            printTicket.PageMediaSize = new PageMediaSize(pageSize);//A4纸
            printTicket.PageOrientation = pageOrientation;//默认竖向打印
        }
    }
}
