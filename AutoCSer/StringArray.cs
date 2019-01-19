using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 字符串连接(适应于较长的字符串链接,一个一个字符添加的请用unmanagedStream)
    /// </summary>
    public sealed class StringArray
    {
        /// <summary>
        /// 字符串数组
        /// </summary>
        private LeftArray<string> array;
        /// <summary>
        /// 字符串数量
        /// </summary>
        public int Length
        {
            get { return array.Length; }
            internal set { array.Length = value; }
        }
        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(string value)
        {
            array.Add(value);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(StringArray values)
        {
            array.Add(ref values.array);
        }
        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="values">字符串集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Append(params string[] values)
        {
            array.Add(values);
        }
        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="values">字符串集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Append(ref LeftArray<string> values)
        {
            array.Add(ref values);
        }
        /// <summary>
        /// 逆转列表
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Reverse()
        {
            array.Reverse();
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="join">字符连接</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string Join(char join)
        {
            return AutoCSer.Extension.ArrayExtension.JoinString(array.ToArray(), join);
        }
        /// <summary>
        /// 生成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return string.Concat(array.ToArray());
        }
    }
}
