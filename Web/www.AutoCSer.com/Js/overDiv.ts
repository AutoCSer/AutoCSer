/// <reference path = "./base.page.ts" />
'use strict';
//AutoCSer.OverDiv.Default.Show();
module AutoCSer {
    interface IOverDivId {
        Id: string;
        ZIndex: any;
    }
    export class OverDiv {
        private static DefaultParameter = { Color: '#444444', Opacity: 90 };
        private Color: string;
        private Opacity: number;

        private Id: string;
        private Ids: IOverDivId[];
        constructor(Parameter = null) {
            Pub.GetParameter(this, OverDiv.DefaultParameter, Parameter);
            this.Id = '_' + (++Pub.Identity) + '_OVER_';
            this.Ids = [];
        }
        Show(Id: string = null, ZIndex = 0) {
            if (Id) {
                if (this.Ids.length) {
                    if (this.Ids[this.Ids.length - 1].Id != Id) {
                        var Index = this.Ids.IndexOf(function (Value) { return Value.Id == Id; });
                        if (Index + 1) this.Ids.splice(Index, 1);
                        var Value = this.Ids[this.Ids.length - 1], Element = HtmlElement.$Id(Value.Id);
                        if (!Value.ZIndex) this.Ids[this.Ids.length - 1].ZIndex = Element.Style0('zIndex') || 0;
                        Element.Style('zIndex', -100);
                        this.Ids.push({ Id: Id, ZIndex: ZIndex || 0 });
                    }
                }
                else this.Ids.push({ Id: Id, ZIndex: ZIndex || 0 });
            }
            var Element = HtmlElement.$Id(this.Id);
            if (Element.Element0()) Element.Display(1);
            else HtmlElement.$Create('div').Style('zIndex', HtmlElement.OverZIndex).Style('position', 'fixed').Styles('top,left', '0px').Styles('width,height', '100%').Style('backgroundColor', this.Color).Opacity(this.Opacity).Set('id', this.Id).To();
        }
        Hide(Id: string = null) {
            if (Id) {
                if (this.Ids.length) {
                    if (this.Ids[this.Ids.length - 1].Id == Id) {
                        this.Ids.pop();
                        if (this.Ids.length) {
                            var Value = this.Ids[this.Ids.length - 1];
                            HtmlElement.$Id(Value.Id).Style('zIndex', Value.ZIndex);
                        }
                    }
                    else this.Ids.Remove(function (Value) { return Value.Id == Id; });
                }
            }
            else {
                while (this.Ids.length) {
                    var Value = this.Ids.pop();
                    HtmlElement.$Id(Value.Id).Style('zIndex', Value.ZIndex);
                }
            }
            if (!this.Ids.length) HtmlElement.$Id(this.Id).Display(0);
        }
        static Default: OverDiv = new OverDiv();
    }
}