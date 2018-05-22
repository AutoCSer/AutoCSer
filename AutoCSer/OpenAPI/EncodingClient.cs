using System;
using System.Collections.Specialized;
using System.Text;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI
{
    /// <summary>
    /// 编码绑定请求
    /// </summary>
    internal class EncodingClient : IDisposable
    {
        /// <summary>
        /// web请求
        /// </summary>
        private readonly WebClient request;
        /// <summary>
        /// 请求编码
        /// </summary>
        private readonly Encoding encoding;
        /// <summary>
        /// 编码绑定请求
        /// </summary>
        /// <param name="request">web请求</param>
        /// <param name="encoding">请求编码</param>
        public EncodingClient(WebClient request, Encoding encoding)
        {
            this.request = request;
            this.encoding = encoding;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            request.Dispose();
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns>数据对象,失败放回null</returns>
        public valueType Request<valueType>(string url)
            where valueType : class, IReturn
        {
            return parseJson<valueType>(RequestForm(url), url);
        }
        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="form">POST表单内容</param>
        /// <returns>返回内容,失败为null</returns>
        public string RequestForm(string url, NameValueCollection form = null)
        {
            return request.Request(url, encoding, form);
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="formValue">POST表单</param>
        /// <returns>数据对象,失败放回null</returns>
        public valueType RequestForm<valueType, formType>(string url, formType formValue)
            where valueType : class, IReturn
        {
            string json;
            NameValueCollection form = AutoCSer.Threading.RingPool<NameValueCollection>.Default.Pop() ?? new NameValueCollection();
            try
            {
                AutoCSer.Net.WebClient.Form.Getter<formType>.Get(formValue, form);
                json = RequestForm(url, form);
            }
            finally
            {
                form.Clear();
                AutoCSer.Threading.RingPool<NameValueCollection>.Default.PushNotNull(form);
            }
            return parseJson<valueType>(json, url);
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="form">POST表单</param>
        /// <returns>数据对象,失败放回null</returns>
        public valueType RequestForm<valueType>(string url, NameValueCollection form)
            where valueType : class, IReturn
        {
            return parseJson<valueType>(RequestForm(url, form), url);
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="formValue">POST表单</param>
        /// <returns>数据对象,失败放回null</returns>
        public valueType RequestJson<valueType, formType>(string url, formType formValue)
            where valueType : class, IReturn
        {
            string json = request.Request(url, encoding, null, Encoding.UTF8.GetBytes(AutoCSer.Json.Serializer.Serialize(formValue)));
            return parseJson<valueType>(json, url);
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">文件数据</param>
        /// <param name="filename">文件名称</param>
        /// <param name="contentType">输出内容类型</param>
        /// <param name="form">表单数据</param>
        /// <returns>数据对象,失败放回null</returns>
        public valueType RequestJson<valueType>(string url, byte[] data, string filename, byte[] contentType, KeyValue<byte[], byte[]>[] form)
            where valueType : class, IReturn
        {
            string json = request.Upload(url, encoding, data, filename ?? "file", contentType, form ?? NullValue<KeyValue<byte[], byte[]>>.Array);
            return parseJson<valueType>(json, url);
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="url"></param>
        /// <returns>数据对象,失败放回null</returns>
        private static valueType parseJson<valueType>(string json, string url)
            where valueType : class, IReturn
        {
            if (json != null)
            {
                valueType value = null;
                bool isError = false, isJson = false;
                try
                {
                    if (AutoCSer.Json.Parser.Parse(json, ref value)) isJson = true;
                }
                catch (Exception error)
                {
                    isError = true;
                    AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error, url + @"
" + json);
                }
                if (isJson && value.IsReturn) return value;
                if (!isError) AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, url + @"
" + value.Message + @"
" + json);
            }
            return default(valueType);
        }
        /// <summary>
        /// API请求XML数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="formValue">POST表单</param>
        /// <param name="config">XML序列化配置</param>
        /// <param name="isValue">是否验证数据</param>
        /// <returns>数据对象,失败放回null</returns>
        public valueType RequestXml<valueType, formType>(string url, formType formValue, AutoCSer.Xml.SerializeConfig config = null, bool isValue = true)
            where valueType : class, IReturn
        {
            string xml;
            return RequestXml<valueType, formType>(url, formValue, out xml, config);
        }
        /// <summary>
        /// API请求XML数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="formValue">POST表单</param>
        /// <param name="xml">输出XML字符串</param>
        /// <param name="config">XML序列化配置</param>
        /// <returns>数据对象,失败放回null</returns>
        public valueType RequestXml<valueType, formType>(string url, formType formValue, out string xml, AutoCSer.Xml.SerializeConfig config = null)
            where valueType : class, IReturn
        {
            return parseXml<valueType>(xml = request.Request(url, encoding, null, Encoding.UTF8.GetBytes(AutoCSer.Xml.Serializer.Serialize(formValue, config))), url, true);
        }
        /// <summary>
        /// API请求XML数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="xml">XML字符串</param>
        /// <param name="url"></param>
        /// <param name="isValue"></param>
        /// <returns>数据对象,失败放回null</returns>
        private static valueType parseXml<valueType>(string xml, string url, bool isValue)
            where valueType : class, IReturn
        {
            if (xml != null)
            {
                valueType value = null;
                bool isError = false, isXml = false;
                try
                {
                    if (AutoCSer.Xml.Parser.Parse(xml, ref value)) isXml = true;
                }
                catch (Exception error)
                {
                    isError = true;
                    AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error, url + @"
" + xml);
                }
                if (isXml && (!isValue || value.IsReturn)) return value;
                if (!isError) AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, url + @"
" + value.Message + @"
" + xml);
            }
            return default(valueType);
        }
        /// <summary>
        /// API请求XML返回文本数据
        /// </summary>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="formValue">POST表单</param>
        /// <param name="config">XML序列化配置</param>
        /// <returns>数据对象,失败放回null</returns>
        public string RequestXml<formType>(string url, formType formValue, AutoCSer.Xml.SerializeConfig config = null)
        {
            return request.Request(url, encoding, null, Encoding.UTF8.GetBytes(AutoCSer.Xml.Serializer.Serialize(formValue, config)));
        }
        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public byte[] Download(string url)
        {
            return request.Download(url, null);
        }
        /// <summary>
        /// API请求
        /// </summary>
        /// <typeparam name="formType"></typeparam>
        /// <param name="url"></param>
        /// <param name="formValue"></param>
        /// <returns></returns>
        public byte[] DownloadJson<formType>(string url, formType formValue)
        {
            return request.Download(url, Encoding.UTF8.GetBytes(AutoCSer.Json.Serializer.Serialize(formValue)));
        }
    }
}
