using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DebugString
    {
        /// <summary>
        /// 字符串转换流
        /// </summary>
        internal CharStream DebugStream;
        /// <summary>
        /// 日志类型
        /// </summary>
        internal string Type;
        /// <summary>
        /// 调用堆栈
        /// </summary>
        internal string StackTrace;
        /// <summary>
        /// 错误异常
        /// </summary>
        internal string Exception;

        /// <summary>
        /// 调用堆栈帧 函数类型名称
        /// </summary>
        private string stackFrameMethodTypeName;
        /// <summary>
        /// 调用堆栈帧 函数名称
        /// </summary>
        private string stackFrameMethodString;
        /// <summary>
        /// 调用堆栈帧 源代码文件路径
        /// </summary>
        private string stackFrameFile;
        /// <summary>
        /// 调用堆栈帧 源代码行号
        /// </summary>
        private int stackFrameLine;
        /// <summary>
        /// 调用堆栈帧 源代码列号
        /// </summary>
        private int stackFrameColumn;
        /// <summary>
        /// 设置 调用堆栈帧
        /// </summary>
        /// <param name="StackFrame"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(StackFrame StackFrame)
        {
            MethodBase stackFrameMethod = StackFrame.GetMethod();
            if (stackFrameMethod == null)
            {
                StackFrame = null;
                //if (StackTrace == null) StackTrace = new StackTrace(true);
            }
            else
            {
                stackFrameMethodTypeName = stackFrameMethod.ReflectedType.FullName;
                stackFrameMethodString = stackFrameMethod.ToString();
                stackFrameFile = StackFrame.GetFileName();
                if (stackFrameFile != null)
                {
                    stackFrameLine = StackFrame.GetFileLineNumber();
                    stackFrameColumn = StackFrame.GetFileColumnNumber();
                }
            }
        }

        /// <summary>
        /// 输出 提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(string message)
        {
            DebugStream.ByteSize = 0;
            DebugStream.SimpleWriteNotNull(@"
");
            DebugStream.SimpleWriteNotNull(Type);
            DebugStream.SimpleWriteNotNull(@" - ");
            if (message != null) DebugStream.WriteNotNull(message);
            if (stackFrameMethodTypeName != null)
            {
                DebugStream.Write(@"
堆栈帧信息 : ");
                DebugStream.WriteNotNull(stackFrameMethodTypeName);
                DebugStream.SimpleWriteNotNull(" + ");
                DebugStream.WriteNotNull(stackFrameMethodString);
                if (stackFrameFile != null)
                {
                    DebugStream.SimpleWriteNotNull(" in ");
                    DebugStream.WriteNotNull(stackFrameFile);
                    DebugStream.SimpleWriteNotNull(" line ");
                    AutoCSer.Extension.Number.ToString(stackFrameLine, DebugStream);
                    DebugStream.SimpleWriteNotNull(" col ");
                    AutoCSer.Extension.Number.ToString(stackFrameColumn, DebugStream);
                }
            }
        }
        /// <summary>
        /// 输出堆栈与异常信息
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write()
        {
            if (StackTrace != null)
            {
                DebugStream.SimpleWriteNotNull(@"
堆栈信息 : ");
                DebugStream.WriteNotNull(StackTrace);
            }
            if (Exception != null)
            {
                DebugStream.SimpleWriteNotNull(@"
异常信息 : ");
                DebugStream.WriteNotNull(Exception);
            }
        }
    }
}
