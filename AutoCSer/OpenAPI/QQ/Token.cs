using System;
using System.Runtime.InteropServices;
using AutoCSer.Extensions;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 访问令牌
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct Token
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string access_token;
        /// <summary>
        /// 有效期，单位为秒
        /// </summary>
        public int expires_in;
        /// <summary>
        /// 访问令牌是否有效
        /// </summary>
        public bool IsToken
        {
            get
            {
                return access_token != null && expires_in != 0;
            }
        }
        /// <summary>
        /// 获取用户身份的标识
        /// </summary>
        /// <returns>用户身份的标识</returns>
        public OpenId GetOpenId()
        {
            if (IsToken)
            {
                string json = Config.Client.RequestForm(@"https://graph.qq.com/oauth2.0/me?access_token=" + access_token);
                if (json != null)
                {
                    bool isError = false, isJson = false;
                    OpenId openId = new OpenId();
                    try
                    {
                        if (AutoCSer.JsonDeSerializer.DeSerialize(formatJson(json), ref openId)) isJson = true;
                    }
                    catch (Exception error)
                    {
                        isError = true;
                        AutoCSer.LogHelper.Exception(error, json, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    if (isJson && openId.openid != null) return openId;
                    if (!isError) AutoCSer.LogHelper.Debug(json, LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
                }
            }
            return default(OpenId);
        }
        /// <summary>
        /// 格式化json，去掉函数调用
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static SubString formatJson(string json)
        {
            int functionIndex = json.IndexOf('(');
            if (functionIndex != -1)
            {
                int objectIndex = json.IndexOf('{');
                if (objectIndex == -1)
                {
                    int arrayIndex = json.IndexOf('[');
                    if (arrayIndex != -1 && functionIndex < arrayIndex)
                    {
                        return new SubString(++functionIndex, json.LastIndexOf(')') - functionIndex, json);
                    }
                }
                else if (functionIndex < objectIndex)
                {
                    int arrayIndex = json.IndexOf('[');
                    if (arrayIndex == -1 || functionIndex < arrayIndex)
                    {
                        return new SubString(++functionIndex, json.LastIndexOf(')') - functionIndex, json);
                    }
                }
            }
            return json;
        }
    }
}
