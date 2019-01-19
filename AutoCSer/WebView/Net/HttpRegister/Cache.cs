using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// 域名注册信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Cache
    {
        /// <summary>
        /// 程序集文件名,包含路径
        /// </summary>
        public string AssemblyFile;
        /// <summary>
        /// 服务程序类型名称
        /// </summary>
        public string ServerTypeName;
        /// <summary>
        /// 域名信息集合
        /// </summary>
        public Domain[] Domains;
        /// <summary>
        /// 是否共享程序集
        /// </summary>
        public bool IsShareAssembly;
        /// <summary>
        /// 获取域名注册信息
        /// </summary>
        /// <param name="domains"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Cache Get(Domain[] domains)
        {
            return new Cache { AssemblyFile = AssemblyFile, ServerTypeName = ServerTypeName, Domains = domains, IsShareAssembly = IsShareAssembly };
        }
    }
}
