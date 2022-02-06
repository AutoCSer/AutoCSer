using System;
using System.Collections.Generic;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static unsafe partial class StringExtensionExpand
    {
        /// <summary>
        /// URL 编码小写转大写，用于匹配对接 Java
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static string encodeUrlToUpper(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                fixed (char* urlFixed = url) encodeUrlToUpper(urlFixed, urlFixed + url.Length, 0);
            }
            return url;
        }
        /// <summary>
        /// URL 编码小写转大写，用于匹配对接 Java
        /// </summary>
        /// <param name="read"></param>
        /// <param name="end"></param>
        /// <param name="index"></param>
        private static void encodeUrlToUpper(char* read, char* end, byte index)
        {
            do
            {
                switch (index)
                {
                    case 0:
                        if (*read == '%') index = 2;
                        break;
                    case 1:
                        if ((uint)*read - 'a' <= 'f' - 'a') *read -= (char)0x20;
                        index = 0;
                        break;
                    case 2:
                        if ((uint)*read - 'a' <= 'f' - 'a') *read -= (char)0x20;
                        index = 1;
                        break;
                }
            }
            while (++read != end);
        }
        /// <summary>
        /// URL 编码小写转大写，用于匹配对接 Java
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string EncodeUrlToUpper(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                fixed (char* urlFixed = url)
                {
                    byte index = 0;
                    char* read = urlFixed, end = urlFixed + url.Length;
                    do
                    {
                        switch (index)
                        {
                            case 0:
                                if (*read == '%') index = 2;
                                break;
                            case 1:
                            case 2:
                                if ((uint)*read - 'a' <= 'f' - 'a')
                                {
                                    string upperUrl = new string(urlFixed, 0, url.Length);
                                    fixed (char* upperUrlFixed = upperUrl)
                                    {
                                        encodeUrlToUpper(upperUrlFixed + (read - urlFixed), upperUrlFixed + upperUrl.Length, index);
                                    }
                                    return upperUrl;
                                }
                                break;
                        }
                    }
                    while (++read != end);
                }
            }
            return url;
        }
        /// <summary>
        /// 获取宽字符数量集合 https://www.cnblogs.com/sdflysha/p/20191026-split-string-to-character-list.html
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>宽字符数量</returns>
        public static IEnumerable<int> getWideCharSize(this string value)
        {
            if (value != null)
            {
                int size = 0, step = 0;
                foreach (char code in value)
                {
                    switch (step)
                    {
                        case 0:
                            if (char.IsHighSurrogate(code)) size = step = 1;
                            else yield return 1;
                            break;
                        case 1: size = step = 2; break;
                        case 2:
                            if (code != 0x200D)
                            {
                                yield return size;
                                if (char.IsHighSurrogate(code)) size = step = 1;
                                else
                                {
                                    yield return 1;
                                    step = size = 0;
                                }
                            }
                            else
                            {
                                ++size;
                                step = 3;
                            }
                            break;
                        case 3:
                            ++size;
                            step = 4;
                            break;
                        case 4:
                            ++size;
                            step = 2;
                            break;
                    }
                }
                if (size != 0) yield return size;
            }
        }
    }
}
