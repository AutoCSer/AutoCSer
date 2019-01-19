using System;

namespace AutoCSer
{
    /// <summary>
    /// 随机防 HASH 构造关键字
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RandomKey<keyType> : IEquatable<RandomKey<keyType>>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        internal keyType Key;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator RandomKey<keyType>(keyType value)
        {
            return new RandomKey<keyType> { Key = value };
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator keyType(RandomKey<keyType> value)
        {
            return value.Key;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Random.Hash;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RandomKey<keyType> other)
        {
            return Key.Equals(other.Key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(keyType other)
        {
            return Key.Equals(other);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((RandomKey<keyType>)obj);
        }
    }
}
