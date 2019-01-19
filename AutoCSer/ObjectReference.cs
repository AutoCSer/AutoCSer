using System;

namespace AutoCSer
{
    /// <summary>
    /// 对象引用
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
            Type type = Value.GetType();
            if (Value.GetType() == other.Value.GetType())
            {
                return type == typeof(string) ? (string)Value == (string)other.Value : object.ReferenceEquals(Value, other.Value);
            }
            return false;
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
