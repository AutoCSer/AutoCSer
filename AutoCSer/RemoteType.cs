using System;
using System.Reflection;

namespace AutoCSer
{
    /// <summary>
    /// 远程类型
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct RemoteType// : IEquatable<RemoteType>
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        private string assemblyName;
        /// <summary>
        /// 类型名称
        /// </summary>
        private string name;
        /// <summary>
        /// 远程类型
        /// </summary>
        /// <param name="type">类型</param>
        public RemoteType(Type type)
        {
            name = type.FullName;
            assemblyName = type.Assembly.FullName;
        }
        /// <summary>
        /// 类型隐式转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>远程类型</returns>
        public static implicit operator RemoteType(Type type) { return new RemoteType(type); }
        /// <summary>
        /// 尝试获取类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否成功</returns>
        public bool TryGet(out Type type)
        {
            Assembly assembly = Reflection.Assembly.Get(assemblyName);
            if (assembly != null)
            {
                if ((type = assembly.GetType(name)) != null) return true;
            }
            else type = null;
            return false;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //public bool Equals(RemoteType other)
        //{
        //    return name == other.name && assemblyName == other.assemblyName;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public override bool Equals(object obj)
        //{
        //    return Equals((RemoteType)obj);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return name.GetHashCode() ^ assemblyName.GetHashCode();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return assemblyName + " + " + name;
        }
    }
}
