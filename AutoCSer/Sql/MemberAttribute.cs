using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据列配置
    /// </summary>
    public class MemberAttribute : Data.MemberAttribute
    {
        /// <summary>
        /// 数据库成员信息空值
        /// </summary>
        internal static readonly MemberAttribute DefaultDataMember = new MemberAttribute();
        /// <summary>
        /// 是否数据库成员信息空值
        /// </summary>
        internal bool IsDefaultMember
        {
            get
            {
                return this == DefaultDataMember;
            }
        }
        /// <summary>
        /// 数据库字段类型
        /// </summary>
        public Type DataType;
        /// <summary>
        /// 枚举真实类型
        /// </summary>
        internal Type EnumType;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue;
        /// <summary>
        /// 新增字段时的计算子查询
        /// </summary>
        public string UpdateValue;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark;
        /// <summary>
        /// 字符串是否ASCII
        /// </summary>
        public bool IsAscii;
        /// <summary>
        /// 是否忽略字符串最大长度提示
        /// </summary>
        public bool IsIgnoreMaxStringLength;
        /// <summary>
        /// 是否生成成员索引
        /// </summary>
        public bool IsMemberIndex;
        /// <summary>
        /// 是否生成当前时间
        /// </summary>
        public bool IsNowTime;
        /// <summary>
        /// decimal 整数位数
        /// </summary>
        public byte DecimalIntegerSize;
        /// <summary>
        /// decimal 小数位数
        /// </summary>
        public byte DecimalSize;

        /// <summary>
        /// 获取数据库成员信息
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <returns>数据库成员信息</returns>
        internal static MemberAttribute Get(MemberIndexInfo member)
        {
            MemberAttribute value = member.GetAttribute<MemberAttribute>(true);
            if (value == null || value.DataType == null)
            {
                if (member.MemberSystemType.IsEnum)
                {
                    if (value == null) value = new MemberAttribute();
                    value.DataType = System.Enum.GetUnderlyingType(member.MemberSystemType);
                }
                else
                {
                    MemberAttribute sqlMember = TypeAttribute.GetAttribute<MemberAttribute>(member.MemberSystemType, false);
                    if (sqlMember != null && sqlMember.DataType != null)
                    {
                        if (value == null) value = new MemberAttribute();
                        value.DataType = sqlMember.DataType;
                        if (sqlMember.DataType.nullableType() != null) value.IsNull = true;
                    }
                }
            }
            else if (member.MemberSystemType.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(member.MemberSystemType);
                if (enumType != value.DataType) value.EnumType = enumType;
            }
            if (value == null)
            {
                Type nullableType = member.MemberSystemType.nullableType();
                if (nullableType == null)
                {
                    if (TypeAttribute.GetAttribute<ColumnAttribute>(member.MemberSystemType, false) == null)
                    {
                        Type dataType = member.MemberSystemType.formCSharpType().toCSharpType();
                        if (dataType != member.MemberSystemType)
                        {
                            value = new MemberAttribute();
                            value.DataType = dataType;
                        }
                    }
                }
                else
                {
                    value = new MemberAttribute();
                    value.IsNull = true;
                    Type dataType = nullableType.formCSharpType().toCSharpType();
                    if (dataType != nullableType) value.DataType = dataType.toNullableType();
                }
            }
            return value ?? DefaultDataMember;
        }
    }
}
