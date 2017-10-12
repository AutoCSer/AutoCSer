using System;
using AutoCSer.Extension;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.NumberToCharStream
{
    /// <summary>
    /// 动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct JoinDynamicMethod
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
        /// 
        /// </summary>
        private Label indexLable;
        /// <summary>
        /// 
        /// </summary>
        private Label nextLable;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arrayType"></param>
        public JoinDynamicMethod(Type type, Type arrayType)
        {
            dynamicMethod = new DynamicMethod("NumberJoinChar", null, new Type[] { typeof(CharStream), arrayType, typeof(int), typeof(int), typeof(char), typeof(string) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            generator.DeclareLocal(typeof(int));

            indexLable = generator.DefineLabel();
            nextLable = generator.DefineLabel();
            Label toString = generator.DefineLabel();

            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Stloc_0);
            generator.Emit(OpCodes.Br_S, indexLable);

            generator.MarkLabel(nextLable);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Beq_S, toString);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_S, 4);
            generator.charStreamWriteChar();

            generator.MarkLabel(toString);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="type"></param>
        public void JoinChar(MethodInfo method, Type type)
        {
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldloc_0);
            if (type == typeof(int) || type == typeof(uint)) generator.Emit(OpCodes.Ldelem_I4);
            else if (type == typeof(byte) || type == typeof(sbyte)) generator.Emit(OpCodes.Ldelem_I1);
            else if (type == typeof(long) || type == typeof(ulong)) generator.Emit(OpCodes.Ldelem_I8);
            else if (type == typeof(short) || type == typeof(ushort)) generator.Emit(OpCodes.Ldelem_I2);
            else generator.Emit(OpCodes.Ldelem, type);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Call, method);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="type"></param>
        public void JoinCharNull(MethodInfo method, Type type)
        {
            Label writeNull = generator.DefineLabel(), end = generator.DefineLabel();
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ldelem, type);
            generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableHasValue(type));
            generator.Emit(OpCodes.Brfalse_S, writeNull);

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ldelema, type);
            generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableValue(type));
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Call, method);
            generator.Emit(OpCodes.Br_S, end);

            generator.MarkLabel(writeNull);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_S, 5);
            generator.charStreamWriteString();
            generator.MarkLabel(end);
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <returns>成员转换委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Add);
            generator.Emit(OpCodes.Stloc_0);

            generator.MarkLabel(indexLable);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ldarg_3);
            generator.Emit(OpCodes.Bne_Un_S, nextLable);

            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
