using System;
using System.Reflection;
using System.Reflection.Emit;
using AutoCSer.Extension;

namespace AutoCSer.Emit
{
    /// <summary>
    /// 创建参数类型
    /// </summary>
    public static class BuildParameterType
    {
        /// <summary>
        /// object 构造 函数信息
        /// </summary>
        private static readonly ConstructorInfo objectConstructorInfo = typeof(object).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, NullValue<Type>.Array, null);
        /// <summary>
        /// 参数类型编号
        /// </summary>
        private static int parametrTypeIndex;
        /// <summary>
        /// 创建参数类型
        /// </summary>
        /// <param name="builder">动态程序集模块</param>
        /// <param name="parameterInfoArray">参数数组</param>
        /// <param name="typeNamePrefix">生成参数类型名称前缀</param>
        /// <returns></returns>
        public static Type CreateParameterType(this ModuleBuilder builder, ParameterInfo[] parameterInfoArray, string typeNamePrefix)
        {
            TypeBuilder TypeBuilder = (builder ?? AutoCSer.Emit.Builder.Module.Builder).DefineType(typeNamePrefix + "+" + System.Threading.Interlocked.Increment(ref parametrTypeIndex).toString(), TypeAttributes.Class | TypeAttributes.Sealed, typeof(object), NullValue<Type>.Array);
            ConstructorBuilder ConstructorBuilder = TypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterInfoArray.getArray(p => p.ParameterType));
            ILGenerator ConstructorGenerator = ConstructorBuilder.GetILGenerator();
            #region 构造函数
            #region : base()
            ConstructorGenerator.Emit(OpCodes.Ldarg_0);
            ConstructorGenerator.Emit(OpCodes.Call, objectConstructorInfo);
            #endregion
            int ParameterIndex = 0;
            foreach (ParameterInfo ParameterInfo in parameterInfoArray)
            {
                ConstructorGenerator.Emit(OpCodes.Ldarg_0);
                ConstructorGenerator.ldarg(++ParameterIndex);
                ConstructorGenerator.Emit(OpCodes.Stfld, TypeBuilder.DefineField(ParameterInfo.Name, ParameterInfo.ParameterType, FieldAttributes.Public));
            }
            ConstructorGenerator.Emit(OpCodes.Ret);
            #endregion
            return TypeBuilder.CreateType();
        }
    }
}
