/// <reference path = "./base.page.ts" />
'use strict';
//鼠标覆盖处理	<div mouseover="{}" id="XXX"></div>
var AutoCSer;
(function (AutoCSer) {
    var MouseOver = (function () {
        function MouseOver(Parameter) {
            AutoCSer.Pub.GetParameter(this, MouseOver.DefaultParameter, Parameter);
            AutoCSer.Pub.GetEvents(this, MouseOver.DefaultEvents, Parameter);
            this.Start(this.Event || AutoCSer.DeclareEvent.Default);
        }
        MouseOver.prototype.Start = function (Event) {
            if (!Event.IsGetOnly) {
                var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
                if (Element != this.Element) {
                    this.Element = Element;
                    AutoCSer.HtmlElement.$AddEvent(Element, ['mouseout'], AutoCSer.Pub.ThisEvent(this, this.Out));
                }
                this.OnOver.Function(Event, Element);
            }
        };
        MouseOver.prototype.Out = function (Event) {
            this.OnOut.Function(Event, AutoCSer.HtmlElement.$IdElement(this.Id));
        };
        MouseOver.DefaultParameter = { Id: null, Event: null };
        MouseOver.DefaultEvents = { OnOver: null, OnOut: null };
        return MouseOver;
    }());
    AutoCSer.MouseOver = MouseOver;
    new AutoCSer.Declare(MouseOver, 'MouseOver', 'mouseover', 'AttributeName');
})(AutoCSer || (AutoCSer = {}));
