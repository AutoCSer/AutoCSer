/// <reference path = "./base.page.ts" />
'use strict';
//默认密码输入框	<input defaultpassword="{PasswordId:'YYY'}" id="XXX" type="text" /><input id="YYY" type="text" />
var AutoCSer;
(function (AutoCSer) {
    var DefaultPassword = (function () {
        function DefaultPassword(Parameter) {
            AutoCSer.Pub.GetParameter(this, DefaultPassword.DefaultParameter, Parameter);
            this.Start(this.Event || AutoCSer.DeclareEvent.Default);
        }
        DefaultPassword.prototype.Start = function (Event) {
            if (!Event.IsGetOnly) {
                var Element = AutoCSer.HtmlElement.$Id(this.Id), Input = Element.Element0(), Password = AutoCSer.HtmlElement.$Id(this.PasswordId);
                if (Input != this.Element) {
                    this.Element = Input;
                    Password.AddEvent('blur', AutoCSer.Pub.ThisFunction(this, this.OnBlur));
                }
                Element.Display(0);
                Password.Display(1).Focus0();
            }
        };
        DefaultPassword.prototype.OnBlur = function () {
            var Password = AutoCSer.HtmlElement.$Id(this.PasswordId);
            if (!Password.Element0().value) {
                Password.Display(0);
                AutoCSer.HtmlElement.$Id(this.Id).Display(1);
            }
        };
        DefaultPassword.DefaultParameter = { Id: null, Event: null, PasswordId: null };
        return DefaultPassword;
    }());
    AutoCSer.DefaultPassword = DefaultPassword;
    new AutoCSer.Declare(DefaultPassword, 'DefaultPassword', 'focus', 'Src');
})(AutoCSer || (AutoCSer = {}));
