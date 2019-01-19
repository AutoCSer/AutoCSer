using System;

namespace AutoCSer.Data
{
    /// <summary>
    /// 组合关键字
    /// </summary>
    /// <typeparam name="keyType1">关键字类型1</typeparam>
    /// <typeparam name="keyType2">关键字类型2</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct PrimaryKey<keyType1, keyType2> : IEquatable<PrimaryKey<keyType1, keyType2>>, IComparable<PrimaryKey<keyType1, keyType2>>
        where keyType1 : IEquatable<keyType1>, IComparable<keyType1>
        where keyType2 : IEquatable<keyType2>, IComparable<keyType2>
    {
        /// <summary>
        /// 关键字1
        /// </summary>
        public keyType1 Key1;
        /// <summary>
        /// 关键字2
        /// </summary>
        public keyType2 Key2;
        /// <summary>
        /// 哈希编码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Key1.GetHashCode() ^ Key2.GetHashCode();
        }
        /// <summary>
        /// 关键字比较
        /// </summary>
        /// <param name="obj">关键字</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((PrimaryKey<keyType1, keyType2>)obj);
        }
        /// <summary>
        /// 关键字比较
        /// </summary>
        /// <param name="other">关键字</param>
        /// <returns>是否相等</returns>
        public bool Equals(PrimaryKey<keyType1, keyType2> other)
        {
            return Key1.Equals(other.Key1) && Key2.Equals(other.Key2);
        }
        /// <summary>
        /// 关键字大小
        /// </summary>
        /// <param name="other">关键字</param>
        /// <returns>比较结果</returns>
        public int CompareTo(PrimaryKey<keyType1, keyType2> other)
        {
            int cmp = Key1.CompareTo(other.Key1);
            return cmp != 0 ? cmp : Key2.CompareTo(other.Key2);
        }
    }
}
