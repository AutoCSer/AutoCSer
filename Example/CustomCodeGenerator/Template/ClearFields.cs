using System;

namespace AutoCSer.CodeGenerator.Custom.Template
{
    class ClearFields : AutoCSer.CodeGenerator.Template.Pub
    {
        #region PART CLASS
        /*NOTE*/
        public partial class /*NOTE*/@TypeNameDefinition
        {
            /// <summary>
            /// 清除字段数据
            /// </summary>
            public void ClearFields()
            {
                ClearFields(/*NOTE*/(Type.FullName)(object)/*NOTE*/this);
            }
            /// <summary>
            /// 清除字段数据
            /// </summary>
            /// <param name="value">目标对象</param>
            public static void ClearFields(@Type.FullName value)
            {
                if (value != null)
                {
                    #region LOOP Members
                    value.@MemberName = default(@MemberType.FullName);
                    #endregion LOOP Members
                }
            }
        }
        #endregion PART CLASS
    }
}
