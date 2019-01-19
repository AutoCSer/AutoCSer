using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 请求
    /// </summary>
    [ClearMember(IsIgnoreCurrent = true)]
    public abstract class Request
    {
        /// <summary>
        /// HTTP套接字接口设置
        /// </summary>
        internal AutoCSer.Net.Http.SocketBase Socket;
        /// <summary>
        /// 域名服务
        /// </summary>
        internal AutoCSer.Net.HttpDomainServer.ViewServer DomainServer;
        /// <summary>
        /// 会话标识
        /// </summary>
        public AutoCSer.Net.HttpDomainServer.ISession Session
        {
            get { return DomainServer.Session; }
        }
        /// <summary>
        /// 输出编码
        /// </summary>
        public Encoding ResponseEncoding
        {
            get { return DomainServer.ResponseEncoding.Encoding; }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string WorkPath
        {
            get { return DomainServer.WorkPath; }
        }
        /// <summary>
        /// 套接字请求编号
        /// </summary>
        public long SocketIdentity { get; internal set; }
        /// <summary>
        /// HTTP 头部标志位
        /// </summary>
        public AutoCSer.Net.Http.HeaderFlag HeaderFlag
        {
            get { return Socket.HttpHeader.Flag; }
        }
        /// <summary>
         /// 提交表单信息
         /// </summary>
        public IEnumerable<AutoCSer.Net.Http.FormValue> Form
        {
            get
            {
                foreach (AutoCSer.Net.Http.FormValue formValue in Socket.Form.Files)
                {
                    yield return formValue;
                }
            }
        }
        /// <summary>
        /// 获取上传文件信息
        /// </summary>
        public IEnumerable<AutoCSer.Net.Http.FormValue> Files
        {
            get
            {
                foreach (AutoCSer.Net.Http.FormValue formValue in Socket.Form.Files)
                {
                    yield return formValue;
                }
            }
        }
        ///// <summary>
        ///// 获取 Cookie
        ///// </summary>
        ///// <param name="name">名称</param>
        ///// <returns>值</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public string GetCookie(string name)
        //{
        //    return Socket.HttpHeader.GetCookie(name);
        //}
        ///// <summary>
        ///// 获取 Cookie
        ///// </summary>
        ///// <param name="name">名称</param>
        ///// <returns>值</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public string GetCookie(byte[] name)
        //{
        //    return Socket.HttpHeader.GetCookieString(name);
        //}
        /// <summary>
        /// 判断是否存在Cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在Cookie值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool IsCookie(byte[] name)
        {
            return Socket.HttpHeader.IsCookie(name);
        }
        /// <summary>
        /// 是否使用对象池
        /// </summary>
        internal bool IsPool;
        /// <summary>
        /// JSON解析配置参数
        /// </summary>
        protected virtual AutoCSer.Json.ParseConfig jsonParserConfig { get { return null; } }
        /// <summary>
        /// XML解析配置参数
        /// </summary>
        protected virtual AutoCSer.Xml.ParseConfig xmlParserConfig { get { return null; } }
        /// <summary>
        /// JSON 解析
        /// </summary>
        internal static Json.Parser JsonParser;
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected bool parseJson<parameterType>(ref parameterType parameter, string json) where parameterType : struct
        {
            Json.Parser parser = Interlocked.Exchange(ref JsonParser, null) ?? Json.Parser.YieldPool.Default.Pop() ?? new Json.Parser();
            bool isParse = parser.ParseWebViewNotEmpty(ref parameter, json, jsonParserConfig);
            if ((parser = Interlocked.Exchange(ref JsonParser, parser)) != null) parser.Free();
            return isParse;
        }
        /// <summary>
        /// 解析web调用参数
        /// </summary>
        /// <typeparam name="parameterType">web调用参数类型</typeparam>
        /// <param name="parameter">web调用参数</param>
        /// <returns>是否成功</returns>
        protected bool parseParameterQuery<parameterType>(ref parameterType parameter) where parameterType : struct
        {
            AutoCSer.Net.Http.Header header = Socket.HttpHeader;
            if (header.ParseQuery(ref parameter))
            {
                string queryJson = header.QueryJson;
                if (queryJson != null) return parseJson(ref parameter, queryJson);
                string queryXml = header.QueryXml;
                if (queryXml != null) return AutoCSer.Xml.Parser.Parse(queryXml, ref parameter, xmlParserConfig);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 解析web调用参数
        /// </summary>
        /// <typeparam name="parameterType">web调用参数类型</typeparam>
        /// <param name="parameter">web调用参数</param>
        /// <returns>是否成功</returns>
        protected bool parseParameterQueryAny<parameterType>(ref parameterType parameter)
        {
            AutoCSer.Net.Http.Header header = Socket.HttpHeader;
            if (header.ParseQuery(ref parameter))
            {
                string queryJson = header.QueryJson;
                if (queryJson != null) return AutoCSer.Json.Parser.ParseNotEmpty(queryJson, ref parameter, jsonParserConfig);
                string queryXml = header.QueryXml;
                if (queryXml != null) return AutoCSer.Xml.Parser.Parse(queryXml, ref parameter, xmlParserConfig);
                return true;
            }
            return false;
        }
        /// <summary>
        /// JSON 序列化预编译
        /// </summary>
        /// <param name="deSerializeTypes"></param>
        /// <param name="serializeTypes"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal protected static void CompileJsonSerialize(Type[] deSerializeTypes, Type[] serializeTypes)
        {
            if (deSerializeTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(deSerializeTypes, AutoCSer.Threading.Thread.CallType.CompileJsonDeSerialize);
            if (serializeTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(serializeTypes, AutoCSer.Threading.Thread.CallType.CompileJsonSerialize);
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            if (count == 0) JsonParser = null;
        }
        static Request()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
