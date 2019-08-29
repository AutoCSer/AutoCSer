using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// 字符分词类型
    /// </summary>
    public unsafe struct CharWordType
    {
        /// <summary> 
        /// 分词字符类型数据
        /// </summary>
        internal byte* Data;
        /// <summary>
        /// 获取分词类型
        /// </summary>
        /// <param name="code">字符</param>
        /// <returns>分词类型</returns>
        public WordType this[char code]
        {
            get { return (WordType)Data[code]; }
        }
    }
}
