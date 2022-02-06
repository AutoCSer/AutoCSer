using System;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 公共配置，该类型不允许增加依赖成员
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 根配置
        /// </summary>
        internal static IRoot Root = AutoCSer.Configuration.Root.Null;
        /// <summary>
        /// 配置缓存是否已经加载
        /// </summary>
        internal static bool IsConfigLoaded;
        /// <summary>
        /// 设置根配置，用于如果不是 Assembly.GetEntryAssembly() 或者不希望扫描程序集的场景
        /// </summary>
        /// <param name="root">根配置</param>
        /// <returns>false 表示配置缓存已经加载，设置无效</returns>
        public static bool SetRoot(IRoot root)
        {
            if (IsConfigLoaded || root == null) return false;
            Root = root;
            return true;
        }

        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="name">配置名称，默认为 null 表示默认名称</param>
        /// <returns>配置项数据</returns>
        public static object Get(Type type, string name = "")
        {
            return Cache.Get(type, name);
        }
    }
}
