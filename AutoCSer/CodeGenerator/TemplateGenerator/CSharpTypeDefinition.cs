using System;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// C# 类定义生成
    /// </summary>
    internal sealed class CSharpTypeDefinition : TypeDefinition
    {
        /// <summary>
        /// 类定义生成
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="isPartial"></param>
        /// <param name="isClass"></param>
        /// <param name="typeNamespace"></param>
        public CSharpTypeDefinition(Type type, bool isPartial, bool isClass, string typeNamespace = null)
        {
            create(type, isPartial, isClass, typeNamespace);
            end.Reverse();
        }
        /// <summary>
        /// 生成类定义
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="isPartial">是否部分定义</param>
        /// <param name="isClass">是否建立类定义</param>
        /// <param name="typeNamespace"></param>
        private void create(Type type, bool isPartial, bool isClass, string typeNamespace)
        {
            if (type.ReflectedType == null)
            {
                start.Add("namespace " + (typeNamespace ?? type.Namespace) + @"
{");
                end.Add(@"
}");
            }
            else
            {
                create(type.ReflectedType.IsGenericType ? type.ReflectedType.MakeGenericType(type.GetGenericArguments()) : type.ReflectedType, true, true, null);
            }
            if (isClass)
            {
                start.Add(@"
    " + type.getAccessDefinition()
          + (type.IsAbstract ? (type.IsSealed ? " static" : " abstract") : null)
          + (isPartial ? " partial" : null)
          + (type.IsInterface ? " interface" : " class")
          + " " + type.Name + (type.IsGenericType ? "<" + type.GetGenericArguments().joinString(", ", x => x.fullName()) + ">" : null) + @"
    {");
                end.Add(@"
    }");
            }
        }
    }
}
