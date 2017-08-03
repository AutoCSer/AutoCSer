//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer.CodeGenerator.Custom
{
    internal partial class ClearFields
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected override void create(bool _isOut_)
        {
            if (outStart(AutoCSer.CodeGenerator.CodeLanguage.CSharp, _isOut_))
            {
                
            _code_.Add(@"
        ");
            _code_.Add(TypeNameDefinition);
            _code_.Add(@"
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
            /// <param name=""value"">目标对象</param>
            public static void ClearFields(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value1_ = Type;
                    if (_value1_ != null)
                    {
            _code_.Add(_value1_.FullName);
                    }
                }
            _code_.Add(@" value)
            {
                if (value != null)
                {");
                {
                    AutoCSer.CodeGenerator.Metadata.MemberIndex[] _value1_;
                    _value1_ = Members;
                    if (_value1_ != null)
                    {
                        int _loopIndex1_ = _loopIndex_, _loopCount1_ = _loopCount_;
                        _loopIndex_ = 0;
                        _loopCount_ = _value1_.Length;
                        foreach (AutoCSer.CodeGenerator.Metadata.MemberIndex _value2_ in _value1_)
                        {
            _code_.Add(@"
                    value.");
            _code_.Add(_value2_.MemberName);
            _code_.Add(@" = default(");
                {
                    AutoCSer.CodeGenerator.Metadata.ExtensionType _value3_ = _value2_.MemberType;
                    if (_value3_ != null)
                    {
            _code_.Add(_value3_.FullName);
                    }
                }
            _code_.Add(@");");
                            ++_loopIndex_;
                        }
                        _loopIndex_ = _loopIndex1_;
                        _loopCount_ = _loopCount1_;
                    }
                }
            _code_.Add(@"
                }
            }
        }");
                if (_isOut_) outEnd();
            }
        }
    }
}
#endif