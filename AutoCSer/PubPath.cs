using System;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 常用公共定义
    /// </summary>
    public static class PubPath
    {
        /// <summary>
        /// 程序执行主目录(小写字母)
        /// </summary>
        public static readonly string ApplicationPath;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string fileNameToLower(this string fileName)
        {
#if MONO
            return fileName;
#else
            return fileName.toLower();
#endif
        }
        /// <summary>
        /// 文件名转小写（MONO 不做处理）
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string FileNameToLower(this string fileName)
        {
#if MONO
            return fileName;
#else
            return fileName.ToLower();
#endif
        }
        /// <summary>
        /// 文件路径比较
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>匹配返回 0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int PathCompare(this string path, string fileName)
        {
#if MONO
            return string.Compare(fileName, 0, path, 0, path.Length, false);
#else
            return string.Compare(fileName, 0, path, 0, path.Length, true);
#endif
        }
        static PubPath()
        {
            string applicationPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory).FullName;
            if (applicationPath[applicationPath.Length - 1] == Path.DirectorySeparatorChar)
            {
#if MONO
                ApplicationPath = applicationPath;
#else
                ApplicationPath = applicationPath.ToLower();
#endif
            }
            else
            {
                ApplicationPath = (applicationPath + AutoCSer.Extension.DirectoryExtension.Separator).toLowerNotEmpty();
            }
        }
    }
}
