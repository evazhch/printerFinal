using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace printerFinal.BLL
{
    public class ConfigBLL
    {
        //第一个参数是xml文件中的add节点的value，第二个参数是add节点的key
        public void SaveConfig(string strKey,string ConnenctionString)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径  
            string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            // string  strFileName= AppDomain.CurrentDomain.BaseDirectory + "\\exe.config";  
            doc.Load(strFileName);
            //找出名称为“add”的所有元素  
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性  
                XmlAttribute att = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素  
                if (att.Value == strKey)
                {
                    //对目标元素中的第二个属性赋值  
                    att = nodes[i].Attributes["value"];
                    att.Value = ConnenctionString;
                    break;
                }
            }
            //保存上面的修改  
            doc.Save(strFileName);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
        //整个App.setting 更改
        public int SaveConfigSuper(List<Models.Config_m> data)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (Models.Config_m m in data)
            {
                //根据Key读取<add>元素的Value
                var name = config.AppSettings.Settings[m.key];
                if (name != null)
                {
                    //写入<add>元素的Value
                    config.AppSettings.Settings[m.key].Value = m.value;
                }
                else
                {
                    //增加<add>元素
                    config.AppSettings.Settings.Add(m.key, m.value);
                }
                //删除<add>元素
                //config.AppSettings.Settings.Remove("name");
                //一定要记得保存，写不带参数的config.Save()也可以
                config.Save(ConfigurationSaveMode.Modified);

            }
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            return 0;
        }
    }
}
