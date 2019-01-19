using System;
using AutoCSer.Extension;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字数据接收器
    /// </summary>
    /// <typeparam name="socketType">HTTP 套接字类型</typeparam>
    internal abstract unsafe class BoundaryReceiver<socketType>
        where socketType : SocketBase
    {
        /// <summary>
        /// 缓存文件名称前缀
        /// </summary>
        protected static readonly string cacheFileName = AutoCSer.Config.Pub.Default.CachePath + ((ulong)Date.NowTime.Now.Ticks).toHex();
        /// <summary>
        /// HTTP 套接字
        /// </summary>
        protected readonly socketType httpSocket;
        /// <summary>
        /// HTTP 头部
        /// </summary>
        protected Header header;
        /// <summary>
        /// HTTP 请求表单
        /// </summary>
        protected Form form;
        /// <summary>
        /// 当前表单值文件流
        /// </summary>
        protected FileStream fileStream;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        protected SubBuffer.PoolBufferFull buffer;
        /// <summary>
        /// 提交数据分隔符
        /// </summary>
        protected BufferIndex boundary;
        /// <summary>
        /// 数据缓冲区大小
        /// </summary>
        protected int bufferSize;
        /// <summary>
        /// 剩余表单数据内容长度
        /// </summary>
        protected int contentLength;
        /// <summary>
        /// 接收数据结束位置
        /// </summary>
        protected int receiveEndIndex;
        /// <summary>
        /// 数据起始位置
        /// </summary>
        protected int startIndex;
        /// <summary>
        /// 当前数据位置
        /// </summary>
        protected int currentIndex;
        /// <summary>
        /// 名称
        /// </summary>
        protected byte[] currentName;
        /// <summary>
        /// 表单值
        /// </summary>
        protected byte[] currentValue;
        /// <summary>
        /// 客户端文件名称
        /// </summary>
        protected byte[] currentFileName;
        /// <summary>
        /// 服务端保存文件名称
        /// </summary>
        protected string saveFileName;
        /// <summary>
        /// 表单值当前起始位置换行符标识
        /// </summary>
        protected int valueEnterIndex;
        /// <summary>
        /// 接受数据处理类型
        /// </summary>
        protected BoundaryReceiveType receiveType;
        /// <summary>
        /// 接受数据处理任务类型
        /// </summary>
        protected BoundaryReceiveLinkTaskType linkTaskType;
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        public long LinkTaskTicks { get; set; }
        /// <summary>
        /// 下一个任务节点
        /// </summary>
        public AutoCSer.Threading.ILinkTask NextLinkTask { get; set; }
        /// <summary>
        /// HTTP 套接字数据接收器
        /// </summary>
        /// <param name="socket"></param>
        protected BoundaryReceiver(socketType socket)
        {
            httpSocket = socket;
            header = socket.HttpHeader;
            form = socket.Form;
        }
        /// <summary>
        /// 获取表单名称值
        /// </summary>
        /// <param name="dataFixed">数据</param>
        /// <param name="start">数据起始位置</param>
        /// <param name="end">数据结束位置</param>
        /// <returns></returns>
        protected byte[] getName(byte* dataFixed, byte* start, byte* end)
        {
            while (*start == ' ') ++start;
            if (*start == '=')
            {
                while (*++start == ' ') ;
                if (*start == '"')
                {
                    byte* valueStart = ++start;
                    for (*end = (byte)'"'; *start != '"'; ++start) ;
                    if (start != end)
                    {
                        byte[] value = new byte[start - valueStart];
                        System.Buffer.BlockCopy(buffer.Buffer, (int)(valueStart - dataFixed), value, 0, value.Length);
                        return value;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取表单值
        /// </summary>
        protected void getValue()
        {
            System.Buffer.BlockCopy(buffer.Buffer, buffer.StartIndex + startIndex, currentValue = new byte[currentIndex - startIndex], 0, currentValue.Length);
            if (currentFileName == null)
            {
                if (currentName.Length == 1)
                {
                    switch (currentName[0])
                    {
                        case (byte)Header.QueryJsonNameChar: form.SetText(httpSocket.GetFormText(currentValue), Header.QueryJsonNameChar); return;
                        case (byte)Header.QueryXmlNameChar: form.SetText(httpSocket.GetFormText(currentValue), Header.QueryXmlNameChar); return;
                    }
                }
                form.Add(currentName, currentValue);
            }
            else form.AddFile(currentName, currentValue, currentFileName);
        }
        /// <summary>
        /// 获取文件表单值
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void addFile()
        {
            fileStream.Write(buffer.Buffer, buffer.StartIndex + startIndex, currentIndex - startIndex);
            fileStream.Dispose();
            fileStream = null;
            form.AddFile(currentName, currentFileName, saveFileName);
        }
    }
}
