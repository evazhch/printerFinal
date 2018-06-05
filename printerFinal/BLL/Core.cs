using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterThird.BLL
{
    public class Core
    {
        //重启方法
        public void restart()
        {
            System.Windows.Application.Current.Shutdown();
            System.Reflection.Assembly.GetEntryAssembly();
            string startpath = System.IO.Directory.GetCurrentDirectory();
            System.Diagnostics.Process.Start(startpath + "/PrinterThird.exe");  //xxxx.exe为要启动的程序 
        }
    }
}
