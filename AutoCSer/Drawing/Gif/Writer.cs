using System;
using System.IO;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Drawing.Imaging;
using AutoCSer.Log;
using System.Drawing;
using AutoCSer.Algorithm;
using System.Runtime.CompilerServices;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// GIF 文件数据写入器
    /// </summary>
    public sealed class Writer : IDisposable
    {
        /// <summary>
        /// GIF文件标识与版本信息
        /// </summary>
        private const ulong fileVersion = 'G' + ('I' << 8) + ('F' << 16) + ('8' << 24) + ((ulong)'9' << 32) + ((ulong)'a' << 40);
        /// <summary>
        /// LZW压缩编码查询表缓冲区
        /// </summary>
        internal static readonly UnmanagedPool LzwEncodeTableBufferPool = UnmanagedPool.GetOrCreate(4096 * 256 * 2);
        ///// <summary>
        ///// 文件缓冲区
        ///// </summary>
        //private static readonly UnmanagedPool fileBufferPool = UnmanagedPool.GetOrCreate(UnmanagedPool.DefaultSize + (256 * 3) + 8);
        /// <summary>
        /// 输出数据流
        /// </summary>
        private Stream stream;
        /// <summary>
        /// 文件缓冲区
        /// </summary>
        private readonly byte[] fileBuffer = new byte[UnmanagedPool.DefaultSize + (256 * 3) + 8];
        /// <summary>
        /// 当前图像色彩缓存
        /// </summary>
        private readonly Color[] colors;
        /// <summary>
        /// 当前图像色彩数量缓存
        /// </summary>
        private readonly int[] colorCounts;
        /// <summary>
        /// 当前图像色彩数量
        /// </summary>
        private readonly ReusableDictionary<Color, int> colorIndexs;
        /// <summary>
        /// 日志处理
        /// </summary>
        private readonly ILog log;
        /// <summary>
        /// 全局颜色数量
        /// </summary>
        private readonly int globalColorCount;
        /// <summary>
        /// 是否自动释放输出数据流
        /// </summary>
        private readonly bool isLeaveDisposeStream;
        /// <summary>
        /// 素数宽度
        /// </summary>
        public short Width { get; private set; }
        /// <summary>
        /// 素数高度
        /// </summary>
        public short Height { get; private set; }
        /// <summary>
        /// 当前文件缓存位置
        /// </summary>
        private int bufferIndex;
        /// <summary>
        /// GIF文件写入器
        /// </summary>
        /// <param name="stream">输出数据流</param>
        /// <param name="width">素数宽度</param>
        /// <param name="height">素数高度</param>
        /// <param name="globalColors">全局颜色列表</param>
        /// <param name="backgroundColorIndex">背景颜色在全局颜色列表中的索引，如果没有全局颜色列表，该值没有意义</param>
        /// <param name="log">日志处理</param>
        /// <param name="isLeaveDisposeStream">是否自动释放输出数据流</param>
        public unsafe Writer(Stream stream, short width, short height, Color[] globalColors = null, byte backgroundColorIndex = 0, ILog log = null, bool isLeaveDisposeStream = false)
        {
            this.stream = stream;
            this.isLeaveDisposeStream = isLeaveDisposeStream;
            this.log = log ?? AutoCSer.Log.Pub.Log;
            if (stream == null) throw new ArgumentNullException();
            if (width <= 0) throw new IndexOutOfRangeException("width[" + width.toString() + "] <= 0");
            if (height <= 0) throw new IndexOutOfRangeException("height[" + height.toString() + "] <= 0");
            //if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename is null or empty");
            //fileStream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1, FileOptions.WriteThrough);
            Width = width;
            Height = height;
            globalColorCount = globalColors.length();
            int pixel = 0;
            if (globalColorCount != 0)
            {
                if (globalColorCount < 256)
                {
                    pixel = ((uint)globalColorCount).bits() - 1;
                    if (globalColorCount != (1 << pixel)) ++pixel;
                }
                else
                {
                    globalColorCount = 256;
                    pixel = 7;
                }
                pixel |= 0x80;
            }
            fixed (byte* bufferFixed = fileBuffer)
            {
                *(ulong*)bufferFixed = fileVersion | ((ulong)width << 48);
                *(uint*)(bufferFixed + 8) = (uint)(int)height | (globalColorCount == 0 ? 0 : ((uint)pixel << 16)) | (7 << (16 + 4))
                    | (backgroundColorIndex >= globalColorCount ? 0 : ((uint)backgroundColorIndex << 24));
                bufferIndex = 13;
                if (globalColorCount != 0)
                {
                    byte* currentBuffer = bufferFixed + 13;
                    fixed (Color* colorFixed = globalColors)
                    {
                        for (Color* currentColor = colorFixed, colorEnd = colorFixed + globalColorCount; currentColor != colorEnd; ++currentColor)
                        {
                            Color color = *currentColor;
                            *currentBuffer++ = color.Red;
                            *currentBuffer++ = color.Green;
                            *currentBuffer++ = color.Blue;
                        }
                    }
                    bufferIndex += 3 << (pixel ^ 0x80);
                }
            }
            colors = new Color[(int)Width * Height];
            colorCounts = new int[colors.Length];
            colorIndexs = ReusableDictionary<Color>.Create<int>();
        }
        /// <summary>
        /// 释放文件
        /// </summary>
        public void Dispose()
        {
            Stream stream = this.stream;
            if (stream != null)
            {
                this.stream = null;
                if (isLeaveDisposeStream)
                {
                    using (stream) onDispose();
                }
                else onDispose();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private void onDispose()
        {
            fileBuffer[bufferIndex++] = 0x3b;
            try
            {
                stream.Write(fileBuffer, 0, bufferIndex);
            }
            catch (Exception error)
            {
                log.Add(AutoCSer.Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 检测文件缓存
        /// </summary>
        /// <param name="bufferFixed">文件缓存起始位置</param>
        /// <returns>文件是否写入成功</returns>
        private unsafe bool checkBuffer(byte* bufferFixed)
        {
            int count = bufferIndex - UnmanagedPool.DefaultSize;
            if (count >= 0)
            {
                try
                {
                    stream.Write(fileBuffer, 0, UnmanagedPool.DefaultSize);
                    AutoCSer.Memory.CopyNotNull(bufferFixed + UnmanagedPool.DefaultSize, bufferFixed, bufferIndex = count);
                }
                catch (Exception error)
                {
                    log.Add(AutoCSer.Log.LogType.Error, error);
                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检测文件缓存
        /// </summary>
        /// <param name="bufferFixed">文件缓存起始位置</param>
        /// <param name="length">新增长度</param>
        /// <returns>文件是否写入成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe bool checkBuffer(byte* bufferFixed, int length)
        {
            bufferIndex += length;
            return checkBuffer(bufferFixed);
        }
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <param name="leftOffset">X方向偏移量</param>
        /// <param name="topOffset">Y方向偏移量</param>
        /// <param name="width">图象宽度</param>
        /// <param name="height">图象高度</param>
        /// <param name="bitmapLeftOffset">位图剪切X方向偏移量</param>
        /// <param name="bitmapTopOffset">位图剪切Y方向偏移量</param>
        /// <param name="isInterlace">图象数据是否连续方式排列，否则使用顺序排列</param>
        /// <param name="maxPixel">最大色彩深度</param>
        /// <returns>图片是否添加成功</returns>
        public unsafe bool AddImage(Bitmap bitmap, int bitmapLeftOffset = 0, int bitmapTopOffset = 0
            , int leftOffset = 0, int topOffset = 0, int width = 0, int height = 0, bool isInterlace = false, byte maxPixel = 8)
        {
            if (fileBuffer == null || bitmap == null) return false;
            if (width == 0) width = Width;
            if (height == 0) height = Height;
            if (leftOffset < 0)
            {
                bitmapLeftOffset -= leftOffset;
                width += leftOffset;
                leftOffset = 0;
            }
            if (topOffset < 0)
            {
                bitmapTopOffset -= topOffset;
                height += topOffset;
                topOffset = 0;
            }
            if (bitmapLeftOffset < 0)
            {
                leftOffset -= bitmapLeftOffset;
                width += bitmapLeftOffset;
                bitmapLeftOffset = 0;
            }
            if (bitmapTopOffset < 0)
            {
                topOffset -= bitmapTopOffset;
                height += bitmapTopOffset;
                bitmapTopOffset = 0;
            }
            int minWidth = bitmap.Width - bitmapLeftOffset, minHeight = bitmap.Height - bitmapTopOffset;
            if (minWidth < width) width = minWidth;
            if (minHeight < height) height = minHeight;
            if ((minWidth = width - leftOffset) < width) width = minWidth;
            if ((minHeight = height - topOffset) < height) height = minHeight;
            if (width <= 0 || height <= 0) return false;
            if ((byte)(maxPixel - 2) >= 8) maxPixel = 8;
            BitmapData bitmapData = null;
            try
            {
                bitmapData = bitmap.LockBits(new Rectangle(bitmapLeftOffset, bitmapTopOffset, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            }
            catch (Exception error)
            {
                log.Add(AutoCSer.Log.LogType.Error, error);
                return false;
            }
            try
            {
                return addImage(bitmapData, 0, 0, leftOffset, topOffset, width, height, isInterlace, maxPixel);
            }
            finally { bitmap.UnlockBits(bitmapData); }
        }
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <param name="leftOffset">X方向偏移量</param>
        /// <param name="topOffset">Y方向偏移量</param>
        /// <param name="width">图象宽度</param>
        /// <param name="height">图象高度</param>
        /// <param name="bitmapLeftOffset">位图剪切X方向偏移量</param>
        /// <param name="bitmapTopOffset">位图剪切Y方向偏移量</param>
        /// <param name="isInterlace">图象数据是否连续方式排列，否则使用顺序排列</param>
        /// <param name="maxPixel">最大色彩深度</param>
        /// <returns>图片是否添加成功</returns>
        internal unsafe bool addImage(BitmapData bitmap, int bitmapLeftOffset, int bitmapTopOffset
            , int leftOffset, int topOffset, int width, int height, bool isInterlace, byte maxPixel)
        {
            if (fileBuffer == null) return false;
            fixed (Color* colorFixed = colors)
            fixed (int* colorCountFixed = colorCounts)
            {
                byte* bitmapFixed = (byte*)bitmap.Scan0, currentBitmap = bitmapFixed + bitmap.Stride * (bitmapTopOffset - 1) + (bitmapLeftOffset + width) * 3;
                Color* currentColor = colorFixed;
                int bitMapSpace = bitmap.Stride - (width << 1) - width;
                colorIndexs.Empty();
                for (int colorIndex, row = height; row != 0; --row)
                {
                    currentBitmap += bitMapSpace;
                    for (int col = width; col != 0; --col)
                    {
                        Color color = new Color { Green = *currentBitmap++, Blue = *currentBitmap++, Red = *currentBitmap++ };
                        if (colorIndexs.TryGetValue(color, out colorIndex)) ++colorCountFixed[colorIndex];
                        else
                        {
                            colorIndexs.Set(color, colorIndex = (int)(currentColor - colorFixed));
                            colorCountFixed[colorIndex] = 1;
                        }
                        *currentColor++ = color;
                    }
                }
                int pixel = ((uint)colorIndexs.Count).bits() - 1;
                if ((1 << pixel) != colorIndexs.Count) ++pixel;
                if (pixel > maxPixel) pixel = maxPixel;
                else if (pixel < 2) pixel = 2;
                int maxColorCount = 1 << pixel;
                fixed (byte* bufferFixed = fileBuffer)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    *currentBuffer = 0x2c;
                    *(short*)(currentBuffer + 1) = (short)leftOffset;
                    *(short*)(currentBuffer + 3) = (short)topOffset;
                    *(short*)(currentBuffer + 5) = (short)width;
                    *(short*)(currentBuffer + 7) = (short)height;
                    *(currentBuffer + 9) = (byte)(0x80 + (isInterlace ? 0x40 : 0) + (pixel - 1));
                    if (!checkBuffer(bufferFixed, 10)) return false;
                }
                if (colorIndexs.Count <= maxColorCount)
                {
                    fixed (byte* bufferFixed = fileBuffer)
                    {
                        int* currentColorCount = colorCountFixed;
                        foreach (Color colorKey in colorIndexs.Keys) *currentColorCount++ = colorKey.Value;
                        Color color = new Color();
                        int currentColorIndex = 0;
                        byte* currentBuffer = bufferFixed + bufferIndex;
                        while (currentColorCount != colorCountFixed)
                        {
                            color.Value = *--currentColorCount;
                            *currentBuffer++ = color.Red;
                            *currentBuffer++ = color.Blue;
                            *currentBuffer++ = color.Green;
                            colorIndexs.Set(color, currentColorIndex++);
                        }
                        *(bufferFixed + bufferIndex + (maxColorCount << 1) + maxColorCount) = (byte)pixel;
                        if (!checkBuffer(bufferFixed, (maxColorCount << 1) + maxColorCount + 1)) return false;
                    }
                }
                else
                {
                    int indexCount = colorIndexs.Count;
                    UnmanagedPool pool = UnmanagedPool.GetDefaultPool(indexCount * sizeof(IntSortIndex));
                    Pointer.Size sizeBuffer = pool.GetSize(indexCount * (sizeof(IntSortIndex) + sizeof(int)));
                    int* buffer = sizeBuffer.Int;
                    try
                    {
                        IntSortIndex* indexFixed = (IntSortIndex*)(buffer + indexCount), currentSortIndex = indexFixed;
                        foreach (KeyValue<Color, int> colorIndex in colorIndexs.KeyValues)
                        {
                            int color0 = colorIndex.Key.Value;
                            int color3 = ((color0 >> 3) & 0x111111) * 0x1020400;
                            int color2 = ((color0 >> 2) & 0x111111) * 0x1020400;
                            int color1 = ((color0 >> 1) & 0x111111) * 0x1020400;
                            color0 = (color0 & 0x111111) * 0x1020400;
                            (*currentSortIndex++).Set((color3 & 0x70000000) | ((color2 >> 4) & 0x7000000)
                                | ((color1 >> 8) & 0x700000) | ((color0 >> 12) & 0x70000) | ((color3 >> 12) & 0x7000)
                                | ((color2 >> 16) & 0x700) | ((color1 >> 20) & 0x70) | ((color0 >> 24) & 7), colorIndex.Value);
                        }
                        AutoCSer.Algorithm.FixedArrayQuickSort.sort(indexFixed, indexFixed + indexCount - 1);
                        int* currentSortArray;
                        if (maxColorCount != 2)
                        {
                            currentSortArray = buffer;
                            for (int currentColorCode, lastColorCode = (*--currentSortIndex).Value; currentSortIndex != indexFixed; lastColorCode = currentColorCode)
                            {
                                currentColorCode = (*--currentSortIndex).Value;
                                *currentSortArray++ = lastColorCode - currentColorCode;
                            }
                            currentSortArray = buffer + (maxColorCount >> 1) - 2;
                            new AutoCSer.Algorithm.FixedArrayQuickRangeSort.IntRangeSorterDesc { SkipCount = currentSortArray, GetEndIndex = currentSortArray }.Sort(buffer, buffer + indexCount - 2);
                            int minColorDifference = *currentSortArray, minColorDifferenceCount = 1;
                            while (currentSortArray != buffer)
                            {
                                if (*--currentSortArray == minColorDifference) ++minColorDifferenceCount;
                            }
                            currentSortIndex = indexFixed + indexCount;
                            int maxCountIndex = (*--currentSortIndex).Index, maxCount = *(colorCountFixed + maxCountIndex);
                            for (int currentColorCode, lastColorCode = (*currentSortIndex).Value; currentSortIndex != indexFixed; lastColorCode = currentColorCode)
                            {
                                currentColorCode = (*--currentSortIndex).Value;
                                int colorDifference = lastColorCode - currentColorCode;
                                if (colorDifference >= minColorDifference)
                                {
                                    if (colorDifference == minColorDifference && --minColorDifferenceCount == 0) ++minColorDifference;
                                    *(colorCountFixed + maxCountIndex) = int.MaxValue;
                                    maxCount = *(colorCountFixed + (maxCountIndex = (*currentSortIndex).Index));
                                }
                                else
                                {
                                    int countIndex = (*currentSortIndex).Index, count = *(colorCountFixed + countIndex);
                                    if (count > maxCount)
                                    {
                                        maxCountIndex = countIndex;
                                        maxCount = count;
                                    }
                                }
                            }
                            *(colorCountFixed + maxCountIndex) = int.MaxValue;
                        }
                        for (currentSortArray = buffer + indexCount, currentSortIndex = indexFixed; currentSortArray != buffer; *(--currentSortArray) = *(colorCountFixed + (*currentSortIndex++).Index)) ;
                        currentSortArray = buffer + maxColorCount - 1;
                        new AutoCSer.Algorithm.FixedArrayQuickRangeSort.IntRangeSorterDesc { SkipCount = currentSortArray, GetEndIndex = currentSortArray }.Sort(buffer, buffer + indexCount - 1);
                        int minColorCount = *currentSortArray, minColorCounts = 1;
                        while (currentSortArray != buffer)
                        {
                            if (*--currentSortArray == minColorCount) ++minColorCounts;
                        }
                        fixed (byte* fileBufferFixed = fileBuffer)
                        {
                            byte* currentBuffer = fileBufferFixed + bufferIndex;
                            IntSortIndex* lastSortIndex = indexFixed, endSortIndex = indexFixed + indexCount;
                            while (*(colorCountFixed + (*lastSortIndex).Index) < minColorCount) colorIndexs.Set(*(colorFixed + (*lastSortIndex++).Index), 0);
                            if (*(colorCountFixed + (*lastSortIndex).Index) == minColorCount && --minColorCounts == 0) ++minColorCount;
                            Color outputColor = *(colorFixed + (*lastSortIndex).Index);
                            *currentBuffer++ = outputColor.Red;
                            *currentBuffer++ = outputColor.Blue;
                            *currentBuffer++ = outputColor.Green;
                            colorIndexs.Set(outputColor, 0);
                            for (--maxColorCount; *(colorCountFixed + (*--endSortIndex).Index) < minColorCount; colorIndexs.Set(*(colorFixed + (*endSortIndex).Index), maxColorCount)) ;
                            if (*(colorCountFixed + (*endSortIndex).Index) == minColorCount && --minColorCounts == 0) ++minColorCount;
                            colorIndexs.Set(*(colorFixed + (*endSortIndex).Index), maxColorCount++);
                            int currentColorIndex = 0;
                            for (int* lastColorCount = colorCountFixed + (*endSortIndex).Index; lastSortIndex != endSortIndex; )
                            {
                                for (*lastColorCount = 0; *(colorCountFixed + (*++lastSortIndex).Index) >= minColorCount; colorIndexs.Set(outputColor, ++currentColorIndex))
                                {
                                    if (*(colorCountFixed + (*lastSortIndex).Index) == minColorCount && --minColorCounts == 0) ++minColorCount;
                                    outputColor = *(colorFixed + (*lastSortIndex).Index);
                                    *currentBuffer++ = outputColor.Red;
                                    *currentBuffer++ = outputColor.Blue;
                                    *currentBuffer++ = outputColor.Green;
                                }
                                if (lastSortIndex == endSortIndex) break;
                                *lastColorCount = int.MaxValue;
                                IntSortIndex* nextSortIndex = lastSortIndex;
                                while (*(colorCountFixed + (*++nextSortIndex).Index) < minColorCount) ;
                                for (int lastColorCode = (*(lastSortIndex - 1)).Value, nextColorCode = (*nextSortIndex).Value; lastSortIndex != nextSortIndex; ++lastSortIndex)
                                {
                                    colorIndexs.Set(*(colorFixed + (*lastSortIndex).Index), (*lastSortIndex).Value - lastColorCode <= nextColorCode - (*lastSortIndex).Value ? currentColorIndex : (currentColorIndex + 1));
                                }
                                if (lastSortIndex != endSortIndex)
                                {
                                    if (*(colorCountFixed + (*lastSortIndex).Index) == minColorCount && --minColorCounts == 0) ++minColorCount;
                                    outputColor = *(colorFixed + (*lastSortIndex).Index);
                                    *currentBuffer++ = outputColor.Red;
                                    *currentBuffer++ = outputColor.Blue;
                                    *currentBuffer++ = outputColor.Green;
                                    colorIndexs.Set(outputColor, ++currentColorIndex);
                                }
                            }
                            outputColor = *(colorFixed + (*lastSortIndex).Index);
                            *currentBuffer++ = outputColor.Red;
                            *currentBuffer++ = outputColor.Blue;
                            *currentBuffer++ = outputColor.Green;
                            *currentBuffer = (byte)pixel;
                            if (!checkBuffer(fileBufferFixed, (maxColorCount << 1) + maxColorCount + 1)) return false;
                        }
                    }
                    finally { pool.Push(ref sizeBuffer); }
                }
                byte* colorIndexFixed = (byte*)colorCountFixed;
                if (isInterlace)
                {
                    Color* colorEnd = colorFixed + width * height;
                    int inputSpace = (width << 3) - width;
                    for (Color* inputColor = colorFixed; inputColor < colorEnd; inputColor += inputSpace)
                    {
                        for (Color* inputEnd = inputColor + width; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[*inputColor++]) ;
                    }
                    for (Color* inputColor = colorFixed + (width << 2); inputColor < colorEnd; inputColor += inputSpace)
                    {
                        for (Color* inputEnd = inputColor + width; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[*inputColor++]) ;
                    }
                    inputSpace -= width << 2;
                    for (Color* inputColor = colorFixed + (width << 1); inputColor < colorEnd; inputColor += inputSpace)
                    {
                        for (Color* inputEnd = inputColor + width; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[*inputColor++]) ;
                    }
                    for (Color* inputColor = colorFixed + width; inputColor < colorEnd; inputColor += width)
                    {
                        for (Color* inputEnd = inputColor + width; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[*inputColor++]) ;
                    }
                }
                else
                {
                    for (Color* inputColor = colorFixed, inputEnd = colorFixed + width * height; inputColor != inputEnd; *colorIndexFixed++ = (byte)colorIndexs[*inputColor++]) ;
                }
                return lzwEncode((byte*)colorCountFixed, colorIndexFixed, pixel);
            }
        }
        /// <summary>
        /// LZW压缩编码
        /// </summary>
        /// <param name="inputFixed">输入数据</param>
        /// <param name="outputFixed">输出数据缓冲</param>
        /// <param name="size">编码长度</param>
        /// <returns>LZW压缩编码输出是否成功</returns>
        private unsafe bool lzwEncode(byte* inputFixed, byte* outputFixed, int size)
        {
            byte* lzwEncodeTable = LzwEncodeTableBufferPool.Get();
            try
            {
                ulong tableClearIndex = (ulong)1 << size, outputValue = tableClearIndex;
                byte* currentOutput = outputFixed;
                int tableSize = (int)size + 1;
                short clearIndex = (short)tableClearIndex, nextIndex = clearIndex;
                tableClearIndex |= tableClearIndex << 16;
                tableClearIndex |= tableClearIndex << 32;
                AutoCSer.Memory.Fill((ulong*)lzwEncodeTable, tableClearIndex, ((4096 * 2) / sizeof(ulong)) << size);
                int outputSize = tableSize;
                if (size == 1) ++outputSize;
                int outputStart = outputSize, nextClearIndex = 1 << outputSize;
                nextIndex += 2;
                short prefixIndex = *inputFixed;
                for (byte* currentInput = inputFixed; ++currentInput != outputFixed; )
                {
                    byte* currentTable = lzwEncodeTable + (prefixIndex << tableSize) + (*currentInput << 1);
                    if (*(short*)currentTable == clearIndex)
                    {
                        outputValue |= (ulong)(uint)(int)prefixIndex << outputStart;
                        if ((outputStart += outputSize) >= sizeof(ulong) << 3)
                        {
                            *(ulong*)currentOutput = outputValue;
                            outputStart -= sizeof(ulong) << 3;
                            currentOutput += sizeof(ulong);
                            outputValue = (uint)(int)prefixIndex >> (outputSize - outputStart);
                        }
                        if (nextIndex == nextClearIndex)
                        {
                            *(short*)currentTable = nextIndex++;
                            ++outputSize;
                            nextClearIndex <<= 1;
                        }
                        else if (nextIndex == 4095)
                        {
                            outputValue |= (ulong)(uint)(int)clearIndex << outputStart;
                            if ((outputStart += 12) >= sizeof(ulong) << 3)
                            {
                                *(ulong*)currentOutput = outputValue;
                                outputStart -= sizeof(ulong) << 3;
                                currentOutput += sizeof(ulong);
                                outputValue = (uint)(int)clearIndex >> (12 - outputStart);
                            }
                            AutoCSer.Memory.Fill((ulong*)lzwEncodeTable, tableClearIndex, ((4096 * 2) / sizeof(ulong)) << size);
                            outputSize = tableSize;
                            if (size == 1) ++outputSize;
                            nextClearIndex = 1 << outputSize;
                            nextIndex = clearIndex;
                            nextIndex += 2;
                        }
                        else *(short*)currentTable = nextIndex++;
                        prefixIndex = *currentInput;
                    }
                    else prefixIndex = *(short*)currentTable;
                }
                outputValue |= (ulong)(uint)(int)prefixIndex << outputStart;
                if ((outputStart += outputSize) >= sizeof(ulong) << 3)
                {
                    *(ulong*)currentOutput = outputValue;
                    outputStart -= sizeof(ulong) << 3;
                    currentOutput += sizeof(ulong);
                    outputValue = (uint)(int)prefixIndex >> (outputSize - outputStart);
                }
                outputValue |= (ulong)(uint)(int)++clearIndex << outputStart;
                if ((outputStart += outputSize) >= sizeof(ulong) << 3)
                {
                    *(ulong*)currentOutput = outputValue;
                    outputStart -= sizeof(ulong) << 3;
                    currentOutput += sizeof(ulong);
                    outputValue = (uint)(int)clearIndex >> (outputSize - outputStart);
                }
                if (outputStart != 0)
                {
                    *(ulong*)currentOutput = outputValue;
                    currentOutput += (outputStart + 7) >> 3;
                }
                fixed (byte* bufferFixed = fileBuffer) return addBlocks(bufferFixed, outputFixed, currentOutput);
            }
            finally { LzwEncodeTableBufferPool.Push(lzwEncodeTable); }
        }
        /// <summary>
        /// 添加数据块
        /// </summary>
        /// <param name="bufferFixed">文件缓存</param>
        /// <param name="outputFixed">输出数据起始位置</param>
        /// <param name="outputEnd">输出数据结束位置</param>
        /// <returns>数据块添加是否成功</returns>
        private unsafe bool addBlocks(byte* bufferFixed, byte* outputFixed, byte* outputEnd)
        {
            for (outputEnd -= 255 * 3; outputFixed <= outputEnd; outputFixed += 255 * 3)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *currentBuffer = 255;
                AutoCSer.Memory.CopyNotNull(outputFixed, currentBuffer + 1, 255);
                *(currentBuffer + 256) = 255;
                AutoCSer.Memory.CopyNotNull(outputFixed + 255, currentBuffer + 257, 255);
                *(currentBuffer + 512) = 255;
                AutoCSer.Memory.CopyNotNull(outputFixed + 255 * 2, currentBuffer + 513, 255);
                if (!checkBuffer(bufferFixed, 256 * 3)) return false;
            }
            for (outputEnd += 255 * 2; outputFixed <= outputEnd; outputFixed += 255)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *currentBuffer = 255;
                AutoCSer.Memory.CopyNotNull(outputFixed, currentBuffer + 1, 255);
                bufferIndex += 256;
            }
            int outputLength = (int)(outputEnd + 255 - outputFixed);
            if (outputLength != 0)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *currentBuffer = (byte)outputLength;
                AutoCSer.Memory.CopyNotNull(outputFixed, currentBuffer + 1, outputLength);
                bufferIndex += outputLength + 1;
            }
            *(bufferFixed + bufferIndex++) = 0;
            return checkBuffer(bufferFixed);
        }
        /// <summary>
        /// 添加数据块
        /// </summary>
        /// <param name="bufferFixed">文件缓存</param>
        /// <param name="text">文本数据</param>
        /// <returns>数据块添加是否成功</returns>
        private unsafe bool addBlocks(byte* bufferFixed, string text)
        {
            fixed (char* textFixed = text)
            {
                char* outputFixed = textFixed, outputEnd = outputFixed + text.Length - 255;
                while (outputFixed <= outputEnd)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    *currentBuffer = 255;
                    for (char* nextOutput = outputFixed + 255; outputFixed != nextOutput; ++outputFixed)
                    {
                        *++currentBuffer = *(byte*)outputFixed;
                    }
                    if (!checkBuffer(bufferFixed, 256)) return false;
                }
                int outputLength = (int)((outputEnd += 255) - outputFixed);
                if (outputLength != 0)
                {
                    byte* currentBuffer = bufferFixed + bufferIndex;
                    for (*currentBuffer = (byte)outputLength; outputFixed != outputEnd; ++outputFixed)
                    {
                        *++currentBuffer = *(byte*)outputFixed;
                    }
                    bufferIndex += outputLength + 1;
                }
                *(bufferFixed + bufferIndex++) = 0;
                return checkBuffer(bufferFixed);
            }
        }
        /// <summary>
        /// 添加图形控制扩展
        /// </summary>
        /// <param name="delayTime">延迟时间，单位1/100秒</param>
        /// <param name="method">图形处置方法</param>
        /// <param name="isUseInput">用户输入标志，指出是否期待用户有输入之后才继续进行下去，置位表示期待，值否表示不期待。</param>
        /// <returns>图形控制扩展是否添加成功</returns>
        public unsafe bool AddGraphicControl(short delayTime, PraphicControlMethodType method = PraphicControlMethodType.None
            , bool isUseInput = false)
        {
            if (fileBuffer == null) return false;
            if (delayTime <= 0) delayTime = 1;
            fixed (byte* bufferFixed = fileBuffer)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *(int*)currentBuffer = 0x4f921 | ((int)method << 26) | (isUseInput ? (0x2000000) : 0);
                *(int*)(currentBuffer + 4) = delayTime <= 0 ? 1 : (int)delayTime;
                return checkBuffer(bufferFixed, 8);
            }
        }
        /// <summary>
        /// 添加图形文本扩展
        /// </summary>
        /// <param name="text">文本数据</param>
        /// <param name="left">文本框离逻辑屏幕的左边界距离</param>
        /// <param name="top">文本框离逻辑屏幕的上边界距离</param>
        /// <param name="width">文本框像素宽度</param>
        /// <param name="height">文本框像素高度</param>
        /// <param name="colorIndex">前景色在全局颜色列表中的索引</param>
        /// <param name="blackgroundColorIndex">背景色在全局颜色列表中的索引</param>
        /// <param name="characterWidth">字符宽度</param>
        /// <param name="characterHeight">字符高度</param>
        /// <returns>图形文本扩展是否添加成功</returns>
        public unsafe bool AddPlainText(string text, short left, short top, short width, short height
            , byte colorIndex, byte blackgroundColorIndex, byte characterWidth, byte characterHeight)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (left + width <= Width || left >= Width || top >= Height || top + height <= Height) return false;
            if (colorIndex >= globalColorCount || blackgroundColorIndex >= globalColorCount) return false;
            if (characterWidth == 0 || characterHeight == 0) return false;
            fixed (byte* bufferFixed = fileBuffer)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *(short*)currentBuffer = 0x121;
                *(currentBuffer + 2) = 12;
                *(short*)(currentBuffer + 3) = left;
                *(short*)(currentBuffer + 5) = top;
                *(short*)(currentBuffer + 7) = width;
                *(short*)(currentBuffer + 9) = height;
                *(currentBuffer + 11) = characterWidth;
                *(currentBuffer + 12) = characterHeight;
                *(currentBuffer + 13) = colorIndex;
                *(currentBuffer + 14) = blackgroundColorIndex;
                return checkBuffer(bufferFixed, 15) && addBlocks(bufferFixed, text);
            }
        }
        /// <summary>
        /// 添加注释扩展
        /// </summary>
        /// <param name="comment">注释内容</param>
        /// <returns>注释扩展是否添加成功</returns>
        public unsafe bool AddComment(byte[] comment)
        {
            if (fileBuffer == null || comment.length() == 0) return false;
            fixed (byte* bufferFixed = fileBuffer)
            {
                *(ushort*)(bufferFixed + bufferIndex) = 0xfe21;
                if (!checkBuffer(bufferFixed, 2)) return false;
                fixed (byte* commentFixed = comment) return addBlocks(bufferFixed, commentFixed, commentFixed + comment.Length);
            }
        }
        /// <summary>
        /// 添加注释扩展
        /// </summary>
        /// <param name="comment">注释内容</param>
        /// <returns>注释扩展是否添加成功</returns>
        public unsafe bool AddComment(string comment)
        {
            if (fileBuffer == null || string.IsNullOrEmpty(comment)) return false;
            fixed (byte* bufferFixed = fileBuffer)
            {
                *(ushort*)(bufferFixed + bufferIndex) = 0xfe21;
                return checkBuffer(bufferFixed, 2) && addBlocks(bufferFixed, comment);
            }
        }
        /// <summary>
        /// 添加应用程序扩展
        /// </summary>
        /// <param name="identifier">用来鉴别应用程序自身的标识(8个连续ASCII字符)</param>
        /// <param name="authenticationCode">应用程序定义的特殊标识码(3个连续ASCII字符)</param>
        /// <param name="customData">应用程序自定义数据块集合</param>
        /// <returns>应用程序扩展是否添加成功</returns>
        public unsafe bool AddApplication(byte[] identifier, byte[] authenticationCode, byte[] customData)
        {
            if (((identifier.length() ^ 8) | (authenticationCode.length() ^ 3)) != 0) return false;
            fixed (byte* bufferFixed = fileBuffer)
            {
                byte* currentBuffer = bufferFixed + bufferIndex;
                *(ushort*)currentBuffer = 0xff21;
                *(currentBuffer + 2) = 11;
                fixed (byte* identifierFixed = identifier) *(ulong*)(currentBuffer + 3) = *(ulong*)identifierFixed;
                fixed (byte* authenticationCodeFixed = authenticationCode) *(int*)(currentBuffer + 11) = *(int*)authenticationCodeFixed;
                if (customData.length() == 0)
                {
                    *(currentBuffer + 14) = 0;
                    return checkBuffer(bufferFixed, 15);
                }
                else
                {
                    if (!checkBuffer(bufferFixed, 14)) return false;
                    fixed (byte* customDataFixed = customData) return addBlocks(bufferFixed, customDataFixed, customDataFixed + customData.Length);
                }
            }
        }
    }
}
