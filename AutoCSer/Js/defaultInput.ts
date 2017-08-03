/// <reference path = "./base.page.ts" />
'use strict';
//默认值输入框	<input defaultinput="" readonly="readonly" id="XXX" type="text" />
module AutoCSer {
    export class DefaultInput implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, Default: null, IsTrim: true };
        private Id: string;
        private Event: DeclareEvent;
        private Default: string;
        private IsTrim: boolean;

        private Element: HTMLInputElement;
        private IsFocus: boolean;
        private IsEmpty: boolean;
        constructor(Parameter) {
            Pub.GetParameter(this, DefaultInput.DefaultParameter, Parameter);
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                var Element = HtmlElement.$Id(this.Id), Input = Element.Element0() as HTMLInputElement;
                if (Input != this.Element) {
                    this.Element = Input;
                    Element.AddEvent('blur', Pub.ThisFunction(this, this.OnBlur)).Set('readOnly', false);
                    if (this.Default == null) {
                        this.Default = Input.value;
                        this.IsEmpty = true;
                    }
                    else this.IsEmpty = this.Default == Input.value;
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
        }
        private OnBlur() {
            var Element = HtmlElement.$IdElement(this.Id) as HTMLInputElement, Value = Element.value;
            if (this.IsTrim) Value = Value.Trim();
            if (Value == '') {
                Element.value = this.Default;
                this.IsEmpty = true;
            }
            else this.IsEmpty = false;
            this.IsFocus = false;
        }
        GetValue(): string {
            if (!this.IsEmpty) {
                var Element = HtmlElement.$IdElement(this.Id) as HTMLInputElement;
                if (Element == this.Element) {
                    var Value = Element.value;
                    return this.IsTrim ? Value.Trim() : Value;
                }
            }
            return '';
        }
    }
    new AutoCSer.Declare(DefaultInput, 'DefaultInput', 'focus', 'Src');
}