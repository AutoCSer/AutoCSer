/// <reference path = "./base.page.ts" />
'use strict';
module AutoCSer {
    export class Color512_64 {
        private static DefaultParameter = { Id: null, CurrentColor: null, CurrentColorSpan: null };
        private static DefaultEvents = { OnClick: null, OnOver: null, OnMove: null };
        private Id: string;
        private CurrentColor: string;
        private CurrentColorSpan: string;
        private OnClick: Events;
        private OnOver: Events;
        private OnMove: Events;

        private Identity: number;
        private Iframe: Window;
        private Image: string;
        constructor(Parameter) {
            Pub.GetParameter(this, Color512_64.DefaultParameter, Parameter);
            Pub.GetEvents(this, Color512_64.DefaultEvents, Parameter);
            this.Identity = ++Pub.Identity;
            this.Image = Loader.JsDomain + 'Js/color512_64.bmp';
        }
        Start() {
            var Iframe = '_' + this.Identity + '_COLOR_';
            HtmlElement.$Id(this.Id).Html("<iframe Id='" + Iframe + "' name='" + Iframe + "' width='512px' height='64px' frameborder='0' marginwidth='0' marginheight='0' vspace='0' hspace='0' allowtransparency='true' scrolling='no'></iframe>");
            this.Iframe = frames[Iframe];
            this.Iframe.document.open();
            this.Iframe.document.write("<html><body><img id='Color512_64' src='" + this.Image + "' Width='512' Height='64' border='0' alt='点击改变最近选择的色彩参数' /></body></html>");
            this.Iframe.document.close();
            HtmlElement.$(this.Iframe.document.getElementById('Color512_64')).AddEvent('mousemove', Pub.ThisEvent(this, this.MoveColor, null, this.Iframe))
                .AddEvent('click', Pub.ThisEvent(this, this.ClickColor, null, this.Iframe))
                .AddEvent('mouseover', Pub.ThisFunction(this, this.Over, [true]))
                .AddEvent('mouseout', Pub.ThisFunction(this, this.Over, [false]));
            HtmlElement.$Id(this.CurrentColor).Set('maxLength', 6).Value('000000').Set('readOnly', true).AddEvent('focus', Color512_64.Select);
            if (this.CurrentColorSpan) {
                HtmlElement.$Id(this.CurrentColorSpan).Style('backgroundColor', '#000000').Set('title', '点击改变最近选择的色彩参数').Cursor('pointer')
                    .AddEvent('mouseover', Pub.ThisFunction(this, this.Over, [true]))
                    .AddEvent('mouseout', Pub.ThisFunction(this, this.Over, [false]))
                    .AddEvent('click', Pub.ThisFunction(this, this.Click));
            }
        }
        GetColor(Event: BrowserEvent): number {
            var Width = Event.clientX, Height = Event.clientY, Color = (((Width >> 5) << 4) + ((Height >> 5) << 3) + 4) << 16;
            Color += (((Height < 32 ? Height : 63 - Height) << 3) + 4) << 8;
            Width &= 63;
            return Color + ((Width < 32 ? Width : 63 - Width) << 3) + 4;
        }
        static Select() { this['select'](); }
        MoveColor(Event: BrowserEvent) {
            this.Move(this.GetColor(Event));
        }
        ClickColor(Event: BrowserEvent) {
            this.OnClick.Function(this.Move(this.GetColor(Event)));
        }
        Over(IsOver: boolean) {
            this.OnOver.Function(IsOver);
        }
        Move(Color: number): string {
            var Hex = Color.toString(16).PadLeft(6, '0');
            HtmlElement.$SetValueById(this.CurrentColor, Hex);
            HtmlElement.$SetStyle(HtmlElement.$IdElement(this.CurrentColorSpan), 'backgroundColor', '#' + Hex);
            this.OnMove.Function(Hex);
            return Hex;
        }
        Click() {
            this.OnClick.Function(this.Move(parseInt(HtmlElement.$GetValueById(this.CurrentColor), 16)));
        }
        Show(IsShow: boolean) {
            HtmlElement.$([HtmlElement.$IdElement(this.Id), HtmlElement.$IdElement(this.CurrentColor), HtmlElement.$IdElement(this.CurrentColorSpan)]).Display(IsShow);
        }
    }
}