using System;
using System.Threading;
using System.Drawing;
using AutoCSer.Extension;
using System.Drawing.Imaging;
using System.IO;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 定时获取图片生成 GIF 文件数据，一般用于截屏（比如 Graphics.CopyFromScreen）
    /// </summary>
    public sealed class TimerWriter : IDisposable
    {
        /// <summary>
        /// GIF 文件数据写入器
        /// </summary>
        private readonly Writer gif;
        /// <summary>
        /// 获取图片委托
        /// </summary>
        private readonly Func<Bitmap> getBitmap;
        /// <summary>
        /// GIF 文件处理结束等待锁
        /// </summary>
        private readonly AutoCSer.Threading.WaitHandle finallyWait;
        /// <summary>
        /// 位图等待锁
        /// </summary>
        private readonly AutoCSer.Threading.AutoWaitHandle bitmapWait;
        /// <summary>
        /// 未处理图片队列
        /// </summary>
        private readonly BitmapInfo.YieldQueue bitmapQueue;
        /// <summary>
        /// 截屏定时毫秒数
        /// </summary>
        private readonly double interval;

        /// <summary>
        /// 截屏定时器
        /// </summary>
        private System.Threading.Timer timer;
        /// <summary>
        /// 最大色彩深度
        /// </summary>
        private readonly byte maxPixel;
        /// <summary>
        /// 释放资源是否等待 GIF 文件处理结束
        /// </summary>
        private readonly bool isWaitFinally;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 跳图数量
        /// </summary>
        private int keepCount;
        /// <summary>
        /// 是否正在获取图片
        /// </summary>
        private int isTimer;
        /// <summary>
        /// 定时获取图片生成 GIF 文件
        /// </summary>
        /// <param name="stream">输出数据流</param>
        /// <param name="getBitmap">获取图片委托</param>
        /// <param name="width">图片最大高度</param>
        /// <param name="height">图片最大宽度</param>
        /// <param name="interval">截屏定时毫秒数</param>
        /// <param name="maxPixel">最大色彩深度</param>
        /// <param name="isWaitFinally">释放资源是否等待 GIF 文件处理结束</param>
        /// <param name="isLeaveDisposeStream">是否自动释放输出数据流</param>
        public TimerWriter(Stream stream, Func<Bitmap> getBitmap, int width, int height, int interval = 40, byte maxPixel = 8, bool isWaitFinally = true, bool isLeaveDisposeStream = false)
        {
            if (stream == null || getBitmap == null) throw new ArgumentNullException();
            if (width <= 0) throw new IndexOutOfRangeException("width");
            if (height <= 0) throw new IndexOutOfRangeException("height");
            this.getBitmap = getBitmap;
            this.maxPixel = (byte)(maxPixel - 2) < 8 ? maxPixel : (byte)8;
            bitmapQueue = new BitmapInfo.YieldQueue(new BitmapInfo());
            gif = new Writer(stream, (short)width, (short)height, null, 0, null, isLeaveDisposeStream);
            this.interval = interval < 40 ? 40 : interval;
            bitmapWait.Set(0);
            if (this.isWaitFinally = isWaitFinally) finallyWait.Set(0);
            timer = new System.Threading.Timer(onTimer, this, interval, interval);
            onTimer(null);
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(write);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            if (isWaitFinally) finallyWait.Wait();
        }
        /// <summary>
        /// 定时获取图片
        /// </summary>
        /// <param name="state"></param>
        private void onTimer(object state)
        {
            if (Interlocked.Exchange(ref isTimer, 1) == 0)
            {
                if (isDisposed)
                {
                    if (timer != null)
                    {
                        timer.Dispose();
                        timer = null;
                    }
                    if (bitmapQueue.IsPushHead(new BitmapInfo())) bitmapWait.Set();
                    return;
                }
                try
                {
                    Bitmap bitmap = getBitmap();
                    if (bitmap != null)
                    {
                        if (bitmapQueue.IsPushHead(new BitmapInfo { Bitmap = bitmap, KeepCount = Interlocked.Exchange(ref this.keepCount, 0) })) bitmapWait.Set();
                        return;
                    }
                }
                catch (Exception error)
                {
                    AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
                finally { isTimer = 0; }
            }
            Interlocked.Increment(ref keepCount);
        }
        /// <summary>
        /// GIF文件处理线程
        /// </summary>
        private unsafe void write()
        {
            Bitmap lastBitmap = null, currentBitmap = null;
            BitmapData lastBitmapData = null, currentBitmapData = null;
            try
            {
                double currentInterval = -interval;
                int keepCount;
                do
                {
                    bitmapWait.Wait();
                    BitmapInfo head = bitmapQueue.GetClear();
                    while (head != null)
                    {
                        currentInterval += interval;
                        head = head.Next(out currentBitmap, out keepCount);
                        if (currentBitmap == null) return;
                        int left = 0, top = 0, right = Math.Min(gif.Width, currentBitmap.Width), bottom = Math.Min(gif.Height, currentBitmap.Height);
                        currentBitmapData = currentBitmap.LockBits(new Rectangle(0, 0, right, bottom), System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        if (lastBitmapData != null)
                        {
                            byte* lastBitmapFixed = (byte*)lastBitmapData.Scan0, currentBitmapFixed = (byte*)currentBitmapData.Scan0;
                            int minHeight = currentBitmap.Height <= lastBitmap.Height ? currentBitmap.Height : lastBitmap.Height;
                            int minWidth = lastBitmap.Width, width3;
                            if (currentBitmap.Width <= lastBitmap.Width)
                            {
                                minWidth = currentBitmap.Width;
                                width3 = (minWidth << 1) + minWidth;
                                for (byte* lastRow = lastBitmapFixed, currentRow = currentBitmapFixed; top != minHeight; ++top)
                                {
                                    if (!AutoCSer.Memory.EqualNotNull(lastRow, currentRow, width3)) break;
                                    lastRow += lastBitmapData.Stride;
                                    currentRow += currentBitmapData.Stride;
                                }
                                if (currentBitmap.Height <= lastBitmap.Height && top != minHeight)
                                {
                                    ++top;
                                    for (byte* lastRow = lastBitmapFixed + lastBitmapData.Stride * minHeight, currentRow = currentBitmapFixed + currentBitmapData.Stride * minHeight; top != bottom; --bottom)
                                    {
                                        if (!AutoCSer.Memory.EqualNotNull(lastRow -= lastBitmapData.Stride, currentRow -= currentBitmapData.Stride, width3)) break;
                                    }
                                    --top;
                                }
                            }
                            if (currentBitmap.Height <= lastBitmap.Height && top != minHeight)
                            {
                                width3 = (minWidth << 1) + minWidth;
                                int endRowStride = lastBitmapData.Stride * (bottom - top);
                                byte* lastTopRow = lastBitmapFixed + lastBitmapData.Stride * top, currentTopRow = currentBitmapFixed + currentBitmapData.Stride * top;
                                if ((((int)lastBitmapFixed & (sizeof(ulong) - 1)) | (lastBitmapData.Stride & (sizeof(ulong) - 1)) | ((int)currentBitmapFixed & (sizeof(ulong) - 1)) | (currentBitmapData.Stride & (sizeof(ulong) - 1))) == 0)
                                {
                                    byte* lastTopCol = lastTopRow, topColEnd = lastTopRow + width3 - 1;
                                    ulong color = 0;
                                    for (byte* currentTopCol = currentTopRow; lastTopCol <= topColEnd; lastTopCol += sizeof(ulong), currentTopCol += sizeof(ulong))
                                    {
                                        color = 0;
                                        for (byte* lastRow = lastTopCol, currentRow = currentTopCol, endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += currentBitmapData.Stride)
                                        {
                                            color |= *(ulong*)lastRow ^ *(ulong*)currentRow;
                                        }
                                        if (color != 0) break;
                                    }
                                    int length = (int)(lastTopCol - lastTopRow);
                                    if (lastTopCol <= topColEnd) length += color.endBits() >> 3;
                                    left += (length /= 3) < minWidth ? length : minWidth;
                                    if (currentBitmap.Width <= lastBitmap.Width && left != minWidth)
                                    {
                                        int offset = width3 & (sizeof(ulong) - 1);
                                        byte* currentTopCol = currentTopRow + width3;
                                        lastTopCol = lastTopRow + width3;
                                        length = 0;
                                        if (offset != 0)
                                        {
                                            currentTopCol -= offset;
                                            lastTopCol -= offset;
                                            color = 0;
                                            for (byte* lastRow = lastTopCol, currentRow = currentTopCol, endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += currentBitmapData.Stride)
                                            {
                                                color |= *(ulong*)lastRow ^ *(ulong*)currentRow;
                                            }
                                            if ((color <<= ((sizeof(ulong) - offset) << 3)) == 0) length = offset;
                                            else length = ((sizeof(ulong) << 3) - color.bits()) >> 3;
                                        }
                                        if (length == offset)
                                        {
                                            topColEnd = lastTopCol;
                                            do
                                            {
                                                color = 0;
                                                for (byte* lastRow = (lastTopCol -= sizeof(ulong)), currentRow = (currentTopCol -= sizeof(ulong)), endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += currentBitmapData.Stride)
                                                {
                                                    color |= *(ulong*)lastRow ^ *(ulong*)currentRow;
                                                }
                                            }
                                            while (color == 0);
                                            length += (int)(topColEnd - lastTopCol) - sizeof(ulong) + ((sizeof(ulong) << 3) - color.bits()) >> 3;
                                        }
                                        right -= length / 3;
                                    }
                                }
                                else
                                {
                                    for (byte* lastTopCol = lastTopRow, topColEnd = lastTopRow + width3, currentTopCol = currentTopRow; lastTopCol != topColEnd; lastTopCol += 3, currentTopCol += 3, ++left)
                                    {
                                        int color = 0;
                                        for (byte* lastRow = lastTopCol, currentRow = currentTopCol, endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += currentBitmapData.Stride)
                                        {
                                            color |= *(int*)lastRow ^ *(int*)currentRow;
                                        }
                                        if ((color & 0xffffff) != 0) break;
                                    }
                                    if (currentBitmap.Width <= lastBitmap.Width && left != minWidth)
                                    {
                                        byte* lastTopCol = lastTopRow + width3, currentTopCol = currentTopRow + width3;
                                        do
                                        {
                                            int color = 0;
                                            for (byte* lastRow = (lastTopCol -= 3), currentRow = (currentTopCol -= 3), endRow = lastRow + endRowStride; lastRow != endRow; lastRow += lastBitmapData.Stride, currentRow += currentBitmapData.Stride)
                                            {
                                                color |= *(int*)lastRow ^ *(int*)currentRow;
                                            }
                                            if ((color & 0xffffff) != 0) break;
                                            --right;
                                        }
                                        while (true);
                                    }
                                }
                            }
                        }
                        if (top == bottom)
                        {
                            currentBitmap.UnlockBits(currentBitmapData);
                            currentBitmapData = null;
                            currentBitmap.Dispose();
                            currentBitmap = null;
                        }
                        else
                        {
                            int delayTime = (int)(currentInterval / 10);
                            for (currentInterval -= delayTime * 10; delayTime > short.MaxValue; delayTime -= short.MaxValue)
                            {
                                if (!gif.AddGraphicControl(short.MaxValue, PraphicControlMethodType.None, true)) break;
                                if (!gif.addImage(lastBitmapData, 0, 0, 0, 0, 1, 1, false, maxPixel)) break;
                            }
                            if (lastBitmap != null)
                            {
                                lastBitmap.UnlockBits(lastBitmapData);
                                lastBitmapData = null;
                                lastBitmap.Dispose();
                                lastBitmap = null;
                            }
                            if (delayTime != 0 && !gif.AddGraphicControl((short)delayTime, PraphicControlMethodType.None, true)) break;
                            if (!gif.addImage(currentBitmapData, left, top, left, top, right - left, bottom - top, false, maxPixel)) break;
                            lastBitmapData = currentBitmapData;
                            lastBitmap = currentBitmap;
                            currentBitmapData = null;
                            currentBitmap = null;
                        }
                        if (keepCount != 0) currentInterval += keepCount * interval;
                    }
                }
                while (true);
            }
            finally
            {
                gif.Dispose();
                if (isWaitFinally) finallyWait.Set();
                if (lastBitmap != null)
                {
                    if (lastBitmapData != null) lastBitmap.UnlockBits(lastBitmapData);
                    lastBitmap.Dispose();
                }
                if (currentBitmap != null)
                {
                    if (currentBitmapData != null) currentBitmap.UnlockBits(currentBitmapData);
                    currentBitmap.Dispose();
                }
                bitmapQueue.GetClear();
            }
        }
    }
}
