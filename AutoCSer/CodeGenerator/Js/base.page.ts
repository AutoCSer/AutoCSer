/// <reference path = "./load.page.ts" />
'use strict';
interface Array<T> {
    Copy(): Array<T>;
    Push(Value: T): Array<T>;
    RemoveValue(Value: T): Array<T>;
    Remove(IsValue: (Value: T) => boolean): Array<T>;
    RemoveAt(Index: number): Array<T>;
    RemoveAt(Index: number, Count: number): Array<T>;
    RemoveAtEnd(Index: number): Array<T>;
    IndexOf(Function: (T) => boolean): number;
    IndexOfValue(Value: T): number;
    First(Function: (T) => boolean): T;
    MaxValue(): T;
    Max(GetKey: (Value: T) => any): T;
    FormatAjax(): any[];
    FormatView(): any[];
    Find(IsValue: (Value: T) => boolean): Array<T>;
    ToArray<R>(GetValue: (Value: T) => R): Array<R>;
    For(Function: (Value: T) => any): Array<T>;
    ToHash<K>(Function: (Value: T) => K): Object;
    Sort(GetKey: (Value: any) => number): T[];
    MakeString(): string;
}
Array.prototype.Copy = function () {
    for (var Index = 0, Array = []; Index !== this.length; Array.push(this[Index++]));
    return Array;
}
Array.prototype.Push = function (Value: any): any[] {
    this.push(Value);
    return this;
}
Array.prototype.RemoveValue = function (Value: any): any[] {
    return this.Remove(function (Data) { return Data == Value; });
}
Array.prototype.Remove = function (IsValue: (Value: any) => boolean): any[] {
    for (var Index = -1; ++Index != this.length;) {
        if (IsValue(this[Index])) {
            for (var WriteIndex = Index; ++Index != this.length;) {
                if (!IsValue(this[Index])) this[WriteIndex++] = this[Index];
            }
            this.length = WriteIndex;
            break;
        }
    }
    return this;
}
Array.prototype.RemoveAt = function (Index: number, Count = 1) {
    if (Index >= 0) this.splice(Index, Count);
    return this;
}
Array.prototype.RemoveAtEnd = function (Index: number) {
    if (Index >= 0 && Index < this.length) {
        this[Index] = this[this.length - 1];
        --this.length;
    }
    return this;
}
Array.prototype.IndexOf = function (Function: (any) => boolean): number {
    for (var Index = -1; ++Index !== this.length;) {
        if (Function(this[Index])) return Index;
    }
    return -1;
}
Array.prototype.IndexOfValue = function (Value: any): number {
    if (AutoCSer.Pub.IE) {
        for (var Index = -1; ++Index !== this.length;) {
            if (this[Index] == Value) return Index;
        }
        return -1;
    }
    return this.indexOf(Value);
}
Array.prototype.First = function (Function: (any) => boolean): any {
    var Index = this.IndexOf(Function);
    if (Index >= 0) return this[Index];
}
Array.prototype.MaxValue = function (): any {
    if (this.length) {
        for (var Index = this.length, Value = this[--Index]; Index;) {
            if (this[--Index] > Value) Value = this[Index];
        }
        return Value;
    }
}
Array.prototype.Max = function (GetKey: (Value: any) => any): any {
    if (this.length) {
        for (var Index = this.length, Value = this[--Index], Key = GetKey(Value); Index;) {
            var NextKey = GetKey(this[--Index]);
            if (NextKey > Key) {
                Value = this[Index];
                Key = NextKey;
            }
        }
        return Value;
    }
}
Array.prototype.FormatAjax = function (): any[]{
    if (this.length) {
        for (var Index = 0, Names = (this[0] as string).split(','), Values = []; ++Index !== this.length;) {
            for (var Value = this[Index] as any[], NewValue = {},NameIndex = -1; ++NameIndex !== Names.length; NewValue[Names[NameIndex]] = Value[NameIndex]);
            Values.push(NewValue);
        }
        return Values;
    }
}
Array.prototype.FormatView = function (): any[] {
    return this.length > 1 ? AutoCSer.Pub.FormatAjaxs(AutoCSer.Pub.AjaxName(this[0], 0), this, 1) : [];
}
Array.prototype.Find = function (IsValue: (Value: any) => boolean): any[] {
    for (var Values = [], Index = 0; Index !== this.length; ++Index) {
        var Value = this[Index];
        if (IsValue(Value)) Values.push(Value);
    }
    return Values;
}
Array.prototype.ToArray = function (GetValue: (Value: any) => any): any[] {
    for (var Values = [], Index = 0; Index !== this.length; Values.push(GetValue(this[Index++])));
    return Values;
}
Array.prototype.For = function (Function: (Value: any) => any): any[] {
    for (var Index = 0; Index !== this.length; Function(this[Index++]));
    return this;
}
Array.prototype.ToHash = function (Function: (Value: any) => any): Object {
    for (var Values = {}, Index = 0; Index !== this.length;) {
        var Value = this[Index++];
        Values[Function ? Function(Value) : Value] = Value;
    }
    return Values;
}
Array.prototype.Sort = function (GetKey: (Value: any) => number): any[] {
    return this.sort(function (Left, Right) { return GetKey(Left) - GetKey(Right); });
}
Array.prototype.MakeString = function (): string {
    return String.fromCharCode.apply(null, this);
}
interface String {
    Escape(): string;
    ToHTML(): string;
    ToTextArea(): string;
    Trim(): string;
    PadLeft(Count: number, Char: string): string;
    Left(Length: number): string;
    Right(Length: number): string;
    ToLower(): string;
    Length(): number;
    SplitInt(Split: string): number[];
    ParseDate(): Date;
}
String.prototype.Escape = function (): string {
    return window['escape'](this.replace(/\xA0/g, ' ')).replace(/\+/g, '%2b');
}
String.prototype.ToHTML = function (): string {
    return this.ToTextArea().replace(/ /g, '&nbsp;').replace(/"/g, '&#34;').replace(/'/g, '&#39;');
    //.replace(/\\/g, '&#92;');
};
String.prototype.ToTextArea = function (): string{
    return this.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
};
String.prototype.Trim = function (): string {
    return this.replace(/(^\s*)|(\s*$)/g, '');
}
String.prototype.PadLeft = function (Count: number, Char: string): string {
    var Value = '';
    if ((Count -= this.length) > 0) {
        for (Char = Char.charAt(0); Count; Char += Char, Count >>= 1) {
            if (Count & 1) Value += Char;
        }
    }
    return Value + this;
}
String.prototype.Left = function (Length: number): string {
    for (var Value = 0, Index = -1; ++Index != this.length && Length > 0;)	if ((Length -= this.charCodeAt(Index) > 0xff ? 2 : 1) >= 0)++Value;
    return this.substring(0, Value);
}
String.prototype.Right = function (Length: number): string {
    return this.length > Length ? this.substring(this.Length - Length) : this;
}
String.prototype.ToLower = function ():string {
    return this.substring(0, 1).toLowerCase() + this.substring(1);
}
String.prototype.Length = function ():number {
    for (var Value = this.length, Index = 0; Index - this.length;)	if (this.charCodeAt(Index++) > 0xff)++Value;
    return Value;
}
String.prototype.SplitInt = function (Split: string): number[] {
    var Value = this.Trim();
    return Value.length ? this.split(Split).ToArray(function (Value) { return parseInt(0 + Value); }) : [];
}
String.prototype.ParseDate = function (): Date {
    var Value = this.Trim();
    if (Value) {
        var DateValue = new Date(Value = Value.replace(/\-/g, '/'));
        if (!isNaN(DateValue.getTime())) return DateValue;
        Value = Value.replace(/[ :\/]+/g, ' ').split(' ');
        DateValue = new Date(Value[0], parseInt(Value[1]) - 1, Value[2], Value[3], Value[4], Value[5]);
        if (!isNaN(DateValue.getTime())) return DateValue;
    }
}
interface Date {
    AddMilliseconds(Value: number): Date;
    AddMinutes(Value: number): Date;
    AddHours(Value: number): Date;
    AddDays(Value: number): Date;
    ToString(Format: string, IsFixed: boolean): string;
    ToDateString(): string;
    ToTimeString(): string;
    ToMinuteString(): string;
    ToSecondString(): string;
    ToMinuteOrDateString(): string;
    ToInt(): number;
}
Date.prototype.AddMilliseconds = function (Value: number): Date {
    var NewDate = new Date;
    NewDate.setTime(this.getTime() + Value);
    return NewDate;
}
Date.prototype.AddMinutes = function (Value: number) {
    return this.AddMilliseconds(Value * 1000 * 60);
}
Date.prototype.AddHours = function (Value: number) {
    return this.AddMilliseconds(Value * 1000 * 60 * 60);
}
Date.prototype.AddDays = function (Value: number) {
    return this.AddMilliseconds(Value * 1000 * 60 * 60 * 24);
}
Date.prototype.ToString = function (Format: string, IsFixed = true): string {
    var Value: { [key: string]: number } = { y: this.getFullYear(), M: this.getMonth() + 1, d: this.getDate(), h: this.getHours(), m: this.getMinutes(), s: this.getSeconds(), S: this.getMilliseconds() };
    Format = Format.replace(/Y/g, 'y').replace(/D/g, 'd').replace(/H/g, 'h').replace(/W/g, 'w');
    if (IsFixed) {
        Format += Value[Format.charAt(Format.length - 1)] ? '.' : 'y';
        for (var Values = [], LastChar = '', LastIndex = 0, Index = -1; ++Index - Format.length; LastChar = Char) {
            var Char = Format.charAt(Index);
            if (Char != LastChar) {
                if (Value[LastChar] != null) {
                    Values.push(Value[LastChar].toString().Right(Index - LastIndex).PadLeft(Index - LastIndex, '0'));
                    LastIndex = Index;
                }
                else if (Value[Char] != null) Values.push(Format.substring(LastIndex, LastIndex = Index));
            }
        }
        Format = Values.join('');
    }
    else {
        for (var Name in Value) Format = Format.replace(new RegExp(Name, 'g'), Value[Name].toString());
    }
    return Format.replace(/w/g, ['日', '一', '二', '三', '四', '五', '六'][this.getDay()]);
};
Date.prototype.ToDateString = function (): string { return this.ToString('yyyy/MM/dd'); };
Date.prototype.ToTimeString = function (): string { return this.ToString('HH:mm:ss'); };
Date.prototype.ToMinuteString = function (): string { return this.ToString('yyyy/MM/dd HH:mm'); };
Date.prototype.ToSecondString = function (): string { return this.ToString('yyyy/MM/dd HH:mm:ss'); };
Date.prototype.ToMinuteOrDateString = function () { return this.ToInt() == new Date().ToInt() ? this.ToString('HH:mm') : this.ToDateString(); };
Date.prototype.ToInt = function (): number {
    return (this.getFullYear() << 9) + ((this.getMonth() + 1) << 5) + this.getDate();
}
interface Number {
    IntToDate(): Date;
    ToDisplay(): string;
    ToDisplayNone(): string;
    ToTrue(): boolean;
    ToFalse(): boolean;
}
Number.prototype.IntToDate = function (): Date { return new Date(this >> 9, ((this >> 5) & 15) - 1, this & 31); };
//Web视图style.display转换
Number.prototype.ToDisplay = function (): string { return this.toString() == '0' ? 'none' : ''; };
Number.prototype.ToDisplayNone = function (): string { return this.toString() == '0' ? '' : 'none'; };
//Web视图checked转换
Number.prototype.ToTrue = function (): boolean{ return this.toString() != '0'; };
Number.prototype.ToFalse = function (): boolean { return this.toString() == '0'; };
interface Boolean {
    ToDisplay(): string;
    ToDisplayNone(): string;
    ToTrue(): boolean;
    ToFalse(): boolean;
}
//Web视图style.display转换
Boolean.prototype.ToDisplay = function () { return this.toString() == 'true' ? '' : 'none'; };
Boolean.prototype.ToDisplayNone = function () { return this.toString() == 'true' ? 'none' : ''; };
//Web视图checked转换
Boolean.prototype.ToTrue = function () { return this.toString() == 'true'; };
Boolean.prototype.ToFalse = function () { return this.toString() != 'true'; };
module AutoCSer {
    interface IArray {
        [index: number]: any;
        length: number;
    }
    interface IModuleLoader {
        OnLoad: Function;
        Count: number;
        IsLoad: boolean;
        Paths: {};
    }
    interface IAjaxName {
        Names: IAjaxName[];
        Name: string;
        Index: number;
        ViewType: string;
    }
    export class Pub {
        static Alert;
        static Extends(Son: Function, Base: Function) {
            for (var Name in Base) if (Base.hasOwnProperty(Name)) Son[Name] = Base[Name];
            function Constructor() { this.constructor = Son; }
            Son.prototype = Base === null ? Object.create(Base) : (Constructor.prototype = Base.prototype, new Constructor());
        }
        static ToArray(Value: IArray, StartIndex = 0): any[] {
            for (var Values = []; StartIndex < Value.length; ++StartIndex) Values.push(Value[StartIndex]);
            return Values;
        }
        static Errors: { [key: string]: string; } = {}
        static SendError(Error: string) {
            if (!this.Errors[Error]) {
                this.Errors[Error] = Error;
                HttpRequest.Get('__PUBERROR__', { error: Error.length > 512 ? Error.substring(0, 512) + '...' : Error });
            }
        }
        static IsTryError = true;
        static ThisFunction<T extends Function>(This: any, Function: T, Arguments: any[] = null, IsArgument = true): T {
            if (Function) {
                var Value = function () {
                    if (Pub.IsTryError) {
                        try {
                            return Function.apply(This, IsArgument ? Pub.ToArray(arguments).concat(Arguments || []) : Arguments);
                        } catch (e) {
                            Pub.SendError((e ? (e.stack || e) .toString() : '未知错误') + '\r\n' + Function.toString());
                        }
                    }
                    return Function.apply(This, IsArgument ? Pub.ToArray(arguments).concat(Arguments || []) : Arguments);
                } as Object as T;
                Value['Test'] = Function;
                return Value;
            }
            Pub.SendError('Function is null\r\n' + window['caller']);
        }
        static ThisEvent(This: any, Function: Function, Arguments: any[] = null, Frame: Window = null): (Event: Event) => boolean {
            if (Function) {
                return function (Event: Event) {
                    var Browser = new BrowserEvent(Pub.IE ? Frame ? Frame.event || event : event : Event);
                    if (Pub.IsTryError) {
                        try {
                            Function.apply(This, Arguments ? [Browser].concat(Arguments || []) : [Browser]);
                        } catch (e) {
                            Pub.SendError((e ? (e.stack || e).toString() : '未知错误') + '\r\n' + Function.toString());
                        }
                    }
                    else Function.apply(This, Arguments ? [Browser].concat(Arguments || []) : [Browser]);
                    return Browser.Return;
                };
            }
            Pub.SendError('Event is null\r\n' + window['caller']);
        }
        static AjaxAppendJs: HttpRequestQueryInfo;
        static AjaxCallBack: Function;
        private static JsonLoopObjects: Object[] = [];
        static EvalJson(Text: string): any {
            var Value = eval(Text);
            Pub.JsonLoopObjects.length = 0;
            return Value;
        }
        static Copy(Left: Object, Right: Object): Object {
            for (var Name in Right) Left[Name] = Right[Name];
            return Left;
        }
        static SetJsonLoop(Index: number, Value: Object): Object {
            var CacheValue = this.JsonLoopObjects[Index];
            if (CacheValue) {
                if (CacheValue instanceof Array) CacheValue.push.apply(CacheValue, Value);
                else this.Copy(CacheValue, Value);
                return CacheValue;
            }
            return this.JsonLoopObjects[Index] = Value;
        }
        static GetJsonLoop(Index: number, Array: Object): Object{
            return this.JsonLoopObjects[Index] || (this.JsonLoopObjects[Index] = Array || {});
        }
        static AjaxName(Name: string, StartIndex: number): IAjaxName {
            for (var Values = { Names: [] } as IAjaxName, Index = StartIndex; Index - Name.length;) {
                var Code = Name.charCodeAt(Index);
                //[
                if (Code === 91) {
                    var Value = this.AjaxName(Name, Index + 1);
                    Value.Name = Name.substring(StartIndex, Index);
                    StartIndex = Index = Value.Index;
                    Values.Names.push(Value);
                }
                //]
                else if (Code === 93) {
                    var SubName = Name.substring(StartIndex, Index++);
                    if (SubName) Values.Names.push({ Name: SubName } as IAjaxName);
                    Values.Index = Index;
                    return Values;
                }
                //,
                else if (Code === 44) {
                    var SubName = Name.substring(StartIndex, Index++);
                    if (SubName) {
                        //@
                        if (SubName.charCodeAt(0) === 64) Values.ViewType = SubName.substring(1);
                        else Values.Names.push({ Name: SubName } as IAjaxName);
                    }
                    StartIndex = Index;
                }
                else ++Index;
            }
            if (StartIndex - Name.length) {
                var SubName = Name.substring(StartIndex);
                //@
                if (SubName.charCodeAt(0) === 64) Values.ViewType = SubName.substring(1);
                else Values.Names.push({ Name: SubName } as IAjaxName);
            }
            return Values;
        }
        static FormatAjaxs(Name: IAjaxName, Values: any[], StartIndex: number): any[] {
            for (var Value = [], Index = StartIndex - 1; ++Index - Values.length; Value.push(Values[Index] ? this.FormatAjax(Name, Values[Index]) : null));
            return Value;
        }
        static FormatAjax(Name: IAjaxName, Values: any[]): any {
            var Names = Name.Names;
            if (Names && Names.length) {
                if (Names[0].Name) {
                    for (var Value = {}, Index = Names.length; --Index >= 0;) {
                        Value[Names[Index].Name] = Values[Index] != null ? (Names[Index].Names && Names[Index].Names.length ? this.FormatAjax(Names[Index], Values[Index] as any[]) : Values[Index]) : null;
                    }
                    if (Name.ViewType) {
                        if (Name.ViewType.charAt(0) == '.') Value = eval(Name.ViewType.substring(1) + '.Get(Value)');
                        else Value = eval('new ' + Name.ViewType + '(Value)');
                    }
                    return Value;
                }
                else if (Values[0]) return this.FormatAjaxs(Names[0], Values[0], 0);
            }
            else return Values;
        }
        static AppendJs(Src: string, Charset = Loader.Charset, OnLoad: Function = null, OnError: Function = null): void {
            if (OnLoad || OnError) new LoadJs(Loader.CreateJavascipt(Src, Charset), OnLoad, OnError);
            else Loader.AppendJavaScript(Src, Charset);
        }
        static ToJson(Value, IsIgnore = false, IsNameQuery = true, IsSortName = true, Parents: any[] = null) {
            if (Value != null) {
                var Type = typeof (Value);
                if (Type != 'function' && (!IsIgnore || Value)) {
                    if (Type == 'string') return '"' + Value.toString().replace(/[\\"]/g, '\\$&').replace(/\n/g, '\\n') + '"';
                    if (Type == 'number' || Type == 'boolean') return Value.toString();
                    Type = Object.prototype.toString.apply(Value);
                    if (Type == '[object Date]') return 'new Date(' + Value.getTime() + ')';
                    if (!Parents) Parents = [];
                    for (var Index = 0; Index - Parents.length; ++Index) if (Parents[Index] == Value) return 'null';
                    if (typeof (Value.ToJson) == 'function') return Value.ToJson(IsIgnore, IsNameQuery, IsSortName, Parents);
                    Parents.push(Value);
                    var Values = [] as string[];
                    if (Type == '[object Array]') {
                        for (var Index = 0; Index - Value.length; ++Index)    Values.push(this.ToJson(Value[Index], IsIgnore, IsNameQuery, IsSortName, Parents));
                        Parents.pop();
                        return '[' + Values.join(',') + ']';
                    }
                    for (var Name in Value) {
                        if (Name != '$') {
                            var NextValue = Value[Name];
                            if (NextValue !== undefined) {
                                if ((!IsIgnore || NextValue) && typeof (NextValue) != 'function') {
                                    Values.push(IsSortName ? Name : ((IsNameQuery ? this.ToJson(Name.toString()) : Name.toString()) + ':' + this.ToJson(NextValue, IsIgnore, IsNameQuery, false, Parents)));
                                }
                            }
                        }
                    }
                    if (IsSortName && Values.length) {
                        Values.sort();
                        for (var Index = Values.length; Index;) {
                            var Name = Values[--Index];
                            Values[Index] = (IsNameQuery ? this.ToJson(Name.toString()) : Name.toString()) + ':' + this.ToJson(Value[Name], IsIgnore, IsNameQuery, true, Parents);
                        }
                    }
                    Parents.pop();
                    return '{' + Values.join(',') + '}';
                }
            }
            return 'null';
        }
        static ToQuery(Value, IsIgnore = false): string {
            var Values = [];
            for (var Name in Value) {
                if (Name != '$' && Value[Name] != null) {
                    var Type = typeof (Value[Name]);
                    if (Type != 'function' && (!IsIgnore || Value[Name])) Values.push(Name.Escape() + '=' + Value[Name].toString().Escape());
                }
            }
            return Values.join('&');
        }
        static Query: { [key: string]: any; };
        static GetLocationSearch(Location: Location): string {
            return (Location || location).search.toString().replace(/^\?/g, '');
        }
        static GetLocationHash(Location: Location): string {
            return (Location || location).hash.toString().replace(/^#(\!|\%21)?/g, '');
        }
        static FillQuery(Value: { [key: string]: string; }, Search: string, IsVersion: boolean) {
            var Query = Search.split('&'), Unescape = window['unescape'];
            if (Query.length == 1 && Search.indexOf('=') == -1) Value[''] = Unescape(Search);
            else {
                for (var Index = Query.length; --Index >= 0;) {
                    var KeyValue = Query.pop().split('='), key = Unescape(KeyValue[0]);
                    if (IsVersion || key != '__VERSIONNAME__') Value[key] = KeyValue.length < 2 ? '' : Unescape(KeyValue[1]);
                }
            }
        }
        static CreateQuery(Location: Location): { [key: string]: string; } {
            var Value: { [key: string]: string; } = {}, Search = this.GetLocationSearch(Location), Hash = this.GetLocationHash(Location);
            if (Hash.length) this.FillQuery(Value, Hash, true);
            if (Search.length) this.FillQuery(Value, Search, false);
            return Value;
        }
        private static IsLoad: boolean;
        private static OnLoads: Function[] = [];
        static OnLoadedHash: Events;
        static OnLoad(OnLoad: Function, This: Object = null, IsOnce = false) {
            if (This) OnLoad = this.ThisFunction(This, OnLoad);
            if (!IsOnce) this.OnLoadedHash.Add(OnLoad);
            if (this.IsLoad) OnLoad();
            else this.OnLoads.push(OnLoad);
        }
        static IsModules: { [key: string]: boolean } = {};
        static LoadModules: { [key: string]: IModuleLoader[] } = {};
        static LoadModule(Path: string) {
            if (this.IsModules[Path] == null) HttpRequest.Get('__PUBERROR__', { error: document.location.toString() + ' 加载了未知模块 ' + Path });
            else {
                this.IsModules[Path] = true;
                for (var Loads = this.LoadModules[Path], Index = Loads ? Loads.length : 0; --Index >= 0;) {
                    var Load = Loads[Index];
                    if (Load && Load.Paths[Path]) {
                        Load.Paths[Path] = 0;
                        if (--Load.Count == 0 && Load.OnLoad) {
                            if (Load.IsLoad) this.OnLoad(Load.OnLoad);
                            else Load.OnLoad();
                        }
                    }
                }
            }
        }
        static ModuleVersions: { [key: string]: string } = { 'htmlEditor': '4', 'ace/ace': '3', 'highcharts/highcharts': '2', 'mathJax/MathJax': '6' };
        static OnModule(Paths: string[], OnLoad: Function, IsLoad: boolean = true, Version: string = Loader.Version) {
            for (var Index = Paths.length, Load = { IsLoad: IsLoad, OnLoad: OnLoad, Count: 0, Paths: {} }; Index;) {
                var Path = Paths[--Index];
                if (!this.IsModules[Path]) {
                    ++Load.Count;
                    var Loads = this.LoadModules[Path];
                    if (!Loads) this.LoadModules[Path] = Loads = [];
                    Load.Paths[Path] = 1;
                    Loads.push(Load);
                    this.LoadModuleWhenNull(Path, Version);
                }
            }
            if (!Load.Count && OnLoad) {
                if (IsLoad) this.OnLoad(OnLoad);
                else OnLoad();
            }
        }
        private static LoadModuleWhenNull(Path: string, Version: string) {
            if (this.IsModules[Path] == null) {
                this.IsModules[Path] = false;
                this.AppendJs(Loader.JsDomain + 'Js/' + Path + '.js?__VERSIONNAME__=' + (this.ModuleVersions[Path]||Version));
            }
        }
        static ClientView: Object = {};
        static LoadView(View: PageView, IsReView: boolean) {
            if (!View) (View = new PageView).ErrorPath = '__VIEWLOCATION__';
            if ((View as Object as IHttpRequestReturn).ErrorRequest) {
                this.PageView.IsLoadView = this.PageView.IsLoad = this.PageView.IsView = this.PageView.LoadError = true;
                this.ReadyState();
                return;
            }
            if (View.ErrorPath) {
                var HashIndex = View.ErrorPath.indexOf('#');
                if (HashIndex + 1) {
                    var Hash = View.ErrorPath.substring(HashIndex);
                    View.ErrorPath = View.ErrorPath.substring(0, HashIndex);
                    location.replace(View.ErrorPath + (View.ErrorPath.indexOf('?') + 1 ? '&' : '?') + 'url=' + (View.ReturnPath || location.toString()).Escape() + Hash);
                }
                else location.replace(View.ErrorPath + '#url=' + (View.ReturnPath || location.toString()).Escape());
            }
            else if (View.LocationPath) location.replace(View.LocationPath);
            View.Client = this.ClientView;
            View.Query = this.Query;
            if (!IsReView) {
                View.OnShowed = this.PageView.OnShowed;
                View.OnSet = this.PageView.OnSet;
                View.IsLoadView = View.IsLoad = View.IsView = true;
                this.PageView = View;
                this.ReadyState();
            }
        }
        static ReLoad() {
            var ViewOver = document.getElementById('__VIEWOVERID__');
            if (ViewOver) ViewOver.innerHTML = '正在尝试重新加载视图数据...';
            var Query = this.CreateLoadViewQuery();
            if (!Query.IsVersion) Query.IsRandom = true;
            HttpRequest.GetQuery(Query);
        }
        static LoadHashScrollTop = true;
        static LoadHash(PageView: PageView) {
            if (!(PageView as Object as IHttpRequestReturn).ErrorRequest) {
                this.OnBeforeUnLoad.Function();
                var Data = Skin.Body.Data;
                for (var Name in PageView) {
                    var Value = PageView[Name];
                    if (Value == null || (!Value.__VIEWONLY__ && typeof (Value) != 'function')) Data[Name] = Value;
                }
                this.OnLoadHash.Function(Data);
                this.LoadView(Data as PageView, true);
                Skin.Body.Show(Data);
                Skin.ChangeHeader();
                this.OnLoadedHash.Function();
                if (this.LoadHashScrollTop) HtmlElement.$SetScrollTop(0);
            }
        }
        static OnQueryEvents;
        private static CheckHashFunction;
        static CheckHash(Event: HashChangeEvent): any {
            var Hash = HtmlElement.$Hash();
            if (Hash !== this.LocationHash) {
                this.LocationHash = Hash;
                this.Query = Pub.CreateQuery(location);
                this.ReView();
                Pub.OnQueryEvents.Function();
            }
            if (Pub.IE) setTimeout(this.CheckHashFunction, 100);
        }
        static ReView(IsReView: boolean = false) {
            var Query = new HttpRequestQuery(document.location.pathname + (IsReView ? '&__REVIEW__=':''), this.Query, this.ThisFunction(this, this.LoadHash));
            Query.IsRandom = true;
            HttpRequest.GetQuery(Query);
        }
        private static LoadComplete: boolean;
        private static ReadyFunction: Function;
        static ReadyState() {
            var View = this.PageView, IsLoad = document.body && (document.readyState == null || document.readyState.toLowerCase() === 'complete');
            if (IsLoad && !this.LoadComplete) {
                HtmlElement.$('@body').To();
                Skin.Create(View);
                this.DeleteElements = HtmlElement.$Create('div').Styles('padding,margin', '10px').Style('border', '10px solid red').Opacity(0).To();
                this.IsBorder = this.DeleteElements.XY0().Left - 10;
                this.DeleteElements.Styles('padding,margin,border', '0px');
                this.IsBorder -= this.DeleteElements.XY0().Left;
                if (this.IsBorder === 20) this.IsPadding = true;
                this.IsFixed = Pub.IE ? this.DeleteElements.Style('position', 'fixed').Style('left', '50%').Element0().offsetLeft : 1;
                this.DeleteElements.Display(0);
                this.LoadComplete = true;
            }
            if (IsLoad && View.IsLoad) {
                if (View.IsLoadView) {
                    if (View.LoadError) {
                        View.IsLoad = View.IsLoadView = View.LoadError = false;
                        var ViewOver = document.getElementById('__VIEWOVERID__');
                        if (ViewOver) ViewOver.innerHTML = '错误：视图数据加载失败，稍后尝试重新加载';
                        document.title = 'Server Error';
                        setTimeout(this.ThisFunction(this, this.ReLoad), 2000);
                        return;
                    }
                    Skin.Body.Show(View);
                    Skin.ChangeHeader();
                }
                else {
                    document.body.innerHTML = document.body.innerHTML.replace(/ @(src|style)=/gi, ' $1=');
                    var ViewOver = document.getElementById('__VIEWOVERID__');
                    if (ViewOver) document.body.removeChild(ViewOver);
                }
                this.OnReadyState();
                this.IsLoad = true;
                for (var Index = -1; ++Index - this.OnLoads.length; this.OnLoads[Index]());
                this.OnLoads = this.ReadyFunction = null;
            }
            else setTimeout(this.ReadyFunction, 1);
        }
        static LocationHash: string;
        static DeleteElements: HtmlElement;
        static FocusEvents: Events;
        static IsBorder: number;
        static IsFixed: number;
        static IsPadding: boolean;
        static OnReadyState() {
            this.LocationHash = HtmlElement.$Hash();
            HtmlElement.$(document.body).AddEvent('focus', this.FocusEvents = new Events());
            if (!Loader.Version||Loader.LoadScript) {
                var Path = document.location.pathname, Index = Path.lastIndexOf('/'), EndIndex = Path.lastIndexOf('.');
                if (EndIndex > Index) Path = Path.substring(0, EndIndex);
                if (Path.charCodeAt(Path.length - 1) === 47) Path += 'index';
                this.AppendJs(Loader.JsDomain + Path.substring(1) + '.js?__VERSIONNAME__=' + Loader.Version);
            }
            if (Pub.IE) setTimeout(this.CheckHashFunction = this.ThisFunction(this, this.CheckHash), 100);
            else window.onhashchange = this.ThisFunction(this, this.CheckHash);
        }
        static CreateLoadViewQuery(): HttpRequestQuery {
            return new HttpRequestQuery(document.location.pathname, this.Query, this.ThisFunction(this, this.LoadView), Loader.ViewVersion != null);
        }
        static OnLoadHash: Events;
        static OnBeforeUnLoad: Events;
        static PageView: PageView;
        static IE: boolean;
        static LoadIE():void {
            Pub.IE = !arguments.length || navigator.appName == 'Microsoft Internet Explorer';
            HttpRequest.Load();
            Pub.Load();
        }
        private static Load() {
            this.OnLoadHash = new Events();
            this.OnLoadedHash = new Events();
            this.OnBeforeUnLoad = new Events();
            window.onbeforeunload = function (Event: BeforeUnloadEvent) { Pub.OnBeforeUnLoad.Function(Event); }
            this.Query = this.CreateQuery(self.location);
            this.PageView = new PageView();
            this.ReadyFunction = this.ThisFunction(this, this.ReadyState);
            if (Loader.PageView) {
                this.PageView.IsLoadView = true;
                this.PageView.OnShowed = new Events();
                this.PageView.OnSet = new Events();
                HttpRequest.GetQuery(this.CreateLoadViewQuery());
            }
            else this.PageView.IsLoad = true;
            if (!Loader.PageView) this.ReadyState();
        }
        static Functions: { [key: string]: FunctionConstructor } = {};
        static GetParameter(Value, DefaultParameter, Parameter = null) {
            if (Parameter) {
                for (var Name in DefaultParameter) {
                    var ParameterValue = Parameter[Name];
                    Value[Name] = ParameterValue == null ? DefaultParameter[Name] : ParameterValue;
                }
            }
            else {
                for (var Name in DefaultParameter) Value[Name] = DefaultParameter[Name];
            }
        }
        static GetEvents(Value, DefaultEvents, Parameter = null) {
            if (Parameter) {
                for (var Name in DefaultEvents) {
                    var Function = Parameter[Name] as Function, Event = new Events();
                    if (Function) Event.Add(Function);
                    Value[Name] = Event;
                }
            }
            else {
                for (var Name in DefaultEvents) Value[Name] = new Events();
            }
        }
        static Identity = 0;
        static LoadViewType(Type: any, Name = 'Id') {
            Type.Views = {};
            Type.Get = function (Value: Object, IsGetOnly: boolean): Object {
                if (IsGetOnly) return Type.Views[Value[Name] || Value];
                var Id = Value[Name];
                if (Id) {
                    var ViewValue = Type.Views[Id];
                    if (ViewValue) {
                        var Values = [];
                        Pub.CopyView(ViewValue, Value, Values);
                        if (ViewValue.OnCopyView) ViewValue.OnCopyView();
                        return ViewValue;
                    }
                    return Type.Views[Id] = new Type(Value);
                }
                return new Type(Value);
            };
        }
        private static CopyView(Left: Object, Right: Object, Values: any[]): Object{
            if (Left === Right) return Left;
            if (Values.IndexOfValue(Right) < 0) {
                Values.push(Right);
                for (var Name in Right) {
                    if (Name != '__VIEWONLY__') {
                        if (Left[Name] == null || Right[Name] == null || typeof (Left[Name]) != 'object') Left[Name] = Right[Name];
                        else this.CopyView(Left[Name], Right[Name], Values);
                    }
                }
                Values.pop();
            }
            return Left;
        }
        static ViewFlagEnum(EnumString: string, Enum: { [key: string]: number }): Object {
            var Value = {}, IntValue = parseInt(EnumString);
            if (isNaN(IntValue)) {
                IntValue = 0;
                if (EnumString) for (var Values = EnumString.split(','), Index = Values.length; Index; IntValue |= Enum[Values[--Index].Trim()] || 0);
            }
            for (var Name in Enum) {
                var EnumValue = Enum[Name];
                if (typeof (EnumValue) == 'number') Value[Name] = (IntValue & EnumValue) == EnumValue ? EnumValue : 0;
            }
            return Value;
        }
        static DisableZoom() {
            if (navigator.appVersion && navigator.appVersion.indexOf('MicroMessenger') + 1) {
                var Meta = document.createElement('meta');
                Meta.name = 'viewport';
                Meta.content = 'width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;';
                Loader.DocumentHead.appendChild(Meta);
            }
        }
        private static DefaultCallAjaxPost(Url: string, Send: Object = null, Callback: Function = null)
        {
            HttpRequest.Post(Url, Send, Callback);
        }
        static CallAjaxPost: (Url: string, Send: Object, Callback: Function) => void;
        static GetAjaxPost(): (Url: string, Send: Object, Callback: Function) => void {
            return this.CallAjaxPost || this.DefaultCallAjaxPost;
        }
        private static DefaultCallAjaxGet(Url: string, Send: Object = null, Callback: Function = null, IsVersion = false) {
            AutoCSer.HttpRequest.Get(Url, Send, Callback, IsVersion);
        }
        static CallAjaxGet: (Url: string, Send: Object, Callback: Function, IsVersion: boolean) => void;
        static GetAjaxGet(): (Url: string, Send: Object, Callback: Function, IsVersion: boolean) => void {
            return this.CallAjaxGet || this.DefaultCallAjaxGet;
        }
        static GetHtmlEditor(Element: HTMLElement): HTMLElement {
            return HtmlElement.$ElementName(Element, 'htmleditor');
        }
    }
    Pub.Alert = Pub.ThisFunction(window, alert);
    export interface IIndexPool {
        PoolIndex: number;
        PoolIdentity: number;
    }
    class IndexPoolNode {
        Identity: number;
        Value: IIndexPool;
        Set(Value: IIndexPool) {
            (this.Value = Value).PoolIdentity = ++this.Identity;
        }
        Pop(Value: IIndexPool): boolean {
            if (Value.PoolIdentity === this.Identity) {
                this.Value = null;
                Value.PoolIdentity = 0;
                return true;
            }
            return false;
        }
        Get(Identity: number): IIndexPool {
            return Identity === this.Identity ? this.Value : null;
        }
    }
    export class IndexPool {
        static Nodes: IndexPoolNode[] = [];
        static Indexs: number[] = [];
        static Push(Value: IIndexPool) {
            if (this.Indexs.length) this.Nodes[Value.PoolIndex = this.Indexs.pop()].Set(Value);
            else {
                var Node = new IndexPoolNode();
                Node.Identity = 0;
                Node.Set(Value);
                Value.PoolIndex = this.Nodes.length;
                this.Nodes.push(Node);
            }
        }
        static Pop(Value: IIndexPool) {
            this.Nodes[Value.PoolIndex]
            var Node = this.Nodes[Value.PoolIndex];
            if (Node && Node.Pop(Value)) this.Indexs.push(Value.PoolIndex);
        }
        static Get(Index: number, Identity: number): IIndexPool {
            var Node = this.Nodes[Index];
            return Node ? Node.Get(Identity) : null;
        }
        static ToString(Value: IIndexPool) {
            return 'AutoCSer.IndexPool.Get(' + Value.PoolIndex + ',' + Value.PoolIdentity + ')';
        }
    }
    export class LoadJs {
        private OnLoad: Function;
        private OnError: Function;
        private Script: HTMLScriptElement;
        private LoadFunction: (Event: Event) => any;
        private ErrorFunction: (Event: Event) => any;
        constructor(Script: HTMLScriptElement, OnLoad: Function = null, OnError: Function = null) {
            this.OnLoad = OnLoad;
            this.OnError = OnError;
            this.LoadFunction = Pub.ThisFunction(this, this.OnLoadJs) as (Event: Event) => any;
            this.ErrorFunction = Pub.ThisFunction(this, this.OnErrorJs) as (Event: Event) => any;
            (this.Script = Script).onload = this.LoadFunction;
            Script.onerror = this.ErrorFunction;
            Loader.DocumentHead.appendChild(Script);
        }
        private OnLoadJs(Event: Event) {
            if (this.OnLoad) this.OnLoad(Event);
            Loader.DocumentHead.removeChild(this.Script);
        }
        private OnErrorJs(Event: Event) {
            if (this.OnError) this.OnError(Event);
            Loader.DocumentHead.removeChild(this.Script);
        }
    }
    export class Events {
        private OnAdd: () => boolean;
        private Functions: Function[];
        Function: Function;
        constructor(OnAdd: () => boolean = null, This: any = null) {
            this.OnAdd = This ? Pub.ThisFunction(This, OnAdd) as () => boolean : OnAdd;
            this.Functions = [];
            this.Function = Pub.ThisFunction(this, this.Call);
        }
        private Call() {
            for (var Argument = Pub.ToArray(arguments), Index = 0; Index - this.Functions.length; this.Functions[Index++].apply(null, Argument));
        }
        Add(Function: Function): Events {
            if (Function && this.Functions.IndexOfValue(Function) < 0) {
                this.Functions.push(Function);
                if (this.OnAdd && this.OnAdd()) Function();
            }
            return this;
        }
        AddEvent(Event: Events): Events {
            if (Event) for (var Index = 0, Functions = Event.Functions; Index - Functions.length; this.Add(Functions[Index++]));
            return this;
        }
        Remove(Function: Function): Events {
            if (Function) this.Functions.RemoveAt(this.Functions.IndexOfValue(Function));
            return this;
        }
        RemoveEvent(Event: Events): Events {
            if (Event) for (var Index = 0, Functions = Event.Get(); Index - Functions.length; this.Remove(Functions[Index++]));
            return this;
        }
        Clear(): Events {
            this.Functions.length = 0;
            return this;
        }
        Get(): Function[] {
            return this.Functions;
        }
    }
    Pub.OnQueryEvents = new Events();
    export class HttpRequestQuery {
        CallBack: Function;
        Url: string;
        FormData: FormData;
        Send: any;
        Method: string;
        UserName: string;
        Password: string;
        IsRandom: boolean;
        IsVersion: boolean;
        IsOnLoad: boolean;
        ErrorRequest: HttpRequest;
        constructor(Url: string, Send: Object = null, CallBack: Function = null, IsVersion = false) {
            this.Url = Url;
            this.Send = Send;
            this.CallBack = CallBack;
            this.IsVersion = IsVersion;
        }
        ToQueryInfo(): HttpRequestQueryInfo {
            var Query = new HttpRequestQueryInfo(null);
            Query.CallBack = this.CallBack || HttpRequestQuery.NullCallBack;
            Query.Url = this.Url;
            Query.FormData = this.FormData;
            Query.Send = this.Send;
            Query.Method = this.Method;
            Query.UserName = this.UserName;
            Query.Password = this.Password;
            Query.IsRandom = this.IsRandom;
            Query.IsVersion = this.IsVersion;
            Query.IsOnLoad = this.IsOnLoad;
            return Query;
        }
        GetOnError(HttpRequest: HttpRequest) {
            if (this.CallBack || HttpRequest) {
                this.ErrorRequest = HttpRequest;
                return Pub.ThisFunction(this, this.OnError);
            }
            return null;
        }
        private OnError(Value: IHttpRequestReturn) {
            if (this.ErrorRequest != null) {
                this.ErrorRequest.NextRequest();
                this.ErrorRequest = null;
            }
            if (this.CallBack != null) {
                Value.ErrorRequest = this;
                this.CallBack(Value);
            }
        }
        private static NullCallBack() { }
    }
    class HttpRequestQueryInfo extends HttpRequestQuery implements IIndexPool {
        PoolIndex: number;
        PoolIdentity: number;
        SendString: string;
        Header: { [key: string]: string; };
        RetryCount: number;
        Request: XMLHttpRequest;
    }
    export class HttpRequest {
        private Requesting: boolean;
        private WriteOrder: number;
        private ReadOrder: number;
        private Queue: HttpRequestQueryInfo[];
        private OnResponse: Events;
        private ReadXMLHttpRequest: XMLHttpRequest;
        private OnReadyStateChangeFunction: (Event: ProgressEvent) => any;
        constructor(OnResponse: Function = null) {
            this.Requesting = false;
            this.WriteOrder = 0;
            this.ReadOrder = -1;
            this.Queue = [];
            this.OnResponse = new Events().Add(OnResponse);
            this.OnReadyStateChangeFunction = Pub.ThisFunction(this, this.OnReadyStateChange) as (Event: ProgressEvent) => any;
        }
        Request(Request: HttpRequestQueryInfo) {
            if (Request.Send && !Request.FormData) {
                Request.SendString = Pub.ToJson(Request.Send);
                if (Request.SendString === '{}') Request.SendString = '';
            }
            this.Queue[this.WriteOrder++] = Request;
            if (!this.Requesting) {
                this.Requesting = true;
                this.MakeXMLHttpRequest();
            }
        }
        CallBack() {
            var Request = this.Queue[this.ReadOrder];
            this.NextRequest();
            if (Request.CallBack) {
                if (Request.IsOnLoad) Pub.OnLoad(Pub.ThisFunction(this, this.OnLoad, [Request.CallBack, Pub.ToArray(arguments)]), null, true);
                else Request.CallBack.apply(null, Pub.ToArray(arguments));
            }
        }
        private OnLoad(CallBack: Function, Arguments: any[]) {
            CallBack.apply(null, Arguments);
        }
        private OnReadyStateChange(Event: ProgressEvent): any {
            var Request = this.ReadXMLHttpRequest;
            if (Request.readyState == 4) {
                var Query = this.Queue[this.ReadOrder], IsError = true;
                try {
                    if (Request.status == 200 || Request.status == 304) {
                        var Text = Request.responseText;
                        this.OnResponse.Function(Text);
                        if (Query.CallBack) {
                            Pub.EvalJson(Text);
                            IsError = false;
                        }
                        else {
                            this.NextRequest();
                            Pub.EvalJson(Text);
                            IsError = false;
                        }
                    }
                    else {
                        this.NextRequest();
                        if (Query.Url.substring(0, HttpRequest.ErrorPath.length) !== HttpRequest.ErrorPath) {
                            HttpRequest.Post('__PUBERROR__', { error: '服务器请求失败 : ' + location.toString() + '\r\n' + Query.Url + (Query.SendString && !Query.FormData ? ('\r\n' + Query.SendString.length + '\r\n' + Query.SendString.substring(0, 256)) : '') });
                        }
                    }
                }
                finally {
                    IndexPool.Pop(Query);
                    if (IsError && Query.CallBack) Query.CallBack({ __AJAXRETURN__: null, ErrorEvent: null, ErrorRequest: Query } as IHttpRequestReturn);
                }
            }
        }
        NextRequest() {
            if (this.ReadOrder === this.WriteOrder - 1) {
                this.WriteOrder = 0;
                this.ReadOrder = -1;
                this.Requesting = false;
            }
            else this.MakeXMLHttpRequest();
        }
        private MakeXMLHttpRequest() {
            var Request = this.ReadXMLHttpRequest = HttpRequest.CreateRequest(), Info = this.Queue[++this.ReadOrder];
            var Url = Info.Url;
            if (Info.Method == null || Info.FormData) Info.Method = 'POST';
            if (Info.SendString && !Info.FormData) {
                if (Info.Method === 'GET') {
                    Url += (Url.indexOf('?') + 1 ? '&' : '?') + '__JSON__=' + Info.SendString.Escape();
                    Info.SendString = null;
                }
                else Info.SendString = Info.SendString.replace(/\xA0/g, ' ');
            }
            if (Info.IsRandom) Url += (Url.indexOf('?') + 1 ? '&' : '?') + 't=' + (new Date).getTime();
            else if (Info.IsVersion && Info.Method === 'GET') Url += (Url.indexOf('?') + 1 ? '&' : '?') + '__VERSIONNAME__=' + AutoCSer.Loader.Version;
            Info.Request = Request;
            if (!Pub.IE && Info.Method === 'GET' && !Info.UserName && !Info.IsOnLoad) {
                Info.RetryCount = 2;
                Pub.AppendJs(Url, Loader.Charset, null, (Pub.AjaxAppendJs = Info).GetOnError(this));
            }
            else {
                Request.onreadystatechange = this.OnReadyStateChangeFunction;
                if (Info.UserName == null || Info.UserName === '') Request.open(Info.Method, Url, true);
                else Request.open(Info.Method, Url, true, Info.UserName, Info.Password);
                if (Info.Header) {
                    for (var Name in Info.Header) Request.setRequestHeader(Name, Info.Header[Name]);
                }
                else if (Info.Method === 'POST' && !Info.FormData) Request.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
                Request.send(Info.FormData || Info.SendString);
            }
        }
        private static MicrosoftXmlHttps: string[];
        static CreateRequest(): XMLHttpRequest {
            if (Pub.IE) {
                for (var Index = HttpRequest.MicrosoftXmlHttps.length; Index;) {
                    try {
                        return new ActiveXObject(HttpRequest.MicrosoftXmlHttps[--Index]);
                    }
                    catch (e) { }
                }
            }
            else if (window['XMLHttpRequest']) return new XMLHttpRequest;
            AutoCSer.Pub.Alert('你的浏览器不支持服务器请求,请升级您的浏览器！');
            return null;
        }
        static PostQuery(HttpRequestQuery: HttpRequestQuery) {
            HttpRequestQuery.Method = 'POST';
            var Query = HttpRequestQuery.ToQueryInfo(), Request = new HttpRequest;
            IndexPool.Push(Query);
            Query.Url = '__AJAX__?__AJAXCALL__=' + Query.Url + '&__CALLBACK__=' + IndexPool.ToString(Query).Escape() + '.CallBack';
            Request.Request(Query);
        }
        static Post(Url: string, Send: Object = null, CallBack: Function = null) {
            this.PostQuery(new HttpRequestQuery(Url, Send, CallBack));
        }
        static GetQuery(HttpRequestQuery: HttpRequestQuery) {
            HttpRequestQuery.Method = 'GET';
            if (!Pub.IE && !HttpRequestQuery.IsRandom) {
                var Query = HttpRequestQuery.ToQueryInfo();
                Query.IsRandom = false;
                Query.Url = '__AJAX__?__AJAXCALL__=' + Query.Url + '&__CALLBACK__=AutoCSer.Pub.AjaxCallBack';
                this.AjaxGetRequest.Request(Query);
                return;
            }
            var Query = HttpRequestQuery.ToQueryInfo();
            IndexPool.Push(Query);
            if (!Query.IsVersion) Query.IsRandom = true;
            Query.Url = '__AJAX__?__AJAXCALL__=' + Query.Url + '&__CALLBACK__=' + IndexPool.ToString(Query).Escape() + '.CallBack';
            (new HttpRequest).Request(Query);
        }
        static Get(Url: string, Send: Object = null, CallBack: Function = null, IsVersion = false) {
            this.GetQuery(new HttpRequestQuery(Url, Send, CallBack, IsVersion));
        }
        static AjaxGetRequest: HttpRequest;
        private static ErrorPath = '__AJAX__?__AJAXCALL__=__PUBERROR__&';
        static CheckError(Value: Object, ErrorInfo = '服务器请求失败，请稍后重试'): boolean {
            if ((Value as IHttpRequestReturn).ErrorRequest) {
                if (ErrorInfo) AutoCSer.Pub.Alert(ErrorInfo);
                return false;
            }
            return true;
        }
        static Load() {
            //['Microsoft.XMLHTTP','MSXML2.XMLHTTP','MSXML2.XMLHTTP.3.0','MSXML2.XMLHTTP.4.0','MSXML2.XMLHTTP.5.0']
            if (Pub.IE) this.MicrosoftXmlHttps = ['Microsoft.XMLHTTP', 'Msxml2.XMLHTTP'];
            Pub.AjaxCallBack = Pub.ThisFunction(this.AjaxGetRequest = new HttpRequest, this.AjaxGetRequest.CallBack);
        }
    }
    export interface IHttpRequestReturn {
        __AJAXRETURN__: any;
        ErrorEvent: Event;
        ErrorRequest: HttpRequestQuery;
    }
    export interface IPointer {
        Left: number;
        Top: number;
    }
    export class HtmlElement {
        private FilterString: string;
        private Elements: HTMLElement[];
        private Parent: HTMLElement;
        private Filter: (Parent: HTMLElement, HtmlElement: HtmlElement) => void;
        private FilterIndex: number;
        private FilterCreator: { [key: number]: Function; };
        private FilterBuilder: string[];
        constructor(Value: any, Parent: any) {
            if (typeof (Value) == 'string') {
                this.FilterString = Value;
                this.Parent = Parent ? (Parent instanceof HtmlElement ? (Parent as HtmlElement).Element0() : Parent as HTMLElement) : document.body;
            }
            else if (Value) {
                if (Value instanceof HtmlElement) {
                    this.FilterString = Value.FilterString;
                    this.Parent = Value.Parent;
                    this.Elements = Value.Elements;
                }
                else this.Elements = Value instanceof Array ? Value as HTMLElement[] : [Value as HTMLElement];
            }
            else this.Elements = [];
        }
        IsParent(Element: HTMLElement): boolean {
            return !this.Parent || (Element && HtmlElement.$IsParent(Element, this.Parent));
        }
        FilterId() {
            var Id = this.FilterValue();
            this.FilterBuilder.push('function(Element,Value){if(Element==this.Parent?Value.IsParent(Element=document.getElementById("');
            this.FilterBuilder.push(Id);
            this.FilterBuilder.push('")):Element.id=="');
            this.FilterBuilder.push(Id);
            this.FilterBuilder.push('")(');
            this.FilterNext(true);
        }
        FilterChildTag() {
            if (this.FilterIndex === this.FilterString.length || this.FilterString.charCodeAt(this.FilterIndex) - 47) {
                var Name = this.FilterValue();
                this.FilterBuilder.push('function(Element,Value){for(var Elements=Element.childNodes,Index=0;Index-Elements.length;)if((Element=Elements[Index++])');
                if (Name) {
                    this.FilterBuilder.push('.tagName&&Element.tagName.toLowerCase()=="');
                    this.FilterBuilder.push(Name.toLowerCase());
                    this.FilterBuilder.push('"');
                }
                this.FilterBuilder.push(')(');
                this.FilterNext(true);
            }
            else {
                ++this.FilterIndex;
                this.FilterTag();
            }
        }
        FilterTag() {
            var Name = this.FilterValue();
            if (Name) this.FilterChildren('tagName', Name);
            else {
                this.FilterChildren();
                this.FilterBuilder.push('if(Element=Childs[Index++])(');
                this.FilterNext(true);
            }
        }
        FilterChildren(Name: string = null, Value = '') {
            this.FilterBuilder.push('function(Element,Value){var Elements=[],ElementIndex=-1;while(ElementIndex-Elements.length)for(var Childs=ElementIndex+1?Elements[ElementIndex++].childNodes:[arguments[++ElementIndex]],Index=0;Index-Childs.length;Elements.push(Element))');
            if (Name) {
                if (!Value) Value = this.FilterValue();
                this.FilterBuilder.push('if((Element=Childs[Index++]).');
                this.FilterBuilder.push(Name);
                this.FilterBuilder.push('&&Element.');
                this.FilterBuilder.push(Name);
                this.FilterBuilder.push('.toLowerCase()');
                this.FilterBuilder.push('=="');

                this.FilterBuilder.push(Value.toLowerCase());
                this.FilterBuilder.push('")(');
                this.FilterNext(true);
            }
        }
        FilterClass() {
            this.FilterChildren();
            this.FilterBuilder.push('if((Element=Childs[Index++]).className&&Element.className.toString().split(" ").IndexOfValue("');
            this.FilterBuilder.push(this.FilterValue());
            this.FilterBuilder.push('")+1)(');
            this.FilterNext(true);
        }
        FilterAttribute() {
            this.FilterChildren();
            var Value = this.FilterValue().split('=');
            if (Value.length == 1) {
                this.FilterBuilder.push('if(AutoCSer.HtmlElement.$IsAttribute(Element=Childs[Index++],"');
                this.FilterBuilder.push(Value[0]);
                this.FilterBuilder.push('"))(');
            }
            else {
                this.FilterBuilder.push('if(AutoCSer.HtmlElement.$Attribute(Element=Childs[Index++],"');
                this.FilterBuilder.push(Value[0]);
                this.FilterBuilder.push('")=="');
                this.FilterBuilder.push(Value[1]);
                this.FilterBuilder.push('")(');
            }
            this.FilterNext(true);
        }
        FilterName() {
            this.FilterChildren();
            this.FilterBuilder.push('if(AutoCSer.HtmlElement.$Attribute(Element=Childs[Index++],"name")=="');
            this.FilterBuilder.push(this.FilterValue());
            this.FilterBuilder.push('")(');
            this.FilterNext(true);
        }
        FilterCss() {
            this.FilterChildren();
            var Value = this.FilterValue().split('=');
            this.FilterBuilder.push('if(AutoCSer.HtmlElement.$GetStyle(Element=Childs[Index++],"');
            this.FilterBuilder.push(Value[0]);
            this.FilterBuilder.push('")=="');
            this.FilterBuilder.push(Value[1]);
            this.FilterBuilder.push('")(');
            this.FilterNext(true);
        }
        FilterValue(): string {
            var Index = this.FilterIndex;
            while (this.FilterIndex !== this.FilterString.length && !this.FilterCreator[this.FilterString.charCodeAt(this.FilterIndex)])++this.FilterIndex;
            return this.FilterString.substring(Index, this.FilterIndex);
        }
        FilterNext(IsEnd: boolean) {
            if (this.FilterIndex !== this.FilterString.length) {
                var Creator = this.FilterCreator[this.FilterString.charCodeAt(this.FilterIndex)];
                if (Creator) {
                    ++this.FilterIndex;
                    Creator();
                }
                else this.FilterTag();
                if (IsEnd) this.FilterBuilder.push(')(Element,Value);}');
            }
            else if (this.FilterBuilder.length) this.FilterBuilder.push('Value.Elements.push)(Element);}');
        }
        private static FilterCache: { [key: string]: (Parent: HTMLElement, HtmlElement: HtmlElement) => void; } = {};
        GetElements() {
            if (this.Elements) return this.Elements;
            if (!this.Filter) {
                var Filter = this.FilterString ? HtmlElement.FilterCache[this.FilterString] : HtmlElement.NullFilter;
                if (!Filter) {
                    this.FilterIndex = 0;
                    //                         #id                                        *name                                        .className                                    /tagName                                         :type                                                      ?css                                        @value
                    this.FilterCreator = { 35: Pub.ThisFunction(this, this.FilterId), 42: Pub.ThisFunction(this, this.FilterName), 46: Pub.ThisFunction(this, this.FilterClass), 47: Pub.ThisFunction(this, this.FilterChildTag), 58: Pub.ThisFunction(this, this.FilterChildren, ['type']), 63: Pub.ThisFunction(this, this.FilterCss), 64: Pub.ThisFunction(this, this.FilterAttribute) };
                    this.FilterBuilder = [];
                    this.FilterNext(false);
                    eval('Filter=' + this.FilterBuilder.join('') + ';');
                    HtmlElement.FilterCache[this.FilterString] = Filter;
                    this.FilterBuilder = this.FilterCreator = null;
                }
                this.Filter = Filter;
            }
            this.Elements = [];
            this.Filter(this.Parent, this);
            return this.Elements;
        }
        static NullFilter(Parent: HTMLElement, HtmlElement: HtmlElement) { }

        Element0(): HTMLElement {
            return this.GetElements()[0];
        }
        AddEvent(Name: string, Value: any, IsStop: boolean = !Pub.IE): HtmlElement {
            return this.Event(Name, Value, IsStop, '$AddEvent');
        }
        DeleteEvent(Name: string, Value: any, IsStop: boolean = !Pub.IE): HtmlElement {
            return this.Event(Name, Value, IsStop, '$DeleteEvent');
        }
        private Event(Name: string, Value: any, IsStop: boolean, CallName: string) {
            var Elements = this.GetElements();
            if (Elements.length) {
                var Names = Name.split(',');
                for (var Index = 0; Index - Elements.length; HtmlElement[CallName](Elements[Index++], Names, Value, IsStop));
            }
            return this;
        }
        Value0(): string {
            var Elements = this.GetElements();
            return Elements.length ? HtmlElement.$GetValue(Elements[0]) : null;
        }
        Value(Value: string): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetValue(Elements[Index++], Value));
            return this;
        }

        Get0<T>(Name: string, Value: T = null): any {
            var Elements = this.GetElements();
            return Elements.length ? Elements[0][Name] : Value;
        }
        Id0(): string {
            return this.Get0<string>('id');
        }
        Html0(): string {
            return this.Get0<string>('innerHTML');
        }
        TagName0(): string {
            return this.Get0<string>('tagName');
        }
        ScrollHeight0() {
            return this.Get0('scrollHeight', 0);
        }
        Attribute0(Name: string): string {
            var Elements = this.GetElements();
            if (Elements.length) return HtmlElement.$Attribute(Elements[0], Name);
        }
        Name0(): string {
            return this.Attribute0('name');
        }
        private GetCall0<T>(CallName: string): T {
            var Elements = this.GetElements();
            if (Elements.length) return HtmlElement[CallName](Elements[0]);
        }
        Text0(): string {
            return this.GetCall0<string>('$GetText');
        }
        Text(Text: string): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetText(Elements[Index++], Text));
            return this;
        }
        Css0(): CSSStyleDeclaration {
            return this.GetCall0<CSSStyleDeclaration>('$Css');
        }
        Width0(): number {
            return this.GetCall0<number>('$Width');
        }
        Height0(): number {
            return this.GetCall0<number>('$Height');
        }
        Opacity0(): number {
            return this.GetCall0<number>('$GetOpacity');
        }
        XY0(): IPointer {
            return this.GetCall0<IPointer>('$XY');
        }
        Style0(Name: string): string {
            var Css = this.Css0();
            return Css ? Css[Name] : null;
        }
        Parent0(): HtmlElement {
            var Elements = this.GetElements();
            return Elements.length ? HtmlElement.$(HtmlElement.$ParentElement(Elements[0])) : null;
        }
        Next0(): HtmlElement {
            var Elements = this.GetElements();
            return Elements.length ? HtmlElement.$(HtmlElement.$NextElement(Elements[0])) : null;
        }
        Previous0(): HtmlElement {
            var Elements = this.GetElements();
            return Elements.length ? HtmlElement.$(HtmlElement.$PreviousElement(Elements[0])) : null;
        }
        Replace0(Element: HTMLElement): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) HtmlElement.$ParentElement(Elements[0]).replaceChild(Element, Elements[0]);
            return this;
        }

        Set(Name: string, Value: any): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; Elements[Index++][Name] = Value);
            return this;
        }
        Html(Html: string, IsToHtml = false): HtmlElement {
            return this.Set('innerHTML', IsToHtml ? Html.ToHTML() : Html);
        }

        To(Parent: any = document.body): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) {
                if (Parent instanceof HtmlElement) Parent = (Parent as HtmlElement).Element0();
                for (var Index = -1; ++Index - Elements.length;) {
                    if (HtmlElement.$ParentElement(Elements[Index]) != Parent) Parent.appendChild(Elements[Index]);
                }
            }
            return this;
        }
        Child(): HtmlElement {
            for (var Nodes: Node[] = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; ++Index) {
                var Childs = Elements[Index].childNodes;
                if (Childs) for (var NodeIndex = 0; NodeIndex !== Childs.length; Nodes.push(Childs[NodeIndex++]));
            }
            return new HtmlElement(Nodes, null);
        }
        Childs(IsChild: (HTMLElement) => boolean = null) {
            for (var Value = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; ++Index) {
                var Values = HtmlElement.$ChildElements(Elements[Index], IsChild);
                if (Values) Value.push(Values)
            }
            return HtmlElement.$(Value.concat.apply([], Value));
        }
        InsertBefore(Element: HTMLElement, Parent: HTMLElement = null): HtmlElement {
            if (!Parent) Parent = HtmlElement.$ParentElement(Element);
            for (var Elements = this.GetElements(), Index = 0; Index !== Elements.length; Parent.insertBefore(Elements[Index++], Element));
            return this;
        }
        Delete(): HtmlElement {
            for (var Elements = this.GetElements(), Index = 0; Index !== Elements.length; HtmlElement.$Delete(Elements[Index++]));
            return this;
        }
        AddClass(Name: string): HtmlElement {
            return this.Class(Name, '$AddClass');
        }
        RemoveClass(Name: string): HtmlElement {
            return this.Class(Name, '$RemoveClass');
        }
        private Class(Name: string, CallName: string): HtmlElement {
            if (Name) {
                var Elements = this.GetElements();
                if (Elements.length) {
                    for (var Index = 0, Names = Name.split(' '); Index !== Elements.length; HtmlElement[CallName](Elements[Index++], Names));
                }
            }
            return this;
        }
        Style(Name: string, Value: any): HtmlElement {
            for (var Elements = this.GetElements(), Index = Elements.length; Index; Elements[--Index].style[Name] = Value);
            return this;
        }
        Styles(Name: string, Value: string): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) {
                for (var ElementIndex = 0, Names = Name.split(','); ElementIndex - Elements.length;) {
                    for (var Element = Elements[ElementIndex++], Index = 0; Index - Names.length; Element.style[Names[Index++]] = Value);
                }
            }
            return this;
        }
        Display(IsShow: any): HtmlElement {
            return this.Style('display', typeof (IsShow) == 'string' ? IsShow : (IsShow ? '' : 'none'));
        }
        Disabled(Value: boolean) {
            return this.Style('disabled', Value);
        }
        Left(Value: number): HtmlElement {
            return this.Style('left', Value + 'px');
        }
        Top(Value: number): HtmlElement {
            return this.Style('top', Value + 'px');
        }
        ToXY(Left: number, Top: number) {
            var Elements = this.GetElements();
            if (Elements.length) {
                for (var Index = Elements.length; Index; HtmlElement.$ToXY(Elements[--Index], Left, Top));
            }
            return this;
        }
        Cursor(Value: string) {
            return this.Style('cursor', Value);
        }
        Opacity(Value: number): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetOpacity(Elements[Index++], Value));
            return this;
        }
        FirstElement(IsValue: (HTMLElement) => boolean): HTMLElement {
            return this.GetElements().First(IsValue);
        }
        CssArray(Name: string): any[] {
            for (var Value = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; Value.push(HtmlElement.$Css(Elements[Index++])[Name]));
            return Value;
        }
        TopIndex(): HtmlElement{
            if (this.CssArray('zIndex').MaxValue() != HtmlElement.ZIndex && this.Elements.length) this.Style('zIndex', ++HtmlElement.ZIndex);
            return this;
        }
        Focus0(): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) Elements[0].focus();
            return this;
        }
        Blur0(): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) Elements[0].blur();
            return this;
        }
        ChangeBool(Name: string): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; ++Index)	Elements[Index][Name] = !Elements[Index][Name];
            return this;
        }
        ScrollTop0(): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) HtmlElement.$SetScrollTop(HtmlElement.$XY(Elements[0]).Top);
            return this;
        }

        static $(Value: any, Parent: any = null): HtmlElement {
            return new HtmlElement(Value, Parent);
        }
        static $Id(Id: string): HtmlElement {
            return this.$(document.getElementById(Id));
        }
        static $IdElement(Id: string): HTMLElement {
            return document.getElementById(Id);
        }
        static $AddEvent(Element: HTMLElement, Names: string[], Value: any, IsStop = !Pub.IE) {
            var IsBody = Pub.IE && (Element == document.body || (Element.tagName && Element.tagName.toLowerCase() == 'body'));
            this.$DeleteEvent(Element, Names, Value, IsStop);
            for (var Index = Names.length; --Index >= 0;) {
                var Name = Names[Index].toLowerCase();
                if (Pub.IE) {
                    if (Name.substring(0, 2) != 'on') Name = 'on' + Name;
                    if (IsBody && Name == 'onfocus') Name = 'onfocusin';
                    Element['attachEvent'](Name, Value);
                }
                else {
                    if (Name.substring(0, 2) === 'on') Name = Name.substring(2);
                    Element.addEventListener(Name, Value, IsStop);
                }
            }
        }
        static $DeleteEvent(Element: HTMLElement, Names: string[], Value: any, IsStop = !Pub.IE) {
            var IsBody = Pub.IE && (Element == document.body || (Element.tagName && Element.tagName.toLowerCase() == 'body'));
            for (var Index = Names.length; --Index >= 0;) {
                var Name = Names[Index].toLowerCase();
                if (Pub.IE) {
                    if (Name.substring(0, 2) != 'on') Name = 'on' + Name;
                    if (IsBody && Name == 'onfocus') Name = 'onfocusin';
                    Element['detachEvent'](Name, Value);
                }
                else {
                    if (Name.substring(0, 2) === 'on') Name = Name.substring(2);
                    Element.removeEventListener(Name, Value, IsStop);
                }
            }
        }
        static $IsParent(Element: HTMLElement, Parent: HTMLElement): boolean {
            while (Element != Parent && Element) Element = (Pub.IE ? Element.parentElement || Element.parentNode : Element.parentNode) as HTMLElement;
            return Element != null;
        }
        static $ParentElement(Element: HTMLElement): HTMLElement {
            return (Pub.IE ? Element.parentElement || Element.parentNode : Element.parentNode) as HTMLElement;
        }
        static $NextElement(Element: HTMLElement): HTMLElement {
            return Element ? Element.nextSibling as HTMLElement : null;
        }
        static $PreviousElement(Element: HTMLElement): HTMLElement {
            return Element ? Element.previousSibling as HTMLElement : null;
        }
        static $Delete(Element: HTMLElement, Parent: HTMLElement = null) {
            if (Element != null) (Parent || HtmlElement.$ParentElement(Element)).removeChild(Element);
        }
        static $ChildElements(Element: HTMLElement, IsElement: (HTMLElement) => boolean = null): HTMLElement[] {
            var Value = [], Elements = [Element], ElementIndex = 0;
            while (ElementIndex < Elements.length) {
                for (var Childs = Elements[ElementIndex++].childNodes, Index = -1; ++Index - Childs.length;) {
                    if (IsElement == null || IsElement(Childs[Index])) Value.push(Childs[Index]);
                    Elements.push(Childs[Index] as HTMLElement);
                }
            }
            return Value;
        }
        static $Create(TagName: string, Document: Document = document): HtmlElement {
            return this.$(Document.createElement(TagName));
        }
        static $CreateElement(TagName: string, Document: Document = document): HTMLElement {
            return Document.createElement(TagName);
        }
        static $GetValueById(Id: string): string {
            return this.$GetValue(this.$IdElement(Id));
        }
        static $GetValue(Element: HTMLElement): string {
            if (Element) {
                if (Element.tagName.toLowerCase() === 'select') {
                    if ((Element as HTMLSelectElement).selectedIndex >= 0) return ((Element as HTMLSelectElement).options[(Element as HTMLSelectElement).selectedIndex] as HTMLInputElement).value;
                    return null;
                }
                return (Element as Object as HTMLInputElement).value;
            }
        }
        static $SetValue(Element: HTMLElement, Value: string){
            if (Element) {
                if (Element.tagName.toLowerCase() == 'select') {
                    for (var Index = (Element as HTMLSelectElement).options.length; Index;) {
                        if (((Element as HTMLSelectElement).options[--Index] as HTMLInputElement).value == Value) break;
                    }
                    (Element as HTMLSelectElement).selectedIndex = Index;
                }
                else (Element as HTMLInputElement).value = Value;
            }
        }
        static $SetValueById(Id: string, Value: string): HTMLElement{
            var Element = this.$IdElement(Id);
            this.$SetValue(Element, Value);
            return Element;
        }
        static $IntById(Id: string, DefaultValue: number = null): number {
            var Value = this.$GetValueById(Id);
            return Value ? parseInt(Value, 10) : (DefaultValue || 0);
        }
        static $FloatById(Id: string, DefaultValue: number = null): number {
            var Value = this.$GetValueById(Id);
            return Value ? parseFloat(Value) : (DefaultValue || 0);
        }
        static $GetCheckedById(Id: string): boolean {
            var Element = this.$IdElement(Id);
            return Element && Element['checked'];
        }
        static $SetCheckedById(Id: string, Checked: boolean): HTMLElement {
            var Element = this.$IdElement(Id);
            if (Element) Element['checked'] = Checked;
            return Element;
        }
        static $GetText(Element: HTMLElement): string {
            return Pub.IE ? Element.nodeType == 3 ? Element.nodeValue : Element.innerText : Element.textContent;
        }
        static $SetText(Element: HTMLElement, Text: string): HTMLElement {
            if (Pub.IE) {
                if (Element.nodeType == 3) Element.nodeValue = Text;
                else Element.innerText = Text;
            }
            else Element.textContent = Text;
            return Element;
        }
        static $AddClass(Element: HTMLElement, Names: string[]) {
            if (Names) {
                if (Element.classList) {
                    for (var Index = Names.length; Index ; Element.classList.add(Names[--Index]));
                }
                else {
                    var OldName = Element.className;
                    if (OldName) {
                        for (var Index = Names.length, OldNames = OldName.split(' '); --Index >= 0;)	if (OldNames.IndexOfValue(Names[Index]) < 0) OldNames.push(Names[Index]);
                        Names = OldNames;
                    }
                    Element.className = Names.join(' ');
                }
            }
        }
        static $RemoveClass(Element: HTMLElement, Names: string[]) {
            if (Names) {
                if (Element.classList) {
                    for (var Index = Names.length; Index; Element.classList.remove(Names[--Index]));
                }
                else {
                    var OldName = Element.className;
                    if (OldName) {
                        for (var Index = Names.length, OldNames = OldName.split(' '); Index; OldNames.RemoveAtEnd(OldNames.IndexOfValue(Names[--Index])));
                        Element.className = OldNames.length ? OldNames.join(' ') : '';
                    }
                }
            }
        }
        static $Css(Element: HTMLElement): CSSStyleDeclaration {
            return Pub.IE ? Element['currentStyle'] : document.defaultView.getComputedStyle(Element);
        }
        static $GetStyle(Element: HTMLElement, Name: string): string {
            var Css = this.$Css(Element);
            return Css ? Css[Name] : null;
        }
        static $SetStyle(Element: HTMLElement, Name: string, Value: any) {
            Element.style[Name] = Value;
        }
        static $AttributeOrStyle(Element: HTMLElement, Name: string ): string {
            return this.$Attribute(Element, Name) || this.$GetStyle(Element, Name);
        }
        static $Attribute(Element: HTMLElement, Name: string): string {
            var Value = Element[Name];
            return Value == undefined && Element.attributes && (Value = Element.attributes[Name]) ? Value.value : Value;
        }
        static $IsAttribute(Element: HTMLElement, Name: string): boolean {
            return Element[Name] !== undefined || (Element.attributes != null && Element.attributes[Name] !== undefined);
        }
        static $ElementName(Element: HTMLElement, Name: string, Value: string = null): HTMLElement {
            if (Value == null) {
                while (Element && HtmlElement.$Attribute(Element, Name) == null) Element = HtmlElement.$ParentElement(Element);
            }
            else while (Element && HtmlElement.$Attribute(Element, Name) != Value) Element = HtmlElement.$ParentElement(Element);
            return Element;
        }
        static $Transform_matrix(a, b, c, d, Left: number, Top: number): IPointer {
            return { Left: Left, Top: Top };
        }
        static $XY(Element: HTMLElement): IPointer {
            for (var Left = 0, Top = 0; Element != null && Element != document.body; Element = Element.offsetParent as HTMLElement) {
                Left += Element.offsetLeft;
                Top += Element.offsetTop;
                if (Pub.IsFixed) {
                    var Css = this.$Css(Element), Transform = Css['transform'] || Css['-webkit-transform'];
                    if (Css['position'] == 'fixed') {
                        Left += this.$GetScrollLeft();
                        Top += this.$GetScrollTop();
                    }
                    var XY = this.$Transform(Transform);
                    if (XY) {
                        if (XY.Left) Left += XY.Left;
                        if (XY.Top) Top += XY.Top;
                    }
                }
                if (Pub.IsBorder) {
                    var Css = this.$Css(Element);
                    Left -= parseInt(0 + Css['border-left-width'], 10);
                    Top -= parseInt(0 + Css['border-top-width'], 10);
                    if (Pub.IsPadding) {
                        Left -= parseInt(0 + Css['padding-left'], 10);
                        Top -= parseInt(0 + Css['padding-top'], 10);
                    }
                }
            }
            return { Left: Left, Top: Top};
        }
        private static $Transform(Transform: string): IPointer {
            if(Transform && Transform.indexOf('matrix(') != -1) return eval('HtmlElement.$Transform_' + Transform) as IPointer;
        }
        static $ToXY(Element: HTMLElement, Left: number, Top: number): HTMLElement{
            var Value = this.$XY(Element['offsetParent'] as HTMLElement);
            Element.style.left = (Left - Value.Left) + 'px';
            Element.style.top = (Top - Value.Top) + 'px';
            return Element;
        }
        static $Width(Element: HTMLElement = null): number {
            if (Element == null) return window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
            return this.$Offset(Element, 'offsetWidth');
        }
        static $Height(Element: HTMLElement = null): number {
            if (Element == null) return window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
            return this.$Offset(Element, 'offsetHeight');
        }
        private static $Offset(Element: HTMLElement, Name: string): number {
            while (Element) {
                var Value = Element[Name];
                if (Value != null) return Value;
                var Elements = Element.children;
                if (Elements == null) return 0;
                Element = Elements[0] as HTMLElement;
            }
            return 0;
        }
        static $GetScrollLeft(): number {
            return Math.max(document.body.scrollLeft, document.documentElement.scrollLeft);
        }
        static $SetScrollLeft(Left: number) {
            return document.body.scrollLeft = document.documentElement.scrollLeft = Left;
        }
        static $GetScrollTop(): number {
            return Math.max(document.body.scrollTop, document.documentElement.scrollTop);
        }
        static $SetScrollTop(Top: number) {
            document.body.scrollTop = document.documentElement.scrollTop = Top;
        }
        static $GetScrollHeight(): number {
            return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
        }
        static $GetScrollWidth(): number {
            return Math.max(document.body.scrollWidth, document.documentElement.scrollWidth);
        }
        static $ScrollTopById(Id: string) {
            var Element = this.$IdElement(Id);
            if (Element) this.$SetScrollTop(this.$XY(Element).Top);
        }
        static $SetOpacity(Element: HTMLElement, Value: number) {
            if (Pub.IE) Element.style.filter = 'alpha(opacity=' + Value + ')';
            else Element.style.opacity = Element.style['MozOpacity'] = (Value / 100).toString();
        }
        static $GetOpacity(Element: HTMLElement): number {
            if (Pub.IE) return Element.style.filter['alphas'].opacity;
            var Value = this.$Css(Element).opacity;
            return Value ? parseFloat(Value) * 100 : null;
        }
        static $Name(Value: string, Element: HTMLElement = null): HtmlElement {
            if (Pub.IE) return HtmlElement.$(HtmlElement.$ChildElements(Element || document.body, function (Element) { return HtmlElement.$Attribute(Element, 'name') == Value; }));
            return HtmlElement.$(Element ? HtmlElement.$ChildElements(Element, function (Element) { return HtmlElement.$Attribute(Element, 'name') == Value; }) : Pub.ToArray(document.getElementsByName(Value)));
        }
        static $Hash(Location: Location = location): string {
            return Location.hash.toString().replace(/^#(\!|\%21)?/g, '');
        }
        static $Paste(Element: HTMLTextAreaElement, Text: string, IsAll: boolean) {
            if (Pub.IE) {
                var Selection = Element['document'].selection.createRange() as TextRange;
                if (IsAll && Selection.text == '') Element.value = Text;
                else {
                    Selection.text = Text;
                    Selection.moveStart('character', 0);
                    Selection.select();
                }
            }
            else {
                var StartIndex = Element.selectionStart, EndIndex = Element.selectionEnd;
                if (IsAll && StartIndex == EndIndex) {
                    Element.value = Text;
                    StartIndex = 0;
                }
                else {
                    var OldText = Element.value;
                    Element.value = OldText.substring(0, StartIndex) + Text + OldText.substring(EndIndex);
                }
                Element.selectionStart = Element.selectionEnd = StartIndex + Text.length;
            }
        }
        static $SelectOption(Id: string): HTMLOptionElement {
            var Element = this.$IdElement(Id) as HTMLSelectElement;
            if (Element && Element.selectedIndex >= 0) return Element.options[Element.selectedIndex] as Object as HTMLOptionElement;
        }

        static OverZIndex = 100;
        static ZIndex = 10000;
    }
    enum SkinReShowType {
        None,
        ReShow,
        PushArray,
        PushArrayExpand,
        RemoveArray,
    }
    class SkinViewNode {
        Parent: SkinViewNode;
        Node: SkinNode;
        Views: SkinView[];
        LoopNodes: SkinViewNode[];
        SearchNodes: SkinViewNode[];
        IsSetSearchNode: boolean;
        IsLoopValue: boolean;
        IsExpired: boolean;
        NoMark: boolean;
        Identity: number;
        SkinNoMark: number;
        SkinNoOutput: number;
        ReShowType: SkinReShowType;
        constructor(Node: SkinNode, Parent: SkinViewNode, Data: SkinData = null) {
            this.Parent = Parent;
            this.Node = Node;
            this.ReShowType = SkinReShowType.None;
            if (Data) this.SetView(Data);
        }
        CreateView() {
            var Expressions = this.Node.Expressions;
            if (Expressions) {
                this.Views = [];
                for (var Index = 0; Index !== Expressions.length; this.Views.push(Expressions[Index++].CreateView(this)));
            }
            else this.SetView(this.Node.Skin.Datas[this.Node.Skin.Datas.length - 1]);
        }
        SetView(Data: SkinData) {
            this.Views = [new SkinView([Data])];
            this.SetData(Data);
        }
        SetData(Data: SkinData) {
            if (Data.$Nodes) Data.$Nodes.push(this);
            else Data.$Nodes = [this];
        }
        Create() {
            this.SkinNoMark = this.Node.Skin.NoMark;
            this.SkinNoOutput = this.Node.Skin.NoOutput;
            switch (this.Node.TypeIndex) {
                case 0:
                    this.Mark(true);
                    this.CreateNodes();
                    this.Mark(false);
                    break;
                case 1: this.CreateIf(); break;
                case 2: this.CreateNot(); break;
                case 3: this.CreateLoop(); break;
                case 4: this.CreateValue(); break;
                case 5: this.CreateNoMark(); break;
                case 6: SkinViewNode.CreateHtml(this.Node); break;
                case 7: this.CreateAt(); break;
            }
        }
        GetMarkId(IsStart: boolean) {
            return '_' + this.Identity + (IsStart ? '_MARKSTART_' : '_MARKEND_');
        }
        static DeleteMark(Span: HTMLElement) {
            var Id = Span.id;
            if (Id && Id.length > 10) {
                if (Id.substring(Id.length - 9) == '_MARKEND_' || Id.substring(Id.length - 11) == '_MARKSTART_') HtmlElement.$Delete(Span);
            }
        }
        Mark(IsStart: boolean) {
            var Skin = this.Node.Skin;
            if ((Skin.NoOutput | Skin.NoMark) === 0 && !this.NoMark) {
                if (!this.Identity) this.Identity = ++SkinViewNode.NodeIdentity;
                Skin.Htmls.push('<span id="' + this.GetMarkId(IsStart) + '" style="display:none"></span>');
            }
        }
        CreateNodes() {
            var Nodes = this.Node.Nodes;
            if (Nodes) for (var Index = 0; Index !== Nodes.length; ++Index) {
                var Node = Nodes[Index];
                if (Node.TypeIndex == 6) SkinViewNode.CreateHtml(Node);
                else new SkinViewNode(Node, this).Create();
            }
        }
        CreateIf() {
            this.CreateView();
            this.CreateLogic(this.CheckLogic());
        }
        CreateNot() {
            this.CreateView();
            this.CreateLogic(!this.CheckLogic());
        }
        CreateLogic(IsOutput: boolean) {
            this.Mark(true);
            if (!IsOutput) ++this.Node.Skin.NoOutput;
            this.CreateNodes();
            if (!IsOutput) --this.Node.Skin.NoOutput;
            this.Mark(false);
        }
        CheckLogic(): boolean {
            for (var Index = this.Views.length; Index;) {
                var Expression = this.Node.GetExpression(--Index), Value = this.Views[Index].CheckLogic(Expression);
                if (Expression.IsNot ? !Value : Value) {
                    if (this.Node.IsOrExpression) return true;
                }
                else if (!this.Node.IsOrExpression) return false;
            }
            return !this.Node.IsOrExpression;
        }
        CreateLoop() {
            if (this.IsLoopValue) {
                this.SkinNoMark = this.Node.Skin.NoMark;
                this.SkinNoOutput = this.Node.Skin.NoOutput;
                this.Mark(true);
                var Data = this.Views[0].GetData();
                if (Data && Data.$Data != null) {
                    this.Node.Skin.Datas.push(Data);
                    this.CreateNodes();
                    this.Node.Skin.Datas.pop();
                }
            }
            else {
                this.CreateView();
                this.Mark(true);
                if (this.LoopNodes) this.LoopNodes.length = 0;
                else this.LoopNodes = [];
                this.CreateLoopOnly();
            }
            this.Mark(false);
        }
        private CreateLoopOnly() {
            var Data = this.Views[0].GetData();
            if (Data) {
                var Value = Data.$Data as any[];
                if (Value instanceof Array && Value.length) {
                    this.Node.Skin.Datas.push(Data);
                    if (!this.LoopNodes) this.LoopNodes = [];
                    for (var Index = this.LoopNodes.length; Index < Value.length; ++Index) {
                        var Node = new SkinViewNode(this.Node, this, Data.$Get(Index));
                        Node.IsLoopValue = true;
                        this.LoopNodes.push(Node);
                        Node.CreateLoop();
                    }
                    this.Node.Skin.Datas.pop();
                }
            }
        }
        private CreateLoopExpand() {
            var Data = this.Views[0].GetData();
            if (Data) {
                var Value = Data.$Data as any[];
                if (Value instanceof Array && Value.length) {
                    this.Node.Skin.Datas.push(Data);
                    if (!this.LoopNodes) this.LoopNodes = [];
                    for (var Index = 0; Index != Value.length; ++Index) {
                        if (this.LoopNodes[Index]) break;
                        var Node = new SkinViewNode(this.Node, this, Data.$Get(Index));
                        Node.IsLoopValue = true;
                        this.LoopNodes[Index] = Node;
                        Node.CreateLoop();
                    }
                    this.Node.Skin.Datas.pop();
                }
            }
        }
        CreateValue() {
            this.CreateView();
            this.Mark(true);
            var Data = this.Views[0].GetData();
            if (Data && Data.$Data != null) {
                this.Node.Skin.Datas.push(Data);
                this.CreateNodes();
                this.Node.Skin.Datas.pop();
            }
            this.Mark(false);
        }
        CreateNoMark() {
            ++this.Node.Skin.NoMark;
            this.CreateNodes();
            --this.Node.Skin.NoMark;
        }
        static CreateHtml(Node: SkinNode) {
            if (Node.Skin.NoOutput === 0) Node.Skin.Htmls.push(Node.Html);
        }
        CreateAt() {
            this.CreateView();
            if (this.Node.Skin.NoOutput === 0) {
                if (this.Node.IsIdentity) {
                    if (!this.Identity) this.Identity = ++SkinViewNode.NodeIdentity;
                    this.Node.Skin.Htmls.push(this.Identity.toString());
                    if (this.Parent != null && !this.IsSetSearchNode) this.Parent.SetSearchNode(this);
                }
                else if (this.Node.IsLoopIndex) this.Node.Skin.Htmls.push(this.GetLoopIndex().toString());
                else {
                    if (!this.Node.NoMarkAt) this.Mark(true);
                    var Data = this.Views[0].GetData();
                    if (Data && Data.$Data != null) {
                        var Value = Data.$Data.toString();
                        this.Node.Skin.Htmls.push(this.Node.IsHtml ? Value.ToHTML() : (this.Node.IsTextArea ? Value.ToTextArea() : Value));
                    }
                    if (!this.Node.NoMarkAt) this.Mark(false);
                }
            }
        }
        private GetLoopIndex(): number {
            if (this.Node.TypeIndex == 3 && this.IsLoopValue) {
                var Data = this.Views[0].GetData();
                return Data ? Data.$Name as number : -1;
            }
            return this.Parent ? this.Parent.GetLoopIndex() : -1; 
        }
        private SetSearchNode(Node: SkinViewNode) {
            Node.IsSetSearchNode = true;
            if (this.SearchNodes) this.SearchNodes.push(Node);
            else this.SearchNodes = [Node];
            if (this.Parent != null && !this.IsSetSearchNode) this.Parent.SetSearchNode(this);
        }
        SearchNode(Identity: number): SkinViewNode {
            if (this.Identity == Identity) return this;
            if (this.SearchNodes) {
                for (var Index = this.SearchNodes.length; Index;) {
                    var Node = this.SearchNodes[--Index].SearchNode(Identity);
                    if (Node) return Node;
                }
            }
        }
        ClearSearchNode(IsStart: boolean) {
            if (!IsStart) this.IsSetSearchNode = false;
            if (this.SearchNodes) {
                for (var Index = this.SearchNodes.length; Index; this.SearchNodes[--Index].ClearSearchNode(false));
                this.SearchNodes.length = 0;
            }
        }
        TryShow(Type: SkinReShowType) {
            if (!this.SkinNoOutput) {
                if (this.Identity) {
                    if (this.ReShowType == SkinReShowType.None) {
                        this.ReShowType = Type;
                        SkinViewNode.ReShowNodes.push(this);
                        if (!SkinViewNode.IsReShowTask) {
                            SkinViewNode.IsReShowTask = true;
                            setTimeout(SkinViewNode.ReShowTask, 0);
                        }
                    }
                    else if (this.ReShowType != Type) this.ReShowType = SkinReShowType.ReShow;
                }
                else if (this.Parent) this.Parent.TryShow(SkinReShowType.ReShow);
            }
        }
        ReShow(): Skin {
            var MarkStart = HtmlElement.$IdElement(this.GetMarkId(true)), ReShowType = this.ReShowType;
            this.ReShowType = SkinReShowType.None;
            if (MarkStart) {
                var MarkEndId = this.GetMarkId(false), MarkEnd = HtmlElement.$IdElement(MarkEndId);
                if (MarkEnd) {
                    for (var Parent = this.Parent, Parents: SkinViewNode[] = []; Parent != null; Parent = Parent.Parent) {
                        if (Parent.Node.TypeIndex == 3 || Parent.Node.TypeIndex == 4) Parents.push(Parent);
                    }
                    for (var Index = Parents.length, Datas = this.Node.Skin.ResetNode(this); Index; Datas.push(Parents[--Index].Views[0].GetData()));
                    switch (ReShowType) {
                        case SkinReShowType.ReShow: this.ReShowOnly(MarkStart, MarkEnd, MarkEndId); break;
                        case SkinReShowType.PushArray: this.ReShowPushArray(MarkStart, MarkEnd); break;
                        case SkinReShowType.PushArrayExpand: this.ReShowPushArrayExpand(MarkStart); break;
                        case SkinReShowType.RemoveArray: this.ReShowRemoveArray(MarkStart, MarkEnd, MarkEndId); break;
                    }
                    return this.Node.Skin;
                }
            }
            this.IsExpired = true;
            return null;
        }
        RemoveLoop(Index: number, WriteIndex: number) {
            if (this.LoopNodes) this.LoopNodes[WriteIndex] = this.LoopNodes[Index];
        }
        SetRemoveLoopLength(Length: number) {
            if (this.LoopNodes && this.LoopNodes.length > Length) this.LoopNodes.length = Length;
        }
        SetPushLoopLength(Length: number) {
            if (this.LoopNodes && this.LoopNodes.length < Length) this.LoopNodes.length = Length;
        }
        ResetLoop(Index: number) {
            if (this.LoopNodes) this.LoopNodes[Index] = null;
        }
        private ReShowOnly(MarkStart: HTMLElement, MarkEnd: HTMLElement, MarkEndId: string) {
            this.NoMark = true;
            this.ClearSearchNode(true);
            this.Create();
            this.NoMark = false;
            for (var MarkParent = HtmlElement.$ParentElement(MarkStart), Element = HtmlElement.$NextElement(MarkStart); Element && Element.id != MarkEndId; Element = HtmlElement.$NextElement(MarkStart))	HtmlElement.$Delete(Element, MarkParent);
            Pub.DeleteElements.Html(this.Node.Skin.EndHtml()).Child().InsertBefore(MarkEnd, MarkParent);
        }
        private ReShowPushArray(MarkStart: HTMLElement, MarkEnd: HTMLElement) {
            this.CreateView();
            this.CreateLoopOnly();
            Pub.DeleteElements.Html(this.Node.Skin.EndHtml()).Child().InsertBefore(MarkEnd, HtmlElement.$ParentElement(MarkStart));
        }
        private ReShowPushArrayExpand(MarkStart: HTMLElement) {
            this.CreateView();
            this.CreateLoopExpand();
            Pub.DeleteElements.Html(this.Node.Skin.EndHtml()).Child().InsertBefore(AutoCSer.HtmlElement.$NextElement(MarkStart), HtmlElement.$ParentElement(MarkStart));
        }
        private ReShowRemoveArray(MarkStart: HTMLElement, MarkEnd: HTMLElement, MarkEndId: string) {
            this.ClearSearchNode(true);
            for (var MarkParent = HtmlElement.$ParentElement(MarkStart), Element = HtmlElement.$NextElement(MarkStart); Element && Element.id != MarkEndId; Element = HtmlElement.$NextElement(MarkStart))	HtmlElement.$Delete(Element, MarkParent);
            HtmlElement.$Delete(MarkStart, MarkParent);
            HtmlElement.$Delete(MarkEnd, MarkParent);
        }
        static NodeIdentity: number = 0;
        static ReShowNodes: SkinViewNode[] = [];
        static IsReShowTask: boolean;
        static ReShowTask() {
            for (var NodeHash: { [key: number]: SkinViewNode; } = {}, Index = SkinViewNode.ReShowNodes.length; Index;) {
                var Node = SkinViewNode.ReShowNodes[--Index];
                if (Node.ReShowType != SkinReShowType.None) {
                    if (Node.SkinNoOutput) Node.ReShowType = SkinReShowType.None;
                    else NodeHash[Node.Identity] = Node;
                }
            }
            for (var Nodes: SkinViewNode[] = [], Index = SkinViewNode.ReShowNodes.length; Index;) {
                var Node = SkinViewNode.ReShowNodes[--Index];
                if (Node.ReShowType != SkinReShowType.None) {
                    for (var Parent = Node.Parent; Parent != null; Parent = Parent.Parent) {
                        var ParentNode = NodeHash[Parent.Identity];
                        if (ParentNode) {
                            if (ParentNode === SkinViewNode.NullNode) Parent = null;
                            break;
                        }
                        NodeHash[Parent.Identity] = SkinViewNode.NullNode;
                    }
                    if (Parent == null) Nodes.push(Node);
                    else Node.ReShowType = SkinReShowType.None;
                }
            }
            SkinViewNode.ReShowNodes = [];
            SkinViewNode.IsReShowTask = false;
            for (var SkinArray: Skin[] = [], Skins: { [key: number]: Skin } = {}, Index = Nodes.length; Index;) {
                var Skin = Nodes[--Index].ReShow();
                if (Skin && !Skins[Skin.Identity]) {
                    Skins[Skin.Identity] = Skin;
                    SkinArray.push(Skin);
                }
            }
            Pub.DeleteElements.Html('');
            for (var Index = SkinArray.length; Index; SkinArray[--Index].OnSet.Function());
        }
        static NullNode: SkinViewNode = new SkinViewNode(null, null, null);
    }
    class SkinView {
        Datas: SkinData[];
        ClientData: SkinData;
        IsClient: boolean;
        constructor(Datas: SkinData[], ClientData: SkinData = null, IsClient = false) {
            this.Datas = Datas;
            this.ClientData = ClientData;
            this.IsClient = IsClient;
        }
        GetData(): SkinData {
            return this.IsClient ? this.ClientData : (this.Datas ? this.Datas[0] : null);
        }
        CheckLogic(Expression: SkinExpression): boolean {
            var Data = this.GetData();
            if (Data == null) return false;
            var Value = Data.$Data;
            if (Value == null) return false;
            var Member = Expression.Get();
            return Member.Value == null ? !!Value : (Value.toString() === Member.Value);
        }
    }
    export class SkinData {
        private $Parent: SkinData;
        $Name: any;
        $Data: any;
        private $Function: Function;
        $Nodes: SkinViewNode[];
        private $IsRemove: boolean;
        constructor(Parent: SkinData, Name: any, Data: Object, Function: Function) {
            if (Data != null) SkinDatas.SetDatas(this, Data);
            this.$Parent = Parent;
            this.$Name = Name;
            this.$Data = Data;
            this.$Function = Function;
        }
        $ReShow(Type: SkinReShowType = SkinReShowType.ReShow) {
            if (this.$Nodes) {
                var Nodes = this.$Nodes;
                this.$Nodes = null;
                for (var Index = Nodes.length; Index; Nodes[--Index].TryShow(Type));
            }
        }
        $Get(Name: any): SkinData {
            if (this.$Data == null) return null;
            var Value = this.$Data[Name];
            if (Value === undefined) return null;
            var Data = this[Name] as SkinData, Function = null;
            if (Value != null && typeof (Value) == 'function') Value = (Function = Value as Function).apply(this.$Data);
            return Data && Data.$Data === Value && Data.$Function === Function ? Data : this[Name] = new SkinData(this, Name, Value, Function);
        }
        $Set(Data: Object, IsReShow = true) {
            if (!this.$Function) {
                if (arguments.length) {
                    var IsData, ParentData;
                    if (this.$Parent) {
                        if ((ParentData = this.$Parent.$Data) == null) {
                            if (this.$Data != null) {
                                SkinDatas.RemoveDatas(this);
                                this.$Data = null;
                            }
                        }
                        else IsData = true;
                    }
                    else IsData = true;
                    if (IsData) {
                        if (this.$Data == null) {
                            if (Data != null) SkinDatas.SetDatas(this, Data);
                        }
                        else if (Data == null) SkinDatas.RemoveDatas(this);
                        else SkinDatas.ReplaceDatas(this, Data);
                        this.$Data = Data;
                        if (ParentData != null) ParentData[this.$Name] = Data;
                    }
                    if (IsReShow) this.$ReShow(SkinReShowType.ReShow);
                }
                else this.$ReShow(SkinReShowType.ReShow);
            }
        }
        $Add(Data: number, IsReShow = true) {
            this.$Set(this.$Data == null ? Data : this.$Data as number + Data, IsReShow);
        }
        $Not(IsReShow = true): boolean {
            this.$Set(!this.$Data, IsReShow);
            return this.$Data as boolean;
        }
        $Copy(Data: Object, IsReShow = true) {
            if (Data) this.$Set(this.$Data == null ? Data : Pub.Copy(this.$Data, Data), IsReShow);
        }
        $Array(): SkinData[] {
            var Data = this.$Data as any[], Datas = [];
            if (Data instanceof Array) {
                for (var Index = 0; Index !== Data.length; Datas.push(this[Index++]));
            }
            return Datas;
        }
        $For(Function: (Data: SkinData) => void) {
            var Data = this.$Data as any[];
            if (Data instanceof Array) {
                for (var Index = 0; Index !== Data.length; Function(this[Index++] as SkinData));
            }
        }
        $Find(IsValue: (Data: any) => boolean): SkinData[] {
            var Data = this.$Data as any[], Datas = [];
            if (Data instanceof Array) {
                for (var Index = -1; ++Index !== Data.length;) {
                    if (IsValue(Data[Index])) Datas.push(this[Index]);
                }
            }
            return Datas;
        }
        $Remove(IsValue: (Data: any) => boolean, IsReShow = true) {
            var Data = this.$Data as any[], Datas = [];
            if (Data instanceof Array) {
                for (var Index = 0; Index !== Data.length; ++Index) {
                    if (IsValue(Data[Index])) {
                        Skin.Refresh();
                        this.$RemoveIndex(Index);
                        for (var WriteIndex = Index; ++Index !== Data.length; ) {
                            if (IsValue(Data[Index])) this.$RemoveIndex(Index);
                            else {
                                Data[WriteIndex] = Data[Index];
                                this.$MoveIndex(Index, WriteIndex++);
                            }
                        }
                        Data.length = WriteIndex;
                        this.$RemoveFinally();
                        return;
                    }
                }
            }
        }
        private $RemoveIndex(Index: number) {
            var RemoveData = this[Index] as SkinData;
            if (RemoveData) RemoveData.$ReShow(SkinReShowType.RemoveArray);
        }
        private $MoveIndex(Index: number, WriteIndex: number) {
            var MoveData = this[Index] as SkinData;
            if (MoveData) (this[WriteIndex] = MoveData).$Name = Index;
            if (this.$Nodes) {
                for (var NodeIndex = this.$Nodes.length; NodeIndex; this.$Nodes[--NodeIndex].RemoveLoop(Index, WriteIndex));
            }
        }
        private $RemoveFinally() {
            if (this.$Nodes) {
                for (var Index = this.$Nodes.length; Index; this.$Nodes[--Index].SetRemoveLoopLength((this.$Data as any[]).length));
            }
            Skin.Refresh();
        }
        $RemoveAt(Index: number, Count: number = 1, IsReShow = true) {
            var Data = this.$Data as any[];
            if (Data instanceof Array && Index < Data.length) {
                Skin.Refresh();
                if (Count = Data.splice(Index, Count).length) {
                    var WriteIndex = Index;
                    for (Index += Count; Index !== WriteIndex; this.$RemoveIndex(--Index));
                    Index += Count;
                    for (var EndIndex = Data.length + Count; Index != EndIndex; this.$MoveIndex(Index++, WriteIndex++));
                    this.$RemoveFinally();
                }
            }
        }
        $Replace(Value: Object, IsValue: (Data: any) => boolean, IsReShow = true) {
            var Data = this.$Data as any[];
            if (Data instanceof Array) {
                for (var Index = 0; Index !== Data.length; ++Index) {
                    if (IsValue(Data[Index])) {
                        Data[Index] = Value;
                        var ReplaceData = this[Index] as SkinData;
                        if (ReplaceData) ReplaceData.$Set(Value, IsReShow);
                    }
                }
            }
        }
        $Push(Value: Object, IsReShow = true) {
            var Data = this.$Data as any[];
            if (Data == null) this.$Set([Value], IsReShow);
            else if (Data instanceof Array) {
                Skin.Refresh();
                Data.push(Value);
                if (IsReShow) this.$ReShow(SkinReShowType.PushArray);
                Skin.Refresh();
            }
        }
        $Pushs(Datas: Object[], IsReShow = true) {
            if (Datas && Datas.length) {
                var Data = this.$Data as any[];
                if (Data == null) this.$Set(Datas, IsReShow);
                else if (Data instanceof Array) {
                    Skin.Refresh();
                    for (var Index = 0; Index !== Datas.length; Data.push(Datas[Index++]));
                    if (IsReShow) this.$ReShow(SkinReShowType.PushArray);
                    Skin.Refresh();
                }
            }
        }
        $PushExpand(Value: Object, IsReShow = true) {
            this.$PushExpands([Value], IsReShow);
        }
        $PushExpands(Datas: Object[], IsReShow = true) {
            if (Datas && Datas.length) {
                var Data = this.$Data as any[];
                if (Data == null) this.$Set(Datas, IsReShow);
                else if (Data instanceof Array) {
                    Skin.Refresh();
                    this.$SetPushLength(Data.length + Datas.length);
                    for (var Index = Data.length; Index; this.$MoveIndex(Index, Index + Datas.length))--Index;
                    var CopyData = Data.Copy();
                    Data.length = 0;
                    for (var Index = 0; Index !== Datas.length; Data.push(Datas[Index++])) this.$ResetIndex(Index);
                    for (var Index = 0; Index !== CopyData.length; Data.push(CopyData[Index++]));
                    if (IsReShow) this.$ReShow(SkinReShowType.PushArrayExpand);
                    Skin.Refresh();
                }
            }
        }
        private $SetPushLength(Count: number) {
            if (this.$Nodes) {
                for (var Index = this.$Nodes.length; Index; this.$Nodes[--Index].SetPushLoopLength(Count));
            }
        }
        private $ResetIndex(Index: number) {
            this[Index] = null;
            if (this.$Nodes) {
                for (var NodeIndex = this.$Nodes.length; NodeIndex; this.$Nodes[--NodeIndex].ResetLoop(Index));
            }
        }
        $Sort(Function: (Left: any, Right: any) => number) {
            var Data = this.$Data as any[];
            if (Data instanceof Array) this.$Set(Data.sort(Function));
        }
        $(Name: string): SkinData {
            return this[Name] as SkinData;
        }
    }
    class SkinDatas {
        Datas: SkinData[];
        constructor(Data: SkinData) {
            this.Datas = [Data];
        }
        //Set(Data: Object) {
        //    var Datas = this.Datas.Copy();
        //    if (arguments.length) {
        //        for (var Index = Datas.length; Index; Datas[--Index].$Set(Data));
        //    }
        //    else {
        //        for (var Index = Datas.length; Index; Datas[--Index].$ReShow());
        //    }
        //}
        Remove(SkinData: SkinData) {
            this.Datas.RemoveAtEnd(this.Datas.IndexOfValue(SkinData));
        }
        //For(Function: (Data: SkinData) => any) {
        //    this.Datas.For(Function);
        //}
        ReShowName(Name: string) {
            for (var Datas = this.Datas.Copy(), Index = Datas.length; Index;) {
                var Data = Datas[--Index][Name];
                if (Data) Data.$ReShow();
            }
        }
        static RemoveDatas(SkinData: SkinData) {
            var Datas = SkinData.$Data['$'] as SkinDatas;
            if (Datas) Datas.Remove(SkinData);
        }
        static ReplaceDatas(SkinData: SkinData, Data: Object) {
            var OldDatas = SkinData.$Data['$'] as SkinDatas, NewDatas = Data['$'] as SkinDatas;
            if (OldDatas) {
                if (NewDatas == OldDatas) return;
                OldDatas.Remove(SkinData);
            }
            this.Push(NewDatas, SkinData, Data);
        }
        static SetDatas(SkinData: SkinData, Data: Object) {
            var Datas = Data['$'] as SkinDatas;
            this.Push(Datas, SkinData, Data);
        }
        private static Push(Datas: SkinDatas, SkinData: SkinData, Data: Object) {
            if (Datas) Datas.Datas.push(SkinData);
            else {
                try {
                    Data['$'] = 1;
                    if (Data['$']) Data['$'] = new SkinDatas(SkinData);
                }
                catch (e) { }
            }
        }
    }
    class SkinMemberName {
        Depth: number;
        Names: string[];
        Value: string;
        constructor(Html: string, StartIndex: number, EndIndex: number, IsLogic: boolean) {
            this.Depth = 0;
            if (StartIndex !== EndIndex) {
                this.Names = [];
                //.
                while (StartIndex !== EndIndex && Html.charCodeAt(StartIndex) === 46) {
                    ++this.Depth;
                    ++StartIndex;
                }
                for (var Index = StartIndex; Index !== EndIndex;) {
                    var Code = Html.charCodeAt(Index);
                    //=
                    if (IsLogic && Code === 61) {
                        this.Names.push(Html.substring(StartIndex, Index));
                        this.Value = Html.substring(Index + 1, EndIndex);
                        return;
                    }
                    //.
                    if (Code === 46) {
                        this.Names.push(Html.substring(StartIndex, Index));
                        StartIndex = ++Index;
                    }
                    else ++Index;
                }
                if (StartIndex !== EndIndex) this.Names.push(Html.substring(StartIndex, EndIndex));
            }
        }
        CreateData(Node: SkinViewNode, IsClient: boolean): SkinData {
            var Datas = Node.Node.Skin.Datas, ParentIndex = Math.max(Datas.length - this.Depth, 1);
            if (!this.Names) {
                var Data = Datas[ParentIndex - 1];
                Node.SetData(Data);
                return Data;
            }
            var NameDatas = [], ClientDatas: SkinData[];
            while (ParentIndex) {
                NameDatas.length = 0;
                var ParentData = Datas[--ParentIndex], NameIndex = 0;
                do {
                    var Data = ParentData.$Get(this.Names[NameIndex]);
                    if (Data == null) break;
                    NameDatas.push(ParentData);
                    if (++NameIndex === this.Names.length) {
                        NameDatas.push(Data);
                        return this.SetData(Node, NameDatas);
                    }
                    ParentData = Data;
                }
                while (true);
                if (IsClient && NameDatas.length >= (ClientDatas ? ClientDatas.length : 0)) {
                    ClientDatas = ParentIndex ? NameDatas.Copy() : NameDatas;
                    ClientDatas.push(ParentData);
                }
            }
            if (IsClient) {
                var ParentData = ClientDatas[NameIndex = ClientDatas.length - 1], Value = ParentData.$Data;
                if (Value != null) {
                    var Name = this.Names[NameIndex];
                    try {
                        Value[Name] = null;
                        ClientDatas.push(ParentData.$Get(Name));
                        return this.SetData(Node, ClientDatas);
                    }
                    catch (e) { }
                }
            }
            return null;
        }
        private SetData(Node: SkinViewNode, Datas: SkinData[]): SkinData {
            if (Datas) {
                for (var Index = 1; Index !== Datas.length; Node.SetData(Datas[Index++]));
                return Datas[Index - 1];
            }
        }
    }
    class SkinExpression {
        Names: SkinMemberName[];
        ClientName: SkinMemberName;
        IsNot: boolean;
        constructor(Names: SkinMemberName[], ClientName: SkinMemberName, IsNot: boolean) {
            this.Names = Names;
            this.ClientName = ClientName;
            this.IsNot = IsNot;
        }
        Get(): SkinMemberName {
            return this.ClientName || this.Names[0];
        }
        CreateView(Node: SkinViewNode): SkinView {
            var Views;
            if (this.Names) {
                Views = [];
                for (var Index = 0; Index !== this.Names.length; Views.push(Data)) {
                    var Data = this.Names[Index++].CreateData(Node, false);
                    if (Data) Node.SetData(Data);
                }
            }
            return new SkinView(Views, this.ClientName ? this.ClientName.CreateData(Node, true) : null, !!this.ClientName);
        }
        static NullExpression: SkinExpression = new SkinExpression(null, new SkinMemberName('', 0, 0, false), false);
    }
    class SkinNode {
        Skin: Skin;
        TypeIndex: number;
        TypeSize: number;
        Html: string;
        Expressions: SkinExpression[];
        StartIndex: number;
        EndIndex: number;
        IsHtml: boolean;
        IsTextArea: boolean;
        IsIdentity: boolean;
        IsLoopIndex: boolean;
        NoMarkAt: boolean;
        IsOrExpression: boolean;
        Nodes: SkinNode[];
        constructor(Skin: Skin, TypeIndex: number) {
            this.Skin = Skin;
            this.TypeIndex = TypeIndex;
            this.TypeSize = SkinBuilder.TypeSizes[TypeIndex];
        }
        GetExpression(Index: number): SkinExpression {
            return this.Expressions ? this.Expressions[Index] : SkinExpression.NullExpression;
        }
    }
    export class Skin {
        Id: string;
        private Node: SkinNode;
        private OnShowed: Events;
        OnSet: Events;
        NoMark: number;
        NoOutput: number;
        Htmls: string[];
        private ViewNode: SkinViewNode;
        Data: any;
        Datas: SkinData[];
        Identity: number;
        constructor(Id: string = null, Html: string = null, OnShowed: Events = null, OnSet: Events = null) {
            this.OnShowed = OnShowed || new Events;
            this.OnSet = OnSet || new Events;
            this.Id = Id;
            this.Identity = ++Pub.Identity;
            (this.Node = new SkinNode(this, 0)).Nodes = new SkinBuilder(this, Html == null ? (Id ? HtmlElement.$Id(Id) : HtmlElement.$(document.body)).Html0().replace(/ @(src|style)=/gi, ' $1=').replace(/select@/gi, 'select') : Html).Nodes;
        }
        private Reset(Value: Object) {
            if (this.Datas == null || this.Data != Value) {
                var Data = new SkinData(null, null, this.Data = Value, null);
                this.Datas = [Data];
                this.ViewNode = new SkinViewNode(this.Node, null, Data);
            }
            else {
                this.Data = Value;
                this.Datas.length = 1;
            }
            this.Htmls = [];
            this.NoOutput = 0;
        }
        ToHtml(Data: Object, IsMark = false): string {
            this.NoMark = IsMark ? 0 : 1;
            this.Reset(Data);
            this.ViewNode.ClearSearchNode(true);
            this.ViewNode.Create();
            return this.EndHtml();
        }
        EndHtml(): string {
            var Html = this.Htmls.join('').replace(/ src="=@[^"]+"/gi, '').replace(/ @check="true"/g, ' checked="checked"');
            this.Datas.length = 1;
            this.Htmls = null;
            return location.protocol == 'https:' ? Html.replace(/http\:\/\/__IMAGEDOMAIN__\//g, 'https://__IMAGEDOMAIN__/') : Html;
        }
        ResetNode(Node: SkinViewNode): SkinData[] {
            this.NoMark = Node.SkinNoMark || 0;
            this.NoOutput = 0;
            this.Datas.length = 1;
            this.Htmls = [];
            return this.Datas;
        }
        Show(Data: Object, IsMark = true) {
            (this.Id ? HtmlElement.$Id(this.Id) : HtmlElement.$(document.body)).Html(this.ToHtml(Data, IsMark)).Display(1);
            this.OnShowed.Function(Data);
            this.OnSet.Function(Data);
        }
        SetHtml(Data: Object, IsMark = true) {
            (this.Id ? HtmlElement.$Id(this.Id) : HtmlElement.$(document.body)).Html(this.ToHtml(Data, IsMark));
            this.OnSet.Function(Data);
        }
        SkinData(Name: string = null): SkinData {
            var Data = this.Datas[0];
            return Name ? Data.$(Name) : Data;
        }
        SearchData(Identity: number): SkinData {
            if (this.ViewNode) {
                var Node = this.ViewNode.SearchNode(Identity);
                if (Node) return Node.Views[0].GetData();
            }
        }
        Hide() {
            (this.Id ? HtmlElement.$Id(this.Id) : HtmlElement.$(document.body)).Display(0);
        }
        static Refresh() {
            SkinViewNode.ReShowTask();
        }
        static Skins: { [key: string]: Skin; } = {};
        static Body: Skin;
        static BodyData(Name: string = null): SkinData {
            return this.Body.SkinData(Name);
        }
        private static Header: Skin;
        static ChangeHeader() {
            document.title = this.Header.ToHtml(this.Body.Data, false);
        }
        static Create(PageView: PageView) {
            for (var Childs = HtmlElement.$('@skin').GetElements(), Index = Childs.length; Index; this.Skins[Id] = new Skin(Id)) {
                var Child = Childs[--Index], Id = Child.id;
                if (!Id) Child.id = Id = HtmlElement.$Attribute(Child, 'skin');
            }
            if (PageView.IsLoadView) {
                this.Header = new Skin(null, document.title.replace(/\=@@/g, '=@'));
                var ViewOver = document.getElementById('__VIEWOVERID__');
                if (ViewOver) {
                    var Display = ViewOver.style.display;
                    ViewOver.style.display = 'none';
                    this.Body = new Skin(null, null, PageView.OnShowed, PageView.OnSet);
                    ViewOver.style.display = Display || '';
                }
                else this.Body = new Skin(null, null, PageView.OnShowed, PageView.OnSet);
            }
        }
        static SearchIdentity(Identity: number): SkinData {
            var Data = this.Body.SearchData(Identity);
            if (Data == null) {
                for (var Name in this.Skins) {
                    var Skin = this.Skins[Name];
                    if (Skin && Skin.SearchData && (Data = Skin.SearchData(Identity))) break;
                }
            }
            return Data;
        }
        static DeleteMark(Span: HTMLElement) {
            SkinViewNode.DeleteMark(Span);
        }
    }
    class SkinBuilder {
        Skin: Skin;
        Html: string;
        Nodes: SkinNode[];
        CheckIndex: number;
        HtmlIndex: number;
        constructor(Skin: Skin, Html: string) {
            this.Skin = Skin;
            this.Html = Html;
            this.Nodes = [];
            var EndIndex = this.HtmlIndex = 0;
            do {
                var Index = this.Html.indexOf('<!--', EndIndex);
                if (Index < 0) break;
                var TypeIndex = this.GetType(Index += 4);
                if (TypeIndex) {
                    EndIndex = this.Html.indexOf('-->', Index);
                    if (EndIndex < 0) break;
                    var TypeSize = SkinBuilder.TypeSizes[TypeIndex], IsExpression = EndIndex - Index - TypeSize;
                    //:
                    if (IsExpression !== 0 && this.Html.charCodeAt(Index + TypeSize) !== 58) {
                        EndIndex += 3;
                        continue;
                    }
                    this.At(Index - 4);
                    var Node = new SkinNode(Skin, TypeIndex);
                    Node.StartIndex = Index;
                    Node.EndIndex = EndIndex;
                    if (this.CheckRound(Node)) {
                        if (IsExpression !== 0) {
                            switch (TypeIndex) {
                                case 1: case 2: this.LogicExpression(Node); break;
                                case 3: case 4: this.ValueExpression(Node); break;
                            }
                        }
                        this.Nodes.push(Node);
                        this.CheckIndex = this.Nodes.length;
                    }
                    this.HtmlIndex = (EndIndex += 3);
                }
                else {
                    EndIndex = this.Html.indexOf('-->', Index - 2);
                    if (EndIndex < 0) break;
                    EndIndex += 3;
                }
            }
            while (true);
            this.At(this.Html.length);
            var ErrorQuery;
            while (this.CheckIndex) {
                var Node = this.Nodes[--this.CheckIndex];
                if (Node.StartIndex && !Node.Nodes) {
                    if (!ErrorQuery) HttpRequest.Post('__PUBERROR__', { error: navigator.appName + ' : ' + navigator.appVersion + '\r\nSkin解析失败: ' + document.location.toString() + '\r\nId[' + Skin.Id + '] ' + this.Html.substring(0, Node.StartIndex).split('\n').length + ' <!--' + this.Html.substring(Node.StartIndex, Node.EndIndex) + '-->' });
                    Node.Nodes = this.Nodes.slice(this.CheckIndex + 1, this.Nodes.length);
                    this.Nodes.length = this.CheckIndex + 1;
                }
            }
        }
        //                            null,If,Not,Loop,Value,NoMark,Html,At
        static TypeSizes: number[] = [0, 2, 3, 4, 5, 6, 4, 2];
        GetType(Index: number): number {
            switch (this.Html.charCodeAt(Index) - 72) {
                case 73 - 72:
                    if (this.Html.charCodeAt(Index + 1) === 102) return 1;
                    break;
                case 76 - 72:
                    if (this.Html.charCodeAt(Index + 1) === 111 && this.Html.charCodeAt(Index + 2) === 111 && this.Html.charCodeAt(Index + 3) === 112) return 3;
                    break;
                case 78 - 72:
                    if (this.Html.charCodeAt(Index + 2) === 116) {
                        if (this.Html.charCodeAt(Index + 1) === 111) return 2;
                    }
                    else if (this.Html.charCodeAt(Index + 1) === 111 && this.Html.charCodeAt(Index + 2) === 77 && this.Html.charCodeAt(Index + 3) === 97 && this.Html.charCodeAt(Index + 4) === 114 && this.Html.charCodeAt(Index + 5) === 107) return 5;
                    break;
                case 86 - 72:
                    if (this.Html.charCodeAt(Index + 1) === 97 && this.Html.charCodeAt(Index + 2) === 108 && this.Html.charCodeAt(Index + 3) === 117 && this.Html.charCodeAt(Index + 4) === 101) return 4;
                    break;
            }
            return 0;
        }
        CheckRound(NewNode: SkinNode): boolean {
            while (this.CheckIndex) {
                var Node = this.Nodes[--this.CheckIndex];
                if (Node.StartIndex && !Node.Nodes) {
                    if (Node.EndIndex - Node.StartIndex === NewNode.EndIndex - NewNode.StartIndex && Node.TypeIndex === NewNode.TypeIndex) {
                        var Start = Node.StartIndex + Node.TypeSize, NewStart = NewNode.StartIndex + Node.TypeSize;
                        while (Start != Node.EndIndex) {
                            if (this.Html.charCodeAt(NewStart++) !== this.Html.charCodeAt(Start++)) return true;
                        }
                        Node.Nodes = this.Nodes.slice(this.CheckIndex + 1, this.Nodes.length);
                        this.Nodes.length = this.CheckIndex + 1;
                        return false;
                    }
                    break;
                }
            }
            return true;
        }
        At(EndIndex: number) {
            for (var NoMark = false, Index = this.HtmlIndex + 1; Index < EndIndex; ++Index) {
                //@
                if (this.Html.charCodeAt(Index) === 64) {
                    //=
                    if (this.Html.charCodeAt(Index - 1) === 61) {
                        if (this.HtmlIndex !== Index - 1) NoMark = this.PushHtml(Index - 1, NoMark);
                        var Node = new SkinNode(this.Skin, 7);
                        Node.NoMarkAt = NoMark;
                        if (++Index !== EndIndex) {
                            var Code = this.Html.charCodeAt(Index);
                            //$
                            if (Code == 36) {
                                Node.IsIdentity = true;
                                ++Index;
                            }
                            //[
                            else if (Code == 91)
                            {
                                //]
                                if (this.Html.charCodeAt(Index + 1) == 93) {
                                    Node.IsLoopIndex = true;
                                    Index += 2;
                                }
                            }
                            else {
                                //@
                                if (Code === 64) Node.IsHtml = true;
                                //*
                                if (Code === 42) Node.IsTextArea = true;
                                if (Node.IsHtml || Node.IsTextArea || Node.IsIdentity)++Index;
                                if (Index !== EndIndex) {
                                    var Names = null, ClientName = null, NameIndex = Index;
                                    do {
                                        while (Index !== EndIndex && SkinBuilder.ValueMap[this.Html.charCodeAt(Index)])++Index;
                                        if (NameIndex === Index) break;
                                        if (!Names) Names = [];
                                        Names.push(new SkinMemberName(this.Html, NameIndex, Index, false));
                                        if (this.Html.charCodeAt(Index) !== 124) break;
                                        NameIndex = ++Index;
                                    }
                                    while (true);
                                    if (Index !== EndIndex) {
                                        //#
                                        if (this.Html.charCodeAt(Index) === 35) {
                                            for (NameIndex = ++Index; Index !== EndIndex && SkinBuilder.ValueMap[this.Html.charCodeAt(Index)]; ++Index);
                                            ClientName = new SkinMemberName(this.Html, NameIndex, Index, false);
                                        }
                                        //$
                                        if (Index !== EndIndex && this.Html.charCodeAt(Index) === 36)++Index;
                                    }
                                    if (Names || ClientName) Node.Expressions = [new SkinExpression(Names, ClientName, false)];
                                }
                            }
                        }
                        this.Nodes.push(Node);
                        this.HtmlIndex = Index;
                    }
                    else ++Index;
                }
            }
            if (this.HtmlIndex < EndIndex) this.PushHtml(EndIndex, NoMark);
        }
        PushHtml(EndIndex: number, NoMark: boolean): boolean {
            var Node = new SkinNode(this.Skin, 6);
            Node.Html = this.Html.substring(this.HtmlIndex, EndIndex);
            this.Nodes.push(Node);
            var Start = Node.Html.lastIndexOf('<'), End = Node.Html.lastIndexOf('>');
            return Start >= 0 ? End < 0 || Start > End : (End < 0 && NoMark);
        }
        LogicExpression(Node: SkinNode) {
            Node.Expressions = [];
            var StartIndex = Node.StartIndex + Node.TypeSize + 1, EndIndex = Node.EndIndex;
            for (var Index = StartIndex; Index !== EndIndex;) {
                var Code = this.Html.charCodeAt(Index);
                //|
                if (Code === 124) {
                    Node.IsOrExpression = true;
                    Node.Expressions.push(this.ParseExpression(StartIndex, Index, true));
                    StartIndex = ++Index;
                }
                //&
                else if (Code === 38) {
                    Node.Expressions.push(this.ParseExpression(StartIndex, Index, true));
                    StartIndex = ++Index;
                }
                //@
                else if (Code === 64) {
                    Node.Expressions.push(this.ParseExpression(StartIndex, Index, true));
                    return;
                }
                else ++Index;
            }
            Node.Expressions.push(this.ParseExpression(StartIndex, EndIndex, true));
        }
        ValueExpression(Node: SkinNode) {
            var StartIndex = Node.StartIndex + Node.TypeSize + 1, EndIndex = Node.EndIndex;
            for (var Index = StartIndex; Index !== EndIndex; ++Index) {
                if (this.Html.charCodeAt(Index) === 64) {
                    EndIndex = Index;
                    break;
                }
            }
            Node.Expressions = [this.ParseExpression(StartIndex, EndIndex)];
        }
        ParseExpression(StartIndex: number, EndIndex: number, IsLogic = false): SkinExpression {
            var Names = null, ClientName = null, IsNot = false;
            if (StartIndex != EndIndex) {
                //!
                if (IsLogic && this.Html.charCodeAt(StartIndex) === 33) {
                    IsNot = true;
                    ++StartIndex;
                }
                for (var HashIndex = EndIndex; HashIndex !== StartIndex;) {
                    //#
                    if (this.Html.charCodeAt(--HashIndex) === 35) {
                        ClientName = new SkinMemberName(this.Html, HashIndex + 1, EndIndex, IsLogic);
                        EndIndex = HashIndex;
                        break;
                    }
                }
                if (StartIndex !== EndIndex) {
                    Names = [];
                    for (var Index = StartIndex; Index !== EndIndex;) {
                        //,
                        if (this.Html.charCodeAt(Index) === 44) {
                            Names.push(new SkinMemberName(this.Html, StartIndex, Index, IsLogic));
                            StartIndex = ++Index;
                        }
                        else ++Index;
                    }
                    if (StartIndex !== EndIndex) Names.push(new SkinMemberName(this.Html, StartIndex, EndIndex, IsLogic));
                }
            }
            return new SkinExpression(Names, ClientName, IsNot);
        }
        static ValueMap: boolean[] = new Array<boolean>(0x7b);
    }
    for (var Index = 0x30; Index !== 0x3a; SkinBuilder.ValueMap[Index++] = true);
    for (var Index = 0x41; Index !== 0x5b; SkinBuilder.ValueMap[Index++] = true);
    for (var Index = 0x61; Index !== 0x7b; SkinBuilder.ValueMap[Index++] = true);
    //.                          _
    SkinBuilder.ValueMap[0x2e] = SkinBuilder.ValueMap[0x5f] = true;
    class PageView {
        IsLoad: boolean;
        IsLoadView: boolean;
        IsView: boolean;
        LoadError: boolean;
        OnShowed: Events;
        OnSet: Events;
        ErrorPath: string;
        ReturnPath: string;
        LocationPath: string;
        Client: Object;
        Query: { [key: string]: string; };
    }
    window.onerror = function (message: string, filename?: string, lineno?: number, colno?: number, error?: Error): void {
        if (Pub.IE) {
            var Location = document.location.toString();
            if ((message != '语法错误' && message != 'Syntax error' && message != '語法錯誤') || Location.substring(0, filename.length) != filename) {
                Pub.SendError(navigator.appName + ' : ' + navigator.appVersion + '\r\n' + Location + '\r\n' + filename + ' [' + lineno + ',' + colno + '] ' + message);
            }
        }
        else {
            if ((lineno || message != 'Script error.') && (lineno != 1 || message != 'Error loading script')) {
                if (lineno == 1) {
                    var Ajax = Pub.AjaxAppendJs;
                    if (Ajax.RetryCount && document.location.origin + Ajax.Url == filename && --Ajax.RetryCount) {
                        Pub.AppendJs(Ajax.Url, Loader.Charset, null, Ajax.GetOnError(null));
                        return;
                    }
                }
                Pub.SendError(navigator.appName + ' : ' + navigator.appVersion + '\r\n' + document.location.toString() + '\r\n' + filename + ' [' + lineno + ',' + colno + '] ' + message);
            }
        }
    };
    export class BrowserEvent {
        Event: Event;
        private target: HTMLElement;
        private pageX: number;
        private pageY: number;
        private which: number;
        srcElement: HTMLElement;
        clientX: number;
        clientY: number;
        keyCode: number;
        shiftKey: boolean;
        Return: boolean;
        touches: BrowserEvent[];
        changedTouches: BrowserEvent[];
        constructor(Event: any) {
            if (this.Event = Event as Event || event) {
                this.srcElement = this.target = (Pub.IE ? this.Event.srcElement : this.Event.target) as HTMLElement;
                this.clientX = this.pageX = Pub.IE ? (this.Event as MouseEvent).clientX : (this.Event as MouseEvent).pageX;
                this.clientY = this.pageY = Pub.IE ? (this.Event as MouseEvent).clientY : (this.Event as MouseEvent).pageY;
                this.keyCode = this.which = Pub.IE ? (this.Event as KeyboardEvent).keyCode : (this.Event as KeyboardEvent).which;
                this.shiftKey = this.Event['shiftKey'];
                this.touches = this.Event['touches'];
                this.changedTouches = this.Event['changedTouches'];
            }
            this.Return = true;
        }
        CancelBubble() {
            this.Return = false;
            if (Pub.IE) this.Event.returnValue = false;
            else this.Event.preventDefault();
        }
        $Name(Name: string, Value: string = null): HTMLElement {
            return HtmlElement.$ElementName(this.srcElement, Name, Value);
        }
    }
    export class DeclareEvent extends BrowserEvent {
        DeclareId: string;
        IsGetOnly: boolean;
        constructor(Id: string = null, IsGetOnly = true) {
            if (Id) {
                var Element = HtmlElement.$IdElement(Id);
                super({ srcElement: Element, target: Element });
                this.DeclareId = Id;
            }
            this.IsGetOnly = IsGetOnly;
        }
        static Default: DeclareEvent = new DeclareEvent();
    }
    export interface IDeclareParameter {
        Id: string;
        Event: DeclareEvent;
        DeclareElement: HTMLElement;
    }
    export interface IDeclare {
        Start(Event: DeclareEvent);
    }
    export interface IDeclareMany  {
        Reset(Parameter: IDeclareParameter, Element: HTMLElement);
    }
    export class Declare {
        private Type: string;
        private EventName: string;
        private Name: string;
        private LowerName: string;
        private AutoCSerName: string;
        constructor(Function: Function, Name: string, EventName: string, Type: string) {
            this.Type = Type;
            this.EventName = EventName;
            this.LowerName = (this.Name = Name).toLowerCase();
            this.AutoCSerName = Name + 's';
            Pub.Functions[Name] = Function as FunctionConstructor;
            Declare.Getters[this.Name] = Pub.ThisFunction(this, this.Get) as (Id: string, IsGetOnly = true) => any;
            Pub.OnLoad(this.Load, this, true);
        }
        private Load() {
            Declare.Declares[this.AutoCSerName] = {};
            HtmlElement.$(document.body).AddEvent(this.EventName, Pub.ThisEvent(this, this[this.Type]));
        }
        private NewDeclare(Parameter: IDeclareParameter): IDeclare {
            return new Pub.Functions[this.Name](Parameter as Object as string) as Object as IDeclare;
        }
        private Src(Event: DeclareEvent): IDeclare {
            var Element = Event.srcElement, ParameterString = HtmlElement.$Attribute(Element, this.LowerName);
            if (ParameterString != null) {
                var Id = Element.id;
                if (Id) {
                    var Values = Declare.Declares[this.AutoCSerName], Value = Values[Id];
                    if (Value) Value.Start(Event);
                    else Values[Id] = Value = this.NewDeclare(Declare.GetParameter(ParameterString, Event, Element, Id));
                    return Value;
                }
                return this.NewDeclare(Declare.GetParameter(ParameterString, Event, Element));
            }
            return Declare.Declares[this.AutoCSerName][Event.DeclareId];
        }
        private AttributeName(Event: DeclareEvent): IDeclare {
            var Element = Event.$Name(this.LowerName);
            if (Element) {
                var Id = Element.id;
                if (Id) {
                    var Values = Declare.Declares[this.AutoCSerName], Value = Values[Id];
                    if (Value) Value.Start(Event);
                    else Values[Id] = Value = this.NewDeclare(Declare.GetParameter(HtmlElement.$Attribute(Element, this.LowerName), Event, Element, Id));
                    return Value;
                }
                return this.NewDeclare(Declare.GetParameter(HtmlElement.$Attribute(Element, this.LowerName), Event, Element));
            }
            return Declare.Declares[this.AutoCSerName][Event.DeclareId];
        }
        private ParameterId(Event: DeclareEvent): IDeclare {
            var Element = Event.$Name(this.LowerName);
            if (Element) {
                var Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')') as IDeclareParameter;
                if (Parameter.Id) {
                    Element = HtmlElement.$IdElement(Parameter.Id);
                    if (!Element) return Declare.Declares[this.AutoCSerName][Parameter.Id];
                    Parameter = null;
                }
                var Id = Element.id, Values = Declare.Declares[this.AutoCSerName], Value;
                if (Id) Value = Values[Id];
                else Element.id = Id = Declare.NextId();
                if (Value) Value.Start(Event);
                else {
                    if (Parameter == null) Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')') as IDeclareParameter;
                    Parameter.Event = Event;
                    Parameter.DeclareElement = Element;
                    Values[Parameter.Id = Id] = Value = this.NewDeclare(Parameter);
                }
                return Value;
            }
            return Declare.Declares[this.AutoCSerName][Event.DeclareId];
        }
        private ParameterMany(Event: DeclareEvent): IDeclareMany {
            if (!Event.IsGetOnly) {
                var Element = Event.$Name(this.LowerName);
                if (Element) {
                    var Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')') as IDeclareParameter;
                    Parameter.DeclareElement = Element;
                    Parameter.Event = Event;
                    if (Parameter.Id) {
                        var Value = Declare.Declares[this.AutoCSerName][Parameter.Id] as Object as IDeclareMany;
                        if (Value) {
                            Value.Reset(Parameter, Element);
                            return Value;
                        }
                        Declare.Declares[this.AutoCSerName][Parameter.Id] = (Value = this.NewDeclare(Parameter) as Object as IDeclareMany) as Object as IDeclare;
                        return Value;
                    }
                }
            }
            return Declare.Declares[this.AutoCSerName][Event.DeclareId] as Object as IDeclareMany;
        }
        private Get(Id: string, IsGetOnly = true) {
            return this[this.Type](new DeclareEvent(Id, IsGetOnly));
        }
        private static NextId(): string {
            return '_' + (++Pub.Identity) + '_DECLARE_';
        }
        private static GetParameter(ParameterString: string, Event: DeclareEvent, Element: HTMLElement, Id: string = null): IDeclareParameter {
            var Parameter = (ParameterString ? eval('(' + ParameterString + ')') : {}) as IDeclareParameter;
            Parameter.Id = Id || Declare.NextId();
            Parameter.DeclareElement = Element;
            Parameter.Event = Event;
            return Parameter;
        }
        static Getters: { [key: string]: (Id: string, IsGetOnly: boolean) => IDeclare } = {};
        static Declares: { [key: string]: { [key: string]: IDeclare } } = {};
    }
    export interface ICookieValue {
        Name: string;
        Value: any;
        Expires: Date;
        Path: string;
        Domain: string;
        Secure: string;
    }
    export class Cookie {
        private static DefaultParameter = { Expires: null, Path: '/', Domain: location.hostname, Secure: null };
        private Expires: number;
        private Path: string;
        private Domain: string;
        private Secure: string;

        //private Name: string;
        constructor(Parameter) {
            Pub.GetParameter(this, Cookie.DefaultParameter, Parameter);
        }
        Write(Value: any) {
            this.WriteCookie(Value as ICookieValue);
        }
        private WriteCookie(Value: ICookieValue) {
            var Cookie = Value.Name.Escape() + '=' + (Value.Value == null ? '.' : Value.Value.toString().Escape()), Expires = Value.Expires;
            if (Value.Value == null) Expires = new Date;
            else if (!Expires && this.Expires) Expires = (new Date).AddMilliseconds(this.Expires);
            if (Expires) Cookie += '; expires=' + Expires['toGMTString']();
            var Path = Value.Path || this.Path;
            if (Path) Cookie += '; path=' + Path;
            var Domain = Value.Domain || this.Domain;
            if (Domain) Cookie += '; domain=' + Domain;
            var Secure = Value.Secure || this.Secure;
            if (Secure) Cookie += '; secure=' + Secure;
            document.cookie = Cookie;
            //this.Name = '';
        }
        Read(Name: string, DefaultValue: string = null): string {
            for (var Values = document.cookie.split('; '), Value = null, Index = Values.length; --Index >= 0 && Value == null;) {
                var IndexOf = Values[Index].indexOf('=');
                if (window['unescape'](Values[Index].substring(0, IndexOf)) == Name) Value = window['unescape'](Values[Index].substring(IndexOf + 1));
            }
            return Value || DefaultValue;
        }
        ReadJson(Name: string, DefaultValue: any): any {
            var Value = this.Read(Name, null);
            return Value ? eval('(' + Value + ')') : DefaultValue;
        }
        static Default: Cookie = new Cookie({ Expires: 24 * 3600 * 1000 });
    }
    interface IServerTime {
        Now: Date;
    }
    export class ServerTime {
        private Time: number;
        constructor(Time: IServerTime) {
            this.Time = -(new Date().getTime() - Time.Now.getTime());
        }
        Now() {
            return new Date().AddMilliseconds(this.Time);
        }
    }
}
setTimeout(AutoCSer.Pub.LoadIE, 0, 'javascript');