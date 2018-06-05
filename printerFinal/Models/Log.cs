using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterThird.Models
{
    public class Log
    {
        public string time;//日志记录时间
        public string role;//角色 学生 管理员
        public string name;//学生学号 管理员名称
        public string operation;//操作 打印 换纸 硒鼓 修理
        public string describe;//操作描述
        public string success;//成功与否
    }
}
