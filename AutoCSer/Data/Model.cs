using System;
using System.Reflection;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Data
{
    /// <summary>
    /// 数据模型
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal abstract class Model<valueType>
    {
#if !NOJIT
        /// <summary>
        /// 获取关键字获取器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="primaryKeys"></param>
        /// <returns></returns>
        internal static Func<valueType, keyType> GetPrimaryKeyGetter<keyType>(string name, FieldInfo[] primaryKeys)
        {
            if (primaryKeys.Length == 0) return null;
            DynamicMethod dynamicMethod = new DynamicMethod(name, typeof(keyType), new Type[] { typeof(valueType) }, typeof(valueType), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            if (primaryKeys.Length == 1)
            {
                if (primaryKeys[0].FieldType != typeof(keyType)) throw new InvalidCastException(typeof(keyType).fullName() + " != " + primaryKeys[0].FieldType.fullName());
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, primaryKeys[0]);
            }
            else
            {
                LocalBuilder key = generator.DeclareLocal(typeof(keyType));
                generator.Emit(OpCodes.Ldloca_S, key);
                generator.Emit(OpCodes.Initobj, typeof(keyType));
                foreach (FieldInfo primaryKey in primaryKeys)
                {
                    generator.Emit(OpCodes.Ldloca_S, key);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, primaryKey);
                    generator.Emit(OpCodes.Stfld, typeof(keyType).GetField(primaryKey.Name, BindingFlags.Instance | BindingFlags.Public));
                }
                generator.Emit(OpCodes.Ldloc_0);
            }
            generator.Emit(OpCodes.Ret);
            return (Func<valueType, keyType>)dynamicMethod.CreateDelegate(typeof(Func<valueType, keyType>));
        }
        /// <summary>
        /// 获取关键字设置器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="primaryKeys"></param>
        /// <returns></returns>
        internal static Action<valueType, keyType> GetPrimaryKeySetter<keyType>(string name, FieldInfo[] primaryKeys)
        {
            if (primaryKeys.Length == 0) return null;
            DynamicMethod dynamicMethod = new DynamicMethod(name, null, new Type[] { typeof(valueType), typeof(keyType) }, typeof(valueType), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            if (primaryKeys.Length == 1)
            {
                if (primaryKeys[0].FieldType != typeof(keyType)) throw new InvalidCastException(typeof(keyType).fullName() + " != " + primaryKeys[0].FieldType.fullName());
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Stfld, primaryKeys[0]);
            }
            else
            {
                foreach (FieldInfo primaryKey in primaryKeys)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarga_S, 1);
                    generator.Emit(OpCodes.Ldfld, typeof(keyType).GetField(primaryKey.Name, BindingFlags.Instance | BindingFlags.Public));
                    generator.Emit(OpCodes.Stfld, primaryKey);
                }
            }
            generator.Emit(OpCodes.Ret);
            return (Action<valueType, keyType>)dynamicMethod.CreateDelegate(typeof(Action<valueType, keyType>));
        }
        /// <summary>
        /// 获取自增字段获取器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        protected static Func<valueType, int> getIdentityGetter32(string name, FieldInfo field)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(name, typeof(int), new Type[] { typeof(valueType) }, typeof(valueType), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, field);
            if (field.FieldType != typeof(int) && field.FieldType != typeof(uint)) generator.Emit(OpCodes.Conv_I4);
            generator.Emit(OpCodes.Ret);
            return (Func<valueType, int>)dynamicMethod.CreateDelegate(typeof(Func<valueType, int>));
        }
#endif
    }
}
