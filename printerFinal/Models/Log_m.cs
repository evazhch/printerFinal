using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printerFinal.Models
{
    public class Log_m
    {

        public DateTime datetime { get; set; }//时间

        public string code { get; set; } //机器编号

        public int role { get; set; } //操作角色

        public string text { get; set; } //操作描述

        public string result { get; set; }//操作结果

        public string node { get; set; }//备注

        public Log_m()
        {
            datetime = DateTime.Now;
            code = App.set.code;
            role = App.user.role;
            text = "";
            result = "";
            node = "";
        }
        public Log_m(string text,string result,string node)
        {
            datetime = DateTime.Now;
            code = App.set.code;
            role = App.user.role;
            this.text =text;
            this.result = result;
            this.node = node;
        }
    }
}
