using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 脚本安全属性名称
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct SafeAttributeName : IEquatable<SafeAttributeName>
    {
        /// <summary>
        /// 脚本安全属性名称
        /// </summary>
        public SubString Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">脚本安全属性名称</param>
        /// <returns>脚本安全属性名称唯一哈希</returns>
        public static implicit operator SafeAttributeName(string name) { return new SafeAttributeName { Name = name }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">脚本安全属性名称</param>
        /// <returns>脚本安全属性名称唯一哈希</returns>
        public static implicit operator SafeAttributeName(SubString name) { return new SafeAttributeName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            if (Name.Length < 3) return 0;
            int code = ((Name[Name.Length - 2] | 0x20) << 14) + ((Name[Name.Length >> 1] | 0x20) << 7) + (Name[Name.Length >> 3] | 0x20);
            return ((code >> 8) ^ (code >> 3) ^ (code >> 1)) & ((1 << 8) - 1);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(SafeAttributeName other)
        {
            return Name.EqualCase(ref other.Name);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj">待匹配数据</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((SafeAttributeName)obj);
        }
        /// <summary>
        /// 脚本安全属性名称集合
        /// </summary>
        public static readonly UniqueHashSet<SafeAttributeName> Names = new UniqueHashSet<SafeAttributeName>(new SafeAttributeName[] { "align", "allowtransparency", "alt", "behavior", "bgcolor", "border", "bordercolor", "bordercolordark", "bordercolorlight", "cellpadding", "cellspacing", "checked", "class", "clear", "color", "cols", "colspan", "controls", "coords", "direction", "face", "frame", "frameborder", "gutter", "height", "hspace", "loop", "loopdelay", "marginheight", "marginwidth", "maxlength", "method", "multiple", "rows", "rowspan", "rules", "scrollamount", "scrolldelay", "scrolling", "selected", "shape", "size", "span", "start", "target", "title", "type", "unselectable", "usemap", "valign", "value", "vspace", "width", "wrap" }, 253);
    }
}
