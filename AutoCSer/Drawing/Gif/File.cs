using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// GIF文件
    /// </summary>
    public class File
    {
        ///// <summary>
        ///// 版本号
        ///// </summary>
        //public subArray<byte> Version { get; private set; }
        /// <summary>
        /// 素数宽度
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 素数高度
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// 颜色深度
        /// </summary>
        private byte colorResoluTion;
        /// <summary>
        /// 全局颜色列表是否分类排列
        /// </summary>
        private byte sortFlag;
        /// <summary>
        /// 全局颜色列表
        /// </summary>
        public Color[] GlobalColors { get; private set; }
        /// <summary>
        /// 背景颜色在全局颜色列表中的索引，如果没有全局颜色列表，该值没有意义
        /// </summary>
        private byte backgroundColorIndex;
        /// <summary>
        /// 像素宽高比
        /// </summary>
        private byte pixelAspectRadio;
        /// <summary>
        /// 数据块集合
        /// </summary>
        private LeftArray<DataBlock> blocks;
        /// <summary>
        /// 数据块集合
        /// </summary>
        public DataBlock[] Blocks
        {
            get { return blocks.GetArray(); }
        }
        /// <summary>
        /// GIF文件是否解析成功
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// GIF文件
        /// </summary>
        /// <param name="data">GIF文件数据</param>
        private unsafe File(byte[] data)
        {
            fixed (byte* dataFixed = data)
            {
                if ((*(int*)dataFixed & 0xffffff) == ('G' | ('I' << 8) | ('F' << 16)))
                {
                    Width = *(short*)(dataFixed + 6);
                    Height = *(short*)(dataFixed + 8);
                    byte globalFlag = *(dataFixed + 10);
                    backgroundColorIndex = *(dataFixed + 11);
                    pixelAspectRadio = *(dataFixed + 12);
                    colorResoluTion = (byte)(((globalFlag >> 4) & 7) + 1);
                    sortFlag = (byte)(globalFlag & 8);
                    byte* currentData = dataFixed + 6 + 7;
                    if ((globalFlag & 0x80) != 0)
                    {
                        int colorCount = 1 << ((globalFlag & 7) + 1);
                        if (data.Length < 14 + (colorCount << 1) + colorCount) return;
                        currentData = Decoder.FillColor(GlobalColors = new Color[colorCount], currentData);
                    }
                    Decoder decoder = new Decoder(data, dataFixed, currentData);
                    while (!decoder.IsFileEnd)
                    {
                        blocks.PrepLength(1);
                        if (!decoder.Next(ref blocks.Array[blocks.Length++])) return;
                    }
                    IsCompleted = true;
                }
            }
        }
        /// <summary>
        /// GIF文件
        /// </summary>
        /// <param name="data">GIF文件数据</param>
        /// <returns>GIF文件,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static File Create(byte[] data)
        {
            if (data.length() > 3 + 3 + 7 + 1)
            {
                File file = new File(data);
                if (file.IsCompleted) return file;
            }
            return null;
        }
        /// <summary>
        /// GIF文件
        /// </summary>
        /// <param name="filename">GIF文件名</param>
        /// <returns>GIF文件,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static File Create(string filename)
        {
            return System.IO.File.Exists(filename) ? Create(System.IO.File.ReadAllBytes(filename)) : null;
        }
    }
}
