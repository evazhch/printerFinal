using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterThird.Models
{
    public class PrintJobs
    {
        public string name { get; set; }
        public int count { get; set; }
        public int now { get; set; }
        //public List<string> jobsName { get; set; }
        //public List<string> jobsType { get; set; }
        //public List<JObject> jobsdetail { get; set; }
    }
}
