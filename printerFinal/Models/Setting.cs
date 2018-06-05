using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printerFinal.Models
{
    public class SettingModel
    {
        public string Id { get; set; }
        public string universityCode { get; set; } //学校代码
        public string code { get; set; } //机器编号

        public int printePageNum { get; set; } //已打印张数
        public int remainPageNum { get; set; } //剩余张数

        public string Toner_Cartridge { get; set; }
        public string position { get; set; }  //地点

        public string IPposition { get; set; } //Ip地点
        public string TeamviewerId { get; set; }

        public string ContactName { get; set; } //联系人姓名
        public string ContactPhone { get; set; } //联系电话

        public string nowStatus { get; set; }
        public SettingModel()
        {
            Id = ConfigurationManager.AppSettings["id"];
            universityCode = ConfigurationManager.AppSettings["universityCode"];
            code = ConfigurationManager.AppSettings["code"];

            printePageNum = int.Parse(ConfigurationManager.AppSettings["printedPageNum"]);
            remainPageNum = int.Parse(ConfigurationManager.AppSettings["remainPageNum"]);

            nowStatus = ConfigurationManager.AppSettings["nowStatus"];

            Toner_Cartridge = ConfigurationManager.AppSettings["Toner_Cartridge"];
            position = ConfigurationManager.AppSettings["position"];
            IPposition = ConfigurationManager.AppSettings["IPposition"];
            TeamviewerId = ConfigurationManager.AppSettings["TeamviewerId"];
            ContactName = ConfigurationManager.AppSettings["ContactName"];
            ContactPhone = ConfigurationManager.AppSettings["ContactPhoto"];
            nowStatus = ConfigurationManager.AppSettings["nowStatus"];
        }
    }
}
