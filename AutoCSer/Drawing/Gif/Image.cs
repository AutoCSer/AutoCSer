using System;
using System.Drawing;
using System.Drawing.Imaging;
using AutoCSer.Extension;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 图象块
    /// </summary>
    public sealed class Image
    {
        /// <summary>
        /// X方向偏移量
        /// </summary>
        public short LeftOffset { get; private set; }
        /// <summary>
        /// Y方向偏移量
        /// </summary>
        public short TopOffset { get; private set; }
        /// <summary>
        /// 图象宽度
        /// </summary>
        public short Width { get; private set; }
        /// <summary>
        /// 图象高度
        /// </summary>
        public short Height { get; private set; }
        /// <summary>
        /// 颜色列表
        /// </summary>
        public Color[] Colors { get; private set; }
        /// <summary>
        /// 图象数据是否连续方式排列，否则使用顺序排列
        /// </summary>
        public byte InterlaceFlag { get; private set; }
        /// <summary>
        /// 颜色列表是否分类排列
        /// </summary>
        public byte SortFlag { get; private set; }
        /// <summary>
        /// LZW编码初始码表大小的位数
        /// </summary>
        public byte LzwSize { get; internal set; }
        /// <summary>
        /// 压缩数据集合
        /// </summary>
        internal LeftArray<SubArray<byte>> LzwDatas;
        /// <summary>
        /// 创建位图
        /// </summary>
        /// <param name="globalColors">全局颜色列表</param>
        /// <returns>位图,失败返回null</returns>
        public unsafe Bitmap CreateBitmap(Color[] globalColors)
        {
            if (Width == 0 || Height == 0 || LzwSize == 0 || LzwSize > 8) return null;
            int colorSize = Width * Height;
            Pointer.Size colorIndexs = Writer.LzwEncodeTableBufferPool.GetSize(colorSize);
            try
            {
                int length = lzwDecode(Decoder.BlocksToByte(ref LzwDatas), colorIndexs.Byte, LzwSize);
                if (length == colorSize)
                {
                    Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                    try
                    {
                        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                        byte* bitmapFixed = (byte*)bitmapData.Scan0;
                        int bitMapSpace = bitmapData.Stride - (Width << 1) - Width;
                        if (globalColors == null) globalColors = Colors;
                        if (globalColors != null)
                        {
                            fixed (Color* colorFixed = globalColors)
                            {
                                ImageFillBitmap fillBitmap = new ImageFillBitmap { CurrentIndex = colorIndexs.Byte, Colors = colorFixed, Width = Width };
                                if (InterlaceFlag == 0) fillBitmap.FillColor(Height, bitmapFixed, bitMapSpace);
                                else
                                {
                                    int bitmapStride = bitMapSpace + (bitmapData.Stride << 3) - bitmapData.Stride;
                                    fillBitmap.FillColor((Height + 7) >> 3, bitmapFixed, bitmapStride);
                                    fillBitmap.FillColor((Height + 3) >> 3, bitmapFixed + (bitmapData.Stride << 2), bitmapStride);
                                    fillBitmap.FillColor((Height + 1) >> 2, bitmapFixed + (bitmapData.Stride << 1), bitmapStride -= bitmapData.Stride << 2);
                                    fillBitmap.FillColor(Height >> 1, bitmapFixed + bitmapData.Stride, bitmapStride - (bitmapData.Stride << 1));
                                }
                            }
                        }
                        else
                        {
                            ImageFillBitmap fillBitmap = new ImageFillBitmap { CurrentIndex = colorIndexs.Byte, Width = Width };
                            if (InterlaceFlag == 0) fillBitmap.FillIndex(Height, bitmapFixed, bitMapSpace);
                            else
                            {
                                int bitmapStride = bitMapSpace + (bitmapData.Stride << 3) - bitmapData.Stride;
                                fillBitmap.FillIndex((Height + 7) >> 3, bitmapFixed, bitmapStride);
                                fillBitmap.FillIndex((Height + 3) >> 3, bitmapFixed + (bitmapData.Stride << 2), bitmapStride);
                                fillBitmap.FillIndex((Height + 1) >> 2, bitmapFixed + (bitmapData.Stride << 1), bitmapStride -= bitmapData.Stride << 2);
                                fillBitmap.FillIndex(Height >> 1, bitmapFixed + bitmapData.Stride, bitmapStride - (bitmapData.Stride << 1));
                            }
                        }
                        bitmap.UnlockBits(bitmapData);
                        return bitmap;
                    }
                    catch (Exception error)
                    {
                        bitmap.Dispose();
                        AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
                    }
                }
            }
            finally { Writer.LzwEncodeTableBufferPool.Push(ref colorIndexs); }
            return null;
        }
        /// <summary>
        /// 图象标识符设置
        /// </summary>
        /// <param name="data">当前解析数据</param>
        internal unsafe void SetDescriptor(byte* data)
        {
            LeftOffset = *(short*)data;
            TopOffset = *(short*)(data + 2);
            Width = *(short*)(data + 4);
            Height = *(short*)(data + 6);
            byte localFlag = *(data + 8);
            InterlaceFlag = (byte)(localFlag & 0x40);
            SortFlag = (byte)(localFlag & 0x20);
            if ((localFlag & 0x80) != 0) Colors = new Color[1 << ((localFlag & 7) + 1)];
        }
        /// <summary>
        /// LZW压缩解码字符串缓冲区
        /// </summary>
        private static readonly SubBuffer.Pool stringBufferPool = SubBuffer.Pool.GetPool(SubBuffer.Size.Kilobyte32);//4097 * 8
        /// <summary>
        /// LZW压缩解码
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <param name="output">输出数据缓冲</param>
        /// <param name="size">编码长度</param>
        /// <returns>解码数据长度,失败返回-1</returns>
        private unsafe static int lzwDecode(byte[] input, byte* output, byte size)
        {
            int tableSize = (int)size + 1;
            short clearIndex = (short)(1 << size), nextIndex = clearIndex;
            SubBuffer.PoolBufferFull stringBuffer = default(SubBuffer.PoolBufferFull);
            stringBufferPool.Get(ref stringBuffer);
            try
            {
                fixed (byte* inputFixed = input, stringFixed = stringBuffer.Buffer)
                {
                    byte* nextStrings = null, stringStart = stringFixed + stringBuffer.StartIndex;
                    byte* currentInput = inputFixed, inputEnd = inputFixed + input.Length;
                    byte* currentOutput = output, outputEnd = output + Writer.LzwEncodeTableBufferPool.Size;
                    int valueBits = 0, inputSize = 0, inputOffset = (int)inputEnd & (sizeof(ulong) - 1), startSize = tableSize;
                    ulong inputValue = 0, inputMark = ushort.MaxValue, startMark = ((ulong)1UL << startSize) - 1;
                    short endIndex = (short)(clearIndex + 1), prefixIndex, currentIndex = 0;
                    if (inputOffset == 0)
                    {
                        inputEnd -= sizeof(ulong);
                        inputOffset = sizeof(ulong);
                    }
                    else inputEnd -= inputOffset;
                    if (size == 1) ++startSize;
                    while (currentIndex != endIndex)
                    {
                        if (valueBits >= startSize)
                        {
                            prefixIndex = (short)(inputValue & startMark);
                            valueBits -= startSize;
                            inputValue >>= startSize;
                        }
                        else
                        {
                            if (currentInput > inputEnd) return -1;
                            ulong nextValue = *(ulong*)currentInput;
                            prefixIndex = (short)((inputValue | (nextValue << valueBits)) & startMark);
                            inputValue = nextValue >> -(valueBits -= startSize);
                            valueBits += sizeof(ulong) << 3;
                            if (currentInput == inputEnd && (valueBits -= (sizeof(ulong) - inputOffset) << 3) < 0) return -1;
                            currentInput += sizeof(ulong);
                        }
                        if (prefixIndex == clearIndex) continue;
                        if (prefixIndex == endIndex) break;
                        if (currentOutput == outputEnd) return -1;

                        AutoCSer.Memory.ClearUnsafe((ulong*)stringStart, 4097);
                        inputSize = startSize;
                        inputMark = startMark;
                        nextIndex = (short)(endIndex + 1);
                        *(short*)(nextStrings = stringStart + (nextIndex << 3)) = prefixIndex;
                        *(short*)(nextStrings + 2) = prefixIndex;
                        *(int*)(nextStrings + 4) = 2;
                        *currentOutput++ = (byte)prefixIndex;
                        do
                        {
                            if (valueBits >= inputSize)
                            {
                                currentIndex = (short)(inputValue & inputMark);
                                valueBits -= inputSize;
                                inputValue >>= inputSize;
                            }
                            else
                            {
                                if (currentInput > inputEnd) return -1;
                                ulong nextValue = *(ulong*)currentInput;
                                currentIndex = (short)((inputValue | (nextValue << valueBits)) & inputMark);
                                inputValue = nextValue >> -(valueBits -= inputSize);
                                valueBits += sizeof(ulong) << 3;
                                if (currentInput == inputEnd && (valueBits -= (sizeof(ulong) - inputOffset) << 3) < 0) return -1;
                                currentInput += sizeof(ulong);
                            }
                            *(short*)(nextStrings += 8) = currentIndex;
                            if (currentIndex < clearIndex)
                            {
                                if (currentOutput == outputEnd) return -1;
                                *(short*)(nextStrings + 2) = currentIndex;
                                *(int*)(nextStrings + 4) = 2;
                                *currentOutput++ = (byte)currentIndex;
                            }
                            else if (currentIndex > endIndex)
                            {
                                byte* currentString = stringStart + (currentIndex << 3);
                                int outputCount = *(int*)(currentString + 4);
                                if (outputCount == 0) return -1;
                                *(short*)(nextStrings + 2) = *(short*)(currentString + 2);
                                *(int*)(nextStrings + 4) = outputCount + 1;
                                if ((currentOutput += outputCount) > outputEnd) return -1;
                                do
                                {
                                    *--currentOutput = *(currentString + 2 + 8);
                                    prefixIndex = *(short*)currentString;
                                    if (prefixIndex < clearIndex) break;
                                    currentString = stringStart + (prefixIndex << 3);
                                }
                                while (true);
                                *--currentOutput = (byte)prefixIndex;
                                currentOutput += outputCount;
                            }
                            else break;
                            prefixIndex = currentIndex;
                            if (nextIndex++ == (short)inputMark)
                            {
                                if (inputSize == 12) return -1;
                                inputMark <<= 1;
                                ++inputSize;
                                ++inputMark;
                            }
                        }
                        while (true);
                    }
                    return (int)(currentOutput - output);
                }
            }
            finally { stringBufferPool.Push(ref stringBuffer); }
        }
    }
}
