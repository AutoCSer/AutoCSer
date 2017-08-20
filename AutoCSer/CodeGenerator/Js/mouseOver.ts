/// <reference path = "./base.page.ts" />
'use strict';
//鼠标覆盖处理	<div mouseover="{}" id="XXX"></div>
module AutoCSer {
    export class MouseOver implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null };
        private static DefaultEvents = { OnOver: null, OnOut: null };
        private Id: string;
        private Event: DeclareEvent;
        private OnOver: Events;
        private OnOut: Events;

        private Element: HTMLElement;
        constructor(Parameter) {
            Pub.GetParameter(this, MouseOver.DefaultParameter, Parameter);
            Pub.GetEvents(this, MouseOver.DefaultEvents, Parameter);
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                var Element = HtmlElement.$IdElement(this.Id);
                if (Element != this.Element) {
                    this.Element = Element;
                    HtmlElement.$AddEvent(Element, ['mouseout'], Pub.ThisEvent(this, this.Out));
                }
                this.OnOver.Function(Event, Element);
            }
        }
        private Out(Event) {
            this.OnOut.Function(Event, HtmlElement.$IdElement(this.Id));
        }
    }
    new AutoCSer.Declare(MouseOver, 'MouseOver', 'mouseover', 'AttributeName');
}