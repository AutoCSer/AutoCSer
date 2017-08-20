/// <reference path = "./base.page.ts" />
'use strict';
//默认值输入框	<input defaultinput="" readonly="readonly" id="XXX" type="text" />
var AutoCSer;
(function (AutoCSer) {
    var DefaultInput = (function () {
        function DefaultInput(Parameter) {
            AutoCSer.Pub.GetParameter(this, DefaultInput.DefaultParameter, Parameter);
            this.Start(this.Event || AutoCSer.DeclareEvent.Default);
        }
        DefaultInput.prototype.Start = function (Event) {
            if (!Event.IsGetOnly) {
                var Element = AutoCSer.HtmlElement.$Id(this.Id), Input = Element.Element0();
                if (Input != this.Element) {
                    this.Element = Input;
                    Element.AddEvent('blur', AutoCSer.Pub.ThisFunction(this, this.OnBlur)).Set('readOnly', false);
                    if (this.Default == null) {
                        this.Default = Input.value;
                        this.IsEmpty = true;
                    }
                    else
                        this.IsEmpty = this.Default == Input.value;
                    this.IsFocus = false;
                }
                if (!this.IsFocus) {
                    if (this.IsEmpty) {
                        Input.value = '';
                        this.IsEmpty = false;
                    }
                    Input.focus();
                    this.IsFocus = true;
                }
            }
        };
        DefaultInput.prototype.OnBlur = function () {
            var Element = AutoCSer.HtmlElement.$IdElement(this.Id), Value = Element.value;
            if (this.IsTrim)
                Value = Value.Trim();
            if (Value == '') {
                Element.value = this.Default;
                this.IsEmpty = true;
            }
            else
                this.IsEmpty = false;
            this.IsFocus = false;
        };
        DefaultInput.prototype.GetValue = function () {
            if (!this.IsEmpty) {
                var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
                if (Element == this.Element) {
                    var Value = Element.value;
                    return this.IsTrim ? Value.Trim() : Value;
                }
            }
            return '';
        };
        DefaultInput.DefaultParameter = { Id: null, Event: null, Default: null, IsTrim: true };
        return DefaultInput;
    }());
    AutoCSer.DefaultInput = DefaultInput;
    new AutoCSer.Declare(DefaultInput, 'DefaultInput', 'focus', 'Src');
})(AutoCSer || (AutoCSer = {}));
