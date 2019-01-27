using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.CsvFile
{
    /// <summary>
    /// CSV 解析数据对象
    /// </summary>
    public sealed class ObjectReader
    {
        /// <summary>
        /// CSV 字符串读取器
        /// </summary>
        private Reader reader;
        /// <summary>
        /// CSV 字符串读取器索引
        /// </summary>
        internal readonly Index Index;
        /// <summary>
        /// 数据集合
        /// </summary>
        internal readonly string[] ValueArray;
        /// <summary>
        /// 数据集合
        /// </summary>
        public string[] Values
        {
            get { return ValueArray; }
        }
        /// <summary>
        /// 当前记录起始位置
        /// </summary>
        private readonly int start;
        /// <summary>
        /// 当前记录结束位置
        /// </summary>
        private int end;
        /// <summary>
        /// 当前行原始字符串
        /// </summary>
        public string RowString
        {
            get
            {
                return reader.GetString(start, end);
            }
        }
        /// <summary>
        /// 当前记录行号
        /// </summary>
        public readonly int Row;
        /// <summary>
        /// 根据索引名称获取数据
        /// </summary>
        /// <param name="name">索引名称</param>
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                string value;
                if (TryGetValue(name, out value)) return value;
                throw new ArgumentOutOfRangeException(name);
            }
        }
        /// <summary>
        /// CSV 解析数据对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        internal ObjectReader(Index index, ref ReaderItem item)
        {
            Index = index;
            this.reader = item.Reader;
            start = item.Start;
            end = item.End;
            Row = item.Row;
            ValueArray = new string[Index.ColCount];
        }
        /// <summary>
        /// 根据索引名称获取数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out string value)
        {
            int index = Index.GetIndex(name);
            if (index >= 0)
            {
                value = ValueArray[index];
                return true;
            }
            value = null;
            return false;
        }
        /// <summary>
        /// 设置结束位置
        /// </summary>
        /// <param name="item"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetEnd(ref ReaderItem item)
        {
            if (item.Row == Row) end = item.End;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool GetValue(string Name, out string Value, ref string Message)
        {
            if (TryGetValue(Name, out Value)) return true;
            Message = "第 "+Row.toString()+" 行没有找到 {Name}";
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="OtherName"></param>
        /// <param name="Value"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool GetValue(string Name, string OtherName, out string Value, ref string Message)
        {
            if (TryGetValue(Name, out Value) || TryGetValue(OtherName, out Value)) return true;
            Message = "第 " + Row.toString() + " 行没有找到 " + Name + " / " + OtherName;
            return false;
        }
    }
}
