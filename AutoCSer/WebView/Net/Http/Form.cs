using System;
using System.Text;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 请求表单
    /// </summary>
    public sealed unsafe partial class Form
    {
        /// <summary>
        /// HTTP 套接字
        /// </summary>
        private SocketBase socket;
        /// <summary>
        /// 表单数据集合
        /// </summary>
        internal LeftArray<FormValue> Values;
        /// <summary>
        /// 文件集合
        /// </summary>
        internal LeftArray<FormValue> Files;
        /// <summary>
        /// 字符串
        /// </summary>
        public string Text { get; internal set; }
        /// <summary>
        /// 查询字符
        /// </summary>
        internal char TextQueryChar;
        /// <summary>
        /// HTTP 请求表单
        /// </summary>
        /// <param name="socket"></param>
        internal Form(SocketBase socket)
        {
            this.socket = socket;
        }
        /// <summary>
        /// 清除表单数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            if (Values.Length != 0) clear(ref Values);
            if (Files.Length != 0) clear(ref Files);
            Text = null;
            TextQueryChar = (char)0;
        }
        /// <summary>
        /// 添加表单值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Add(byte[] name, byte[] value)
        {
            Values.PrepLength(1);
            Files.Array[Files.Length++].Set(name, value);
        }
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="saveFileName"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddFile(byte[] name, byte[] fileName, string saveFileName)
        {
            Files.PrepLength(1);
            Files.Array[Files.Length++].SetFile(name, fileName, saveFileName);
        }
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="fileName"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddFile(byte[] name, byte[] value, byte[] fileName)
        {
            Files.PrepLength(1);
            Files.Array[Files.Length++].SetFile(name, value, fileName);
        }
        /// <summary>
        /// 设置表单字符串
        /// </summary>
        /// <param name="text">表单字符串</param>
        /// <param name="textQueryChar">查询字符</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetText(string text, char textQueryChar)
        {
            Text = text;
            TextQueryChar = textQueryChar;
        }
        /// <summary>
        /// 解析表单数据
        /// </summary>
        internal void Parse()
        {
            byte[] buffer = socket.FormBuffer.Buffer;
            fixed (byte* bufferFixed = buffer)
            {
                byte* bufferStart = bufferFixed + socket.FormBuffer.StartIndex;
                byte* current = bufferStart - 1, end = bufferStart + socket.FormBufferReceiveEndIndex;
                *end = (byte)'&';
                do
                {
                    int nameIndex = (int)(++current - bufferFixed);
                    while (*current != '&' && *current != '=') ++current;
                    int nameLength = (int)(current - bufferFixed) - nameIndex;
                    if (*current == '=')
                    {
                        int valueIndex = (int)(++current - bufferFixed);
                        while (*current != '&') ++current;
                        if (nameLength == 1)
                        {
                            int valueLength = (int)(current - bufferFixed) - valueIndex;
                            switch (bufferFixed[nameIndex])
                                {
                                    case (byte)Header.QueryJsonNameChar:
                                    case (byte)Header.QueryXmlNameChar:
                                        TextQueryChar = (char)bufferFixed[nameIndex];
                                        Text = valueLength == 0 ? string.Empty : Header.UnescapeUtf8(bufferFixed + valueIndex, valueLength, buffer, valueIndex);
                                        break;
                                    default:
                                        Values.PrepLength(1);
                                        Values.Array[Values.Length++].SetNameValue(buffer, nameIndex, nameLength, valueIndex, valueLength);
                                        break;
                                }
                        }
                        else
                        {
                            Values.PrepLength(1);
                            Values.Array[Values.Length++].SetNameValue(buffer, nameIndex, nameLength, valueIndex, (int)(current - bufferFixed) - valueIndex);
                        }
                    }
                    else if (nameLength != 0)
                    {
                        Values.PrepLength(1);
                        Values.Array[Values.Length++].SetName(buffer, nameIndex, nameLength);
                    }
                }
                while (current != end);
                //this.Buffer = buffer;
            }
        }

        /// <summary>
        /// 清除表单数据
        /// </summary>
        /// <param name="values">表单数据集合</param>
        private static void clear(ref LeftArray<FormValue> values)
        {
            FormValue[] formArray = values.Array;
            for (int index = values.Length; index != 0; formArray[--index].Clear()) ;
            values.Length = 0;
        }
    }
}
