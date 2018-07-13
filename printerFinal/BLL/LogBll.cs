using printerFinal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace printerFinal.BLL
{
    public class LogBll
    {

        public void CreatLog()
        {

        }

        public void AddLog(Log_m log,string fileName)
        {
            //读取log
            List<Log_m> loglist = loadLog(fileName);
            //添加log
            loglist.Add(log);
            //避免log文件过大，只保存1000条操作
            if(loglist.Count>1000)
            {
                //删除过时的表项
                loglist.RemoveRange(0,loglist.Count-1000);
            }

            XmlSerializer ser = new XmlSerializer(typeof(List<Log_m>));

            //创建或覆盖log文件
            using (FileStream fs = File.Create(fileName))
            {
                //序列化list
                ser.Serialize(fs, loglist);
            }
        }

        public List<Log_m> loadLog(string fileName)
        {
            //XML序列与反序列化类
            XmlSerializer ser = new XmlSerializer(typeof(List<Log_m>));
            //日志列表
            List<Log_m> logList = new List<Log_m>();
            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {
                    //读取log
                    logList = ser.Deserialize(fs) as List<Log_m>;

                    return logList;
                }
            }
            catch
            {
                return logList;
            }
        }
    }
}
