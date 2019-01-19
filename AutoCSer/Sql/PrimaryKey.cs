using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 组合关键字
    /// </summary>
    /// <typeparam name="keyType1">关键字类型1</typeparam>
    /// <typeparam name="keyType2">关键字类型2</typeparam>
    /// <typeparam name="keyType3">关键字类型3</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct PrimaryKey<keyType1, keyType2, keyType3> : IEquatable<PrimaryKey<keyType1, keyType2, keyType3>>, IComparable<PrimaryKey<keyType1, keyType2, keyType3>>
        where keyType1 : IEquatable<keyType1>, IComparable<keyType1>
        where keyType2 : IEquatable<keyType2>, IComparable<keyType2>
        where keyType3 : IEquatable<keyType3>, IComparable<keyType3>
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
        /// 关键字3
        /// </summary>
        public keyType3 Key3;
        /// <summary>
        /// 哈希编码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Key1.GetHashCode() ^ Key2.GetHashCode() ^ Key3.GetHashCode();
        }
        /// <summary>
        /// 关键字比较
        /// </summary>
        /// <param name="obj">关键字</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((PrimaryKey<keyType1, keyType2, keyType3>)obj);
        }
        /// <summary>
        /// 关键字比较
        /// </summary>
        /// <param name="other">关键字</param>
        /// <returns>是否相等</returns>
        public bool Equals(PrimaryKey<keyType1, keyType2, keyType3> other)
        {
            return Key1.Equals(other.Key1) && Key2.Equals(other.Key2) && Key3.Equals(other.Key3);
        }
        /// <summary>
        /// 关键字大小
        /// </summary>
        /// <param name="other">关键字</param>
        /// <returns>比较结果</returns>
        public int CompareTo(PrimaryKey<keyType1, keyType2, keyType3> other)
        {
            int cmp = Key1.CompareTo(other.Key1);
            if (cmp == 0)
            {
                cmp = Key2.CompareTo(other.Key2);
                if (cmp == 0) cmp = Key3.CompareTo(other.Key3);
            }
            return cmp;
        }
    }
}
