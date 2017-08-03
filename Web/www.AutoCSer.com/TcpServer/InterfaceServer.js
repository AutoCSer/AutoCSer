
'use strict';
var AutoCSer;
(function (AutoCSer) {
	var AceParameter = (function () {
		function AceParameter() {
		}
		return AceParameter;
	}());
	var AceSessionIE6 = (function () {
		function AceSessionIE6(Editor) {
			this.Editor = Editor;
		}
		AceSessionIE6.prototype.getLength = function () {
			var Code = AutoCSer.HtmlElement.$Id(this.Editor.Id).Value0();
			return Code ? 0 : Code.length;
		};
		AceSessionIE6.prototype.getScreenLength = function () {
			return 0;
		};
		AceSessionIE6.prototype.setMode = function (Mode) { };
		AceSessionIE6.prototype.setTheme = function (Theme) { };
		AceSessionIE6.prototype.setUseWrapMode = function (IsWrap) { };
		AceSessionIE6.prototype.on = function (Name, Function) { };
		return AceSessionIE6;
	}());
	var AceEditorIE6 = (function () {
		function AceEditorIE6(Ace) {
			this.Ace = Ace;
			var Div = AutoCSer.HtmlElement.$Id(Ace.Id);
			if (Ace.Code == null)
				Ace.Code = Div.Text0();
			Div.Html('<textarea id="' + (this.Id = 'AceIe6_' + Ace.Id) + '" style="width:' + Div.Width0() + 'px;height:' + Div.Height0() + 'px"></textarea>');
			this.Session = new AceSessionIE6(this);
		}
		AceEditorIE6.prototype.getSession = function () {
			return this.Session;
		};
		AceEditorIE6.prototype.setValue = function (Code) {
			AutoCSer.HtmlElement.$SetValueById(this.Id, Code);
		};
		AceEditorIE6.prototype.getValue = function () {
			return AutoCSer.HtmlElement.$GetValueById(this.Id);
		};
		AceEditorIE6.prototype.setTheme = function (Theme) { };
		AceEditorIE6.prototype.setReadOnly = function (IsReadOnly) {
			AutoCSer.HtmlElement.$Id(this.Id).Set('readOnly', IsReadOnly);
		};
		AceEditorIE6.prototype.moveCursorTo = function (Row, Col) { };
		AceEditorIE6.prototype.focus = function () {
			AutoCSer.HtmlElement.$Id(this.Id).Focus0();
		};
		AceEditorIE6.prototype.resize = function () { };
		AceEditorIE6.prototype.on = function (Name, Function) { };
		return AceEditorIE6;
	}());
	var Ace = (function (_super) {
		AutoCSer.Pub.Extends(Ace, _super);
		function Ace(Parameter) {
			_super.call(this);
			AutoCSer.Pub.GetParameter(this, Ace.DefaultParameter, Parameter);
			(this.OnChange = new AutoCSer.Events).Add(AutoCSer.Pub.ThisFunction(this, this.Resize));
		}
		Ace.prototype.Check = function () {
			if (AutoCSer.HtmlElement.$Id(this.Id).Attribute0('ace') == 'ace')
				return this;
			var Value = new Ace(this.Parameter);
			Value.Show();
			return Value;
		};
		Ace.prototype.Show = function () {
			AutoCSer.Skin.Refresh();
			var Div = AutoCSer.HtmlElement.$Id(this.Id);
			if (Div.Element0()) {
				if (Ace.IsIE6)
					this.Editor = new AceEditorIE6(this);
				else {
					var Height = this.IsReadOnly ? 0 : Div.Height0();
					(this.Editor = window['ace'].edit(this.Id)).setFontSize(this.FontSize);
					var LineHeight = this.Editor.renderer.lineHeight || 14;
					if (this.IsReadOnly) {
						this.MinLength = 1;
						Div.Style('height', (((this.LastLength = this.Editor.getSession().getLength()) + (this.IsWrap ? 0 : 1)) * LineHeight + 2) + 'px');
					}
					else {
						if (!this.MinLength)
							this.MinLength = Math.floor((Height + LineHeight - 1) / LineHeight);
						Div.Style('height', (((this.LastLength = this.MinLength) + (this.IsWrap ? 0 : 1)) * LineHeight + 2) + 'px');
					}
					var Session = this.Editor.getSession();
					Session.setMode('ace/mode/' + this.Mode);
					Session.setUseWrapMode(this.IsWrap);
					Session.on('change', this.OnChange.Function);
					Session.on('changeFold', this.OnChange.Function);
					this.Editor.setTheme('ace/theme/' + this.Theme);
				}
				if (this.Code != null)
					this.Editor.setValue(this.Code);
				this.Editor.setReadOnly(this.IsReadOnly);
				this.Editor.moveCursorTo(0, 0);
				if (!this.IsReadOnly)
					this.Editor.focus();
				Div.Set('ace', 'ace');
			}
		};
		Ace.prototype.Set = function (Value) {
			var Session = this.Editor.getSession();
			if (Value.Mode)
				Session.setMode('ace/mode/' + (this.Mode = Value.Mode));
			if (Value.Theme)
				Session.setTheme('ace/theme/' + (this.Theme = Value.Theme));
			if (Value.Code != null)
				this.Editor.setValue(Value.Code);
			this.Editor.moveCursorTo(0, 0);
			if (!this.IsReadOnly)
				this.Editor.focus();
		};
		Ace.prototype.Resize = function () {
			if (!Ace.IsIE6) {
				var Length = this.Editor.getSession().getScreenLength();
				if (this.MaxHeight) {
					var MaxLength = Math.floor(((this.MaxHeight < 0 ? (AutoCSer.HtmlElement.$Height() + this.MaxHeight) : this.MaxHeight) - 2) / this.Editor.renderer.lineHeight) - (this.IsWrap ? 0 : 1);
					if (Length > MaxLength)
						Length = MaxLength;
				}
				if (Length < this.MinLength)
					Length = this.MinLength;
				if (Length != this.LastLength) {
					AutoCSer.HtmlElement.$Id(this.Id).Style('height', (((this.LastLength = Length) + (this.IsWrap ? 0 : 1)) * this.Editor.renderer.lineHeight + 2) + 'px');
					this.Editor.resize();
				}
			}
		};
		Ace.LoadIE6 = function () {
			this.Load();
			this.Show();
		};
		Ace.Load = function () {
			if (AutoCSer.Pub.PageView.OnSet)
				AutoCSer.Pub.PageView.OnSet.Add(AutoCSer.Pub.ThisFunction(this, this.Show));
		};
		Ace.Show = function () {
			if (this.IsIE6 || window['ace']) {
				for (var Elements = AutoCSer.HtmlElement.$Name('ace').GetElements(), Index = 0; Index - Elements.length; ++Index) {
					if (!AutoCSer.Pub.GetHtmlEditor(Elements[Index])) {
						var Div = Elements[Index];
						if (Div.offsetHeight) {
							var Mode = AutoCSer.HtmlElement.$Attribute(Div, 'mode');
							if (!Div.id && Mode) {
								var ParameterString = AutoCSer.HtmlElement.$Attribute(Div, 'ace'), Parameter = ParameterString && ParameterString != 'ace' ? eval('(' + ParameterString + ')') : new AceParameter(), Codes = [];
								Parameter.Id = Div.id = 'AutoCSerAce' + (++this.Identity);
								for (var CodeNodes = Div.childNodes, CodeIndex = 0; CodeIndex !== CodeNodes.length; ++CodeIndex) {
									var Node = CodeNodes[CodeIndex];
									if (Node.tagName)
										Codes.push(AutoCSer.HtmlElement.$GetText(Node));
								}
								Parameter.Code = Codes.join('\n').replace(/\xA0/g, ' ');
								Parameter.Mode = Mode;
								if (Parameter.IsReadOnly == null)
									Parameter.IsReadOnly = true;
								new Ace(Parameter).Show();
							}
						}
					}
				}
			}
		};
		Ace.LoadMoule = function (Function, IsLoad) {
			if (IsLoad === void 0) { IsLoad = true; }
			if (Function) {
				if (this.IsIE6) {
					if (IsLoad)
						AutoCSer.Pub.OnLoad(Function, null, true);
					else
						Function();
				}
				else
					AutoCSer.Pub.OnModule(['ace/ace'], Function, IsLoad);
			}
		};
		Ace.CheckIE6 = function () {
			if (AutoCSer.Pub.IE) {
				var Version = navigator.appVersion.match(/MSIE\s+(\d+)/);
				if (Version && Version.length == 2 && parseInt('0' + Version[1], 10) < 7) {
					AutoCSer.Pub.OnLoad(this.LoadIE6, this, this.IsIE6 = true);
					return;
				}
			}
			AutoCSer.Pub.OnLoad(this.Load, this, true);
			AutoCSer.Pub.OnModule(['ace/ace'], AutoCSer.Pub.ThisFunction(this, this.Show), true);
		};
		Ace.DefaultParameter = { Id: null, MinLength: null, MaxHeight: 0, FontSize: 12, Code: '', Mode: 'csharp', Theme: 'eclipse', IsWrap: true, IsReadOnly: false };
		Ace.Identity = 0;
		return Ace;
	}(AceParameter));
	AutoCSer.Ace = Ace;
	AutoCSer.Ace.CheckIE6();
})(AutoCSer || (AutoCSer = {}));

var _hmt = _hmt || [];
(function() {
  var hm = document.createElement("script");
  hm.src = "https://hm.baidu.com/hm.js?dbf5c7b4884f5ce1150a825c5d55bdc9";
  var s = document.getElementsByTagName("script")[0]; 
  s.parentNode.insertBefore(hm, s);
})();


var AutoCSer;
(function (AutoCSer) {
	var Web;
	(function (Web) {
		var Include;
		(function (Include) {
			var Code = (function () {
				function Code(mode, code, div) {
					this.mode = mode;
					this.div = div;
					div.Display(this.display = 1);
					if (AutoCSer.Ace)
						(this.ace = new AutoCSer.Ace({ Id: div.Id0(), MinLength: null, MaxHeight: 0, FontSize: 12, Code: code, Mode: this.mode, Theme: 'eclipse', IsWrap: true, IsReadOnly: true })).Show();
					else
						div.Text(code);
				}
				Code.prototype.Show = function () {
					this.div.Display(this.display ^= 1);
				};
				Code.id = 0;
				return Code;
			}());
			var File = (function () {
				function File(type, file, mode, button) {
					this.file = file;
					this.mode = mode;
					this.buttons = [button];
					if (type == 'Example')
						AutoCSerAPI.Ajax.Example.GetCode(file, AutoCSer.Pub.ThisFunction(this, this.onGetCode));
					else
						AutoCSerAPI.Ajax.TestCase.GetCode(file, AutoCSer.Pub.ThisFunction(this, this.onGetCode));
				}
				File.prototype.onGetCode = function (Value) {
					if (this.code = Value.Return) {
						for (var index = this.buttons.length; index; this.show(this.buttons[--index]))
							;
						this.buttons = null;
					}
				};
				File.prototype.show = function (button) {
					GetCode.Codes[button.Id0()] = new Code(this.mode, this.code, AutoCSer.HtmlElement.$Create("div").Set('id', button.Id0().substring(3)).To(button.Parent0()));
				};
				File.prototype.Add = function (button) {
					if (this.buttons)
						this.buttons.push(button);
					else
						this.show(button);
				};
				return File;
			}());
			var GetCode = (function () {
				function GetCode() {
				}
				GetCode.Get = function (type, buttonId) {
					var code = this.Codes[buttonId = 'GetCode' + buttonId];
					if (code)
						code.Show();
					else {
						var button = AutoCSer.HtmlElement.$Id(buttonId), path = button.Text0(), file = this.files[path];
						if (file)
							file.Add(button);
						else
							this.files[path] = new File(type, path, this.modes[path.substring(path.lastIndexOf('.') + 1)], button);
					}
				};
				GetCode.Codes = {};
				GetCode.files = {};
				GetCode.modes = { "cs": "csharp", "ts": "typescript", "css": "css", "html": "html", "json": "javascript" };
				return GetCode;
			}());
			Include.GetCode = GetCode;
		})(Include = Web.Include || (Web.Include = {}));
	})(Web = AutoCSer.Web || (AutoCSer.Web = {}));
})(AutoCSer || (AutoCSer = {}));


