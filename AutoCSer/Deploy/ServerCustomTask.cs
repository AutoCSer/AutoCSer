using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 自定义任务调用
    /// </summary>
    public interface IServerCustomTask
    {
        /// <summary>
        /// 发布服务更新以后的后续处理
        /// </summary>
        /// <param name="server"></param>
        /// <param name="customData"></param>
        /// <returns></returns>
        DeployState OnDeployServerUpdated(Server server, byte[] customData);
    }
    /// <summary>
    /// 自定义任务调用
    /// </summary>
    internal class ServerCustomTask
    {
        /// <summary>
        /// 调用自定义任务
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        internal virtual DeployState Call(Server server, Task task) { return DeployState.Success; }
        /// <summary>
        /// 自定义任务调用
        /// </summary>
        internal ServerCustomTask() { }
        /// <summary>
        /// 默认空任务
        /// </summary>
        internal static readonly ServerCustomTask Null = new ServerCustomTask();
    }
    /// <summary>
    /// 自定义任务调用
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal sealed class ServerCustomTask<valueType> : ServerCustomTask
    {
        /// <summary>
        /// 任务目标对象
        /// </summary>
        private readonly valueType value;
        /// <summary>
        /// 自定义任务集合
        /// </summary>
        private readonly Dictionary<string, Func<valueType, Server, byte[], DeployState>> tasks = DictionaryCreator.CreateOnly<string, Func<valueType, Server, byte[], DeployState>>();
        /// <summary>
        /// 自定义任务调用
        /// </summary>
        /// <param name="value">任务目标对象</param>
        internal ServerCustomTask(valueType value)
        {
            this.value = value;
            foreach (MethodInfo method in typeof(valueType).GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!method.IsGenericMethodDefinition && method.ReturnType == typeof(DeployState))
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 2 && parameters[0].ParameterType == typeof(Server) && parameters[1].ParameterType == typeof(byte[]))
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
                            tasks.Add(method.Name, (Func<valueType, Server, byte[], DeployState>)Delegate.CreateDelegate(typeof(Func<valueType, Server, byte[], DeployState>), method));
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
        internal override DeployState Call(Server server, Task task)
        {
            return tasks[task.RunFileName](value, server, task.CustomData);
        }
    }
}
