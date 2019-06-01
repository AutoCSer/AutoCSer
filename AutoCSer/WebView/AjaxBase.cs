using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// AJAX 调用
    /// </summary>
    public abstract unsafe partial class AjaxBase : Page
    {
        /// <summary>
        /// 视图类型名称
        /// </summary>
        internal const char ViewClientType = '@';
        /// <summary>
        /// 视图类型成员标识
        /// </summary>
        internal const char ViewClientMember = '.';
        /// <summary>
        /// 客户端格式化视图数组函数
        /// </summary>
        public const string FormatView = ".FormatView()";
        /// <summary>
        /// AJAX响应输出参数
        /// </summary>
        internal static readonly AutoCSer.Json.SerializeConfig JsonSerializeConfig = new AutoCSer.Json.SerializeConfig { IsViewClientType = true, GetLoopObject = "AutoCSer.Pub.GetJsonLoop", SetLoopObject = "AutoCSer.Pub.SetJsonLoop", NullChar = ' ', IsMaxNumberToString = true, CheckLoopDepth = AutoCSer.Json.SerializeConfig.DefaultCheckLoopDepth, IsDateTimeToString = false, IsNumberToHex = true };
        /// <summary>
        /// 公用错误处理函数名称
        /// </summary>
        public const string PubErrorCallName = "Pub.Error";
        /// <summary>
        /// AJAX 输出数据流
        /// </summary>
        internal readonly CharStream AjaxStream = new CharStream(null, 0);
        /// <summary>
        /// AJAX 调用信息
        /// </summary>
        internal AjaxMethodInfo MethodInfo;
        /// <summary>
        /// 内存流最大字节数
        /// </summary>
        internal override SubBuffer.Size MaxMemoryStreamSize { get { return MethodInfo.MaxMemoryStreamSize; } }
        /// <summary>
        /// 是否异步
        /// </summary>
        internal bool IsAsynchronous;
        /// <summary>
        /// AJAX 调用加载
        /// </summary>
        private CallBase loader;
        /// <summary>
        /// 设置 AJAX 调用加载
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="methodInfo"></param>
        internal void SetLoader(CallBase loader, AjaxMethodInfo methodInfo)
        {
            this.loader = loader;
            MethodInfo = methodInfo;
        }
        /// <summary>
        /// AJAX 调用
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CallAjax()
        {
            return loader.CallAjax(this);
        }
        /// <summary>
        /// AJAX 响应输出
        /// </summary>
        /// <typeparam name="valueType">输出数据类型</typeparam>
        /// <param name="value">输出数据</param>
        /// <param name="buffer">输出缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Response<valueType>(ref valueType value, byte* buffer) where valueType : struct
        {
            AjaxStream.Reset(buffer, AutoCSer.UnmanagedPool.DefaultSize);
            using (AjaxStream)
            {
                SetJsContentType();
                bool isCallBack = ResponseAjaxCallBack(AjaxStream);
                AutoCSer.Json.Serializer.Serialize(ref value, AjaxStream, JsonSerializeConfig);
                if (isCallBack) AjaxStream.Write(')');
                PageResponse.SetBody(AjaxStream, ref DomainServer.ResponseEncoding);
            }
        }
        /// <summary>
        /// AJAX 响应输出
        /// </summary>
        /// <param name="buffer">输出缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Response(byte* buffer)
        {
            AjaxStream.Reset(buffer, AutoCSer.UnmanagedPool.TinySize);
            using (AjaxStream)
            {
                SetJsContentType();
                if (ResponseAjaxCallBack(AjaxStream)) AjaxStream.Write(')');
                PageResponse.SetBody(AjaxStream, ref DomainServer.ResponseEncoding);
            }
        }
    }
}
