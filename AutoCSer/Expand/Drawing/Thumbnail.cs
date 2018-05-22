using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Drawing
{
    /// <summary>
    /// 缩略图
    /// </summary>
    public static class Thumbnail
    {
        /// <summary>
        /// 图像缩略切剪
        /// </summary>
        /// <param name="data">图像文件数据</param>
        /// <param name="width">缩略宽度,0表示与高度同比例</param>
        /// <param name="height">缩略高度,0表示与宽度同比例</param>
        /// <param name="type">目标图像文件格式</param>
        /// <param name="seek">输出数据起始位置</param>
        /// <returns>图像缩略文件数据</returns>
        public static SubArray<byte> Cut(byte[] data, int width, int height, ImageFormat type, int seek = 0)
        {
            if (data == null) return default(SubArray<byte>);
            SubArray<byte> dataArray = new SubArray<byte>(0, data.Length, data);
            Cut(ref dataArray, width, height, type, seek);
            return dataArray;
        }
        /// <summary>
        /// 图像缩略切剪
        /// </summary>
        /// <param name="data">图像文件数据</param>
        /// <param name="width">缩略宽度,0表示与高度同比例</param>
        /// <param name="height">缩略高度,0表示与宽度同比例</param>
        /// <param name="type">目标图像文件格式</param>
        /// <param name="seek">输出数据起始位置</param>
        /// <returns>图像缩略文件数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<byte> Cut(SubArray<byte> data, int width, int height, ImageFormat type, int seek = 0)
        {
            Cut(ref data, width, height, type, seek);
            return data;
        }
        /// <summary>
        /// 图像缩略切剪
        /// </summary>
        /// <param name="data">图像文件数据</param>
        /// <param name="width">缩略宽度,0表示与高度同比例</param>
        /// <param name="height">缩略高度,0表示与宽度同比例</param>
        /// <param name="type">目标图像文件格式</param>
        /// <param name="seek">输出数据起始位置</param>
        /// <returns>图像缩略文件数据</returns>
        public static void Cut(ref SubArray<byte> data, int width, int height, ImageFormat type, int seek = 0)
        {
            if (data.Length != 0 && width > 0 && height > 0 && (width | height) != 0 && seek >= 0)
            {
                try
                {
                    using (MemoryStream memory = new MemoryStream(data.Array, data.Start, data.Length))
                    {
                        ThumbnailBuilder builder = new ThumbnailBuilder();
                        using (Image image = builder.CreateImage(memory))
                        {
                            builder.Cut(ref data, ref width, ref height, type, seek);
                            return;
                        }
                    }
                }
                catch (Exception error)
                {
                    AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                }
            }
            data.SetNull();
        }
        /// <summary>
        /// 图像缩略补白
        /// </summary>
        /// <param name="data">图像文件数据</param>
        /// <param name="width">缩略宽度,0表示与高度同比例</param>
        /// <param name="height">缩略高度,0表示与宽度同比例</param>
        /// <param name="type">目标图像文件格式</param>
        /// <param name="backColor">背景色</param>
        /// <param name="seek">输出数据起始位置</param>
        /// <returns>图像缩略文件数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<byte> Pad(SubArray<byte> data, int width, int height, ImageFormat type, Color backColor, int seek = 0)
        {
            Pad(ref data, width, height, type, backColor, seek);
            return data;
        }
        /// <summary>
        /// 图像缩略补白
        /// </summary>
        /// <param name="data">图像文件数据</param>
        /// <param name="width">缩略宽度,0表示与高度同比例</param>
        /// <param name="height">缩略高度,0表示与宽度同比例</param>
        /// <param name="type">目标图像文件格式</param>
        /// <param name="backColor">背景色</param>
        /// <param name="seek">输出数据起始位置</param>
        /// <returns>图像缩略文件数据</returns>
        public static void Pad(ref SubArray<byte> data, int width, int height, ImageFormat type, Color backColor, int seek = 0)
        {
            if (data.Length != 0 && width > 0 && height > 0 && (width | height) != 0 && seek >= 0)
            {
                try
                {
                    using (MemoryStream memory = new MemoryStream(data.Array, data.Start, data.Length))
                    {
                        ThumbnailBuilder builder = new ThumbnailBuilder();
                        using (Image image = builder.CreateImage(memory))
                        {
                            builder.Pad(ref data, ref width, ref height, type, backColor, seek);
                            return;
                        }
                    }
                }
                catch (Exception error)
                {
                    AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                }
            }
            data.SetNull();
        }
    }
}
