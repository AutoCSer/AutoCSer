using System;
using AutoCSer.Extension;
using System.Reflection;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// 自定义队列
    /// </summary>
    internal sealed class ServerCallQueue
    {
        /// <summary>
        /// 队列编号
        /// </summary>
        internal readonly int Index;
        /// <summary>
        /// 队列名称
        /// </summary>
        internal readonly string Name;
        /// <summary>
        /// 队列类型
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// 参数类型
        /// </summary>
        internal readonly Type ParameterType;
        /// <summary>
        /// 获取队列函数信息
        /// </summary>
        internal readonly MethodInfo GetMethod;
        /// <summary>
        /// 自定义队列
        /// </summary>
        /// <param name="index">队列编号</param>
        /// <param name="parameterType">参数类型</param>
        internal ServerCallQueue(int index, Type parameterType)
        {
            Index = index;
            Name = "_p" + index.toString();
            ParameterType = parameterType;
            Type = typeof(IServerCallQueue<>).MakeGenericType(parameterType);
            GetMethod = Type.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public);
        }
    }
}
