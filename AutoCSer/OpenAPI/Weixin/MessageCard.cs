using System;
using System.Runtime.InteropServices;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 卡券扩展
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct MessageCard
    {
        /// <summary>
        /// 指定的卡券code码，只能被领一次。use_custom_code字段为true的卡券必须填写，非自定义code不必填写。
        /// </summary>
        public string code;
        /// <summary>
        /// 指定领取者的openid，只有该用户能领取。bind_openid字段为true的卡券必须填写，bind_openid字段为false不必填写。
        /// </summary>
        public string openid;
        /// <summary>
        /// 时间戳，商户生成从1970年1月1日00:00:00至今的秒数,即当前的时间,且最终需要转换为字符串形式;由商户生成后传入,不同添加请求的时间戳须动态生成，若重复将会导致领取失败！
        /// </summary>
        public string timestamp;
        /// <summary>
        /// 随机字符串，由开发者设置传入，加强签名的安全性。随机字符串，不长于32位。推荐使用大小写字母和数字，不同添加请求的nonce须动态生成，若重复将会导致领取失败！
        /// </summary>
        public string nonce_str;
        /// <summary>
        /// 签名，商户将接口列表中的参数按照指定方式进行签名,签名方式使用SHA1,具体签名方案参见下文;由商户按照规范签名后传入。
        /// </summary>
        public string signature;
        /// <summary>
        /// 卡券扩展http://mp.weixin.qq.com/wiki/7/aaa137b55fb2e0456bf8dd9148dd613f.html#.E9.99.84.E5.BD.954-.E5.8D.A1.E5.88.B8.E6.89.A9.E5.B1.95.E5.AD.97.E6.AE.B5.E5.8F.8A.E7.AD.BE.E5.90.8D.E7.94.9F.E6.88.90.E7.AE.97.E6.B3.95
        /// </summary>
        /// <param name="api_ticket"></param>
        /// <param name="card_id"></param>
        /// <param name="code"></param>
        /// <param name="openid"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce_str"></param>
        public MessageCard(string api_ticket, string card_id, string code, string openid, string timestamp, string nonce_str)
        {
            signature = getSignature(new string[] { api_ticket, card_id, this.code = code, this.openid = openid, this.timestamp = timestamp, this.nonce_str = nonce_str });
        }
        /// <summary>
        /// 卡券扩展
        /// </summary>
        /// <param name="api_ticket"></param>
        /// <param name="card_id"></param>
        public void SetSignature(string api_ticket, string card_id)
        {
            signature = getSignature(new string[] { api_ticket, card_id, code, openid, timestamp, nonce_str });
        }
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static string getSignature(string[] values)
        {
            Array.Sort(values, string.CompareOrdinal);
            return SHA.Sha1(Extension.String_Weixin.ConcatBytes(values)).toLowerHex();
        }
    }
}
