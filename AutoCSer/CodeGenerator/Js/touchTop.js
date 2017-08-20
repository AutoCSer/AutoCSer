/// <reference path = "./base.page.ts" />
'use strict';
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var AutoCSer;
(function (AutoCSer) {
    var TouchTopParameter = (function () {
        function TouchTopParameter() {
        }
        return TouchTopParameter;
    }());
    AutoCSer.TouchTopParameter = TouchTopParameter;
    var TouchTop = (function (_super) {
        __extends(TouchTop, _super);
        function TouchTop(Parameter) {
            if (Parameter === void 0) { Parameter = null; }
            _super.call(this);
            AutoCSer.Pub.GetParameter(this, TouchTop.DefaultParameter, Parameter);
            AutoCSer.Pub.GetEvents(this, TouchTop.DefaultEvents, Parameter);
            this.CheckFunction = AutoCSer.Pub.ThisEvent(this, this.Check);
            AutoCSer.HtmlElement.$AddEvent(document, ['touchstart'], AutoCSer.Pub.ThisEvent(this, this.Start));
            AutoCSer.HtmlElement.$AddEvent(document, ['touchmove'], AutoCSer.Pub.ThisEvent(this, this.Move));
            AutoCSer.HtmlElement.$AddEvent(document, ['touchend'], AutoCSer.Pub.ThisEvent(this, this.End));
            AutoCSer.HtmlElement.$AddEvent(document, ['touchcancel'], AutoCSer.Pub.ThisEvent(this, this.Cancel));
        }
        TouchTop.prototype.Start = function (Event) {
            this.Cancel();
            this.Step = 1;
            this.StartTop = this.EndTop = Event.touches[0].clientY;
        };
        TouchTop.prototype.Move = function (Event) {
            if (this.Step == 1)
                this.Step = 2;
            if (this.Step == 2) {
                this.EndTop = Event.touches[0].clientY;
                this.Check();
            }
        };
        TouchTop.prototype.Check = function () {
            if (this.Step == 2 && (this.EndTop - this.StartTop) > this.Top && AutoCSer.HtmlElement.$GetScrollTop() == 0) {
                this.Cancel();
                this.OnTop.Function();
            }
        };
        TouchTop.prototype.End = function (Event) {
            if (this.Step == 2) {
                this.EndTop = Event.changedTouches[0].clientY;
                if (!this.Interval)
                    this.Interval = setInterval(this.CheckFunction, this.CheckTimeout);
                this.Check();
            }
        };
        TouchTop.prototype.Cancel = function () {
            this.Step = 0;
            if (this.Interval) {
                clearInterval(this.Interval);
                this.Interval = 0;
            }
        };
        TouchTop.DefaultParameter = { Top: 12, CheckTimeout: 200 };
        TouchTop.DefaultEvents = { OnTop: null };
        TouchTop.Default = new TouchTop();
        return TouchTop;
    }(TouchTopParameter));
    AutoCSer.TouchTop = TouchTop;
})(AutoCSer || (AutoCSer = {}));
