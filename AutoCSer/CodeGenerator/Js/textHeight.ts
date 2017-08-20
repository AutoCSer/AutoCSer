/// <reference path = "./base.page.ts" />
'use strict';
//文本收起	<div id="XXX" textheight="{Height:80,HideSize:280,TextId:'YYY',ShowButton:'AAA',HideButton:'BBB'}"><span id="YYY">Text<span><span id="AAA" style="display:none">[收起]</span><span id="BBB" style="display:none">[全文]</span></div>
module AutoCSer {
    export class TextHegiht {
        private static DefaultParameter = { Id: null, Suffix: null, Height: null, HideSize: null, TextId: null, ShowButton: null, HideButton: null, ScrollId: null, IsHide: 1 };
        private Id: string;
        private Suffix: string;
        private Height: number;
        private HideSize: number;
        private TextId: string;
        private ShowButton: string;
        private HideButton: string;
        private ScrollId: string;
        private IsHide: boolean;

        private Element: HTMLElement;
        private HideId: string;
        private IsCheck: boolean;
        constructor(Parameter) {
            Pub.GetParameter(this, TextHegiht.DefaultParameter, Parameter);
            if (this.Suffix) {
                this.ScrollId += this.Suffix;
                if (this.ShowButton) this.ShowButton += this.Suffix;
                else this.ShowButton = this.TextId + 'Show' + this.Suffix;
                if (this.HideButton) this.HideButton += this.Suffix;
                else this.HideButton = this.TextId + 'Hide' + this.Suffix;
                this.TextId += this.Suffix;
            }
            this.HideId = this.TextId + 'Hide';
            this.Start();
        }
        private Start() {
            var Element = HtmlElement.$IdElement(this.Id);
            if (Element != this.Element) {
                this.Element = Element;
                HtmlElement.$Create('span').Set('id', this.HideId).InsertBefore(HtmlElement.$IdElement(this.TextId)).Display(0);
                HtmlElement.$Id(this.ShowButton).AddEvent('click', Pub.ThisFunction(this, this.Show));
                HtmlElement.$Id(this.HideButton).AddEvent('click', Pub.ThisFunction(this, this.Hide));
                this.IsCheck = false;
            }
            if (!this.IsCheck) {
                this.IsCheck = true;
                if (HtmlElement.$Id(this.Id).Height0() > this.Height) {
                    HtmlElement.$Id(this.HideId).Html(HtmlElement.$Id(this.TextId).Text0().Left(this.HideSize).ToHTML());
                    this[this.IsHide ? 'Hide' : 'Show'](1);
                }
            }
        }
        private Show(NoScroll: boolean) {
            if (!TextHegiht.IsSetPageView) {
                this.Hide(NoScroll, false);
                TextHegiht.IsSetPageView = true;
                Skin.Body.OnSet.Function();
                TextHegiht.IsSetPageView = false;
            }
        }
        private Hide(NoScroll: boolean, IsHide: boolean = true) {
            this.IsHide = IsHide;
            HtmlElement.$Id(this.TextId).Display(!IsHide);
            HtmlElement.$Id(this.HideButton).Display(!IsHide);
            HtmlElement.$Id(this.HideId).Display(IsHide);
            HtmlElement.$Id(this.ShowButton).Display(IsHide);
            if (!NoScroll) HtmlElement.$SetScrollTop(HtmlElement.$Id(this.ScrollId || this.HideId).XY0().Top);
        }
        private static IsSetPageView: boolean;
        private static TextHegihts: { [key: string]: TextHegiht };
        static CheckTextHegiht() {
            if (!this.TextHegihts) {
                this.TextHegihts = {};
                Skin.Body.OnSet.Add(Pub.ThisFunction(this, this.CheckTextHegiht));
            }
            if (!this.IsSetPageView) {
                for (var Elements = HtmlElement.$('@textheight').GetElements(), Index = Elements.length; --Index >= 0;) {
                    var Element = Elements[Index], TestHeight = this.TextHegihts[Element.id];
                    if (TestHeight) TestHeight.Start();
                    else {
                        var Parameter = eval('(' + HtmlElement.$Attribute(Element, 'textheight') + ')');
                        Parameter.Id = Element.id;
                        this.TextHegihts[Element.id] = new TextHegiht(Parameter);
                    }
                }
            }
        }
    }
    Pub.OnLoad(TextHegiht.CheckTextHegiht, TextHegiht, true);
}