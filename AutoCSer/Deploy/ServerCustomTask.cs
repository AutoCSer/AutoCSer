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
        void OnDeployServerUpdated(Server server, byte[] customData);
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
        internal virtual void Call(Server server, Task task) { }
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
        private readonly Dictionary<string, Action<valueType, Server, byte[]>> tasks = DictionaryCreator.CreateOnly<string, Action<valueType, Server, byte[]>>();
        /// <summary>
        /// 自定义任务调用
        /// </summary>
        /// <param name="value">任务目标对象</param>
        internal ServerCustomTask(valueType value)
        {
            this.value = value;
            foreach (MethodInfo method in typeof(valueType).GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!method.IsGenericMethodDefinition && method.ReturnType == typeof(void))
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
                            tasks.Add(method.Name, (Action<valueType, Server, byte[]>)Delegate.CreateDelegate(typeof(Action<valueType, Server, byte[]>), method));
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
        internal override void Call(Server server, Task task)
        {
            tasks[task.RunFileName](value, server, task.CustomData);
        }
    }
}
