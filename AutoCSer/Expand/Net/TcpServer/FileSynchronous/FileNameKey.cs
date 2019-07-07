using System;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 文件名称关键字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct FileNameKey : IEquatable<FileNameKey>
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 文件名称信息
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名称</param>
        internal FileNameKey(string path, string name)
        {
            Path = path;
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(FileNameKey other)
        {
            return Name == other.Name && Path == other.Path;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode() ^ Name.GetHashCode();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((FileNameKey)obj);
        }
    }
}
