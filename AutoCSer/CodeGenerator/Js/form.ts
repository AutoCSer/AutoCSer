/// <reference path = "./base.page.ts" />
'use strict';
//基本表单	<div FORM="YYY.user" id="XXX"><input name="name1" /><input name="name2" /></div>
//AutoCSer.Form.Forms.XXX.SubmitName='name2';
//AutoCSer.Form.Forms.XXX.OnSubmit.Add(AAA);
//AutoCSer.Form.Forms.XXX.Start({name1:{MaxLength:32,OnVerify:/^[0-9A-Z_\.]+@[0-9A-Z_\.]+$/gi,ErrorInfo:'错误'},name2:{...}});
//AutoCSer.Form.Forms.XXX.GetValue();
module AutoCSer {
    interface IFormElement {
        NullValue: string;
        DefaultValue: string;
        MaxLength: number;
        OnVerify: any;
        ErrorInfo: string;
        OnFormat: Function;
        OnChange: Function;
    }
    export class Form {
        private static DefaultParameter = { Id: null, SubmitName: null };
        private static DefaultEvents = { OnSubmit: null, OnError: null };
        private Id: string;
        private SubmitName: string;
        private OnSubmit: Events;
        private OnError: Events;

        private Elements: { [key: string]: IFormElement };
        private OnCancel: Events;
        private IsDisabled: boolean;
        constructor(Parameter) {
            Pub.GetParameter(this, Form.DefaultParameter, Parameter);
            Pub.GetEvents(this, Form.DefaultEvents, Parameter);
        }
        private static TagNames: { [key: string]: number } = { input: 0, select: 2, textarea: 4 };
        private static InpuTagNames: { [key: string]: number } = { hidden: 1, text: 1, password: 1, checkbox: 3, radio: 5 };
        private static ElementType(Element: HTMLInputElement): number {
            var Value = Form.TagNames[Element.tagName.toLowerCase()];
            return (Value == 0 ? Form.InpuTagNames[Element.type.toLowerCase()] : Value) || 0;
        }
        private static IsElement(Element: HTMLInputElement): boolean {
            return !!(Element.name && Form.ElementType(Element));
        }
        private GetElements(): HTMLInputElement[] {
            return HtmlElement.$Id(this.Id).Childs(Form.IsElement).GetElements() as HTMLInputElement[];
        }
        private Start(Elements: { [key: string]: IFormElement }, IsFocus: boolean = true) {
            this.Elements = Elements || {};
            for (var ElementArray = this.GetElements(), Index = -1; ++Index - ElementArray.length;) {
                var Element = ElementArray[Index];
                if (Element.readOnly) {
                    if (this.Elements[Element.name]) this.Elements[Element.name].NullValue = Element.value.Trim();
                    else this.Elements[Element.name] = { NullValue: Element.value.Trim(), DefaultValue: null, MaxLength: 0, OnVerify: null, ErrorInfo: null, OnFormat: null, OnChange: null };
                    Element.readOnly = false;
                }
                HtmlElement.$AddEvent(Element, ['keydown'], Pub.ThisEvent(this, this.KeyDown));
                var Type = Form.ElementType(Element);
                if (Type == 1 || Type == 4) {
                    var FormElement = this.Elements[Element.name];
                    if (FormElement && FormElement.MaxLength) Element.maxLength = FormElement.MaxLength;
                    HtmlElement.$AddEvent(Element, ['blur'], Pub.ThisEvent(this, this.Format));
                    HtmlElement.$AddEvent(Element, ['focus'], Pub.ThisEvent(this, this.Focus));
                }
            }
            HtmlElement.$Name('submit', HtmlElement.$IdElement(this.Id)).Cursor('pointer').AddEvent('click', Pub.ThisEvent(this, this.Submit));
            this.Disabled(false);
            this.Cancel(false);
            if (IsFocus) {
                try { ElementArray[0].focus(); }
                catch (e) { }
            }
        }
        private Submit() {
            var Value = this.GetValue();
            if (Value) this.OnSubmit.Function(Value);
        }
        private Cancel(IsReset: boolean, IsOnCancel = true) {
            for (var Elements = this.GetElements(), Index = -1; ++Index - Elements.length;) {
                var Element = Elements[Index], FormElement = this.Elements[Element.name], Value = FormElement ? (FormElement.DefaultValue != null ? FormElement.DefaultValue : FormElement.NullValue) : null;
                if (Value != null) {
                    switch (Form.ElementType(Element)) {
                        case 1: case 4:
                            if (Element.value == '' || IsReset == null || IsReset) Element.value = Value ? Value : '';
                            break;
                        case 2:
                            var SelectElement = Element as Object as HTMLSelectElement;
                            if ((SelectElement.options[0] as HTMLInputElement).value == '') SelectElement.selectedIndex = Value as Object as number;
                            else {
                                for (var OptionIndex = SelectElement.options.length - 1; OptionIndex > 0 && (SelectElement.options[OptionIndex] as HTMLInputElement).value != FormElement.DefaultValue; OptionIndex--);
                                SelectElement.selectedIndex = OptionIndex;
                            }
                            break;
                        case 3:
                            HtmlElement.$(Element).Set('checked', Value ? Value : false);
                            break;
                        case 5:
                            HtmlElement.$(HtmlElement.$Name(Element.name, HtmlElement.$IdElement(this.Id)).FirstElement(function (Element) { return Element.value == Value; })).Set('checked', true);
                            break;
                    }
                    if (FormElement.OnChange) FormElement.OnChange();
                }
            }
            if (IsOnCancel && this.OnCancel) this.OnCancel.Function();
        }
        private KeyDown(Event: BrowserEvent) {
            var Key = (Event.Event as KeyboardEvent).shiftKey || Event.keyCode, Element = Event.srcElement as HTMLInputElement, Type = Form.ElementType(Element), Next;
            if (Type == 1 || Type == 3 || Type == 5) {
                if (Type == 3 || Type == 5) {
                    if (Key == 13) Element.click();
                    else if (Key == 39 || Key == 40) Next = 1;
                    else if (Key == 37 || Key == 38) Next = -1;
                }
                else if (Key == 40) Next = 1;
                else if (Key == 38) Next = -1;
                else if (Key == 13) {
                    if (Element.name == this.SubmitName) {
                        Event.CancelBubble();
                        this.Submit();
                    }
                    else Next = 1;
                }
            }
            else if (Type == 2 && Key == 13) Next = 1;
            if (Next) {
                for (var Elements = this.GetElements(), Index = -1; ++Index - Elements.length;) {
                    if (Elements[Index] == Element) {
                        if ((Index += Next) >= 0 && Index < Elements.length) Elements[Index].focus();
                        break;
                    }
                }
                Event.CancelBubble();
            }
        }
        private Format(Event: BrowserEvent) {
            var Element = Event.srcElement as HTMLInputElement, FormElement = this.Elements[Element.name], Type = Form.ElementType(Element);
            if (Type == 1 || Type == 4) Element.value = Element.value.replace(/[\x00]/g, '');
            if (FormElement) {
                if (FormElement.OnFormat) {
                    var Value = FormElement.OnFormat(this.GetElementValue(Element)).toString();
                    if (Type == 1 || Type == 4) Element.value = Value == '' ? (FormElement.NullValue || '') : Value;
                    else if (Type != 2 && Type != 3 && Type != 5) Element.innerHTML = Value;
                }
                if (FormElement.NullValue && (Type == 1 || Type == 4) && (!Element.value)) Element.value = FormElement.NullValue;
                if (Type == 4 && FormElement.MaxLength && FormElement.MaxLength && Element.value != FormElement.NullValue && Element.value.length > FormElement.MaxLength) {
                    Element.value = Element.value.substring(0, FormElement.MaxLength);
                }
            }
        }
        private Focus(Event: BrowserEvent) {
            var Element = Event.srcElement as HTMLInputElement, FormElement = this.Elements[Element.name];
            if (FormElement && FormElement.NullValue) {
                var Value = Element.value.Trim();
                if (Value == FormElement.NullValue) Element.value = '';
                else if (Value == '') Element.value = FormElement.NullValue;
            }
            Element.select();
        }
        private GetElementValue(Element: HTMLInputElement): any {
            var Type = Form.ElementType(Element);
            if (Type == 1 || Type == 4) {
                var FormElement = this.Elements[Element.name];
                return FormElement && Element.value.Trim() == FormElement.NullValue ? '' : Element.value;
            }
            if (Type == 3) return Element.checked;
            if (Type == 5) return (HtmlElement.$Name(Element.name, HtmlElement.$IdElement(this.Id)).FirstElement(Form.IsChecked) as HTMLInputElement).value;
            if (Type == 2) {
                var Value = ((Element as Object as HTMLSelectElement).options[(Element as Object as HTMLSelectElement).selectedIndex] as HTMLInputElement).value;
                return Value ? Value : null;
            }
            return Element.innerHTML;
        }
        private static IsChecked(Element) {
            return Element.checked;
        }
        private GetValue(Name: string = null): any {
            if (Name) {
                for (var Elements = this.GetElements(), Index = -1; ++Index - Elements.length;) {
                    var Element = Elements[Index];
                    if (Element.name == Name) return this.GetElementValue(Element);
                }
            }
            else {
                for (var ReturnValue = {}, Elements = this.GetElements(), Index = -1; ++Index - Elements.length;) {
                    var Element = Elements[Index], FormElement = this.Elements[Element.name], Value = this.GetElementValue(Element), OnVerify = FormElement ? FormElement.OnVerify : null;
                    if (OnVerify && (!(typeof (OnVerify) == 'function' && OnVerify.toString().charAt(0) != '/' ? OnVerify(Value) : Value.toString().match(OnVerify)))) {
                        ReturnValue = null;
                        Element.focus();
                        if (this.OnError.Get().length) this.OnError.Function(FormElement);
                        else if (FormElement.ErrorInfo) AutoCSer.Pub.Alert(FormElement.ErrorInfo);
                        break;
                    }
                    ReturnValue[Element.name] = Value;
                }
                return ReturnValue;
            }
        }
        private Disabled(IsDisabled = true) {
            var Cursor = (this.IsDisabled = IsDisabled) ? 'auto' : 'pointer';
            for (var Elements = this.GetElements(), Index = Elements.length; --Index >= 0;)	Elements[Index].disabled = this.IsDisabled;
            HtmlElement.$Name('submit', HtmlElement.$IdElement(this.Id)).Disabled(this.IsDisabled).Cursor(Cursor);
        }
        static Forms: { [key: string]: Form } = {};
        static GetForms() {
            for (var Childs = HtmlElement.$('@form').GetElements(), Index = Childs.length; --Index >= 0; this.Forms[Childs[Index].id] = new Form({ Id: Childs[Index].id }));
        }
    }
    Pub.OnLoad(Form.GetForms, Form, true);
}