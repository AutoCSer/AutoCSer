'use strict';
var AutoCSer;
(function (AutoCSer) {
	var Loader = (function () {
		function Loader() {
		}
		Loader.CreateJavascipt = function (Src, Charset) {
			if (Charset === void 0) { Charset = Loader.Charset; }
			var Script = document.createElement('script');
			Script.lang = 'javascript';
			Script.type = 'text/javascript';
			Script.src = Src;
			Script.charset = Charset;
			return Script;
		};
		Loader.AppendJavaScript = function (Src, Charset) {
			if (Charset === void 0) { Charset = Loader.Charset; }
			this.DocumentHead.appendChild(this.CreateJavascipt(Src, Charset));
		};
		Loader.Load = function () {
			Loader.DocumentHead = document.getElementsByTagName('head')[0];
			for (var Nodes = Loader.DocumentHead.childNodes, Index = Nodes.length; Index !== 0;) {
				var Node = Nodes[--Index];
				if (Node.tagName && Node.tagName.toLowerCase() === 'script') {
					var LoadJs = Node.src.match(/^(https?:\/\/[^\/]+\/)Js\/load(Page)?\.js\?v(v?)=(0?)([\dABCDEF]+)?$/i);
					if (LoadJs && LoadJs[1] && LoadJs[5]) {
						Loader.JsDomain = LoadJs[1];
						Loader.ViewVersion = LoadJs[3];
						Loader.LoadScript = LoadJs[4];
						Loader.Version = LoadJs[5];
						Loader.Charset = Node.charset;
						break;
					}
				}
			}
			if (!Loader.JsDomain)
				Loader.JsDomain = '/';
			Loader.Charset='utf-8';
			Loader.AppendJavaScript(Loader.JsDomain + 'Js/base.js?v=' + Loader.Version);
		};
		return Loader;
	}());
	AutoCSer.Loader = Loader;
	Loader.Load();
})(AutoCSer || (AutoCSer = {}));
