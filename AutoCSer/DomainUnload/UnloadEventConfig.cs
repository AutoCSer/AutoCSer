using System;

namespace AutoCSer.DomainUnload
{
    /// <summary>
    /// 应用程序默认卸载配置
    /// </summary>
    public class UnloadEventConfig
    {
        /// <summary>
        /// 绑定卸载事件
        /// </summary>
        /// <param name="unloadEvent"></param>
        /// <param name="onError"></param>
        public virtual void Set(EventHandler unloadEvent, UnhandledExceptionEventHandler onError)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain()) AppDomain.CurrentDomain.ProcessExit += unloadEvent;
            else AppDomain.CurrentDomain.DomainUnload += unloadEvent;
            AppDomain.CurrentDomain.UnhandledException += onError;
        }
    }
}
