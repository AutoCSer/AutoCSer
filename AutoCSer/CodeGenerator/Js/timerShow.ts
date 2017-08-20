/// <reference path = "./base.page.ts" />
'use strict';
//HTML高度更新模拟动画
//AutoCSer.Declare.Getters.TimerShow(Id).Show();
//AutoCSer.Declare.Getters.TimerShow(Id).Hide();
module AutoCSer {
    interface ITimerShowOption {
        Position: string;
        Overflow: string;
        Height: number;
        Width: number;
    }
    export class TimerShow {
        private static DefaultParameter = { Id: null, Time: 250, Type: 'height' };
        private Id: string;
        private Time: number;
        private Type: string;

        private Element: HtmlElement;
        private Option: ITimerShowOption;
        private NextShowFunction: Function;
        private NextHideFunction: Function;
        private OnShowed: Function;
        private Interval: number;
        private IsShow: boolean;
        private StartTime: number;
        constructor(Parameter) {
            Pub.GetParameter(this, TimerShow.DefaultParameter, Parameter);
            this.NextShowFunction = Pub.ThisFunction(this, this.NextShow);
            this.NextHideFunction = Pub.ThisFunction(this, this.NextHide);
        }
        private End() {
            if (this.Interval) {
                clearTimeout(this.Interval);
                this[this.IsShow ? 'ShowEnd' : 'HideEnd']();
                if (this.OnShowed) this.OnShowed();
                this.Interval = null;
            }
        }
        private Show(OnShowed) {
            this.End();
            this.IsShow = true;
            this.OnShowed = OnShowed;
            this.Element = HtmlElement.$Id(this.Id);
            this.Option = {
                Position: this.Element.Style0('position'),
                Overflow: this.Element.Style0('overflow'),
                Height: this.Element.Opacity(0).Style('position', 'absolute').Display(1).Height0(),
                Width: this.Element.Width0()
            };
            this.Element.Style(this.Type, '0px').Style('overflow', 'hidden').Display('block').Opacity(100).Style('position', this.Option.Position);//.Css('margin,padding','0px')
            this.StartTime = new Date().getTime();
            this.Interval = setInterval(this.NextShowFunction, 50);
        }
        private NextShow() {
            var Time = new Date().getTime() - this.StartTime;
            if (Time < this.Time) this.Element.Style(this.Type, (this.Type == 'height' ? this.Option.Height : this.Option.Width) * Time / this.Time + 'px');
            else this.End();
        }
        private ShowEnd() {
            this.Element.Display(1).Style('overflow', this.Option.Overflow).Style(this.Type, null);//.Css('margin,padding','')
        }
        private Hide(OnShowed) {
            this.End();
            this.IsShow = false;
            this.OnShowed = OnShowed;
            this.Element = HtmlElement.$Id(this.Id).Display('block');
            this.Option = {
                Position: null,
                Overflow: this.Element.Style0('overflow'),
                Height: this.Element.Height0(),
                Width: this.Element.Width0()
            };
            this.Element.Style('overflow', 'hidden');//.Css('margin,padding','0px')
            this.StartTime = new Date().getTime();
            this.Interval = setInterval(this.NextHideFunction, 50);
        }
        private NextHide() {
            var Time = new Date().getTime() - this.StartTime;
            if (Time < this.Time) this.Element.Style(this.Type, ((this.Type == 'height' ? this.Option.Height : this.Option.Width) * (this.Time - Time) / this.Time) + 'px');
            else this.End();
        }
        private HideEnd() {
            this.Element.Display(0).Style('overflow', this.Option.Overflow).Style(this.Type, null);
        }
        private static TimerShows: { [key: string]: TimerShow } = {};
        static GetTimerShow = function (Id: string) {
            var Value = TimerShow.TimerShows[Id];
            if (Value) return Value;
            var Element = HtmlElement.$IdElement(Id);
            if (Element) {
                var Parameter = HtmlElement.$Attribute(Element, 'timeshow');
                (Parameter = Parameter ? eval('(' + Parameter + ')') : {}).Id = Id;
                TimerShow.TimerShows[Id] = Value = new TimerShow(Parameter);
                return Value;
            }
        }
    }
}