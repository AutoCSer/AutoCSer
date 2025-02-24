﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.TestCase.ChatServer
{
    /// <summary>
    /// 群聊客户端
    /// </summary>
#if !DOTNET45
    [AutoCSer.Net.TcpOpenServer.Server(Host = "127.0.0.1", Port = 12400, SendBufferSize = AutoCSer.Memory.BufferSize.Kilobyte, ReceiveBufferSize = AutoCSer.Memory.BufferSize.Kilobyte, IsAutoClient = true, ClientSegmentationCopyPath = @"..\..\..\..\ChatClient\")]
#else
    [AutoCSer.Net.TcpOpenServer.Server(Host = "127.0.0.1", Port = 12400, SendBufferSize = AutoCSer.Memory.BufferSize.Kilobyte, ReceiveBufferSize = AutoCSer.Memory.BufferSize.Kilobyte, IsAutoClient = true, ClientSegmentationCopyPath = @"..\..\..\ChatClient\")]
#endif
    public partial class Server
    {
        /// <summary>
        /// 用户信息集合
        /// </summary>
        private readonly Dictionary<RandomKey<HashString>, User> users = DictionaryCreator<RandomKey<HashString>>.Create<User>();
        /// <summary>
        /// 待移除用户名称集合
        /// </summary>
        private LeftArray<HashString> removeUsers = new LeftArray<HashString>(0);
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字数据发送</param>
        /// <param name="userName">用户名称</param>
        /// <returns>是否成功</returns>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsVerifyMethod = true, IsClientAwaiter = false, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue)]
        private bool login(AutoCSer.Net.TcpOpenServer.ServerSocketSender socket, string userName)
        {
            HashString nameHash = userName;
            if (!users.ContainsKey(nameHash))
            {
                users.Add(nameHash, new User { Name = userName });
                socket.ClientObject = userName;
                socket.OnCloseTask += () => logout(socket);
                login(userName, ChatData.UserLoginType.Login);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字数据发送</param>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue, IsClientAwaiter = false)]
        private void logout(AutoCSer.Net.TcpOpenServer.ServerSocketSender socket)
        {
            string userName = (string)socket.ClientObject;
            if (userName != null)
            {
                HashString nameHash = userName;
                if (users.Remove(nameHash))
                {
                    socket.ClientObject = null;
                    login(userName, ChatData.UserLoginType.Logout);
                }
            }
        }
        /// <summary>
        /// 用户登录调用推送
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="type">用户登录类型</param>
        private void login(string userName, ChatData.UserLoginType type)
        {
            removeUsers.ClearOnlyLength();
            foreach (var user in users)
            {
                if (!user.Value.OnUser(userName, type)) removeUsers.Add(user.Key);
            }
            removeUser();
        }
        /// <summary>
        /// 清理待移除用户
        /// </summary>
        private void removeUser()
        {
            foreach (var removeUser in removeUsers) users.Remove(removeUser);
            foreach (var removeUser in removeUsers)
            {
                string userName = removeUser;
                foreach (var user in users) user.Value.OnUser(userName, ChatData.UserLoginType.OffLine);
            }
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字数据发送</param>
        /// <returns>当前用户信息</returns>
        private User getCurrentUser(AutoCSer.Net.TcpOpenServer.ServerSocketSender socket)
        {
            string userName = (string)socket.ClientObject;
            if (userName != null)
            {
                HashString nameHash = userName;
                User currentUser;
                if (users.TryGetValue(nameHash, out currentUser)) return currentUser;
            }
            return null;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字数据发送</param>
        /// <param name="content">消息内容</param>
        [AutoCSer.Net.TcpOpenServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue, IsClientAwaiter = false)]
        private void send(AutoCSer.Net.TcpOpenServer.ServerSocketSender socket, string content)
        {
            User currentUser = getCurrentUser(socket);
            if (currentUser != null)
            {
                ChatData.Message message = new ChatData.Message { User = currentUser.Name, Content = content, Time = AutoCSer.Threading.SecondTimer.Now };
                removeUsers.ClearOnlyLength();
                foreach (var user in users)
                {
                    if (user.Value != currentUser && !user.Value.OnMessage(ref message)) removeUsers.Add(user.Key);
                }
                removeUser();
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字数据发送</param>
        /// <param name="getUser">用户信息推送回调委托</param>
        [AutoCSer.Net.TcpOpenServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getUser(AutoCSer.Net.TcpOpenServer.ServerSocketSender socket, AutoCSer.Net.TcpServer.ServerCallback<ChatData.UserLogin> getUser)
        {
            User currentUser = getCurrentUser(socket);
            if (currentUser != null)
            {
                currentUser.GetUser = getUser;

                bool isLoaded = true;
                removeUsers.ClearOnlyLength();
                foreach (var user in users)
                {
                    if (user.Value != currentUser && !getUser.Callback(new ChatData.UserLogin { Name = user.Value.Name, Type = ChatData.UserLoginType.Load }))
                    {
                        isLoaded = false;
                        removeUsers.Add(currentUser.Name);
                        break;
                    }
                }
                if (isLoaded) getUser.Callback(new ChatData.UserLogin { Type = ChatData.UserLoginType.Loaded });
                removeUser();
            }
        }
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字数据发送</param>
        /// <param name="getMessage">消息推送回调委托</param>
        [AutoCSer.Net.TcpOpenServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void getMessage(AutoCSer.Net.TcpOpenServer.ServerSocketSender socket, AutoCSer.Net.TcpServer.ServerCallback<ChatData.Message> getMessage)
        {
            User currentUser = getCurrentUser(socket);
            if (currentUser != null) currentUser.GetMessage = getMessage;
        }
    }
}
