'use strict';
Array.prototype.Copy = function () {
	for (var Index = 0, Array = []; Index !== this.length; Array.push(this[Index++]))
		;
	return Array;
};
Array.prototype.Push = function (Value) {
	this.push(Value);
	return this;
};
Array.prototype.RemoveValue = function (Value) {
	return this.Remove(function (Data) { return Data == Value; });
};
Array.prototype.Remove = function (IsValue) {
	for (var Index = -1; ++Index != this.length;) {
		if (IsValue(this[Index])) {
			for (var WriteIndex = Index; ++Index != this.length;) {
				if (!IsValue(this[Index]))
					this[WriteIndex++] = this[Index];
			}
			this.length = WriteIndex;
			break;
		}
	}
	return this;
};
Array.prototype.RemoveAt = function (Index, Count) {
	if (Count === void 0) { Count = 1; }
	if (Index >= 0)
		this.splice(Index, Count);
	return this;
};
Array.prototype.RemoveAtEnd = function (Index) {
	if (Index >= 0 && Index < this.length) {
		this[Index] = this[this.length - 1];
		--this.length;
	}
	return this;
};
Array.prototype.IndexOf = function (Function) {
	for (var Index = -1; ++Index !== this.length;) {
		if (Function(this[Index]))
			return Index;
	}
	return -1;
};
Array.prototype.IndexOfValue = function (Value) {
	if (AutoCSer.Pub.IE) {
		for (var Index = -1; ++Index !== this.length;) {
			if (this[Index] == Value)
				return Index;
		}
		return -1;
	}
	return this.indexOf(Value);
};
Array.prototype.First = function (Function) {
	var Index = this.IndexOf(Function);
	if (Index >= 0)
		return this[Index];
};
Array.prototype.MaxValue = function () {
	if (this.length) {
		for (var Index = this.length, Value = this[--Index]; Index;) {
			if (this[--Index] > Value)
				Value = this[Index];
		}
		return Value;
	}
};
Array.prototype.Max = function (GetKey) {
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
};
Array.prototype.FormatAjax = function () {
	if (this.length) {
		for (var Index = 0, Names = this[0].split(','), Values = []; ++Index !== this.length;) {
			for (var Value = this[Index], NewValue = {}, NameIndex = -1; ++NameIndex !== Names.length; NewValue[Names[NameIndex]] = Value[NameIndex])
				;
			Values.push(NewValue);
		}
		return Values;
	}
};
Array.prototype.FormatView = function () {
	return this.length > 1 ? AutoCSer.Pub.FormatAjaxs(AutoCSer.Pub.AjaxName(this[0], 0), this, 1) : [];
};
Array.prototype.Find = function (IsValue) {
	for (var Values = [], Index = 0; Index !== this.length; ++Index) {
		var Value = this[Index];
		if (IsValue(Value))
			Values.push(Value);
	}
	return Values;
};
Array.prototype.ToArray = function (GetValue) {
	for (var Values = [], Index = 0; Index !== this.length; Values.push(GetValue(this[Index++])))
		;
	return Values;
};
Array.prototype.For = function (Function) {
	for (var Index = 0; Index !== this.length; Function(this[Index++]))
		;
	return this;
};
Array.prototype.ToHash = function (Function) {
	for (var Values = {}, Index = 0; Index !== this.length;) {
		var Value = this[Index++];
		Values[Function ? Function(Value) : Value] = Value;
	}
	return Values;
};
Array.prototype.Sort = function (GetKey) {
	return this.sort(function (Left, Right) { return GetKey(Left) - GetKey(Right); });
};
Array.prototype.MakeString = function () {
	return String.fromCharCode.apply(null, this);
};
String.prototype.Escape = function () {
	return window['escape'](this.replace(/\xA0/g, ' ')).replace(/\+/g, '%2b');
};
String.prototype.ToHTML = function () {
	return this.ToTextArea().replace(/ /g, '&nbsp;').replace(/"/g, '&#34;').replace(/'/g, '&#39;');
};
String.prototype.ToTextArea = function () {
	return this.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
};
String.prototype.Trim = function () {
	return this.replace(/(^\s*)|(\s*$)/g, '');
};
String.prototype.PadLeft = function (Count, Char) {
	var Value = '';
	if ((Count -= this.length) > 0) {
		for (Char = Char.charAt(0); Count; Char += Char, Count >>= 1) {
			if (Count & 1)
				Value += Char;
		}
	}
	return Value + this;
};
String.prototype.Left = function (Length) {
	for (var Value = 0, Index = -1; ++Index != this.length && Length > 0;)
		if ((Length -= this.charCodeAt(Index) > 0xff ? 2 : 1) >= 0)
			++Value;
	return this.substring(0, Value);
};
String.prototype.Right = function (Length) {
	return this.length > Length ? this.substring(this.Length - Length) : this;
};
String.prototype.ToLower = function () {
	return this.substring(0, 1).toLowerCase() + this.substring(1);
};
String.prototype.Length = function () {
	for (var Value = this.length, Index = 0; Index - this.length;)
		if (this.charCodeAt(Index++) > 0xff)
			++Value;
	return Value;
};
String.prototype.SplitInt = function (Split) {
	var Value = this.Trim();
	return Value.length ? this.split(Split).ToArray(function (Value) { return parseInt(0 + Value); }) : [];
};
String.prototype.ParseDate = function () {
	var Value = this.Trim();
	if (Value) {
		var DateValue = new Date(Value = Value.replace(/\-/g, '/'));
		if (!isNaN(DateValue.getTime()))
			return DateValue;
		Value = Value.replace(/[ :\/]+/g, ' ').split(' ');
		DateValue = new Date(Value[0], parseInt(Value[1]) - 1, Value[2], Value[3], Value[4], Value[5]);
		if (!isNaN(DateValue.getTime()))
			return DateValue;
	}
};
Date.prototype.AddMilliseconds = function (Value) {
	var NewDate = new Date;
	NewDate.setTime(this.getTime() + Value);
	return NewDate;
};
Date.prototype.AddMinutes = function (Value) {
	return this.AddMilliseconds(Value * 1000 * 60);
};
Date.prototype.AddHours = function (Value) {
	return this.AddMilliseconds(Value * 1000 * 60 * 60);
};
Date.prototype.AddDays = function (Value) {
	return this.AddMilliseconds(Value * 1000 * 60 * 60 * 24);
};
Date.prototype.ToString = function (Format, IsFixed) {
	if (IsFixed === void 0) { IsFixed = true; }
	var Value = { y: this.getFullYear(), M: this.getMonth() + 1, d: this.getDate(), h: this.getHours(), m: this.getMinutes(), s: this.getSeconds(), S: this.getMilliseconds() };
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
				else if (Value[Char] != null)
					Values.push(Format.substring(LastIndex, LastIndex = Index));
			}
		}
		Format = Values.join('');
	}
	else {
		for (var Name in Value)
			Format = Format.replace(new RegExp(Name, 'g'), Value[Name].toString());
	}
	return Format.replace(/w/g, ['日', '一', '二', '三', '四', '五', '六'][this.getDay()]);
};
Date.prototype.ToDateString = function () { return this.ToString('yyyy/MM/dd'); };
Date.prototype.ToTimeString = function () { return this.ToString('HH:mm:ss'); };
Date.prototype.ToMinuteString = function () { return this.ToString('yyyy/MM/dd HH:mm'); };
Date.prototype.ToSecondString = function () { return this.ToString('yyyy/MM/dd HH:mm:ss'); };
Date.prototype.ToMinuteOrDateString = function () { return this.ToInt() == new Date().ToInt() ? this.ToString('HH:mm') : this.ToDateString(); };
Date.prototype.ToInt = function () {
	return (this.getFullYear() << 9) + ((this.getMonth() + 1) << 5) + this.getDate();
};
Number.prototype.IntToDate = function () { return new Date(this >> 9, ((this >> 5) & 15) - 1, this & 31); };
Number.prototype.ToDisplay = function () { return this.toString() == '0' ? 'none' : ''; };
Number.prototype.ToDisplayNone = function () { return this.toString() == '0' ? '' : 'none'; };
Number.prototype.ToTrue = function () { return this.toString() != '0'; };
Number.prototype.ToFalse = function () { return this.toString() == '0'; };
Boolean.prototype.ToDisplay = function () { return this.toString() == 'true' ? '' : 'none'; };
Boolean.prototype.ToDisplayNone = function () { return this.toString() == 'true' ? 'none' : ''; };
Boolean.prototype.ToTrue = function () { return this.toString() == 'true'; };
Boolean.prototype.ToFalse = function () { return this.toString() != 'true'; };
var AutoCSer;
(function (AutoCSer) {
	var Pub = (function () {
		function Pub() {
		}
		Pub.Extends = function (Son, Base) {
			for (var Name in Base)
				if (Base.hasOwnProperty(Name))
					Son[Name] = Base[Name];
			function Constructor() { this.constructor = Son; }
			Son.prototype = Base === null ? Object.create(Base) : (Constructor.prototype = Base.prototype, new Constructor());
		};
		Pub.ToArray = function (Value, StartIndex) {
			if (StartIndex === void 0) { StartIndex = 0; }
			for (var Values = []; StartIndex < Value.length; ++StartIndex)
				Values.push(Value[StartIndex]);
			return Values;
		};
		Pub.SendError = function (Error) {
			if (!this.Errors[Error]) {
				this.Errors[Error] = Error;
				HttpRequest.Get('Pub.Error', { error: Error.length > 512 ? Error.substring(0, 512) + '...' : Error });
			}
		};
		Pub.ThisFunction = function (This, Function, Arguments, IsArgument) {
			if (Arguments === void 0) { Arguments = null; }
			if (IsArgument === void 0) { IsArgument = true; }
			if (Function) {
				var Value = function () {
					if (Pub.IsTryError) {
						try {
							return Function.apply(This, IsArgument ? Pub.ToArray(arguments).concat(Arguments || []) : Arguments);
						}
						catch (e) {
							Pub.SendError((e ? (e.stack || e).toString() : '未知错误') + '\r\n' + Function.toString());
						}
					}
					return Function.apply(This, IsArgument ? Pub.ToArray(arguments).concat(Arguments || []) : Arguments);
				};
				Value['Test'] = Function;
				return Value;
			}
			Pub.SendError('Function is null\r\n' + window['caller']);
		};
		Pub.ThisEvent = function (This, Function, Arguments, Frame) {
			if (Arguments === void 0) { Arguments = null; }
			if (Frame === void 0) { Frame = null; }
			if (Function) {
				return function (Event) {
					var Browser = new BrowserEvent(Pub.IE ? Frame ? Frame.event || event : event : Event);
					if (Pub.IsTryError) {
						try {
							Function.apply(This, Arguments ? [Browser].concat(Arguments || []) : [Browser]);
						}
						catch (e) {
							Pub.SendError((e ? (e.stack || e).toString() : '未知错误') + '\r\n' + Function.toString());
						}
					}
					else
						Function.apply(This, Arguments ? [Browser].concat(Arguments || []) : [Browser]);
					return Browser.Return;
				};
			}
			Pub.SendError('Event is null\r\n' + window['caller']);
		};
		Pub.EvalJson = function (Text) {
			var Value = eval(Text);
			Pub.JsonLoopObjects.length = 0;
			return Value;
		};
		Pub.Copy = function (Left, Right) {
			for (var Name in Right)
				Left[Name] = Right[Name];
			return Left;
		};
		Pub.SetJsonLoop = function (Index, Value) {
			var CacheValue = this.JsonLoopObjects[Index];
			if (CacheValue) {
				if (CacheValue instanceof Array)
					CacheValue.push.apply(CacheValue, Value);
				else
					this.Copy(CacheValue, Value);
				return CacheValue;
			}
			return this.JsonLoopObjects[Index] = Value;
		};
		Pub.GetJsonLoop = function (Index, Array) {
			return this.JsonLoopObjects[Index] || (this.JsonLoopObjects[Index] = Array || {});
		};
		Pub.AjaxName = function (Name, StartIndex) {
			for (var Values = { Names: [] }, Index = StartIndex; Index - Name.length;) {
				var Code = Name.charCodeAt(Index);
				if (Code === 91) {
					var Value = this.AjaxName(Name, Index + 1);
					Value.Name = Name.substring(StartIndex, Index);
					StartIndex = Index = Value.Index;
					Values.Names.push(Value);
				}
				else if (Code === 93) {
					var SubName = Name.substring(StartIndex, Index++);
					if (SubName)
						Values.Names.push({ Name: SubName });
					Values.Index = Index;
					return Values;
				}
				else if (Code === 44) {
					var SubName = Name.substring(StartIndex, Index++);
					if (SubName) {
						if (SubName.charCodeAt(0) === 64)
							Values.ViewType = SubName.substring(1);
						else
							Values.Names.push({ Name: SubName });
					}
					StartIndex = Index;
				}
				else
					++Index;
			}
			if (StartIndex - Name.length) {
				var SubName = Name.substring(StartIndex);
				if (SubName.charCodeAt(0) === 64)
					Values.ViewType = SubName.substring(1);
				else
					Values.Names.push({ Name: SubName });
			}
			return Values;
		};
		Pub.FormatAjaxs = function (Name, Values, StartIndex) {
			for (var Value = [], Index = StartIndex - 1; ++Index - Values.length; Value.push(Values[Index] ? this.FormatAjax(Name, Values[Index]) : null))
				;
			return Value;
		};
		Pub.FormatAjax = function (Name, Values) {
			var Names = Name.Names;
			if (Names && Names.length) {
				if (Names[0].Name) {
					for (var Value = {}, Index = Names.length; --Index >= 0;) {
						Value[Names[Index].Name] = Values[Index] != null ? (Names[Index].Names && Names[Index].Names.length ? this.FormatAjax(Names[Index], Values[Index]) : Values[Index]) : null;
					}
					if (Name.ViewType) {
						if (Name.ViewType.charAt(0) == '.')
							Value = eval(Name.ViewType.substring(1) + '.Get(Value)');
						else
							Value = eval('new ' + Name.ViewType + '(Value)');
					}
					return Value;
				}
				else if (Values[0])
					return this.FormatAjaxs(Names[0], Values[0], 0);
			}
			else
				return Values;
		};
		Pub.AppendJs = function (Src, Charset, OnLoad, OnError) {
			if (Charset === void 0) { Charset = AutoCSer.Loader.Charset; }
			if (OnLoad === void 0) { OnLoad = null; }
			if (OnError === void 0) { OnError = null; }
			if (OnLoad || OnError)
				new LoadJs(AutoCSer.Loader.CreateJavascipt(Src, Charset), OnLoad, OnError);
			else
				AutoCSer.Loader.AppendJavaScript(Src, Charset);
		};
		Pub.ToJson = function (Value, IsIgnore, IsNameQuery, IsSortName, Parents) {
			if (IsIgnore === void 0) { IsIgnore = false; }
			if (IsNameQuery === void 0) { IsNameQuery = true; }
			if (IsSortName === void 0) { IsSortName = true; }
			if (Parents === void 0) { Parents = null; }
			if (Value != null) {
				var Type = typeof (Value);
				if (Type != 'function' && (!IsIgnore || Value)) {
					if (Type == 'string')
						return '"' + Value.toString().replace(/[\\"]/g, '\\$&').replace(/\n/g, '\\n') + '"';
					if (Type == 'number' || Type == 'boolean')
						return Value.toString();
					Type = Object.prototype.toString.apply(Value);
					if (Type == '[object Date]')
						return 'new Date(' + Value.getTime() + ')';
					if (!Parents)
						Parents = [];
					for (var Index = 0; Index - Parents.length; ++Index)
						if (Parents[Index] == Value)
							return 'null';
					if (typeof (Value.ToJson) == 'function')
						return Value.ToJson(IsIgnore, IsNameQuery, IsSortName, Parents);
					Parents.push(Value);
					var Values = [];
					if (Type == '[object Array]') {
						for (var Index = 0; Index - Value.length; ++Index)
							Values.push(this.ToJson(Value[Index], IsIgnore, IsNameQuery, IsSortName, Parents));
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
		};
		Pub.ToQuery = function (Value, IsIgnore) {
			if (IsIgnore === void 0) { IsIgnore = false; }
			var Values = [];
			for (var Name in Value) {
				if (Name != '$' && Value[Name] != null) {
					var Type = typeof (Value[Name]);
					if (Type != 'function' && (!IsIgnore || Value[Name]))
						Values.push(Name.Escape() + '=' + Value[Name].toString().Escape());
				}
			}
			return Values.join('&');
		};
		Pub.GetLocationSearch = function (Location) {
			return (Location || location).search.toString().replace(/^\?/g, '');
		};
		Pub.GetLocationHash = function (Location) {
			return (Location || location).hash.toString().replace(/^#(\!|\%21)?/g, '');
		};
		Pub.FillQuery = function (Value, Search, IsVersion) {
			var Query = Search.split('&'), Unescape = window['unescape'];
			if (Query.length == 1 && Search.indexOf('=') == -1)
				Value[''] = Unescape(Search);
			else {
				for (var Index = Query.length; --Index >= 0;) {
					var KeyValue = Query.pop().split('='), key = Unescape(KeyValue[0]);
					if (IsVersion || key != 'v')
						Value[key] = KeyValue.length < 2 ? '' : Unescape(KeyValue[1]);
				}
			}
		};
		Pub.CreateQuery = function (Location) {
			var Value = {}, Search = this.GetLocationSearch(Location), Hash = this.GetLocationHash(Location);
			if (Hash.length)
				this.FillQuery(Value, Hash, true);
			if (Search.length)
				this.FillQuery(Value, Search, false);
			return Value;
		};
		Pub.OnLoad = function (OnLoad, This, IsOnce) {
			if (This === void 0) { This = null; }
			if (IsOnce === void 0) { IsOnce = false; }
			if (This)
				OnLoad = this.ThisFunction(This, OnLoad);
			if (!IsOnce)
				this.OnLoadedHash.Add(OnLoad);
			if (this.IsLoad)
				OnLoad();
			else
				this.OnLoads.push(OnLoad);
		};
		Pub.LoadModule = function (Path) {
			if (this.IsModules[Path] == null)
				HttpRequest.Get('Pub.Error', { error: document.location.toString() + ' 加载了未知模块 ' + Path });
			else {
				this.IsModules[Path] = true;
				for (var Loads = this.LoadModules[Path], Index = Loads ? Loads.length : 0; --Index >= 0;) {
					var Load = Loads[Index];
					if (Load && Load.Paths[Path]) {
						Load.Paths[Path] = 0;
						if (--Load.Count == 0 && Load.OnLoad) {
							if (Load.IsLoad)
								this.OnLoad(Load.OnLoad);
							else
								Load.OnLoad();
						}
					}
				}
			}
		};
		Pub.OnModule = function (Paths, OnLoad, IsLoad, Version) {
			if (IsLoad === void 0) { IsLoad = true; }
			if (Version === void 0) { Version = AutoCSer.Loader.Version; }
			for (var Index = Paths.length, Load = { IsLoad: IsLoad, OnLoad: OnLoad, Count: 0, Paths: {} }; Index;) {
				var Path = Paths[--Index];
				if (!this.IsModules[Path]) {
					++Load.Count;
					var Loads = this.LoadModules[Path];
					if (!Loads)
						this.LoadModules[Path] = Loads = [];
					Load.Paths[Path] = 1;
					Loads.push(Load);
					this.LoadModuleWhenNull(Path, Version);
				}
			}
			if (!Load.Count && OnLoad) {
				if (IsLoad)
					this.OnLoad(OnLoad);
				else
					OnLoad();
			}
		};
		Pub.LoadModuleWhenNull = function (Path, Version) {
			if (this.IsModules[Path] == null) {
				this.IsModules[Path] = false;
				this.AppendJs(AutoCSer.Loader.JsDomain + 'Js/' + Path + '.js?v=' + (this.ModuleVersions[Path] || Version));
			}
		};
		Pub.LoadView = function (View, IsReView) {
			if (!View)
				(View = new PageView).ErrorPath = '/';
			if (View.ErrorRequest) {
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
				else
					location.replace(View.ErrorPath + '#url=' + (View.ReturnPath || location.toString()).Escape());
			}
			else if (View.LocationPath)
				location.replace(View.LocationPath);
			View.Client = this.ClientView;
			View.Query = this.Query;
			if (!IsReView) {
				View.OnShowed = this.PageView.OnShowed;
				View.OnSet = this.PageView.OnSet;
				View.IsLoadView = View.IsLoad = View.IsView = true;
				this.PageView = View;
				this.ReadyState();
			}
		};
		Pub.ReLoad = function () {
			var ViewOver = document.getElementById('AutoCSerViewOver');
			if (ViewOver)
				ViewOver.innerHTML = '正在尝试重新加载视图数据...';
			var Query = this.CreateLoadViewQuery();
			if (!Query.IsVersion)
				Query.IsRandom = true;
			HttpRequest.GetQuery(Query);
		};
		Pub.LoadHash = function (PageView) {
			if (!PageView.ErrorRequest) {
				this.OnBeforeUnLoad.Function();
				var Data = Skin.Body.Data;
				for (var Name in PageView) {
					var Value = PageView[Name];
					if (Value == null || (!Value.ViewOnly && typeof (Value) != 'function'))
						Data[Name] = Value;
				}
				this.OnLoadHash.Function(Data);
				this.LoadView(Data, true);
				Skin.Body.Show(Data);
				Skin.ChangeHeader();
				this.OnLoadedHash.Function();
				if (this.LoadHashScrollTop)
					HtmlElement.$SetScrollTop(0);
			}
		};
		Pub.CheckHash = function (Event) {
			var Hash = HtmlElement.$Hash();
			if (Hash !== this.LocationHash) {
				this.LocationHash = Hash;
				this.Query = Pub.CreateQuery(location);
				this.ReView();
				Pub.OnQueryEvents.Function();
			}
			if (Pub.IE)
				setTimeout(this.CheckHashFunction, 100);
		};
		Pub.ReView = function (IsReView) {
			if (IsReView === void 0) { IsReView = false; }
			var Query = new HttpRequestQuery(document.location.pathname + (IsReView ? '&r=' : ''), this.Query, this.ThisFunction(this, this.LoadHash));
			Query.IsRandom = true;
			HttpRequest.GetQuery(Query);
		};
		Pub.ReadyState = function () {
			var View = this.PageView, IsLoad = document.body && (document.readyState == null || document.readyState.toLowerCase() === 'complete');
			if (IsLoad && !this.LoadComplete) {
				HtmlElement.$('@body').To();
				Skin.Create(View);
				this.DeleteElements = HtmlElement.$Create('div').Styles('padding,margin', '10px').Style('border', '10px solid red').Opacity(0).To();
				this.IsBorder = this.DeleteElements.XY0().Left - 10;
				this.DeleteElements.Styles('padding,margin,border', '0px');
				this.IsBorder -= this.DeleteElements.XY0().Left;
				if (this.IsBorder === 20)
					this.IsPadding = true;
				this.IsFixed = Pub.IE ? this.DeleteElements.Style('position', 'fixed').Style('left', '50%').Element0().offsetLeft : 1;
				this.DeleteElements.Display(0);
				this.LoadComplete = true;
			}
			if (IsLoad && View.IsLoad) {
				if (View.IsLoadView) {
					if (View.LoadError) {
						View.IsLoad = View.IsLoadView = View.LoadError = false;
						var ViewOver = document.getElementById('AutoCSerViewOver');
						if (ViewOver)
							ViewOver.innerHTML = '错误：视图数据加载失败，稍后尝试重新加载';
						document.title = 'Server Error';
						setTimeout(this.ThisFunction(this, this.ReLoad), 2000);
						return;
					}
					Skin.Body.Show(View);
					Skin.ChangeHeader();
				}
				else {
					document.body.innerHTML = document.body.innerHTML.replace(/ @(src|style)=/gi, ' $1=');
					var ViewOver = document.getElementById('AutoCSerViewOver');
					if (ViewOver)
						document.body.removeChild(ViewOver);
				}
				this.OnReadyState();
				this.IsLoad = true;
				for (var Index = -1; ++Index - this.OnLoads.length; this.OnLoads[Index]())
					;
				this.OnLoads = this.ReadyFunction = null;
			}
			else
				setTimeout(this.ReadyFunction, 1);
		};
		Pub.OnReadyState = function () {
			this.LocationHash = HtmlElement.$Hash();
			HtmlElement.$(document.body).AddEvent('focus', this.FocusEvents = new Events());
			if (!AutoCSer.Loader.Version || AutoCSer.Loader.LoadScript) {
				var Path = document.location.pathname, Index = Path.lastIndexOf('/'), EndIndex = Path.lastIndexOf('.');
				if (EndIndex > Index)
					Path = Path.substring(0, EndIndex);
				if (Path.charCodeAt(Path.length - 1) === 47)
					Path += 'index';
				this.AppendJs(AutoCSer.Loader.JsDomain + Path.substring(1) + '.js?v=' + AutoCSer.Loader.Version);
			}
			if (Pub.IE)
				setTimeout(this.CheckHashFunction = this.ThisFunction(this, this.CheckHash), 100);
			else
				window.onhashchange = this.ThisFunction(this, this.CheckHash);
		};
		Pub.CreateLoadViewQuery = function () {
			return new HttpRequestQuery(document.location.pathname, this.Query, this.ThisFunction(this, this.LoadView), AutoCSer.Loader.ViewVersion != null);
		};
		Pub.LoadIE = function () {
			Pub.IE = !arguments.length || navigator.appName == 'Microsoft Internet Explorer';
			HttpRequest.Load();
			Pub.Load();
		};
		Pub.Load = function () {
			this.OnLoadHash = new Events();
			this.OnLoadedHash = new Events();
			this.OnBeforeUnLoad = new Events();
			window.onbeforeunload = function (Event) { Pub.OnBeforeUnLoad.Function(Event); };
			this.Query = this.CreateQuery(self.location);
			this.PageView = new PageView();
			this.ReadyFunction = this.ThisFunction(this, this.ReadyState);
			if (AutoCSer.Loader.PageView) {
				this.PageView.IsLoadView = true;
				this.PageView.OnShowed = new Events();
				this.PageView.OnSet = new Events();
				HttpRequest.GetQuery(this.CreateLoadViewQuery());
			}
			else
				this.PageView.IsLoad = true;
			if (!AutoCSer.Loader.PageView)
				this.ReadyState();
		};
		Pub.GetParameter = function (Value, DefaultParameter, Parameter) {
			if (Parameter === void 0) { Parameter = null; }
			if (Parameter) {
				for (var Name in DefaultParameter) {
					var ParameterValue = Parameter[Name];
					Value[Name] = ParameterValue == null ? DefaultParameter[Name] : ParameterValue;
				}
			}
			else {
				for (var Name in DefaultParameter)
					Value[Name] = DefaultParameter[Name];
			}
		};
		Pub.GetEvents = function (Value, DefaultEvents, Parameter) {
			if (Parameter === void 0) { Parameter = null; }
			if (Parameter) {
				for (var Name in DefaultEvents) {
					var Function = Parameter[Name], Event = new Events();
					if (Function)
						Event.Add(Function);
					Value[Name] = Event;
				}
			}
			else {
				for (var Name in DefaultEvents)
					Value[Name] = new Events();
			}
		};
		Pub.LoadViewType = function (Type, Name) {
			if (Name === void 0) { Name = 'Id'; }
			Type.Views = {};
			Type.Get = function (Value, IsGetOnly) {
				if (IsGetOnly)
					return Type.Views[Value[Name] || Value];
				var Id = Value[Name];
				if (Id) {
					var ViewValue = Type.Views[Id];
					if (ViewValue) {
						var Values = [];
						Pub.CopyView(ViewValue, Value, Values);
						if (ViewValue.OnCopyView)
							ViewValue.OnCopyView();
						return ViewValue;
					}
					return Type.Views[Id] = new Type(Value);
				}
				return new Type(Value);
			};
		};
		Pub.CopyView = function (Left, Right, Values) {
			if (Left === Right)
				return Left;
			if (Values.IndexOfValue(Right) < 0) {
				Values.push(Right);
				for (var Name in Right) {
					if (Name != 'ViewOnly') {
						if (Left[Name] == null || Right[Name] == null || typeof (Left[Name]) != 'object')
							Left[Name] = Right[Name];
						else
							this.CopyView(Left[Name], Right[Name], Values);
					}
				}
				Values.pop();
			}
			return Left;
		};
		Pub.ViewFlagEnum = function (EnumString, Enum) {
			var Value = {}, IntValue = parseInt(EnumString);
			if (isNaN(IntValue)) {
				IntValue = 0;
				if (EnumString)
					for (var Values = EnumString.split(','), Index = Values.length; Index; IntValue |= Enum[Values[--Index].Trim()] || 0)
						;
			}
			for (var Name in Enum) {
				var EnumValue = Enum[Name];
				if (typeof (EnumValue) == 'number')
					Value[Name] = (IntValue & EnumValue) == EnumValue ? EnumValue : 0;
			}
			return Value;
		};
		Pub.DisableZoom = function () {
			if (navigator.appVersion && navigator.appVersion.indexOf('MicroMessenger') + 1) {
				var Meta = document.createElement('meta');
				Meta.name = 'viewport';
				Meta.content = 'width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;';
				AutoCSer.Loader.DocumentHead.appendChild(Meta);
			}
		};
		Pub.DefaultCallAjaxPost = function (Url, Send, Callback) {
			if (Send === void 0) { Send = null; }
			if (Callback === void 0) { Callback = null; }
			HttpRequest.Post(Url, Send, Callback);
		};
		Pub.GetAjaxPost = function () {
			return this.CallAjaxPost || this.DefaultCallAjaxPost;
		};
		Pub.DefaultCallAjaxGet = function (Url, Send, Callback, IsVersion) {
			if (Send === void 0) { Send = null; }
			if (Callback === void 0) { Callback = null; }
			if (IsVersion === void 0) { IsVersion = false; }
			AutoCSer.HttpRequest.Get(Url, Send, Callback, IsVersion);
		};
		Pub.GetAjaxGet = function () {
			return this.CallAjaxGet || this.DefaultCallAjaxGet;
		};
		Pub.GetHtmlEditor = function (Element) {
			return HtmlElement.$ElementName(Element, 'htmleditor');
		};
		Pub.Errors = {};
		Pub.IsTryError = true;
		Pub.JsonLoopObjects = [];
		Pub.OnLoads = [];
		Pub.IsModules = {};
		Pub.LoadModules = {};
		Pub.ModuleVersions = { 'htmlEditor': '4', 'ace/ace': '3', 'highcharts/highcharts': '2', 'mathJax/MathJax': '6' };
		Pub.ClientView = {};
		Pub.LoadHashScrollTop = true;
		Pub.Functions = {};
		Pub.Identity = 0;
		return Pub;
	}());
	AutoCSer.Pub = Pub;
	Pub.Alert = Pub.ThisFunction(window, alert);
	var IndexPoolNode = (function () {
		function IndexPoolNode() {
		}
		IndexPoolNode.prototype.Set = function (Value) {
			(this.Value = Value).PoolIdentity = ++this.Identity;
		};
		IndexPoolNode.prototype.Pop = function (Value) {
			if (Value.PoolIdentity === this.Identity) {
				this.Value = null;
				Value.PoolIdentity = 0;
				return true;
			}
			return false;
		};
		IndexPoolNode.prototype.Get = function (Identity) {
			return Identity === this.Identity ? this.Value : null;
		};
		return IndexPoolNode;
	}());
	var IndexPool = (function () {
		function IndexPool() {
		}
		IndexPool.Push = function (Value) {
			if (this.Indexs.length)
				this.Nodes[Value.PoolIndex = this.Indexs.pop()].Set(Value);
			else {
				var Node = new IndexPoolNode();
				Node.Identity = 0;
				Node.Set(Value);
				Value.PoolIndex = this.Nodes.length;
				this.Nodes.push(Node);
			}
		};
		IndexPool.Pop = function (Value) {
			this.Nodes[Value.PoolIndex];
			var Node = this.Nodes[Value.PoolIndex];
			if (Node && Node.Pop(Value))
				this.Indexs.push(Value.PoolIndex);
		};
		IndexPool.Get = function (Index, Identity) {
			var Node = this.Nodes[Index];
			return Node ? Node.Get(Identity) : null;
		};
		IndexPool.ToString = function (Value) {
			return 'AutoCSer.IndexPool.Get(' + Value.PoolIndex + ',' + Value.PoolIdentity + ')';
		};
		IndexPool.Nodes = [];
		IndexPool.Indexs = [];
		return IndexPool;
	}());
	AutoCSer.IndexPool = IndexPool;
	var LoadJs = (function () {
		function LoadJs(Script, OnLoad, OnError) {
			if (OnLoad === void 0) { OnLoad = null; }
			if (OnError === void 0) { OnError = null; }
			this.OnLoad = OnLoad;
			this.OnError = OnError;
			this.LoadFunction = Pub.ThisFunction(this, this.OnLoadJs);
			this.ErrorFunction = Pub.ThisFunction(this, this.OnErrorJs);
			(this.Script = Script).onload = this.LoadFunction;
			Script.onerror = this.ErrorFunction;
			AutoCSer.Loader.DocumentHead.appendChild(Script);
		}
		LoadJs.prototype.OnLoadJs = function (Event) {
			if (this.OnLoad)
				this.OnLoad(Event);
			AutoCSer.Loader.DocumentHead.removeChild(this.Script);
		};
		LoadJs.prototype.OnErrorJs = function (Event) {
			if (this.OnError)
				this.OnError(Event);
			AutoCSer.Loader.DocumentHead.removeChild(this.Script);
		};
		return LoadJs;
	}());
	AutoCSer.LoadJs = LoadJs;
	var Events = (function () {
		function Events(OnAdd, This) {
			if (OnAdd === void 0) { OnAdd = null; }
			if (This === void 0) { This = null; }
			this.OnAdd = This ? Pub.ThisFunction(This, OnAdd) : OnAdd;
			this.Functions = [];
			this.Function = Pub.ThisFunction(this, this.Call);
		}
		Events.prototype.Call = function () {
			for (var Argument = Pub.ToArray(arguments), Index = 0; Index - this.Functions.length; this.Functions[Index++].apply(null, Argument))
				;
		};
		Events.prototype.Add = function (Function) {
			if (Function && this.Functions.IndexOfValue(Function) < 0) {
				this.Functions.push(Function);
				if (this.OnAdd && this.OnAdd())
					Function();
			}
			return this;
		};
		Events.prototype.AddEvent = function (Event) {
			if (Event)
				for (var Index = 0, Functions = Event.Functions; Index - Functions.length; this.Add(Functions[Index++]))
					;
			return this;
		};
		Events.prototype.Remove = function (Function) {
			if (Function)
				this.Functions.RemoveAt(this.Functions.IndexOfValue(Function));
			return this;
		};
		Events.prototype.RemoveEvent = function (Event) {
			if (Event)
				for (var Index = 0, Functions = Event.Get(); Index - Functions.length; this.Remove(Functions[Index++]))
					;
			return this;
		};
		Events.prototype.Clear = function () {
			this.Functions.length = 0;
			return this;
		};
		Events.prototype.Get = function () {
			return this.Functions;
		};
		return Events;
	}());
	AutoCSer.Events = Events;
	Pub.OnQueryEvents = new Events();
	var HttpRequestQuery = (function () {
		function HttpRequestQuery(Url, Send, CallBack, IsVersion) {
			if (Send === void 0) { Send = null; }
			if (CallBack === void 0) { CallBack = null; }
			if (IsVersion === void 0) { IsVersion = false; }
			this.Url = Url;
			this.Send = Send;
			this.CallBack = CallBack;
			this.IsVersion = IsVersion;
		}
		HttpRequestQuery.prototype.ToQueryInfo = function () {
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
		};
		HttpRequestQuery.prototype.GetOnError = function (HttpRequest) {
			if (this.CallBack || HttpRequest) {
				this.ErrorRequest = HttpRequest;
				return Pub.ThisFunction(this, this.OnError);
			}
			return null;
		};
		HttpRequestQuery.prototype.OnError = function (Value) {
			if (this.ErrorRequest != null) {
				this.ErrorRequest.NextRequest();
				this.ErrorRequest = null;
			}
			if (this.CallBack != null) {
				Value.ErrorRequest = this;
				this.CallBack(Value);
			}
		};
		HttpRequestQuery.NullCallBack = function () { };
		return HttpRequestQuery;
	}());
	AutoCSer.HttpRequestQuery = HttpRequestQuery;
	var HttpRequestQueryInfo = (function (_super) {
		AutoCSer.Pub.Extends(HttpRequestQueryInfo, _super);
		function HttpRequestQueryInfo() {
			_super.apply(this, arguments);
		}
		return HttpRequestQueryInfo;
	}(HttpRequestQuery));
	var HttpRequest = (function () {
		function HttpRequest(OnResponse) {
			if (OnResponse === void 0) { OnResponse = null; }
			this.Requesting = false;
			this.WriteOrder = 0;
			this.ReadOrder = -1;
			this.Queue = [];
			this.OnResponse = new Events().Add(OnResponse);
			this.OnReadyStateChangeFunction = Pub.ThisFunction(this, this.OnReadyStateChange);
		}
		HttpRequest.prototype.Request = function (Request) {
			if (Request.Send && !Request.FormData) {
				Request.SendString = Pub.ToJson(Request.Send);
				if (Request.SendString === '{}')
					Request.SendString = '';
			}
			this.Queue[this.WriteOrder++] = Request;
			if (!this.Requesting) {
				this.Requesting = true;
				this.MakeXMLHttpRequest();
			}
		};
		HttpRequest.prototype.CallBack = function () {
			var Request = this.Queue[this.ReadOrder];
			this.NextRequest();
			if (Request.CallBack) {
				if (Request.IsOnLoad)
					Pub.OnLoad(Pub.ThisFunction(this, this.OnLoad, [Request.CallBack, Pub.ToArray(arguments)]), null, true);
				else
					Request.CallBack.apply(null, Pub.ToArray(arguments));
			}
		};
		HttpRequest.prototype.OnLoad = function (CallBack, Arguments) {
			CallBack.apply(null, Arguments);
		};
		HttpRequest.prototype.OnReadyStateChange = function (Event) {
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
							HttpRequest.Post('Pub.Error', { error: '服务器请求失败 : ' + location.toString() + '\r\n' + Query.Url + (Query.SendString && !Query.FormData ? ('\r\n' + Query.SendString.length + '\r\n' + Query.SendString.substring(0, 256)) : '') });
						}
					}
				}
				finally {
					IndexPool.Pop(Query);
					if (IsError && Query.CallBack)
						Query.CallBack({ Return: null, ErrorEvent: null, ErrorRequest: Query });
				}
			}
		};
		HttpRequest.prototype.NextRequest = function () {
			if (this.ReadOrder === this.WriteOrder - 1) {
				this.WriteOrder = 0;
				this.ReadOrder = -1;
				this.Requesting = false;
			}
			else
				this.MakeXMLHttpRequest();
		};
		HttpRequest.prototype.MakeXMLHttpRequest = function () {
			var Request = this.ReadXMLHttpRequest = HttpRequest.CreateRequest(), Info = this.Queue[++this.ReadOrder];
			var Url = Info.Url;
			if (Info.Method == null || Info.FormData)
				Info.Method = 'POST';
			if (Info.SendString && !Info.FormData) {
				if (Info.Method === 'GET') {
					Url += (Url.indexOf('?') + 1 ? '&' : '?') + 'j=' + Info.SendString.Escape();
					Info.SendString = null;
				}
				else
					Info.SendString = Info.SendString.replace(/\xA0/g, ' ');
			}
			if (Info.IsRandom)
				Url += (Url.indexOf('?') + 1 ? '&' : '?') + 't=' + (new Date).getTime();
			else if (Info.IsVersion && Info.Method === 'GET')
				Url += (Url.indexOf('?') + 1 ? '&' : '?') + 'v=' + AutoCSer.Loader.Version;
			Info.Request = Request;
			if (!Pub.IE && Info.Method === 'GET' && !Info.UserName && !Info.IsOnLoad) {
				Info.RetryCount = 2;
				Pub.AppendJs(Url, AutoCSer.Loader.Charset, null, (Pub.AjaxAppendJs = Info).GetOnError(this));
			}
			else {
				Request.onreadystatechange = this.OnReadyStateChangeFunction;
				if (Info.UserName == null || Info.UserName === '')
					Request.open(Info.Method, Url, true);
				else
					Request.open(Info.Method, Url, true, Info.UserName, Info.Password);
				if (Info.Header) {
					for (var Name in Info.Header)
						Request.setRequestHeader(Name, Info.Header[Name]);
				}
				else if (Info.Method === 'POST' && !Info.FormData)
					Request.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
				Request.send(Info.FormData || Info.SendString);
			}
		};
		HttpRequest.CreateRequest = function () {
			if (Pub.IE) {
				for (var Index = HttpRequest.MicrosoftXmlHttps.length; Index;) {
					try {
						return new ActiveXObject(HttpRequest.MicrosoftXmlHttps[--Index]);
					}
					catch (e) { }
				}
			}
			else if (window['XMLHttpRequest'])
				return new XMLHttpRequest;
			AutoCSer.Pub.Alert('你的浏览器不支持服务器请求,请升级您的浏览器！');
			return null;
		};
		HttpRequest.PostQuery = function (HttpRequestQuery) {
			HttpRequestQuery.Method = 'POST';
			var Query = HttpRequestQuery.ToQueryInfo(), Request = new HttpRequest;
			IndexPool.Push(Query);
			Query.Url = '/Ajax?n=' + Query.Url + '&c=' + IndexPool.ToString(Query).Escape() + '.CallBack';
			Request.Request(Query);
		};
		HttpRequest.Post = function (Url, Send, CallBack) {
			if (Send === void 0) { Send = null; }
			if (CallBack === void 0) { CallBack = null; }
			this.PostQuery(new HttpRequestQuery(Url, Send, CallBack));
		};
		HttpRequest.GetQuery = function (HttpRequestQuery) {
			HttpRequestQuery.Method = 'GET';
			if (!Pub.IE && !HttpRequestQuery.IsRandom) {
				var Query = HttpRequestQuery.ToQueryInfo();
				Query.IsRandom = false;
				Query.Url = '/Ajax?n=' + Query.Url + '&c=AutoCSer.Pub.AjaxCallBack';
				this.AjaxGetRequest.Request(Query);
				return;
			}
			var Query = HttpRequestQuery.ToQueryInfo();
			IndexPool.Push(Query);
			if (!Query.IsVersion)
				Query.IsRandom = true;
			Query.Url = '/Ajax?n=' + Query.Url + '&c=' + IndexPool.ToString(Query).Escape() + '.CallBack';
			(new HttpRequest).Request(Query);
		};
		HttpRequest.Get = function (Url, Send, CallBack, IsVersion) {
			if (Send === void 0) { Send = null; }
			if (CallBack === void 0) { CallBack = null; }
			if (IsVersion === void 0) { IsVersion = false; }
			this.GetQuery(new HttpRequestQuery(Url, Send, CallBack, IsVersion));
		};
		HttpRequest.CheckError = function (Value, ErrorInfo) {
			if (ErrorInfo === void 0) { ErrorInfo = '服务器请求失败，请稍后重试'; }
			if (Value.ErrorRequest) {
				if (ErrorInfo)
					AutoCSer.Pub.Alert(ErrorInfo);
				return false;
			}
			return true;
		};
		HttpRequest.Load = function () {
			if (Pub.IE)
				this.MicrosoftXmlHttps = ['Microsoft.XMLHTTP', 'Msxml2.XMLHTTP'];
			Pub.AjaxCallBack = Pub.ThisFunction(this.AjaxGetRequest = new HttpRequest, this.AjaxGetRequest.CallBack);
		};
		HttpRequest.ErrorPath = '/Ajax?n=Pub.Error&';
		return HttpRequest;
	}());
	AutoCSer.HttpRequest = HttpRequest;
	var HtmlElement = (function () {
		function HtmlElement(Value, Parent) {
			if (typeof (Value) == 'string') {
				this.FilterString = Value;
				this.Parent = Parent ? (Parent instanceof HtmlElement ? Parent.Element0() : Parent) : document.body;
			}
			else if (Value) {
				if (Value instanceof HtmlElement) {
					this.FilterString = Value.FilterString;
					this.Parent = Value.Parent;
					this.Elements = Value.Elements;
				}
				else
					this.Elements = Value instanceof Array ? Value : [Value];
			}
			else
				this.Elements = [];
		}
		HtmlElement.prototype.IsParent = function (Element) {
			return !this.Parent || (Element && HtmlElement.$IsParent(Element, this.Parent));
		};
		HtmlElement.prototype.FilterId = function () {
			var Id = this.FilterValue();
			this.FilterBuilder.push('function(Element,Value){if(Element==this.Parent?Value.IsParent(Element=document.getElementById("');
			this.FilterBuilder.push(Id);
			this.FilterBuilder.push('")):Element.id=="');
			this.FilterBuilder.push(Id);
			this.FilterBuilder.push('")(');
			this.FilterNext(true);
		};
		HtmlElement.prototype.FilterChildTag = function () {
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
		};
		HtmlElement.prototype.FilterTag = function () {
			var Name = this.FilterValue();
			if (Name)
				this.FilterChildren('tagName', Name);
			else {
				this.FilterChildren();
				this.FilterBuilder.push('if(Element=Childs[Index++])(');
				this.FilterNext(true);
			}
		};
		HtmlElement.prototype.FilterChildren = function (Name, Value) {
			if (Name === void 0) { Name = null; }
			if (Value === void 0) { Value = ''; }
			this.FilterBuilder.push('function(Element,Value){var Elements=[],ElementIndex=-1;while(ElementIndex-Elements.length)for(var Childs=ElementIndex+1?Elements[ElementIndex++].childNodes:[arguments[++ElementIndex]],Index=0;Index-Childs.length;Elements.push(Element))');
			if (Name) {
				if (!Value)
					Value = this.FilterValue();
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
		};
		HtmlElement.prototype.FilterClass = function () {
			this.FilterChildren();
			this.FilterBuilder.push('if((Element=Childs[Index++]).className&&Element.className.toString().split(" ").IndexOfValue("');
			this.FilterBuilder.push(this.FilterValue());
			this.FilterBuilder.push('")+1)(');
			this.FilterNext(true);
		};
		HtmlElement.prototype.FilterAttribute = function () {
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
		};
		HtmlElement.prototype.FilterName = function () {
			this.FilterChildren();
			this.FilterBuilder.push('if(AutoCSer.HtmlElement.$Attribute(Element=Childs[Index++],"name")=="');
			this.FilterBuilder.push(this.FilterValue());
			this.FilterBuilder.push('")(');
			this.FilterNext(true);
		};
		HtmlElement.prototype.FilterCss = function () {
			this.FilterChildren();
			var Value = this.FilterValue().split('=');
			this.FilterBuilder.push('if(AutoCSer.HtmlElement.$GetStyle(Element=Childs[Index++],"');
			this.FilterBuilder.push(Value[0]);
			this.FilterBuilder.push('")=="');
			this.FilterBuilder.push(Value[1]);
			this.FilterBuilder.push('")(');
			this.FilterNext(true);
		};
		HtmlElement.prototype.FilterValue = function () {
			var Index = this.FilterIndex;
			while (this.FilterIndex !== this.FilterString.length && !this.FilterCreator[this.FilterString.charCodeAt(this.FilterIndex)])
				++this.FilterIndex;
			return this.FilterString.substring(Index, this.FilterIndex);
		};
		HtmlElement.prototype.FilterNext = function (IsEnd) {
			if (this.FilterIndex !== this.FilterString.length) {
				var Creator = this.FilterCreator[this.FilterString.charCodeAt(this.FilterIndex)];
				if (Creator) {
					++this.FilterIndex;
					Creator();
				}
				else
					this.FilterTag();
				if (IsEnd)
					this.FilterBuilder.push(')(Element,Value);}');
			}
			else if (this.FilterBuilder.length)
				this.FilterBuilder.push('Value.Elements.push)(Element);}');
		};
		HtmlElement.prototype.GetElements = function () {
			if (this.Elements)
				return this.Elements;
			if (!this.Filter) {
				var Filter = this.FilterString ? HtmlElement.FilterCache[this.FilterString] : HtmlElement.NullFilter;
				if (!Filter) {
					this.FilterIndex = 0;
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
		};
		HtmlElement.NullFilter = function (Parent, HtmlElement) { };
		HtmlElement.prototype.Element0 = function () {
			return this.GetElements()[0];
		};
		HtmlElement.prototype.AddEvent = function (Name, Value, IsStop) {
			if (IsStop === void 0) { IsStop = !Pub.IE; }
			return this.Event(Name, Value, IsStop, '$AddEvent');
		};
		HtmlElement.prototype.DeleteEvent = function (Name, Value, IsStop) {
			if (IsStop === void 0) { IsStop = !Pub.IE; }
			return this.Event(Name, Value, IsStop, '$DeleteEvent');
		};
		HtmlElement.prototype.Event = function (Name, Value, IsStop, CallName) {
			var Elements = this.GetElements();
			if (Elements.length) {
				var Names = Name.split(',');
				for (var Index = 0; Index - Elements.length; HtmlElement[CallName](Elements[Index++], Names, Value, IsStop))
					;
			}
			return this;
		};
		HtmlElement.prototype.Value0 = function () {
			var Elements = this.GetElements();
			return Elements.length ? HtmlElement.$GetValue(Elements[0]) : null;
		};
		HtmlElement.prototype.Value = function (Value) {
			for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetValue(Elements[Index++], Value))
				;
			return this;
		};
		HtmlElement.prototype.Get0 = function (Name, Value) {
			if (Value === void 0) { Value = null; }
			var Elements = this.GetElements();
			return Elements.length ? Elements[0][Name] : Value;
		};
		HtmlElement.prototype.Id0 = function () {
			return this.Get0('id');
		};
		HtmlElement.prototype.Html0 = function () {
			return this.Get0('innerHTML');
		};
		HtmlElement.prototype.TagName0 = function () {
			return this.Get0('tagName');
		};
		HtmlElement.prototype.ScrollHeight0 = function () {
			return this.Get0('scrollHeight', 0);
		};
		HtmlElement.prototype.Attribute0 = function (Name) {
			var Elements = this.GetElements();
			if (Elements.length)
				return HtmlElement.$Attribute(Elements[0], Name);
		};
		HtmlElement.prototype.Name0 = function () {
			return this.Attribute0('name');
		};
		HtmlElement.prototype.GetCall0 = function (CallName) {
			var Elements = this.GetElements();
			if (Elements.length)
				return HtmlElement[CallName](Elements[0]);
		};
		HtmlElement.prototype.Text0 = function () {
			return this.GetCall0('$GetText');
		};
		HtmlElement.prototype.Text = function (Text) {
			for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetText(Elements[Index++], Text))
				;
			return this;
		};
		HtmlElement.prototype.Css0 = function () {
			return this.GetCall0('$Css');
		};
		HtmlElement.prototype.Width0 = function () {
			return this.GetCall0('$Width');
		};
		HtmlElement.prototype.Height0 = function () {
			return this.GetCall0('$Height');
		};
		HtmlElement.prototype.Opacity0 = function () {
			return this.GetCall0('$GetOpacity');
		};
		HtmlElement.prototype.XY0 = function () {
			return this.GetCall0('$XY');
		};
		HtmlElement.prototype.Style0 = function (Name) {
			var Css = this.Css0();
			return Css ? Css[Name] : null;
		};
		HtmlElement.prototype.Parent0 = function () {
			var Elements = this.GetElements();
			return Elements.length ? HtmlElement.$(HtmlElement.$ParentElement(Elements[0])) : null;
		};
		HtmlElement.prototype.Next0 = function () {
			var Elements = this.GetElements();
			return Elements.length ? HtmlElement.$(HtmlElement.$NextElement(Elements[0])) : null;
		};
		HtmlElement.prototype.Previous0 = function () {
			var Elements = this.GetElements();
			return Elements.length ? HtmlElement.$(HtmlElement.$PreviousElement(Elements[0])) : null;
		};
		HtmlElement.prototype.Replace0 = function (Element) {
			var Elements = this.GetElements();
			if (Elements.length)
				HtmlElement.$ParentElement(Elements[0]).replaceChild(Element, Elements[0]);
			return this;
		};
		HtmlElement.prototype.Set = function (Name, Value) {
			for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; Elements[Index++][Name] = Value)
				;
			return this;
		};
		HtmlElement.prototype.Html = function (Html, IsToHtml) {
			if (IsToHtml === void 0) { IsToHtml = false; }
			return this.Set('innerHTML', IsToHtml ? Html.ToHTML() : Html);
		};
		HtmlElement.prototype.To = function (Parent) {
			if (Parent === void 0) { Parent = document.body; }
			var Elements = this.GetElements();
			if (Elements.length) {
				if (Parent instanceof HtmlElement)
					Parent = Parent.Element0();
				for (var Index = -1; ++Index - Elements.length;) {
					if (HtmlElement.$ParentElement(Elements[Index]) != Parent)
						Parent.appendChild(Elements[Index]);
				}
			}
			return this;
		};
		HtmlElement.prototype.Child = function () {
			for (var Nodes = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; ++Index) {
				var Childs = Elements[Index].childNodes;
				if (Childs)
					for (var NodeIndex = 0; NodeIndex !== Childs.length; Nodes.push(Childs[NodeIndex++]))
						;
			}
			return new HtmlElement(Nodes, null);
		};
		HtmlElement.prototype.Childs = function (IsChild) {
			if (IsChild === void 0) { IsChild = null; }
			for (var Value = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; ++Index) {
				var Values = HtmlElement.$ChildElements(Elements[Index], IsChild);
				if (Values)
					Value.push(Values);
			}
			return HtmlElement.$(Value.concat.apply([], Value));
		};
		HtmlElement.prototype.InsertBefore = function (Element, Parent) {
			if (Parent === void 0) { Parent = null; }
			if (!Parent)
				Parent = HtmlElement.$ParentElement(Element);
			for (var Elements = this.GetElements(), Index = 0; Index !== Elements.length; Parent.insertBefore(Elements[Index++], Element))
				;
			return this;
		};
		HtmlElement.prototype.Delete = function () {
			for (var Elements = this.GetElements(), Index = 0; Index !== Elements.length; HtmlElement.$Delete(Elements[Index++]))
				;
			return this;
		};
		HtmlElement.prototype.AddClass = function (Name) {
			return this.Class(Name, '$AddClass');
		};
		HtmlElement.prototype.RemoveClass = function (Name) {
			return this.Class(Name, '$RemoveClass');
		};
		HtmlElement.prototype.Class = function (Name, CallName) {
			if (Name) {
				var Elements = this.GetElements();
				if (Elements.length) {
					for (var Index = 0, Names = Name.split(' '); Index !== Elements.length; HtmlElement[CallName](Elements[Index++], Names))
						;
				}
			}
			return this;
		};
		HtmlElement.prototype.Style = function (Name, Value) {
			for (var Elements = this.GetElements(), Index = Elements.length; Index; Elements[--Index].style[Name] = Value)
				;
			return this;
		};
		HtmlElement.prototype.Styles = function (Name, Value) {
			var Elements = this.GetElements();
			if (Elements.length) {
				for (var ElementIndex = 0, Names = Name.split(','); ElementIndex - Elements.length;) {
					for (var Element = Elements[ElementIndex++], Index = 0; Index - Names.length; Element.style[Names[Index++]] = Value)
						;
				}
			}
			return this;
		};
		HtmlElement.prototype.Display = function (IsShow) {
			return this.Style('display', typeof (IsShow) == 'string' ? IsShow : (IsShow ? '' : 'none'));
		};
		HtmlElement.prototype.Disabled = function (Value) {
			return this.Style('disabled', Value);
		};
		HtmlElement.prototype.Left = function (Value) {
			return this.Style('left', Value + 'px');
		};
		HtmlElement.prototype.Top = function (Value) {
			return this.Style('top', Value + 'px');
		};
		HtmlElement.prototype.ToXY = function (Left, Top) {
			var Elements = this.GetElements();
			if (Elements.length) {
				for (var Index = Elements.length; Index; HtmlElement.$ToXY(Elements[--Index], Left, Top))
					;
			}
			return this;
		};
		HtmlElement.prototype.Cursor = function (Value) {
			return this.Style('cursor', Value);
		};
		HtmlElement.prototype.Opacity = function (Value) {
			for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetOpacity(Elements[Index++], Value))
				;
			return this;
		};
		HtmlElement.prototype.FirstElement = function (IsValue) {
			return this.GetElements().First(IsValue);
		};
		HtmlElement.prototype.CssArray = function (Name) {
			for (var Value = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; Value.push(HtmlElement.$Css(Elements[Index++])[Name]))
				;
			return Value;
		};
		HtmlElement.prototype.TopIndex = function () {
			if (this.CssArray('zIndex').MaxValue() != HtmlElement.ZIndex && this.Elements.length)
				this.Style('zIndex', ++HtmlElement.ZIndex);
			return this;
		};
		HtmlElement.prototype.Focus0 = function () {
			var Elements = this.GetElements();
			if (Elements.length)
				Elements[0].focus();
			return this;
		};
		HtmlElement.prototype.Blur0 = function () {
			var Elements = this.GetElements();
			if (Elements.length)
				Elements[0].blur();
			return this;
		};
		HtmlElement.prototype.ChangeBool = function (Name) {
			for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; ++Index)
				Elements[Index][Name] = !Elements[Index][Name];
			return this;
		};
		HtmlElement.prototype.ScrollTop0 = function () {
			var Elements = this.GetElements();
			if (Elements.length)
				HtmlElement.$SetScrollTop(HtmlElement.$XY(Elements[0]).Top);
			return this;
		};
		HtmlElement.$ = function (Value, Parent) {
			if (Parent === void 0) { Parent = null; }
			return new HtmlElement(Value, Parent);
		};
		HtmlElement.$Id = function (Id) {
			return this.$(document.getElementById(Id));
		};
		HtmlElement.$IdElement = function (Id) {
			return document.getElementById(Id);
		};
		HtmlElement.$AddEvent = function (Element, Names, Value, IsStop) {
			if (IsStop === void 0) { IsStop = !Pub.IE; }
			var IsBody = Pub.IE && (Element == document.body || (Element.tagName && Element.tagName.toLowerCase() == 'body'));
			this.$DeleteEvent(Element, Names, Value, IsStop);
			for (var Index = Names.length; --Index >= 0;) {
				var Name = Names[Index].toLowerCase();
				if (Pub.IE) {
					if (Name.substring(0, 2) != 'on')
						Name = 'on' + Name;
					if (IsBody && Name == 'onfocus')
						Name = 'onfocusin';
					Element['attachEvent'](Name, Value);
				}
				else {
					if (Name.substring(0, 2) === 'on')
						Name = Name.substring(2);
					Element.addEventListener(Name, Value, IsStop);
				}
			}
		};
		HtmlElement.$DeleteEvent = function (Element, Names, Value, IsStop) {
			if (IsStop === void 0) { IsStop = !Pub.IE; }
			var IsBody = Pub.IE && (Element == document.body || (Element.tagName && Element.tagName.toLowerCase() == 'body'));
			for (var Index = Names.length; --Index >= 0;) {
				var Name = Names[Index].toLowerCase();
				if (Pub.IE) {
					if (Name.substring(0, 2) != 'on')
						Name = 'on' + Name;
					if (IsBody && Name == 'onfocus')
						Name = 'onfocusin';
					Element['detachEvent'](Name, Value);
				}
				else {
					if (Name.substring(0, 2) === 'on')
						Name = Name.substring(2);
					Element.removeEventListener(Name, Value, IsStop);
				}
			}
		};
		HtmlElement.$IsParent = function (Element, Parent) {
			while (Element != Parent && Element)
				Element = (Pub.IE ? Element.parentElement || Element.parentNode : Element.parentNode);
			return Element != null;
		};
		HtmlElement.$ParentElement = function (Element) {
			return (Pub.IE ? Element.parentElement || Element.parentNode : Element.parentNode);
		};
		HtmlElement.$NextElement = function (Element) {
			return Element ? Element.nextSibling : null;
		};
		HtmlElement.$PreviousElement = function (Element) {
			return Element ? Element.previousSibling : null;
		};
		HtmlElement.$Delete = function (Element, Parent) {
			if (Parent === void 0) { Parent = null; }
			if (Element != null)
				(Parent || HtmlElement.$ParentElement(Element)).removeChild(Element);
		};
		HtmlElement.$ChildElements = function (Element, IsElement) {
			if (IsElement === void 0) { IsElement = null; }
			var Value = [], Elements = [Element], ElementIndex = 0;
			while (ElementIndex < Elements.length) {
				for (var Childs = Elements[ElementIndex++].childNodes, Index = -1; ++Index - Childs.length;) {
					if (IsElement == null || IsElement(Childs[Index]))
						Value.push(Childs[Index]);
					Elements.push(Childs[Index]);
				}
			}
			return Value;
		};
		HtmlElement.$Create = function (TagName, Document) {
			if (Document === void 0) { Document = document; }
			return this.$(Document.createElement(TagName));
		};
		HtmlElement.$CreateElement = function (TagName, Document) {
			if (Document === void 0) { Document = document; }
			return Document.createElement(TagName);
		};
		HtmlElement.$GetValueById = function (Id) {
			return this.$GetValue(this.$IdElement(Id));
		};
		HtmlElement.$GetValue = function (Element) {
			if (Element) {
				if (Element.tagName.toLowerCase() === 'select') {
					if (Element.selectedIndex >= 0)
						return Element.options[Element.selectedIndex].value;
					return null;
				}
				return Element.value;
			}
		};
		HtmlElement.$SetValue = function (Element, Value) {
			if (Element) {
				if (Element.tagName.toLowerCase() == 'select') {
					for (var Index = Element.options.length; Index;) {
						if (Element.options[--Index].value == Value)
							break;
					}
					Element.selectedIndex = Index;
				}
				else
					Element.value = Value;
			}
		};
		HtmlElement.$SetValueById = function (Id, Value) {
			var Element = this.$IdElement(Id);
			this.$SetValue(Element, Value);
			return Element;
		};
		HtmlElement.$IntById = function (Id, DefaultValue) {
			if (DefaultValue === void 0) { DefaultValue = null; }
			var Value = this.$GetValueById(Id);
			return Value ? parseInt(Value, 10) : (DefaultValue || 0);
		};
		HtmlElement.$FloatById = function (Id, DefaultValue) {
			if (DefaultValue === void 0) { DefaultValue = null; }
			var Value = this.$GetValueById(Id);
			return Value ? parseFloat(Value) : (DefaultValue || 0);
		};
		HtmlElement.$GetCheckedById = function (Id) {
			var Element = this.$IdElement(Id);
			return Element && Element['checked'];
		};
		HtmlElement.$SetCheckedById = function (Id, Checked) {
			var Element = this.$IdElement(Id);
			if (Element)
				Element['checked'] = Checked;
			return Element;
		};
		HtmlElement.$GetText = function (Element) {
			return Pub.IE ? Element.nodeType == 3 ? Element.nodeValue : Element.innerText : Element.textContent;
		};
		HtmlElement.$SetText = function (Element, Text) {
			if (Pub.IE) {
				if (Element.nodeType == 3)
					Element.nodeValue = Text;
				else
					Element.innerText = Text;
			}
			else
				Element.textContent = Text;
			return Element;
		};
		HtmlElement.$AddClass = function (Element, Names) {
			if (Names) {
				if (Element.classList) {
					for (var Index = Names.length; Index; Element.classList.add(Names[--Index]))
						;
				}
				else {
					var OldName = Element.className;
					if (OldName) {
						for (var Index = Names.length, OldNames = OldName.split(' '); --Index >= 0;)
							if (OldNames.IndexOfValue(Names[Index]) < 0)
								OldNames.push(Names[Index]);
						Names = OldNames;
					}
					Element.className = Names.join(' ');
				}
			}
		};
		HtmlElement.$RemoveClass = function (Element, Names) {
			if (Names) {
				if (Element.classList) {
					for (var Index = Names.length; Index; Element.classList.remove(Names[--Index]))
						;
				}
				else {
					var OldName = Element.className;
					if (OldName) {
						for (var Index = Names.length, OldNames = OldName.split(' '); Index; OldNames.RemoveAtEnd(OldNames.IndexOfValue(Names[--Index])))
							;
						Element.className = OldNames.length ? OldNames.join(' ') : '';
					}
				}
			}
		};
		HtmlElement.$Css = function (Element) {
			return Pub.IE ? Element['currentStyle'] : document.defaultView.getComputedStyle(Element);
		};
		HtmlElement.$GetStyle = function (Element, Name) {
			var Css = this.$Css(Element);
			return Css ? Css[Name] : null;
		};
		HtmlElement.$SetStyle = function (Element, Name, Value) {
			Element.style[Name] = Value;
		};
		HtmlElement.$AttributeOrStyle = function (Element, Name) {
			return this.$Attribute(Element, Name) || this.$GetStyle(Element, Name);
		};
		HtmlElement.$Attribute = function (Element, Name) {
			var Value = Element[Name];
			return Value == undefined && Element.attributes && (Value = Element.attributes[Name]) ? Value.value : Value;
		};
		HtmlElement.$IsAttribute = function (Element, Name) {
			return Element[Name] !== undefined || (Element.attributes != null && Element.attributes[Name] !== undefined);
		};
		HtmlElement.$ElementName = function (Element, Name, Value) {
			if (Value === void 0) { Value = null; }
			if (Value == null) {
				while (Element && HtmlElement.$Attribute(Element, Name) == null)
					Element = HtmlElement.$ParentElement(Element);
			}
			else
				while (Element && HtmlElement.$Attribute(Element, Name) != Value)
					Element = HtmlElement.$ParentElement(Element);
			return Element;
		};
		HtmlElement.$Transform_matrix = function (a, b, c, d, Left, Top) {
			return { Left: Left, Top: Top };
		};
		HtmlElement.$XY = function (Element) {
			for (var Left = 0, Top = 0; Element != null && Element != document.body; Element = Element.offsetParent) {
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
						if (XY.Left)
							Left += XY.Left;
						if (XY.Top)
							Top += XY.Top;
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
			return { Left: Left, Top: Top };
		};
		HtmlElement.$Transform = function (Transform) {
			if (Transform && Transform.indexOf('matrix(') != -1)
				return eval('HtmlElement.$Transform_' + Transform);
		};
		HtmlElement.$ToXY = function (Element, Left, Top) {
			var Value = this.$XY(Element['offsetParent']);
			Element.style.left = (Left - Value.Left) + 'px';
			Element.style.top = (Top - Value.Top) + 'px';
			return Element;
		};
		HtmlElement.$Width = function (Element) {
			if (Element === void 0) { Element = null; }
			if (Element == null)
				return window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
			return this.$Offset(Element, 'offsetWidth');
		};
		HtmlElement.$Height = function (Element) {
			if (Element === void 0) { Element = null; }
			if (Element == null)
				return window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
			return this.$Offset(Element, 'offsetHeight');
		};
		HtmlElement.$Offset = function (Element, Name) {
			while (Element) {
				var Value = Element[Name];
				if (Value != null)
					return Value;
				var Elements = Element.children;
				if (Elements == null)
					return 0;
				Element = Elements[0];
			}
			return 0;
		};
		HtmlElement.$GetScrollLeft = function () {
			return Math.max(document.body.scrollLeft, document.documentElement.scrollLeft);
		};
		HtmlElement.$SetScrollLeft = function (Left) {
			return document.body.scrollLeft = document.documentElement.scrollLeft = Left;
		};
		HtmlElement.$GetScrollTop = function () {
			return Math.max(document.body.scrollTop, document.documentElement.scrollTop);
		};
		HtmlElement.$SetScrollTop = function (Top) {
			document.body.scrollTop = document.documentElement.scrollTop = Top;
		};
		HtmlElement.$GetScrollHeight = function () {
			return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
		};
		HtmlElement.$GetScrollWidth = function () {
			return Math.max(document.body.scrollWidth, document.documentElement.scrollWidth);
		};
		HtmlElement.$ScrollTopById = function (Id) {
			var Element = this.$IdElement(Id);
			if (Element)
				this.$SetScrollTop(this.$XY(Element).Top);
		};
		HtmlElement.$SetOpacity = function (Element, Value) {
			if (Pub.IE)
				Element.style.filter = 'alpha(opacity=' + Value + ')';
			else
				Element.style.opacity = Element.style['MozOpacity'] = (Value / 100).toString();
		};
		HtmlElement.$GetOpacity = function (Element) {
			if (Pub.IE)
				return Element.style.filter['alphas'].opacity;
			var Value = this.$Css(Element).opacity;
			return Value ? parseFloat(Value) * 100 : null;
		};
		HtmlElement.$Name = function (Value, Element) {
			if (Element === void 0) { Element = null; }
			if (Pub.IE)
				return HtmlElement.$(HtmlElement.$ChildElements(Element || document.body, function (Element) { return HtmlElement.$Attribute(Element, 'name') == Value; }));
			return HtmlElement.$(Element ? HtmlElement.$ChildElements(Element, function (Element) { return HtmlElement.$Attribute(Element, 'name') == Value; }) : Pub.ToArray(document.getElementsByName(Value)));
		};
		HtmlElement.$Hash = function (Location) {
			if (Location === void 0) { Location = location; }
			return Location.hash.toString().replace(/^#(\!|\%21)?/g, '');
		};
		HtmlElement.$Paste = function (Element, Text, IsAll) {
			if (Pub.IE) {
				var Selection = Element['document'].selection.createRange();
				if (IsAll && Selection.text == '')
					Element.value = Text;
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
		};
		HtmlElement.$SelectOption = function (Id) {
			var Element = this.$IdElement(Id);
			if (Element && Element.selectedIndex >= 0)
				return Element.options[Element.selectedIndex];
		};
		HtmlElement.FilterCache = {};
		HtmlElement.OverZIndex = 100;
		HtmlElement.ZIndex = 10000;
		return HtmlElement;
	}());
	AutoCSer.HtmlElement = HtmlElement;
	var SkinReShowType;
	(function (SkinReShowType) {
		SkinReShowType[SkinReShowType["None"] = 0] = "None";
		SkinReShowType[SkinReShowType["ReShow"] = 1] = "ReShow";
		SkinReShowType[SkinReShowType["PushArray"] = 2] = "PushArray";
		SkinReShowType[SkinReShowType["PushArrayExpand"] = 3] = "PushArrayExpand";
		SkinReShowType[SkinReShowType["RemoveArray"] = 4] = "RemoveArray";
	})(SkinReShowType || (SkinReShowType = {}));
	var SkinViewNode = (function () {
		function SkinViewNode(Node, Parent, Data) {
			if (Data === void 0) { Data = null; }
			this.Parent = Parent;
			this.Node = Node;
			this.ReShowType = SkinReShowType.None;
			if (Data)
				this.SetView(Data);
		}
		SkinViewNode.prototype.CreateView = function () {
			var Expressions = this.Node.Expressions;
			if (Expressions) {
				this.Views = [];
				for (var Index = 0; Index !== Expressions.length; this.Views.push(Expressions[Index++].CreateView(this)))
					;
			}
			else
				this.SetView(this.Node.Skin.Datas[this.Node.Skin.Datas.length - 1]);
		};
		SkinViewNode.prototype.SetView = function (Data) {
			this.Views = [new SkinView([Data])];
			this.SetData(Data);
		};
		SkinViewNode.prototype.SetData = function (Data) {
			if (Data.$Nodes)
				Data.$Nodes.push(this);
			else
				Data.$Nodes = [this];
		};
		SkinViewNode.prototype.Create = function () {
			this.SkinNoMark = this.Node.Skin.NoMark;
			this.SkinNoOutput = this.Node.Skin.NoOutput;
			switch (this.Node.TypeIndex) {
				case 0:
					this.Mark(true);
					this.CreateNodes();
					this.Mark(false);
					break;
				case 1:
					this.CreateIf();
					break;
				case 2:
					this.CreateNot();
					break;
				case 3:
					this.CreateLoop();
					break;
				case 4:
					this.CreateValue();
					break;
				case 5:
					this.CreateNoMark();
					break;
				case 6:
					SkinViewNode.CreateHtml(this.Node);
					break;
				case 7:
					this.CreateAt();
					break;
			}
		};
		SkinViewNode.prototype.GetMarkId = function (IsStart) {
			return '_' + this.Identity + (IsStart ? '_MARKSTART_' : '_MARKEND_');
		};
		SkinViewNode.DeleteMark = function (Span) {
			var Id = Span.id;
			if (Id && Id.length > 10) {
				if (Id.substring(Id.length - 9) == '_MARKEND_' || Id.substring(Id.length - 11) == '_MARKSTART_')
					HtmlElement.$Delete(Span);
			}
		};
		SkinViewNode.prototype.Mark = function (IsStart) {
			var Skin = this.Node.Skin;
			if ((Skin.NoOutput | Skin.NoMark) === 0 && !this.NoMark) {
				if (!this.Identity)
					this.Identity = ++SkinViewNode.NodeIdentity;
				Skin.Htmls.push('<span id="' + this.GetMarkId(IsStart) + '" style="display:none"></span>');
			}
		};
		SkinViewNode.prototype.CreateNodes = function () {
			var Nodes = this.Node.Nodes;
			if (Nodes)
				for (var Index = 0; Index !== Nodes.length; ++Index) {
					var Node = Nodes[Index];
					if (Node.TypeIndex == 6)
						SkinViewNode.CreateHtml(Node);
					else
						new SkinViewNode(Node, this).Create();
				}
		};
		SkinViewNode.prototype.CreateIf = function () {
			this.CreateView();
			this.CreateLogic(this.CheckLogic());
		};
		SkinViewNode.prototype.CreateNot = function () {
			this.CreateView();
			this.CreateLogic(!this.CheckLogic());
		};
		SkinViewNode.prototype.CreateLogic = function (IsOutput) {
			this.Mark(true);
			if (!IsOutput)
				++this.Node.Skin.NoOutput;
			this.CreateNodes();
			if (!IsOutput)
				--this.Node.Skin.NoOutput;
			this.Mark(false);
		};
		SkinViewNode.prototype.CheckLogic = function () {
			for (var Index = this.Views.length; Index;) {
				var Expression = this.Node.GetExpression(--Index), Value = this.Views[Index].CheckLogic(Expression);
				if (Expression.IsNot ? !Value : Value) {
					if (this.Node.IsOrExpression)
						return true;
				}
				else if (!this.Node.IsOrExpression)
					return false;
			}
			return !this.Node.IsOrExpression;
		};
		SkinViewNode.prototype.CreateLoop = function () {
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
				if (this.LoopNodes)
					this.LoopNodes.length = 0;
				else
					this.LoopNodes = [];
				this.CreateLoopOnly();
			}
			this.Mark(false);
		};
		SkinViewNode.prototype.CreateLoopOnly = function () {
			var Data = this.Views[0].GetData();
			if (Data) {
				var Value = Data.$Data;
				if (Value instanceof Array && Value.length) {
					this.Node.Skin.Datas.push(Data);
					if (!this.LoopNodes)
						this.LoopNodes = [];
					for (var Index = this.LoopNodes.length; Index < Value.length; ++Index) {
						var Node = new SkinViewNode(this.Node, this, Data.$Get(Index));
						Node.IsLoopValue = true;
						this.LoopNodes.push(Node);
						Node.CreateLoop();
					}
					this.Node.Skin.Datas.pop();
				}
			}
		};
		SkinViewNode.prototype.CreateLoopExpand = function () {
			var Data = this.Views[0].GetData();
			if (Data) {
				var Value = Data.$Data;
				if (Value instanceof Array && Value.length) {
					this.Node.Skin.Datas.push(Data);
					if (!this.LoopNodes)
						this.LoopNodes = [];
					for (var Index = 0; Index != Value.length; ++Index) {
						if (this.LoopNodes[Index])
							break;
						var Node = new SkinViewNode(this.Node, this, Data.$Get(Index));
						Node.IsLoopValue = true;
						this.LoopNodes[Index] = Node;
						Node.CreateLoop();
					}
					this.Node.Skin.Datas.pop();
				}
			}
		};
		SkinViewNode.prototype.CreateValue = function () {
			this.CreateView();
			this.Mark(true);
			var Data = this.Views[0].GetData();
			if (Data && Data.$Data != null) {
				this.Node.Skin.Datas.push(Data);
				this.CreateNodes();
				this.Node.Skin.Datas.pop();
			}
			this.Mark(false);
		};
		SkinViewNode.prototype.CreateNoMark = function () {
			++this.Node.Skin.NoMark;
			this.CreateNodes();
			--this.Node.Skin.NoMark;
		};
		SkinViewNode.CreateHtml = function (Node) {
			if (Node.Skin.NoOutput === 0)
				Node.Skin.Htmls.push(Node.Html);
		};
		SkinViewNode.prototype.CreateAt = function () {
			this.CreateView();
			if (this.Node.Skin.NoOutput === 0) {
				if (this.Node.IsIdentity) {
					if (!this.Identity)
						this.Identity = ++SkinViewNode.NodeIdentity;
					this.Node.Skin.Htmls.push(this.Identity.toString());
					if (this.Parent != null && !this.IsSetSearchNode)
						this.Parent.SetSearchNode(this);
				}
				else if (this.Node.IsLoopIndex)
					this.Node.Skin.Htmls.push(this.GetLoopIndex().toString());
				else {
					if (!this.Node.NoMarkAt)
						this.Mark(true);
					var Data = this.Views[0].GetData();
					if (Data && Data.$Data != null) {
						var Value = Data.$Data.toString();
						this.Node.Skin.Htmls.push(this.Node.IsHtml ? Value.ToHTML() : (this.Node.IsTextArea ? Value.ToTextArea() : Value));
					}
					if (!this.Node.NoMarkAt)
						this.Mark(false);
				}
			}
		};
		SkinViewNode.prototype.GetLoopIndex = function () {
			if (this.Node.TypeIndex == 3 && this.IsLoopValue) {
				var Data = this.Views[0].GetData();
				return Data ? Data.$Name : -1;
			}
			return this.Parent ? this.Parent.GetLoopIndex() : -1;
		};
		SkinViewNode.prototype.SetSearchNode = function (Node) {
			Node.IsSetSearchNode = true;
			if (this.SearchNodes)
				this.SearchNodes.push(Node);
			else
				this.SearchNodes = [Node];
			if (this.Parent != null && !this.IsSetSearchNode)
				this.Parent.SetSearchNode(this);
		};
		SkinViewNode.prototype.SearchNode = function (Identity) {
			if (this.Identity == Identity)
				return this;
			if (this.SearchNodes) {
				for (var Index = this.SearchNodes.length; Index;) {
					var Node = this.SearchNodes[--Index].SearchNode(Identity);
					if (Node)
						return Node;
				}
			}
		};
		SkinViewNode.prototype.ClearSearchNode = function (IsStart) {
			if (!IsStart)
				this.IsSetSearchNode = false;
			if (this.SearchNodes) {
				for (var Index = this.SearchNodes.length; Index; this.SearchNodes[--Index].ClearSearchNode(false))
					;
				this.SearchNodes.length = 0;
			}
		};
		SkinViewNode.prototype.TryShow = function (Type) {
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
					else if (this.ReShowType != Type)
						this.ReShowType = SkinReShowType.ReShow;
				}
				else if (this.Parent)
					this.Parent.TryShow(SkinReShowType.ReShow);
			}
		};
		SkinViewNode.prototype.ReShow = function () {
			var MarkStart = HtmlElement.$IdElement(this.GetMarkId(true)), ReShowType = this.ReShowType;
			this.ReShowType = SkinReShowType.None;
			if (MarkStart) {
				var MarkEndId = this.GetMarkId(false), MarkEnd = HtmlElement.$IdElement(MarkEndId);
				if (MarkEnd) {
					for (var Parent = this.Parent, Parents = []; Parent != null; Parent = Parent.Parent) {
						if (Parent.Node.TypeIndex == 3 || Parent.Node.TypeIndex == 4)
							Parents.push(Parent);
					}
					for (var Index = Parents.length, Datas = this.Node.Skin.ResetNode(this); Index; Datas.push(Parents[--Index].Views[0].GetData()))
						;
					switch (ReShowType) {
						case SkinReShowType.ReShow:
							this.ReShowOnly(MarkStart, MarkEnd, MarkEndId);
							break;
						case SkinReShowType.PushArray:
							this.ReShowPushArray(MarkStart, MarkEnd);
							break;
						case SkinReShowType.PushArrayExpand:
							this.ReShowPushArrayExpand(MarkStart);
							break;
						case SkinReShowType.RemoveArray:
							this.ReShowRemoveArray(MarkStart, MarkEnd, MarkEndId);
							break;
					}
					return this.Node.Skin;
				}
			}
			this.IsExpired = true;
			return null;
		};
		SkinViewNode.prototype.RemoveLoop = function (Index, WriteIndex) {
			if (this.LoopNodes)
				this.LoopNodes[WriteIndex] = this.LoopNodes[Index];
		};
		SkinViewNode.prototype.SetRemoveLoopLength = function (Length) {
			if (this.LoopNodes && this.LoopNodes.length > Length)
				this.LoopNodes.length = Length;
		};
		SkinViewNode.prototype.SetPushLoopLength = function (Length) {
			if (this.LoopNodes && this.LoopNodes.length < Length)
				this.LoopNodes.length = Length;
		};
		SkinViewNode.prototype.ResetLoop = function (Index) {
			if (this.LoopNodes)
				this.LoopNodes[Index] = null;
		};
		SkinViewNode.prototype.ReShowOnly = function (MarkStart, MarkEnd, MarkEndId) {
			this.NoMark = true;
			this.ClearSearchNode(true);
			this.Create();
			this.NoMark = false;
			for (var MarkParent = HtmlElement.$ParentElement(MarkStart), Element = HtmlElement.$NextElement(MarkStart); Element && Element.id != MarkEndId; Element = HtmlElement.$NextElement(MarkStart))
				HtmlElement.$Delete(Element, MarkParent);
			Pub.DeleteElements.Html(this.Node.Skin.EndHtml()).Child().InsertBefore(MarkEnd, MarkParent);
		};
		SkinViewNode.prototype.ReShowPushArray = function (MarkStart, MarkEnd) {
			this.CreateView();
			this.CreateLoopOnly();
			Pub.DeleteElements.Html(this.Node.Skin.EndHtml()).Child().InsertBefore(MarkEnd, HtmlElement.$ParentElement(MarkStart));
		};
		SkinViewNode.prototype.ReShowPushArrayExpand = function (MarkStart) {
			this.CreateView();
			this.CreateLoopExpand();
			Pub.DeleteElements.Html(this.Node.Skin.EndHtml()).Child().InsertBefore(AutoCSer.HtmlElement.$NextElement(MarkStart), HtmlElement.$ParentElement(MarkStart));
		};
		SkinViewNode.prototype.ReShowRemoveArray = function (MarkStart, MarkEnd, MarkEndId) {
			this.ClearSearchNode(true);
			for (var MarkParent = HtmlElement.$ParentElement(MarkStart), Element = HtmlElement.$NextElement(MarkStart); Element && Element.id != MarkEndId; Element = HtmlElement.$NextElement(MarkStart))
				HtmlElement.$Delete(Element, MarkParent);
			HtmlElement.$Delete(MarkStart, MarkParent);
			HtmlElement.$Delete(MarkEnd, MarkParent);
		};
		SkinViewNode.ReShowTask = function () {
			for (var NodeHash = {}, Index = SkinViewNode.ReShowNodes.length; Index;) {
				var Node = SkinViewNode.ReShowNodes[--Index];
				if (Node.ReShowType != SkinReShowType.None) {
					if (Node.SkinNoOutput)
						Node.ReShowType = SkinReShowType.None;
					else
						NodeHash[Node.Identity] = Node;
				}
			}
			for (var Nodes = [], Index = SkinViewNode.ReShowNodes.length; Index;) {
				var Node = SkinViewNode.ReShowNodes[--Index];
				if (Node.ReShowType != SkinReShowType.None) {
					for (var Parent = Node.Parent; Parent != null; Parent = Parent.Parent) {
						var ParentNode = NodeHash[Parent.Identity];
						if (ParentNode) {
							if (ParentNode === SkinViewNode.NullNode)
								Parent = null;
							break;
						}
						NodeHash[Parent.Identity] = SkinViewNode.NullNode;
					}
					if (Parent == null)
						Nodes.push(Node);
					else
						Node.ReShowType = SkinReShowType.None;
				}
			}
			SkinViewNode.ReShowNodes = [];
			SkinViewNode.IsReShowTask = false;
			for (var SkinArray = [], Skins = {}, Index = Nodes.length; Index;) {
				var Skin = Nodes[--Index].ReShow();
				if (Skin && !Skins[Skin.Identity]) {
					Skins[Skin.Identity] = Skin;
					SkinArray.push(Skin);
				}
			}
			Pub.DeleteElements.Html('');
			for (var Index = SkinArray.length; Index; SkinArray[--Index].OnSet.Function())
				;
		};
		SkinViewNode.NodeIdentity = 0;
		SkinViewNode.ReShowNodes = [];
		SkinViewNode.NullNode = new SkinViewNode(null, null, null);
		return SkinViewNode;
	}());
	var SkinView = (function () {
		function SkinView(Datas, ClientData, IsClient) {
			if (ClientData === void 0) { ClientData = null; }
			if (IsClient === void 0) { IsClient = false; }
			this.Datas = Datas;
			this.ClientData = ClientData;
			this.IsClient = IsClient;
		}
		SkinView.prototype.GetData = function () {
			return this.IsClient ? this.ClientData : (this.Datas ? this.Datas[0] : null);
		};
		SkinView.prototype.CheckLogic = function (Expression) {
			var Data = this.GetData();
			if (Data == null)
				return false;
			var Value = Data.$Data;
			if (Value == null)
				return false;
			var Member = Expression.Get();
			return Member.Value == null ? !!Value : (Value.toString() === Member.Value);
		};
		return SkinView;
	}());
	var SkinData = (function () {
		function SkinData(Parent, Name, Data, Function) {
			if (Data != null)
				SkinDatas.SetDatas(this, Data);
			this.$Parent = Parent;
			this.$Name = Name;
			this.$Data = Data;
			this.$Function = Function;
		}
		SkinData.prototype.$ReShow = function (Type) {
			if (Type === void 0) { Type = SkinReShowType.ReShow; }
			if (this.$Nodes) {
				var Nodes = this.$Nodes;
				this.$Nodes = null;
				for (var Index = Nodes.length; Index; Nodes[--Index].TryShow(Type))
					;
			}
		};
		SkinData.prototype.$Get = function (Name) {
			if (this.$Data == null)
				return null;
			var Value = this.$Data[Name];
			if (Value === undefined)
				return null;
			var Data = this[Name], Function = null;
			if (Value != null && typeof (Value) == 'function')
				Value = (Function = Value).apply(this.$Data);
			return Data && Data.$Data === Value && Data.$Function === Function ? Data : this[Name] = new SkinData(this, Name, Value, Function);
		};
		SkinData.prototype.$Set = function (Data, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
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
						else
							IsData = true;
					}
					else
						IsData = true;
					if (IsData) {
						if (this.$Data == null) {
							if (Data != null)
								SkinDatas.SetDatas(this, Data);
						}
						else if (Data == null)
							SkinDatas.RemoveDatas(this);
						else
							SkinDatas.ReplaceDatas(this, Data);
						this.$Data = Data;
						if (ParentData != null)
							ParentData[this.$Name] = Data;
					}
					if (IsReShow)
						this.$ReShow(SkinReShowType.ReShow);
				}
				else
					this.$ReShow(SkinReShowType.ReShow);
			}
		};
		SkinData.prototype.$Add = function (Data, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			this.$Set(this.$Data == null ? Data : this.$Data + Data, IsReShow);
		};
		SkinData.prototype.$Not = function (IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			this.$Set(!this.$Data, IsReShow);
			return this.$Data;
		};
		SkinData.prototype.$Copy = function (Data, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			if (Data)
				this.$Set(this.$Data == null ? Data : Pub.Copy(this.$Data, Data), IsReShow);
		};
		SkinData.prototype.$Array = function () {
			var Data = this.$Data, Datas = [];
			if (Data instanceof Array) {
				for (var Index = 0; Index !== Data.length; Datas.push(this[Index++]))
					;
			}
			return Datas;
		};
		SkinData.prototype.$For = function (Function) {
			var Data = this.$Data;
			if (Data instanceof Array) {
				for (var Index = 0; Index !== Data.length; Function(this[Index++]))
					;
			}
		};
		SkinData.prototype.$Find = function (IsValue) {
			var Data = this.$Data, Datas = [];
			if (Data instanceof Array) {
				for (var Index = -1; ++Index !== Data.length;) {
					if (IsValue(Data[Index]))
						Datas.push(this[Index]);
				}
			}
			return Datas;
		};
		SkinData.prototype.$Remove = function (IsValue, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			var Data = this.$Data, Datas = [];
			if (Data instanceof Array) {
				for (var Index = 0; Index !== Data.length; ++Index) {
					if (IsValue(Data[Index])) {
						Skin.Refresh();
						this.$RemoveIndex(Index);
						for (var WriteIndex = Index; ++Index !== Data.length;) {
							if (IsValue(Data[Index]))
								this.$RemoveIndex(Index);
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
		};
		SkinData.prototype.$RemoveIndex = function (Index) {
			var RemoveData = this[Index];
			if (RemoveData)
				RemoveData.$ReShow(SkinReShowType.RemoveArray);
		};
		SkinData.prototype.$MoveIndex = function (Index, WriteIndex) {
			var MoveData = this[Index];
			if (MoveData)
				(this[WriteIndex] = MoveData).$Name = Index;
			if (this.$Nodes) {
				for (var NodeIndex = this.$Nodes.length; NodeIndex; this.$Nodes[--NodeIndex].RemoveLoop(Index, WriteIndex))
					;
			}
		};
		SkinData.prototype.$RemoveFinally = function () {
			if (this.$Nodes) {
				for (var Index = this.$Nodes.length; Index; this.$Nodes[--Index].SetRemoveLoopLength(this.$Data.length))
					;
			}
			Skin.Refresh();
		};
		SkinData.prototype.$RemoveAt = function (Index, Count, IsReShow) {
			if (Count === void 0) { Count = 1; }
			if (IsReShow === void 0) { IsReShow = true; }
			var Data = this.$Data;
			if (Data instanceof Array && Index < Data.length) {
				Skin.Refresh();
				if (Count = Data.splice(Index, Count).length) {
					var WriteIndex = Index;
					for (Index += Count; Index !== WriteIndex; this.$RemoveIndex(--Index))
						;
					Index += Count;
					for (var EndIndex = Data.length + Count; Index != EndIndex; this.$MoveIndex(Index++, WriteIndex++))
						;
					this.$RemoveFinally();
				}
			}
		};
		SkinData.prototype.$Replace = function (Value, IsValue, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			var Data = this.$Data;
			if (Data instanceof Array) {
				for (var Index = 0; Index !== Data.length; ++Index) {
					if (IsValue(Data[Index])) {
						Data[Index] = Value;
						var ReplaceData = this[Index];
						if (ReplaceData)
							ReplaceData.$Set(Value, IsReShow);
					}
				}
			}
		};
		SkinData.prototype.$Push = function (Value, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			var Data = this.$Data;
			if (Data == null)
				this.$Set([Value], IsReShow);
			else if (Data instanceof Array) {
				Skin.Refresh();
				Data.push(Value);
				if (IsReShow)
					this.$ReShow(SkinReShowType.PushArray);
				Skin.Refresh();
			}
		};
		SkinData.prototype.$Pushs = function (Datas, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			if (Datas && Datas.length) {
				var Data = this.$Data;
				if (Data == null)
					this.$Set(Datas, IsReShow);
				else if (Data instanceof Array) {
					Skin.Refresh();
					for (var Index = 0; Index !== Datas.length; Data.push(Datas[Index++]))
						;
					if (IsReShow)
						this.$ReShow(SkinReShowType.PushArray);
					Skin.Refresh();
				}
			}
		};
		SkinData.prototype.$PushExpand = function (Value, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			this.$PushExpands([Value], IsReShow);
		};
		SkinData.prototype.$PushExpands = function (Datas, IsReShow) {
			if (IsReShow === void 0) { IsReShow = true; }
			if (Datas && Datas.length) {
				var Data = this.$Data;
				if (Data == null)
					this.$Set(Datas, IsReShow);
				else if (Data instanceof Array) {
					Skin.Refresh();
					this.$SetPushLength(Data.length + Datas.length);
					for (var Index = Data.length; Index; this.$MoveIndex(Index, Index + Datas.length))
						--Index;
					var CopyData = Data.Copy();
					Data.length = 0;
					for (var Index = 0; Index !== Datas.length; Data.push(Datas[Index++]))
						this.$ResetIndex(Index);
					for (var Index = 0; Index !== CopyData.length; Data.push(CopyData[Index++]))
						;
					if (IsReShow)
						this.$ReShow(SkinReShowType.PushArrayExpand);
					Skin.Refresh();
				}
			}
		};
		SkinData.prototype.$SetPushLength = function (Count) {
			if (this.$Nodes) {
				for (var Index = this.$Nodes.length; Index; this.$Nodes[--Index].SetPushLoopLength(Count))
					;
			}
		};
		SkinData.prototype.$ResetIndex = function (Index) {
			this[Index] = null;
			if (this.$Nodes) {
				for (var NodeIndex = this.$Nodes.length; NodeIndex; this.$Nodes[--NodeIndex].ResetLoop(Index))
					;
			}
		};
		SkinData.prototype.$Sort = function (Function) {
			var Data = this.$Data;
			if (Data instanceof Array)
				this.$Set(Data.sort(Function));
		};
		SkinData.prototype.$ = function (Name) {
			return this[Name];
		};
		return SkinData;
	}());
	AutoCSer.SkinData = SkinData;
	var SkinDatas = (function () {
		function SkinDatas(Data) {
			this.Datas = [Data];
		}
		SkinDatas.prototype.Remove = function (SkinData) {
			this.Datas.RemoveAtEnd(this.Datas.IndexOfValue(SkinData));
		};
		SkinDatas.prototype.ReShowName = function (Name) {
			for (var Datas = this.Datas.Copy(), Index = Datas.length; Index;) {
				var Data = Datas[--Index][Name];
				if (Data)
					Data.$ReShow();
			}
		};
		SkinDatas.RemoveDatas = function (SkinData) {
			var Datas = SkinData.$Data['$'];
			if (Datas)
				Datas.Remove(SkinData);
		};
		SkinDatas.ReplaceDatas = function (SkinData, Data) {
			var OldDatas = SkinData.$Data['$'], NewDatas = Data['$'];
			if (OldDatas) {
				if (NewDatas == OldDatas)
					return;
				OldDatas.Remove(SkinData);
			}
			this.Push(NewDatas, SkinData, Data);
		};
		SkinDatas.SetDatas = function (SkinData, Data) {
			var Datas = Data['$'];
			this.Push(Datas, SkinData, Data);
		};
		SkinDatas.Push = function (Datas, SkinData, Data) {
			if (Datas)
				Datas.Datas.push(SkinData);
			else {
				try {
					Data['$'] = 1;
					if (Data['$'])
						Data['$'] = new SkinDatas(SkinData);
				}
				catch (e) { }
			}
		};
		return SkinDatas;
	}());
	var SkinMemberName = (function () {
		function SkinMemberName(Html, StartIndex, EndIndex, IsLogic) {
			this.Depth = 0;
			if (StartIndex !== EndIndex) {
				this.Names = [];
				while (StartIndex !== EndIndex && Html.charCodeAt(StartIndex) === 46) {
					++this.Depth;
					++StartIndex;
				}
				for (var Index = StartIndex; Index !== EndIndex;) {
					var Code = Html.charCodeAt(Index);
					if (IsLogic && Code === 61) {
						this.Names.push(Html.substring(StartIndex, Index));
						this.Value = Html.substring(Index + 1, EndIndex);
						return;
					}
					if (Code === 46) {
						this.Names.push(Html.substring(StartIndex, Index));
						StartIndex = ++Index;
					}
					else
						++Index;
				}
				if (StartIndex !== EndIndex)
					this.Names.push(Html.substring(StartIndex, EndIndex));
			}
		}
		SkinMemberName.prototype.CreateData = function (Node, IsClient) {
			var Datas = Node.Node.Skin.Datas, ParentIndex = Math.max(Datas.length - this.Depth, 1);
			if (!this.Names) {
				var Data = Datas[ParentIndex - 1];
				Node.SetData(Data);
				return Data;
			}
			var NameDatas = [], ClientDatas;
			while (ParentIndex) {
				NameDatas.length = 0;
				var ParentData = Datas[--ParentIndex], NameIndex = 0;
				do {
					var Data = ParentData.$Get(this.Names[NameIndex]);
					if (Data == null)
						break;
					NameDatas.push(ParentData);
					if (++NameIndex === this.Names.length) {
						NameDatas.push(Data);
						return this.SetData(Node, NameDatas);
					}
					ParentData = Data;
				} while (true);
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
		};
		SkinMemberName.prototype.SetData = function (Node, Datas) {
			if (Datas) {
				for (var Index = 1; Index !== Datas.length; Node.SetData(Datas[Index++]))
					;
				return Datas[Index - 1];
			}
		};
		return SkinMemberName;
	}());
	var SkinExpression = (function () {
		function SkinExpression(Names, ClientName, IsNot) {
			this.Names = Names;
			this.ClientName = ClientName;
			this.IsNot = IsNot;
		}
		SkinExpression.prototype.Get = function () {
			return this.ClientName || this.Names[0];
		};
		SkinExpression.prototype.CreateView = function (Node) {
			var Views;
			if (this.Names) {
				Views = [];
				for (var Index = 0; Index !== this.Names.length; Views.push(Data)) {
					var Data = this.Names[Index++].CreateData(Node, false);
					if (Data)
						Node.SetData(Data);
				}
			}
			return new SkinView(Views, this.ClientName ? this.ClientName.CreateData(Node, true) : null, !!this.ClientName);
		};
		SkinExpression.NullExpression = new SkinExpression(null, new SkinMemberName('', 0, 0, false), false);
		return SkinExpression;
	}());
	var SkinNode = (function () {
		function SkinNode(Skin, TypeIndex) {
			this.Skin = Skin;
			this.TypeIndex = TypeIndex;
			this.TypeSize = SkinBuilder.TypeSizes[TypeIndex];
		}
		SkinNode.prototype.GetExpression = function (Index) {
			return this.Expressions ? this.Expressions[Index] : SkinExpression.NullExpression;
		};
		return SkinNode;
	}());
	var Skin = (function () {
		function Skin(Id, Html, OnShowed, OnSet) {
			if (Id === void 0) { Id = null; }
			if (Html === void 0) { Html = null; }
			if (OnShowed === void 0) { OnShowed = null; }
			if (OnSet === void 0) { OnSet = null; }
			this.OnShowed = OnShowed || new Events;
			this.OnSet = OnSet || new Events;
			this.Id = Id;
			this.Identity = ++Pub.Identity;
			(this.Node = new SkinNode(this, 0)).Nodes = new SkinBuilder(this, Html == null ? (Id ? HtmlElement.$Id(Id) : HtmlElement.$(document.body)).Html0().replace(/ @(src|style)=/gi, ' $1=').replace(/select@/gi, 'select') : Html).Nodes;
		}
		Skin.prototype.Reset = function (Value) {
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
		};
		Skin.prototype.ToHtml = function (Data, IsMark) {
			if (IsMark === void 0) { IsMark = false; }
			this.NoMark = IsMark ? 0 : 1;
			this.Reset(Data);
			this.ViewNode.ClearSearchNode(true);
			this.ViewNode.Create();
			return this.EndHtml();
		};
		Skin.prototype.EndHtml = function () {
			var Html = this.Htmls.join('').replace(/ src="=@[^"]+"/gi, '').replace(/ @check="true"/g, ' checked="checked"');
			this.Datas.length = 1;
			this.Htmls = null;
			return location.protocol == 'https:' ? Html.replace(/http\:\/\/127.0.0.1:14000\//g, 'https://127.0.0.1:14000/') : Html;
		};
		Skin.prototype.ResetNode = function (Node) {
			this.NoMark = Node.SkinNoMark || 0;
			this.NoOutput = 0;
			this.Datas.length = 1;
			this.Htmls = [];
			return this.Datas;
		};
		Skin.prototype.Show = function (Data, IsMark) {
			if (IsMark === void 0) { IsMark = true; }
			(this.Id ? HtmlElement.$Id(this.Id) : HtmlElement.$(document.body)).Html(this.ToHtml(Data, IsMark)).Display(1);
			this.OnShowed.Function(Data);
			this.OnSet.Function(Data);
		};
		Skin.prototype.SetHtml = function (Data, IsMark) {
			if (IsMark === void 0) { IsMark = true; }
			(this.Id ? HtmlElement.$Id(this.Id) : HtmlElement.$(document.body)).Html(this.ToHtml(Data, IsMark));
			this.OnSet.Function(Data);
		};
		Skin.prototype.SkinData = function (Name) {
			if (Name === void 0) { Name = null; }
			var Data = this.Datas[0];
			return Name ? Data.$(Name) : Data;
		};
		Skin.prototype.SearchData = function (Identity) {
			if (this.ViewNode) {
				var Node = this.ViewNode.SearchNode(Identity);
				if (Node)
					return Node.Views[0].GetData();
			}
		};
		Skin.prototype.Hide = function () {
			(this.Id ? HtmlElement.$Id(this.Id) : HtmlElement.$(document.body)).Display(0);
		};
		Skin.Refresh = function () {
			SkinViewNode.ReShowTask();
		};
		Skin.BodyData = function (Name) {
			if (Name === void 0) { Name = null; }
			return this.Body.SkinData(Name);
		};
		Skin.ChangeHeader = function () {
			document.title = this.Header.ToHtml(this.Body.Data, false);
		};
		Skin.Create = function (PageView) {
			for (var Childs = HtmlElement.$('@skin').GetElements(), Index = Childs.length; Index; this.Skins[Id] = new Skin(Id)) {
				var Child = Childs[--Index], Id = Child.id;
				if (!Id)
					Child.id = Id = HtmlElement.$Attribute(Child, 'skin');
			}
			if (PageView.IsLoadView) {
				this.Header = new Skin(null, document.title.replace(/\=@@/g, '=@'));
				var ViewOver = document.getElementById('AutoCSerViewOver');
				if (ViewOver) {
					var Display = ViewOver.style.display;
					ViewOver.style.display = 'none';
					this.Body = new Skin(null, null, PageView.OnShowed, PageView.OnSet);
					ViewOver.style.display = Display || '';
				}
				else
					this.Body = new Skin(null, null, PageView.OnShowed, PageView.OnSet);
			}
		};
		Skin.SearchIdentity = function (Identity) {
			var Data = this.Body.SearchData(Identity);
			if (Data == null) {
				for (var Name in this.Skins) {
					var Skin = this.Skins[Name];
					if (Skin && Skin.SearchData && (Data = Skin.SearchData(Identity)))
						break;
				}
			}
			return Data;
		};
		Skin.DeleteMark = function (Span) {
			SkinViewNode.DeleteMark(Span);
		};
		Skin.Skins = {};
		return Skin;
	}());
	AutoCSer.Skin = Skin;
	var SkinBuilder = (function () {
		function SkinBuilder(Skin, Html) {
			this.Skin = Skin;
			this.Html = Html;
			this.Nodes = [];
			var EndIndex = this.HtmlIndex = 0;
			do {
				var Index = this.Html.indexOf('<!--', EndIndex);
				if (Index < 0)
					break;
				var TypeIndex = this.GetType(Index += 4);
				if (TypeIndex) {
					EndIndex = this.Html.indexOf('-->', Index);
					if (EndIndex < 0)
						break;
					var TypeSize = SkinBuilder.TypeSizes[TypeIndex], IsExpression = EndIndex - Index - TypeSize;
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
								case 1:
								case 2:
									this.LogicExpression(Node);
									break;
								case 3:
								case 4:
									this.ValueExpression(Node);
									break;
							}
						}
						this.Nodes.push(Node);
						this.CheckIndex = this.Nodes.length;
					}
					this.HtmlIndex = (EndIndex += 3);
				}
				else {
					EndIndex = this.Html.indexOf('-->', Index - 2);
					if (EndIndex < 0)
						break;
					EndIndex += 3;
				}
			} while (true);
			this.At(this.Html.length);
			var ErrorQuery;
			while (this.CheckIndex) {
				var Node = this.Nodes[--this.CheckIndex];
				if (Node.StartIndex && !Node.Nodes) {
					if (!ErrorQuery)
						HttpRequest.Post('Pub.Error', { error: navigator.appName + ' : ' + navigator.appVersion + '\r\nSkin解析失败: ' + document.location.toString() + '\r\nId[' + Skin.Id + '] ' + this.Html.substring(0, Node.StartIndex).split('\n').length + ' <!--' + this.Html.substring(Node.StartIndex, Node.EndIndex) + '-->' });
					Node.Nodes = this.Nodes.slice(this.CheckIndex + 1, this.Nodes.length);
					this.Nodes.length = this.CheckIndex + 1;
				}
			}
		}
		SkinBuilder.prototype.GetType = function (Index) {
			switch (this.Html.charCodeAt(Index) - 72) {
				case 73 - 72:
					if (this.Html.charCodeAt(Index + 1) === 102)
						return 1;
					break;
				case 76 - 72:
					if (this.Html.charCodeAt(Index + 1) === 111 && this.Html.charCodeAt(Index + 2) === 111 && this.Html.charCodeAt(Index + 3) === 112)
						return 3;
					break;
				case 78 - 72:
					if (this.Html.charCodeAt(Index + 2) === 116) {
						if (this.Html.charCodeAt(Index + 1) === 111)
							return 2;
					}
					else if (this.Html.charCodeAt(Index + 1) === 111 && this.Html.charCodeAt(Index + 2) === 77 && this.Html.charCodeAt(Index + 3) === 97 && this.Html.charCodeAt(Index + 4) === 114 && this.Html.charCodeAt(Index + 5) === 107)
						return 5;
					break;
				case 86 - 72:
					if (this.Html.charCodeAt(Index + 1) === 97 && this.Html.charCodeAt(Index + 2) === 108 && this.Html.charCodeAt(Index + 3) === 117 && this.Html.charCodeAt(Index + 4) === 101)
						return 4;
					break;
			}
			return 0;
		};
		SkinBuilder.prototype.CheckRound = function (NewNode) {
			while (this.CheckIndex) {
				var Node = this.Nodes[--this.CheckIndex];
				if (Node.StartIndex && !Node.Nodes) {
					if (Node.EndIndex - Node.StartIndex === NewNode.EndIndex - NewNode.StartIndex && Node.TypeIndex === NewNode.TypeIndex) {
						var Start = Node.StartIndex + Node.TypeSize, NewStart = NewNode.StartIndex + Node.TypeSize;
						while (Start != Node.EndIndex) {
							if (this.Html.charCodeAt(NewStart++) !== this.Html.charCodeAt(Start++))
								return true;
						}
						Node.Nodes = this.Nodes.slice(this.CheckIndex + 1, this.Nodes.length);
						this.Nodes.length = this.CheckIndex + 1;
						return false;
					}
					break;
				}
			}
			return true;
		};
		SkinBuilder.prototype.At = function (EndIndex) {
			for (var NoMark = false, Index = this.HtmlIndex + 1; Index < EndIndex; ++Index) {
				if (this.Html.charCodeAt(Index) === 64) {
					if (this.Html.charCodeAt(Index - 1) === 61) {
						if (this.HtmlIndex !== Index - 1)
							NoMark = this.PushHtml(Index - 1, NoMark);
						var Node = new SkinNode(this.Skin, 7);
						Node.NoMarkAt = NoMark;
						if (++Index !== EndIndex) {
							var Code = this.Html.charCodeAt(Index);
							if (Code == 36) {
								Node.IsIdentity = true;
								++Index;
							}
							else if (Code == 91) {
								if (this.Html.charCodeAt(Index + 1) == 93) {
									Node.IsLoopIndex = true;
									Index += 2;
								}
							}
							else {
								if (Code === 64)
									Node.IsHtml = true;
								if (Code === 42)
									Node.IsTextArea = true;
								if (Node.IsHtml || Node.IsTextArea || Node.IsIdentity)
									++Index;
								if (Index !== EndIndex) {
									var Names = null, ClientName = null, NameIndex = Index;
									do {
										while (Index !== EndIndex && SkinBuilder.ValueMap[this.Html.charCodeAt(Index)])
											++Index;
										if (NameIndex === Index)
											break;
										if (!Names)
											Names = [];
										Names.push(new SkinMemberName(this.Html, NameIndex, Index, false));
										if (this.Html.charCodeAt(Index) !== 124)
											break;
										NameIndex = ++Index;
									} while (true);
									if (Index !== EndIndex) {
										if (this.Html.charCodeAt(Index) === 35) {
											for (NameIndex = ++Index; Index !== EndIndex && SkinBuilder.ValueMap[this.Html.charCodeAt(Index)]; ++Index)
												;
											ClientName = new SkinMemberName(this.Html, NameIndex, Index, false);
										}
										if (Index !== EndIndex && this.Html.charCodeAt(Index) === 36)
											++Index;
									}
									if (Names || ClientName)
										Node.Expressions = [new SkinExpression(Names, ClientName, false)];
								}
							}
						}
						this.Nodes.push(Node);
						this.HtmlIndex = Index;
					}
					else
						++Index;
				}
			}
			if (this.HtmlIndex < EndIndex)
				this.PushHtml(EndIndex, NoMark);
		};
		SkinBuilder.prototype.PushHtml = function (EndIndex, NoMark) {
			var Node = new SkinNode(this.Skin, 6);
			Node.Html = this.Html.substring(this.HtmlIndex, EndIndex);
			this.Nodes.push(Node);
			var Start = Node.Html.lastIndexOf('<'), End = Node.Html.lastIndexOf('>');
			return Start >= 0 ? End < 0 || Start > End : (End < 0 && NoMark);
		};
		SkinBuilder.prototype.LogicExpression = function (Node) {
			Node.Expressions = [];
			var StartIndex = Node.StartIndex + Node.TypeSize + 1, EndIndex = Node.EndIndex;
			for (var Index = StartIndex; Index !== EndIndex;) {
				var Code = this.Html.charCodeAt(Index);
				if (Code === 124) {
					Node.IsOrExpression = true;
					Node.Expressions.push(this.ParseExpression(StartIndex, Index, true));
					StartIndex = ++Index;
				}
				else if (Code === 38) {
					Node.Expressions.push(this.ParseExpression(StartIndex, Index, true));
					StartIndex = ++Index;
				}
				else if (Code === 64) {
					Node.Expressions.push(this.ParseExpression(StartIndex, Index, true));
					return;
				}
				else
					++Index;
			}
			Node.Expressions.push(this.ParseExpression(StartIndex, EndIndex, true));
		};
		SkinBuilder.prototype.ValueExpression = function (Node) {
			var StartIndex = Node.StartIndex + Node.TypeSize + 1, EndIndex = Node.EndIndex;
			for (var Index = StartIndex; Index !== EndIndex; ++Index) {
				if (this.Html.charCodeAt(Index) === 64) {
					EndIndex = Index;
					break;
				}
			}
			Node.Expressions = [this.ParseExpression(StartIndex, EndIndex)];
		};
		SkinBuilder.prototype.ParseExpression = function (StartIndex, EndIndex, IsLogic) {
			if (IsLogic === void 0) { IsLogic = false; }
			var Names = null, ClientName = null, IsNot = false;
			if (StartIndex != EndIndex) {
				if (IsLogic && this.Html.charCodeAt(StartIndex) === 33) {
					IsNot = true;
					++StartIndex;
				}
				for (var HashIndex = EndIndex; HashIndex !== StartIndex;) {
					if (this.Html.charCodeAt(--HashIndex) === 35) {
						ClientName = new SkinMemberName(this.Html, HashIndex + 1, EndIndex, IsLogic);
						EndIndex = HashIndex;
						break;
					}
				}
				if (StartIndex !== EndIndex) {
					Names = [];
					for (var Index = StartIndex; Index !== EndIndex;) {
						if (this.Html.charCodeAt(Index) === 44) {
							Names.push(new SkinMemberName(this.Html, StartIndex, Index, IsLogic));
							StartIndex = ++Index;
						}
						else
							++Index;
					}
					if (StartIndex !== EndIndex)
						Names.push(new SkinMemberName(this.Html, StartIndex, EndIndex, IsLogic));
				}
			}
			return new SkinExpression(Names, ClientName, IsNot);
		};
		SkinBuilder.TypeSizes = [0, 2, 3, 4, 5, 6, 4, 2];
		SkinBuilder.ValueMap = new Array(0x7b);
		return SkinBuilder;
	}());
	for (var Index = 0x30; Index !== 0x3a; SkinBuilder.ValueMap[Index++] = true)
		;
	for (var Index = 0x41; Index !== 0x5b; SkinBuilder.ValueMap[Index++] = true)
		;
	for (var Index = 0x61; Index !== 0x7b; SkinBuilder.ValueMap[Index++] = true)
		;
	SkinBuilder.ValueMap[0x2e] = SkinBuilder.ValueMap[0x5f] = true;
	var PageView = (function () {
		function PageView() {
		}
		return PageView;
	}());
	window.onerror = function (message, filename, lineno, colno, error) {
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
						Pub.AppendJs(Ajax.Url, AutoCSer.Loader.Charset, null, Ajax.GetOnError(null));
						return;
					}
				}
				Pub.SendError(navigator.appName + ' : ' + navigator.appVersion + '\r\n' + document.location.toString() + '\r\n' + filename + ' [' + lineno + ',' + colno + '] ' + message);
			}
		}
	};
	var BrowserEvent = (function () {
		function BrowserEvent(Event) {
			if (this.Event = Event || event) {
				this.srcElement = this.target = (Pub.IE ? this.Event.srcElement : this.Event.target);
				this.clientX = this.pageX = Pub.IE ? this.Event.clientX : this.Event.pageX;
				this.clientY = this.pageY = Pub.IE ? this.Event.clientY : this.Event.pageY;
				this.keyCode = this.which = Pub.IE ? this.Event.keyCode : this.Event.which;
				this.shiftKey = this.Event['shiftKey'];
				this.touches = this.Event['touches'];
				this.changedTouches = this.Event['changedTouches'];
			}
			this.Return = true;
		}
		BrowserEvent.prototype.CancelBubble = function () {
			this.Return = false;
			if (Pub.IE)
				this.Event.returnValue = false;
			else
				this.Event.preventDefault();
		};
		BrowserEvent.prototype.$Name = function (Name, Value) {
			if (Value === void 0) { Value = null; }
			return HtmlElement.$ElementName(this.srcElement, Name, Value);
		};
		return BrowserEvent;
	}());
	AutoCSer.BrowserEvent = BrowserEvent;
	var DeclareEvent = (function (_super) {
		AutoCSer.Pub.Extends(DeclareEvent, _super);
		function DeclareEvent(Id, IsGetOnly) {
			if (Id === void 0) { Id = null; }
			if (IsGetOnly === void 0) { IsGetOnly = true; }
			if (Id) {
				var Element = HtmlElement.$IdElement(Id);
				_super.call(this, { srcElement: Element, target: Element });
				this.DeclareId = Id;
			}
			this.IsGetOnly = IsGetOnly;
		}
		DeclareEvent.Default = new DeclareEvent();
		return DeclareEvent;
	}(BrowserEvent));
	AutoCSer.DeclareEvent = DeclareEvent;
	var Declare = (function () {
		function Declare(Function, Name, EventName, Type) {
			this.Type = Type;
			this.EventName = EventName;
			this.LowerName = (this.Name = Name).toLowerCase();
			this.AutoCSerName = Name + 's';
			Pub.Functions[Name] = Function;
			Declare.Getters[this.Name] = Pub.ThisFunction(this, this.Get);
			Pub.OnLoad(this.Load, this, true);
		}
		Declare.prototype.Load = function () {
			Declare.Declares[this.AutoCSerName] = {};
			HtmlElement.$(document.body).AddEvent(this.EventName, Pub.ThisEvent(this, this[this.Type]));
		};
		Declare.prototype.NewDeclare = function (Parameter) {
			return new Pub.Functions[this.Name](Parameter);
		};
		Declare.prototype.Src = function (Event) {
			var Element = Event.srcElement, ParameterString = HtmlElement.$Attribute(Element, this.LowerName);
			if (ParameterString != null) {
				var Id = Element.id;
				if (Id) {
					var Values = Declare.Declares[this.AutoCSerName], Value = Values[Id];
					if (Value)
						Value.Start(Event);
					else
						Values[Id] = Value = this.NewDeclare(Declare.GetParameter(ParameterString, Event, Element, Id));
					return Value;
				}
				return this.NewDeclare(Declare.GetParameter(ParameterString, Event, Element));
			}
			return Declare.Declares[this.AutoCSerName][Event.DeclareId];
		};
		Declare.prototype.AttributeName = function (Event) {
			var Element = Event.$Name(this.LowerName);
			if (Element) {
				var Id = Element.id;
				if (Id) {
					var Values = Declare.Declares[this.AutoCSerName], Value = Values[Id];
					if (Value)
						Value.Start(Event);
					else
						Values[Id] = Value = this.NewDeclare(Declare.GetParameter(HtmlElement.$Attribute(Element, this.LowerName), Event, Element, Id));
					return Value;
				}
				return this.NewDeclare(Declare.GetParameter(HtmlElement.$Attribute(Element, this.LowerName), Event, Element));
			}
			return Declare.Declares[this.AutoCSerName][Event.DeclareId];
		};
		Declare.prototype.ParameterId = function (Event) {
			var Element = Event.$Name(this.LowerName);
			if (Element) {
				var Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')');
				if (Parameter.Id) {
					Element = HtmlElement.$IdElement(Parameter.Id);
					if (!Element)
						return Declare.Declares[this.AutoCSerName][Parameter.Id];
					Parameter = null;
				}
				var Id = Element.id, Values = Declare.Declares[this.AutoCSerName], Value;
				if (Id)
					Value = Values[Id];
				else
					Element.id = Id = Declare.NextId();
				if (Value)
					Value.Start(Event);
				else {
					if (Parameter == null)
						Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')');
					Parameter.Event = Event;
					Parameter.DeclareElement = Element;
					Values[Parameter.Id = Id] = Value = this.NewDeclare(Parameter);
				}
				return Value;
			}
			return Declare.Declares[this.AutoCSerName][Event.DeclareId];
		};
		Declare.prototype.ParameterMany = function (Event) {
			if (!Event.IsGetOnly) {
				var Element = Event.$Name(this.LowerName);
				if (Element) {
					var Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')');
					Parameter.DeclareElement = Element;
					Parameter.Event = Event;
					if (Parameter.Id) {
						var Value = Declare.Declares[this.AutoCSerName][Parameter.Id];
						if (Value) {
							Value.Reset(Parameter, Element);
							return Value;
						}
						Declare.Declares[this.AutoCSerName][Parameter.Id] = (Value = this.NewDeclare(Parameter));
						return Value;
					}
				}
			}
			return Declare.Declares[this.AutoCSerName][Event.DeclareId];
		};
		Declare.prototype.Get = function (Id, IsGetOnly) {
			if (IsGetOnly === void 0) { IsGetOnly = true; }
			return this[this.Type](new DeclareEvent(Id, IsGetOnly));
		};
		Declare.NextId = function () {
			return '_' + (++Pub.Identity) + '_DECLARE_';
		};
		Declare.GetParameter = function (ParameterString, Event, Element, Id) {
			if (Id === void 0) { Id = null; }
			var Parameter = (ParameterString ? eval('(' + ParameterString + ')') : {});
			Parameter.Id = Id || Declare.NextId();
			Parameter.DeclareElement = Element;
			Parameter.Event = Event;
			return Parameter;
		};
		Declare.Getters = {};
		Declare.Declares = {};
		return Declare;
	}());
	AutoCSer.Declare = Declare;
	var Cookie = (function () {
		function Cookie(Parameter) {
			Pub.GetParameter(this, Cookie.DefaultParameter, Parameter);
		}
		Cookie.prototype.Write = function (Value) {
			this.WriteCookie(Value);
		};
		Cookie.prototype.WriteCookie = function (Value) {
			var Cookie = Value.Name.Escape() + '=' + (Value.Value == null ? '.' : Value.Value.toString().Escape()), Expires = Value.Expires;
			if (Value.Value == null)
				Expires = new Date;
			else if (!Expires && this.Expires)
				Expires = (new Date).AddMilliseconds(this.Expires);
			if (Expires)
				Cookie += '; expires=' + Expires['toGMTString']();
			var Path = Value.Path || this.Path;
			if (Path)
				Cookie += '; path=' + Path;
			var Domain = Value.Domain || this.Domain;
			if (Domain)
				Cookie += '; domain=' + Domain;
			var Secure = Value.Secure || this.Secure;
			if (Secure)
				Cookie += '; secure=' + Secure;
			document.cookie = Cookie;
		};
		Cookie.prototype.Read = function (Name, DefaultValue) {
			if (DefaultValue === void 0) { DefaultValue = null; }
			for (var Values = document.cookie.split('; '), Value = null, Index = Values.length; --Index >= 0 && Value == null;) {
				var IndexOf = Values[Index].indexOf('=');
				if (window['unescape'](Values[Index].substring(0, IndexOf)) == Name)
					Value = window['unescape'](Values[Index].substring(IndexOf + 1));
			}
			return Value || DefaultValue;
		};
		Cookie.prototype.ReadJson = function (Name, DefaultValue) {
			var Value = this.Read(Name, null);
			return Value ? eval('(' + Value + ')') : DefaultValue;
		};
		Cookie.DefaultParameter = { Expires: null, Path: '/', Domain: location.hostname, Secure: null };
		Cookie.Default = new Cookie({ Expires: 24 * 3600 * 1000 });
		return Cookie;
	}());
	AutoCSer.Cookie = Cookie;
	var ServerTime = (function () {
		function ServerTime(Time) {
			this.Time = -(new Date().getTime() - Time.Now.getTime());
		}
		ServerTime.prototype.Now = function () {
			return new Date().AddMilliseconds(this.Time);
		};
		return ServerTime;
	}());
	AutoCSer.ServerTime = ServerTime;
})(AutoCSer || (AutoCSer = {}));
var AutoCSerAPI;
(function (AutoCSerAPI) {
	var Ajax;
	(function (Ajax) {
		var RefOut = (function () {
			function RefOut() {
			}
			RefOut.Add = function (left, right, Callback) {
				if (Callback === void 0) { Callback = null; }
				AutoCSer.Pub.GetAjaxPost()('RefOut.Add', { left: left, right: right }, Callback);
			};
			return RefOut;
		}());
		Ajax.RefOut = RefOut;
	})(Ajax = AutoCSerAPI.Ajax || (AutoCSerAPI.Ajax = {}));
})(AutoCSerAPI || (AutoCSerAPI = {}));

setTimeout(AutoCSer.Pub.LoadIE, 0, 'javascript');
