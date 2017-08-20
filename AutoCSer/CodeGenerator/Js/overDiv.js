/// <reference path = "./base.page.ts" />
'use strict';
//AutoCSer.OverDiv.Default.Show();
var AutoCSer;
(function (AutoCSer) {
    var OverDiv = (function () {
        function OverDiv(Parameter) {
            if (Parameter === void 0) { Parameter = null; }
            AutoCSer.Pub.GetParameter(this, OverDiv.DefaultParameter, Parameter);
            this.Id = '_' + (++AutoCSer.Pub.Identity) + '_OVER_';
            this.Ids = [];
        }
        OverDiv.prototype.Show = function (Id, ZIndex) {
            if (Id === void 0) { Id = null; }
            if (ZIndex === void 0) { ZIndex = 0; }
            if (Id) {
                if (this.Ids.length) {
                    if (this.Ids[this.Ids.length - 1].Id != Id) {
                        var Index = this.Ids.IndexOf(function (Value) { return Value.Id == Id; });
                        if (Index + 1)
                            this.Ids.splice(Index, 1);
                        var Value = this.Ids[this.Ids.length - 1], Element = AutoCSer.HtmlElement.$Id(Value.Id);
                        if (!Value.ZIndex)
                            this.Ids[this.Ids.length - 1].ZIndex = Element.Style0('zIndex') || 0;
                        Element.Style('zIndex', -100);
                        this.Ids.push({ Id: Id, ZIndex: ZIndex || 0 });
                    }
                }
                else
                    this.Ids.push({ Id: Id, ZIndex: ZIndex || 0 });
            }
            var Element = AutoCSer.HtmlElement.$Id(this.Id);
            if (Element.Element0())
                Element.Display(1);
            else
                AutoCSer.HtmlElement.$Create('div').Style('zIndex', AutoCSer.HtmlElement.OverZIndex).Style('position', 'fixed').Styles('top,left', '0px').Styles('width,height', '100%').Style('backgroundColor', this.Color).Opacity(this.Opacity).Set('id', this.Id).To();
        };
        OverDiv.prototype.Hide = function (Id) {
            if (Id === void 0) { Id = null; }
            if (Id) {
                if (this.Ids.length) {
                    if (this.Ids[this.Ids.length - 1].Id == Id) {
                        this.Ids.pop();
                        if (this.Ids.length) {
                            var Value = this.Ids[this.Ids.length - 1];
                            AutoCSer.HtmlElement.$Id(Value.Id).Style('zIndex', Value.ZIndex);
                        }
                    }
                    else
                        this.Ids.Remove(function (Value) { return Value.Id == Id; });
                }
            }
            else {
                while (this.Ids.length) {
                    var Value = this.Ids.pop();
                    AutoCSer.HtmlElement.$Id(Value.Id).Style('zIndex', Value.ZIndex);
                }
            }
            if (!this.Ids.length)
                AutoCSer.HtmlElement.$Id(this.Id).Display(0);
        };
        OverDiv.DefaultParameter = { Color: '#444444', Opacity: 90 };
        OverDiv.Default = new OverDiv();
        return OverDiv;
    }());
    AutoCSer.OverDiv = OverDiv;
})(AutoCSer || (AutoCSer = {}));
