using System;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 卡券
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CardMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public string card_id;
        /// <summary>
        /// 卡券扩展
        /// </summary>
        public MessageCard card_ext;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.Json.SerializeCustom]
        private unsafe void toJson(AutoCSer.Json.Serializer serializer)
        {
            if (card_ext.signature == null)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, "卡券扩展 签名为空");
                serializer.CharStream.WriteJsonObject();
            }
            else
            {
                serializer.CustomWriteFirstName("card_id");
                serializer.CustomSerialize(card_id);
                serializer.CustomWriteNextName("card_ext");
                serializer.CustomSerialize(AutoCSer.Json.Serializer.Serialize(card_ext));
                serializer.CustomObjectEnd();
            }
        }
    }
}
