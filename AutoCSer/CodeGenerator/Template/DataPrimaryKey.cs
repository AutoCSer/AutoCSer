using System;

namespace AutoCSer.CodeGenerator.Template
{
    class DataPrimaryKey : Pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            /// <summary>
            /// 关键字
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            public struct DataPrimaryKey : IEquatable<DataPrimaryKey>/*IF:Attribute.IsComparable*/, IComparable<DataPrimaryKey>/*IF:Attribute.IsComparable*/
            {
                #region LOOP PrimaryKeys
                #region IF XmlDocument
                /// <summary>
                /// @XmlDocument
                /// </summary>
                #endregion IF XmlDocument
                public @MemberType.FullName @MemberName;
                #endregion LOOP PrimaryKeys
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="other">关键字</param>
                /// <returns>是否相等</returns>
                public bool Equals(DataPrimaryKey other)
                {
                    return /*PUSH:PrimaryKey0*/@MemberName/**/.Equals(other.@MemberName)/*PUSH:PrimaryKey0*//*LOOP:NextPrimaryKeys*/ && @MemberName/**/.Equals(other.@MemberName)/*LOOP:NextPrimaryKeys*/;
                }
                /// <summary>
                /// 哈希编码
                /// </summary>
                /// <returns></returns>
                public override int GetHashCode()
                {
                    return /*PUSH:PrimaryKey0*/@MemberName/*PUSH:PrimaryKey0*/.GetHashCode()/*LOOP:NextPrimaryKeys*/ ^ @MemberName/**/.GetHashCode()/*LOOP:NextPrimaryKeys*/;
                }
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public override bool Equals(object obj)
                {
                    return Equals((DataPrimaryKey)obj);
                }
                #region IF Attribute.IsComparable
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="other"></param>
                /// <returns></returns>
                public int CompareTo(DataPrimaryKey other)
                {
                    int _value_ = /*PUSH:PrimaryKey0*/@MemberName/**/.CompareTo(other.@MemberName)/*PUSH:PrimaryKey0*/;
                    #region LOOP NextPrimaryKeys
                    if (_value_ == 0)
                    {
                        _value_ = @MemberName/**/.CompareTo(other.@MemberName);
                    #endregion LOOP NextPrimaryKeys
                        #region LOOP NextPrimaryKeys
                    }
                        #endregion LOOP NextPrimaryKeys
                    return _value_;
                }
                #endregion IF Attribute.IsComparable
            }
        }
        #endregion PART CLASS
    }
}
