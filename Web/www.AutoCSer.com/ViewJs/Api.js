//本文件由程序自动生成,请不要自行修改
var AutoCSerAPI;
(function (AutoCSerAPI) {
    var Ajax;
    (function (Ajax) {
        var Example = (function () {
            function Example() {
            }
            Example.GetCode = function (file, Callback) {
                if (Callback === void 0) { Callback = null; }
                AutoCSer.Pub.GetAjaxGet()('Example.GetCode', { file: file }, Callback, true);
            };
            return Example;
        }());
        Ajax.Example = Example;
    })(Ajax = AutoCSerAPI.Ajax || (AutoCSerAPI.Ajax = {}));
})(AutoCSerAPI || (AutoCSerAPI = {}));
var AutoCSerAPI;
(function (AutoCSerAPI) {
    var Ajax;
    (function (Ajax) {
        var TestCase = (function () {
            function TestCase() {
            }
            TestCase.GetCode = function (file, Callback) {
                if (Callback === void 0) { Callback = null; }
                AutoCSer.Pub.GetAjaxGet()('TestCase.GetCode', { file: file }, Callback, true);
            };
            return TestCase;
        }());
        Ajax.TestCase = TestCase;
    })(Ajax = AutoCSerAPI.Ajax || (AutoCSerAPI.Ajax = {}));
})(AutoCSerAPI || (AutoCSerAPI = {}));
