using System;
using AutoCSer.Extensions;

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
        [AutoCSer.JsonSerializeCustom]
        private unsafe void toJson(AutoCSer.JsonSerializer serializer)
        {
            if (card_ext.signature == null)
            {
                AutoCSer.LogHelper.Debug("卡券扩展 签名为空", LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
                serializer.CharStream.WriteJsonObject();
            }
            else
            {
                serializer.CustomWriteFirstName("card_id");
                serializer.CallSerialize(card_id);
                serializer.CustomWriteNextName("card_ext");
                serializer.CallSerialize(AutoCSer.JsonSerializer.Serialize(card_ext));
                serializer.CustomObjectEnd();
            }
        }
    }
}
