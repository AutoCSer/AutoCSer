/// <reference path = "../../../AutoCSer/CodeGenerator/Js/ace/load.ts" />
/// <reference path = "../ViewJs/Api.ts" />
/*Include:js\ace\load*/
/*Include:Include\Baidu*/
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
                    if (this.code = Value.__AJAXRETURN__) {
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
