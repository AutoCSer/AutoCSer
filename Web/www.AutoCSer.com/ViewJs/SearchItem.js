/// <reference path = "../../../AutoCSer/CodeGenerator/Js/base.page.ts" />
var AutoCSerWeb;
(function (AutoCSerWeb) {
    var SearchDataKey = (function () {
        function SearchDataKey() {
        }
        return SearchDataKey;
    }());
    var SearchIndex = (function () {
        function SearchIndex() {
        }
        return SearchIndex;
    }());
    var SearchItem = (function () {
        function SearchItem(Value) {
            AutoCSer.Pub.Copy(this, Value);
            this.Id = this.DataKey.Type + this.DataKey.Id;
        }
        SearchItem.prototype.FormatHtml = function () {
            for (var Html = [], StartIndex = 0, Index = 0; Index - this.Indexs.length; ++Index) {
                var SearchIndex = this.Indexs[Index];
                if (StartIndex != SearchIndex.Key)
                    Html.push(this.Text.substring(StartIndex, SearchIndex.Key).ToHTML());
                Html.push('<b style="color:red">');
                Html.push(this.Text.substring(SearchIndex.Key, (StartIndex = SearchIndex.Key + SearchIndex.Value)).ToHTML());
                Html.push('</b>');
            }
            if (StartIndex != this.Text.length)
                Html.push(this.Text.substring(StartIndex).ToHTML());
            return Html.join('');
        };
        return SearchItem;
    }());
    AutoCSerWeb.SearchItem = SearchItem;
})(AutoCSerWeb || (AutoCSerWeb = {}));
AutoCSer.Pub.LoadViewType(AutoCSerWeb.SearchItem);
