/// <reference path = "./base.page.ts" />
'use strict';
//文本收起	<div id="XXX" textheight="{Height:80,HideSize:280,TextId:'YYY',ShowButton:'AAA',HideButton:'BBB'}"><span id="YYY">Text<span><span id="AAA" style="display:none">[收起]</span><span id="BBB" style="display:none">[全文]</span></div>
var AutoCSer;
(function (AutoCSer) {
    var TextHegiht = (function () {
        function TextHegiht(Parameter) {
            AutoCSer.Pub.GetParameter(this, TextHegiht.DefaultParameter, Parameter);
            if (this.Suffix) {
                this.ScrollId += this.Suffix;
                if (this.ShowButton)
                    this.ShowButton += this.Suffix;
                else
                    this.ShowButton = this.TextId + 'Show' + this.Suffix;
                if (this.HideButton)
                    this.HideButton += this.Suffix;
                else
                    this.HideButton = this.TextId + 'Hide' + this.Suffix;
                this.TextId += this.Suffix;
            }
            this.HideId = this.TextId + 'Hide';
            this.Start();
        }
        TextHegiht.prototype.Start = function () {
            var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
            if (Element != this.Element) {
                this.Element = Element;
                AutoCSer.HtmlElement.$Create('span').Set('id', this.HideId).InsertBefore(AutoCSer.HtmlElement.$IdElement(this.TextId)).Display(0);
                AutoCSer.HtmlElement.$Id(this.ShowButton).AddEvent('click', AutoCSer.Pub.ThisFunction(this, this.Show));
                AutoCSer.HtmlElement.$Id(this.HideButton).AddEvent('click', AutoCSer.Pub.ThisFunction(this, this.Hide));
                this.IsCheck = false;
            }
            if (!this.IsCheck) {
                this.IsCheck = true;
                if (AutoCSer.HtmlElement.$Id(this.Id).Height0() > this.Height) {
                    AutoCSer.HtmlElement.$Id(this.HideId).Html(AutoCSer.HtmlElement.$Id(this.TextId).Text0().Left(this.HideSize).ToHTML());
                    this[this.IsHide ? 'Hide' : 'Show'](1);
                }
            }
        };
        TextHegiht.prototype.Show = function (NoScroll) {
            if (!TextHegiht.IsSetPageView) {
                this.Hide(NoScroll, false);
                TextHegiht.IsSetPageView = true;
                AutoCSer.Skin.Body.OnSet.Function();
                TextHegiht.IsSetPageView = false;
            }
        };
        TextHegiht.prototype.Hide = function (NoScroll, IsHide) {
            if (IsHide === void 0) { IsHide = true; }
            this.IsHide = IsHide;
            AutoCSer.HtmlElement.$Id(this.TextId).Display(!IsHide);
            AutoCSer.HtmlElement.$Id(this.HideButton).Display(!IsHide);
            AutoCSer.HtmlElement.$Id(this.HideId).Display(IsHide);
            AutoCSer.HtmlElement.$Id(this.ShowButton).Display(IsHide);
            if (!NoScroll)
                AutoCSer.HtmlElement.$SetScrollTop(AutoCSer.HtmlElement.$Id(this.ScrollId || this.HideId).XY0().Top);
        };
        TextHegiht.CheckTextHegiht = function () {
            if (!this.TextHegihts) {
                this.TextHegihts = {};
                AutoCSer.Skin.Body.OnSet.Add(AutoCSer.Pub.ThisFunction(this, this.CheckTextHegiht));
            }
            if (!this.IsSetPageView) {
                for (var Elements = AutoCSer.HtmlElement.$('@textheight').GetElements(), Index = Elements.length; --Index >= 0;) {
                    var Element = Elements[Index], TestHeight = this.TextHegihts[Element.id];
                    if (TestHeight)
                        TestHeight.Start();
                    else {
                        var Parameter = eval('(' + AutoCSer.HtmlElement.$Attribute(Element, 'textheight') + ')');
                        Parameter.Id = Element.id;
                        this.TextHegihts[Element.id] = new TextHegiht(Parameter);
                    }
                }
            }
        };
        TextHegiht.DefaultParameter = { Id: null, Suffix: null, Height: null, HideSize: null, TextId: null, ShowButton: null, HideButton: null, ScrollId: null, IsHide: 1 };
        return TextHegiht;
    }());
    AutoCSer.TextHegiht = TextHegiht;
    AutoCSer.Pub.OnLoad(TextHegiht.CheckTextHegiht, TextHegiht, true);
})(AutoCSer || (AutoCSer = {}));
