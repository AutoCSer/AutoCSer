using System;
using System.Diagnostics;
using System.Text;

namespace AutoCSer.Text
{
    /// <summary>
    /// 异常编码
    /// </summary>
    internal sealed class ExceptionEncoding : Encoding
    {
        /// <summary>
        /// 编码异常
        /// </summary>
        private readonly Exception exception;
        /// <summary>
        /// 异常编码
        /// </summary>
        /// <param name="exception">编码异常</param>
        internal ExceptionEncoding(Exception exception)
        {
            this.exception = exception;
        }
        /// <summary>
        /// 
        /// </summary>
        public override int CodePage { get { return int.MinValue; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int GetByteCount(char[] chars, int index, int count)
        {
            throw exception;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="charIndex"></param>
        /// <param name="charCount"></param>
        /// <param name="bytes"></param>
        /// <param name="byteIndex"></param>
        /// <returns></returns>
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            throw exception;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            throw exception;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="byteIndex"></param>
        /// <param name="byteCount"></param>
        /// <param name="chars"></param>
        /// <param name="charIndex"></param>
        /// <returns></returns>
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            throw exception;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public override int GetMaxByteCount(int charCount)
        {
            throw exception;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        public override int GetMaxCharCount(int byteCount)
        {
            throw exception;
        }

    }
}
