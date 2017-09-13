/// <reference path = "./menu.ts" />
/*Include:Js\menu*/
'use strict';
//鼠标覆盖菜单	<div id="YYY"></div><div mousemenu="{MenuId:'YYY',IsDisplay:1,Type:'Bottom',Top:5,Left:-5}" id="XXX"></div>
module AutoCSer {
    export class MouseMenu extends Menu implements IDeclare {
        private static DefaultParameter = { Timeout: 100, IsMouseMove: 0 };
        private static DefaultEvents = { OnMove: null };
        private Timeout: number;
        private IsMouseMove: boolean;

        private OnMove: Events;

        private HideFunction: Function;
        private MouseOutFunction: Function;
        private HideInterval: number;
        constructor(Parameter) {
            super(Parameter);
            Pub.GetParameter(this, MouseMenu.DefaultParameter, Parameter);
            Pub.GetEvents(this, MouseMenu.DefaultEvents, Parameter);
            this.HideFunction = Pub.ThisFunction(this, this.Hide);
            this.MouseOutFunction = Pub.ThisFunction(this, this.MouseOut);
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                this.OnStart.Function(this);
                var Element = HtmlElement.$IdElement(this.Id);
                if (Element != this.Element) {
                    this.Element = Element;
                    HtmlElement.$AddEvent(Element, ['mouseout'], this.MouseOutFunction);
                    if (this.IsMouseMove) HtmlElement.$AddEvent(Element, ['mousemove'], Pub.ThisEvent(this, this.ReShow));
                    this.CheckMenuParameter(true);
                }
                this.ClearInterval();
                this.IsOver = true;
                this.Show(Event);
            }
        }
        CheckMenuParameter(IsStart = false) {
            var Element = HtmlElement.$Id(this.MenuId);
            if (Element.Element0()) {
                var Parameter = HtmlElement.$Attribute(Element.Element0(), 'mousemenu');
                if (Parameter != null) {
                    var Id = eval('(' + Parameter + ')').Id;
                    if (Id != this.Id) {
                        Parameter = null;
                        var Menu = Declare.Getters['MouseMenu'](Id, true) as MouseMenu;
                        if (Menu) {
                            Menu.Remove();
                            Element.Set('mousemenu', '{Id:"' + this.Id + '"}');
                            return;
                        }
                    }
                }
                if (IsStart) {
                    if (Parameter == null) Element.Set('mousemenu', '{Id:"' + this.Id + '"}');
                    Element.AddEvent('mouseout', this.MouseOutFunction);
                }
            }
        }
        private Show(Event: BrowserEvent) {
            this.ShowMenu();
            if (this.IsMove) this.ReShow(Event, HtmlElement.$Id(this.Id));
            this.OnShowed.Function();
        }
        private MouseOut() {
            this.IsOver = false;
            this.ClearInterval();
            this.HideInterval = setTimeout(this.HideFunction, this.Timeout);
        }
        private ClearInterval() {
            if (this.HideInterval) {
                clearTimeout(this.HideInterval);
                this.HideInterval = 0;
            }
        }
        Hide() {
            this.ClearInterval();
            this.HideMenu();
        }
        private Remove() {
            this.ClearInterval();
            this.ShowView = null;
            var Element = HtmlElement.$Id(this.Id);
            Element.DeleteEvent('mouseout', this.MouseOutFunction);
            HtmlElement.$Id(this.MenuId).Set('mousemenu', '').DeleteEvent('mouseout', this.MouseOutFunction);
            this.Element = null;
        }
        private ReShow(Event: BrowserEvent, Element: HtmlElement) {
            this.OnMove.Function(Event, this);
            this['To' + (this.Type || 'Mouse')](Event, Element);
        }
        private ToMouse(Event: BrowserEvent, Element: HtmlElement) {
            this.CheckScroll(Event.clientX, Event.clientY);
        }
    }
    export class MouseMenuEnum {
        private Value: string;
        private Show: string;
        constructor(Value: string, Show: string) {
            this.Value = Value;
            this.Show = Show || Value;
        }
        ToJson(IsIgnore: boolean, IsNameQuery: boolean, IsSortName : boolean, Parents: any[]) {
            return AutoCSer.Pub.ToJson(this.Value, IsIgnore, IsNameQuery, IsSortName, Parents);
        }
        toString() {
            return this.Value;
        }
    }
    new AutoCSer.Declare(MouseMenu, 'MouseMenu', 'mouseover', 'ParameterId');
}