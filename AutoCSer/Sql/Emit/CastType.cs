using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Emit
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CastType : IEquatable<CastType>
    {
        /// <summary>
        /// 原始类型
        /// </summary>
        internal Type FromType;
        /// <summary>
        /// 目标类型
        /// </summary>
        internal Type ToType;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CastType other)
        {
            return FromType == other.FromType && ToType == other.ToType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return FromType.GetHashCode() ^ ToType.GetHashCode();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((CastType)obj);
        }

        #region Excel 导入数据特殊处理
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static int DoubleToInt(double Value)
        {
            return (int)Value;
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static uint DoubleToUInt(double Value)
        {
            return (uint)Value;
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static long DoubleToLong(double Value)
        {
            return (long)Value;
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static ulong DoubleToULong(double Value)
        {
            return (ulong)Value;
        }
        #endregion

        /// <summary>
        /// 类型转换函数集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<CastType, MethodInfo> methods = new AutoCSer.Threading.LockDictionary<CastType, MethodInfo>();
        /// <summary>
        /// 获取类型转换函数
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        internal static MethodInfo GetMethod(Type fromType, Type toType)
        {
            if (fromType == toType) return null;
#if !NOJIT
            if (fromType == typeof(int))
            {
                if (toType == typeof(uint)) return null;
            }
            else if (fromType == typeof(long))
            {
                if (toType == typeof(ulong)) return null;
            }
            else if (fromType == typeof(byte))
            {
                if (toType == typeof(sbyte)) return null;
            }
            else if (fromType == typeof(short))
            {
                if (toType == typeof(ushort)) return null;
            }
#endif
            else if (fromType == typeof(double))
            {
                #region Excel 导入数据特殊处理
                if (toType == typeof(int)) return ((Func<double, int>)DoubleToInt).Method;
                else if (toType == typeof(long)) return ((Func<double, long>)DoubleToLong).Method;
                else if (toType == typeof(uint)) return ((Func<double, uint>)DoubleToUInt).Method;
                else if (toType == typeof(ulong)) return ((Func<double, ulong>)DoubleToULong).Method;
                #endregion
            }

            CastType castType = new CastType { FromType = fromType, ToType = toType };
            MethodInfo method;
            if (methods.TryGetValue(castType, out method)) return method;
#if !NOJIT
            if (!toType.IsPrimitive)
#endif
            {
                foreach (MethodInfo methodInfo in toType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    if (methodInfo.ReturnType == toType && (methodInfo.Name == "op_Implicit" || methodInfo.Name == "op_Explicit") && methodInfo.GetParameters()[0].ParameterType == fromType)
                    {
                        method = methodInfo;
                        break;
                    }
                }
                //Type[] castParameterTypes = new Type[] { fromType };
                //method = toType.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, castParameterTypes, null)
                //    ?? toType.GetMethod("op_Explicit", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, castParameterTypes, null);
            }
#if NOJIT
            if (method == null)
#else
            if (method == null && !fromType.IsPrimitive)
#endif
            {
                foreach (MethodInfo methodInfo in fromType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    if (methodInfo.ReturnType == toType && (methodInfo.Name == "op_Implicit" || methodInfo.Name == "op_Explicit") && methodInfo.GetParameters()[0].ParameterType == fromType)
                    {
                        method = methodInfo;
                        break;
                    }
                }
            }
            methods.Set(castType, method);
            return method;
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            methods.Clear();
        }
        static CastType()
        {
            AutoCSer.Pub.ClearCaches += clearCache;
        }
    }
}
