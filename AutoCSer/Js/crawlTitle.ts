/// <reference path = "./base.page.ts" />
'use strict';
module AutoCSer {
    export interface ICrawlTitleLink {
        Link: string;
        Title: string;
        CallBack: ((Link: ICrawlTitleLink) => any)[];
        IsTitle: boolean;
    }
    export class CrawlTitle {
        private static DefaultParameter = { Link: null, AjaxCallName: null };
        private Link: ICrawlTitleLink;
        private AjaxCallName: string;

        constructor(Parameter) {
            Pub.GetParameter(this, CrawlTitle.DefaultParameter, Parameter);
            setTimeout(Pub.ThisFunction(this, this.Request), 1);
        }
        private Request() {
            var Link = this.Link.Link, Index = Link.indexOf('#');
            if ((Index + 1) && Link.charAt(Index + 1) != '!') Link = Link.substring(0, Index);
            HttpRequest.Post(this.AjaxCallName, { link: Link }, Pub.ThisFunction(this, this.OnLink));
        }
        private OnLink(Value: IHttpRequestReturn) {
            this.Link.IsTitle = true;
            if (Value.__AJAXRETURN__) {
                this.Link.Title = Pub.DeleteElements.Html(Value.__AJAXRETURN__).Text0();
                for (var Index = this.Link.CallBack.length; Index; this.Link.CallBack[--Index](this.Link));
            }
        }
        private static Titles: { [key: string]: ICrawlTitleLink }= {};
        private static Links: ICrawlTitleLink[] = [];
        static Get(Link: string, CallBack: (Link: ICrawlTitleLink) => any): ICrawlTitleLink {
            var Value = this.Titles[Link];
            if (!Value) this.Links.push(this.Titles[Link] = Value = { Link: Link, Title: Link, CallBack: [], IsTitle: false });
            if (CallBack && !Value.IsTitle && Value.CallBack.IndexOfValue(CallBack) < 0) Value.CallBack.push(CallBack);
            return Value;
        }
        private static Request(AjaxCallName: string) {
            for (var Index = this.Links.length; Index; new CrawlTitle({ Link: this.Links[--Index], AjaxCallName: AjaxCallName }));
            this.Links = [];
        }
        static TryRequest(AjaxCallName: string) {
            if (this.Links.length) setTimeout(Pub.ThisFunction(this, this.Request, [AjaxCallName]), 0);
        }
    }
}