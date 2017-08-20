/// <reference path = "./base.page.ts" />
'use strict';
//键盘事件	<input inputkeycode="{Keys:{13:AutoCSer.Pub.Alert}}" id="XXX" />
var AutoCSer;
(function (AutoCSer) {
    var InputKeyCode = (function () {
        function InputKeyCode(Parameter) {
            AutoCSer.Pub.GetParameter(this, InputKeyCode.DefaultParameter, Parameter);
            AutoCSer.Pub.GetEvents(this, InputKeyCode.DefaultEvents, Parameter);
            if (!this.Keys)
                this.Keys = {};
            this.Start(this.Event || AutoCSer.DeclareEvent.Default);
        }
        InputKeyCode.prototype.Start = function (Event) {
            if (!Event.IsGetOnly) {
                var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
                if (Element != this.Element) {
                    this.Element = Element;
                    AutoCSer.HtmlElement.$AddEvent(Element, ['keyup', 'keypress'], AutoCSer.Pub.ThisEvent(this, this.OnKey));
                }
            }
        };
        InputKeyCode.prototype.OnKey = function (Event) {
            var Value = this.Keys[Event.keyCode];
            if (Value)
                Value(Event);
            this.OnAnyKey.Function(Event);
        };
        InputKeyCode.DefaultParameter = { Id: null, Event: null, Keys: null };
        InputKeyCode.DefaultEvents = { OnAnyKey: null };
        return InputKeyCode;
    }());
    AutoCSer.InputKeyCode = InputKeyCode;
    new AutoCSer.Declare(InputKeyCode, 'InputKeyCode', 'focus', 'Src');
})(AutoCSer || (AutoCSer = {}));
