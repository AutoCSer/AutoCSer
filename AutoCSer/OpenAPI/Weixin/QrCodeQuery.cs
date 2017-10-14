using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 生成带参数的二维码
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct QrCodeQuery
    {
        /// <summary>
        /// 该二维码有效时间，以秒为单位。 最大不超过604800（即7天）
        /// </summary>
        public int expire_seconds;
        /// <summary>
        /// 二维码详细信息
        /// </summary>
        public QrCodeAction action_info;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.Json.SerializeCustom]
        private void toJson(AutoCSer.Json.Serializer serializer)
        {
            serializer.CustomWriteFirstName("action_name");
            if (expire_seconds == 0)
            {
                serializer.CustomSerialize(action_info.scene.scene_str == null ? "QR_LIMIT_SCENE" : "QR_LIMIT_STR_SCENE");
            }
            else
            {
                serializer.CustomSerialize("QR_SCENE");
                serializer.CustomWriteNextName("expire_seconds");
                serializer.CustomSerialize(expire_seconds);
            }
            serializer.CustomWriteNextName("action_info");
            serializer.CustomSerialize(action_info);
            serializer.CustomObjectEnd();
        }
    }
}
