using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// API调用http://mp.weixin.qq.com/wiki/6/01405db0092f76bb96b12a9f954cd866.html
    /// </summary>
    public sealed class API
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        private DateTime timeout;
        /// <summary>
        /// 应用配置
        /// </summary>
        internal readonly Config config;
        /// <summary>
        /// 访问令牌获取
        /// </summary>
        private readonly Func<AutoCSer.Net.TcpServer.ReturnValue<KeyValue<string, DateTime>>> getter;
        /// <summary>
        /// 重置访问令牌
        /// </summary>
        private readonly Func<string, AutoCSer.Net.TcpServer.ReturnValue<KeyValue<string, DateTime>>> reset;
        /// <summary>
        /// 访问令牌更新锁
        /// </summary>
        private readonly object getTokenLock;
        /// <summary>
        /// 访问令牌
        /// </summary>
        private Token token;
        /// <summary>
        /// 访问令牌锁
        /// </summary>
        private volatile int tokenLock;
        /// <summary>
        /// API调用
        /// </summary>
        /// <param name="config">应用配置</param>
        public API(Config config = null)
        {
            this.config = config ?? Config.Default;
            getTokenLock = new object();
        }
        /// <summary>
        /// API调用
        /// </summary>
        /// <param name="getter">访问令牌获取</param>
        /// <param name="reset">重置令牌委托</param>
        /// <param name="config">应用配置</param>
        public API(Func<AutoCSer.Net.TcpServer.ReturnValue<KeyValue<string, DateTime>>> getter
            , Func<string, AutoCSer.Net.TcpServer.ReturnValue<KeyValue<string, DateTime>>> reset, Config config = null)
        {
            if (getter == null) throw new ArgumentNullException("getter is null");
            if (reset == null) throw new ArgumentNullException("reset is null");
            this.config = config ?? Config.Default;
            this.getter = getter;
            this.reset = reset;
        }
        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private Token checkToken(ref DateTime timeout)
        {
            Token value;
            while (Interlocked.CompareExchange(ref tokenLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
            if (this.timeout > Date.NowTime.Now)
            {
                value = token;
                timeout = this.timeout;
                System.Threading.Interlocked.Exchange(ref tokenLock, 0);
            }
            else
            {
                System.Threading.Interlocked.Exchange(ref tokenLock, 0);
                value = null;
            }
            return value;
        }
        /// <summary>
        /// 设置访问令牌
        /// </summary>
        /// <param name="tokenTime"></param>
        /// <returns></returns>
        private Token setToken(KeyValue<string, DateTime> tokenTime)
        {
            if (tokenTime.Value > Date.NowTime.Now)
            {
                Token value = this.token;
                if (value == null)
                {
                    value = new Token();
                    while (Interlocked.CompareExchange(ref tokenLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
                    value.access_token = tokenTime.Key;
                    this.timeout = tokenTime.Value;
                    this.token = value;
                    System.Threading.Interlocked.Exchange(ref tokenLock, 0);
                }
                else
                {
                    while (Interlocked.CompareExchange(ref tokenLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
                    value.access_token = tokenTime.Key;
                    this.timeout = tokenTime.Value;
                    System.Threading.Interlocked.Exchange(ref tokenLock, 0);
                }
                return value;
            }
            return null;
        }
        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private Token getTokenWithLock(ref DateTime timeout)
        {
            Token value;
            Monitor.Enter(getTokenLock);
            try
            {
                if ((value = checkToken(ref timeout)) != null) return value;
                if ((value = config.GetToken()) != null && value.IsReturn && (timeout = Date.NowTime.Now.AddSeconds(value.expires_in - 60)) > Date.NowTime.Now)
                {
                    while (Interlocked.CompareExchange(ref tokenLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
                    token = value;
                    this.timeout = timeout;
                    System.Threading.Interlocked.Exchange(ref tokenLock, 0);
                    return value;
                }
            }
            finally { Monitor.Exit(getTokenLock); }
            if (value != null) AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, "访问令牌获取失败 " + value.Message);
            return null;
        }
        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <returns></returns>
        public KeyValue<string, DateTime> GetToken()
        {
            if (getter != null) throw new InvalidOperationException();
            DateTime timeout = DateTime.MinValue;
            Token token = checkToken(ref timeout) ?? getTokenWithLock(ref timeout);
            return token == null ? default(KeyValue<string, DateTime>) : new KeyValue<string, DateTime>(token.access_token, timeout);
        }
        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <returns></returns>
        private string getToken()
        {
            DateTime timeout = DateTime.MinValue;
            Token token = checkToken(ref timeout) ?? (getter == null ? getTokenWithLock(ref timeout) : setToken(getter()));
            return token == null ? null : token.access_token;
        }
        /// <summary>
        /// 重置访问令牌
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private string checkToken(string token, ref DateTime timeout)
        {
            while (Interlocked.CompareExchange(ref tokenLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield();
            if (this.timeout > Date.NowTime.Now)
            {
                if (token != this.token.access_token)
                {
                    token = this.token.access_token;
                    timeout = this.timeout;
                    System.Threading.Interlocked.Exchange(ref tokenLock, 0);
                    return token;
                }
                this.timeout = DateTime.MinValue;
            }
            System.Threading.Interlocked.Exchange(ref tokenLock, 0);
            return null;
        }
        /// <summary>
        /// 重置访问令牌
        /// </summary>
        /// <param name="token">访问令牌</param>
        /// <returns></returns>
        private string resetToken(string token)
        {
            DateTime timeout = DateTime.MinValue;
            string newToken = checkToken(token, ref timeout);
            if (newToken != null) return newToken;
            Token value = reset == null ? getTokenWithLock(ref timeout) : setToken(reset(token));
            return value == null ? null : value.access_token;
        }
        /// <summary>
        /// 重置访问令牌
        /// </summary>
        /// <param name="token">访问令牌</param>
        /// <returns></returns>
        private KeyValue<string, DateTime> ResetToken(string token)
        {
            if (getter != null) throw new InvalidOperationException();
            DateTime timeout = DateTime.MinValue;
            string newToken = checkToken(token, ref timeout);
            if (newToken != null) return new KeyValue<string, DateTime>(newToken, timeout);
            Token value = getTokenWithLock(ref timeout);
            return value == null ? default(KeyValue<string, DateTime>) : new KeyValue<string, DateTime>(value.access_token, timeout);
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="urlPrefix">请求地址</param>
        /// <returns>数据对象,失败放回null</returns>
        private valueType request<valueType>(string urlPrefix) where valueType : Return
        {
            string token = getToken();
            if (token != null)
            {
                string url = urlPrefix + token;
                valueType value = Config.Client.Request<valueType>(url);
                if (value == null)
                {
                    if ((token = getToken()) != null)
                    {
                        value = Config.Client.Request<valueType>(urlPrefix + token);
                        if (value != null && value.IsReturn) return value;
                    }
                }
                else
                {
                    if (value.IsReturn) return value;
                    if (value.IsBusy)
                    {
                        valueType newValue = Config.Client.Request<valueType>(url);
                        if (newValue != null && newValue.IsReturn) return newValue;
                    }
                    else if (value.IsTokenError || value.IsTokenExpired)
                    {
                        if ((token = resetToken(token)) != null)
                        {
                            valueType newValue = Config.Client.Request<valueType>(urlPrefix + token);
                            if (newValue != null && newValue.IsReturn) return newValue;
                        }
                    }
                }
                if (value != null) AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, "API " + url + " 请求失败 " + value.Message);
            }
            return null;
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="urlPrefix">请求地址</param>
        /// <param name="form">表单数据</param>
        /// <returns>数据对象,失败放回null</returns>
        private valueType requestJson<valueType, formType>(string urlPrefix, formType form) where valueType : Return
        {
            string token = getToken();
            if (token != null)
            {
                string url = urlPrefix + token;
                valueType value = Config.Client.RequestJson<valueType, formType>(url, form);
                if (value == null)
                {
                    if ((token = getToken()) != null)
                    {
                        value = Config.Client.RequestJson<valueType, formType>(urlPrefix + token, form);
                        if (value != null && value.IsReturn) return value;
                    }
                }
                else
                {
                    if (value.IsReturn) return value;
                    if (value.IsBusy)
                    {
                        valueType newValue = Config.Client.RequestJson<valueType, formType>(url, form);
                        if (newValue != null && newValue.IsReturn) return newValue;
                    }
                    else if (value.IsTokenError || value.IsTokenExpired)
                    {
                        if ((token = resetToken(token)) != null)
                        {
                            valueType newValue = Config.Client.RequestJson<valueType, formType>(urlPrefix + token, form);
                            if (newValue != null && newValue.IsReturn) return newValue;
                        }
                    }
                }
                if (value != null) AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, "API " + url + " 请求失败 " + value.Message);
            }
            return null;
        }
        /// <summary>
        /// API请求json数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="urlPrefix">请求地址</param>
        /// <param name="data">文件数据</param>
        /// <param name="filename">文件名称</param>
        /// <param name="extensionName">扩展名称</param>
        /// <param name="contentType">输出内容类型</param>
        /// <param name="form">表单数据</param>
        /// <returns>数据对象,失败放回null</returns>
        private valueType requestFile<valueType>(string urlPrefix, byte[] data, string filename = "media", string extensionName = null, byte[] contentType = null, KeyValue<byte[], byte[]>[] form = null) where valueType : Return
        {
            string token = getToken();
            if (token != null)
            {
                string url = urlPrefix + token;
                if (!string.IsNullOrEmpty(extensionName)) filename += "." + extensionName;
                valueType value = Config.Client.RequestJson<valueType>(url, data, filename, contentType, form);
                if (value == null)
                {
                    if ((token = getToken()) != null)
                    {
                        value = Config.Client.RequestJson<valueType>(urlPrefix + token, data, filename, contentType, form);
                        if (value != null && value.IsReturn) return value;
                    }
                }
                else
                {
                    if (value.IsReturn) return value;
                    if (value.IsBusy)
                    {
                        valueType newValue = Config.Client.RequestJson<valueType>(url, data, filename, contentType, form);
                        if (newValue != null && newValue.IsReturn) return newValue;
                    }
                    else if (value.IsTokenError || value.IsTokenExpired)
                    {
                        if ((token = resetToken(token)) != null)
                        {
                            valueType newValue = Config.Client.RequestJson<valueType>(urlPrefix + token, data, filename, contentType, form);
                            if (newValue != null && newValue.IsReturn) return newValue;
                        }
                    }
                }
                if (value != null) AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, "API " + url + " 请求失败 " + value.Message);
            }
            return null;
        }
        /// <summary>
        /// API请求
        /// </summary>
        /// <typeparam name="formType">表单数据类型</typeparam>
        /// <param name="urlPrefix">请求地址</param>
        /// <param name="form">表单数据</param>
        /// <returns>数据对象,失败放回null</returns>
        private byte[] downloadJson<formType>(string urlPrefix, formType form)
        {
            string token = getToken();
            if (token != null)
            {
                byte[] data = Config.Client.DownloadJson<formType>(urlPrefix + token, form);
                if (data != null) return data;
                if ((token = getToken()) != null) return Config.Client.DownloadJson<formType>(urlPrefix + token, form);
            }
            return null;
        }
        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="urlPrefix">请求地址</param>
        /// <returns>数据对象,失败放回null</returns>
        private byte[] downloadIsValue(string urlPrefix)
        {
            string token = getToken();
            if (token != null)
            {
                string url = urlPrefix + token;
                byte[] data = Config.Client.Download(url);
                Return value = null;
                if (data == null)
                {
                    if ((token = getToken()) != null && (data = Config.Client.Download(urlPrefix + token)) != null && (value = checkMediaData(data)) == null)
                    {
                        return data;
                    }
                }
                else if ((value = checkMediaData(data)) == null) return data;
                else if (value.IsBusy)
                {
                    if ((data = Config.Client.Download(url)) != null && checkMediaData(data) == null) return data;
                }
                else if (value.IsTokenError || value.IsTokenExpired)
                {
                    if ((token = resetToken(token)) != null && (data = Config.Client.Download(urlPrefix + token)) != null && checkMediaData(data) == null) return data;
                }
                if (value != null) AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, "API " + url + " 请求失败 " + value.Message);
            }
            return null;
        }
        /// <summary>
        /// 检测媒体文件数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static unsafe Return checkMediaData(byte[] data)
        {
            if (data.Length <= 256)
            {
                fixed (byte* dataFixed = data)
                {
                    if (*dataFixed != '{') return null;
                    byte* start = dataFixed, end = dataFixed + data.Length;
                    for (byte* end32 = end - (data.Length & 3); start != end32; start += sizeof(uint))
                    {
                        if ((*(uint*)start & 0x80808080U) != 0) return null;
                    }
                    while (start != end)
                    {
                        if ((*start & 0x80) != 0) return null;
                    }
                    return AutoCSer.Json.Parser.Parse<Return>(Memory_WebClient.BytesToStringNotEmpty(dataFixed, data.Length));
                }
            }
            return null;
        }
        /// <summary>
        /// 获取微信服务器IP地址
        /// </summary>
        /// <returns></returns>
        public string[] GetCallbackIP()
        {
            CallbakIP value = request<CallbakIP>("https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token=");
            return value == null ? null : value.ip_list;
        }
        /// <summary>
        /// 添加客服账号(必须先在公众平台官网为公众号设置微信号后才能使用该能力,每个公众号最多添加10个客服账号)
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool AddAccount(Account account)
        {
            if (account != null)
            {
                Return value = requestJson<Return, Account>("https://api.weixin.qq.com/customservice/kfaccount/add?access_token=", account);
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 获取在线客服接待信息
        /// </summary>
        /// <returns></returns>
        public OnlineAccount[] GetOnlineAccount()
        {
            OnlineAccountResult value = request<OnlineAccountResult>("https://api.weixin.qq.com/cgi-bin/customservice/getonlinekflist?access_token=");
            return value == null ? null : value.kf_online_list;
        }
        /// <summary>
        /// 修改客服帐号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool UpdateAccount(Account account)
        {
            if (account != null)
            {
                Return value = requestJson<Return, Account>("https://api.weixin.qq.com/customservice/kfaccount/update?access_token=", account);
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 删除客服帐号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool DeleteAccount(Account account)
        {
            if (account != null)
            {
                Return value = requestJson<Return, Account>("https://api.weixin.qq.com/customservice/kfaccount/del?access_token=", account);
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 上传客服帐号头像
        /// </summary>
        /// <param name="account"></param>
        /// <param name="data"></param>
        /// <param name="extensionName">扩展名称</param>
        /// <param name="contentType">输出内容类型</param>
        /// <returns></returns>
        public bool UploadAccountImage(string account, byte[] data, string extensionName = null, byte[] contentType = null)
        {
            if (!string.IsNullOrEmpty(account) && data.length() != 0)
            {
                Return value = requestFile<Return>("http://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?kf_account=" + account + "&access_token=", data, "media", extensionName, contentType);
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 获取所有客服账号
        /// </summary>
        /// <returns></returns>
        public AccountListItem[] GetAccountList()
        {
            AccountList value = request<AccountList>("https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token=");
            return value == null ? null : value.kf_list;
        }
        /// <summary>
        /// 发客服消息
        /// </summary>
        /// <param name="message">客服消息</param>
        /// <returns></returns>
        public bool SendMessage(Message message)
        {
            if (message != null)
            {
                Return value = requestJson<Return, Message>("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=", message);
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 发客服 文本消息
        /// </summary>
        /// <param name="message">客服消息</param>
        /// <param name="content">文本消息内容</param>
        /// <returns></returns>
        public bool SendText(Message message, string content)
        {
            if (message != null && !string.IsNullOrEmpty(content))
            {
                message.msgtype = MessageType.text;
                message.text.content = content;
                return SendMessage(message);
            }
            return false;
        }
        /// <summary>
        /// 发客服 图片消息
        /// </summary>
        /// <param name="message">客服消息</param>
        /// <param name="media_id">媒体ID</param>
        /// <returns></returns>
        public bool SendImage(Message message, string media_id)
        {
            if (message != null && !string.IsNullOrEmpty(media_id))
            {
                message.msgtype = MessageType.image;
                message.image.media_id = media_id;
                return SendMessage(message);
            }
            return false;
        }
        /// <summary>
        /// 发客服 语音消息
        /// </summary>
        /// <param name="message">客服消息</param>
        /// <param name="media_id">媒体ID</param>
        /// <returns></returns>
        public bool SendVoice(Message message, string media_id)
        {
            if (message != null && !string.IsNullOrEmpty(media_id))
            {
                message.msgtype = MessageType.voice;
                message.voice.media_id = media_id;
                return SendMessage(message);
            }
            return false;
        }
        /// <summary>
        /// 发客服 视频消息
        /// </summary>
        /// <param name="message">客服消息</param>
        /// <param name="video">视频消息</param>
        /// <returns></returns>
        public bool SendVideo(Message message, VideoMessage video)
        {
            if (message != null)
            {
                message.msgtype = MessageType.video;
                message.video = video;
                return SendMessage(message);
            }
            return false;
        }
        /// <summary>
        /// 发客服 音乐消息
        /// </summary>
        /// <param name="message">客服消息</param>
        /// <param name="music">音乐消息</param>
        /// <returns></returns>
        public bool SendMusic(Message message, MusicMessage music)
        {
            if (message != null)
            {
                message.msgtype = MessageType.music;
                message.music = music;
                return SendMessage(message);
            }
            return false;
        }
        /// <summary>
        /// 发客服 图文消息
        /// </summary>
        /// <param name="message">客服消息</param>
        /// <param name="articles">图文消息</param>
        /// <returns></returns>
        public bool SendNews(Message message, ArticleMessage[] articles)
        {
            if (message != null && articles.length() != 0)
            {
                message.msgtype = MessageType.news;
                message.news.articles = articles;
                return SendMessage(message);
            }
            return false;
        }
        /// <summary>
        /// 发客服 卡券
        /// </summary>
        /// <param name="message">客服消息</param>
        /// <param name="wxcard">卡券</param>
        /// <returns></returns>
        public bool SendCard(Message message, CardMessage wxcard)
        {
            if (message != null)
            {
                message.msgtype = MessageType.wxcard;
                message.wxcard = wxcard;
                return SendMessage(message);
            }
            return false;
        }
        /// <summary>
        /// 发客服 卡券
        /// </summary>
        /// <param name="message"></param>
        /// <param name="card_id"></param>
        /// <param name="api_ticket"></param>
        /// <param name="card_ext"></param>
        /// <returns></returns>
        public bool SendCard(Message message, string card_id, string api_ticket, MessageCard card_ext)
        {
            if (message != null && !string.IsNullOrEmpty(card_id))
            {
                card_ext.SetSignature(card_id, api_ticket);
                message.msgtype = MessageType.wxcard;
                message.wxcard.card_id = card_id;
                message.wxcard.card_ext = card_ext;
                return SendMessage(message);
            }
            return false;
        }
        /// <summary>
        /// 上传图文消息内的图片获取URL。图片仅支持jpg/png格式，大小必须在1MB以下。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="extensionName"></param>
        /// <param name="contentType">输出内容类型</param>
        /// <returns>图片URL</returns>
        public string UploadNewsImage(byte[] data, string extensionName = null, byte[] contentType = null)
        {
            if (data.length() != 0)
            {
                UrlResult value = requestFile<UrlResult>("https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=", data, "media", extensionName, contentType);
                if (value != null) return value.url;
            }
            return null;
        }
        /// <summary>
        /// 上传图文消息素材
        /// </summary>
        /// <param name="articles">支持1到10条图文</param>
        /// <returns></returns>
        public Media UploadNews(Article[] articles)
        {
            if ((uint)(articles.length() - 1) < 10)
            {
                return requestJson<Media, UploadArticle>("https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=", new UploadArticle { articles = articles });
            }
            return null;
        }
        /// <summary>
        /// 视频转换
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public Media UploadVideo(SendVideo video)
        {
            if (!string.IsNullOrEmpty(video.media_id))
            {
                return requestJson<Media, SendVideo>("https://file.api.weixin.qq.com/cgi-bin/media/uploadvideo?access_token=", video);
            }
            return null;
        }
        /// <summary>
        /// 群发消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public MessageId SendMessage(BulkMessage message)
        {
            if (message != null)
            {
                return requestJson<MessageId, BulkMessage>("https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=", message);
            }
            return null;
        }
        /// <summary>
        /// 群发图文消息
        /// </summary>
        /// <param name="filter">用于设定图文消息的接收者</param>
        /// <param name="articles">支持1到10条图文</param>
        /// <returns></returns>
        public MessageId SendNews(BulkMessageFilter filter, Article[] articles)
        {
            Media media = UploadNews(articles);
            return media == null ? null : SendNews(filter, media.media_id);
        }
        /// <summary>
        /// 群发图文消息
        /// </summary>
        /// <param name="filter">用于设定图文消息的接收者</param>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public MessageId SendNews(BulkMessageFilter filter, string media_id)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                return SendMessage(new BulkMessage { filter = filter, msgtype = BulkMessageType.mpnews, mpnews = new MediaMessage { media_id = media_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发文本消息
        /// </summary>
        /// <param name="filter">用于设定图文消息的接收者</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public MessageId SendText(BulkMessageFilter filter, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                return SendMessage(new BulkMessage { filter = filter, msgtype = BulkMessageType.text, text = new TextMessage { content = content } });
            }
            return null;
        }
        /// <summary>
        /// 群发语音消息
        /// </summary>
        /// <param name="filter">用于设定图文消息的接收者</param>
        /// <param name="media_id">需通过基础支持中的上传下载多媒体文件来得到</param>
        /// <returns></returns>
        public MessageId SendVoice(BulkMessageFilter filter, string media_id)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                return SendMessage(new BulkMessage { filter = filter, msgtype = BulkMessageType.voice, voice = new MediaMessage { media_id = media_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发图片消息
        /// </summary>
        /// <param name="filter">用于设定图文消息的接收者</param>
        /// <param name="media_id">需通过基础支持中的上传下载多媒体文件来得到</param>
        /// <returns></returns>
        public MessageId SendImage(BulkMessageFilter filter, string media_id)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                return SendMessage(new BulkMessage { filter = filter, msgtype = BulkMessageType.image, image = new MediaMessage { media_id = media_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发视频
        /// </summary>
        /// <param name="filter">用于设定图文消息的接收者</param>
        /// <param name="media_id">需通过UploadVideo视频转换来得到</param>
        /// <returns></returns>
        public MessageId SendVideo(BulkMessageFilter filter, string media_id)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                return SendMessage(new BulkMessage { filter = filter, msgtype = BulkMessageType.mpvideo, mpvideo = new MediaMessage { media_id = media_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发视频
        /// </summary>
        /// <param name="filter">用于设定图文消息的接收者</param>
        /// <param name="video"></param>
        /// <returns></returns>
        public MessageId SendVideo(BulkMessageFilter filter, SendVideo video)
        {
            Media media = UploadVideo(video);
            return media == null ? null : SendVideo(filter, media.media_id);
        }
        /// <summary>
        /// 群发卡券消息
        /// </summary>
        /// <param name="filter">用于设定图文消息的接收者</param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        public MessageId SendCard(BulkMessageFilter filter, string card_id)
        {
            if (!string.IsNullOrEmpty(card_id))
            {
                return SendMessage(new BulkMessage { filter = filter, msgtype = BulkMessageType.wxcard, wxcard = new BulkCardMessage { card_id = card_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public MessageId SendMessage(OpenIdMessage message)
        {
            if (message != null)
            {
                return requestJson<MessageId, OpenIdMessage>("https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=", message);
            }
            return null;
        }
        /// <summary>
        /// 群发图文消息
        /// </summary>
        /// <param name="touser">接收者，一串OpenID列表，OpenID最少2个，最多10000个</param>
        /// <param name="articles">支持1到10条图文</param>
        /// <returns></returns>
        public MessageId SendNews(string[] touser, Article[] articles)
        {
            if (touser.length() != 0)
            {
                Media media = UploadNews(articles);
                if (media != null) return SendNews(touser, media.media_id);
            }
            return null;
        }
        /// <summary>
        /// 群发图文消息
        /// </summary>
        /// <param name="touser">接收者，一串OpenID列表，OpenID最少2个，最多10000个</param>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public MessageId SendNews(string[] touser, string media_id)
        {
            if (touser.length() != 0 && !string.IsNullOrEmpty(media_id))
            {
                return SendMessage(new OpenIdMessage { touser = touser, msgtype = BulkMessageType.mpnews, mpnews = new MediaMessage { media_id = media_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发文本消息
        /// </summary>
        /// <param name="touser">接收者，一串OpenID列表，OpenID最少2个，最多10000个</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public MessageId SendText(string[] touser, string content)
        {
            if (touser.length() != 0 && !string.IsNullOrEmpty(content))
            {
                return SendMessage(new OpenIdMessage { touser = touser, msgtype = BulkMessageType.text, text = new TextMessage { content = content } });
            }
            return null;
        }
        /// <summary>
        /// 群发语音消息
        /// </summary>
        /// <param name="touser">接收者，一串OpenID列表，OpenID最少2个，最多10000个</param>
        /// <param name="media_id">需通过基础支持中的上传下载多媒体文件来得到</param>
        /// <returns></returns>
        public MessageId SendVoice(string[] touser, string media_id)
        {
            if (touser.length() != 0 && !string.IsNullOrEmpty(media_id))
            {
                return SendMessage(new OpenIdMessage { touser = touser, msgtype = BulkMessageType.voice, voice = new MediaMessage { media_id = media_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发图片消息
        /// </summary>
        /// <param name="touser">接收者，一串OpenID列表，OpenID最少2个，最多10000个</param>
        /// <param name="media_id">需通过基础支持中的上传下载多媒体文件来得到</param>
        /// <returns></returns>
        public MessageId SendImage(string[] touser, string media_id)
        {
            if (touser.length() != 0 && !string.IsNullOrEmpty(media_id))
            {
                return SendMessage(new OpenIdMessage { touser = touser, msgtype = BulkMessageType.image, image = new MediaMessage { media_id = media_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发视频
        /// </summary>
        /// <param name="touser">接收者，一串OpenID列表，OpenID最少2个，最多10000个</param>
        /// <param name="media_id">需通过UploadVideo视频转换来得到</param>
        /// <returns></returns>
        public MessageId SendVideo(string[] touser, string media_id)
        {
            if (touser.length() != 0 && !string.IsNullOrEmpty(media_id))
            {
                return SendMessage(new OpenIdMessage { touser = touser, msgtype = BulkMessageType.mpvideo, mpvideo = new MediaMessage { media_id = media_id } });
            }
            return null;
        }
        /// <summary>
        /// 群发视频
        /// </summary>
        /// <param name="touser">接收者，一串OpenID列表，OpenID最少2个，最多10000个</param>
        /// <param name="video"></param>
        /// <returns></returns>
        public MessageId SendVideo(string[] touser, SendVideo video)
        {
            if (touser.length() != 0)
            {
                Media media = UploadVideo(video);
                if (media == null) return SendVideo(touser, media.media_id);
            }
            return null;
        }
        /// <summary>
        /// 群发卡券消息
        /// </summary>
        /// <param name="touser">接收者，一串OpenID列表，OpenID最少2个，最多10000个</param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        public MessageId SendCard(string[] touser, string card_id)
        {
            if (touser.length() != 0 && !string.IsNullOrEmpty(card_id))
            {
                return SendMessage(new OpenIdMessage { touser = touser, msgtype = BulkMessageType.wxcard, wxcard = new BulkCardMessage { card_id = card_id } });
            }
            return null;
        }
        /// <summary>
        /// 删除消息发送任务的ID
        /// </summary>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        public bool DeleteMessage(string msg_id)
        {
            if (!string.IsNullOrEmpty(msg_id))
            {
                Return value = requestJson<Return, MessageIdQuery>("https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token=", new MessageIdQuery { msg_id = msg_id });
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 发送预览消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public MessageId SendMessage(PreviewMessage message)
        {
            if (message != null)
            {
                return requestJson<MessageId, PreviewMessage>("https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token=", message);
            }
            return null;
        }
        /// <summary>
        /// 图文消息预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="articles">支持1到10条图文</param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendNews(string touser, Article[] articles, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser))
            {
                Media media = UploadNews(articles);
                if (media != null) return SendNews(touser, media.media_id, isOpenId);
            }
            return null;
        }
        /// <summary>
        /// 图文消息预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="media_id">需通过基础支持中的上传下载多媒体文件来得到</param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendNews(string touser, string media_id, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser) && !string.IsNullOrEmpty(media_id))
            {
                PreviewMessage message = new PreviewMessage { msgtype = BulkMessageType.mpnews, mpnews = new MediaMessage { media_id = media_id } };
                if (isOpenId) message.touser = touser;
                else message.towxname = touser;
                return SendMessage(message);
            }
            return null;
        }
        /// <summary>
        /// 文本消息预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="content"></param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendText(string touser, string content, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser) && !string.IsNullOrEmpty(content))
            {
                PreviewMessage message = new PreviewMessage { msgtype = BulkMessageType.text, text = new TextMessage { content = content } };
                if (isOpenId) message.touser = touser;
                else message.towxname = touser;
                return SendMessage(message);
            }
            return null;
        }
        /// <summary>
        /// 语音消息预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="media_id">需通过基础支持中的上传下载多媒体文件来得到</param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendVoice(string touser, string media_id, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser) && !string.IsNullOrEmpty(media_id))
            {
                PreviewMessage message = new PreviewMessage { msgtype = BulkMessageType.voice, voice = new MediaMessage { media_id = media_id } };
                if (isOpenId) message.touser = touser;
                else message.towxname = touser;
                return SendMessage(message);
            }
            return null;
        }
        /// <summary>
        /// 图片消息预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="media_id">需通过基础支持中的上传下载多媒体文件来得到</param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendImage(string touser, string media_id, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser) && !string.IsNullOrEmpty(media_id))
            {
                PreviewMessage message = new PreviewMessage { msgtype = BulkMessageType.image, image = new MediaMessage { media_id = media_id } };
                if (isOpenId) message.touser = touser;
                else message.towxname = touser;
                return SendMessage(message);
            }
            return null;
        }
        /// <summary>
        /// 视频预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="media_id">需通过UploadVideo视频转换来得到</param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendVideo(string touser, string media_id, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser) && !string.IsNullOrEmpty(media_id))
            {
                PreviewMessage message = new PreviewMessage { msgtype = BulkMessageType.mpvideo, mpvideo = new MediaMessage { media_id = media_id } };
                if (isOpenId) message.touser = touser;
                else message.towxname = touser;
                return SendMessage(message);
            }
            return null;
        }
        /// <summary>
        /// 视频预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="video">需通过UploadVideo视频转换来得到</param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendVideo(string touser, SendVideo video, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser))
            {
                Media media = UploadVideo(video);
                if (media == null) return SendVideo(touser, media.media_id, isOpenId);
            }
            return null;
        }
        /// <summary>
        /// 卡券预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="wxcard"></param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendCard(string touser, CardMessage wxcard, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser))
            {
                PreviewMessage message = new PreviewMessage { msgtype = BulkMessageType.wxcard, wxcard = wxcard };
                if (isOpenId) message.touser = touser;
                else message.towxname = touser;
                return SendMessage(message);
            }
            return null;
        }
        /// <summary>
        /// 卡券预览
        /// </summary>
        /// <param name="touser">接收者,isOpenId为true表示OPENID,isOpenId为false标识微信用户</param>
        /// <param name="card_id"></param>
        /// <param name="api_ticket"></param>
        /// <param name="card_ext"></param>
        /// <param name="isOpenId"></param>
        /// <returns></returns>
        public MessageId SendCard(string touser, string card_id, string api_ticket, MessageCard card_ext, bool isOpenId = true)
        {
            if (!string.IsNullOrEmpty(touser) && !string.IsNullOrEmpty(card_id))
            {
                card_ext.SetSignature(card_id, api_ticket);
                return SendCard(touser, new CardMessage { card_id = card_id, card_ext = card_ext }, isOpenId);
            }
            return null;
        }
        /// <summary>
        /// 查询群发消息发送状态
        /// </summary>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        public bool GetMessage(long msg_id)
        {
            return GetMessage(msg_id.toString());
        }
        /// <summary>
        /// 查询群发消息发送状态
        /// </summary>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        public bool GetMessage(string msg_id)
        {
            if (!string.IsNullOrEmpty(msg_id))
            {
                MessageStatus value = requestJson<MessageStatus, MessageIdQuery>("https://api.weixin.qq.com/cgi-bin/message/mass/get?access_token=", new MessageIdQuery { msg_id = msg_id });
                return value != null && value.msg_status == "SEND_SUCCESS";
            }
            return false;
        }
        /// <summary>
        /// 设置行业
        /// </summary>
        /// <param name="industry1"></param>
        /// <param name="industry2"></param>
        /// <returns></returns>
        public bool SetIndustry(Industry industry1, Industry industry2)
        {
            Return value = requestJson<Return, IndustryQuery>("https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token=", new IndustryQuery { industry_id1 = ((byte)industry1).toString(), industry_id2 = ((byte)industry2).toString() });
            return value != null && value.IsReturn;
        }
        /// <summary>
        /// 获得模板ID
        /// </summary>
        /// <param name="template_id_short">模板库中模板的编号，有“TM**”和“OPENTMTM**”等形式</param>
        /// <returns></returns>
        public string GetTemplateId(string template_id_short)
        {
            if (!string.IsNullOrEmpty(template_id_short))
            {
                TemplateId value = requestJson<TemplateId, TemplateQuery>("https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token=", new TemplateQuery { template_id_short = template_id_short });
                if (value != null) return value.template_id;
            }
            return null;
        }
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="template"></param>
        /// <returns>失败返回0</returns>
        public long SendTemplate<valueType>(Template<valueType> template)
        {
            if (template != null)
            {
                SendTemplate value = requestJson<SendTemplate, Template<valueType>>("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=", template);
                if (value != null) return value.msgid;
            }
            return 0;
        }
        /// <summary>
        /// 获取自动回复规则
        /// </summary>
        /// <returns></returns>
        public AutoReply GetAutoReply()
        {
            return request<AutoReply>("https://api.weixin.qq.com/cgi-bin/get_current_autoreply_info?access_token=");
        }
        /// <summary>
        /// 新增临时素材,媒体文件在后台保存时间为3天，即3天后media_id失效
        /// </summary>
        /// <param name="type">排除图文消息</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Media UploadMedia(MediaType type, byte[] data)
        {
            if (data.length() != 0 && type != MediaType.news)
            {
                return requestFile<Media>("https://api.weixin.qq.com/cgi-bin/media/upload?type=" + type.ToString() + "&access_token=", data);
            }
            return null;
        }
        /// <summary>
        /// 获取临时素材
        /// </summary>
        /// <param name="media_id">媒体文件ID</param>
        /// <param name="isVideo"></param>
        /// <returns></returns>
        public byte[] DownloadMedia(string media_id, bool isVideo = false)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                if (isVideo) return downloadIsValue("http://api.weixin.qq.com/cgi-bin/media/get?media_id=" + media_id + "&access_token=");
                return downloadIsValue("https://api.weixin.qq.com/cgi-bin/media/get?media_id=" + media_id + "&access_token=");
            }
            return null;
        }
        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="articles"></param>
        /// <returns>media_id</returns>
        public string AddNews(Article[] articles)
        {
            if (articles.length() != 0)
            {
                Media value = requestJson<Media, UploadArticle>("https://api.weixin.qq.com/cgi-bin/material/add_news?access_token=", new UploadArticle { articles = articles });
                if (value != null) return value.media_id;
            }
            return null;
        }
        /// <summary>
        /// 视频描述信息
        /// </summary>
        private static readonly byte[] videoDescriptionData = "description".getBytes();
        /// <summary>
        /// 新增永久素材
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="video"></param>
        /// <returns></returns>
        public MediaUrl UploadMediaUrl(MediaType type, byte[] data, UploadVideo video = default(UploadVideo))
        {
            if (data.length() != 0)
            {
                KeyValue<byte[], byte[]>[] form = type == MediaType.video ? new KeyValue<byte[], byte[]>[] { new KeyValue<byte[], byte[]>(videoDescriptionData, System.Text.Encoding.UTF8.GetBytes(AutoCSer.Json.Serializer.Serialize(video))) } : null;
                return requestFile<MediaUrl>("https://api.weixin.qq.com/cgi-bin/material/add_material?type=" + type.ToString() + "&access_token=", data, "media", null, null, form);
            }
            return null;
        }
        /// <summary>
        /// 获取图文消息素材
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public ArticleUrl[] GetNews(string media_id)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                Articles value = requestJson<Articles, MediaMessage>("https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=", new MediaMessage { media_id = media_id });
                if (value != null) return value.news_item;
            }
            return null;
        }
        /// <summary>
        /// 获取视频消息素材
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public VideoUrl GetVideo(string media_id)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                return requestJson<VideoUrl, MediaMessage>("https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=", new MediaMessage { media_id = media_id });
            }
            return null;
        }
        /// <summary>
        /// 获取永久素材
        /// </summary>
        /// <param name="media_id">媒体文件ID</param>
        /// <returns></returns>
        public byte[] DownloadMediaPost(string media_id)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                return downloadJson<MediaMessage>("https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=", new MediaMessage { media_id = media_id });
            }
            return null;
        }
        /// <summary>
        /// 删除永久素材
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public bool DeleteMedia(string media_id)
        {
            if (!string.IsNullOrEmpty(media_id))
            {
                Return value = requestJson<Return, MediaMessage>("https://api.weixin.qq.com/cgi-bin/material/del_material?access_token=", new MediaMessage { media_id = media_id });
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 修改永久图文素材
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public bool UpdateNews(News news)
        {
            if (news != null)
            {
                Return value = requestJson<Return, News>("https://api.weixin.qq.com/cgi-bin/material/update_news?access_token=", news);
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 获取素材总数
        /// </summary>
        /// <returns></returns>
        public MediaCount GetMediaCount()
        {
            return request<MediaCount>("https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token=");
        }
        /// <summary>
        /// 获取永久图文消息素材列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public NewsList GetNewsList(MediaQuery query)
        {
            query.type = MediaQueryType.news;
            return requestJson<NewsList, MediaQuery>("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=", query);
        }
        /// <summary>
        /// 获取消息素材列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public MediaList GetMediaList(MediaQuery query)
        {
            if (query.type == MediaQueryType.news) return null;
            return requestJson<MediaList, MediaQuery>("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=", query);
        }
        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Group CreateGroup(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                GroupResult value = requestJson<GroupResult, GroupQuery>("https://api.weixin.qq.com/cgi-bin/groups/create?access_token=", new GroupQuery { group = new GroupName { name = name } });
                if (value != null) return value.group;
            }
            return null;
        }
        /// <summary>
        /// 查询所有分组
        /// </summary>
        /// <returns></returns>
        public GroupCount[] GetGroups()
        {
            GroupsResult value = request<GroupsResult>("https://api.weixin.qq.com/cgi-bin/groups/get?access_token=");
            return value == null ? null : value.groups;
        }
        /// <summary>
        /// 查询用户所在分组
        /// </summary>
        /// <param name="openid"></param>
        /// <returns>失败返回0</returns>
        public int GetGroupId(string openid)
        {
            if (!string.IsNullOrEmpty(openid))
            {
                GroupIdResult value = requestJson<GroupIdResult, OpenidQuery>("https://api.weixin.qq.com/cgi-bin/groups/getid?access_token=", new OpenidQuery { openid = openid });
                if (value == null) return value.groupid;
            }
            return 0;
        }
        /// <summary>
        /// 修改分组名
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool UpdateGroup(Group group)
        {
            if (group != null)
            {
                Return value = requestJson<Return, GroupResult>("https://api.weixin.qq.com/cgi-bin/groups/update?access_token=", new GroupResult { group = group });
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="openid">用户唯一标识符</param>
        /// <param name="to_groupid">分组id</param>
        /// <returns></returns>
        public bool UpdateGroup(string openid, int to_groupid)
        {
            if (!string.IsNullOrEmpty(openid))
            {
                Return value = requestJson<Return, UpdateGroupQuery>("https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token=", new UpdateGroupQuery { openid = openid, to_groupid = to_groupid });
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 批量移动用户分组
        /// </summary>
        /// <param name="openid_list">用户唯一标识符openid的列表（size不能超过50）</param>
        /// <param name="to_groupid">分组id</param>
        /// <returns></returns>
        public bool UpdateGroup(string[] openid_list, int to_groupid)
        {
            if (openid_list.length() != 0)
            {
                Return value = requestJson<Return, UpdateGroupsQuery>("https://api.weixin.qq.com/cgi-bin/groups/members/batchupdate?access_token=", new UpdateGroupsQuery { openid_list = openid_list, to_groupid = to_groupid });
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteGroup(int id)
        {
            Return value = requestJson<Return, DeleteGroupQuery>("https://api.weixin.qq.com/cgi-bin/groups/delete?access_token=", new DeleteGroupQuery { group = new DeleteGroupId { id = id } });
            return value != null && value.IsReturn;
        }
        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <param name="openid">用户标识</param>
        /// <param name="remark">新的备注名，长度必须小于30字符</param>
        /// <returns></returns>
        public bool UpdateRemark(string openid, string remark)
        {
            if (!string.IsNullOrEmpty(openid))
            {
                Return value = requestJson<Return, UpdateRemarkQuery>("https://api.weixin.qq.com/cgi-bin/groups/delete?access_token=", new UpdateRemarkQuery { openid = openid, remark = remark ?? string.Empty });
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User GetUser(UserLanguage user)
        {
            if (!string.IsNullOrEmpty(user.openid))
            {
                return request<User>("https://api.weixin.qq.com/cgi-bin/user/info?openid=" + user.openid + "&lang=" + user.lang.ToString() + "&access_token=");
            }
            return null;
        }
        /// <summary>
        /// 获取用户基本信息,最多支持一次拉取100条
        /// </summary>
        /// <param name="user_list"></param>
        /// <returns></returns>
        public User[] GetUserList(UserLanguage[] user_list)
        {
            if ((uint)(user_list.length() - 1) < 100)
            {
                UserListResult value = requestJson<UserListResult, UserListQuery>("https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token=", new UserListQuery { user_list = user_list });
                if (value != null) return value.user_info_list;
            }
            return null;
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="next_openid">第一个拉取的OPENID，不填默认从头开始拉取</param>
        /// <returns></returns>
        public OpenIdList GetOpenIdList(string next_openid = null)
        {
            if (string.IsNullOrEmpty(next_openid)) return request<OpenIdList>("https://api.weixin.qq.com/cgi-bin/user/get?access_token=");
            return request<OpenIdList>("https://api.weixin.qq.com/cgi-bin/user/get?next_openid=" + next_openid + "&access_token=");
        }
        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="button">最多包括3个一级菜单</param>
        /// <returns></returns>
        public bool CreateMenu(Menu[] button)
        {
            if (((uint)button.length() - 1) < 3)
            {
                Return value = requestJson<Return, MenuQeury>("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=", new MenuQeury { button = button });
                return value != null && value.IsReturn;
            }
            return false;
        }
        /// <summary>
        /// 获取自定义菜单
        /// </summary>
        /// <returns></returns>
        public Menu[] GetMenu()
        {
            MenuResult value = request<MenuResult>("https://api.weixin.qq.com/cgi-bin/menu/get?access_token=");
            return value == null ? null : value.menu.button;
        }
        /// <summary>
        /// 获取自定义菜单
        /// </summary>
        /// <returns></returns>
        public bool DeleteMenu()
        {
            Return value = request<Return>("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=");
            return value == null && value.IsReturn;
        }
        /// <summary>
        /// 获取自定义菜单配置
        /// </summary>
        /// <param name="is_menu_open">菜单是否开启</param>
        /// <returns></returns>
        public Menu[] GetMenuInfo(out bool is_menu_open)
        {
            MenuInfoResult value = request<MenuInfoResult>("https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?access_token=");
            if (value != null)
            {
                is_menu_open = value.is_menu_open != 0;
                return value.selfmenu_info.button;
            }
            is_menu_open = false;
            return null;
        }
        /// <summary>
        /// 创建二维码，下载地址https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=TICKET
        /// </summary>
        /// <param name="scene_id">场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）</param>
        /// <param name="expire_seconds">该二维码有效时间，以秒为单位。0表示永久二维码， 最大不超过604800（即7天）。</param>
        /// <returns></returns>
        public QrCode CreateQrCode(uint scene_id, int expire_seconds)
        {
            if (scene_id != 0 && (uint)expire_seconds <= 604800 && (scene_id <= 100000 || expire_seconds != 0))
            {
                return requestJson<QrCode, QrCodeQuery>("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=", new QrCodeQuery { expire_seconds = expire_seconds, action_info = new QrCodeAction { scene = new SceneId { scene_id = scene_id } } });
            }
            return null;
        }
        /// <summary>
        /// 创建永久二维码，下载地址https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=TICKET
        /// </summary>
        /// <param name="scene_str">场景值ID（字符串形式的ID），字符串类型，长度限制为1到64</param>
        /// <returns></returns>
        public QrCode CreateQrCode(string scene_str)
        {
            if (((uint)scene_str.length() - 1) < 64)
            {
                return requestJson<QrCode, QrCodeQuery>("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=", new QrCodeQuery { action_info = new QrCodeAction { scene = new SceneId { scene_str = scene_str } } });
            }
            return null;
        }
        /// <summary>
        /// 长链接转短链接
        /// </summary>
        /// <param name="long_url">需要转换的长链接，支持http://、https://、weixin://wxpay 格式的url</param>
        /// <returns></returns>
        public string GetShortUrl(string long_url)
        {
            if (!string.IsNullOrEmpty(long_url))
            {
                ShortUrl value = requestJson<ShortUrl, LongUrl>("https://api.weixin.qq.com/cgi-bin/shorturl?access_token=", new LongUrl { action = "long2short", long_url = long_url });
                if (value != null) return value.short_url;
            }
            return null;
        }
        /// <summary>
        /// 获取用户增减数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days">最大时间跨度为7天</param>
        /// <returns></returns>
        public UserSummary[] GetUserSummary(DateTime begin_date, byte days = 7)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 7);
            if (dateRange.begin_date != null)
            {
                UserSummaryResult value = requestJson<UserSummaryResult, DateRange>("https://api.weixin.qq.com/datacube/getusersummary?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取累计用户数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days">最大时间跨度为7天</param>
        /// <returns></returns>
        public UserCumulate[] GetUserCumulate(DateTime begin_date, byte days = 7)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 7);
            if (dateRange.begin_date != null)
            {
                UserCumulateResult value = requestJson<UserCumulateResult, DateRange>("https://api.weixin.qq.com/datacube/getusercumulate?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取图文群发每日数据，某天所有被阅读过的文章（仅包括群发的文章）在当天的阅读次数等数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ArticleSummary GetArticleSummary(DateTime date)
        {
            string dateString = DateRange.ToString(date);
            if (dateString != null)
            {
                ArticleSummaryResult value = requestJson<ArticleSummaryResult, DateRange>("https://api.weixin.qq.com/datacube/getarticlesummary?access_token=", new DateRange { begin_date = dateString, end_date = dateString });
                if (value != null && value.list.length() != 0) return value.list[0];
            }
            return null;
        }
        /// <summary>
        /// 获取图文群发总数据，某天群发的文章，从群发日起到接口调用日（但最多统计发表日后7天数据），每天的到当天的总等数据。例如某篇文章是12月1日发出的，发出后在1日、2日、3日的阅读次数分别为1万，则getarticletotal获取到的数据为，距发出到12月1日24时的总阅读量为1万，距发出到12月2日24时的总阅读量为2万，距发出到12月1日24时的总阅读量为3万。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ArticleTotal GetArticleTotal(DateTime date)
        {
            string dateString = DateRange.ToString(date);
            if (dateString != null)
            {
                ArticleTotalResult value = requestJson<ArticleTotalResult, DateRange>("https://api.weixin.qq.com/datacube/getarticletotal?access_token=", new DateRange { begin_date = dateString, end_date = dateString });
                if (value != null && value.list.length() != 0) return value.list[0];
            }
            return null;
        }
        /// <summary>
        /// 获取图文统计数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public UserRead[] GetUserRead(DateTime begin_date, byte days = 3)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 3);
            if (dateRange.begin_date != null)
            {
                UserReadResult value = requestJson<UserReadResult, DateRange>("https://api.weixin.qq.com/datacube/getuserread?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取图文统计分时数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public UserReadHour[] GetUserReadHour(DateTime date)
        {
            string dateString = DateRange.ToString(date);
            if (dateString != null)
            {
                UserReadHourResult value = requestJson<UserReadHourResult, DateRange>("https://api.weixin.qq.com/datacube/getuserreadhour?access_token=", new DateRange { begin_date = dateString, end_date = dateString });
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取图文分享转发数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public UserShare[] GetUserShare(DateTime begin_date, byte days = 7)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 7);
            if (dateRange.begin_date != null)
            {
                UserShareResult value = requestJson<UserShareResult, DateRange>("https://api.weixin.qq.com/datacube/getusershare?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取图文分享转发分时数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public UserShareHour[] GetUserShareHour(DateTime date)
        {
            string dateString = DateRange.ToString(date);
            if (dateString != null)
            {
                UserShareHourResult value = requestJson<UserShareHourResult, DateRange>("https://api.weixin.qq.com/datacube/getusersharehour?access_token=", new DateRange { begin_date = dateString, end_date = dateString });
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取消息发送概况数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public MessageCount[] GetMessageCount(DateTime begin_date, byte days = 7)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 7);
            if (dateRange.begin_date != null)
            {
                MessageCountResult value = requestJson<MessageCountResult, DateRange>("https://api.weixin.qq.com/datacube/getupstreammsg?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取消息分送分时数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public MessageCountHour[] GetMessageCountHour(DateTime date)
        {
            string dateString = DateRange.ToString(date);
            if (dateString != null)
            {
                MessageCountHourResult value = requestJson<MessageCountHourResult, DateRange>("https://api.weixin.qq.com/datacube/getupstreammsghour?access_token=", new DateRange { begin_date = dateString, end_date = dateString });
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取消息发送周数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public MessageCount[] GetMessageCountWeek(DateTime begin_date, byte days = 30)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 30);
            if (dateRange.begin_date != null)
            {
                MessageCountResult value = requestJson<MessageCountResult, DateRange>("https://api.weixin.qq.com/datacube/getupstreammsgweek?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取消息发送月数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public MessageCount[] GetMessageCountMonth(DateTime begin_date, byte days = 30)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 30);
            if (dateRange.begin_date != null)
            {
                MessageCountResult value = requestJson<MessageCountResult, DateRange>("https://api.weixin.qq.com/datacube/getupstreammsgmonth?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取消息发送分布数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public MessageDistributed[] GetMessageDistributed(DateTime begin_date, byte days = 15)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 15);
            if (dateRange.begin_date != null)
            {
                MessageDistributedResult value = requestJson<MessageDistributedResult, DateRange>("https://api.weixin.qq.com/datacube/getupstreammsgdist?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取消息发送分布周数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public MessageDistributed[] GetMessageDistributedWeek(DateTime begin_date, byte days = 30)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 30);
            if (dateRange.begin_date != null)
            {
                MessageDistributedResult value = requestJson<MessageDistributedResult, DateRange>("https://api.weixin.qq.com/datacube/getupstreammsgdistweek?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取消息发送分布月数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public MessageDistributed[] GetMessageDistributedMonth(DateTime begin_date, byte days = 30)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 30);
            if (dateRange.begin_date != null)
            {
                MessageDistributedResult value = requestJson<MessageDistributedResult, DateRange>("https://api.weixin.qq.com/datacube/getupstreammsgdistmonth?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取接口分析数据
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public InterfaceSummary[] GetInterfaceSummary(DateTime begin_date, byte days = 30)
        {
            DateRange dateRange = DateRange.Check(begin_date, days, 30);
            if (dateRange.begin_date != null)
            {
                InterfaceSummaryResult value = requestJson<InterfaceSummaryResult, DateRange>("https://api.weixin.qq.com/datacube/getinterfacesummary?access_token=", dateRange);
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 获取接口分析分时数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public InterfaceSummaryHour[] GetInterfaceSummaryHour(DateTime date)
        {
            string dateString = DateRange.ToString(date);
            if (dateString != null)
            {
                InterfaceSummaryHourResult value = requestJson<InterfaceSummaryHourResult, DateRange>("https://api.weixin.qq.com/datacube/getinterfacesummaryhour?access_token=", new DateRange { begin_date = dateString, end_date = dateString });
                if (value != null) return value.list;
            }
            return null;
        }
        /// <summary>
        /// 为多客服的客服工号创建会话
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool CreateCustomSession(CustomSession session)
        {
            Return value = requestJson<Return, CustomSession>("https://api.weixin.qq.com/customservice/kfsession/create?access_token=", session);
            return value != null && value.IsReturn;
        }
        /// <summary>
        /// 关闭多客服的客服工号会话
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool CloseCustomSession(CustomSession session)
        {
            Return value = requestJson<Return, CustomSession>("https://api.weixin.qq.com/customservice/kfsession/close?access_token=", session);
            return value != null && value.IsReturn;
        }
        /// <summary>
        /// 获取客户的会话状态
        /// </summary>
        /// <param name="openid">客户openid</param>
        /// <returns></returns>
        public CustomSessionTime GetCustomSession(string openid)
        {
            if (!string.IsNullOrEmpty(openid))
            {
                return request<CustomSessionTime>("https://api.weixin.qq.com/customservice/kfsession/getsession?openid=" + openid + "&access_token=");
            }
            return null;
        }
        /// <summary>
        /// 获取客服的会话列表
        /// </summary>
        /// <param name="kf_account">完整客服账号，格式为：账号前缀@公众号微信号，账号前缀最多10个字符，必须是英文或者数字字符。</param>
        /// <returns></returns>
        public CustomTime[] GetSessionList(string kf_account)
        {
            if (!string.IsNullOrEmpty(kf_account))
            {
                CustomListResult value = request<CustomListResult>("https://api.weixin.qq.com/customservice/kfsession/getsessionlist?kf_account=" + kf_account + "&access_token=");
                if (value != null) return value.sessionlist;
            }
            return null;
        }
        /// <summary>
        /// 获取未接入会话列表
        /// </summary>
        /// <returns></returns>
        private WaitCustomSession GetWaitSession()
        {
            return request<WaitCustomSession>("https://api.weixin.qq.com/customservice/kfsession/getwaitcase?access_token=");
        }
        /// <summary>
        /// 获取客服聊天记录
        /// </summary>
        /// <param name="starttime">查询开始时间</param>
        /// <param name="endtime">查询结束时间，每次查询不能跨日查询</param>
        /// <param name="pageindex">查询第几页，从1开始</param>
        /// <param name="pagesize">每页大小，每页最多拉取50条</param>
        /// <returns></returns>
        public CustomRecord[] GetCustomRecord(DateTime starttime, DateTime endtime, int pageindex = 1, byte pagesize = 50)
        {
            if (((starttime.Year ^ endtime.Year) | (starttime.Month ^ endtime.Month) | (starttime.Day ^ endtime.Day)) == 0 && pageindex > 0 && (uint)(pagesize - 1) < 50)
            {
                CustomRecordList value = requestJson<CustomRecordList, RecordQuery>("https://api.weixin.qq.com/customservice/msgrecord/getrecord?access_token=", new RecordQuery { starttime = (long)(starttime - Config.MinTime).TotalSeconds, endtime = (long)(endtime - Config.MinTime).TotalSeconds, pageindex = pageindex, pagesize = pagesize });
                if (value != null) return value.recordlist;
            }
            return null;
        }
        /// <summary>
        /// 申请开通摇一摇周边功能。成功提交申请请求后，工作人员会在三个工作日内完成审核。
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool RegisterShakeAround(ShakeAroundAccount account)
        {
            Return value = requestJson<Return, ShakeAroundAccount>("https://api.weixin.qq.com/shakearound/account/register?access_token=", account);
            return value != null && value.IsReturn;
        }
        /// <summary>
        /// 查询申请开通摇一摇周边审核状态
        /// </summary>
        /// <returns></returns>
        public ShakeAroundAccountStatus GetShakeAroundAccountStatus()
        {
            ShakeAroundAccountStatusResult value = request<ShakeAroundAccountStatusResult>("https://api.weixin.qq.com/shakearound/account/auditstatus?access_token=");
            return value == null ? null : value.data;
        }

        /// <summary>
        /// XML序列化配置
        /// </summary>
        private static readonly AutoCSer.Xml.SerializeConfig xmlSerializeConfig = new AutoCSer.Xml.SerializeConfig { CheckLoopDepth = AutoCSer.Xml.SerializeConfig.DefaultCheckLoopDepth, Header = null, IsOutputEmptyString = false };
        /// <summary>
        /// 获取交易会话标识
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetPrePayId(PrePayIdQuery value)
        {
            value.SetConfig(config);
            PrePayId prePayId = Config.Client.RequestXml<PrePayId, PrePayIdQuery>("https://api.mch.weixin.qq.com/pay/unifiedorder", value, xmlSerializeConfig);
            return prePayId != null && prePayId.Verify(config) ? prePayId.prepay_id : null;
        }
        /// <summary>
        /// 获取交易会话标识
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppPrePayIdOrder GetPrePayId(AppPrePayIdQuery value)
        {
            value.SetConfig(config);
            AppPrePayId prePayId = Config.Client.RequestXml<AppPrePayId, AppPrePayIdQuery>("https://api.mch.weixin.qq.com/pay/unifiedorder", value, xmlSerializeConfig);
            return prePayId != null && prePayId.Verify(config) ? new AppPrePayIdOrder(config, prePayId.prepay_id, value.nonce_str) : null;
        }
        /// <summary>
        /// 订单查询信息
        /// </summary>
        /// <param name="transaction_id">微信的订单号</param>
        /// <returns></returns>
        public OrderResult GetOrder(string transaction_id)
        {
            string xml;
            return getOrder(new OrderQuery { transaction_id = transaction_id }, out xml);
        }
        /// <summary>
        /// 订单查询信息
        /// </summary>
        /// <param name="transaction_id">微信的订单号</param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public OrderResult GetOrder(string transaction_id, out string xml)
        {
            return getOrder(new OrderQuery { transaction_id = transaction_id }, out xml);
        }
        /// <summary>
        /// 订单查询信息
        /// </summary>
        /// <param name="out_trade_no">商户系统内部的订单号</param>
        /// <returns></returns>
        public OrderResult GetOrderByLocal(string out_trade_no)
        {
            string xml;
            return getOrder(new OrderQuery { out_trade_no = out_trade_no }, out xml);
        }
        /// <summary>
        /// 订单查询信息
        /// </summary>
        /// <param name="out_trade_no">商户系统内部的订单号</param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public OrderResult GetOrderByLocal(string out_trade_no, out string xml)
        {
            return getOrder(new OrderQuery { out_trade_no = out_trade_no }, out xml);
        }
        /// <summary>
        /// 订单查询信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        private OrderResult getOrder(OrderQuery value, out string xml)
        {
            value.SetConfig(config);
            OrderResult result = Config.Client.RequestXml<OrderResult, OrderQuery>("https://api.mch.weixin.qq.com/pay/orderquery", value, out xml, xmlSerializeConfig);
            return result != null && result.Verify(config) ? result : null;
        }
        /// <summary>
        /// 关闭订单，订单生成后不能马上调用关单接口，最短调用时间间隔为5分钟
        /// </summary>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public bool CloseOrder(string out_trade_no, out ErrorCodeEnum errorCode)
        {
            OrderQuery value = new OrderQuery { out_trade_no = out_trade_no };
            value.SetConfig(config);
            CloseOrderResult result = Config.Client.RequestXml<CloseOrderResult, OrderQuery>("https://api.mch.weixin.qq.com/pay/closeorder", value, xmlSerializeConfig, false);
            if (result != null)
            {
                errorCode = result.err_code == null ? ErrorCodeEnum.Unknown : (ErrorCodeEnum)result.err_code;
                if (result.Verify(config)) return true;
            }
            else errorCode = ErrorCodeEnum.Unknown;
            return false;
        }
        /// <summary>
        /// 查询退款，用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
        /// </summary>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="device_info">调用接口提交的终端设备号</param>
        /// <returns></returns>
        public RefundResult GetRefundByOrder(string transaction_id, string device_info = "WEB")
        {
            return getRefund(new RefundQuery { transaction_id = transaction_id, device_info = device_info });
        }
        /// <summary>
        /// 查询退款，用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
        /// </summary>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="device_info">调用接口提交的终端设备号</param>
        /// <returns></returns>
        public RefundResult GetRefundByLocalOrder(string out_trade_no, string device_info = "WEB")
        {
            return getRefund(new RefundQuery { out_trade_no = out_trade_no, device_info = device_info });
        }
        /// <summary>
        /// 查询退款，用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
        /// </summary>
        /// <param name="refund_id">微信生成的退款单号</param>
        /// <param name="device_info">调用接口提交的终端设备号</param>
        /// <returns></returns>
        public RefundResult GetRefund(string refund_id, string device_info = "WEB")
        {
            return getRefund(new RefundQuery { refund_id = refund_id, device_info = device_info });
        }
        /// <summary>
        /// 查询退款，用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
        /// </summary>
        /// <param name="out_refund_no">商户侧传给微信的退款单号</param>
        /// <param name="device_info">调用接口提交的终端设备号</param>
        /// <returns></returns>
        public RefundResult GetRefundByLocal(string out_refund_no, string device_info = "WEB")
        {
            return getRefund(new RefundQuery { out_refund_no = out_refund_no, device_info = device_info });
        }
        /// <summary>
        /// 查询退款，用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private RefundResult getRefund(RefundQuery value)
        {
            value.SetConfig(config);
            string xml;
            RefundResult result = Config.Client.RequestXml<RefundResult, RefundQuery>("https://api.mch.weixin.qq.com/pay/refundquery", value, out xml, xmlSerializeConfig);
            return result != null && result.Verify(config) ? result : null;
        }
        /// <summary>
        /// 对账单数据分割
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private unsafe static LeftArray<SubString> splitBill(ref SubString value)
        {
            LeftArray<SubString> values = default(LeftArray<SubString>);
            if (value.Length > 1)
            {
                fixed (char* valueFixed = value.String)
                {
                    char* start = valueFixed + value.Start;
                    if (*start == '`')
                    {
                        char* end = start + value.Length, data = ++start;
                        while (++start != end)
                        {
                            if (*start == '`' && *(start - 1) == ',')
                            {
                                values.Add(new SubString((int)(data - valueFixed), (int)(start - data) - 1, value.String));
                                data = ++start;
                                if (start == end) break;
                            }
                        }
                        if (*(end - 1) == '\r') --end;
                        values.Add(new SubString((int)(data - valueFixed), (int)(end - data), value.String));
                    }
                }
            }
            return values;
        }
        /// <summary>
        /// 下载对账单
        /// </summary>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="device_info"></param>
        /// <returns>对账单统计数据 https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=9_6</returns>
        public BillTotal DownloadBill(BillType type, DateTime date, string device_info = "WEB")
        {
            BillQuery query = new BillQuery { bill_type = type, bill_date = date.ToString("yyyyMMdd"), device_info = device_info };
            query.SetConfig(config);
            string text = Config.Client.RequestXml<BillQuery>("https://api.mch.weixin.qq.com/pay/downloadbill", query, xmlSerializeConfig);
            ReturnCode returnCode = AutoCSer.Xml.Parser.Parse<ReturnCode>(text);
            if (returnCode == null)
            {
                LeftArray<SubString> rows = text.split('\n');
                if (rows.Length >= 3)
                {
                    SubString[] rowArray = rows.Array;
                    int endIndex = rows.Length - 1;
                    if (rowArray[endIndex].Length == 0) rows.Length = endIndex--;
                    if (endIndex >= 2 && BillTotal.CheckName(rowArray[endIndex - 1].TrimEnd('\r')) && Bill.CheckName(rowArray[0].TrimEnd('\r'), type))
                    {
                        try
                        {
                            Bill[] bills = new Bill[endIndex - 2];
                            for (int index = 1; index != endIndex; ++index) bills[index - 1] = new Bill(splitBill(ref rowArray[index]), type);
                            return new BillTotal(splitBill(ref rowArray[endIndex]), bills);
                        }
                        catch (Exception error)
                        {
                            AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error, text);
                        }
                        return null;
                    }
                }
            }
            AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, text);
            return null;
        }
        /// <summary>
        /// 二维码长链接转短链接
        /// </summary>
        /// <param name="long_url"></param>
        /// <returns></returns>
        public string GetQrCodeShortUrl(string long_url)
        {
            if (!string.IsNullOrEmpty(long_url))
            {
                QrCodeLongUrl url = new QrCodeLongUrl { long_url = long_url };
                url.SetConfig(config);
                QrCodeShortUrl shortUrl = Config.Client.RequestXml<QrCodeShortUrl, QrCodeLongUrl>("https://api.mch.weixin.qq.com/tools/shorturl", url, xmlSerializeConfig);
                if (shortUrl != null && shortUrl.Verify(config)) return shortUrl.short_url;
            }
            return null;
        }

    }
}
