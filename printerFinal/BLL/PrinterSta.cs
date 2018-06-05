using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace PrinterThird.BLL
{
    public class PrinterSta
    {
        public enum enum_printerSys_status
        {
            /// <summary>  
            /// 其他状态  
            /// </summary>  
            其他 = 1,
            /// <summary>  
            /// 未知  
            /// </summary>  
            未知,
            /// <summary>  
            /// 空闲  
            /// </summary>  
            空闲,
            /// <summary>  
            /// 正在打印  
            /// </summary>  
            正在打印,
            /// <summary>  
            /// 预热  
            /// </summary>  
            预热,
            /// <summary>  
            /// 停止打印  
            /// </summary>  
            停止打印,
            /// <summary>  
            /// 打印中  
            /// </summary>  
            打印中,
            /// <summary>  
            /// 离线  
            /// </summary>  
            离线,
        }
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="name">打印机名称</param>  
        public PrinterSta(string name)
        {
            this.printer_name = name;
        }

        // 设备名：EPSON R330 Series  
        private string _printer_name;
        /// <summary>  
        /// 打印机名称  
        /// </summary>  
        public string printer_name
        {
            get
            {
                return _printer_name;
            }
            set
            {
                _printer_name = value;
            }
        }

        /// <summary>  
        /// 获取打印机状态  
        /// </summary>  
        /// <returns></returns>  
        public string getStatus()
        {
            string path = @"win32_printer.DeviceId='" + this.printer_name + "'";
            try
            {
                ManagementObject printer = new ManagementObject(path);
                printer.Get();
                var aa = printer.Properties;
                var b = printer.Properties["WorkOffline"].Value;
                //var b1 = printer.Properties[""].Value;
                enum_printerSys_status a =(enum_printerSys_status)(Convert.ToInt32(printer.Properties["PrinterStatus"].Value));

                //printer.Dispose();
                return a.ToString();
            }
            catch(Exception ex)
            {
                return ("出错"+ex.Message);
            }
        }
    }
}
