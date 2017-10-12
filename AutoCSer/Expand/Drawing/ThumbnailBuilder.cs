using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Drawing
{
    /// <summary>
    /// 缩略图创建器
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct ThumbnailBuilder
    {
        /// <summary>
        /// 原始图片
        /// </summary>
        private Image image;
        /// <summary>
        /// 原始图片宽度
        /// </summary>
        private int width;
        /// <summary>
        /// 原始图片高度
        /// </summary>
        private int height;
        /// <summary>
        /// 原始图片裁剪横坐标起始位置
        /// </summary>
        private int left;
        /// <summary>
        /// 原始图片裁剪纵坐标起始位置
        /// </summary>
        private int top;
        /// <summary>
        /// 原始图片裁剪横坐标结束位置
        /// </summary>
        private int right;
        /// <summary>
        /// 原始图片裁剪纵坐标结束位置
        /// </summary>
        private int bottom;
        /// <summary>
        /// 根据数据流创建原始图片
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <returns>原始图片</returns>
        public Image CreateImage(Stream stream)
        {
            image = Image.FromStream(stream);
            width = image.Width;
            height = image.Height;
            return image;
        }
        /// <summary>
        /// 计算缩略图尺寸位置
        /// </summary>
        /// <param name="width">缩略宽度,0表示与高度同比例</param>
        /// <param name="height">缩略高度,0表示与宽度同比例</param>
        /// <returns>是否需要生成缩略图</returns>
        private bool checkCut(ref int width, ref int height)
        {
            if (width > 0)
            {
                if (height > 0)
                {
                    if ((long)width * this.height >= (long)height * this.width)
                    {
                        int value = (int)((long)height * this.width / width);
                        if (width > this.width)
                        {
                            if (value == 0) value = 1;
                            width = this.width;
                        }
                        left = 0;
                        top = (this.height - value) >> 1;
                        right = this.width;
                        bottom = top + value;
                    }
                    else
                    {
                        int value = (int)((long)width * this.height / height);
                        if (height > this.height)
                        {
                            if (value == 0) value = 1;
                            height = this.height;
                        }
                        left = (this.width - value) >> 1;
                        top = 0;
                        right = left + value;
                        bottom = this.height;
                    }
                    return true;
                }
                if (width < this.width)
                {
                    left = top = 0;
                    right = this.width;
                    bottom = this.height;
                    if ((height = (int)((long)this.height * width / this.width)) == 0) height = 1;
                    return true;
                }
            }
            else if (height < this.height)
            {
                left = top = 0;
                right = this.width;
                bottom = this.height;
                if ((width = (int)((long)this.width * height / this.height)) == 0) width = 1;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="data">输出数据</param>
        /// <param name="width">缩略宽度</param>
        /// <param name="height">缩略高度</param>
        /// <param name="type">目标图像文件格式</param>
        /// <param name="seek">输出数据起始位置</param>
        public void Cut(ref SubArray<byte> data, ref int width, ref int height, ImageFormat type, int seek)
        {
            if (checkCut(ref width, ref height))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    if (seek != 0) stream.Seek(seek, SeekOrigin.Begin);
                    cut(stream, width, height, type);
                    data.Set(stream.GetBuffer(), seek, (int)stream.Position - seek);
                    return;
                }
            }
            data.SetNull();
        }
        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="stream">输出数据流</param>
        /// <param name="width">缩略宽度</param>
        /// <param name="height">缩略高度</param>
        /// <param name="type">目标图像文件格式</param>
        private void cut(Stream stream, int width, int height, ImageFormat type)
        {
            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics graphic = Graphics.FromImage(bitmap))
            {
                if (type != ImageFormat.Png) graphic.Clear(Color.White);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(left, top, right - left, bottom - top), GraphicsUnit.Pixel);
                bitmap.Save(stream, getImageCodec(type ?? ImageFormat.Jpeg) ?? jpegImageCodecInfo, qualityEncoder);
            }
        }
        /// <summary>
        /// 计算缩略图尺寸位置
        /// </summary>
        /// <param name="width">缩略宽度,0表示与高度同比例</param>
        /// <param name="height">缩略高度,0表示与宽度同比例</param>
        /// <returns>是否需要生成缩略图</returns>
        private bool checkPad(ref int width, ref int height)
        {
            if (width > 0)
            {
                if (height > 0)
                {
                    if ((long)width * this.height >= (long)height * this.width)
                    {
                        int value = (int)((long)this.width * height / this.height);
                        if (this.height > height)
                        {
                            if (value == 0) value = 1;
                            //height = this.height;
                        }
                        left = (width - value) >> 1;
                        top = 0;
                        right = left + value;
                        bottom = height;
                    }
                    else
                    {
                        int value = (int)((long)this.height * width / this.width);
                        if (this.width > width)
                        {
                            if (value == 0) value = 1;
                            //width = this.width;
                        }
                        left = 0;
                        top = (height - value) >> 1;
                        right = width;
                        bottom = top + value;
                    }
                    return true;
                }
                if (width < this.width)
                {
                    if ((height = (int)((long)this.height * width / this.width)) == 0) height = 1;
                    left = top = 0;
                    right = width;
                    bottom = height;
                    return true;
                }
            }
            else if (height < this.height)
            {
                if ((width = (int)((long)this.width * height / this.height)) == 0) width = 1;
                left = top = 0;
                right = width;
                bottom = height;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="data">输出数据</param>
        /// <param name="width">缩略宽度</param>
        /// <param name="height">缩略高度</param>
        /// <param name="type">目标图像文件格式</param>
        /// <param name="backColor">背景色</param>
        /// <param name="seek">输出数据起始位置</param>
        public void Pad(ref SubArray<byte> data, ref int width, ref int height, ImageFormat type, Color backColor, int seek)
        {
            if (checkPad(ref width, ref height))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    if (seek != 0) stream.Seek(seek, SeekOrigin.Begin);
                    pad(stream, width, height, type, backColor);
                    data.Set(stream.GetBuffer(), seek, (int)stream.Position - seek);
                    return;
                }
            }
            data.SetNull();
        }
        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="stream">输出数据流</param>
        /// <param name="width">缩略宽度</param>
        /// <param name="height">缩略高度</param>
        /// <param name="type">目标图像文件格式</param>
        /// <param name="backColor">背景色</param>
        private void pad(Stream stream, int width, int height, ImageFormat type, Color backColor)
        {
            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics graphic = Graphics.FromImage(bitmap))
            {
                if (!backColor.IsEmpty) graphic.Clear(backColor);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, new Rectangle(left, top, right - left, bottom - top), new Rectangle(0, 0, this.width, this.height), GraphicsUnit.Pixel);
                bitmap.Save(stream, getImageCodec(type ?? ImageFormat.Jpeg) ?? jpegImageCodecInfo, qualityEncoder);
            }
        }

        /// <summary>
        /// 高质量图像编码参数
        /// </summary>
        private static readonly EncoderParameters qualityEncoder;
        /// <summary>
        /// 图像编码解码器集合
        /// </summary>
        private static readonly AutoCSer.StateSearcher.ByteSearcher<ImageCodecInfo> imageCodecs;
        /// <summary>
        /// JPEG图像编码解码器
        /// </summary>
        private static readonly ImageCodecInfo jpegImageCodecInfo;
        /// <summary>
        /// 获取图像编码解码器
        /// </summary>
        /// <param name="format">图像文件格式</param>
        /// <returns>图像编码解码器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static ImageCodecInfo getImageCodec(ImageFormat format)
        {
            if (format != null)
            {
                AutoCSer.GuidCreator guid = new AutoCSer.GuidCreator { Value = format.Guid };
                return imageCodecs.Get(&guid, 16);
            }
            return null;
        }
        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <returns>字节数组</returns>
        private unsafe static byte[] toByteArray(Guid guid)
        {
            byte[] data = new byte[16];
            GuidCreator newGuid = new GuidCreator { Value = guid };
            fixed (byte* dataFixed = data)
            {
                *(ulong*)dataFixed = *(ulong*)&newGuid;
                *(ulong*)(dataFixed + sizeof(ulong)) = *(ulong*)((byte*)&newGuid + sizeof(ulong));
            }
            return data;
        }
        unsafe static ThumbnailBuilder()
        {
            (qualityEncoder = new EncoderParameters(1)).Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            ImageCodecInfo[] infos = ImageCodecInfo.GetImageDecoders();
            imageCodecs = new AutoCSer.StateSearcher.ByteSearcher<ImageCodecInfo>(infos.getArray(value => toByteArray(value.FormatID)), infos, true);
            GuidCreator guid = new GuidCreator { Value = ImageFormat.Jpeg.Guid };
            jpegImageCodecInfo = imageCodecs.Get(&guid, 16);
        }
    }
}
