using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.IO;

namespace printerFinal.BLL
{
    public class JSONBLL
    {
        /// <summary>
        /// json转化为Jobject
        /// </summary>
        /// <param name="json"></param>
        /// <param name="jo"></param>
        /// <returns></returns>
        public int jsonToJobject(string json, out JObject jo)
        {
            try
            {
                jo = JsonConvert.DeserializeObject<JObject>(json);
            }
            catch (Exception ex)
            {
                jo = null;
                //MessageBox.Show(ex.Message);
                return ex.HResult;
            }
            return 0;
        }
        /// <summary>
        /// 非数组的情况下将jo拆分
        /// </summary>
        /// <param name="jo"></param>
        /// <param name="inquireStr"></param>
        /// <param name="newJo"></param>
        /// <returns></returns>
        public int jobjectDisassem(JObject jo, string inquireStr, out JObject newJo)
        {
            JObject ja = (JObject)jo;
            try
            {
                newJo = (JObject)ja[inquireStr];
            }
            catch (Exception ex)
            {
                newJo = null;
                //MessageBox.Show(ex.Message);
                return ex.HResult;
            }
            return 0;
        }
        /// <summary>
        /// 数组情况下将JO拆分
        /// </summary>
        /// <param name="jo"></param>
        /// <param name="inquireStr"></param>
        /// <param name="newJo"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public int jobjectDisassem(JObject jo, string inquireStr, out JArray newJo)
        {
            try
            {
                newJo = (JArray)jo[inquireStr];
            }
            catch (Exception ex)
            {
                newJo = null;
                //MessageBox.Show(ex.Message);
                return ex.HResult;
            }
            return 0;
        }
        /// <summary>
        /// 从文件读取Json //用处不大没法处理数组
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jo"></param>
        /// <returns></returns>
        public int textJsontoObject(string url, out JObject jo)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());//指定转化日期的格式
                serializer.NullValueHandling = NullValueHandling.Ignore;//忽略空值

                using (StreamReader sr = new StreamReader(url))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    jo = (JObject)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                jo = null; //有问题资源浪费
                //MessageBox.Show(ex.Message);
                return ex.HResult;
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jo"></param>
        /// <returns></returns>
        public int textJsontoObject(string url, out object jo)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());//指定转化日期的格式
                serializer.NullValueHandling = NullValueHandling.Ignore;//忽略空值

                using (StreamReader sr = new StreamReader(url))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    jo = (object)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                jo = null; //有问题资源浪费
                //MessageBox.Show(ex.Message);
                return ex.HResult;
            }
            return 0;
        }
        /// <summary>
        /// 将类信息转为Json并输出到txt文件
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public int classtoJsontxt(object ob, string url)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());//指定转化日期的格式
                serializer.NullValueHandling = NullValueHandling.Ignore;//忽略空值

                using (StreamWriter sw = new StreamWriter(url))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, ob);
                    // {"ExpiryDate":new Date(1230375600000),"Price":0}
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
                return ex.HResult;
            }
            return 0;
        }
    }
}
