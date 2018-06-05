using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printerFinal.Models
{
    public class print_type_m
    {
        public int sid { get; set; }
        public string typeName { get; set; }
        public decimal unitPrice { get; set; }
        public string printUrl { get; set; }
        public string printDerection { get; set; }
        public int status { get; set; }
        public int isFree { get; set; }
        public int universityCode { get; set; }
        public string printType { get; set; }
    }
}
