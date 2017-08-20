/// <reference path = "./base.page.ts" />
'use strict';
//输入框输入长度控件	<input inputlength="{Length:500,SkinId:'YYY',ErrorSkinId:'ZZZ'}" id="XXX" /><div id="YYY" SKIN="true" style="display:none">还可以输入=@InputLength字</div><div id="ZZZ" SKIN="true" style="display:none">已经超出=@InputLength字</div>
module AutoCSer {
    export class InputLength implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, Length: 0, SkinId: null, LengthName: 'InputLength', SkinView: null, ErrorSkinId: null, ErrorLengthName: 'InputLength', ErrorSkinView: null };
        private Id: string;
        private Event: DeclareEvent;
        private Length: number;
        private SkinId: string;
        private LengthName: string;
        private SkinView: Object;
        private ErrorSkinId: string;
        private ErrorLengthName: string;
        private ErrorSkinView: Object;

        private Element: HTMLInputElement;
        constructor(Parameter) {
            Pub.GetParameter(this, InputLength.DefaultParameter, Parameter);
            if (!this.SkinView) this.SkinView = {};
            if (!this.ErrorSkinView) this.ErrorSkinView = {};
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                var Element = HtmlElement.$IdElement(this.Id) as HTMLInputElement;
                if (Element != this.Element) {
                    this.Element = Element;
                    HtmlElement.$AddEvent(Element, ['keypress', 'keyup', 'blur'], Pub.ThisEvent(this, this.Check));
                }
            }
        }
        private Check() {
            var Length = (HtmlElement.$IdElement(this.Id) as HTMLInputElement).value.length;
            if (Length <= this.Length) {
                if (this.ErrorSkinId) Skin.Skins[this.ErrorSkinId].Hide();
                if (this.SkinId) {
                    this.SkinView[this.LengthName] = this.Length - Length;
                    Skin.Skins[this.SkinId].Show(this.SkinView);
                }
            }
            else {
                if (this.SkinId) Skin.Skins[this.SkinId].Hide();
                if (this.ErrorSkinId) {
                    this.ErrorSkinView[this.ErrorLengthName] = Length - this.Length;
                    Skin.Skins[this.ErrorSkinId].Show(this.ErrorSkinView);
                }
            }
        }
        GetValue () {
            var Value = (HtmlElement.$IdElement(this.Id) as HTMLInputElement).value;
            return Value.length <= this.Length ? Value : null;
        }
    }
    new AutoCSer.Declare(InputLength, 'InputLength', 'focus', 'Src');
}