using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 场景值ID
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct SceneId
    {
        /// <summary>
        /// 场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）
        /// </summary>
        public uint scene_id;
        /// <summary>
        /// 场景值ID（字符串形式的ID），字符串类型，长度限制为1到64，仅永久二维码支持此字段
        /// </summary>
        public string scene_str;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.Json.SerializeCustom]
        private void toJson(AutoCSer.Json.Serializer serializer)
        {
            if (scene_str == null)
            {
                serializer.CustomWriteFirstName("scene_id");
                serializer.CustomSerialize(scene_id);
            }
            else
            {
                serializer.CustomWriteFirstName("scene_str");
                serializer.CustomSerialize(scene_str);
            }
            serializer.CustomObjectEnd();
        }
    }
}
