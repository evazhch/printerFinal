using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printerFinal.Models
{
    public class detail_m
    {
      public int sid { get; set; }
      public int applyId { get; set; }
      public int printTypeId { get; set; }
      public string printTypeName { get; set; }
      public string printContent { get; set; }
      public int printNum { get; set; }
    }
}
