using System;
using System.IO;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 文件操作扩展
    /// </summary>
    public static class FileInfo_Expand
    {
        /// <summary>
        /// 判断文件头部是否 UTF-8
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool isUtf8(this FileInfo file)
        {
            if (file.Exists && file.Length >= 3)
            {
                byte[] data = new byte[4];
                using (FileStream fileStream = file.OpenRead()) fileStream.Read(data, 0, 3);
                return BitConverter.ToUInt32(data, 0) == 0xbfbbefU;
            }
            return false;
        }
        /// <summary>
        /// 判断文件头部是否 UTF-8
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public static bool isUtf8(this byte[] fileData)
        {
            if (fileData.Length >= 3)
            {
                if (fileData.Length == 3) return ((fileData[0] ^ 0xef) | (fileData[1] ^ 0xbb) | (fileData[2] ^ 0xbf)) == 0;
                return (BitConverter.ToUInt32(fileData, 0) & 0xffffffU) == 0xbfbbefU;
            }
            return false;
        }
    }
}
