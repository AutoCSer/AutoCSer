using System;

namespace AutoCSer
{
    /// <summary>
    /// System.Type 包装 IEquatable，用于 Hash 比较
    /// </summary>
    internal struct HashType : IEquatable<HashType>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(HashType other)
        {
            return Type == other.Type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((HashType)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator HashType(Type type) { return new HashType { Type = type }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator Type(HashType type) { return type.Type; }
    }
}
