using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 列表文件数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct ListFileItem
    {
        /// <summary>
        /// 最后修改时间
        /// </summary>
        internal DateTime LastWriteTime;
        /// <summary>
        /// 是否文件, long.MinValue 表示目录
        /// </summary>
        internal long Length;
        /// <summary>
        /// 名称
        /// </summary>
        internal string Name;

        /// <summary>
        /// 是否文件, false 表示目录
        /// </summary>
        internal bool IsFile
        {
            get { return Length != long.MinValue; }
        }
        /// <summary>
        /// 列表文件数据
        /// </summary>
        /// <param name="File"></param>
        internal ListFileItem(FileInfo File)
        {
            Name = File.Name;
            Length = File.Length;
            LastWriteTime = File.LastWriteTimeUtc;
        }
        /// <summary>
        /// 列表文件数据
        /// </summary>
        /// <param name="Directory"></param>
        internal ListFileItem(DirectoryInfo Directory)
        {
            Name = Directory.Name;
            Length = long.MinValue;
            LastWriteTime = Directory.LastWriteTimeUtc;
        }
        /// <summary>
        /// 列表文件数据
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="LastWriteTime">最后修改时间</param>
        /// <param name="Length">文件长度</param>
        internal ListFileItem(string Name, DateTime LastWriteTime, long Length)
        {
            this.Name = Name;
            this.Length = Length;
            this.LastWriteTime = LastWriteTime;
        }
        /// <summary>
        /// 检测文件是否没有改变
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Check(FileInfo File)
        {
            return Length == File.Length && LastWriteTime == File.LastWriteTimeUtc;
        }
        /// <summary>
        /// 列表名称
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="LastWriteTime">最后修改时间</param>
        /// <param name="Length">文件长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(string Name, DateTime LastWriteTime, long Length)
        {
            this.Name = Name;
            this.Length = Length;
            this.LastWriteTime = LastWriteTime;
        }
    }
}
