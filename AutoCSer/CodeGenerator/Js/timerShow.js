/// <reference path = "./base.page.ts" />
'use strict';
//HTML高度更新模拟动画
//AutoCSer.Declare.Getters.TimerShow(Id).Show();
//AutoCSer.Declare.Getters.TimerShow(Id).Hide();
var AutoCSer;
(function (AutoCSer) {
    var TimerShow = (function () {
        function TimerShow(Parameter) {
            AutoCSer.Pub.GetParameter(this, TimerShow.DefaultParameter, Parameter);
            this.NextShowFunction = AutoCSer.Pub.ThisFunction(this, this.NextShow);
            this.NextHideFunction = AutoCSer.Pub.ThisFunction(this, this.NextHide);
        }
        TimerShow.prototype.End = function () {
            if (this.Interval) {
                clearTimeout(this.Interval);
                this[this.IsShow ? 'ShowEnd' : 'HideEnd']();
                if (this.OnShowed)
                    this.OnShowed();
                this.Interval = null;
            }
        };
        TimerShow.prototype.Show = function (OnShowed) {
            this.End();
            this.IsShow = true;
            this.OnShowed = OnShowed;
            this.Element = AutoCSer.HtmlElement.$Id(this.Id);
            this.Option = {
                Position: this.Element.Style0('position'),
                Overflow: this.Element.Style0('overflow'),
                Height: this.Element.Opacity(0).Style('position', 'absolute').Display(1).Height0(),
                Width: this.Element.Width0()
            };
            this.Element.Style(this.Type, '0px').Style('overflow', 'hidden').Display('block').Opacity(100).Style('position', this.Option.Position); //.Css('margin,padding','0px')
            this.StartTime = new Date().getTime();
            this.Interval = setInterval(this.NextShowFunction, 50);
        };
        TimerShow.prototype.NextShow = function () {
            var Time = new Date().getTime() - this.StartTime;
            if (Time < this.Time)
                this.Element.Style(this.Type, (this.Type == 'height' ? this.Option.Height : this.Option.Width) * Time / this.Time + 'px');
            else
                this.End();
        };
        TimerShow.prototype.ShowEnd = function () {
            this.Element.Display(1).Style('overflow', this.Option.Overflow).Style(this.Type, null); //.Css('margin,padding','')
        };
        TimerShow.prototype.Hide = function (OnShowed) {
            this.End();
            this.IsShow = false;
            this.OnShowed = OnShowed;
            this.Element = AutoCSer.HtmlElement.$Id(this.Id).Display('block');
            this.Option = {
                Position: null,
                Overflow: this.Element.Style0('overflow'),
                Height: this.Element.Height0(),
                Width: this.Element.Width0()
            };
            this.Element.Style('overflow', 'hidden'); //.Css('margin,padding','0px')
            this.StartTime = new Date().getTime();
            this.Interval = setInterval(this.NextHideFunction, 50);
        };
        TimerShow.prototype.NextHide = function () {
            var Time = new Date().getTime() - this.StartTime;
            if (Time < this.Time)
                this.Element.Style(this.Type, ((this.Type == 'height' ? this.Option.Height : this.Option.Width) * (this.Time - Time) / this.Time) + 'px');
            else
                this.End();
        };
        TimerShow.prototype.HideEnd = function () {
            this.Element.Display(0).Style('overflow', this.Option.Overflow).Style(this.Type, null);
        };
        TimerShow.DefaultParameter = { Id: null, Time: 250, Type: 'height' };
        TimerShow.TimerShows = {};
        TimerShow.GetTimerShow = function (Id) {
            var Value = TimerShow.TimerShows[Id];
            if (Value)
                return Value;
            var Element = AutoCSer.HtmlElement.$IdElement(Id);
            if (Element) {
                var Parameter = AutoCSer.HtmlElement.$Attribute(Element, 'timeshow');
                (Parameter = Parameter ? eval('(' + Parameter + ')') : {}).Id = Id;
                TimerShow.TimerShows[Id] = Value = new TimerShow(Parameter);
                return Value;
            }
        };
        return TimerShow;
    }());
    AutoCSer.TimerShow = TimerShow;
})(AutoCSer || (AutoCSer = {}));
