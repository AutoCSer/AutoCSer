/// <reference path = "../../../AutoCSer/CodeGenerator/Js/ace/load.ts" />
/// <reference path = "../ViewJs/Api.ts" />
/*Include:js\ace\load*/
/*Include:Include\Baidu*/
module AutoCSer.Web.Include {
    class Code {
        private static id = 0;
        private div: AutoCSer.HtmlElement;
        private mode: string;
        private ace: AutoCSer.Ace;
        private display: number;
        constructor(mode: string, code: string, div: AutoCSer.HtmlElement) {
            this.mode = mode;
            this.div = div;
            div.Display(this.display = 1);
            if (AutoCSer.Ace) (this.ace = new AutoCSer.Ace({ Id: div.Id0(), MinLength: null, MaxHeight: 0, FontSize: 12, Code: code, Mode: this.mode, Theme: 'eclipse', IsWrap: true, IsReadOnly: true })).Show();
            else div.Text(code);
        }
        Show() {
            this.div.Display(this.display ^= 1);
        }
    }
    class File {
        private file: string;
        private mode: string;
        private code: string;
        private buttons: AutoCSer.HtmlElement[];
        constructor(type: string, file: string, mode: string, button: AutoCSer.HtmlElement) {
            this.file = file;
            this.mode = mode;
            this.buttons = [button];
            if (type == 'Example') AutoCSerAPI.Ajax.Example.GetCode(file, AutoCSer.Pub.ThisFunction(this, this.onGetCode));
            else AutoCSerAPI.Ajax.TestCase.GetCode(file, AutoCSer.Pub.ThisFunction(this, this.onGetCode));
        }
        private onGetCode(Value: AutoCSer.IHttpRequestReturn) {
            if (this.code = Value.__AJAXRETURN__) {
                for (var index = this.buttons.length; index; this.show(this.buttons[--index]));
                this.buttons = null;
            }
        }
        private show(button: AutoCSer.HtmlElement) {
            GetCode.Codes[button.Id0()] = new Code(this.mode, this.code, AutoCSer.HtmlElement.$Create("div").Set('id', button.Id0().substring(3)).To(button.Parent0()));
        }
        Add(button: AutoCSer.HtmlElement) {
            if (this.buttons) this.buttons.push(button);
            else this.show(button);
        }
    }
    export class GetCode {
        static Codes: { [key: string]: Code; } = {};
        private static files: { [key: string]: File; } = {};
        private static modes = { "cs": "csharp", "ts": "typescript", "css": "css", "html": "html", "json": "javascript" };
        static Get(type: string, buttonId: string) {
            var code = this.Codes[buttonId = 'GetCode' + buttonId];
            if (code) code.Show();
            else {
                var button = AutoCSer.HtmlElement.$Id(buttonId), path = button.Text0(), file = this.files[path];
                if (file) file.Add(button);
                else this.files[path] = new File(type, path, this.modes[path.substring(path.lastIndexOf('.') + 1)], button);
            }
        }
    }
}