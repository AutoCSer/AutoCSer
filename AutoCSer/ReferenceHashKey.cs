using System;

namespace AutoCSer
{
    /// <summary>
    /// 引用哈希关键字
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReferenceHashKey<T> : IEquatable<ReferenceHashKey<T>> where T : class
    {
        /// <summary>
        /// 哈希关键字
        /// </summary>
        internal T Value;
        /// <summary>
        /// 哈希关键字
        /// </summary>
        /// <param name="value">关键字</param>
        internal ReferenceHashKey(T value)
        {
            Value = value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ReferenceHashKey<T> other)
        {
            return object.ReferenceEquals(Value, other.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((ReferenceHashKey<T>)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
