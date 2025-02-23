﻿using System;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

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
        internal static readonly AutoCSer.Json.SerializeConfig JsonSerializeConfig = new AutoCSer.Json.SerializeConfig { IsViewClientType = true, GetLoopObject = "AutoCSer.Pub.GetJsonLoop", SetLoopObject = "AutoCSer.Pub.SetJsonLoop", NullChar = ' ', IsMaxNumberToString = true, CheckLoopDepth = AutoCSer.Json.SerializeConfig.DefaultCheckLoopDepth, DateTimeType = Json.DateTimeType.Javascript, IsIntegerToHex = true, IsBoolToInt = true };
        /// <summary>
        /// 公用错误处理函数名称
        /// </summary>
        public const string PubErrorCallName = "Pub.Error";
        /// <summary>
        /// AJAX 输出数据流
        /// </summary>
        internal readonly CharStream AjaxStream = new CharStream(default(AutoCSer.Memory.Pointer));
        /// <summary>
        /// AJAX 调用信息
        /// </summary>
        internal AjaxMethodInfo MethodInfo;
        /// <summary>
        /// 内存流最大字节数
        /// </summary>
        internal override AutoCSer.Memory.BufferSize MaxMemoryStreamSize { get { return MethodInfo.MaxMemoryStreamSize; } }
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
        internal void Response<valueType>(ref valueType value, ref AutoCSer.Memory.Pointer buffer) where valueType : struct
        {
            AjaxStream.Reset(ref buffer);
            using (AjaxStream)
            {
                SetJsContentType();
                bool isCallBack = ResponseAjaxCallBack(AjaxStream);
                AutoCSer.JsonSerializer.Serialize(ref value, AjaxStream, JsonSerializeConfig);
                if (isCallBack) AjaxStream.Write(')');
                PageResponse.SetBody(AjaxStream, ref DomainServer.ResponseEncoding);
            }
        }
        /// <summary>
        /// AJAX 响应输出
        /// </summary>
        /// <param name="buffer">输出缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Response(ref AutoCSer.Memory.Pointer buffer)
        {
            AjaxStream.Reset(ref buffer);
            using (AjaxStream)
            {
                SetJsContentType();
                if (ResponseAjaxCallBack(AjaxStream)) AjaxStream.Write(')');
                PageResponse.SetBody(AjaxStream, ref DomainServer.ResponseEncoding);
            }
        }
    }
}
