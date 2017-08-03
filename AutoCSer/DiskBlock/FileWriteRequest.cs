using System;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 数据写入请求
    /// </summary>
    internal unsafe class FileWriteRequest : WriteRequest
    {
        /// <summary>
        /// 文件块
        /// </summary>
        private File file;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="onWrite">添加数据回调委托</param>
        /// <param name="file">文件块</param>
        internal FileWriteRequest(ref AppendBuffer buffer, Func<AutoCSer.Net.TcpServer.ReturnValue<ulong>, bool> onWrite, File file)
            : base(ref buffer, onWrite)
        {
            this.file = file;
            Index = buffer.Index;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        internal override void AppendWrite()
        {
            Index = 0;
            file.Append(this);
        }
    }
}
