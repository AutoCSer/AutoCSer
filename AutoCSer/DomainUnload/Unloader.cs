using System;
using System.Threading;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.DomainUnload
{
    /// <summary>
    /// 应用程序卸载处理
    /// </summary>
    public static class Unloader
    {
        /// <summary>
        /// 卸载处理函数集合
        /// </summary>
        private static readonly HashSet<UnloadInfo> unloaders = HashSetCreator<UnloadInfo>.Create();
        /// <summary>
        /// 卸载处理函数集合
        /// </summary>
        private static readonly HashSet<UnloadInfo> lastUnloaders = HashSetCreator<UnloadInfo>.Create();
        /// <summary>
        /// 卸载处理函数访问锁
        /// </summary>
        private static readonly object unloaderLock = new object();
        /// <summary>
        /// 卸载状态
        /// </summary>
        internal static State State = State.Run;
        /// <summary>
        /// 事务数量
        /// </summary>
        private static int transactionCount;
        /// <summary>
        /// 添加应用程序卸载处理
        /// </summary>
        /// <param name="unload">卸载处理函数</param>
        /// <param name="type"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Add(object unload, Type type)
        {
            UnloadInfo unloadInfo = new UnloadInfo { Unload = unload, Type = type };
            Add(ref unloadInfo);
        }
        /// <summary>
        /// 添加应用程序卸载处理
        /// </summary>
        /// <param name="unloadInfo">卸载处理函数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Add(ref UnloadInfo unloadInfo)
        {
            Monitor.Enter(unloaderLock);
            try
            {
                if (State == State.Run || State == State.WaitTransaction)
                {
                    unloaders.Add(unloadInfo);
                    unloadInfo.Type = Type.None;
                }
            }
            finally
            {
                Monitor.Exit(unloaderLock);
                unloadInfo.Call();
            }
        }
        /// <summary>
        /// 删除卸载处理函数
        /// </summary>
        /// <param name="unload">卸载处理函数</param>
        /// <param name="type"></param>
        /// <param name="isRun">是否执行删除的函数</param>
        /// <returns>是否删除成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool Remove(object unload, Type type, bool isRun)
        {
            UnloadInfo unloadInfo = new UnloadInfo { Unload = unload, Type = type };
            Monitor.Enter(unloaderLock);
            bool isRemove = unloaders.Remove(unloadInfo);
            Monitor.Exit(unloaderLock);
            if (isRemove && isRun) unloadInfo.Call();
            return isRemove;
        }
        /// <summary>
        /// 删除卸载处理函数
        /// </summary>
        /// <param name="unloadInfo">卸载处理函数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Remove(ref UnloadInfo unloadInfo)
        {
            Monitor.Enter(unloaderLock);
            unloaders.Remove(unloadInfo);
            Monitor.Exit(unloaderLock);
            unloadInfo.Unload = null;
        }
        /// <summary>
        /// 添加应用程序卸载处理
        /// </summary>
        /// <param name="unload">卸载处理函数</param>
        /// <param name="type"></param>
        internal static void AddLast(object unload, Type type)
        {
            UnloadInfo unloadInfo = new UnloadInfo { Unload = unload, Type = type };
            Monitor.Enter(unloaderLock);
            try
            {
                if (State == State.Run || State == State.WaitTransaction)
                {
                    lastUnloaders.Add(unloadInfo);
                    unloadInfo.Type = Type.None;
                }
            }
            finally
            {
                Monitor.Exit(unloaderLock);
                unloadInfo.Call();
            }
        }
        /// <summary>
        /// 删除卸载处理函数
        /// </summary>
        /// <param name="unload">卸载处理函数</param>
        /// <param name="type"></param>
        /// <param name="isRun">是否执行删除的函数</param>
        internal static void RemoveLast(object unload, Type type, bool isRun)
        {
            if (Monitor.TryEnter(unloaderLock))
            {
                bool isRemove = lastUnloaders.Remove(new UnloadInfo { Unload = unload, Type = type });
                Monitor.Exit(unloaderLock);
                if (isRemove && isRun) new UnloadInfo { Unload = unload, Type = type }.Call();
            }
            else if (isRun) AutoCSer.Threading.ThreadPool.Tiny.FastStart(new UnloadObject { Unload = unload, Type = type }, AutoCSer.Threading.Thread.CallType.DomainUnloadRemoveLastRun);
            else AutoCSer.Threading.ThreadPool.Tiny.FastStart(new UnloadObject { Unload = unload, Type = type }, AutoCSer.Threading.Thread.CallType.DomainUnloadRemoveLast);
        }
        /// <summary>
        /// 删除卸载处理函数
        /// </summary>
        /// <param name="onUnload"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void RemoveLast(UnloadInfo onUnload)
        {
            Monitor.Enter(unloaderLock);
            lastUnloaders.Remove(onUnload);
            Monitor.Exit(unloaderLock);
        }
        /// <summary>
        /// 删除卸载处理函数
        /// </summary>
        /// <param name="onUnload"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void RemoveLastRun(UnloadInfo onUnload)
        {
            Monitor.Enter(unloaderLock);
            bool isRemove = lastUnloaders.Remove(onUnload);
            Monitor.Exit(unloaderLock);
            if (isRemove) onUnload.Call();
        }
        /// <summary>
        /// 新事务开始,请保证唯一调用TransactionEnd,否则将导致卸载事件不被执行
        /// </summary>
        /// <param name="ignoreWait">忽略卸载中的等待事务，用于事务派生的事务</param>
        /// <returns>是否成功</returns>
        internal static bool TransactionStart(bool ignoreWait)
        {
            Monitor.Enter(unloaderLock);
            if (State == DomainUnload.State.Run || (ignoreWait && State == DomainUnload.State.WaitTransaction))
            {
                ++transactionCount;
                Monitor.Exit(unloaderLock);
                return true;
            }
            Monitor.Exit(unloaderLock);
            AutoCSer.Log.Pub.Log.Wait(Log.LogType.Debug, "应用程序正在退出");
            return false;
        }
        /// <summary>
        /// 请保证TransactionStart与TransactionEnd一一对应，否则将导致卸载事件不被执行
        /// </summary>
        internal static void TransactionEnd()
        {
            Monitor.Enter(unloaderLock);
            --transactionCount;
            Monitor.Exit(unloaderLock);
        }
        /// <summary>
        /// 应用程序卸载事件
        /// </summary>
        private static void unloadEvent()
        {
            Monitor.Enter(unloaderLock);
            if (State == State.Run)
            {
                if (transactionCount != 0)
                {
                    State = State.WaitTransaction;
                    Monitor.Exit(unloaderLock);
                    for (DateTime logTime = DateTime.MinValue; transactionCount != 0; Thread.Sleep(1))
                    {
                        if (Date.NowTime.Now > logTime)
                        {
                            AutoCSer.Log.Pub.Log.Wait(Log.LogType.Debug, "事务未结束 " + transactionCount.toString());
                            logTime = Date.NowTime.Now.AddTicks(TimeSpan.TicksPerMinute);
                        }
                    }
                    Monitor.Enter(unloaderLock);
                }
                State = State.Event;
                try
                {
                    foreach (UnloadInfo value in unloaders.getArray())
                    {
                        try { value.Call(); }
                        catch (Exception error)
                        {
                            AutoCSer.Log.Pub.Log.Wait(Log.LogType.Error, error);
                        }
                    }
                    foreach (UnloadInfo value in lastUnloaders.getArray())
                    {
                        try { value.Call(); }
                        catch (Exception error)
                        {
                            AutoCSer.Log.Pub.Log.Wait(Log.LogType.Error, error);
                        }
                    }
                    State = State.Unloaded;
                }
                finally { Monitor.Exit(unloaderLock); }
            }
            else Monitor.Exit(unloaderLock);
        }
        /// <summary>
        /// 应用程序卸载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void unloadEvent(object sender, EventArgs e)
        {
            unloadEvent();
        }
        /// <summary>
        /// 线程错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="error"></param>
        private static void onError(object sender, UnhandledExceptionEventArgs error)
        {
            Exception exception = error.ExceptionObject as Exception;
            if (exception != null) AutoCSer.Log.Pub.Log.Wait(Log.LogType.Error, exception);
            else AutoCSer.Log.Pub.Log.Wait(Log.LogType.Error, error.ExceptionObject.ToString());
            unloadEvent(null, null);
        }

        /// <summary>
        /// 公共默认应用程序默认卸载配置
        /// </summary>
        internal static readonly UnloadEventConfig DefaultConfig = ConfigLoader.GetUnion(typeof(UnloadEventConfig)).UnloadEventConfig ?? new UnloadEventConfig();

        static Unloader()
        {
            DefaultConfig.Set(unloadEvent, onError);
        }
    }
}
