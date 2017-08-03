//本文件由程序自动生成,请不要自行修改
var AutoCSerAPI;
(function (AutoCSerAPI) {
    var Ajax;
    (function (Ajax) {
        var RefOut = (function () {
            function RefOut() {
            }
            RefOut.Add = function (left, right, Callback) {
                if (Callback === void 0) { Callback = null; }
                AutoCSer.Pub.GetAjaxPost()('RefOut.Add', { left: left, right: right }, Callback);
            };
            return RefOut;
        }());
        Ajax.RefOut = RefOut;
    })(Ajax = AutoCSerAPI.Ajax || (AutoCSerAPI.Ajax = {}));
})(AutoCSerAPI || (AutoCSerAPI = {}));
