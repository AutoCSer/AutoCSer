using System;
using AutoCSer.Extensions;
using System.Reflection;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeDynamicMethod
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
        /// <param name="fixedSize"></param>
        public SerializeDynamicMethod(Type type, int fixedSize)
        {
            dynamicMethod = new DynamicMethod("SimpleSerializer", null, new Type[] { typeof(UnmanagedStream), type.MakeByRefType() }, type, true);
            generator = dynamicMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.int32(fixedSize);
            generator.call(AutoCSer.Extensions.EmitGenerator.UnmanagedStreamBasePrepSizeMethod);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(BinarySerialize.FieldSize field)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldfld, field.Field);
            Type type = field.Field.FieldType;
            MethodInfo method = null;
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(int)) method = ((Action<UnmanagedStream, int>)UnmanagedStream.UnsafeWrite).Method;
                else if (enumType == typeof(uint)) method = ((Action<UnmanagedStream, uint>)UnmanagedStream.UnsafeWrite).Method;
                else if (enumType == typeof(byte)) method = ((Action<UnmanagedStream, byte>)UnmanagedStream.UnsafeWrite).Method;
                else if (enumType == typeof(ulong)) method = ((Action<UnmanagedStream, ulong>)UnmanagedStream.UnsafeWrite).Method;
                else if (enumType == typeof(ushort)) method = ((Action<UnmanagedStream, ushort>)UnmanagedStream.UnsafeWrite).Method;
                else if (enumType == typeof(long)) method = ((Action<UnmanagedStream, long>)UnmanagedStream.UnsafeWrite).Method;
                else if (enumType == typeof(short)) method = ((Action<UnmanagedStream, short>)UnmanagedStream.UnsafeWrite).Method;
                else if (enumType == typeof(sbyte)) method = ((Action<UnmanagedStream, sbyte>)UnmanagedStream.UnsafeWrite).Method;
            }
            else method = Serializer.GetSerializeMethod(type);
            generator.call(method);
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
                generator.call(AutoCSer.Extensions.EmitGenerator.UnmanagedStreamBaseUnsafeMoveSizeMethod);
            }
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
