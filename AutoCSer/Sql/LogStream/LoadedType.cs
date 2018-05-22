using System;
using System.Threading;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 日志流数据完成类型加载
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct LoadedType : IEquatable<LoadedType>
    {
        /// <summary>
        /// 数据模型类型
        /// </summary>
        internal Type Type;
        /// <summary>
        /// 表格编号，主要使用枚举识别同一数据模型下的不同表格
        /// </summary>
        internal int TableNumber;
        /// <summary>
        /// 日志流数据完成类型加载
        /// </summary>
        /// <param name="type">数据模型类型</param>
        /// <param name="tableNumber">表格编号</param>
        public LoadedType(Type type, int tableNumber)
        {
            Type = type;
            TableNumber = tableNumber;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tableNumber"></param>
        /// <returns></returns>
        public bool Equals(Type type, int tableNumber)
        {
            return Type == type && tableNumber == TableNumber;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(LoadedType other)
        {
            return Type == other.Type && TableNumber == other.TableNumber;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((LoadedType)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ TableNumber;
        }

        /// <summary>
        /// 已经加载的数据完成类型集合
        /// </summary>
        private static readonly HashSet<LoadedType> loadedTypes = HashSetCreator<LoadedType>.Create();
        /// <summary>
        /// 数据完成类型加载集合
        /// </summary>
        private static readonly Dictionary<LoadedType, ListArray<Action<LoadedType>>> loadTypes = DictionaryCreator<LoadedType>.Create<ListArray<Action<LoadedType>>>();
        /// <summary>
        /// 数据完成类型加载集合访问锁
        /// </summary>
        private static readonly object loadedLock = new object();
        /// <summary>
        /// 数据完成类型注册
        /// </summary>
        /// <param name="modelType">数据模型类型</param>
        /// <param name="tableNumber">表格编号</param>
        /// <param name="onLoaded">数据完成操作</param>
        /// <param name="types">待加载类型集合</param>
        internal static void Add(Type modelType, int tableNumber, Action<LoadedType> onLoaded, params LoadedType[] types)
        {
            bool isLoaded = false;
            LoadedType loadedType = new LoadedType(modelType, tableNumber);
            Monitor.Enter(loadedLock);
            try
            {
                if (loadedTypes.Contains(loadedType)) isLoaded = true;
                else
                {
                    loadedTypes.Add(loadedType);
                    ListArray<Action<LoadedType>> waitLoads;
                    if (loadTypes.TryGetValue(loadedType, out waitLoads))
                    {
                        loadTypes.Remove(loadedType);
                        foreach (Action<LoadedType> waitLoader in waitLoads) waitLoader(loadedType);
                    }
                    if (types != null)
                    {
                        foreach (LoadedType type in types)
                        {
                            if (loadedTypes.Contains(type)) onLoaded(type);
                            else
                            {
                                if (!loadTypes.TryGetValue(type, out waitLoads)) loadTypes.Add(type, waitLoads = new ListArray<Action<LoadedType>>());
                                waitLoads.Add(onLoaded);
                            }
                        }
                    }
                }
            }
            finally { Monitor.Exit(loadedLock); }
            if (isLoaded) AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, "数据完成类型注册冲突 " + modelType.fullName() + "[" + tableNumber.toString() + "]");
        }
        /// <summary>
        /// 数据完成类型注册
        /// </summary>
        /// <param name="modelType">数据模型类型</param>
        /// <param name="tableNumber">表格编号</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Add(Type modelType, int tableNumber)
        {
            Add(modelType, tableNumber, null, null);
        }
    }
}
