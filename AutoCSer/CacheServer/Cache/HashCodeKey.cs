using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 哈希关键字
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [StructLayout(LayoutKind.Auto)]
    internal struct HashCodeKey<valueType> : IEquatable<HashCodeKey<valueType>>
        where valueType : IEquatable<valueType>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        internal readonly valueType Value;
        /// <summary>
        /// 哈希值
        /// </summary>
        internal readonly int HashCode;
        /// <summary>
        /// 哈希关键字
        /// </summary>
        /// <param name="value">关键字</param>
        internal HashCodeKey(valueType value)
        {
            this.Value = value;
            HashCode = value.GetHashCode();
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((HashCodeKey<valueType>)obj);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(HashCodeKey<valueType> other)
        {
            return HashCode == other.HashCode && Value.Equals(other.Value);
        }

        /// <summary>
        /// 获取关键字
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool Get(ref OperationParameter.NodeParser parser, out HashCodeKey<valueType> key)
        {
            if (parser.ValueData.Type == ValueData.Data<valueType>.DataType)
            {
                key = new HashCodeKey<valueType>(ValueData.Data<valueType>.GetData(ref parser.ValueData));
                return true;
            }
            parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            key = default(HashCodeKey<valueType>);
            return false;
        }
        /// <summary>
        /// 获取关键字
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool Get(ref OperationParameter.NodeParser parser, out valueType key)
        {
            if (parser.ValueData.Type == ValueData.Data<valueType>.DataType)
            {
                key = ValueData.Data<valueType>.GetData(ref parser.ValueData);
                return true;
            }
            parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            key = default(valueType);
            return false;
        }
    }
}
