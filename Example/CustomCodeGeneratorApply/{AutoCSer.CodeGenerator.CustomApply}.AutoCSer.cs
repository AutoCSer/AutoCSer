//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CodeGenerator.CustomApply
{
        internal partial class Data
        {
            /// <summary>
            /// 清除字段数据
            /// </summary>
            public void ClearFields()
            {
                ClearFields(this);
            }
            /// <summary>
            /// 清除字段数据
            /// </summary>
            /// <param name="value">目标对象</param>
            public static void ClearFields(AutoCSer.CodeGenerator.CustomApply.Data value)
            {
                if (value != null)
                {
                    value.PublicField = default(int);
                    value.PrivateField = default(int);
                }
            }
        }
}
#endif