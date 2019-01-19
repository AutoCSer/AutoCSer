using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务套接字数据发送
    /// </summary>
    public abstract partial class ServerSocketSenderBase
    {
        /// <summary>
        /// 添加自定义 TCP 服务器端同步调用任务
        /// </summary>
        /// <param name="task">任务委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task AddTaskAsync(Action task)
        {
            if (task != null)
            {
                CustomTaskAsyncServerCall serverCall = new CustomTaskAsyncServerCall(this, task);
                ServerCallTask.Task.Add(serverCall);
                await serverCall;
            }
        }
    }
    /// <summary>
    /// TCP 服务客户端套接字数据发送
    /// </summary>
    public abstract partial class ClientSocketSender<attributeType>
    {
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="inputParameter">输入参数</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>保持回调</returns>
        public ReturnType GetAsync<inputParameterType, outputParameterType>(CommandInfo identityCommand, TaskAsyncReturnValue<outputParameterType> callback
            , ref inputParameterType inputParameter, ref outputParameterType outputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputOutputCommand<inputParameterType, outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>>.Default.Pop() ?? new ClientCommand.InputOutputCommand<inputParameterType, outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback, ref inputParameter, ref outputParameter);
                    push(command);
                    return ReturnType.Success;
                }
                return ReturnType.ClientException;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        /// <typeparam name="outputParameterType">输出参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="callback">异步回调</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>保持回调</returns>
        public ReturnType GetAsync<outputParameterType>(CommandInfo identityCommand, TaskAsyncReturnValue<outputParameterType> callback, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.OutputCommand<outputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.OutputCommand<outputParameterType>>.Default.Pop() ?? new ClientCommand.OutputCommand<outputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, callback, ref outputParameter);
                    push(command);
                    return ReturnType.Success;
                }
                return ReturnType.ClientException;
            }
            return ReturnType.ClientDisposed;
        }

        /// <summary>
        /// TCP调用
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        public ReturnType CallAsync<inputParameterType>(CommandInfo identityCommand, TaskAsyncReturnValue onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
        {
            if (IsSocket)
            {
                ClientCommand.InputCommand<inputParameterType> command = AutoCSer.Threading.RingPool<ClientCommand.InputCommand<inputParameterType>>.Default.Pop() ?? new ClientCommand.InputCommand<inputParameterType>();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, onCall.Call, ref inputParameter);
                    push(command);
                    return ReturnType.Success;
                }
                return ReturnType.ClientException;
            }
            return ReturnType.ClientDisposed;
        }
        /// <summary>
        /// TCP调用（用于代码生成编译）
        /// </summary>
        /// <typeparam name="inputParameterType">输入参数类型</typeparam>
        /// <typeparam name="outputParameterType">输入参数类型</typeparam>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <param name="inputParameter">输入参数</param>
        /// <returns>保持回调</returns>
        internal ReturnType CallAsync<inputParameterType, outputParameterType>(CommandInfo identityCommand, TaskAsyncReturnValue<outputParameterType> onCall, ref inputParameterType inputParameter)
            where inputParameterType : struct
            where outputParameterType : struct
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand">命令信息</param>
        /// <param name="onCall">回调委托</param>
        /// <returns>保持回调</returns>
        public ReturnType CallAsync(CommandInfo identityCommand, TaskAsyncReturnValue onCall)
        {
            if (IsSocket)
            {
                ClientCommand.CallCommand command = AutoCSer.Threading.RingPool<ClientCommand.CallCommand>.Default.Pop() ?? new ClientCommand.CallCommand();
                if (command != null)
                {
                    command.Set(ClientSocket, identityCommand, onCall.Call);
                    push(command);
                    return ReturnType.Success;
                }
                return ReturnType.ClientException;
            }
            return ReturnType.ClientDisposed;
        }
    }
}
