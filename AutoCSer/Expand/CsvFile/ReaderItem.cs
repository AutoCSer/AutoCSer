using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CsvFile
{
    /// <summary>
    /// 内容
    /// </summary>
    public struct ReaderItem
    {
        /// <summary>
        /// CSV 字符串读取器
        /// </summary>
        public Reader Reader;
        /// <summary>
        /// 当前行号
        /// </summary>
        public int Row;
        /// <summary>
        /// 当前列号
        /// </summary>
        public int Col;
        /// <summary>
        /// 起始字符位置
        /// </summary>
        public int Start;
        /// <summary>
        /// 结束字符位置
        /// </summary>
        public int End;
        /// <summary>
        /// 字符串长度
        /// </summary>
        public int Length
        {
            get { return End - Start; }
        }
        /// <summary>
        /// 是否存在内容，否则表示解析错误
        /// </summary>
        public bool IsItem
        {
            get { return End >= 0; }
        }
        /// <summary>
        /// 错误位置
        /// </summary>
        public int ErrorIndex
        {
            get { return -End; }
        }
        /// <summary>
        /// 字符串
        /// </summary>
        public string String
        {
            get
            {
                return End > Start ? Reader.GetString(ref this) : (Start == End ? string.Empty : null);
            }
        }
        /// <summary>
        /// 移动到下一列
        /// </summary>
        /// <param name="step"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void NextCol(ref ReaderStep step)
        {
            ++Col;
            Start = End + 1;
            step = ReaderStep.Start;
        }
        /// <summary>
        /// 移动到下一行
        /// </summary>
        /// <param name="step"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void NextIgnoreRow(ref ReaderStep step)
        {
            ++Row;
            Col = 0;
            Start = End + 2;
            step = ReaderStep.Start;
        }
        /// <summary>
        /// 移动到下一行
        /// </summary>
        /// <param name="step"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void NextRow(ref ReaderStep step)
        {
            ++Row;
            Col = 0;
            Start = End + 1;
            step = ReaderStep.Start;
        }
        /// <summary>
        /// 错误位置
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Error(int index)
        {
            End = -index;
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="ignorePrefix">忽略前缀</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetString(char ignorePrefix)
        {
            return End > Start ? Reader.GetString(ref this, ignorePrefix) : (Start == End ? string.Empty : null);
        }
    }
}
