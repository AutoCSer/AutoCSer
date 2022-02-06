using System;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extensions;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 配置加载缓存
    /// </summary>
    internal static class Cache
    {
        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="name">配置缓存名称</param>
        /// <returns>配置项数据</returns>
        internal static object Get(Type type, string name)
        {
            Creator creator;
            return cache.TryGetValue(new HashKey<HashType, string>(type, name), out creator) ? creator.Create() : null;
        }

        /// <summary>
        /// 配置集合 [类型+名称]
        /// </summary>
        private static readonly Dictionary<HashKey<HashType, string>, Creator> cache = DictionaryCreator<HashKey<HashType, string>>.Create<Creator>();
        static Cache()
        {
            LeftArray<KeyValue<Type, Exception>> exceptionTypeArray = new LeftArray<KeyValue<Type, Exception>>(0);
            IRoot root = Common.Root;
            if (object.ReferenceEquals(root, Root.Null))
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                if(assembly!=null){
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IRoot).IsAssignableFrom(type))
                    {
                        try
                        {
                            root = (IRoot)Activator.CreateInstance(type);
                            break;
                        }
                        catch (Exception exception)
                        {
                            exceptionTypeArray.Add(new KeyValue<Type, Exception>(type, exception));
                        }
                    }
                }
                }
            }

            foreach (Type type in root.MainTypes.concat(root.PublicTypes))
            {
                foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    foreach (MemberAttribute attribute in field.GetCustomAttributes(typeof(MemberAttribute), false))
                    {
                        HashKey<HashType, string> key = new HashKey<HashType, string>(field.FieldType, attribute.GetCacheName(field.Name));
                        if (!cache.ContainsKey(key)) cache.Add(key, new Creator.Field(field));
                        break;
                    }
                }
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (property.CanRead)
                    {
                        MethodInfo method = property.GetGetMethod(true);
                        if (method != null && method.GetParameters().Length == 0)
                        {
                            foreach (MemberAttribute attribute in property.GetCustomAttributes(typeof(MemberAttribute), false))
                            {
                                HashKey<HashType, string> key = new HashKey<HashType, string>(property.PropertyType, attribute.GetCacheName(property.Name));
                                if (!cache.ContainsKey(key)) cache.Add(key, new Creator.Property(method));
                                break;
                            }
                        }
                    }
                }
            }

            if (exceptionTypeArray.Length != 0) root.OnLoadException(ref exceptionTypeArray);
        }
    }
}
