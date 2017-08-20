/// <reference path = "./base.page.ts" />
'use strict';
var AutoCSer;
(function (AutoCSer) {
    var Menu = (function () {
        function Menu(Parameter) {
            AutoCSer.Pub.GetParameter(this, Menu.NenuParameter, Parameter);
            AutoCSer.Pub.GetEvents(this, Menu.MenuEvents, Parameter);
        }
        Menu.prototype.ShowMenu = function () {
            var Menu = AutoCSer.HtmlElement.$Id(this.MenuId), Skin;
            if (this.IsMove)
                Menu.Style('position', 'absolute').Style('zIndex', AutoCSer.HtmlElement.ZIndex + this.ZIndex);
            if (this.SkinView)
                Skin = AutoCSer.Skin.Skins[this.MenuId];
            var IsShow = this.ShowViewFunctionName == null || this.SkinView[this.ShowViewFunctionName]();
            if (Skin && this.ShowView != this.SkinView) {
                this.ShowView = this.SkinView;
                if (IsShow)
                    Skin.Show(this.SkinView);
            }
            if (this.IsShow && IsShow)
                Menu.Display(1);
            var Element = AutoCSer.HtmlElement.$(this.Element);
            if (this.OutClassName)
                Element.RemoveClass(this.OutClassName);
            if (this.OverClassName)
                Element.AddClass(this.OverClassName);
        };
        Menu.prototype.HideMenu = function () {
            var Element = AutoCSer.HtmlElement.$(this.Element);
            if (this.OverClassName)
                Element.RemoveClass(this.OverClassName);
            if (this.OutClassName)
                Element.AddClass(this.OutClassName);
            if (this.IsShow)
                AutoCSer.HtmlElement.$Id(this.MenuId).Display(0);
            this.ShowView = null;
        };
        Menu.prototype.CheckScroll = function (Left, Top, XY) {
            if (XY === void 0) { XY = null; }
            if (this.Left)
                Left += this.Left;
            if (this.Top)
                Top += this.Top;
            var Menu = AutoCSer.HtmlElement.$Id(this.MenuId);
            if (this.IsCheckScroll) {
                var ScrollLeft = AutoCSer.HtmlElement.$GetScrollLeft(), ScrollTop = AutoCSer.HtmlElement.$GetScrollTop();
                if (XY) {
                    var Width = AutoCSer.HtmlElement.$Width(Menu.Element0()), Height = AutoCSer.HtmlElement.$Height(Menu.Element0());
                    if (Width) {
                        var ClientWidth = AutoCSer.HtmlElement.$Width() + ScrollLeft;
                        if (ClientWidth > XY.Left && Left > (ClientWidth -= Width))
                            Left = ClientWidth;
                    }
                    if (Height) {
                        var ClientHeight = AutoCSer.HtmlElement.$Height() + ScrollTop;
                        if (ClientHeight > XY.Top && Top > (ClientHeight -= Height))
                            Top = ClientHeight;
                    }
                }
                if (Left < ScrollLeft)
                    Left = ScrollLeft;
                if (Top < ScrollTop)
                    Top = ScrollTop;
            }
            Menu.ToXY(Left, Top);
        };
        Menu.prototype.ToTopLeft = function (Event, Element) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left, XY.Top - AutoCSer.HtmlElement.$Height(AutoCSer.HtmlElement.$IdElement(this.MenuId)), XY);
        };
        Menu.prototype.ToTopRight = function (Event, Element) {
            var Menu = AutoCSer.HtmlElement.$Id(this.MenuId), XY = Element.XY0();
            this.CheckScroll(XY.Left + AutoCSer.HtmlElement.$Width(Element.Element0()) - AutoCSer.HtmlElement.$Width(Menu.Element0()), XY.Top - AutoCSer.HtmlElement.$Height(Menu.Element0()), XY);
        };
        Menu.prototype.ToTop = function (Event, Element) {
            var Menu = AutoCSer.HtmlElement.$Id(this.MenuId), XY = Element.XY0();
            this.CheckScroll(XY.Left + (AutoCSer.HtmlElement.$Width(Element.Element0()) - AutoCSer.HtmlElement.$Width(Menu.Element0())) / 2, XY.Top - AutoCSer.HtmlElement.$Height(Menu.Element0()), XY);
        };
        Menu.prototype.ToBottomLeft = function (Event, Element) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left, XY.Top + AutoCSer.HtmlElement.$Height(Element.Element0()), XY);
        };
        Menu.prototype.ToBottomRight = function (Event, Element) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + AutoCSer.HtmlElement.$Width(Element.Element0()) - AutoCSer.HtmlElement.$Width(AutoCSer.HtmlElement.$IdElement(this.MenuId)), XY.Top + AutoCSer.HtmlElement.$Height(Element.Element0()), XY);
        };
        Menu.prototype.ToBottom = function (Event, Element) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + (AutoCSer.HtmlElement.$Width(Element.Element0()) - AutoCSer.HtmlElement.$Width(AutoCSer.HtmlElement.$IdElement(this.MenuId))) / 2, XY.Top + AutoCSer.HtmlElement.$Height(Element.Element0()), XY);
        };
        Menu.prototype.ToLeftTop = function (Event, Element) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left - AutoCSer.HtmlElement.$Width(AutoCSer.HtmlElement.$IdElement(this.MenuId)), XY.Top, XY);
        };
        Menu.prototype.ToLeftBottom = function (Event, Element) {
            var Menu = AutoCSer.HtmlElement.$Id(this.MenuId), XY = Element.XY0();
            this.CheckScroll(XY.Left - AutoCSer.HtmlElement.$Width(Menu.Element0()), XY.Top + AutoCSer.HtmlElement.$Height(Element.Element0()) - AutoCSer.HtmlElement.$Height(Menu.Element0()), XY);
        };
        Menu.prototype.ToLeft = function (Event, Element) {
            var Menu = AutoCSer.HtmlElement.$Id(this.MenuId), XY = Element.XY0();
            this.CheckScroll(XY.Left - AutoCSer.HtmlElement.$Width(Menu.Element0()), XY.Top + (AutoCSer.HtmlElement.$Height(Element.Element0()) - AutoCSer.HtmlElement.$Height(Menu.Element0())) / 2, XY);
        };
        Menu.prototype.ToRightTop = function (Event, Element) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + AutoCSer.HtmlElement.$Width(Element.Element0()), XY.Top, XY);
        };
        Menu.prototype.ToRightBottom = function (Event, Element) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + AutoCSer.HtmlElement.$Width(Element.Element0()), XY.Top + AutoCSer.HtmlElement.$Height(Element.Element0()) - AutoCSer.HtmlElement.$Height(AutoCSer.HtmlElement.$IdElement(this.MenuId)), XY);
        };
        Menu.prototype.ToRight = function (Event, Element) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + AutoCSer.HtmlElement.$Width(Element.Element0()), XY.Top + (AutoCSer.HtmlElement.$Height(Element.Element0()) - AutoCSer.HtmlElement.$Height(AutoCSer.HtmlElement.$IdElement(this.MenuId))) / 2, XY);
        };
        Menu.NenuParameter = { Id: null, Event: null, MenuId: null, SkinView: null, ShowViewFunctionName: null, Type: null, Top: null, Left: null, OverClassName: null, OutClassName: null, IsCheckScroll: 1, IsShow: 1, IsMove: 1, ZIndex: 1 };
        Menu.MenuEvents = { OnStart: null, OnShowed: null };
        return Menu;
    }());
    AutoCSer.Menu = Menu;
})(AutoCSer || (AutoCSer = {}));
