/// <reference path = "./base.page.ts" />
'use strict';
module AutoCSer {
    export interface ITouchTopEvents {
        OnTop: Function;
    }
    export class TouchTopParameter {
        Top: number;
        CheckTimeout: number;
    }
    export class TouchTop extends TouchTopParameter {
        private static DefaultParameter: TouchTopParameter = { Top: 12, CheckTimeout:200 };
        private static DefaultEvents: ITouchTopEvents = { OnTop: null };
        OnTop: Events;

        private CheckFunction: Function;
        private StartTop: number;
        private Step: number;
        private EndTop: number;
        private Interval: number;
        constructor(Parameter: TouchTopParameter = null) {
            super();
            Pub.GetParameter(this, TouchTop.DefaultParameter, Parameter);
            Pub.GetEvents(this, TouchTop.DefaultEvents, Parameter);

            this.CheckFunction = AutoCSer.Pub.ThisEvent(this, this.Check);
            HtmlElement.$AddEvent(document as Object as HTMLElement, ['touchstart'], Pub.ThisEvent(this, this.Start));
            HtmlElement.$AddEvent(document as Object as HTMLElement, ['touchmove'], Pub.ThisEvent(this, this.Move));
            HtmlElement.$AddEvent(document as Object as HTMLElement, ['touchend'], Pub.ThisEvent(this, this.End));
            HtmlElement.$AddEvent(document as Object as HTMLElement, ['touchcancel'], Pub.ThisEvent(this, this.Cancel));
        }
        private Start(Event: BrowserEvent) {
            this.Cancel();
            this.Step = 1;
            this.StartTop = this.EndTop = Event.touches[0].clientY;
        }
        private Move(Event: BrowserEvent) {
            if (this.Step == 1) this.Step = 2;
            if (this.Step == 2) {
                this.EndTop = Event.touches[0].clientY;
                this.Check();
            }
        }
        private Check() {
            if (this.Step == 2 && (this.EndTop - this.StartTop) > this.Top && HtmlElement.$GetScrollTop() == 0) {
                this.Cancel();
                this.OnTop.Function();
            }
        }
        private End(Event: BrowserEvent) {
            if (this.Step == 2) {
                this.EndTop = Event.changedTouches[0].clientY;
                if (!this.Interval) this.Interval = setInterval(this.CheckFunction, this.CheckTimeout);
                this.Check();
            }
        }
        private Cancel() {
            this.Step = 0;
            if (this.Interval) {
                clearInterval(this.Interval);
                this.Interval = 0;
            }
        }

        static Default: TouchTop = new TouchTop();
    }
}