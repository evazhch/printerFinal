using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterThird.Models
{
    public class machine_m
    {
        public int sid { get; set; }
        public int universityCode { get; set; }
        public string code { get; set; }
        public int printedPageNum { get; set; }
        public int remainPageNum { get; set; }
        public string nowStatus { get; set; }
    }
}
