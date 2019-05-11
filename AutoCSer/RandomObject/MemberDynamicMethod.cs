using System;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MemberDynamicMethod
    {
        /// <summary>
        /// 动态函数
        /// </summary>
        private DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private ILGenerator generator;
        /// <summary>
        /// 数据类型
        /// </summary>
        private Type type;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        public MemberDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod("random", null, new Type[] { type.MakeByRefType(), typeof(Config) }, this.type = type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldInfo field)
        {
            generator.Emit(OpCodes.Ldarg_0);
            if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldarg_1);
            //generator.Emit(OpCodes.Call, MethodCache.CreateMethod.MakeGenericMethod(field.FieldType));
            generator.Emit(OpCodes.Call, AutoCSer.RandomObject.Metadata.GenericType.Get(field.FieldType).CreateMethod);
            generator.Emit(OpCodes.Stfld, field);
        }
        /// <summary>
        /// 基类调用
        /// </summary>
        public void Base()
        {
            if (!isValueType && (type = type.BaseType) != typeof(object))
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                //generator.Emit(OpCodes.Call, MethodCache.CreateMemberMethod.MakeGenericMethod(type));
                generator.Emit(OpCodes.Call, AutoCSer.RandomObject.Metadata.GenericType.Get(type).CreateMemberMethod);
            }
        }
        /// <summary>
        /// 创建委托
        /// </summary>
        /// <returns>委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
