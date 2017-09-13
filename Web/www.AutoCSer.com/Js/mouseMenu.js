/// <reference path = "./menu.ts" />
/*Include:Js\menu*/
'use strict';
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
//鼠标覆盖菜单	<div id="YYY"></div><div mousemenu="{MenuId:'YYY',IsDisplay:1,Type:'Bottom',Top:5,Left:-5}" id="XXX"></div>
var AutoCSer;
(function (AutoCSer) {
    var MouseMenu = (function (_super) {
        __extends(MouseMenu, _super);
        function MouseMenu(Parameter) {
            _super.call(this, Parameter);
            AutoCSer.Pub.GetParameter(this, MouseMenu.DefaultParameter, Parameter);
            AutoCSer.Pub.GetEvents(this, MouseMenu.DefaultEvents, Parameter);
            this.HideFunction = AutoCSer.Pub.ThisFunction(this, this.Hide);
            this.MouseOutFunction = AutoCSer.Pub.ThisFunction(this, this.MouseOut);
            this.Start(this.Event || AutoCSer.DeclareEvent.Default);
        }
        MouseMenu.prototype.Start = function (Event) {
            if (!Event.IsGetOnly) {
                this.OnStart.Function(this);
                var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
                if (Element != this.Element) {
                    this.Element = Element;
                    AutoCSer.HtmlElement.$AddEvent(Element, ['mouseout'], this.MouseOutFunction);
                    if (this.IsMouseMove)
                        AutoCSer.HtmlElement.$AddEvent(Element, ['mousemove'], AutoCSer.Pub.ThisEvent(this, this.ReShow));
                    this.CheckMenuParameter(true);
                }
                this.ClearInterval();
                this.IsOver = true;
                this.Show(Event);
            }
        };
        MouseMenu.prototype.CheckMenuParameter = function (IsStart) {
            if (IsStart === void 0) { IsStart = false; }
            var Element = AutoCSer.HtmlElement.$Id(this.MenuId);
            if (Element.Element0()) {
                var Parameter = AutoCSer.HtmlElement.$Attribute(Element.Element0(), 'mousemenu');
                if (Parameter != null) {
                    var Id = eval('(' + Parameter + ')').Id;
                    if (Id != this.Id) {
                        Parameter = null;
                        var Menu = AutoCSer.Declare.Getters['MouseMenu'](Id, true);
                        if (Menu) {
                            Menu.Remove();
                            Element.Set('mousemenu', '{Id:"' + this.Id + '"}');
                            return;
                        }
                    }
                }
                if (IsStart) {
                    if (Parameter == null)
                        Element.Set('mousemenu', '{Id:"' + this.Id + '"}');
                    Element.AddEvent('mouseout', this.MouseOutFunction);
                }
            }
        };
        MouseMenu.prototype.Show = function (Event) {
            this.ShowMenu();
            if (this.IsMove)
                this.ReShow(Event, AutoCSer.HtmlElement.$Id(this.Id));
            this.OnShowed.Function();
        };
        MouseMenu.prototype.MouseOut = function () {
            this.IsOver = false;
            this.ClearInterval();
            this.HideInterval = setTimeout(this.HideFunction, this.Timeout);
        };
        MouseMenu.prototype.ClearInterval = function () {
            if (this.HideInterval) {
                clearTimeout(this.HideInterval);
                this.HideInterval = 0;
            }
        };
        MouseMenu.prototype.Hide = function () {
            this.ClearInterval();
            this.HideMenu();
        };
        MouseMenu.prototype.Remove = function () {
            this.ClearInterval();
            this.ShowView = null;
            var Element = AutoCSer.HtmlElement.$Id(this.Id);
            Element.DeleteEvent('mouseout', this.MouseOutFunction);
            AutoCSer.HtmlElement.$Id(this.MenuId).Set('mousemenu', '').DeleteEvent('mouseout', this.MouseOutFunction);
            this.Element = null;
        };
        MouseMenu.prototype.ReShow = function (Event, Element) {
            this.OnMove.Function(Event, this);
            this['To' + (this.Type || 'Mouse')](Event, Element);
        };
        MouseMenu.prototype.ToMouse = function (Event, Element) {
            this.CheckScroll(Event.clientX, Event.clientY);
        };
        MouseMenu.DefaultParameter = { Timeout: 100, IsMouseMove: 0 };
        MouseMenu.DefaultEvents = { OnMove: null };
        return MouseMenu;
    }(AutoCSer.Menu));
    AutoCSer.MouseMenu = MouseMenu;
    var MouseMenuEnum = (function () {
        function MouseMenuEnum(Value, Show) {
            this.Value = Value;
            this.Show = Show || Value;
        }
        MouseMenuEnum.prototype.ToJson = function (IsIgnore, IsNameQuery, IsSortName, Parents) {
            return AutoCSer.Pub.ToJson(this.Value, IsIgnore, IsNameQuery, IsSortName, Parents);
        };
        MouseMenuEnum.prototype.toString = function () {
            return this.Value;
        };
        return MouseMenuEnum;
    }());
    AutoCSer.MouseMenuEnum = MouseMenuEnum;
    new AutoCSer.Declare(MouseMenu, 'MouseMenu', 'mouseover', 'ParameterId');
})(AutoCSer || (AutoCSer = {}));
