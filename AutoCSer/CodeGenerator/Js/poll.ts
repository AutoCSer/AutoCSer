/// <reference path = "./base.page.ts" />
'use strict';
module AutoCSer {
    export interface IPollReturn extends IHttpRequestReturn {
        isVerify: boolean;
    }
    export interface IPollEvents {
        OnMessage: (Message: IPollReturn) => any;
    }
    export class PollParameter {
        Domain: string;
        Path: string;
        VerifyPath: string;
        Query: Object;
        IsView: boolean;
        ReturnName: string;
        VerifyTimeout: number;
    }
    export class Poll extends PollParameter implements IIndexPool {
        private static DefaultParameter: PollParameter = { Domain: null, Path: null, VerifyPath: 'poll.Verify', Query: null, IsView: true, ReturnName: null, VerifyTimeout: 100 };
        private static DefaultEvents: IPollEvents = { OnMessage: null };
        OnMessage: Events;

        PoolIndex: number;
        PoolIdentity: number;
        private Identity: number;
        private GetFunction: Function;
        private VerifyFunction: Function;
        private OnVerifyFunction: Function;
        private VerifyInfo: Object;
        constructor(Parameter: PollParameter = null) {
            super();
            Pub.GetParameter(this, Poll.DefaultParameter, Parameter);
            Pub.GetEvents(this, Poll.DefaultEvents, Parameter);
            if (!this.Query) this.Query = {};
            if (this.Path == null) this.Path = this.IsView ? '/poll.html' : 'poll';
            this.GetFunction = Pub.ThisFunction(this, this.Get);
            this.VerifyFunction = Pub.ThisFunction(this, this.Verify);
            this.OnVerifyFunction = Pub.ThisFunction(this, this.OnVerify);
            IndexPool.Push(this);
        }
        Start(Verify: Object) {
            if (Verify != null) this.VerifyInfo = Verify;
            if (!this.Identity) setTimeout(this.GetFunction, this.Identity = 1);
        }
        private Close() {
            IndexPool.Pop(this);
        }
        Stop() {
            this.Identity = 0;
        }
        private Get () {
            if (this.Identity) {
                if (this.OnMessage.Get().length) {
                    Pub.AppendJs((this.Domain == null ? '//__POLLDOMAIN__' : (this.Domain ? '//' + this.Domain : '')) + '__AJAX__?__AJAXCALL__=' + this.Path + '&__CALLBACK__=' + IndexPool.ToString(this) + '.OnGet' + (this.IsView ? 'View' : '') + '&__JSON__=' + Pub.ToJson({ verify: this.VerifyInfo, query: this.Query }, true, false).Escape() + (Pub.IE ? '&t=' + (new Date).getTime() : ''), null, null, Pub.ThisFunction(this, this.OnError, [this.Identity]));
                }
                else setTimeout(this.GetFunction, 1000);
            }
        }
        private OnGet(Value: IHttpRequestReturn) {
            this.OnGetView(Value ? Value.__AJAXRETURN__ : null);
        }
        private OnGetView(Value: IPollReturn) {
            if (this.Identity) {
                ++this.Identity;
                if (Value) {
                    if (!this.VerifyPath || Value.isVerify) {
                        this.VerifyTimeout = 100;
                        if (this.ReturnName) {
                            if (Value[this.ReturnName]) this.OnMessage.Function(Value[this.ReturnName]);
                        }
                        else this.OnMessage.Function(Value);
                        if (this.Identity) setTimeout(this.GetFunction, 100);
                    }
                    else {
                        if ((this.VerifyTimeout *= 2) > 2000) this.VerifyTimeout = 2000;
                        this.Verify();
                    }
                }
                else setTimeout(this.GetFunction, 2000);
            }
        }
        private OnError(Event: Event, Identity: number) {
            if (Identity == this.Identity) setTimeout(Pub.ThisFunction(this, this.Check, [Identity]), 2000);
        }
        private Check(Identity: number) {
            if (Identity == this.Identity) setTimeout(this.GetFunction, 8000);
        }
        private Verify() {
            HttpRequest.Post(this.VerifyPath, null, this.OnVerifyFunction);
        }
        private OnVerify(Value: IPollReturn) {
            if (Value && Value.isVerify) {
                this.VerifyInfo = Value.__AJAXRETURN__;
                if (this.Identity) setTimeout(this.GetFunction, this.VerifyTimeout);
            }
            else if (this.Identity) setTimeout(this.VerifyFunction, 8000);
        }
        static Default: Poll = new Poll();
    }
}