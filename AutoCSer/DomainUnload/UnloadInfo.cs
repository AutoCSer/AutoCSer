using System;

namespace AutoCSer.DomainUnload
{
    /// <summary>
    /// 委托回调
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct UnloadInfo : IEquatable<UnloadInfo>
    {
        /// <summary>
        /// 卸载处理对象
        /// </summary>
        public object Unload;
        /// <summary>
        /// 卸载处理类型
        /// </summary>
        public Type Type;
        /// <summary>
        /// 委托回调
        /// </summary>
        /// <param name="unload"></param>
        /// <param name="type"></param>
        public void Set(object unload, Type type)
        {
            Unload = unload;
            Type = type;
        }
        /// <summary>
        /// 卸载处理
        /// </summary>
        public void Call()
        {
            switch (Type)
            {
                case Type.Action: new AutoCSer.Threading.UnionType { Value = Unload }.Action(); return;
                case Type.LogFileDispose: new AutoCSer.Log.UnionType { Value = Unload }.File.Dispose(); return;
                case Type.TcpCommandBaseDispose: new AutoCSer.Net.TcpServer.UnionType { Value = Unload }.CommandBase.Dispose(); return;
                case Type.TcpRegisterClientClose: AutoCSer.Net.TcpRegister.Client.Close(); return;
                case Type.ThreadPoolDispose: new AutoCSer.Threading.UnionType { Value = Unload }.ThreadPool.Dispose(); return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(UnloadInfo other)
        {
            return Type == other.Type && Unload == other.Unload;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Unload == null) return (byte)Type;
            return Unload.GetHashCode() ^ (byte)Type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((UnloadInfo)obj);
        }
    }
}
