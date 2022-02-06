using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程任务
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ThreadTask
    {
        /// <summary>
        /// 任务委托
        /// </summary>
        internal object Value;
        /// <summary>
        /// 调用类型
        /// </summary>
        internal ThreadTaskType Type;
        /// <summary>
        /// 设置任务信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(object value, ThreadTaskType type)
        {
            Value = value;
            Type = type;
        }
        /// <summary>
        /// 设置任务委托
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(Action value)
        {
            Value = value;
            Type = ThreadTaskType.Action;
        }
        /// <summary>
        /// 任务调用
        /// </summary>
        public void Call()
        {
            switch (Type)
            {
                case ThreadTaskType.Action: new AutoCSer.UnionType.Action { Object = Value }.Value(); return;
                case ThreadTaskType.SecondTimerTaskNodeOnTimer: new UnionType.SecondTimerTaskNode { Object = Value }.Value.OnTimer(); return;
                case ThreadTaskType.SecondTimerTaskNodeAfter: new UnionType.SecondTimerTaskNode { Object = Value }.Value.After(); return;
                case ThreadTaskType.ConfigLoadException: AutoCSer.Configuration.Root.ConfigLoadException((ListArray<KeyValue<Type, Exception>>)Value); return;
                case ThreadTaskType.ConfigFormatException: AutoCSer.Config.ConfigFormatException((Exception)Value); return;
                case ThreadTaskType.CompileBinarySerialize: AutoCSer.BinarySerializer.Compile((Type[])Value); return;
                case ThreadTaskType.CompileBinaryDeSerialize: AutoCSer.BinaryDeSerializer.Compile((Type[])Value); return;
                case ThreadTaskType.CompileJsonSerialize: AutoCSer.JsonSerializer.Compile((Type[])Value); return;
                case ThreadTaskType.CompileJsonDeSerialize: AutoCSer.JsonDeSerializer.Compile((Type[])Value); return;
                case ThreadTaskType.CompileSimpleSerialize: AutoCSer.Net.SimpleSerialize.Serializer.Compile((Type[])Value); return;
                case ThreadTaskType.CompileSimpleDeSerialize: AutoCSer.Net.SimpleSerialize.DeSerializer.Compile((Type[])Value); return;
                case ThreadTaskType.DomainUnloadRemoveLast: new AutoCSer.DomainUnload.UnionType.UnloadObject { Object = Value }.Value.RemoveLast(); return;
                case ThreadTaskType.DomainUnloadRemoveLastRun: new AutoCSer.DomainUnload.UnionType.UnloadObject { Object = Value }.Value.RemoveLastRun(); return;
                //case ThreadTaskType.FileStreamWriterDispose: new AutoCSer.IO.UnionType { Value = Value }.FileStreamWriter.Dispose(); return;
                //case ThreadTaskType.FileStreamWriteFile: new AutoCSer.IO.UnionType { Value = Value }.FileStreamWriter.WriteFile(); return;
                case ThreadTaskType.TaskSwitchThreadRun: new UnionType.TaskSwitchThreadBase { Object = Value }.Value.run(); return;
                case ThreadTaskType.TcpClientSocketBaseCreate: ((AutoCSer.Net.TcpServer.ClientSocketBase)Value).Create(); return;
                case ThreadTaskType.TcpClientCommandPoolTimeout: new AutoCSer.Net.TcpServer.UnionType.CommandPoolTimeout { Object = Value }.Value.OnTimeout(); return;
                case ThreadTaskType.TcpInternalClientSocketSenderBuildOutput: new AutoCSer.Net.TcpInternalServer.UnionType.ClientSocketSender { Object = Value }.Value.BuildOutput(); return;
                case ThreadTaskType.TcpInternalServerGetSocket: ((AutoCSer.Net.TcpInternalServer.Server)Value).GetSocket(); return;
                case ThreadTaskType.TcpInternalServerSocketSenderBuildOutput: new AutoCSer.Net.TcpInternalServer.UnionType.ServerSocketSender { Object = Value }.Value.BuildOutput(); return;
                case ThreadTaskType.TcpOpenClientSocketSenderBuildOutput: new AutoCSer.Net.TcpOpenServer.UnionType.ClientSocketSender { Object = Value }.Value.BuildOutput(); return;
                case ThreadTaskType.TcpOpenServerGetSocket: new AutoCSer.Net.TcpOpenServer.UnionType.Server { Object = Value }.Value.GetSocket(); return;
                case ThreadTaskType.TcpOpenServerOnSocket: new AutoCSer.Net.TcpOpenServer.UnionType.Server { Object = Value }.Value.OnSocket(); return;
                case ThreadTaskType.TcpOpenServerSocketSenderBuildOutput: new AutoCSer.Net.TcpOpenServer.UnionType.ServerSocketSender { Object = Value }.Value.BuildOutput(); return;
            }
        }
    }
}
