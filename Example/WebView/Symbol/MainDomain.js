
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
					alert('127.0.0.1:14000');
				};
				return MainDomain;
			}());
			Symbol.MainDomain = MainDomain;
		})(Symbol = Example.Symbol || (Example.Symbol = {}));
	})(Example = AutoCSer.Example || (AutoCSer.Example = {}));
})(AutoCSer || (AutoCSer = {}));
