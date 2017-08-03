/// <reference path = "./base.page.ts" />
'use strict';
//鼠标拖拽	<div body="true" floatcenter="true" id="YYY" style="display:none"><div mousemove="{MoveId:'YYY'}" id="XXX"></div></div>
module AutoCSer {
    export class MouseMove implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, MoveId: null };
        private Id: string;
        private Event: DeclareEvent;
        private MoveId: string;

        private Element: HTMLElement;
        private MoveEvent: (Event: Event) => any;
        private StopEvent: Function;
        private ClientX: number;
        private ClientY: number;
        private Left: number;
        private Top: number;
        private IsStart: boolean;
        constructor(Parameter) {
            Pub.GetParameter(this, MouseMove.DefaultParameter, Parameter);
            this.MoveEvent = Pub.ThisEvent(this, this.Move);
            this.StopEvent = Pub.ThisFunction(this, this.Stop);
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                if (this.IsStart) this.Stop();
                var Element = HtmlElement.$IdElement(this.Id);
                if (Element != this.Element) {
                    this.Element = Element;
                    HtmlElement.$(document.body).AddEvent('mousedown,mouseup,blur', this.StopEvent).AddEvent('mousemove', this.MoveEvent);
                }
                var Element = HtmlElement.$IdElement(this.MoveId);
                Element.style.position = 'absolute';
                this.Left = Element.offsetLeft;
                this.Top = Element.offsetTop;
                this.ClientX = Event.clientX;
                this.ClientY = Event.clientY;
                this.IsStart = true;
            }
        }
        private Move(Event: BrowserEvent) {
            HtmlElement.$Id(this.MoveId).Left(this.Left + Event.clientX - this.ClientX).Top(this.Top + Event.clientY - this.ClientY);
        }
        private Stop() {
            if (this.IsStart) {
                HtmlElement.$(document.body).DeleteEvent('mousedown,mouseup,blur', this.StopEvent).DeleteEvent('mousemove', this.MoveEvent);
                this.IsStart = false;
            }
        }
    }
    new AutoCSer.Declare(MouseMove, 'MouseMove', 'mousedown', 'AttributeName');
}