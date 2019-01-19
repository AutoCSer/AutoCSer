using System;
using System.Diagnostics;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志处理
    /// </summary>
    public partial interface ILog
    {
        /// <summary>
        /// 日志处理类型
        /// </summary>
        LogType Type { get; set; }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        void Add(LogType type, Exception error, string message, CacheType cache);
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        void Add(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache);
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        void Wait(LogType type, Exception error, string message, CacheType cache);
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        void Wait(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache);
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        void Throw(LogType type, Exception error, string message, CacheType cache);
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        void Throw(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache);
        /// <summary>
        /// 同步添加日志并抛出异常
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        void WaitThrow(LogType type, Exception error, string message, CacheType cache);
        /// <summary>
        /// 同步添加日志并抛出异常
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="cache">缓存类型</param>
        void WaitThrow(LogType type, string message, StackFrame stackFrame, StackTrace stackTrace, CacheType cache);
        /// <summary>
        /// 等待写入文件
        /// </summary>
        void Flush();
    }
}
