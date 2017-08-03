using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.TestCase.ChatServer
{
    /// <summary>
    /// 用户信息
    /// </summary>
    internal sealed class User
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 用户登录时间
        /// </summary>
        public DateTime LoginTime = Date.Now;
        /// <summary>
        /// 用户信息推送回调委托
        /// </summary>
        public Func<AutoCSer.Net.TcpServer.ReturnValue<ChatData.UserLogin>, bool> GetUser;
        /// <summary>
        /// 其它用户登录调用推送
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="type">用户登录类型</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool OnUser(string userName, ChatData.UserLoginType type)
        {
            return GetUser == null || GetUser(new ChatData.UserLogin { Name = userName, Type = type });
        }
        /// <summary>
        /// 消息推送回调委托
        /// </summary>
        public Func<AutoCSer.Net.TcpServer.ReturnValue<ChatData.Message>, bool> GetMessage;
        /// <summary>
        /// 用户消息调用推送
        /// </summary>
        /// <param name="message">用户消息</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool OnMessage(ref ChatData.Message message)
        {
            return GetMessage == null || GetMessage(message);
        }
    }
}
