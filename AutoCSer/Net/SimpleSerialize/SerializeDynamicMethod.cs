using System;
using AutoCSer.Extension;
using System.Reflection;
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
            generator.call(AutoCSer.Extension.EmitGenerator.UnmanagedStreamPrepLengthMethod);
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
                if (enumType == typeof(int)) method = unmanagedStreamUnsafeWriteIntMethod;
                else if (enumType == typeof(uint)) method = unmanagedStreamUnsafeWriteUIntMethod;
                else if (enumType == typeof(byte)) method = unmanagedStreamUnsafeWriteByteMethod;
                else if (enumType == typeof(ulong)) method = unmanagedStreamUnsafeWriteULongMethod;
                else if (enumType == typeof(ushort)) method = unmanagedStreamUnsafeWriteUShortMethod;
                else if (enumType == typeof(long)) method = unmanagedStreamUnsafeWriteLongMethod;
                else if (enumType == typeof(short)) method = unmanagedStreamUnsafeWriteShortMethod;
                else if (enumType == typeof(sbyte)) method = unmanagedStreamUnsafeWriteSByteMethod;
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
                generator.Emit(OpCodes.Dup);
                generator.Emit(OpCodes.Ldfld, AutoCSer.Extension.EmitGenerator.UnmanagedStreamBaseByteSizeField);
                generator.int32(fixedFillSize);
                generator.Emit(OpCodes.Add);
                generator.Emit(OpCodes.Stfld, AutoCSer.Extension.EmitGenerator.UnmanagedStreamBaseByteSizeField);
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

        /// <summary>
        /// 非托管内存数据流写数据函数信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamUnsafeWriteByteMethod;
        /// <summary>
        /// 非托管内存数据流写数据函数信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamUnsafeWriteSByteMethod;
        /// <summary>
        /// 非托管内存数据流写数据函数信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamUnsafeWriteShortMethod;
        /// <summary>
        /// 非托管内存数据流写数据函数信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamUnsafeWriteUShortMethod;
        /// <summary>
        /// 非托管内存数据流写数据函数信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamUnsafeWriteIntMethod;
        /// <summary>
        /// 非托管内存数据流写数据函数信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamUnsafeWriteUIntMethod;
        /// <summary>
        /// 非托管内存数据流写数据函数信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamUnsafeWriteLongMethod;
        /// <summary>
        /// 非托管内存数据流写数据函数信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamUnsafeWriteULongMethod;

        static SerializeDynamicMethod()
        {
            foreach (MethodInfo method in typeof(UnmanagedStream).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.Name == "UnsafeWrite")
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 1)
                    {
                        Type type = parameters[0].ParameterType;
                        if (type == typeof(int)) unmanagedStreamUnsafeWriteIntMethod = method;
                        else if (type == typeof(uint)) unmanagedStreamUnsafeWriteUIntMethod = method;
                        else if (type == typeof(byte)) unmanagedStreamUnsafeWriteByteMethod = method;
                        else if (type == typeof(ulong)) unmanagedStreamUnsafeWriteULongMethod = method;
                        else if (type == typeof(ushort)) unmanagedStreamUnsafeWriteUShortMethod = method;
                        else if (type == typeof(long)) unmanagedStreamUnsafeWriteLongMethod = method;
                        else if (type == typeof(short)) unmanagedStreamUnsafeWriteShortMethod = method;
                        else if (type == typeof(sbyte)) unmanagedStreamUnsafeWriteSByteMethod = method;
                    }
                }
            }
        }
    }
}
