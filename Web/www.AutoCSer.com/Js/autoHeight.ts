/// <reference path = "./base.page.ts" />
'use strict';
//TextArea高度自适应控件	<textarea autoheight="{LineHeight:20}" id="XXX"></textarea>
module AutoCSer {
    export class AutoHeight implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, LineHeight: null, MaxHeight: 0, Timeout: 200 };
        private Id: string;
        private Event: DeclareEvent;
        private LineHeight: number;
        private MaxHeight: number;
        private Timeout: number;

        private Element: HTMLInputElement;
        private MinHeight: number;
        private Div: HtmlElement;
        private SetFunction: Function;
        private Interval: number;
        private IsStart: boolean;
        constructor(Parameter) {
            Pub.GetParameter(this, AutoHeight.DefaultParameter, Parameter);
            var Element = HtmlElement.$IdElement(this.Id);
            if (this.LineHeight == null) this.LineHeight = parseInt(0 + HtmlElement.$AttributeOrStyle(Element, 'lineHeight'));
            this.SetFunction = Pub.ThisFunction(this, this.SetHeight);
            this.Div = HtmlElement.$Create('div').Display(0).AddClass(Element.className).Style('position', 'absolute').To();
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                var Element = HtmlElement.$IdElement(this.Id) as HTMLInputElement;
                if (!this.Element || Element != this.Element) {
                    this.MinHeight = (this.Element = Element).scrollHeight;
                    HtmlElement.$AddEvent(Element, ['blur'], Pub.ThisFunction(this, this.Stop));
                    HtmlElement.$AddEvent(Element, ['keypress','keyup'], this.SetFunction);
                    var Width = HtmlElement.$Width(Element), Css = HtmlElement.$Css(Element);
                    if (!Pub.IsBorder) {
                        Width -= parseInt(0 + Css['border-left-width'], 10) + parseInt(0 + Css['border-right-width'], 10);
                        if (!Pub.IsPadding) Width -= parseInt(0 + Css['padding-left'], 10) + parseInt(0 + Css['padding-right'], 10);
                    }
                    this.Div.Style('width', Width + 'px');
                    if (this.Interval) clearTimeout(this.Interval);
                    this.IsStart = false;
                }
                if (!this.IsStart) {
                    if (!this.Interval) this.Interval = setTimeout(this.SetFunction, this.Timeout);
                    this.IsStart = true;
                }
            }
        }
        private SetHeight() {
            if (this.IsStart) {
                this.Div.Display(1).Html(this.Element.value.ToHTML().replace(/\r\n/g, '<br />').replace(/[\r\n]/g, '<br />'));
                var Height = this.Div.ScrollHeight0() + this.LineHeight;
                if (this.MaxHeight && Height > this.MaxHeight) Height = this.MaxHeight;
                HtmlElement.$SetStyle(this.Element, 'height', Math.max(Height, this.MinHeight) + 'px');
                this.Div.Display(1);
                if (this.Interval) clearTimeout(this.Interval);
                this.Interval = setTimeout(this.SetFunction, this.Timeout);
            }
        }
        private Stop() {
            if (this.Interval) {
                clearTimeout(this.Interval);
                this.Interval = 0;
            }
            this.IsStart = false;
        }
    }
    new AutoCSer.Declare(AutoHeight, 'AutoHeight', 'focus', 'Src');
}