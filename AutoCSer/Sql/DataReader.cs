using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DataReader
    {
        /// <summary>
        /// 基本类型设置函数
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> dataReaderGetMethods;
        /// <summary>
        /// 获取基本类型设置函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>设置函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetMethod(Type type)
        {
            MethodInfo method;
            return dataReaderGetMethods.TryGetValue(type, out method) ? method : null;
        }
#if NOJIT
        /// <summary>
        /// 获取基本类型设置函数信息
        /// </summary>
        internal static readonly MethodInfo GetMethodInfo;
#else
        /// <summary>
        /// 判断数据是否为空
        /// </summary>
        internal static readonly MethodInfo IsDBNullMethod = typeof(DbDataReader).GetMethod("IsDBNull", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(int) }, null);
#endif

        static DataReader()
        {
            dataReaderGetMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
#if NOJIT
            GetMethodInfo = typeof(pub).GetMethod("GetDataReaderMethod", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(Type) }, null);
            dataReaderGetMethods.Add(typeof(bool), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(byte), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(char), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(DateTime), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(decimal), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(double), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(float), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(Guid), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(short), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(int), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(long), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(string), GetMethodInfo);
#else
            Type[] intType = new Type[] { typeof(int) };
            dataReaderGetMethods.Add(typeof(bool), typeof(DbDataReader).GetMethod("GetBoolean", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(byte), typeof(DbDataReader).GetMethod("GetByte", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(char), typeof(DbDataReader).GetMethod("GetChar", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(DateTime), typeof(DbDataReader).GetMethod("GetDateTime", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(decimal), typeof(DbDataReader).GetMethod("GetDecimal", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(double), typeof(DbDataReader).GetMethod("GetDouble", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(float), typeof(DbDataReader).GetMethod("GetFloat", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(Guid), typeof(DbDataReader).GetMethod("GetGuid", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(short), typeof(DbDataReader).GetMethod("GetInt16", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(int), typeof(DbDataReader).GetMethod("GetInt32", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(long), typeof(DbDataReader).GetMethod("GetInt64", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
            dataReaderGetMethods.Add(typeof(string), typeof(DbDataReader).GetMethod("GetString", BindingFlags.Public | BindingFlags.Instance, null, intType, null));
#endif
        }
    }
}
