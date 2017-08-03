using System;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 基本类型随机数创建函数
    /// </summary>
    internal sealed class CreateConfigNullMethod : Attribute { }

    /// <summary>
    /// 随机对象生成函数信息
    /// </summary>
    internal static partial class MethodCache
    {
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [CreateConfigNullMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static string createStringNull(Config config)
        {
            object historyValue = config.TryGetValue(typeof(string));
            if (historyValue != null) return (string)historyValue;
            char[] value = createArrayNull<char>(config);
            return config.SaveHistory(value == null ? null : new string(value));
        }
    }
}
