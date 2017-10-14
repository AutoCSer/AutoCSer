using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 用于设定图文消息的接收者
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct BulkMessageFilter
    {
        /// <summary>
        /// 群发到的分组的group_id，参加用户管理中用户分组接口，若is_to_all值为true，可不填写group_id
        /// </summary>
        public string group_id;
        /// <summary>
        /// 用于设定是否向全部用户发送，值为true或false，选择true该消息群发给所有用户，选择false可根据group_id发送给指定群组的用户
        /// </summary>
        public bool is_to_all;
    }
}
