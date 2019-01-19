using System;
using System.Reflection.Emit;

namespace AutoCSer.Emit.Builder
{
    /// <summary>
    /// 动态程序集模块
    /// </summary>
    internal static class Module
    {
        /// <summary>
        /// 动态程序集
        /// </summary>
        private static readonly AssemblyBuilder assemblyBuilder;
        /// <summary>
        /// 动态程序集模块
        /// </summary>
        internal static readonly ModuleBuilder Builder;
        static Module()
        {
#if DOTNET2 || DOTNET4 || UNITY3D
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new System.Reflection.AssemblyName("AutoCSer.DynamicAssembly"), AssemblyBuilderAccess.Run);
#else
            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName("AutoCSer.DynamicAssembly"), AssemblyBuilderAccess.Run);
#endif
            Builder = assemblyBuilder.DefineDynamicModule("AutoCSer.DynamicModule");
        }
    }
}
