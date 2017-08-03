using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 基本类型随机数创建函数
    /// </summary>
    internal sealed class CreateConfigMethod : Attribute { }

    /// <summary>
    /// 随机对象生成函数信息
    /// </summary>
    internal static partial class MethodCache
    {
        /// <summary>
        /// 创建随机字符
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [CreateConfigMethod]
        [CreateConfigNullMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static char createChar(Config config)
        {
            if (config.IsNullChar) return (char)AutoCSer.Random.Default.NextUShort();
            char value = (char)AutoCSer.Random.Default.NextUShort();
            return value == 0 ? char.MaxValue : value;
        }
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [CreateConfigMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static string createString(Config config)
        {
            object historyValue = config.TryGetValue(typeof(string));
            if (historyValue != null) return (string)historyValue;
            return config.SaveHistory(new string(createArray<char>(config)));
        }
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [CreateConfigMethod]
        [CreateConfigNullMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static SubString createSubString(Config config)
        {
            return createStringNull(config);
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [CreateConfigMethod]
        [CreateConfigNullMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe static float createFloat(Config config)
        {
            if (config.IsParseFloat)
            {
                return float.Parse(AutoCSer.Random.Default.NextFloat().ToString());
            }
            return AutoCSer.Random.Default.NextFloat();
        }
        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [CreateConfigMethod]
        [CreateConfigNullMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe static double createDouble(Config config)
        {
            if (config.IsParseFloat)
            {
                return double.Parse(AutoCSer.Random.Default.NextDouble().ToString());
            }
            return AutoCSer.Random.Default.NextDouble();
        }
        /// <summary>
        /// 创建随机时间
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [CreateConfigMethod]
        [CreateConfigNullMethod]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static DateTime createDateTime(Config config)
        {
            if (config.IsSecondDateTime)
            {
                return new DateTime((long)(AutoCSer.Random.Default.NextULong() % (ulong)DateTime.MaxValue.Ticks) / Date.SecondTicks * Date.SecondTicks);
            }
            return new DateTime((long)(AutoCSer.Random.Default.NextULong() % (ulong)DateTime.MaxValue.Ticks));
        }
    }
}
