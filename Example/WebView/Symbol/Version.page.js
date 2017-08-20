/// <reference path = "../../../AutoCSer/CodeGenerator/Js/load.page.ts" />
var AutoCSer;
(function (AutoCSer) {
    var Example;
    (function (Example) {
        var Symbol;
        (function (Symbol) {
            var Version = (function () {
                function Version() {
                }
                Version.Test = function () {
                    alert(AutoCSer.Loader.Version);
                };
                return Version;
            }());
            Symbol.Version = Version;
        })(Symbol = Example.Symbol || (Example.Symbol = {}));
    })(Example = AutoCSer.Example || (AutoCSer.Example = {}));
})(AutoCSer || (AutoCSer = {}));
