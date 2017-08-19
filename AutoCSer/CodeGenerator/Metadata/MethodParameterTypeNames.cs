using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 函数参数类型与名称集合关键字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MethodParameterTypeNames : IEquatable<MethodParameterTypeNames>
    {
        /// <summary>
        /// 类型集合
        /// </summary>
        public MethodParameter[] Parameters;
        /// <summary>
        /// 返回值类型
        /// </summary>
        public ExtensionType MethodReturnType;
        /// <summary>
        /// 哈希值
        /// </summary>
        private int hashCode;
        /// <summary>
        /// 参数序号
        /// </summary>
        public int Index;
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterTypeName
        {
            get { return MethodParameterTypes.GetParameterTypeName(Index); }
        }
        /// <summary>
        /// 二进制序列化是否需要检测循环引用
        /// </summary>
        public bool IsSerializeReferenceMember;
        /// <summary>
        /// 是否添加包装处理申明 AutoCSer.Metadata.BoxSerialize
        /// </summary>
        public bool IsSerializeBox;
        /// <summary>
        /// 参数是否有效
        /// </summary>
        public bool IsParameter
        {
            get { return Parameters.Length != 0 || MethodReturnType.Type != null; }
        }
        /// <summary>
        /// 函数参数类型集合关键字
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="isSerializeReferenceMember">二进制序列化是否需要检测循环引用</param>
        /// <param name="isSerializeBox">二进制序列化是否需要检测循环引用</param>
        public MethodParameterTypeNames(MethodParameter[] parameters, bool isSerializeReferenceMember, bool isSerializeBox)
        {
            Index = 0;
            MethodReturnType = (Type)null;
            hashCode = (IsSerializeReferenceMember = isSerializeReferenceMember) ^ (IsSerializeBox = isSerializeBox) ? 1 : 0;
            if (parameters.Length == 0) this.Parameters = parameters;
            else
            {
                this.Parameters = parameters.copy().sort(compare);
                foreach (MethodParameter parameter in parameters) hashCode ^= parameter.ParameterType.GetHashCode();
            }
        }
        /// <summary>
        /// 函数参数类型集合关键字
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="returnType"></param>
        /// <param name="isSerializeReferenceMember">二进制序列化是否需要检测循环引用</param>
        /// <param name="isSerializeBox">二进制序列化是否需要检测循环引用</param>
        public MethodParameterTypeNames(MethodParameter[] parameters, Type returnType, bool isSerializeReferenceMember, bool isSerializeBox)
        {
            Index = 0;
            hashCode = (this.MethodReturnType = returnType).GetHashCode();
            if ((IsSerializeReferenceMember = isSerializeReferenceMember) ^ (IsSerializeBox = isSerializeBox)) hashCode ^= 1;
            if (parameters.Length == 0) this.Parameters = parameters;
            else
            {
                this.Parameters = parameters.copy().sort(compare);
                foreach (MethodParameter parameter in parameters) hashCode ^= parameter.ParameterType.GetHashCode();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MethodParameterTypeNames other)
        {
            if (((hashCode ^ other.hashCode) | (Parameters.Length ^ other.Parameters.Length)) == 0 && MethodReturnType == other.MethodReturnType
                && IsSerializeReferenceMember == other.IsSerializeReferenceMember && IsSerializeBox == other.IsSerializeBox)
            {
                int index = 0;
                foreach (MethodParameter otherParameter in other.Parameters)
                {
                    MethodParameter parameter = Parameters[index++];
                    if (parameter.ParameterType != otherParameter.ParameterType) return false;
                    if (parameter.ParameterName != otherParameter.ParameterName) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((MethodParameterTypeNames)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashCode;
        }
        /// <summary>
        /// 复制参数集合
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Copy(int index)
        {
            Index = index;
            for (index = Parameters.Length; index != 0; )
            {
                --index;
                Parameters[index] = new MethodParameter(Parameters[index]);
            }
        }

        /// <summary>
        /// 比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(MethodParameter left, MethodParameter right)
        {
            int value = string.CompareOrdinal(left.ParameterName, right.ParameterName);
            return value == 0 ? string.CompareOrdinal(left.ParameterType.FullName, right.ParameterType.FullName) : value;
        }
    }
}
