using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Json
{
    /// <summary>
    /// 反序列化动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ParseDynamicMethod
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
        private LocalBuilder indexLocalBuilder;
        /// <summary>
        /// 
        /// </summary>
        private Label returnIndexLabel;
        /// <summary>
        /// 
        /// </summary>
        private Label returnErrorLabel;
        /// <summary>
        /// 
        /// </summary>
        private int index;
        /// <summary>
        /// 
        /// </summary>
        private bool isMemberMap;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private bool isValueType;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isMemberMap"></param>
        public ParseDynamicMethod(Type type, bool isMemberMap)
        {
            dynamicMethod = new DynamicMethod((this.isMemberMap = isMemberMap) ? "JsonMemberMapParser" : "JsonParser", typeof(int), isMemberMap ? new Type[] { typeof(Parser), type.MakeByRefType(), typeof(byte*), typeof(MemberMap) } : new Type[] { typeof(Parser), type.MakeByRefType(), typeof(byte*) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            indexLocalBuilder = generator.DeclareLocal(typeof(int));
            returnIndexLabel = generator.DefineLabel();
            returnErrorLabel = generator.DefineLabel();

            generator.int32(index = 0);
            generator.Emit(OpCodes.Stloc_0);

            isValueType = type.IsValueType;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        private void isName()
        {
            #region if ((names = parser.IsName(names, ref index)) != null)
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Ldloca_S, indexLocalBuilder);
            generator.call(ParseMethodCache.ParserIsNameMethod);
            generator.Emit(OpCodes.Dup);
            generator.Emit(OpCodes.Starg_S, 2);
            generator.int32(0);
            generator.Emit(OpCodes.Conv_U);
            generator.Emit(OpCodes.Beq, returnIndexLabel);
            #endregion
            #region if (index != -1)
            generator.Emit(OpCodes.Ldloc_0);
            generator.int32(-1);
            generator.Emit(OpCodes.Beq, returnErrorLabel);
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        private void nextIndex()
        {
            #region if (parser.State == ParseState.Success)
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, ParseMethodCache.ParseStateField);
            generator.Emit(OpCodes.Brtrue, returnErrorLabel);
            #endregion
            #region ++index;
            generator.int32(++index);
            generator.Emit(OpCodes.Stloc_0);
            #endregion
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(FieldIndex field)
        {
            isName();

            bool isCustom = false;
            MethodInfo method = ParseMethodCache.GetMemberMethodInfo(field.Member.FieldType, ref isCustom);
            if (isCustom)
            {
                generator.Emit(OpCodes.Ldarg_1);
                if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Ldflda, field.Member);
                generator.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Ldflda, field.Member);
            }
            generator.call(method);

            nextIndex();
            if (isMemberMap) generator.memberMapSetMember(OpCodes.Ldarg_3, field.MemberIndex);
        }
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="propertyMethod">函数信息</param>
        public void Push(PropertyIndex property, MethodInfo propertyMethod)
        {
            isName();

            Type memberType = property.Member.PropertyType;
            LocalBuilder loadMember = generator.DeclareLocal(memberType);
            generator.initobj(memberType, loadMember);
            bool isCustom = false;
            MethodInfo method = ParseMethodCache.GetMemberMethodInfo(memberType, ref isCustom);
            if (isCustom)
            {
                generator.Emit(OpCodes.Ldloca, loadMember);
                generator.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldloca, loadMember);
            }
            generator.call(method);

            generator.Emit(OpCodes.Ldarg_1);
            if (!isValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldloc, loadMember);
            generator.call(propertyMethod);

            nextIndex();
            if (isMemberMap) generator.memberMapSetMember(OpCodes.Ldarg_3, property.MemberIndex);
        }
        /// <summary>
        /// 创建成员转换委托
        /// </summary>
        /// <returns>成员转换委托</returns>
        public Delegate Create<delegateType>()
        {
            #region return index;
            generator.MarkLabel(returnIndexLabel);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ret);
            #endregion
            #region return -1;
            generator.MarkLabel(returnErrorLabel);
            generator.int32(-1);
            generator.Emit(OpCodes.Ret);
            #endregion
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
}
