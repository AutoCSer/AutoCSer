using System;

namespace AutoCSer
{
    /// <summary>
    /// 哈希关键字
    /// </summary>
    /// <typeparam name="T1">关键字类型1</typeparam>
    /// <typeparam name="T2">关键字类型2</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HashKey<T1, T2> : IEquatable<HashKey<T1, T2>>
        where T1 : IEquatable<T1>
        where T2 : IEquatable<T2>
    {
        /// <summary>
        /// 关键字1
        /// </summary>
        private readonly T1 value1;
        /// <summary>
        /// 关键字2
        /// </summary>
        private readonly T2 value2;
        /// <summary>
        /// 哈希关键字
        /// </summary>
        /// <param name="value1">关键字1</param>
        /// <param name="value2">关键字2</param>
        internal HashKey(T1 value1, T2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(HashKey<T1, T2> other)
        {
            return value1.Equals(other.value1) && value2.Equals(other.value2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((HashKey<T1, T2>)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value1.GetHashCode() ^ value2.GetHashCode();
        }
    }
}
