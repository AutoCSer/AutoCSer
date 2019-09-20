﻿using System;
using System.Runtime.InteropServices;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 一级菜单数组，个数应为1~3个
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Menu
    {
        /// <summary>
        /// 菜单标题，不超过16个字节
        /// </summary>
        public string name;
        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节(click等点击类型必须)
        /// </summary>
        public string key;
        /// <summary>
        /// 网页链接，用户点击菜单可打开链接，不超过256字节(view类型必须)
        /// </summary>
        public string url;
        /// <summary>
        /// 调用新增永久素材接口返回的合法media_id(media_id类型和view_limited类型必须)
        /// </summary>
        public string media_id;
        /// <summary>
        /// 对于不同的菜单类型，value的值意义不同
        /// 官网上设置的自定义菜单：Text:保存文字到value； Img、voice：保存mediaID到value； Video：保存视频下载链接到value； News：保存图文消息到news_info，同时保存mediaID到value； View：保存链接到url。
        /// 使用API设置的自定义菜单：click、scancode_push、scancode_waitmsg、pic_sysphoto、pic_photo_or_album、	pic_weixin、location_select：保存值到key；view：保存链接到url
        /// </summary>
        public string value;
        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        public SubMenu[] sub_button;
        /// <summary>
        /// 菜单的响应动作类型
        /// </summary>
        public MenuType type;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.Json.SerializeCustom]
        private void toJson(AutoCSer.Json.Serializer serializer)
        {
            serializer.CustomWriteFirstName("name");
            serializer.CallSerialize(name);
            if (type != MenuType.none)
            {
                serializer.CustomWriteNextName("type");
                serializer.TypeSerialize(ref type);
            }
            if (!string.IsNullOrEmpty(key))
            {
                serializer.CustomWriteNextName("key");
                serializer.CallSerialize(key);
            }
            if (!string.IsNullOrEmpty(url))
            {
                serializer.CustomWriteNextName("url");
                serializer.CallSerialize(url);
            }
            if (!string.IsNullOrEmpty(media_id))
            {
                serializer.CustomWriteNextName("media_id");
                serializer.CallSerialize(media_id);
            }
            if (sub_button.length() != 0)
            {
                serializer.CustomWriteNextName("sub_button");
                serializer.TypeSerialize(sub_button);
            }
            serializer.CustomObjectEnd();
        }
    }
}
