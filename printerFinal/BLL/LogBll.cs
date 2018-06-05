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
            List<Log_m> loglist = loadLog(fileName);
            loglist.Add(log);
            if(loglist.Count>1000)
            {
                loglist.RemoveRange(0,loglist.Count-1000);
            }
            XmlSerializer ser = new XmlSerializer(typeof(List<Log_m>));
            using (FileStream fs = File.Create(fileName))
            {
                ser.Serialize(fs, loglist);
            }
        }

        public List<Log_m> loadLog(string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Log_m>));
            List<Log_m> logList = new List<Log_m>();
            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {

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
