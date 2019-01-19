using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberMapDynamicMethod
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
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        public SerializeMemberMapDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod("MemberMapBinarySerializer", null, new Type[] { typeof(MemberMap), typeof(Serializer), type }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldSize field)
        {
            Label end = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_0, field.MemberIndex);
            generator.Emit(OpCodes.Brfalse_S, end);

            generator.Emit(OpCodes.Ldarg_1);
            if (isValueType) generator.Emit(OpCodes.Ldarga_S, 2);
            else generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Ldfld, field.Field);
            generator.call(Serializer.GetMemberMapSerializeMethod(field.Field.FieldType) ?? SerializeMethodCache.GetMember(field.Field.FieldType));

            generator.MarkLabel(end);
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <returns>成员转换委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
