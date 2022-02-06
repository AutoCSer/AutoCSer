using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 自定义任务调用
    /// </summary>
    public class ServerCustomTask
    {
        /// <summary>
        /// 调用自定义任务
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual DeployResultData Call(Server server, ClientTask.Custom task) { return DeployState.Success; }
        /// <summary>
        /// 自定义任务调用
        /// </summary>
        internal ServerCustomTask() { }
        /// <summary>
        /// 默认空任务
        /// </summary>
        public static readonly ServerCustomTask Null = new ServerCustomTask();
    }
    /// <summary>
    /// 自定义任务调用
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public sealed class ServerCustomTask<valueType> : ServerCustomTask
    {
        /// <summary>
        /// 任务目标对象
        /// </summary>
        private readonly valueType value;
        /// <summary>
        /// 自定义任务集合
        /// </summary>
        private readonly Dictionary<string, Func<valueType, Server, AutoCSer.Net.TcpInternalServer.ServerSocketSender, byte[], DeployResultData>> tasks = DictionaryCreator.CreateOnly<string, Func<valueType, Server, AutoCSer.Net.TcpInternalServer.ServerSocketSender, byte[], DeployResultData>>();
        /// <summary>
        /// 自定义任务调用
        /// </summary>
        /// <param name="value">任务目标对象</param>
        public ServerCustomTask(valueType value)
        {
            this.value = value;
            foreach (MethodInfo method in typeof(valueType).GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!method.IsGenericMethodDefinition && method.ReturnType == typeof(DeployResultData))
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 3 && parameters[0].ParameterType == typeof(Server) && parameters[1].ParameterType == typeof(AutoCSer.Net.TcpInternalServer.ServerSocketSender) && parameters[2].ParameterType == typeof(byte[]))
                    {
                        CustomAttribute customAttribute = null;
                        foreach (CustomAttribute nextCustomAttribute in method.GetCustomAttributes(typeof(CustomAttribute), false))
                        {
                            if (customAttribute == null) customAttribute = nextCustomAttribute;
                            else if (nextCustomAttribute.GetIsIgnoreCurrent)
                            {
                                customAttribute = nextCustomAttribute;
                                break;
                            }
                        }
                        if (customAttribute == null || !customAttribute.GetIsIgnoreCurrent)
                        {
                            tasks.Add(method.Name, (Func<valueType, Server, AutoCSer.Net.TcpInternalServer.ServerSocketSender, byte[], DeployResultData>)Delegate.CreateDelegate(typeof(Func<valueType, Server, AutoCSer.Net.TcpInternalServer.ServerSocketSender, byte[], DeployResultData>), method));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 调用自定义任务
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public override DeployResultData Call(Server server, ClientTask.Custom task)
        {
            return tasks[task.CallName](value, server, task.Sender, task.CustomData);
        }
    }
}
