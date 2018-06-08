using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 缓存主服务配置
        /// </summary>
        [FieldOffset(0)]
        public MasterServerConfig MasterServerConfig;
        /// <summary>
        /// 缓存从服务配置
        /// </summary>
        [FieldOffset(0)]
        public SlaveServerConfig SlaveServerConfig;
        /// <summary>
        /// 客户端数据结构定义信息
        /// </summary>
        [FieldOffset(0)]
        public ClientDataStructure ClientDataStructure;
        /// <summary>
        /// 短路径节点
        /// </summary>
        [FieldOffset(0)]
        public ShortPath.Node ShortPath;
    }
}
