using System;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeSerializeDynamicMethod
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
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        public DeSerializeDynamicMethod(Type type)
        {
            dynamicMethod = new DynamicMethod("SimpleDeSerializer", typeof(byte*), new Type[] { typeof(byte*), type.MakeByRefType(), typeof(byte*) }, type, true);
            generator = dynamicMethod.GetILGenerator();
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(BinarySerialize.FieldSize field)
        {
            Type type = field.Field.FieldType;
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                int size = sizeof(int);
                if (enumType == typeof(int) || enumType == typeof(uint)) size = sizeof(int);
                else if (enumType == typeof(byte) || enumType == typeof(sbyte)) size = sizeof(byte);
                else if (enumType == typeof(ulong) || enumType == typeof(long)) size = sizeof(long);
                else if (enumType == typeof(ushort) || enumType == typeof(short)) size = sizeof(short);

                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(size == sizeof(int) ? OpCodes.Ldind_I4 : (size == sizeof(byte) ? OpCodes.Ldind_I1 : (size == sizeof(long) ? OpCodes.Ldind_I8 : (OpCodes.Ldind_I2))));
                generator.Emit(OpCodes.Stfld, field.Field);

                generator.Emit(OpCodes.Ldarg_0);
                generator.int32(size);
                generator.Emit(OpCodes.Add);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldflda, field.Field);
                if (type == typeof(string)) generator.Emit(OpCodes.Ldarg_2);
                generator.call(DeSerializer.GetDeSerializeMethod(type));
            }
            generator.Emit(OpCodes.Starg_S, 0);
        }
        /// <summary>
        /// 填充对齐数据
        /// </summary>
        /// <param name="fixedFillSize"></param>
        public void FixedFill(int fixedFillSize)
        {
            if (fixedFillSize != 0)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.int32(fixedFillSize);
                generator.Emit(OpCodes.Add);
                generator.Emit(OpCodes.Starg_S, 0);
            }
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <returns>成员转换委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
