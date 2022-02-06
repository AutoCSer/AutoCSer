using System;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 对象引用
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ObjectReference : IEquatable<ObjectReference>
    {
        /// <summary>
        /// 对象数据
        /// </summary>
        private readonly object value;
        /// <summary>
        /// 哈希值
        /// </summary>
        internal readonly int HashCode;
        /// <summary>
        /// 对象引用
        /// </summary>
        /// <param name="value">对象数据</param>
        public ObjectReference(object value)
        {
            this.value = value;
            try
            {
                HashCode = value.GetHashCode();
            }
            catch
            {
                HashCode = value.GetType().GetHashCode();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ObjectReference other)
        {
            return object.ReferenceEquals(value, other.value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode;
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
