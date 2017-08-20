/// <reference path = "./menu.ts" />
/*Include:Js\menu*/
'use strict';
//鼠标点击菜单	<div id="YYY"></div><div clickmenu="{MenuId:'YYY',IsDisplay:1,Type:'Bottom',Top:5,Left:-5}" onclick="void(0);" id="XXX"></div>
module AutoCSer {
    export class ManyClickMenu extends Menu implements IDeclareMany {
        private static DefaultEvents = { OnReset: null };
        private OnReset: Events;

        constructor(Parameter: AutoCSer.IDeclareParameter) {
            super(Parameter);
            Pub.GetEvents(this, ManyClickMenu.DefaultEvents, Parameter);
            HtmlElement.$(document.body).AddEvent('click', Pub.ThisEvent(this, this.Check));
            this.Reset(Parameter, Parameter.DeclareElement);
        }
        Reset(Parameter: AutoCSer.IDeclareParameter, Element: HTMLElement) {
            if (Element != this.Element) {
                if (this.Element) this.HideMenu();
                this.Element = Element;
                this.IsOver = false;
                this.OnReset.Function(this, Parameter, Element);
            }
            this.Show();
        }
        private Check(Event: DeclareEvent) {
            if (!Event.$Name("manyclickmenu")) this.Hide();
        }
        private Show() {
            if (this.IsOver) this.Hide();
            else {
                this.OnStart.Function(this);
                this.ShowMenu();
                if (this.IsMove) this['To' + (this.Type || 'Bottom')](Event, HtmlElement.$(this.Element));
                this.OnShowed.Function();
                this.IsOver = true;
            }
        }
        Hide() {
            this.HideMenu();
            this.IsOver = false;
        }
    }
    new AutoCSer.Declare(ManyClickMenu, 'ManyClickMenu', 'click', 'ParameterMany');
}