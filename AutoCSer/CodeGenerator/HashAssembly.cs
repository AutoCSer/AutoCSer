using System;
using System.Reflection;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// System.Reflection.Assembly 包装 IEquatable，用于 Hash 比较
    /// </summary>
    internal struct HashAssembly : IEquatable<HashAssembly>
    {
        /// <summary>
        /// 程序集
        /// </summary>
        public Assembly Assembly;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(HashAssembly other)
        {
            return Assembly == other.Assembly;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((HashAssembly)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Assembly.GetHashCode();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator HashAssembly(Assembly assembly) { return new HashAssembly { Assembly = assembly }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator Assembly(HashAssembly assembly) { return assembly.Assembly; }
    }
}
