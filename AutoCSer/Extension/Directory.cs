using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 目录相关操作
    /// </summary>
    public static partial class DirectoryExtension
    {
        /// <summary>
        /// 目录分隔符
        /// </summary>
        public static readonly string Separator = Path.DirectorySeparatorChar.ToString();
        /// <summary>
        /// 取以\结尾的路径全名
        /// </summary>
        /// <param name="path">目录</param>
        /// <returns>\结尾的路径全名</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string fullName(this System.IO.DirectoryInfo path)
        {
            string name = path.FullName;
            return name[name.Length - 1] == Path.DirectorySeparatorChar ? name : (name + Separator);
        }
        /// <summary>
        /// 目录分隔符\替换
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>替换\后的路径</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string pathSeparator(this string path)
        {
            if (Path.DirectorySeparatorChar != '\\') path.replaceNotNull('\\', Path.DirectorySeparatorChar);
            return path;
        }
        /// <summary>
        /// 路径补全结尾的\
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>路径</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string pathSuffix(this string path)
        {
            if (string.IsNullOrEmpty(path)) return Separator;
            return path[path.Length - 1] == Path.DirectorySeparatorChar ? path : (path + Separator);
        }
    }
}
