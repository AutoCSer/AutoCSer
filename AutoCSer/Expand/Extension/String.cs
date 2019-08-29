using System;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static unsafe partial class String_Expand
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
    }
}
