/// <reference path = "./base.page.ts" />
'use strict';
module AutoCSer {
    export class OverIframe {
        private static DefaultParameter = { WaitingImage: '//__STATICDOMAIN__/upFile/waiting.gif', IframePath: null, IframeCode: null, Width: null, Height:null };
        private WaitingImage: string;
        private IframePath: string;
        private IframeCode: string;
        private Width: number;
        private Height: number;

        private MoveFunction: Function;
        private FocusFunction: Function;
        OnLoad: Events;
        OnHide: Events;
        private Id: string;
        private TableId: string;
        private IframeId: string;
        private InputId0: string;
        private InputId1: string;
        private Interval: number;
        constructor(Parameter = null) {
            Pub.GetParameter(this, OverIframe.DefaultParameter, Parameter);
            var Identity = ++Pub.Identity;
            this.Id = '_' + Identity + '_OVER_';
            this.TableId = '_' + Identity + '_OVERTABLE_';
            this.IframeId = '_' + Identity + '_OVERIFRAME_';
            this.InputId0 = '_' + Identity + '_OVERINPUT0_';
            this.InputId1 = '_' + Identity + '_OVERINPUT1_';
            HtmlElement.$Create('div').Style('position', 'absolute').Styles('width,height', '100%').Display(0).Set('id', this.Id)
                .Html("<table id='" + this.TableId + "' cellspacing='0' cellpadding='0' border='0' width='100%' height='100%' style='background-color:#444444;'><tr height='100%'><td width='100%' height='100%' align='center'><iframe id='" + this.IframeId + "' name='" + this.IframeId + "' frameborder='0' marginwidth='0' marginheight='0' vspace='0' hspace='0' allowtransparency='true' scrolling='no'></iframe></td></tr></table>")
                .To();
            HtmlElement.$Id(this.TableId).Opacity(90);
            this.MoveFunction = Pub.ThisFunction(this, this.Move);
            this.FocusFunction = Pub.ThisFunction(this, this.Focus);
            this.OnLoad = new Events();
            frames[this.IframeId].onload = Pub.ThisFunction(this, this.LoadIframe);
        }
        private Move() {
            var Iframe = frames[this.IframeId] as Window, IframeElement = HtmlElement.$Id(this.IframeId);
            try {
                if (this.Width == null) IframeElement.Set('width', Iframe.document.body.scrollWidth || Iframe.document.documentElement.scrollWidth);
                if (this.Height == null) IframeElement.Set('height', Iframe.document.body.scrollHeight || Iframe.document.documentElement.scrollHeight);
            }
            catch (e) { }
            HtmlElement.$Id(this.Id).Style('zIndex', 0).Style('top', HtmlElement.$GetScrollTop() + 'px').Style('left', HtmlElement.$GetScrollLeft() + 'px').TopIndex();
        }
        Hide(IsOnHide = true) {
            if (this.Interval) clearTimeout(this.Interval);
            frames[this.IframeId].document.onblur = null;
            HtmlElement.$Id(this.Id).Styles('top,left', '0px').Display(0);
            if (IsOnHide && this.OnHide) this.OnHide.Function();
        }
        SetHtml(Html = '') {
            try {
                frames[this.IframeId].document.body.innerHTML = Html;
            }
            catch (e) { }
        }
        Show(ReLoad = true) {
            this.Hide(false);
            var Iframe = frames[this.IframeId] as Window, IframeElement = HtmlElement.$Id(this.IframeId);
            HtmlElement.$Id(this.Id).Display(1);
            this.Move();
            this.Interval = setInterval(this.MoveFunction, 500);
            if (ReLoad) {
                if (this.IframeCode) {
                    try {
                        Iframe.document.body.innerHTML = "<div align='center'>" + (this.WaitingImage ? "<img src='" + this.WaitingImage+"'><br />" : '') + "" + this.IframeCode + '</div>';
                    }
                    catch (e) {
                        if (this.IframeCode.substring(0, 6).toLowerCase() != '<html>') this.IframeCode = '<html><body>' + this.IframeCode + '</body></html>';
                        Iframe.document.open();
                        Iframe.document.write(this.IframeCode);
                        Iframe.document.close();
                    }
                    this.LoadIframe();
                }
                else if (this.IframePath) IframeElement.Set('src', this.IframePath);
            }
        }
        ShowPath(Path: string) {
            this.IframePath = Path;
            this.Show();
        }
        ShowHtml(Html: string) {
            this.IframeCode = Html;
            this.Show();
        }
        private LoadIframe () {
            if (HtmlElement.$Id(this.Id).Style0('display') == '') {
                var Iframe = frames[this.IframeId] as Window;
                HtmlElement.$Id(this.IframeId).Set('width', this.Width ? this.Width : (Iframe.document.body.scrollWidth || Iframe.document.documentElement.scrollWidth))
                    .Set('height', this.Height ? this.Height : (Iframe.document.body.scrollHeight || Iframe.document.documentElement.scrollHeight));
                this.OnLoad.Function();
                this.AppendInput(this.InputId0);
                (this.AppendInput(this.InputId1)).onfocus = (Iframe.document).onblur = this.FocusFunction as (Event: FocusEvent) => any;
            }
        }
        private AppendInput(InputId: string): HTMLInputElement {
            var Iframe = frames[this.IframeId] as Window, Element = Iframe.document.getElementById(InputId) as HTMLInputElement;
            if (Element == null) {
                Element = Iframe.document.createElement('input');
                Element.id = InputId;
                Element.style.position = 'absolute';
                Element.style.width = Element.style.height = '1px';
                Element.readOnly = true;
                Iframe.document.body.appendChild(Element);
            }
            return Element;
        }
        Focus() {
            (frames[this.IframeId] as Window).document.getElementById(this.InputId0).focus();
        }
        static Default: OverIframe;
        static CreateDefault() {
            OverIframe.Default = new OverIframe();
        }
    }
    Pub.OnLoad(OverIframe.CreateDefault, OverIframe, true);
}