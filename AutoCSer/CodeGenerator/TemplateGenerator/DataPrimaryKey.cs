using System;
using AutoCSer.Extension;
using AutoCSer.CodeGenerator.Metadata;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 数据关键字 代码生成
    /// </summary>
    internal abstract partial class DataPrimaryKey
    {
        /// <summary>
        /// 数据关键字 代码生成
        /// </summary>
        [Generator(Name = "数据关键字")]
        internal partial class Generator : MemberGenerator<AutoCSer.Data.PrimaryKeyAttribute>
        {
            /// <summary>
            /// 关键字成员集合
            /// </summary>
            internal MemberIndex[] PrimaryKeys;
            /// <summary>
            /// 第一个关键字成员
            /// </summary>
            internal MemberIndex PrimaryKey0
            {
                get { return PrimaryKeys[0]; }
            }
            /// <summary>
            /// 后续关键字成员集合
            /// </summary>
            internal MemberIndex[] NextPrimaryKeys
            {
                get { return PrimaryKeys.getSub(1, PrimaryKeys.Length - 1); }
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                LeftArray<KeyValue<MemberIndex, AutoCSer.Data.MemberAttribute>> members = new LeftArray<KeyValue<MemberIndex, AutoCSer.Data.MemberAttribute>>(PrimaryKeys.Length);
                foreach (MemberIndex member in PrimaryKeys)
                {
                    AutoCSer.Data.MemberAttribute attribute = member.GetSetupAttribute<AutoCSer.Data.MemberAttribute>(false);
                    members.Add(new KeyValue<MemberIndex, AutoCSer.Data.MemberAttribute>(member, attribute));
                }
                PrimaryKeys = members.GetSort(value => value.Value.PrimaryKeyIndex)
                    .getArray(value => value.Key);
                create(true);
            }
            /// <summary>
            /// 安装入口
            /// </summary>
            /// <param name="type">数据类型</param>
            /// <param name="primaryKeys">数据类型</param>
            public void Run(Type type, MemberIndex[] primaryKeys)
            {
                if (primaryKeys.Length > 1 && !Coder.CheckCodeType(typeof(Generator), type))
                {
                    Type = type;
                    Attribute = type.customAttribute<AutoCSer.Data.PrimaryKeyAttribute>() ?? AutoCSer.Data.PrimaryKeyAttribute.Default;
                    PrimaryKeys = primaryKeys;
                    nextCreate();
                }
            }
        }
    }
}
