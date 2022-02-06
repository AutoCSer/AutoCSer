using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 类型扫描（非线程安全）
    /// </summary>
    public abstract class TypeScanner
    {
        /// <summary>
        /// 已添加扫描程序集
        /// </summary>
        public readonly HashSet<Assembly> ScanAssemblys = HashSetCreator.CreateOnly<Assembly>();
        /// <summary>
        /// 已添加扫描的类型
        /// </summary>
        public readonly HashSet<Type> ScanTypes = HashSetCreator.CreateOnly<Type>();
        /// <summary>
        /// 每个静态根对象最大统计对象数量
        /// </summary>
        internal readonly int MaxObjectCount;
        /// <summary>
        /// 静态根对象最大统计数量，超出部分淘汰数量最小的静态根对象
        /// </summary>
        internal readonly int MaxRootFieldCount;
        /// <summary>
        /// 对象扫描（非线程安全）
        /// </summary>
        /// <param name="maxObjectCount">每个静态根对象最大统计对象数量</param>
        /// <param name="maxRootFieldCount">静态根对象最大统计数量，超出部分淘汰数量最小的静态根对象</param>
        protected TypeScanner(int maxObjectCount, int maxRootFieldCount)
        {
            MaxObjectCount = Math.Max(maxObjectCount, 65536);
            MaxRootFieldCount = Math.Max(Math.Min(maxRootFieldCount, int.MaxValue - 1), 3) + 1;

            ScanTypes.Add(typeof(object));
            ScanTypes.Add(typeof(ValueType));
        }
        /// <summary>
        /// 检测程序集是否需要扫描
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        protected virtual bool check(Assembly assembly)
        {
            return true;
        }
        /// <summary>
        /// 添加程序集扫描类型
        /// </summary>
        /// <param name="assembly"></param>
        public virtual void Scan(Assembly assembly)
        {
            if (check(assembly) && ScanAssemblys.Add(assembly))
            {
                Type[] types = EmptyArray<Type>.Array;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (Exception exception)
                {
                    onException(assembly, exception);
                }
                foreach (Type type in types) Scan(type);
            }
        }
        /// <summary>
        /// 程序集获取类型异常处理
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="exception"></param>
        protected virtual void onException(Assembly assembly, Exception exception)
        {
            AutoCSer.LogHelper.Exception(exception, assembly.FullName + " 加载类型失败", LogLevel.Exception | LogLevel.AutoCSer);
        }
        /// <summary>
        /// 扫描当前应用程序集已加载程序集
        /// </summary>
        /// <param name="isSystem">是否扫描系统程序集，比如 System. 开始的</param>
        public virtual void ScanCurrentDomain(bool isSystem = false)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (isSystem || !isSystemAssembly(assembly)) Scan(assembly);
            }
        }
        /// <summary>
        /// 判断是否系统程序集，比如 System. 开始的
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        protected virtual bool isSystemAssembly(Assembly assembly)
        {
            string name = assembly.FullName;
            if ((name[0] & 2) != 0) return name.StartsWith("System", StringComparison.Ordinal) && (name[6] == '.' || name[6] == ','); // S = 0x53
            return name.StartsWith("mscorlib,", StringComparison.Ordinal); // m = 0x6d
        }
        /// <summary>
        /// 检测类型是否需要扫描
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool check(Type type)
        {
            return true;
        }
        /// <summary>
        /// 添加类型扫描静态字段
        /// </summary>
        /// <param name="type">必须是 struct 或者 class，可以是泛型，但是不能是泛型定义类型</param>
        public virtual void Scan(Type type)
        {
            if (type.IsClass)
            {
                if (!type.IsArray && !type.IsInterface && !type.IsGenericTypeDefinition)
                {
                    while (type != typeof(object))
                    {
                        if (check(type))
                        {
                            if (ScanTypes.Add(type)) scan(type);
                            else return;
                        }
                        type = type.BaseType;
                    }
                }
            }
            else if (type.IsValueType)
            {
                if (!type.IsEnum && !type.IsPrimitive && !type.IsPointer && !type.IsInterface && !type.IsGenericTypeDefinition
                    && (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>)))
                {
                    if (check(type) && ScanTypes.Add(type)) scan(type);
                }
            }
        }
        /// <summary>
        /// 检测字段是否需要扫描
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        protected virtual bool check(FieldInfo fieldInfo)
        {
            return true;
        }
        /// <summary>
        /// 扫描静态字段
        /// </summary>
        /// <param name="type"></param>
        protected virtual void scan(Type type)
        {
            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                try
                {
                    Type fieldType = fieldInfo.FieldType;
                    if (!fieldType.IsEnum && !fieldType.IsPrimitive && !fieldType.IsPointer && check(fieldInfo))
                    {
                        object value = fieldInfo.GetValue(null);
                        if (value != null) scanRoot(fieldInfo, value);
                    }
                }
                catch (Exception exception)
                {
                    onException(fieldInfo, exception);
                }
            }
        }
        /// <summary>
        /// 扫描静态字段异常处理
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="exception"></param>
        protected virtual void onException(FieldInfo fieldInfo, Exception exception)
        {
            AutoCSer.LogHelper.Exception(exception, fieldInfo.DeclaringType.FullName+"."+fieldInfo.Name+" 扫描失败", LogLevel.Exception | LogLevel.AutoCSer);
        }
        /// <summary>
        /// 扫描根对象
        /// </summary>
        /// <param name="fieldInfo">根对象静态字段</param>
        /// <param name="value">根对象</param>
        protected abstract void scanRoot(FieldInfo fieldInfo, object value);
        /// <summary>
        /// 统计对象数量超出限制
        /// </summary>
        /// <param name="fieldInfo"></param>
        protected internal virtual void OnLimitExceeded(FieldInfo fieldInfo) { }
    }
}
