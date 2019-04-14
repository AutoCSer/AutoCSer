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
                switch (AutoCSer.Random.Default.NextByte())
                {
                    case 0: return float.MinValue;
                    case 1: return float.MaxValue;
                    case 2: return float.Epsilon;
                    case 3: return float.NaN;
                }
                float value = AutoCSer.Random.Default.NextFloat();
                if (float.IsNaN(value) || float.IsInfinity(value)) return float.NaN;
                return float.Parse(value.ToString());
            }
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return float.MinValue;
                case 1: return float.MaxValue;
                case 2: return float.Epsilon;
                case 3: return float.NaN;
                case 4: return float.PositiveInfinity;
                case 5: return float.NegativeInfinity;
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
                switch (AutoCSer.Random.Default.NextByte())
                {
                    case 0: return double.MinValue;
                    case 1: return double.MaxValue;
                    case 2: return double.Epsilon;
                    case 3: return double.Epsilon;
                    case 4: return double.NaN;
                    case 10: return 1.7976931348623157E+308;
                    case 11: return 1.7976931348623156E+308;
                    case 12: return 1.7976931348623155E+308;
                    case 13: return 1.7976931348623154E+308;
                    case 14: return 1.7976931348623153E+308;
                    case 15: return 1.7976931348623152E+308;
                    case 16: return 1.7976931348623151E+308;
                    case 17: return 1.7976931348623150E+308;
                    case 20: return -1.7976931348623157E+308;
                    case 21: return -1.7976931348623156E+308;
                    case 22: return -1.7976931348623155E+308;
                    case 23: return -1.7976931348623154E+308;
                    case 24: return -1.7976931348623153E+308;
                    case 25: return -1.7976931348623152E+308;
                    case 26: return -1.7976931348623151E+308;
                    case 27: return -1.7976931348623150E+308;
                }
                double value = AutoCSer.Random.Default.NextDouble();
                if (double.IsNaN(value) || double.IsInfinity(value)) return double.NaN;
                return double.Parse(value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return double.MinValue;
                case 1: return double.MaxValue;
                case 2: return double.Epsilon;
                case 3: return double.Epsilon;
                case 4: return double.NaN;
                case 5: return double.PositiveInfinity;
                case 6: return double.NegativeInfinity;
                case 10: return 1.7976931348623157E+308;
                case 11: return 1.7976931348623156E+308;
                case 12: return 1.7976931348623155E+308;
                case 13: return 1.7976931348623154E+308;
                case 14: return 1.7976931348623153E+308;
                case 15: return 1.7976931348623152E+308;
                case 16: return 1.7976931348623151E+308;
                case 17: return 1.7976931348623150E+308;
                case 20: return -1.7976931348623157E+308;
                case 21: return -1.7976931348623156E+308;
                case 22: return -1.7976931348623155E+308;
                case 23: return -1.7976931348623154E+308;
                case 24: return -1.7976931348623153E+308;
                case 25: return -1.7976931348623152E+308;
                case 26: return -1.7976931348623151E+308;
                case 27: return -1.7976931348623150E+308;
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
                return new DateTime((long)(AutoCSer.Random.Default.NextULong() % (ulong)DateTime.MaxValue.Ticks) / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond);
            }
            switch (AutoCSer.Random.Default.NextByte())
            {
                case 0: return DateTime.MinValue;
                case 1: return DateTime.MaxValue;
            }
            return new DateTime((long)(AutoCSer.Random.Default.NextULong() % (ulong)DateTime.MaxValue.Ticks));
        }
    }
}
