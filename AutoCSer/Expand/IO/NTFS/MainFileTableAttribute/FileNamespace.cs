using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 命名空间
    /// </summary>
    public enum FileNamespace : byte
    {
        /// <summary>
        /// 可以使用除NULL和分隔符“/”之外的所有UNICODE字符，最大可以使用255个字符。注意：“：”是合法字符，但Windows不允许使用。
        /// </summary>
        POSIX = 0,
        /// <summary>
        /// Win32是POSIX的一个子集，不区分大小写，可以使用除““”、“＊”、“?”、“：”、“/”、“小于”、“大于”、“/”、“|”之外的任意UNICODE字符，但名字不能以“.”或空格结尾。
        /// </summary>
        Win32 = 1,
        /// <summary>
        /// DOS命名空间是Win32的子集，只支持ASCII码大于空格的8BIT大写字符并且不支持以下字符““”、“＊”、“?”、“：”、“/”、“小于”、“大于”、“/”、“|”、“+”、“,”、“;”、“=”；同时名字必须按以下格式命名：1~8个字符，然后是“.”，然后再是1 ~3个字符。
        /// </summary>
        DOS = 2,
        /// <summary>
        /// 这个命名空间意味着Win32和DOS文件名都存放在同一个文件名属性中。
        /// </summary>
        Win32_DOS = 3,
    }
}
