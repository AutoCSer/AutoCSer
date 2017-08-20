/// <reference path = "./base.page.ts" />
'use strict';
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var AutoCSer;
(function (AutoCSer) {
    var PollParameter = (function () {
        function PollParameter() {
        }
        return PollParameter;
    }());
    AutoCSer.PollParameter = PollParameter;
    var Poll = (function (_super) {
        __extends(Poll, _super);
        function Poll(Parameter) {
            if (Parameter === void 0) { Parameter = null; }
            _super.call(this);
            AutoCSer.Pub.GetParameter(this, Poll.DefaultParameter, Parameter);
            AutoCSer.Pub.GetEvents(this, Poll.DefaultEvents, Parameter);
            if (!this.Query)
                this.Query = {};
            if (this.Path == null)
                this.Path = this.IsView ? '/poll.html' : 'poll';
            this.GetFunction = AutoCSer.Pub.ThisFunction(this, this.Get);
            this.VerifyFunction = AutoCSer.Pub.ThisFunction(this, this.Verify);
            this.OnVerifyFunction = AutoCSer.Pub.ThisFunction(this, this.OnVerify);
            AutoCSer.IndexPool.Push(this);
        }
        Poll.prototype.Start = function (Verify) {
            if (Verify != null)
                this.VerifyInfo = Verify;
            if (!this.Identity)
                setTimeout(this.GetFunction, this.Identity = 1);
        };
        Poll.prototype.Close = function () {
            AutoCSer.IndexPool.Pop(this);
        };
        Poll.prototype.Stop = function () {
            this.Identity = 0;
        };
        Poll.prototype.Get = function () {
            if (this.Identity) {
                if (this.OnMessage.Get().length) {
                    AutoCSer.Pub.AppendJs((this.Domain == null ? '//__POLLDOMAIN__' : (this.Domain ? '//' + this.Domain : '')) + '__AJAX__?__AJAXCALL__=' + this.Path + '&__CALLBACK__=' + AutoCSer.IndexPool.ToString(this) + '.OnGet' + (this.IsView ? 'View' : '') + '&__JSON__=' + AutoCSer.Pub.ToJson({ verify: this.VerifyInfo, query: this.Query }, true, false).Escape() + (AutoCSer.Pub.IE ? '&t=' + (new Date).getTime() : ''), null, null, AutoCSer.Pub.ThisFunction(this, this.OnError, [this.Identity]));
                }
                else
                    setTimeout(this.GetFunction, 1000);
            }
        };
        Poll.prototype.OnGet = function (Value) {
            this.OnGetView(Value ? Value.__AJAXRETURN__ : null);
        };
        Poll.prototype.OnGetView = function (Value) {
            if (this.Identity) {
                ++this.Identity;
                if (Value) {
                    if (!this.VerifyPath || Value.isVerify) {
                        this.VerifyTimeout = 100;
                        if (this.ReturnName) {
                            if (Value[this.ReturnName])
                                this.OnMessage.Function(Value[this.ReturnName]);
                        }
                        else
                            this.OnMessage.Function(Value);
                        if (this.Identity)
                            setTimeout(this.GetFunction, 100);
                    }
                    else {
                        if ((this.VerifyTimeout *= 2) > 2000)
                            this.VerifyTimeout = 2000;
                        this.Verify();
                    }
                }
                else
                    setTimeout(this.GetFunction, 2000);
            }
        };
        Poll.prototype.OnError = function (Event, Identity) {
            if (Identity == this.Identity)
                setTimeout(AutoCSer.Pub.ThisFunction(this, this.Check, [Identity]), 2000);
        };
        Poll.prototype.Check = function (Identity) {
            if (Identity == this.Identity)
                setTimeout(this.GetFunction, 8000);
        };
        Poll.prototype.Verify = function () {
            AutoCSer.HttpRequest.Post(this.VerifyPath, null, this.OnVerifyFunction);
        };
        Poll.prototype.OnVerify = function (Value) {
            if (Value && Value.isVerify) {
                this.VerifyInfo = Value.__AJAXRETURN__;
                if (this.Identity)
                    setTimeout(this.GetFunction, this.VerifyTimeout);
            }
            else if (this.Identity)
                setTimeout(this.VerifyFunction, 8000);
        };
        Poll.DefaultParameter = { Domain: null, Path: null, VerifyPath: 'poll.Verify', Query: null, IsView: true, ReturnName: null, VerifyTimeout: 100 };
        Poll.DefaultEvents = { OnMessage: null };
        Poll.Default = new Poll();
        return Poll;
    }(PollParameter));
    AutoCSer.Poll = Poll;
})(AutoCSer || (AutoCSer = {}));
