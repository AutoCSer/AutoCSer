/// <reference path = "../../../AutoCSer/CodeGenerator/Js/base.page.ts" />
module AutoCSerWeb {
    class SearchDataKey {
        Type: string;
        Id: number;
    }
    class SearchIndex {
        Key: number;
        Value: number;
    }
    export class SearchItem {
        DataKey: SearchDataKey;
        Id: string;
        Indexs: SearchIndex[];
        Text: string;
        constructor(Value: Object) {
            AutoCSer.Pub.Copy(this, Value);
            this.Id = this.DataKey.Type + this.DataKey.Id;
        }
        FormatHtml() {
            for (var Html = [], StartIndex = 0, Index = 0; Index - this.Indexs.length; ++Index) {
                var SearchIndex = this.Indexs[Index];
                if (StartIndex != SearchIndex.Key) Html.push(this.Text.substring(StartIndex, SearchIndex.Key).ToHTML());
                Html.push('<b style="color:red">');
                Html.push(this.Text.substring(SearchIndex.Key, (StartIndex = SearchIndex.Key +  SearchIndex.Value)).ToHTML());
                Html.push('</b>');
            }
            if (StartIndex != this.Text.length) Html.push(this.Text.substring(StartIndex).ToHTML());
            return Html.join('');
        }
    }
}
AutoCSer.Pub.LoadViewType(AutoCSerWeb.SearchItem);