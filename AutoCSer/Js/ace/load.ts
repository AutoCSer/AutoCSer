/// <reference path = "../base.page.ts" />
'use strict';
module AutoCSer {
    interface IAceEditor {
        renderer: IAceRenderer;
        getSession(): IAceSession;
        setValue(Code: string);
        getValue(): string;
        setTheme(Theme: string);
        setReadOnly(IsReadOnly: boolean);
        moveCursorTo(Row: number, Col: number);
        focus();
        resize();
        on(Name: string, Function: Function);
    }
    interface IAceRenderer {
        lineHeight: number;
    }
    export interface IAceSession {
        getLength(): number;
        getScreenLength(): number;
        setMode(Mode: string);
        setTheme(Theme: string);
        setUseWrapMode(IsWrap: boolean);
        on(Name: string, Function: Function);
    }
    class AceParameter {
        Id: string;
        MinLength: number;
        MaxHeight: number;
        FontSize: number;
        Code: string;
        Mode: string;
        Theme: string;
        IsWrap: boolean;
        IsReadOnly: boolean;
    }
    class AceSessionIE6 implements IAceSession {
        private Editor: AceEditorIE6;
        constructor(Editor: AceEditorIE6) {
            this.Editor = Editor;
        }
        getLength(): number {
            var Code = HtmlElement.$Id(this.Editor.Id).Value0();
            return Code ? 0 : Code.length;
        }
        getScreenLength(): number {
            return 0;
        }
        setMode(Mode: string) { }
        setTheme(Theme: string) { }
        setUseWrapMode(IsWrap: boolean) { }
        on(Name: string, Function: Function) { }
    }
    class AceEditorIE6 implements IAceEditor {
        private Ace: Ace;
        Id: string;
        private Session: AceSessionIE6;
        renderer: IAceRenderer;
        constructor(Ace: Ace) {
            this.Ace = Ace;
            var Div = HtmlElement.$Id(Ace.Id);
            if (Ace.Code == null) Ace.Code = Div.Text0();
            Div.Html('<textarea id="' + (this.Id = 'AceIe6_' + Ace.Id) + '" style="width:' + Div.Width0() + 'px;height:' + Div.Height0() + 'px"></textarea>');
            this.Session = new AceSessionIE6(this);
        }
        getSession(): IAceSession {
            return this.Session;
        }
        setValue(Code: string) {
            HtmlElement.$SetValueById(this.Id, Code);
        }
        getValue(): string {
            return HtmlElement.$GetValueById(this.Id);
        }
        setTheme(Theme: string) { }
        setReadOnly(IsReadOnly: boolean) {
            HtmlElement.$Id(this.Id).Set('readOnly', IsReadOnly);
        }
        moveCursorTo(Row: number, Col: number) { }
        focus() {
            HtmlElement.$Id(this.Id).Focus0();
        }
        resize() { }
        on(Name: string, Function: Function) { }
    }
    export class Ace extends AceParameter {
        private static DefaultParameter: AceParameter = { Id: null, MinLength: null, MaxHeight: 0, FontSize: 12, Code: '', Mode: 'csharp', Theme: 'eclipse', IsWrap: true, IsReadOnly: false };

        private Parameter: AceParameter;
        private OnChange: Events;
        Editor: IAceEditor;
        private LastLength: number;
        constructor(Parameter: AceParameter) {
            super();
            Pub.GetParameter(this, Ace.DefaultParameter, Parameter);
            (this.OnChange = new Events).Add(Pub.ThisFunction(this, this.Resize));
        }
        Check(): Ace {
            if (HtmlElement.$Id(this.Id).Attribute0('ace')=='ace') return this;
            var Value = new Ace(this.Parameter);
            Value.Show();
            return Value;
        }
        Show() {
            AutoCSer.Skin.Refresh();
            var Div = HtmlElement.$Id(this.Id);
            if (Div.Element0()) {
                if (Ace.IsIE6) this.Editor = new AceEditorIE6(this);
                else {
                    var Height = this.IsReadOnly ? 0 : Div.Height0();
                    (this.Editor = window['ace'].edit(this.Id)).setFontSize(this.FontSize);
                    var LineHeight = this.Editor.renderer.lineHeight || 14;
                    if (this.IsReadOnly) {
                        this.MinLength = 1;
                        Div.Style('height', (((this.LastLength = this.Editor.getSession().getLength()) + (this.IsWrap ? 0 : 1)) * LineHeight + 2) + 'px');
                    }
                    else {
                        if (!this.MinLength) this.MinLength = Math.floor((Height + LineHeight - 1) / LineHeight);
                        Div.Style('height', (((this.LastLength = this.MinLength) + (this.IsWrap ? 0 : 1)) * LineHeight + 2) + 'px');
                    }
                    var Session = this.Editor.getSession();
                    Session.setMode('ace/mode/' + this.Mode);
                    Session.setUseWrapMode(this.IsWrap);
                    Session.on('change', this.OnChange.Function);
                    Session.on('changeFold', this.OnChange.Function);
                    this.Editor.setTheme('ace/theme/' + this.Theme);
                }
                if (this.Code != null) this.Editor.setValue(this.Code);
                this.Editor.setReadOnly(this.IsReadOnly);
                this.Editor.moveCursorTo(0, 0);
                if (!this.IsReadOnly) this.Editor.focus();
                Div.Set('ace', 'ace');
            }
        }
        private Set(Value) {
            var Session = this.Editor.getSession();
            if (Value.Mode) Session.setMode('ace/mode/' + (this.Mode = Value.Mode));
            if (Value.Theme) Session.setTheme('ace/theme/' + (this.Theme = Value.Theme));
            if (Value.Code != null) this.Editor.setValue(Value.Code);
            this.Editor.moveCursorTo(0, 0);
            if (!this.IsReadOnly) this.Editor.focus();
        }
        private Resize() {
            if (!Ace.IsIE6) {
                var Length = this.Editor.getSession().getScreenLength();
                if (this.MaxHeight) {
                    var MaxLength = Math.floor(((this.MaxHeight < 0 ? (HtmlElement.$Height() + this.MaxHeight) : this.MaxHeight) - 2) / this.Editor.renderer.lineHeight) - (this.IsWrap ? 0 : 1);
                    if (Length > MaxLength) Length = MaxLength;
                }
                if (Length < this.MinLength) Length = this.MinLength;
                if (Length != this.LastLength) {
                    HtmlElement.$Id(this.Id).Style('height', (((this.LastLength = Length) + (this.IsWrap ? 0 : 1)) * this.Editor.renderer.lineHeight + 2) + 'px');
                    this.Editor.resize();
                }
            }
        }
        private static Identity = 0;
        private static IsIE6: boolean;
        private static LoadIE6() {
            this.Load();
            this.Show();
        }
        private static Load() {
            if (Pub.PageView.OnSet) Pub.PageView.OnSet.Add(Pub.ThisFunction(this, this.Show));
        }
        private static Show() {
            if (this.IsIE6 || window['ace']) {
                for (var Elements = HtmlElement.$Name('ace').GetElements(), Index = 0; Index - Elements.length; ++Index) {
                    if (!Pub.GetHtmlEditor(Elements[Index])) {
                        var Div = Elements[Index];
                        if (Div.offsetHeight) {
                            var Mode = HtmlElement.$Attribute(Div, 'mode');
                            if (!Div.id && Mode) {
                                var ParameterString = HtmlElement.$Attribute(Div, 'ace'), Parameter = ParameterString && ParameterString != 'ace' ? eval('(' + ParameterString + ')') : new AceParameter(), Codes = [];
                                Parameter.Id = Div.id = 'AutoCSerAce' + (++this.Identity);
                                for (var CodeNodes = Div.childNodes, CodeIndex = 0; CodeIndex !== CodeNodes.length; ++CodeIndex) {
                                    var Node = CodeNodes[CodeIndex] as HTMLElement;
                                    if (Node.tagName) Codes.push(HtmlElement.$GetText(Node));
                                }
                                Parameter.Code = Codes.join('\n').replace(/\xA0/g, ' ');
                                Parameter.Mode = Mode;
                                if (Parameter.IsReadOnly == null) Parameter.IsReadOnly = true;
                                new Ace(Parameter).Show();
                            }
                        }
                    }
                }
            }
        }
        static LoadMoule(Function: Function, IsLoad = true) {
            if (Function) {
                if (this.IsIE6) {
                    if (IsLoad) Pub.OnLoad(Function, null, true);
                    else Function();
                }
                else Pub.OnModule(['ace/ace'], Function, IsLoad);
            }
        }
        static CheckIE6() {
            if (Pub.IE) {
                var Version = navigator.appVersion.match(/MSIE\s+(\d+)/);
                if (Version && Version.length == 2 && parseInt('0' + Version[1], 10) < 7) {
                    Pub.OnLoad(this.LoadIE6, this, this.IsIE6 = true);
                    return;
                }
            }
            Pub.OnLoad(this.Load, this, true);
            Pub.OnModule(['ace/ace'], Pub.ThisFunction(this, this.Show), true);
        }
    }
    AutoCSer.Ace.CheckIE6();
}