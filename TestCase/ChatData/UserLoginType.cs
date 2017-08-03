using System;

namespace AutoCSer.TestCase.ChatData
{
    /// <summary>
    /// 用户登录类型
    /// </summary>
    public enum UserLoginType : byte
    {
        /// <summary>
        /// 加载历史数据
        /// </summary>
        Load,
        /// <summary>
        /// 历史数据加载完成
        /// </summary>
        Loaded,
        /// <summary>
        /// 用户登录
        /// </summary>
        Login,
        /// <summary>
        /// 用户退出
        /// </summary>
        Logout,
        /// <summary>
        /// 掉线退出
        /// </summary>
        OffLine,
    }
}
