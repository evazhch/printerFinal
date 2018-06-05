using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace printerFinal.BLL
{
    public class HttpBLL
    {
        #region 变量区
        #endregion
        #region 通用http post 方法
        /// <summary>
        /// 发送http post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="parameters">查询参数集合</param>
        /// <returns></returns>
        public HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;//创建请求对象
            request.Method = "POST";//请求方式
            request.ContentType = "application/x-www-form-urlencoded";//链接类型
                                                                      //构造查询字符串
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                bool first = true;
                foreach (string key in parameters.Keys)
                {

                    if (!first)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        first = false;
                    }
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                //写入请求流
                try
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取服务器信息出错", ex.Message);
                    return null;
                }
            }
            try
            {
              return request.GetResponse() as HttpWebResponse;
            }
            catch(Exception ex)
            {
                MessageBox.Show("获取服务器信息出错",ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 从HttpWebResponse对象中提取响应的数据转换为字符串
        /// </summary>
        /// <param name="webresponse"></param>
        /// <returns></returns>
        public string GetResponseString(HttpWebResponse webresponse)
        {
            try
            {
                using (Stream s = webresponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("提取字符串出错",ex.Message);
                return null;
            }
        }
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;
        }
        /// <summary>
        /// 表单项排序并加盐
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string getJsonString(string secret, IDictionary<string, string> parameters)
        {
            Dictionary<string, string> parameters1 = parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            string str = "";

            foreach (var a in parameters1)//拼接出要加密的字符串
            {
                str += a.Key + "=" + a.Value + "&";
            }

            string str1 = str.Substring(0, str.Length - 1);
            str1 += secret;//加盐
            return str1;
        }
        #endregion

        #region 应对需求的方法
        /// <summary>
        /// 向服务器发送打印成功与否的方法
        /// </summary>
        public string updatePrintStatus(string printResualt)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("code", App.user.PassWord);
            dic.Add("universityCode", ConfigurationManager.AppSettings["universityCode"]);
            dic.Add("universityId", App.user.Name);
            dic.Add("machineCode", ConfigurationManager.AppSettings["machineCode"]);
            dic.Add("printResult", printResualt);
            dic.Add("printTime", DateTime.Now.ToString("yyyy/mm/dd hh:mm:ss"));
            dic.Add("printIp", "0");
            dic.Add("printResult1", null);
            dic.Add("printResult2", null);
            dic.Add("printResult3", null);
            dic.Add("printResult4", null);
            string result = "";
            try
            {
                result = GetResponseString(CreatePostHttpResponse(ConfigurationManager.AppSettings["apply_updatePrintStatus"], dic));
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("上传打印结果出错", ex.Message);
                return result;
            }
        }
        /// <summary>
        /// 上传纸张减少数据
        /// </summary>
        /// <param name="printResualt"></param>
        /// <returns></returns>
        public string updatePageNum(int num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("code", ConfigurationManager.AppSettings["machineCode"]);
            dic.Add("universityCode", ConfigurationManager.AppSettings["universityCode"]);
            dic.Add("printedPageNum", num.ToString());
            string result="";
            try
            {
                result = GetResponseString(CreatePostHttpResponse(ConfigurationManager.AppSettings["machine_updatePaperNum"], dic));
                return result;
            }
            catch(Exception ex)
            {
                MessageBox.Show("上传纸张更改出错", ex.Message);
                return result;
            }
        }
        #endregion

    }
}
