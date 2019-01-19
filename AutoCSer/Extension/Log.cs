using System;
using AutoCSer.Log;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static partial class Log
    {
        /// <summary>
        /// 判断是否支持任意类型
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool IsAnyType(this ILog log, AutoCSer.Log.LogType type)
        {
            return (log.Type & type) != 0;
        }
        /// <summary>
        /// 判断是否支持所有类型
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool IsAllType(this ILog log, AutoCSer.Log.LogType type)
        {
            return (log.Type & type) == type;
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Add(this ILog log, AutoCSer.Log.LogType type, string message, StackFrame stackFrame, bool isCache = false)
        {
            log.Add(type, message, stackFrame, null, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Add(this ILog log, AutoCSer.Log.LogType type, string message, StackTrace stackTrace = null, bool isCache = false)
        {
            log.Add(type, message, null, stackTrace, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Add(this ILog log, AutoCSer.Log.LogType type, Exception error, string message = null, bool isCache = false)
        {
            log.Add(type, error, message, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Wait(this ILog log, AutoCSer.Log.LogType type, string message, StackFrame stackFrame, bool isCache = false)
        {
            log.Wait(type, message, stackFrame, null, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Wait(this ILog log, AutoCSer.Log.LogType type, string message, StackTrace stackTrace = null, bool isCache = false)
        {
            log.Wait(type, message, null, stackTrace, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Wait(this ILog log, AutoCSer.Log.LogType type, Exception error, string message = null, bool isCache = false)
        {
            log.Wait(type, error, message, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Throw(this ILog log, AutoCSer.Log.LogType type, string message, StackFrame stackFrame, bool isCache = false)
        {
            log.Throw(type, message, stackFrame, null, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Throw(this ILog log, AutoCSer.Log.LogType type, string message, StackTrace stackTrace = null, bool isCache = false)
        {
            log.Throw(type, message, null, stackTrace, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 抛出一般错误异常
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Throw(this ILog log, AutoCSer.Log.ErrorType error, string message = null, StackTrace stackTrace = null, bool isCache = false)
        {
            log.Throw(AutoCSer.Log.LogType.Error, "[" + error.ToString() + "] " + message, null, stackTrace, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 添加日志并抛出异常
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Throw(this ILog log, AutoCSer.Log.LogType type, Exception error, string message = null, bool isCache = false)
        {
            log.Throw(type, error, message, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 同步添加日志并抛出异常
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackFrame">堆栈帧函数信息</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void WaitThrow(this ILog log, AutoCSer.Log.LogType type, string message, StackFrame stackFrame, bool isCache = false)
        {
            log.WaitThrow(type, message, stackFrame, null, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 同步添加日志并抛出异常
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void WaitThrow(this ILog log, AutoCSer.Log.LogType type, string message, StackTrace stackTrace = null, bool isCache = false)
        {
            log.WaitThrow(type, message, null, stackTrace, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
        /// <summary>
        /// 同步添加日志并抛出异常
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="error">错误异常</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void WaitThrow(this ILog log, AutoCSer.Log.LogType type, Exception error, string message = null, bool isCache = false)
        {
            log.WaitThrow(type, error, message, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None);
        }
    }
}
