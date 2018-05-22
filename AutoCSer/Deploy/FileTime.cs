using System;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 文件最后修改时间
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct FileTime
    {
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        public DateTime LastWriteTimeUtc;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 文件数据
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        /// <param name="file"></param>
        internal FileTime(FileInfo file)
        {
            LastWriteTimeUtc = file.LastWriteTimeUtc;
            FileName = file.Name;
            Data = null;
        }
        /// <summary>
        /// 设置文件最后修改时间
        /// </summary>
        /// <param name="fileName"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(string fileName)
        {
            LastWriteTimeUtc = DateTime.MinValue;
            FileName = fileName;
        }
        /// <summary>
        /// 设置文件最后修改时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="fileName"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(DateTime time, string fileName)
        {
            LastWriteTimeUtc = time;
            FileName = fileName;
        }
        /// <summary>
        /// 加载文件数据
        /// </summary>
        /// <param name="path"></param>
        internal void Load(string path)
        {
            FileInfo file = new FileInfo(path + FileName);
            if (file.LastWriteTimeUtc >= LastWriteTimeUtc)
            {
                Data = File.ReadAllBytes(file.FullName);
                LastWriteTimeUtc = file.LastWriteTimeUtc;
            }
            else AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, "文件同步时间冲突 " + file.FullName);
        }
    }
}
