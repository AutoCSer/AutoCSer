'use strict';
var AutoCSer;
(function (AutoCSer) {
    var EmailDomain = (function () {
        function EmailDomain() {
        }
        EmailDomain.Get = function (Email) {
            var Domain = Email.split('@')[1], Key = Domain.toLowerCase();
            return this.Mails[Key] ? 'http://mail.' + Key : (this.Domains[Key] || ('http://' + Domain));
        };
        EmailDomain.Mails = {
            '11185.cn': 1,
            '128.com': 1,
            '163.com': 1,
            '17173.com': 1,
            '173.com': 1,
            '189.cn': 1,
            '21cn.com': 1,
            '263.net': 1,
            '263.com': 1,
            'aliyun.com': 1,
            'aol.com': 1,
            'bxemail.com': 1,
            'china.com': 1,
            'chinaacc.com': 1,
            'citiz.net': 1,
            'cntv.cn': 1,
            'hainan.net': 1,
            'hainan.com': 1,
            'hexun.com': 1,
            'qq.com': 1,
            'sina.com': 1,
            'sina.cn': 1,
            'sogou.com': 1,
            'sohu.com': 1,
            'tianya.cn': 1,
            'tom.com': 1,
            'wo.cn': 1,
            'yahoo.com': 1
        };
        EmailDomain.Domains = {
            'gmail.com': 'https://mail.google.com/',
            '263.net.cn': 'http://mail.263.net/',
            'x263.net': 'http://mail.263.net/',
            'vnet.citiz.net': 'http://mail.citiz.net/'
        };
        return EmailDomain;
    }());
    AutoCSer.EmailDomain = EmailDomain;
})(AutoCSer || (AutoCSer = {}));
