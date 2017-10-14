using System;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Net.WebClient
{
    /// <summary>
    /// URI相关操作
    /// </summary>
    public static class UriCreator
    {
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static unsafe string Encode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                MemoryMap map = urlMap;
                fixed (char* valueFixed = value)
                {
                    char* start = valueFixed, end = valueFixed + value.Length;
                    int length = 0, isSpace = 0;
                    do
                    {
                        if (*((byte*)start + 1) == 0)
                        {
                            if (map.Get(*(byte*)start) == 0)
                            {
                                if (*start == ' ') isSpace = 1;
                                else length += 2;
                            }
                        }
                        else length += 5;
                    }
                    while (++start != end);
                    if ((length | isSpace) != 0)
                    {
                        string url = AutoCSer.Extension.StringExtension.FastAllocateString(value.Length + length);
                        fixed (char* urlFixed = url)
                        {
                            char* write = urlFixed;
                            start = valueFixed;
                            do
                            {
                                if (*((byte*)start + 1) == 0)
                                {
                                    if (map.Get(*(byte*)start) != 0) *write++ = *start;
                                    else if (*start == ' ') *write++ = '+';
                                    else
                                    {
                                        *write = '%';
                                        int code = *((byte*)start) >> 4;
                                        *(write + 1) = (char)(code + (code < 10 ? '0' : ('0' + 'A' - '9' - 1)));
                                        code = *((byte*)start) & 0xf;
                                        *(write + 2) = (char)(code + (code < 10 ? '0' : ('0' + 'A' - '9' - 1)));
                                        write += 3;
                                    }
                                }
                                else
                                {
                                    int code = *((byte*)start + 1) >> 4;
                                    *(int*)write = '%' + ('u' << 16);
                                    *(write + 2) = (char)(code + (code < 10 ? '0' : ('0' + 'A' - '9' - 1)));
                                    code = *((byte*)start + 1) & 0xf;
                                    *(write + 3) = (char)(code + (code < 10 ? '0' : ('0' + 'A' - '9' - 1)));
                                    code = *((byte*)start) >> 4;
                                    *(write + 4) = (char)(code + (code < 10 ? '0' : ('0' + 'A' - '9' - 1)));
                                    code = *((byte*)start) & 0xf;
                                    *(write + 5) = (char)(code + (code < 10 ? '0' : ('0' + 'A' - '9' - 1)));
                                    write += 6;
                                }
                            }
                            while (++start != end);
                        }
                        return url;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// URL编码字符位图
        /// </summary>
        private static readonly MemoryMap urlMap;
#if !NOJIT
        /// <summary>
        /// 创建URI并修复小数点结尾的BUG
        /// </summary>
        /// <param name="url">URI字符串</param>
        /// <returns>创建的URI</returns>
        public static Uri Create(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) && url[url.Length - 1] == '.')
            {
                setAbsolute(uri, uri.AbsoluteUri + ".", UriFlags.ShouldBeCompressed);
            }
            return uri;
        }
        /// <summary>
        /// 创建URI并修复绝对URI
        /// </summary>
        /// <param name="url">URI字符串</param>
        /// <param name="removeFlags">删除状态,比如 UriFlags.ShouldBeCompressed | UriFlags.E_QueryNotCanonical</param>
        /// <returns>创建的URI</returns>
        public static Uri CreateAbsolute(string url, UriFlags removeFlags)
        {//#uri.m_Info.Offset.Fragment = uri.m_Info.Offset.End
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) && uri.AbsoluteUri != url) setAbsolute(uri, url, removeFlags);
            return uri;
        }
        /// <summary>
        /// 修复绝对URI
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="absoluteUri">绝对URI</param>
        /// <param name="removeFlags">删除状态</param>
        private static void setAbsolute(Uri uri, string absoluteUri, UriFlags removeFlags)
        {
            setFlags(uri, setAbsoluteUri(uri, absoluteUri) & (ulong.MaxValue ^ (ulong)removeFlags));
        }

        /// <summary>
        /// Uri.m_Info.MoreInfo.AbsoluteUri
        /// </summary>
        private static readonly Func<Uri, string, ulong> setAbsoluteUri;
        /// <summary>
        /// Uri.m_Flags
        /// </summary>
        private static readonly Action<Uri, ulong> setFlags;
#endif
        unsafe static UriCreator()
        {
            urlMap = new MemoryMap(Unmanaged.GetStatic64(256 >> 3, true));
            urlMap.Set('0', 10);
            urlMap.Set('A', 26);
            urlMap.Set('a', 26);
            urlMap.Set('(');
            urlMap.Set(')');
            urlMap.Set('*');
            urlMap.Set('-');
            urlMap.Set('.');
            urlMap.Set('!');
            urlMap.Set('_');
#if !NOJIT
            Assembly uriAssembly = typeof(Uri).Assembly;
            FieldInfo flags = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            DynamicMethod dynamicMethod = new DynamicMethod("setAbsoluteUri", typeof(ulong), new Type[] { typeof(Uri), typeof(string) }, typeof(Uri), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, typeof(Uri).GetField("m_Info", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
            generator.Emit(OpCodes.Ldfld, uriAssembly.GetType("System.Uri+UriInfo").GetField("MoreInfo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, uriAssembly.GetType("System.Uri+MoreInfo").GetField("AbsoluteUri", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, flags);
            //generator.Emit(OpCodes.Ldarg_0);
            //generator.Emit(OpCodes.Ldarg_2);
            //generator.Emit(OpCodes.Stfld, typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
            generator.Emit(OpCodes.Ret);
            setAbsoluteUri = (Func<Uri, string, ulong>)dynamicMethod.CreateDelegate(typeof(Func<Uri, string, ulong>));

            setFlags = AutoCSer.Emit.Field.UnsafeSetField<Uri, ulong>("m_Flags");
#endif
        }
    }
}
