var AutoCSer;
(function (AutoCSer) {
    var Example;
    (function (Example) {
        var Symbol;
        (function (Symbol) {
            var MainDomain = (function () {
                function MainDomain() {
                }
                MainDomain.Test = function () {
                    alert('__MAINDOMAIN__');
                };
                return MainDomain;
            }());
            Symbol.MainDomain = MainDomain;
        })(Symbol = Example.Symbol || (Example.Symbol = {}));
    })(Example = AutoCSer.Example || (AutoCSer.Example = {}));
})(AutoCSer || (AutoCSer = {}));
