using System;
using AutoCSer.Metadata;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 查询类型解析器
    /// </summary>
    /// <typeparam name="valueType">目标类型</typeparam>
    internal unsafe static partial class HeaderQueryTypeParser<valueType>
    {
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">查询解析器</param>
        /// <param name="value">目标数据</param>
        internal delegate void TryParse(HeaderQueryParser parser, ref valueType value);
        /// <summary>
        /// 成员解析器集合
        /// </summary>
        private static readonly TryParse[] memberParsers;
        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        private static readonly HeaderQueryParseStateSearcher memberSearcher;
        /// <summary>
        /// 默认顺序成员名称数据
        /// </summary>
        private static Pointer memberNames;
        //internal static int x(HeaderQueryParser parser, ref valueType value, byte* names)
        //{
        //    int index = 0;
        //    while ((names = parser.IsName(names, ref index)) != null)
        //    {
        //        if (index == -1) return -1;
        //        memberParsers[index](parser, ref value);
        //        ++index;
        //    }
        //    return index;
        //}
        /// <summary>
        /// 对象解析
        /// </summary>
        /// <param name="parser">查询解析器</param>
        /// <param name="value">目标数据</param>
        internal static void Parse(HeaderQueryParser parser, ref valueType value)
        {
            byte* names = memberNames.Byte;
            int index = 0;
            while ((names = parser.IsName(names, ref index)) != null)
            {
                if (index == -1) return;
                memberParsers[index](parser, ref value);
                ++index;
            }
            do
            {
                if ((index = memberSearcher.SearchName(parser)) != -1) memberParsers[index](parser, ref value);
            }
            while (parser.IsQuery());
        }
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Compile() { }

        static HeaderQueryTypeParser()
        {
            Type type = typeof(valueType);
            AutoCSer.Json.ParseAttribute attribute = TypeAttribute.GetAttribute<AutoCSer.Json.ParseAttribute>(type, true) ?? AutoCSer.Json.Parser.AllMemberAttribute;
            FieldIndex defaultMember = null;
            LeftArray<FieldIndex> fields = AutoCSer.Json.ParseMethodCache.GetFields(MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), attribute, ref defaultMember);
            LeftArray<KeyValue<PropertyIndex, MethodInfo>> properties = AutoCSer.Json.ParseMethodCache.GetProperties(MemberIndexGroup<valueType>.GetProperties(attribute.MemberFilters), attribute);
            memberParsers = new TryParse[fields.Length + properties.Length + (defaultMember == null ? 0 : 1)];
            string[] names = new string[memberParsers.Length];
            int index = 0, nameLength = 0, maxNameLength = 0;
            foreach (FieldIndex member in fields)
            {
#if NOJIT
                        TryParse tryParse = new HeaderQueryFieldParser(member.Member).Parser();
#else
                ILGenerator generator;
                DynamicMethod memberDynamicMethod = HeaderQueryParser.CreateDynamicMethod(type, member.Member.Name, member.Member.FieldType, out generator);
                generator.Emit(OpCodes.Stfld, member.Member);
                generator.Emit(OpCodes.Ret);
                TryParse tryParse = (TryParse)memberDynamicMethod.CreateDelegate(typeof(TryParse));
#endif
                memberParsers[index] = tryParse;
                if (member.Member.Name.Length > maxNameLength) maxNameLength = member.Member.Name.Length;
                nameLength += (names[index++] = member.Member.Name).Length;
                if (member == defaultMember)
                {
                    memberParsers[names.Length - 1] = tryParse;
                    names[names.Length - 1] = string.Empty;
                }
            }
            foreach (KeyValue<PropertyIndex, MethodInfo> member in properties)
            {
#if NOJIT
                        memberParsers[index] = new HeaderQueryPropertyParser(member.Key.Member).Parser();
#else
                ILGenerator generator;
                DynamicMethod memberDynamicMethod = HeaderQueryParser.CreateDynamicMethod(type, member.Key.Member.Name, member.Key.Member.PropertyType, out generator);
                generator.call(member.Value);
                generator.Emit(OpCodes.Ret);
                memberParsers[index] = (TryParse)memberDynamicMethod.CreateDelegate(typeof(TryParse));
#endif
                if (member.Key.Member.Name.Length > maxNameLength) maxNameLength = member.Key.Member.Name.Length;
                nameLength += (names[index++] = member.Key.Member.Name).Length;
            }
            if (maxNameLength > short.MaxValue || nameLength == 0) memberNames = Unmanaged.NullByte8;
            else
            {
                memberNames = new Pointer { Data = Unmanaged.GetStatic(nameLength + (names.Length - (defaultMember == null ? 0 : 1)) * sizeof(short) + sizeof(short), false) };
                byte* write = memberNames.Byte;
                foreach (string name in names)
                {
                    if (name.Length != 0)
                    {
                        *(short*)write = (short)name.Length;
                        fixed (char* nameFixed = name) StringExtension.WriteBytesNotNull(nameFixed, name.Length, write + sizeof(short));
                        write += sizeof(short) + name.Length;
                    }
                }
                *(short*)write = 0;
            }
            memberSearcher = new HeaderQueryParseStateSearcher(AutoCSer.Json.StateSearcher.GetMemberSearcher(type, names));
        }
    }
}