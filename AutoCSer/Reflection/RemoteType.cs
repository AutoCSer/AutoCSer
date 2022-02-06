using System;
using System.Reflection;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 远程类型
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct RemoteType
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 远程类型
        /// </summary>
        /// <param name="type">类型</param>
        public RemoteType(Type type)
        {
            Name = type.FullName;
            AssemblyName = type.Assembly.FullName;
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
            Assembly assembly = AssemblyCache.Get(AssemblyName);
            if (assembly != null)
            {
                if ((type = assembly.GetType(Name)) != null) return true;
            }
            else type = null;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return AssemblyName + " + " + Name;
        }
    }
}
