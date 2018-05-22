using System;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 错误信息
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// 提示信息集合
        /// </summary>
        private static readonly HashSet<string> messages = HashSetCreator.CreateOnly<string>();
        /// <summary>
        /// 是否存在提示信息
        /// </summary>
        private static bool isMessage;
        /// <summary>
        /// 是否存在提示信息
        /// </summary>
        internal static bool IsMessage
        {
            get { return isMessage || messages.Count != 0; }
        }
        /// <summary>
        /// 错误信息集合
        /// </summary>
        private static readonly HashSet<string> errors = HashSetCreator.CreateOnly<string>();
        /// <summary>
        /// 异常集合
        /// </summary>
        private static LeftArray<Exception> exceptions;
        /// <summary>
        /// 是否存在错误或者异常信息
        /// </summary>
        private static bool isError;
        /// <summary>
        /// 是否存在错误或者异常信息
        /// </summary>
        internal static bool IsError
        {
            get { return isError || errors.Count != 0 || exceptions.Length != 0; }
        }
        /// <summary>
        /// 清除所有信息
        /// </summary>
        internal static void Clear()
        {
            errors.Clear();
            exceptions.SetNull();
            messages.Clear();
            isError = isMessage = false;
        }
        /// <summary>
        /// 信息输出到日志
        /// </summary>
        /// <returns>是否存在错误信息</returns>
        private static bool output()
        {
            if (messages.Count != 0)
            {
#if DOTNET2
                AutoCSer.Log.Pub.Log.Wait(Log.LogType.All, messages.getArray().joinString(@"
- - - - - - - -
"));
#else
                AutoCSer.Log.Pub.Log.Wait(Log.LogType.All, messages.joinString(@"
- - - - - - - -
"));
#endif
                messages.Clear();
                isMessage = true;
            }
            if (errors.Count != 0)
            {
#if DOTNET2
                AutoCSer.Log.Pub.Log.Wait(Log.LogType.All, errors.getArray().joinString(@"
- - - - - - - -
"));
#else
                AutoCSer.Log.Pub.Log.Wait(Log.LogType.All, errors.joinString(@"
- - - - - - - -
"));
#endif
                errors.Clear();
                isError = true;
            }
            if (exceptions.Length != 0)
            {
                foreach (Exception error in exceptions) AutoCSer.Log.Pub.Log.Wait(Log.LogType.All, error);
                exceptions.Length = 0;
                isError = true;
            }
            return isError;
        }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Add(string error)
        {
            errors.Add(error);
        }
        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="error">异常</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Add(Exception error)
        {
            exceptions.Add(error);
        }
        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="value">提示信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Message(string value)
        {
            messages.Add(value);
        }
        /// <summary>
        /// 输出错误信息到日志并打开日志文件
        /// </summary>
        /// <returns>安装是否顺利,没有错误或者提示信息</returns>
        public static bool Open()
        {
            if (output() || IsMessage)
            {
                AutoCSer.Log.Pub.Log.Flush();
                string fileName = (AutoCSer.Log.Pub.Log as AutoCSer.Log.File).UnsafeMoveBak();
                if (fileName != null)
                {
#if DotNetStandard
                    string notepad = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System)).fullName() + "notepad.exe";
                    if (System.IO.File.Exists(notepad)) Process.Start(notepad, fileName);
                    else
#endif
                        Process.Start(fileName);
                }
                Clear();
                return false;
            }
            return true;
        }
    }
}
