using System;
using System.Reflection;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.OpenAPI.Weixin.Emit
{
    /// <summary>
    /// 属性操作
    /// </summary>
    internal partial class Property
    {
#if NOJIT
        /// <summary>
        /// 创建设置属性委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="propertyType"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Action<valueType, propertyType> SetProperty<valueType, propertyType>(PropertyInfo property) where valueType : class
        {
            if (!property.CanWrite || property.ReflectedType != typeof(valueType) || !typeof(propertyType).IsAssignableFrom(property.PropertyType)) throw new InvalidOperationException();
            return new PropertySetter { SetMethod = property.GetSetMethod(true) }.Set<valueType, propertyType>;
        }
        /// <summary>
        /// 属性
        /// </summary>
        private sealed class PropertySetter
        {
            /// <summary>
            /// 属性
            /// </summary>
            public MethodInfo SetMethod;
            /// <summary>
            /// 设置字段
            /// </summary>
            /// <typeparam name="valueType"></typeparam>
            /// <typeparam name="propertyType"></typeparam>
            /// <param name="value"></param>
            /// <param name="propertyValue"></param>
            public void Set<valueType, propertyType>(valueType value, propertyType propertyValue)
            {
                SetMethod.Invoke(value, new object[] { propertyValue });
            }
        }
#else
        /// <summary>
        /// 创建设置属性委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="propertyType"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Action<valueType, propertyType> SetProperty<valueType, propertyType>(PropertyInfo property) where valueType : class
        {
            Type type = typeof(valueType);
            if (!property.CanWrite || property.ReflectedType != type || !typeof(propertyType).IsAssignableFrom(property.PropertyType)) throw new InvalidOperationException();
            DynamicMethod dynamicMethod = new DynamicMethod("Set_" + property.Name, null, new Type[] { type, typeof(propertyType) }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.call(property.GetSetMethod(true));
            generator.Emit(OpCodes.Ret);
            return (Action<valueType, propertyType>)dynamicMethod.CreateDelegate(typeof(Action<valueType, propertyType>));
        }
#endif
    }
}
