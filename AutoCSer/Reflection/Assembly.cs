using System;
using System.Reflection;
using AutoCSer.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 程序集扩展操作
    /// </summary>
    internal static class Assembly
    {
        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="fullName">程序集名称</param>
        /// <returns>程序集,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static System.Reflection.Assembly Get(string fullName)
        {
            if (fullName != null)
            {
                System.Reflection.Assembly value;
                if (nameCache.TryGetValue(fullName, out value)) return value;
            }
            return null;
        }
        /// <summary>
        /// 程序集名称缓存
        /// </summary>
        private static readonly LockDictionary<string, System.Reflection.Assembly> nameCache = new LockDictionary<string, System.Reflection.Assembly>();
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void loadAssembly(System.Reflection.Assembly assembly)
        {
            nameCache.Set(assembly.FullName, assembly);
        }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void loadAssembly(object sender, AssemblyLoadEventArgs args)
        {
            loadAssembly(args.LoadedAssembly);
        }

        static Assembly()
        {
            AppDomain.CurrentDomain.AssemblyLoad += loadAssembly;
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) nameCache.Set(assembly.FullName, assembly);
        }
    }
}
