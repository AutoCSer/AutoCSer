using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// GIF文件解码器
    /// </summary>
    internal sealed unsafe class Decoder
    {
        /// <summary>
        /// GIF文件数据
        /// </summary>
        private byte[] data;
        /// <summary>
        /// GIF文件数据起始位置
        /// </summary>
        private byte* dataPoint;
        /// <summary>
        /// GIF文件数据当前解析位置
        /// </summary>
        private byte* currentData;
        /// <summary>
        /// GIF文件数据结束位置
        /// </summary>
        private byte* dataEnd;
        /// <summary>
        /// 是否文件结束
        /// </summary>
        internal bool IsFileEnd
        {
            get { return *currentData == 0x3b; }
        }
        /// <summary>
        /// GIF文件解码器
        /// </summary>
        /// <param name="data">GIF文件数据</param>
        /// <param name="dataPoint"></param>
        /// <param name="currentData"></param>
        internal Decoder(byte[] data, byte* dataPoint, byte* currentData)
        {
            this.data = data;
            this.dataPoint = dataPoint;
            this.currentData = currentData;
            dataEnd = dataPoint + data.Length - 1;
        }
        /// <summary>
        /// 解码下一个数据块
        /// </summary>
        /// <param name="dataBlock"></param>
        /// <returns></returns>
        internal bool Next(ref DataBlock dataBlock)
        {
            if (*currentData == 0x2c) return decodeImage(ref dataBlock);
            if (*currentData == 0x21)
            {
                if (*++currentData == 1) return decodePlainText(ref dataBlock);
                switch (*currentData - 0xf9)
                {
                    case 0xf9 - 0xf9:
                        return decodeGraphicControl(ref dataBlock);
                    case 0xfe - 0xf9:
                        return decodeComment(ref dataBlock);
                    case 0xff - 0xf9:
                        return decodeApplication(ref dataBlock);
                }
            }
            return false;
        }
        /// <summary>
        /// 解码图像块
        /// </summary>
        /// <param name="dataBlock"></param>
        /// <returns></returns>
        private bool decodeImage(ref DataBlock dataBlock)
        {
            int length = data.Length - (int)(currentData - dataPoint) - 12;
            if (length > 0)
            {
                Image image = new Image();
                image.SetDescriptor(++currentData);
                currentData += 9;
                if (image.Colors != null)
                {
                    int colorCount = image.Colors.Length;
                    length -= (colorCount << 1) + colorCount;
                    if (length <= 0) return false;
                    currentData = FillColor(image.Colors, currentData);
                }
                image.LzwSize = *currentData++;
                if (getBlockList(ref image.LzwDatas))
                {
                    dataBlock.Set(image);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 解码图形控制扩展
        /// </summary>
        /// <param name="dataBlock"></param>
        /// <returns></returns>
        private bool decodeGraphicControl(ref DataBlock dataBlock)
        {
            if (data.Length - (int)(++currentData - dataPoint) > 6 && ((*currentData ^ 4) | *(currentData + 5)) == 0)
            {
                PraphicControl graphicControl = new PraphicControl(++currentData);
                currentData += 5;
                dataBlock.Set(graphicControl);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 解码图形文本扩展
        /// </summary>
        /// <param name="dataBlock"></param>
        /// <returns></returns>
        private bool decodePlainText(ref DataBlock dataBlock)
        {
            if (*++currentData == 12)
            {
                PlainText plainText = new PlainText(++currentData);
                if ((currentData += 12) < dataEnd && getBlockList(ref plainText.TextData))
                {
                    dataBlock.Set(plainText);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 解码注释扩展
        /// </summary>
        /// <param name="dataBlock"></param>
        /// <returns></returns>
        private bool decodeComment(ref DataBlock dataBlock)
        {
            if (++currentData < dataEnd)
            {
                SubArray<byte> comment = default(SubArray<byte>);
                if (getBlocks(ref comment))
                {
                    dataBlock.Set(ref comment);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 解码应用程序扩展
        /// </summary>
        /// <param name="dataBlock"></param>
        /// <returns></returns>
        private bool decodeApplication(ref DataBlock dataBlock)
        {
            if (*++currentData == 11)
            {
                int startIndex = (int)(currentData - dataPoint);
                if ((currentData += 12) < dataEnd)
                {
                    LeftArray<SubArray<byte>> customDatas = default(LeftArray<SubArray<byte>>);
                    if (getBlockList(ref customDatas))
                    {
                        dataBlock.Set(new Application(new SubArray<byte> { Array = data, Start = startIndex + 1, Length = 8 }, new SubArray<byte> { Array = data, Start = startIndex + 9, Length = 3 }, customDatas));
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 填充数据块
        /// </summary>
        /// <param name="blockData"></param>
        /// <returns></returns>
        private bool getBlocks(ref SubArray<byte> blockData)
        {
            byte* dataStart = currentData;
            for (byte count = *currentData; count != 0; count = *currentData)
            {
                currentData += count;
                if (++currentData >= dataEnd) return false;
            }
            blockData.Set(data, (int)(dataStart - dataPoint), (int)(currentData - dataStart));
            ++currentData;
            return true;
        }
        /// <summary>
        /// 填充数据块集合
        /// </summary>
        /// <param name="datas">填充数据块集合</param>
        /// <returns></returns>
        private bool getBlockList(ref LeftArray<SubArray<byte>> datas)
        {
            int startIndex = (int)(currentData - dataPoint);
            for (byte count = *currentData; count != 0; count = *currentData)
            {
                currentData += count;
                if (++currentData >= dataEnd) return false;
                datas.PrepLength(1);
                datas.Array[datas.Length++].Set(data, ++startIndex, count);
                startIndex += count;
            }
            ++currentData;
            return true;
        }
        /// <summary>
        /// 颜色列表数据填充
        /// </summary>
        /// <param name="colors">颜色列表数组</param>
        /// <param name="data">颜色列表数据</param>
        /// <returns>数据结束位置</returns>
        internal static unsafe byte* FillColor(Color[] colors, byte* data)
        {
            fixed (Color* globalColorsFixed = colors)
            {
                int offset = colors.Length & (sizeof(ulong) - 1);
                Color* currentColor = globalColorsFixed;
                for (Color* endColor = currentColor + (colors.Length - offset); currentColor != endColor; ++currentColor)
                {
                    ulong value0 = *(ulong*)data, value1 = *(ulong*)(data + sizeof(ulong));
                    (*currentColor).Value = (int)(uint)value0;
                    (*++currentColor).Value = (int)(uint)(value0 >> 24);
                    (*++currentColor).Value = (int)((uint)(value0 >> 48) | ((uint)value1 << 16));
                    (*++currentColor).Value = (int)((uint)value1 >> 8);
                    value0 = *(ulong*)(data + sizeof(ulong) * 2);
                    (*++currentColor).Value = (int)(uint)(value1 >> 32);
                    (*++currentColor).Value = (int)((uint)(value1 >> 56) | ((uint)value0 << 8));
                    (*++currentColor).Value = (int)(uint)(value0 >> 16);
                    (*++currentColor).Value = (int)(uint)(value0 >> 40);
                    //(*++currentColor).Value = (int)((value0 >> 24) | (value1 << 8));
                    //value0 = *(cpuUint*)(data + sizeof(cpuUint) * 2);
                    //(*++currentColor).Value = (int)((value1 >> 16) | (value0 << 16));
                    //(*++currentColor).Value = (int)(value0 >> 8);
                    data += sizeof(ulong) * 3;
                }
                for (Color* endColor = currentColor + offset; currentColor != endColor; ++currentColor)
                {
                    (*currentColor).Red = *data++;
                    (*currentColor).Green = *data++;
                    (*currentColor).Blue = *data++;
                }
            }
            return data;
        }
        /// <summary>
        /// 合并数据块集合
        /// </summary>
        /// <param name="datas">数据块集合</param>
        /// <returns>合并后的数据块</returns>
        internal unsafe static byte[] BlocksToByte(ref LeftArray<SubArray<byte>> datas)
        {
            if (datas.Length != 0)
            {
                int length = 0, count = datas.Length;
                SubArray<byte>[] array = datas.Array;
                for (int index = 0; index != count; ++index) length += array[index].Length;
                byte[] data = new byte[length];
                fixed (byte* dataFixed = data)
                {
                    byte* currentData = dataFixed;
                    for (int index = 0; index != count; ++index)
                    {
                        SubArray<byte> subArray = array[index];
                        fixed (byte* subArrayFixed = subArray.Array)
                        {
                            AutoCSer.Memory.CopyNotNull(subArrayFixed + subArray.Start, currentData, subArray.Length);
                        }
                        currentData += subArray.Length;
                    }
                }
                return data;
            }
            return null;
        }
    }
}
