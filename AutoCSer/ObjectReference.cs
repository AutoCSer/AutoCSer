using System;

namespace AutoCSer
{
    /// <summary>
    /// 对象引用（用户序列化循环引用比较）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ObjectReference : IEquatable<ObjectReference>
    {
        /// <summary>
        /// 对象
        /// </summary>
        public object Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ObjectReference other)
        {
            if (object.ReferenceEquals(Value, other.Value)) return true;
            return Value.GetType() == typeof(string) && other.Value.GetType() == typeof(string) && (string)Value == (string)other.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            try
            {
                return Value.GetHashCode();
            }
            catch
            {
                return Value.GetType().GetHashCode();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((ObjectReference)obj);
        }
    }
}
