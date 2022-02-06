using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 程序集缓存
    /// </summary>
    internal sealed class AssemblyCache
    {
        /// <summary>
        /// 程序集全名称
        /// </summary>
        private readonly string fullName;
        /// <summary>
        /// 程序集
        /// </summary>
        private readonly Assembly assembly;
        /// <summary>
        /// 程序集缓存
        /// </summary>
        /// <param name="assembly">程序集</param>
        private AssemblyCache(Assembly assembly)
        {
            this.assembly = assembly;
            fullName = assembly.FullName;
        }

        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="fullName">程序集名称</param>
        /// <returns>程序集,失败返回null</returns>
        public static Assembly Get(string fullName)
        {
            AssemblyCache assemblyCache = lastAssembly;
            if (assemblyCache != null && assemblyCache.fullName == fullName) return assemblyCache.assembly;

            while (cache.TryGetValue(fullName, out assemblyCache))
            {
                if (assemblyCache.fullName == fullName)
                {
                    lastAssembly = assemblyCache;
                    return assemblyCache.assembly;
                }
            }
            return null;
        }
        /// <summary>
        /// 程序集缓存
        /// </summary>
        private static readonly Dictionary<string, AssemblyCache> cache = DictionaryCreator.CreateOnly<string, AssemblyCache>();
        /// <summary>
        /// 程序集缓存访问
        /// </summary>
        private static AutoCSer.Threading.SpinLock cacheLock;
        /// <summary>
        /// 最后一次访问的程序集
        /// </summary>
        private static AssemblyCache lastAssembly;
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void loadAssembly(object sender, AssemblyLoadEventArgs args)
        {
            AssemblyCache assemblyCache = new AssemblyCache(args.LoadedAssembly);
            cacheLock.EnterSleep();
            try
            {
                cache[assemblyCache.fullName] = assemblyCache;
            }
            finally { cacheLock.Exit(); }
        }

        static AssemblyCache()
        {
            AppDomain.CurrentDomain.AssemblyLoad += loadAssembly;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssemblyCache assemblyCache = new AssemblyCache(assembly);
                cache[assemblyCache.fullName] = assemblyCache;
            }
        }
    }
}
