using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.IO
{
    /// <summary>
    /// 文件扩展操作
    /// </summary>
    public static class File
    {
        /// <summary>
        /// 完全限定文件名必须少于 260 个字符
        /// </summary>
        internal const int MaxFullNameLength = 260;
        /// <summary>
        /// 临时文件前缀
        /// </summary>
        public const string BakPrefix = "%";
        /// <summary>
        /// 修改文件名成为默认备份文件 %yyyyMMdd-HHmmss_HEX_fileName
        /// </summary>
        /// <param name="fileName">源文件名</param>
        /// <returns>备份文件名称,失败返回null</returns>
        internal static string MoveBak(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                string newFileName = MoveBakFileName(fileName);
                System.IO.File.Move(fileName, newFileName);
                return newFileName;
            }
            return null;
        }
        /// <summary>
        /// 获取备份文件名称 %yyyyMMdd-HHmmss_HEX_fileName
        /// </summary>
        /// <param name="fileName">源文件名</param>
        /// <returns>备份文件名称</returns>
        internal static string MoveBakFileName(string fileName)
        {
            string newFileName = null;
            int index = fileName.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1;
            do
            {
                string bakName = BakPrefix + Date.NowTime.Now.ToString("yyyyMMdd-HHmmss") + "_" + ((uint)Random.Default.Next()).toHex() + "_";
                newFileName = index != 0 ? fileName.Insert(index, bakName) : (bakName + fileName);
            }
            while (System.IO.File.Exists(newFileName));
            return newFileName;
        }
        /// <summary>
        /// 文件名转小写
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string FileNameToLower(string fileName)
        {
#if MONO
            return fileName;
#else
            return fileName.ToLower();
#endif
        }
    }
}
