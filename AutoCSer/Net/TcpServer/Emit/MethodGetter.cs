﻿using System;
using System.Reflection;
using AutoCSer.Extensions;
using System.Collections.Generic;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 函数信息
    /// </summary>
    internal abstract partial class Method<methodType, attributeType, methodAttributeType, serverSocketType>
    {
        /// <summary>
        /// 获取 TCP 服务函数信息
        /// </summary>
        internal abstract class Getter
        {
            /// <summary>
            /// 默认 TCP 服务配置
            /// </summary>
            internal attributeType DefaultServerAttribute;
            /// <summary>
            /// 错误字符串提示信息
            /// </summary>
            internal string ErrorString;
            /// <summary>
            /// TCP 函数集合
            /// </summary>
            internal methodType[] Methods;
            /// <summary>
            /// 自定义队列集合
            /// </summary>
            internal ServerCallQueue[] Queues;
            /// <summary>
            /// 是否 TCP 服务客户端
            /// </summary>
            internal bool IsClient;
            /// <summary>
            /// 获取 TCP 函数
            /// </summary>
            /// <param name="type"></param>
            /// <param name="method"></param>
            /// <param name="serverAttribute"></param>
            /// <param name="methodAttribute"></param>
            /// <returns></returns>
            protected abstract methodType get(Type type, MethodInfo method, attributeType serverAttribute, methodAttributeType methodAttribute);
            /// <summary>
            /// 获取 TCP 函数
            /// </summary>
            /// <param name="type"></param>
            /// <param name="serverAttribute"></param>
            /// <param name="methodAttribute"></param>
            /// <param name="isClient"></param>
            /// <param name="methods"></param>
            /// <returns></returns>
            private bool get(Type type, attributeType serverAttribute, methodAttributeType methodAttribute, bool isClient, ref LeftArray<methodType> methods)
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.IsGenericMethod)
                    {
                        ErrorString = "不支持泛型函数 " + type.fullName() + "." + method.Name;
                        return false;
                    }
                    methods.Add(get(type, method, serverAttribute, methodAttribute));
                }
                return true;
            }
            /// <summary>
            /// 获取 TCP 服务函数信息
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            internal bool Get(Type type)
            {
                if (type.IsInterface)
                {
                    if (type.IsGenericTypeDefinition)
                    {
                        ErrorString = "不支持泛型接口 " + type.fullName();
                        return false;
                    }
                    //if (type.GetInterfaces().length() != 0)
                    //{
                    //    ErrorString = "不支持接口继承 " + type.fullName();
                    //    return false;
                    //}
                    foreach (PropertyInfo property in type.GetProperties())
                    {
                        ErrorString = "不支持属性 " + type.fullName();
                        return false;
                    }
                    attributeType serverAttribute = null;
                    foreach (attributeType attribute in type.GetCustomAttributes(typeof(attributeType), false))
                    {
                        serverAttribute = attribute;
                        break;
                    }
                    if (serverAttribute == null) serverAttribute = AutoCSer.Metadata.DefaultConstructor<attributeType>.Constructor();
                    methodAttributeType methodAttribute = null;
                    foreach (methodAttributeType attribute in type.GetCustomAttributes(typeof(methodAttributeType), false))
                    {
                        methodAttribute = attribute;
                        break;
                    }
                    if (methodAttribute == null)
                    {
                        methodAttribute = AutoCSer.Metadata.DefaultConstructor<methodAttributeType>.Constructor();
                        methodAttribute.IsDefault = true;
                    }

                    LeftArray<methodType> tcpMethods = new LeftArray<methodType>(0);
                    if (!get(type, serverAttribute, methodAttribute, true, ref tcpMethods)) return false;
                    foreach (Type interfaceType in type.GetInterfaces().notNull())
                    {
                        if (!get(interfaceType, serverAttribute, methodAttribute, true, ref tcpMethods)) return false;
                    }
                    //MethodInfo[] typeMethods = type.GetMethods();
                    //Methods = new methodType[typeMethods.Length];
                    //int tcpMethodIndex = 0;
                    //foreach (MethodInfo method in typeMethods)
                    //{
                    //    if (method.IsGenericMethod)
                    //    {
                    //        ErrorString = "不支持泛型函数 " + type.fullName() + "." + method.Name;
                    //        return false;
                    //    }
                    //    Methods[tcpMethodIndex++] = new methodType(method, serverAttribute, servermethodAttributeType, isClient);
                    //}
                    //LeftArray<methodType> tcpMethods = new LeftArray<methodType> { Array = Methods, Length = tcpMethodIndex };
                    //foreach (PropertyInfo property in type.GetProperties())
                    //{
                    //    if (property.CanRead)
                    //    {
                    //        methodType getMethod = new methodType(property, serverAttribute, servermethodAttributeType);
                    //        tcpMethods.Add(getMethod);
                    //        if (getMethod.PropertySetMethod != null) tcpMethods.Add(getMethod.PropertySetMethod);
                    //    }
                    //    else tcpMethods.Add(new methodType(property, serverAttribute, servermethodAttributeType, true));
                    //}
                    if (tcpMethods.Length == 0)
                    {
                        ErrorString = type.fullName() + " 没有找到函数定义";
                        return false;
                    }
                    foreach (methodType method in tcpMethods)
                    {
                        if (!method.CheckRef(ref ErrorString)) return false;
                    }
                    tcpMethods.Sort(Compare);
                    if ((Methods = CheckIdentity(ref tcpMethods, ref ErrorString)) == null) return false;
                    bool isVerifyMethod = false;
                    foreach (methodType method in Methods)
                    {
                        if (method != null)
                        {
                            if (isVerifyMethod) method.Attribute.IsVerifyMethod = false;
                            else if (method.CheckIsVerifyMethod(ref ErrorString))
                            {
                                isVerifyMethod = true;
                                //if (!method.IsVerifyMethodAsynchronous) method.Attribute.ServerTaskType = Net.TcpServer.ServerTaskType.Synchronous;
                            }
                            else if (ErrorString != null) return false;
                        }
                    }
                    Dictionary<Type, ServerCallQueue> queueTypes = null;
                    foreach (methodType method in Methods)
                    {
                        if (method != null)
                        {
                            method.SetParameterType();

                            if (method.ServerCallQueueKeyParameter != null)
                            {
                                if (queueTypes == null) queueTypes = DictionaryCreator.CreateOnly<Type, ServerCallQueue>();
                                Type parameterType = method.ServerCallQueueKeyParameter.ParameterType;
                                ServerCallQueue queueType;
                                if (!queueTypes.TryGetValue(parameterType, out queueType))
                                {
                                    queueTypes.Add(parameterType, queueType = new ServerCallQueue(queueTypes.Count, parameterType));
                                }
                                method.ServerCallQueue = queueType;
                            }
                        }
                    }
                    Queues = queueTypes == null ? EmptyArray<ServerCallQueue>.Array : queueTypes.Values.getArray();
                    string serviceName = serverAttribute.ServerName ?? type.fullName();
                    DefaultServerAttribute = (attributeType)AutoCSer.Configuration.Common.Get(typeof(attributeType), serviceName) ?? AutoCSer.MemberCopy.Copyer<attributeType>.MemberwiseClone(serverAttribute);
                    if (DefaultServerAttribute.Name == null) DefaultServerAttribute.Name = serviceName;
                    if (DefaultServerAttribute.GetServerCallQueueType != null && !typeof(AutoCSer.Net.TcpServer.IServerCallQueueSet).IsAssignableFrom(DefaultServerAttribute.GetServerCallQueueType))
                    {
                        ErrorString = DefaultServerAttribute.GetServerCallQueueType.fullName() + @" 没有继承实现 " + typeof(AutoCSer.Net.TcpServer.IServerCallQueueSet).fullName();
                        return false;
                    }
                    return true;
                }
                ErrorString = type.fullName() + " 不是接口类型";
                return false;
            }
        }
    }
}
