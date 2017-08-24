using System;
using AutoCSer.Extension;
using System.Reflection;
using AutoCSer.CodeGenerator.Metadata;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 模板生成基类
    /// </summary>
    internal abstract class Generator
    {
        /// <summary>
        /// 自动安装参数
        /// </summary>
        public ProjectParameter AutoParameter;
        /// <summary>
        /// 程序集
        /// </summary>
        protected Assembly assembly;
        /// <summary>
        /// 类型
        /// </summary>
        internal ExtensionType Type;
        /// <summary>
        /// 类名称
        /// </summary>
        public string TypeName
        {
            get { return Type.TypeName; }
        }
        /// <summary>
        /// 生成的代码
        /// </summary>
        protected StringArray _code_ = new StringArray();
        /// <summary>
        /// 代码段
        /// </summary>
        protected Dictionary<string, string> _partCodes_ = DictionaryCreator.CreateOnly<string, string>();
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOut">是否输出代码</param>
        protected virtual void create(bool isOut)
        {
            throw new InvalidOperationException(GetType().fullName());
        }
        /// <summary>
        /// 类名称定义
        /// </summary>
        public string TypeNameDefinition
        {
            get
            {
                if (Type.Type == null) return null;
                return Type.Type.getAccessDefinition() + " " + NoAccessTypeNameDefinition;
            }
        }
        /// <summary>
        /// 类名称定义
        /// </summary>
        public string NoAccessTypeNameDefinition
        {
            get
            {
                if (Type.Type == null) return null;
                return "partial" + (Type.IsNull ? " class" : " struct") + " " + Type.TypeName;
            }
        }
        /// <summary>
        /// 代码生成语言
        /// </summary>
        private AutoCSer.CodeGenerator.CodeLanguage _language_;
        /// <summary>
        /// 类定义生成
        /// </summary>
        private TypeDefinition _definition_;
        /// <summary>
        /// 输出类定义开始段代码
        /// </summary>
        /// <param name="language">代码生成语言</param>
        /// <param name="isOutDefinition">是否输出类定义</param>
        /// <returns>类定义</returns>
        protected bool outStart(CodeLanguage language, bool isOutDefinition)
        {
            _definition_ = null;
            _language_ = language;
            if (isOutDefinition)
            {
                _code_.Length = 0;
                if (Coder.Add(GetType(), Type.Type))
                {
                    switch (_language_)
                    {
                        case CodeLanguage.JavaScript:
                        case CodeLanguage.TypeScript:
                            _definition_ = new JavaScriptTypeDefinition(Type);
                            break;
                        default: _definition_ = new CSharpTypeDefinition(Type, true, false); break;
                    }
                    _code_.Add(_definition_.Start);
                    return true;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 临时逻辑变量
        /// </summary>
        protected bool _if_;
        /// <summary>
        /// 当前循环索引
        /// </summary>
        protected int _loopIndex_;
        /// <summary>
        /// 当前循环数量
        /// </summary>
        protected int _loopCount_;
        /// <summary>
        /// 异步关键字
        /// </summary>
        public string Async
        {
            get { return "async"; }
        }
        ///// <summary>
        ///// 异步等待关键字
        ///// </summary>
        //public string Await
        //{
        //    get { return "await"; }
        //}
        /// <summary>
        /// 输出类定义结束段代码
        /// </summary>
        protected void outEnd()
        {
            _code_.Add(_definition_.End);
            switch (_language_)
            {
                case CodeLanguage.JavaScript:
                case CodeLanguage.TypeScript:
                    break;
                default:
                    Coder.Add(_code_.ToString(), _language_);
                    return;
            }
        }

        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <returns>null为0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int _getCount_<valueType>(ICollection<valueType> value)
        {
            return value != null ? value.Count : 0;
        }
    }
    /// <summary>
    /// 自定义属性模板生成基类
    /// </summary>
    /// <typeparam name="attributeType">自定义属性类型</typeparam>
    internal abstract class Generator<attributeType> : Generator, IGenerator
        where attributeType : Attribute
    {
        /// <summary>
        /// 自定义属性
        /// </summary>
        public attributeType Attribute;
        /// <summary>
        /// 是否搜索父类属性
        /// </summary>
        public virtual bool IsBaseType
        {
            get { return false; }
        }
        /// <summary>
        /// 是否必须配置自定义属性
        /// </summary>
        public virtual bool IsAttribute
        {
            get { return true; }
        }
        /// <summary>
        /// 安装入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否安装成功</returns>
        public virtual bool Run(ProjectParameter parameter)
        {
            if (parameter != null)
            {
                AutoParameter = parameter;
                assembly = parameter.Assembly;
                KeyValue<Type, attributeType>[] types = GetTypeAttributes();
                foreach (KeyValue<Type, attributeType> type in types)
                {
                    Type = type.Key;
                    Attribute = type.Value;
                    nextCreate();
                }
                onCreated();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取类型与自定义配置信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal KeyValue<Type, attributeType>[] GetTypeAttributes()
        {
            return AutoParameter.Types.getArray(type => new KeyValue<Type, attributeType>(type, AutoCSer.Metadata.TypeAttribute.GetAttribute<attributeType>(type, IsBaseType)))
                    .getFindArray(attribute => (attribute.Value != null || !IsAttribute) && !attribute.Key.IsDefined(typeof(AutoCSer.Metadata.IgnoreAttribute), IsBaseType));
        }
        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected abstract void nextCreate();
        /// <summary>
        /// 安装完成处理
        /// </summary>
        protected abstract void onCreated();
    }
}
