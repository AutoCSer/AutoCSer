/// <reference path = "./menu.ts" />
/*Include:Js\menu*/
'use strict';
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
//鼠标点击菜单	<div id="YYY"></div><div clickmenu="{MenuId:'YYY',IsDisplay:1,Type:'Bottom',Top:5,Left:-5}" onclick="void(0);" id="XXX"></div>
var AutoCSer;
(function (AutoCSer) {
    var ManyClickMenu = (function (_super) {
        __extends(ManyClickMenu, _super);
        function ManyClickMenu(Parameter) {
            _super.call(this, Parameter);
            AutoCSer.Pub.GetEvents(this, ManyClickMenu.DefaultEvents, Parameter);
            AutoCSer.HtmlElement.$(document.body).AddEvent('click', AutoCSer.Pub.ThisEvent(this, this.Check));
            this.Reset(Parameter, Parameter.DeclareElement);
        }
        ManyClickMenu.prototype.Reset = function (Parameter, Element) {
            if (Element != this.Element) {
                if (this.Element)
                    this.HideMenu();
                this.Element = Element;
                this.IsOver = false;
                this.OnReset.Function(this, Parameter, Element);
            }
            this.Show();
        };
        ManyClickMenu.prototype.Check = function (Event) {
            if (!Event.$Name("manyclickmenu"))
                this.Hide();
        };
        ManyClickMenu.prototype.Show = function () {
            if (this.IsOver)
                this.Hide();
            else {
                this.OnStart.Function(this);
                this.ShowMenu();
                if (this.IsMove)
                    this['To' + (this.Type || 'Bottom')](Event, AutoCSer.HtmlElement.$(this.Element));
                this.OnShowed.Function();
                this.IsOver = true;
            }
        };
        ManyClickMenu.prototype.Hide = function () {
            this.HideMenu();
            this.IsOver = false;
        };
        ManyClickMenu.DefaultEvents = { OnReset: null };
        return ManyClickMenu;
    }(AutoCSer.Menu));
    AutoCSer.ManyClickMenu = ManyClickMenu;
    new AutoCSer.Declare(ManyClickMenu, 'ManyClickMenu', 'click', 'ParameterMany');
})(AutoCSer || (AutoCSer = {}));
