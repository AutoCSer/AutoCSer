/// <reference path = "./base.page.ts" />
'use strict';
//鼠标覆盖图片修改	<img mouseimage="{OverSrc:'over.jpg',OutSrc:'out.jpg'}" id="XXX" />
module AutoCSer {
    export class MouseImage implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, OverSrc: null, OutSrc: null };
        private Id: string;
        private Event: DeclareEvent;
        private OverSrc: string;
        private OutSrc: string;

        private Element: HTMLImageElement;
        constructor(Parameter) {
            Pub.GetParameter(this, MouseImage.DefaultParameter, Parameter);
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                var Element = HtmlElement.$IdElement(this.Id) as HTMLImageElement;
                if (Element != this.Element) {
                    this.Element = Element;
                    HtmlElement.$AddEvent(Element, ['mouseout'], Pub.ThisEvent(this, this.Out));
                }
                Element.src = this.OverSrc;
            }
        }
        Out(Event: BrowserEvent) {
            (HtmlElement.$IdElement(this.Id) as HTMLImageElement).src = this.OutSrc;
        }
    }
    new AutoCSer.Declare(MouseImage, 'MouseImage', 'mouseover', 'AttributeName');
}