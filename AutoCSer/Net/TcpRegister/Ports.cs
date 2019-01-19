using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 端口集合
    /// </summary>
    internal unsafe sealed class Ports : IDisposable
    {
        /// <summary>
        /// 已使用端口位图
        /// </summary>
        internal Pointer.Size Map;
        /// <summary>
        /// 当前待分配端口
        /// </summary>
        internal int Current;
        /// <summary>
        /// 起始位置
        /// </summary>
        internal int Start;
        /// <summary>
        /// 结束位置
        /// </summary>
        internal int End;
        /// <summary>
        /// 端口集合
        /// </summary>
        /// <param name="config"></param>
        internal Ports(Config config)
        {
            Current = Start = config.PortStart;
            End = config.PortEnd;
            Map = Unmanaged.GetSize64((((End - Start) + 63) >> 6) << 3, true);
        }
        /// <summary>
        /// 端口集合
        /// </summary>
        /// <param name="config"></param>
        /// <param name="current">当前待分配端口</param>
        internal Ports(Config config, int current)
        {
            Start = config.PortStart;
            End = config.PortEnd;
            Current = current >= Start && current < End ? current : Start;
            Map = Unmanaged.GetSize64((((End - Start) + 63) >> 6) << 3, true);
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~Ports()
        {
            Unmanaged.Free(ref Map);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Unmanaged.Free(ref Map);
        }
        /// <summary>
        /// 获取下一个端口
        /// </summary>
        internal int Next
        {
            get
            {
                MemoryMap map = new MemoryMap(Map.Byte);
                int port = End - Start;
                while (map.Get(Current - Start) != 0)
                {
                    if (++Current == End) Current = Start;
                    if (--port == 0) return -1;
                }
                port = Current;
                map.Set(Current - Start);
                if (++Current == End) Current = Start;
                return port;
            }
        }
        /// <summary>
        /// 设置端口使用状态
        /// </summary>
        /// <param name="port"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int port)
        {
            new MemoryMap(Map.Byte).Set(port - Start);
        }
        /// <summary>
        /// 清除端口使用状态
        /// </summary>
        /// <param name="port"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear(int port)
        {
            new MemoryMap(Map.Byte).Clear(port - Start);
        }
    }
}
