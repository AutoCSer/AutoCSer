using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 成员扩展缓存初始化依赖类型加载
    /// </summary>
    internal sealed class MemberCacheLinkWait
    {
        /// <summary>
        /// 成员扩展缓存初始化依赖类型加载等待事件
        /// </summary>
        private AutoCSer.Threading.WaitHandle wait;
        /// <summary>
        /// 成员扩展缓存初始化依赖类型集合
        /// </summary>
        private HashSet<Type> cacheTypes;
        /// <summary>
        /// 当前表格缓存是否加载完毕
        /// </summary>
        private bool isCacheLoaded
        {
            get
            {
                return cacheTypes != null && cacheTypes.Count == 0;
            }
        }
        /// <summary>
        /// 依赖当前缓存初始化的类型集合
        /// </summary>
        private LeftArray<MemberCacheLinkWait> waits;
        /// <summary>
        /// 成员扩展缓存初始化依赖类型加载
        /// </summary>
        /// <param name="wait">依赖当前缓存初始化的类型</param>
        private MemberCacheLinkWait(MemberCacheLinkWait wait)
        {
            this.wait.Set(0);
            waits.Add(wait);
        }
        /// <summary>
        /// 成员扩展缓存初始化依赖类型加载
        /// </summary>
        /// <param name="cacheTypes">成员扩展缓存初始化依赖类型集合</param>
        private MemberCacheLinkWait(HashSet<Type> cacheTypes)
        {
            wait.Set(0);
            set(cacheTypes);
        }
        /// <summary>
        /// 设置成员扩展缓存初始化依赖类型集合
        /// </summary>
        /// <param name="cacheTypes">成员扩展缓存初始化依赖类型集合</param>
        private void set(HashSet<Type> cacheTypes)
        {
            MemberCacheLinkWait wait;
            removeCacheTypes.Length = 0;
            foreach (Type cacheType in cacheTypes)
            {
                if (loadTypes.TryGetValue(cacheType, out wait))
                {
                    if (wait.isCacheLoaded) removeCacheTypes.Add(cacheType);
                    else wait.waits.Add(this);
                }
                else loadTypes.Add(cacheType, new MemberCacheLinkWait(this));
            }
            foreach (Type cacheType in removeCacheTypes) cacheTypes.Remove(cacheType);
            if (cacheTypes.Count == 0)
            {
                this.cacheTypes = emptyCacheTypes;
                this.wait.Set();
            }
            else this.cacheTypes = cacheTypes;
        }
        /// <summary>
        /// 当前表格缓存加载完毕
        /// </summary>
        /// <param name="tableType">当前表格类型</param>
        /// <param name="cacheTypes">成员扩展缓存初始化依赖类型集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void set(Type tableType, HashSet<Type> cacheTypes)
        {
            if (this.cacheTypes == null)
            {
                foreach (MemberCacheLinkWait wait in waits) wait.removeCacheType(tableType);
                waits.SetNull();
                set(cacheTypes);
            }
        }
        /// <summary>
        /// 移除成员扩展缓存初始化依赖类型
        /// </summary>
        /// <param name="cacheType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void removeCacheType(Type cacheType)
        {
            cacheTypes.Remove(cacheType);
            if (cacheTypes.Count == 0) wait.Set();
        }
        /// <summary>
        /// 等待成员扩展缓存初始化依赖加载
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Wait()
        {
            if (cacheTypes == null || cacheTypes.Count != 0) wait.Wait();
        }

        /// <summary>
        /// 数据完成类型加载集合
        /// </summary>
        private static readonly Dictionary<Type, MemberCacheLinkWait> loadTypes = DictionaryCreator.CreateOnly<Type, MemberCacheLinkWait>();
        /// <summary>
        /// 数据完成类型加载集合访问锁
        /// </summary>
        private static readonly object loadedLock = new object();
        /// <summary>
        /// 成员扩展缓存初始化依赖类型空集合
        /// </summary>
        private static readonly HashSet<Type> emptyCacheTypes = AutoCSer.HashSetCreator.CreateOnly<Type>();
        /// <summary>
        /// 待删除成员扩展缓存初始化依赖类型集合
        /// </summary>
        private static LeftArray<Type> removeCacheTypes;
        /// <summary>
        /// 加载成员扩展缓存初始化依赖类型
        /// </summary>
        /// <param name="tableType">表格类型</param>
        /// <param name="cacheTypes">表格成员扩展缓存初始化依赖类型集合</param>
        internal static void Load(Type tableType, HashSet<Type> cacheTypes)
        {
            MemberCacheLinkWait wait;
            Monitor.Enter(loadedLock);
            try
            {
                if (loadTypes.TryGetValue(tableType, out wait)) wait.set(tableType, cacheTypes);
                else loadTypes.Add(tableType, wait = new MemberCacheLinkWait(cacheTypes));
            }
            finally { Monitor.Exit(loadedLock); }
        }
        /// <summary>
        /// 等待成员扩展缓存初始化
        /// </summary>
        /// <param name="tableType">等待成员扩展缓存初始化的类型</param>
        internal static void Wait(Type tableType)
        {
            MemberCacheLinkWait wait;
            Monitor.Enter(loadedLock);
            loadTypes.TryGetValue(tableType, out wait);
            Monitor.Exit(loadedLock);
            wait.Wait();
        }
    }
}
