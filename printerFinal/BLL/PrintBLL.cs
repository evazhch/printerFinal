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
        /// 
        public FlowDocument LoadDocument(string type,string strTmplName, JObject data)
        {
            switch (type)
            {
                case "CJD": return loadSorceDocument(strTmplName, data); //成绩单
                case "XJZM": return loadStutasDocument(strTmplName, data); //学籍证明
                case "SXW":return loadSorceDocument(strTmplName, data); //双学位成绩单
                case "CGCJ": return loadCGSorceDocument(strTmplName, data);  //出国成绩单
                default: return loadSimpleDocument(strTmplName, data); //其他类型
            }
        }
        /// <summary>
        /// 简单流文档模板加载
        /// </summary>
        /// <param name="strTmplName">流文档的路径</param>
        /// <param name="data">流文档要填写的数据</param>
        /// <returns></returns>
        public FlowDocument loadSimpleDocument(string strTmplName, JObject data)
        {
            try
            {
                //读取文档
                FileStream xamlFile = new FileStream(strTmplName, FileMode.Open, FileAccess.Read);
                FlowDocument doc = XamlReader.Load(xamlFile) as FlowDocument;
                xamlFile.Dispose();

                //生成打印时间
                DateTime now = DateTime.Now;
                data.Add("printDate", now.ToString("yyyy/MM/dd"));
                //性别用语
                if (data["sex"] != null)
                {
                    if (data["sex"].ToString() == "男")
                    {
                        data.Add("heorshe", "he");
                        data.Add("hisorher", "his");
                    }
                    else
                    {
                        data.Add("heorshe", "she");
                        data.Add("hisorher", "her");
                    }
                }
                //绑定数据
                doc.DataContext = data;
                return doc;
            }
            catch (Exception ex)
            {
                messgeBoxBll.Show("模板加载出错", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 学籍加载
        /// </summary>
        /// <param name="strTmplName">文档路径</param>
        /// <param name="data">要填写的数据（姓名，学号。。。。）</param>
        /// <returns></returns>
        public FlowDocument loadStutasDocument(string strTmplName, JObject data)
        {
            try
            {
                FileStream xamlFile = new FileStream(strTmplName, FileMode.Open, FileAccess.Read);
                FlowDocument doc = XamlReader.Load(xamlFile) as FlowDocument;
                xamlFile.Dispose();

                //区分本科，专科，毕业
                if (data["educate_gradation"].ToString()=="本科")
                {
                    doc.Blocks.Remove(doc.FindName("biye") as Section);
                    doc.Blocks.Remove(doc.FindName("zhuanke") as Section);
                }
                else if(data["educate_gradation"].ToString() == "专科")
                {
                    doc.Blocks.Remove(doc.FindName("benke") as Section);
                    doc.Blocks.Remove(doc.FindName("biye") as Section);
                }
                else if (data["educate_gradation"].ToString() == "")
                {
                    doc.Blocks.Remove(doc.FindName("benke") as Section);
                    doc.Blocks.Remove(doc.FindName("zhuanke") as Section);
                }


                DateTime now = DateTime.Now;
                data.Add("printDate", now.ToString("yyyy/MM/dd"));

                if (data["sex"].ToString() == "男")
                {
                    data.Add("heorshe", "he");
                    data.Add("hisorher", "his");
                }
                else
                {
                    data.Add("heorshe", "she");
                    data.Add("hisorher", "her");
                }

                doc.DataContext = data;
                return doc;
            }
            catch (Exception ex)
            {
                messgeBoxBll.Show("模板加载出错", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 成绩单加载
        /// </summary>
        /// <param name="strTmplName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public FlowDocument loadSorceDocument(string strTmplName, JObject data)
        {
            int num = 0;
            JArray ja;
            try
            {
                FileStream xamlFile = new FileStream(strTmplName, FileMode.Open, FileAccess.Read);

                FlowDocument doc = XamlReader.Load(xamlFile) as FlowDocument;

                xamlFile.Dispose();

                JSONBLL jsonbll = new JSONBLL();

                string birth = data["birthday"].ToString();
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                DateTime dt = startTime.AddMilliseconds(long.Parse(birth));
                data["birthday"] = dt.ToString("yyyy/MM/dd");
                jsonbll.jobjectDisassem(data, "scores", out ja);//去一下Json层


                DateTime now = DateTime.Now;

                data.Add("printDate", now.ToString("yyyy/MM/dd"));

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
                    //cell.Style = styleCell1;
                    //row.Cells.Add(cell);
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

                        cell = new TableCell(new Paragraph(new Run(jo1["course_name"].ToString())));
                        cell.Style = styleCell1;
                        row1.Cells.Add(cell);

                        cell = new TableCell(new Paragraph(new Run(jo1["credit_hour"].ToString())));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);

                        cell = new TableCell(new Paragraph(new Run(jo1["course_type"].ToString())));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);

                        cell = new TableCell(new Paragraph(new Run(jo1["mark"].ToString())));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);

                        cell = new TableCell(new Paragraph(new Run("")));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);

                        if (i + 1 < ja1.Count)
                        {
                            string str2 = ja1[i + 1].ToString();
                            jsonbll.jsonToJobject(str2, out jo2);

                            cell = new TableCell(new Paragraph(new Run(jo2["course_name"].ToString())));
                            cell.Style = styleCell1;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run(jo2["credit_hour"].ToString())));
                            cell.Style = styleCell;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run(jo2["course_type"].ToString())));
                            cell.Style = styleCell;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run(jo2["mark"].ToString())));
                            cell.Style = styleCell;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run("")));
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
                        else if (num < 45)
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
            catch (Exception ex)
            {
                messgeBoxBll.Show("模板加载出错", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 出国成绩单加载
        /// </summary>
        /// <param name="strTmplName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public FlowDocument loadCGSorceDocument(string strTmplName, JObject data)
        {
            float num = 0;
            JArray ja;
            JArray ja1;
            JObject ja2;
            try
            {
                FileStream xamlFile = new FileStream(strTmplName, FileMode.Open, FileAccess.Read);

                FlowDocument doc = XamlReader.Load(xamlFile) as FlowDocument;

                xamlFile.Dispose();

                JSONBLL jsonbll = new JSONBLL();

                jsonbll.jobjectDisassem(data, "array", out ja);//去一下Json层
                jsonbll.jobjectDisassem(data, "array_transcript", out ja1);
                //string strr = ja1[0].ToString();
                jsonbll.jsonToJobject(ja1[0].ToString(),out ja2);

                ja2.Add("length_of_schooling", data["length_of_schooling"].ToString());

                DateTime dt= DateTime.Parse(ja2["birthday"].ToString());
                ja2["birthday"] = dt.ToString("yyyy-MM-dd");
                dt = DateTime.Parse(ja2["startCollegeTime"].ToString());
                ja2["startCollegeTime"] = dt.ToString("yyyy-MM-dd");
                ja2.Add("UniversityEnglishName", data["UniversityEnglishName"].ToString());

                //jsonbll.jobjectDisassem(data, "array_transcript", out ja2);//去一下Json层

                if (ja2["sex"].ToString() == "男")
                {
                    ja2.Add("sexE", "Male");
                }
                else
                {
                    ja2.Add("sexE", "Female");
                }

                DateTime now = DateTime.Now;

                ja2.Add("printDate", now.ToString("yyyy/MM/dd"));

                TableRowGroup group = doc.FindName("rowsDetails") as TableRowGroup;
                Style styleCell = doc.Resources["BorderedCell"] as Style;
                Style styleCell1 = doc.Resources["BorderedCell1"] as Style;
                string preyear = "";

                //var nimabi = doc.FindName("section") as Section;

                int rowsnum = 0;

                for(int i=0;i<ja.Count;)
                {
                    //var table= doc.FindName("table") as Table;
                    //table.DataContext = ja;
                    //选择行族
                    if (rowsnum >= 25 && rowsnum < 50)
                    {
                        group = doc.FindName("rowsDetails1") as TableRowGroup;
                    }
                    else if (rowsnum >= 50 && i < 75)
                    {
                        group = doc.FindName("rowsDetails2") as TableRowGroup;
                    }
                    else if (rowsnum >= 100)
                    {
                        group = doc.FindName("rowsDetails3") as TableRowGroup;
                    }
                    else if (i < 25)
                    {
                        group = doc.FindName("rowsDetails") as TableRowGroup;
                    }

                    string a = ja[i].ToString();
                    JObject aa;
                    jsonbll.jsonToJobject(a, out aa);
                    
                    //判读是否加学年
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell(new Paragraph(new Run(" ")));

                    if (preyear == "" || preyear != aa["termYear"].ToString())
                    {
                        preyear = aa["termYear"].ToString();
                        cell = new TableCell(new Paragraph(new Run("Academic Year " + aa["termYear"].ToString() + "-" + (int.Parse(aa["termYear"].ToString()) + 1).ToString())));
                        cell.Style = styleCell1;
                        row.Cells.Add(cell);
                        group.Rows.Add(row);
                        rowsnum++;
                    }
                    //添加成绩
                    TableRow row1 = new TableRow();

                    cell = new TableCell(new Paragraph(new Run(ja[i]["courseName"].ToString() +"\n"+ ja[i]["englishName"].ToString())));
                    cell.Style = styleCell1;
                    row1.Cells.Add(cell);

                    cell = new TableCell(new Paragraph(new Run((ja[i]["creditHour"].ToString()))));
                    num += float.Parse((ja[i]["creditHourPoint"].ToString()));
                    cell.Style = styleCell;
                    row1.Cells.Add(cell);

                    cell = new TableCell(new Paragraph(new Run(ja[i]["mark"].ToString())));
                    cell.Style = styleCell;
                    row1.Cells.Add(cell);

                    if ((ja[i]["termName"].ToString()).Length > 13)
                    {
                        cell = new TableCell(new Paragraph(new Run("Resit")));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);
                    }
                    else
                    {
                        cell = new TableCell(new Paragraph(new Run(" ")));
                        cell.Style = styleCell;
                        row1.Cells.Add(cell);
                    }
                    if (i + 1 < ja.Count)
                    {
                        string a1 = ja[i+1].ToString();
                        JObject aa1;
                        jsonbll.jsonToJobject(a1, out aa1);

                        if (preyear == "" || preyear != aa1["termYear"].ToString())
                        {
                            group.Rows.Add(row1);
                            rowsnum++;
                            i = i + 1;
                        }
                        else
                        {
                            cell = new TableCell(new Paragraph(new Run(ja[i+1]["courseName"].ToString() + "\n" + ja[i+1]["englishName"].ToString())));
                            cell.Style = styleCell1;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run((ja[i+1]["creditHour"].ToString()))));
                            num += float.Parse((ja[i]["creditHourPoint"].ToString()));
                            cell.Style = styleCell;
                            row1.Cells.Add(cell);

                            cell = new TableCell(new Paragraph(new Run(ja[i+1]["mark"].ToString())));
                            cell.Style = styleCell;
                            row1.Cells.Add(cell);

                            if ((ja[i+1]["termName"].ToString()).Length > 13)
                            {
                                cell = new TableCell(new Paragraph(new Run("Resit")));
                                cell.Style = styleCell;
                                row1.Cells.Add(cell);
                            }
                            else
                            {
                                cell = new TableCell(new Paragraph(new Run(" ")));
                                cell.Style = styleCell;
                                row1.Cells.Add(cell);
                            }

                            group.Rows.Add(row1);
                            rowsnum++;
                            i = i + 2;
                        }
                    }                  
                }

                //分页管理

                if (rowsnum > 25)
                {
                    ja2.Add("pagenum", ja.Count /50 + 1);
                }
                else
                {
                    ja2.Add("pagenum", 1);
                }
                if (rowsnum / 25 == 2 && rowsnum > 50)
                {
                    doc.Blocks.Remove(doc.FindName("section3") as Section);
                }
                else if (rowsnum / 25 == 1 && rowsnum > 25)
                {
                    doc.Blocks.Remove(doc.FindName("section2") as Section);
                    doc.Blocks.Remove(doc.FindName("section3") as Section);
                }
                else if (rowsnum / 25 == 0 || rowsnum == 25)
                {
                    doc.Blocks.Remove(doc.FindName("section1") as Section);
                    doc.Blocks.Remove(doc.FindName("section2") as Section);
                    doc.Blocks.Remove(doc.FindName("section3") as Section);
                }
                ja2.Add("passedCredits", num);

                doc.DataContext = ja2;

                return doc;
            }
            catch (Exception ex)
            {
                messgeBoxBll.Show("模板加载出错", ex.Message);
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
