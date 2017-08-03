'use strict';
var AutoCSer;
(function (AutoCSer) {
	var Color512_64 = (function () {
		function Color512_64(Parameter) {
			AutoCSer.Pub.GetParameter(this, Color512_64.DefaultParameter, Parameter);
			AutoCSer.Pub.GetEvents(this, Color512_64.DefaultEvents, Parameter);
			this.Identity = ++AutoCSer.Pub.Identity;
			this.Image = AutoCSer.Loader.JsDomain + 'Js/color512_64.bmp';
		}
		Color512_64.prototype.Start = function () {
			var Iframe = '_' + this.Identity + '_COLOR_';
			AutoCSer.HtmlElement.$Id(this.Id).Html("<iframe Id='" + Iframe + "' name='" + Iframe + "' width='512px' height='64px' frameborder='0' marginwidth='0' marginheight='0' vspace='0' hspace='0' allowtransparency='true' scrolling='no'></iframe>");
			this.Iframe = frames[Iframe];
			this.Iframe.document.open();
			this.Iframe.document.write("<html><body><img id='Color512_64' src='" + this.Image + "' Width='512' Height='64' border='0' alt='点击改变最近选择的色彩参数' /></body></html>");
			this.Iframe.document.close();
			AutoCSer.HtmlElement.$(this.Iframe.document.getElementById('Color512_64')).AddEvent('mousemove', AutoCSer.Pub.ThisEvent(this, this.MoveColor, null, this.Iframe))
				.AddEvent('click', AutoCSer.Pub.ThisEvent(this, this.ClickColor, null, this.Iframe))
				.AddEvent('mouseover', AutoCSer.Pub.ThisFunction(this, this.Over, [true]))
				.AddEvent('mouseout', AutoCSer.Pub.ThisFunction(this, this.Over, [false]));
			AutoCSer.HtmlElement.$Id(this.CurrentColor).Set('maxLength', 6).Value('000000').Set('readOnly', true).AddEvent('focus', Color512_64.Select);
			if (this.CurrentColorSpan) {
				AutoCSer.HtmlElement.$Id(this.CurrentColorSpan).Style('backgroundColor', '#000000').Set('title', '点击改变最近选择的色彩参数').Cursor('pointer')
					.AddEvent('mouseover', AutoCSer.Pub.ThisFunction(this, this.Over, [true]))
					.AddEvent('mouseout', AutoCSer.Pub.ThisFunction(this, this.Over, [false]))
					.AddEvent('click', AutoCSer.Pub.ThisFunction(this, this.Click));
			}
		};
		Color512_64.prototype.GetColor = function (Event) {
			var Width = Event.clientX, Height = Event.clientY, Color = (((Width >> 5) << 4) + ((Height >> 5) << 3) + 4) << 16;
			Color += (((Height < 32 ? Height : 63 - Height) << 3) + 4) << 8;
			Width &= 63;
			return Color + ((Width < 32 ? Width : 63 - Width) << 3) + 4;
		};
		Color512_64.Select = function () { this['select'](); };
		Color512_64.prototype.MoveColor = function (Event) {
			this.Move(this.GetColor(Event));
		};
		Color512_64.prototype.ClickColor = function (Event) {
			this.OnClick.Function(this.Move(this.GetColor(Event)));
		};
		Color512_64.prototype.Over = function (IsOver) {
			this.OnOver.Function(IsOver);
		};
		Color512_64.prototype.Move = function (Color) {
			var Hex = Color.toString(16).PadLeft(6, '0');
			AutoCSer.HtmlElement.$SetValueById(this.CurrentColor, Hex);
			AutoCSer.HtmlElement.$SetStyle(AutoCSer.HtmlElement.$IdElement(this.CurrentColorSpan), 'backgroundColor', '#' + Hex);
			this.OnMove.Function(Hex);
			return Hex;
		};
		Color512_64.prototype.Click = function () {
			this.OnClick.Function(this.Move(parseInt(AutoCSer.HtmlElement.$GetValueById(this.CurrentColor), 16)));
		};
		Color512_64.prototype.Show = function (IsShow) {
			AutoCSer.HtmlElement.$([AutoCSer.HtmlElement.$IdElement(this.Id), AutoCSer.HtmlElement.$IdElement(this.CurrentColor), AutoCSer.HtmlElement.$IdElement(this.CurrentColorSpan)]).Display(IsShow);
		};
		Color512_64.DefaultParameter = { Id: null, CurrentColor: null, CurrentColorSpan: null };
		Color512_64.DefaultEvents = { OnClick: null, OnOver: null, OnMove: null };
		return Color512_64;
	}());
	AutoCSer.Color512_64 = Color512_64;
})(AutoCSer || (AutoCSer = {}));
var AutoCSer;
(function (AutoCSer) {
	var CrawlTitle = (function () {
		function CrawlTitle(Parameter) {
			AutoCSer.Pub.GetParameter(this, CrawlTitle.DefaultParameter, Parameter);
			setTimeout(AutoCSer.Pub.ThisFunction(this, this.Request), 1);
		}
		CrawlTitle.prototype.Request = function () {
			var Link = this.Link.Link, Index = Link.indexOf('#');
			if ((Index + 1) && Link.charAt(Index + 1) != '!')
				Link = Link.substring(0, Index);
			AutoCSer.HttpRequest.Post(this.AjaxCallName, { link: Link }, AutoCSer.Pub.ThisFunction(this, this.OnLink));
		};
		CrawlTitle.prototype.OnLink = function (Value) {
			this.Link.IsTitle = true;
			if (Value.Return) {
				this.Link.Title = AutoCSer.Pub.DeleteElements.Html(Value.Return).Text0();
				for (var Index = this.Link.CallBack.length; Index; this.Link.CallBack[--Index](this.Link))
					;
			}
		};
		CrawlTitle.Get = function (Link, CallBack) {
			var Value = this.Titles[Link];
			if (!Value)
				this.Links.push(this.Titles[Link] = Value = { Link: Link, Title: Link, CallBack: [], IsTitle: false });
			if (CallBack && !Value.IsTitle && Value.CallBack.IndexOfValue(CallBack) < 0)
				Value.CallBack.push(CallBack);
			return Value;
		};
		CrawlTitle.Request = function (AjaxCallName) {
			for (var Index = this.Links.length; Index; new CrawlTitle({ Link: this.Links[--Index], AjaxCallName: AjaxCallName }))
				;
			this.Links = [];
		};
		CrawlTitle.TryRequest = function (AjaxCallName) {
			if (this.Links.length)
				setTimeout(AutoCSer.Pub.ThisFunction(this, this.Request, [AjaxCallName]), 0);
		};
		CrawlTitle.DefaultParameter = { Link: null, AjaxCallName: null };
		CrawlTitle.Titles = {};
		CrawlTitle.Links = [];
		return CrawlTitle;
	}());
	AutoCSer.CrawlTitle = CrawlTitle;
})(AutoCSer || (AutoCSer = {}));


var AutoCSer;
(function (AutoCSer) {
	var HtmlEditorParameter = (function () {
		function HtmlEditorParameter() {
		}
		return HtmlEditorParameter;
	}());
	AutoCSer.HtmlEditorParameter = HtmlEditorParameter;
	var HtmlEditor = (function (_super) {
		AutoCSer.Pub.Extends(HtmlEditor, _super);
		function HtmlEditor(Parameter) {
			_super.call(this);
			AutoCSer.Pub.GetParameter(this, HtmlEditor.DefaultParameter, Parameter);
			AutoCSer.Pub.GetEvents(this, HtmlEditor.DefaultEvents, Parameter);
			this.OverButtonFunction = AutoCSer.Pub.ThisEvent(this, this.OverButton);
			this.OutButtonFunction = AutoCSer.Pub.ThisEvent(this, this.OutButton);
			this.OnCrawlTitleFunction = AutoCSer.Pub.ThisFunction(this, this.OnCrawlTitle);
			this.ReplaceLinkFunction = AutoCSer.Pub.ThisFunction(this, this.ReplaceLink);
			if (!AutoCSer.Pub.IE) {
				this.PasteImageIdentity = 0;
				if (!HtmlEditor.IsPasteImage)
					this.PasteImageAjaxCallName = null;
			}
			this.Identity = ++AutoCSer.Pub.Identity;
			this.EditorFrameId = '_' + this.Identity + 'EDITORIFRAME_';
			this.SaveFrameId = '_' + this.Identity + 'EDITORSAVE_';
			this.TextAreaId = '_' + this.Identity + 'EDITORINPUT_';
			this.TempId = '_' + this.Identity + 'EDITORTEMP_';
			this.SaveText = '';
			this.ButtonArray = ['Name,Title,OnlyDesign,OnClick,DefaultSet',
				['FontColor', '字体颜色', 1, AutoCSer.Pub.ThisEvent(this, this.SetColor, [true]), 0],
				['BgColor', '字体背景颜色', 1, AutoCSer.Pub.ThisEvent(this, this.SetColor, [false]), 0],
				['ClearCode', '清理代码', 0, AutoCSer.Pub.ThisFunction(this, this.ClearCode), 1],
				['SelectAll', '全选', 0, AutoCSer.Pub.ThisFunction(this, this.SelectAll), 1],
				['Cut', '切剪', 0, AutoCSer.Pub.ThisFunction(this, this.Cut), 1],
				['Copy', '复制', 0, AutoCSer.Pub.ThisFunction(this, this.Copy), 1],
				['Paste', '粘贴', 0, AutoCSer.Pub.ThisFunction(this, this.Paste), 1],
				['Undo', '撤消', 0, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['undo']), 1],
				['Redo', '重做', 0, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['redo']), 1],
				['Unlink', '去掉超级链接', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['Unlink']), 1],
				['InsertHorizontalRule', '插入水平线', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['InsertHorizontalRule']), 1],
				['Bold', '加粗', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['bold']), 1],
				['Italic', '斜体', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['italic']), 1],
				['UnderLine', '下划线', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['underline']), 1],
				['Superscript', '上标', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['superscript']), 1],
				['Subscript', '下标', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['subscript']), 1],
				['StrikeThrough', '删除线', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['strikethrough']), 1],
				['RemoveFormat', '取消格式', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['RemoveFormat']), 1],
				['JustifyLeft', '左对齐', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['justifyleft']), 1],
				['JustifyCenter', '居中', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['justifycenter']), 1],
				['JustifyRight', '右对齐', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['justifyright']), 1],
				['InsertOrderedList', '编号', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['insertorderedlist']), 1],
				['InsertUnOrderedList', '项目符号', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['insertunorderedlist']), 1],
				['OutDent', '减少缩进量', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['outdent']), 1],
				['InDent', '增加缩进量', 1, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['indent']), 1],
				['Quote', '插入引用', 1, AutoCSer.Pub.ThisFunction(this, this.Quote), 1],
				['Save', '保存', 0, AutoCSer.Pub.ThisFunction(this, this.Save), 1],
				['LoadSave', '加载', 0, AutoCSer.Pub.ThisFunction(this, this.LoadSave), 1],
				['ClearAll', '清除', 0, AutoCSer.Pub.ThisFunction(this, this.ClearAll), 1],
				['Print', '打印', 0, AutoCSer.Pub.ThisEvent(this, this.ExecCommand, ['print']), 1],
				['FormatCode', '整理代码', 0, AutoCSer.Pub.ThisFunction(this, this.FormatCode), 1]
			].FormatAjax();
			this.ChangeParagraphFunction = AutoCSer.Pub.ThisFunction(this, this.ChangeParagraph);
			this.ChangeFontNameFunction = AutoCSer.Pub.ThisFunction(this, this.ChangeFontName);
			this.ChangeFontSizeFunction = AutoCSer.Pub.ThisFunction(this, this.ChangeFontSize);
			this.SetColorFunction = AutoCSer.Pub.ThisEvent(this, this.SetColor);
			this.OverColorFunction = AutoCSer.Pub.ThisEvent(this, this.OverColor);
			this.SetDesignModeFunction = AutoCSer.Pub.ThisFunction(this, this.SetMode, [true]);
			this.SetInputModeFunction = AutoCSer.Pub.ThisFunction(this, this.SetMode, [false]);
			this.Start(this.Event || AutoCSer.DeclareEvent.Default);
		}
		HtmlEditor.prototype.KeyPress = function (Event) {
			this.OnKeyPress.Function(Event);
			this.CheckHtml();
		};
		HtmlEditor.prototype.KeyDown = function (Event) {
			if (AutoCSer.Pub.IE && Event.Event.ctrlKey) {
				if (Event.keyCode == 90)
					this.ExecCommand(null, 'undo');
				else if (Event.keyCode == 89)
					this.ExecCommand(null, 'redo');
			}
			this.OnKeyDown.Function(Event);
		};
		HtmlEditor.prototype.KeyUp = function (Event) {
			this.OnKeyUp.Function(Event);
			this.CheckHtml();
		};
		HtmlEditor.prototype.MouseMove = function (Event) {
			this.OnMouseMove.Function(Event);
		};
		HtmlEditor.prototype.DoubleClick = function (Event) {
			this.OnDoubleClick.Function(Event);
		};
		HtmlEditor.prototype.OverButton = function (Event) {
			if (this.ButtonOverClassName)
				Event.srcElement.className = this.ButtonOverClassName;
		};
		HtmlEditor.prototype.OutButton = function (Event) {
			if (this.ButtonOutClassName)
				Event.srcElement.className = this.ButtonOutClassName;
		};
		HtmlEditor.prototype.OnCrawlTitle = function (Value) {
			for (var Links = frames[this.EditorFrameId].document.getElementsByTagName('a'), Index = Links.length; Index;) {
				var Link = Links[--Index];
				if (Link.href == Value.Link && AutoCSer.HtmlElement.$Attribute(Link, 'name') == 'AutoCSerEditorLink')
					Link.innerHTML = Value.Title.ToHTML();
			}
		};
		HtmlEditor.prototype.Select = function (Element) {
			frames[this.EditorFrameId].focus();
			if (AutoCSer.Pub.IE) {
				var Range = frames[this.EditorFrameId].document.body.createTextRange();
				Range.moveToElementText(Element.Element0());
				Range.select();
			}
			else {
				var Selection = AutoCSer.HtmlElement.$Id(this.EditorFrameId).Element0()['contentWindow'].getSelection();
				if (Selection.rangeCount) {
					var Range = Selection.getRangeAt(0);
					Selection.removeAllRanges();
					var HtmlElement = Element.Element0();
					Range.setStartBefore(HtmlElement);
					Range.setEndAfter(HtmlElement);
					Selection.addRange(Range);
				}
			}
		};
		HtmlEditor.prototype.ReplaceLink = function (Link) {
			var Value = AutoCSer.CrawlTitle.Get(Link, this.OnCrawlTitleFunction);
			return '<a name="AutoCSerEditorLink" href="' + Value.Link.ToHTML() + '">' + Value.Title.ToHTML() + '</a>';
		};
		HtmlEditor.prototype.PasteFilter = function (Event) {
			if (this.PasteLinkAjaxCallName || this.PasteImageAjaxCallName || this.OnPasteFilter.Get().length) {
				this.SaveRange();
				var Document = frames[this.EditorFrameId].document, Div = Document.createElement('div');
				Div.id = Div['name'] = this.TempId;
				if (AutoCSer.Pub.IE) {
					var SaveFrame = frames[this.SaveFrameId];
					SaveFrame.focus();
					SaveFrame.document.execCommand('selectAll', false, 0);
					SaveFrame.document.execCommand('paste', false, 0);
					Div.style.display = 'none';
					Div.innerHTML = SaveFrame.document.body.innerHTML;
					this.OnPasteFilter.Function([Div]);
					if (this.PasteLinkAjaxCallName)
						this.PasteLink([Div]);
					this.CheckHtml();
					Event.CancelBubble();
				}
				else {
					this.ClipboardImageCount = 0;
					if (this.PasteImageAjaxCallName) {
						var Items = this.ClipboardItems = Event.Event.clipboardData && Event.Event.clipboardData.items;
						if (Items) {
							var IsImage = 0, Form = new FormData();
							for (var Index = 0; Index - Items.length; ++Index) {
								var Item = Items[Index];
								if (Item.kind == 'file' && Item.type.indexOf('image') == 0) {
									var Blob = Item.getAsFile();
									Form.append('i' + (++this.PasteImageIdentity), Blob);
									if (Blob.size)
										IsImage = Blob.size;
									++this.ClipboardImageCount;
								}
							}
							if (IsImage)
								this.PasteImageFormData = Form;
						}
						else {
							var FileItems = Event.Event.clipboardData && Event.Event.clipboardData.files;
							if (FileItems) {
								var IsImage = 0, Form = new FormData();
								for (var Index = 0; Index - FileItems.length; ++Index) {
									var FileItem = FileItems[Index];
									if (FileItem.type.indexOf('image') == 0) {
										Form.append('i' + (++this.PasteImageIdentity), FileItem);
										if (FileItem.size)
											IsImage = FileItem.size;
										++this.ClipboardImageCount;
									}
								}
								if (IsImage)
									this.PasteImageFormData = Form;
							}
						}
					}
					Div.innerHTML = "&nbsp;";
					Div.style.left = "-99999px";
					Div.style.height = Div.style.width = "1px";
					Div.style.position = "absolute";
					Div.style.overflow = "hidden";
					Document.body.appendChild(Div);
					var Range = Document.createRange();
					Range.setStart(Div.firstChild, 0);
					Range.setEnd(Div.firstChild, 1);
					var Selection = AutoCSer.HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection();
					Selection.removeAllRanges();
					Selection.addRange(Range);
					setTimeout(AutoCSer.Pub.ThisFunction(this, this.PasteFilterEnd), 0);
				}
			}
		};
		HtmlEditor.prototype.PasteFilterEnd = function () {
			var Document = frames[this.EditorFrameId].document, Identity = this.PasteImageIdentity, Divs = [];
			if (this.OnPasteFilter.Get().length) {
				for (var Nodes = Document.body.childNodes, Index = 0; Index - Nodes.length; ++Index) {
					var Node = Nodes[Index];
					if (Node.id == this.TempId || AutoCSer.HtmlElement.$Attribute(Node, 'name') == this.TempId)
						Divs.push(Node);
				}
				this.OnPasteFilter.Function(Divs);
			}
			this.CurrentPasteImageIdentity = this.PasteImageIdentity - this.ClipboardImageCount;
			if (this.PasteImageAjaxCallName) {
				for (var Nodes = Document.body.childNodes, Index = 0; Index - Nodes.length; ++Index) {
					var Node = Nodes[Index];
					if (Node.id == this.TempId || AutoCSer.HtmlElement.$Attribute(Node, 'name') == this.TempId)
						this.PasteFilterCheckImage(Node);
				}
				if (Identity == this.PasteImageIdentity && this.ClipboardImageCount == 0)
					this.PasteImageFormData = null;
			}
			for (var Divs = [], Nodes = Document.body.childNodes, Index = Nodes.length; Index;) {
				var Node = Nodes[--Index];
				if (Node.id == this.TempId || AutoCSer.HtmlElement.$Attribute(Node, 'name') == this.TempId) {
					this.PasteFilterCheck(Node, Divs);
					Document.body.removeChild(Node);
				}
			}
			while (this.CurrentPasteImageIdentity < Identity) {
				if (Divs.length == 1 && Divs[0].innerHTML == '&nbsp')
					Divs = [];
				var Div = Document.createElement('div');
				Div.innerHTML = '<img name="AutoCSerEditorImage' + (++this.CurrentPasteImageIdentity) + '" style="display:none" />';
				Divs.push(Div);
			}
			if (Divs.length) {
				this.PasteLink(Divs);
				if (this.PasteImageFormData) {
					this.PasteImageFormData.append('identity', this.PasteImageIdentity);
					var Query = new AutoCSer.HttpRequestQuery(this.PasteImageAjaxCallName, null, AutoCSer.Pub.ThisFunction(this, this.OnUploadImage));
					Query.FormData = this.PasteImageFormData;
					AutoCSer.HttpRequest.PostQuery(Query);
				}
			}
			this.PasteImageFormData = null;
			this.CheckHtml();
		};
		HtmlEditor.prototype.PasteFilterCheckImage = function (Parent) {
			for (var CheckNode, Nodes = Parent.childNodes, Index = 0; Index - Nodes.length; ++Index) {
				var Node = Nodes[Index];
				if (Node.id == this.TempId || AutoCSer.HtmlElement.$Attribute(Node, 'name') == this.TempId)
					this.CheckPasteImage(CheckNode = Node);
			}
			if (!CheckNode)
				this.CheckPasteImage(Parent);
		};
		HtmlEditor.prototype.CheckPasteImage = function (Parent) {
			for (var Nodes = Parent.childNodes, Index = 0; Index - Nodes.length; ++Index) {
				var Node = Nodes[Index];
				if (Node.tagName && Node.tagName.toLowerCase() == 'img') {
					if (Node.src.indexOf('file://') == 0) {
						if (this.ClipboardItems)
							Node['name'] = 'AutoCSerEditorImage' + (++this.CurrentPasteImageIdentity);
						Node.style.display = 'none';
					}
					else if (Node.src.indexOf('data:') == 0) {
						var Match = Node.src.match(/^data\:([^\;]+)\;base64\,(.+)$/);
						if (Match) {
							var Bytes = atob(Match[2]), Codes = [];
							for (var ByteIndex = 0; ByteIndex - Bytes.length; ++ByteIndex)
								Codes.push(Bytes.charCodeAt(ByteIndex));
							if (!this.PasteImageFormData)
								this.PasteImageFormData = new FormData();
							this.PasteImageFormData.append('i' + (++this.PasteImageIdentity), new Blob([new Uint8Array(Codes)], { type: Match[1] }));
							Node.name = 'AutoCSerEditorImage' + this.PasteImageIdentity;
							Node.style.display = 'none';
						}
					}
				}
				else if (Node.nodeType == 1)
					this.CheckPasteImage(Node);
			}
		};
		HtmlEditor.prototype.PasteFilterCheck = function (Parent, Divs) {
			for (var PushNode, Nodes = Parent.childNodes, Index = Nodes.length; Index;) {
				var Node = Nodes[--Index];
				if (Node.id == this.TempId || AutoCSer.HtmlElement.$Attribute(Node, 'name') == this.TempId)
					Divs.push(PushNode = Node);
			}
			if (!PushNode)
				Divs.push(Parent);
		};
		HtmlEditor.prototype.OnUploadImage = function (Value) {
			var Images = Value.Return;
			if (Images) {
				for (var Identity = Value['identity'], Identitys = {}, Index = Images.length; Index; Identitys[(Identity--).toString()] = this.FormatPasteImage ? this.FormatPasteImage(Images[--Index]) : Images[--Index])
					;
				for (var ImageNodes = frames[this.EditorFrameId].document.getElementsByTagName('img'), Index = ImageNodes.length; Index;) {
					var Image = ImageNodes[--Index], Name = Image.name;
					if (Name.substring(0, 21) == 'AutoCSerEditorImage') {
						if (!Image.src || Image.src.indexOf('file://') == 0 || Image.src.indexOf('data:') == 0) {
							var Src = Identitys[Name.substring(21)];
							if (Src) {
								Image.src = Src;
								Image['identity'] = Image.name = Image.style.display = '';
							}
						}
					}
				}
			}
		};
		HtmlEditor.prototype.CutSave = function (IsAll) {
			var SaveFrame = frames[this.SaveFrameId];
			var Body = SaveFrame.document.body, Selection = AutoCSer.HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection();
			Body.innerHTML = '';
			if (Selection.rangeCount)
				Body.appendChild(Selection.getRangeAt(0).cloneContents());
			if (IsAll && Body.innerHTML == '') {
				var EditorFrame = frames[this.EditorFrameId];
				EditorFrame.focus();
				EditorFrame.document.execCommand('selectAll', false, null);
				Selection = AutoCSer.HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection();
				if (Selection.rangeCount)
					Body.appendChild(Selection.getRangeAt(0).extractContents());
			}
			return Body.innerHTML;
		};
		HtmlEditor.prototype.PasteLink = function (Divs) {
			for (var TempDiv = AutoCSer.HtmlElement.$(frames[this.EditorFrameId].document.createElement('div')), Html = [], Index = Divs.length; Index;) {
				var Div = Divs[--Index];
				if (this.PasteLinkAjaxCallName)
					this.PasteCheckLink(Div, TempDiv);
				Html.push(Div.innerHTML);
			}
			this.PasteRange(Html.join('<br />'));
			if (this.PasteLinkAjaxCallName)
				AutoCSer.CrawlTitle.TryRequest(this.PasteLinkAjaxCallName);
		};
		HtmlEditor.prototype.PasteCheckLink = function (Div, TempDiv) {
			for (var Nodes = Div.childNodes, NodeIndex = Nodes.length; NodeIndex;) {
				var Node = Nodes[--NodeIndex];
				if (Node.nodeType == 3) {
					var Text = AutoCSer.HtmlElement.$GetText(Node), NewText = Text.ToHTML().replace(HtmlEditor.PasteLinkRegex, this.ReplaceLinkFunction);
					if (NewText.indexOf('<') + 1) {
						var Texts = NewText.split('<'), Html = [Texts[0]];
						for (var Index = 1; Index < Texts.length;) {
							Html.push('<');
							Html.push(Texts[Index++]);
							Html.push('</a>&nbsp;');
							Html.push(Texts[Index++].substring(3));
						}
						TempDiv.Html(Html.join('')).Child().InsertBefore(Node, Div);
						Div.removeChild(Node);
					}
				}
				else if (Node.nodeType == 1) {
					if (Node.tagName == 'A') {
						if (Node.childNodes.length == 1 && Node.childNodes[0].nodeType == 3 && Node.href == AutoCSer.HtmlElement.$GetText(Node.childNodes[0]) && AutoCSer.HtmlElement.$Attribute(Node, 'name') != 'AutoCSerEditorLink') {
							TempDiv.Html(this.ReplaceLink(Node.href)).Child().InsertBefore(Node, Div);
							Div.removeChild(Node);
						}
					}
					else if (Node.tagName != 'a')
						this.PasteCheckLink(Node, TempDiv);
				}
			}
		};
		HtmlEditor.prototype.GetId = function (Name) {
			return '_' + this.Identity + 'EDITOR_' + Name + '_';
		};
		HtmlEditor.prototype.GetElement = function (Name) {
			return AutoCSer.HtmlElement.$Id(this.ButtonIds ? this.ButtonIds[Name] : Name);
		};
		HtmlEditor.prototype.SetButton = function (Button) {
			this.GetElement(Button.Name).Set('unselectable', 'on').Set('title', Button.Title).AddClass(this.ButtonOutClassName).Cursor('pointer')
				.AddEvent('mouseover', this.OverButtonFunction).AddEvent('mouseout', this.OutButtonFunction).AddEvent('click', Button.OnClick);
		};
		HtmlEditor.prototype.GetSelectionHtml = function (IsAll) {
			if (IsAll === void 0) { IsAll = false; }
			if (AutoCSer.Pub.IE) {
				var EditorFrame = frames[this.EditorFrameId];
				EditorFrame.focus();
				var Document = EditorFrame.document, Html = '', Selection = Document['selection'].createRange();
				if (Selection.htmlText)
					Html = (IsAll && Selection.htmlText.length == 0 ? Document.body.innerHTML : Selection.htmlText);
				else if (IsAll)
					Html = Document.body.innerHTML;
				return Html;
			}
			return this.CutSave(IsAll);
		};
		HtmlEditor.prototype.SaveRange = function () {
			var EditorFrame = frames[this.EditorFrameId];
			EditorFrame.focus();
			if (AutoCSer.Pub.IE) {
				this.SelectRange = EditorFrame.document['selection'].createRange();
				if (this.SelectRange['type'] == 'Control')
					this.SelectRange = this.SelectRange['length'] ? Range['item'](0) : null;
			}
			else
				this.SelectRange = AutoCSer.HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection().getRangeAt(0);
		};
		HtmlEditor.prototype.PasteRange = function (Html, IsSelect) {
			if (IsSelect === void 0) { IsSelect = false; }
			if (!this.SelectRange && IsSelect)
				this.SaveRange();
			if (this.SelectRange) {
				var EditorFrame = frames[this.EditorFrameId];
				EditorFrame.focus();
				if (AutoCSer.Pub.IE) {
					this.SelectRange['pasteHTML'](Html);
					this.SelectRange['select']();
					this.SelectRange = null;
				}
				else {
					var Selection = AutoCSer.HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection();
					this.SelectRange.deleteContents();
					var Div = document.createElement('div');
					Div.style.display = 'none';
					Div.innerHTML = Html;
					EditorFrame.document.body.appendChild(Div);
					for (var Nodes = Div.childNodes, Index = Nodes.length; Index; this.SelectRange.insertNode(Nodes[--Index]))
						;
					EditorFrame.document.body.removeChild(Div);
					this.SelectRange.setStart(this.SelectRange.endContainer, this.SelectRange.endOffset);
					Selection.removeAllRanges();
					Selection.addRange(this.SelectRange);
					this.SelectRange = null;
					EditorFrame.focus();
				}
				this.CheckHtml();
			}
			else
				this.PasteHtml(Html);
		};
		HtmlEditor.prototype.PasteHtml = function (Html, IsAll) {
			if (IsAll === void 0) { IsAll = false; }
			if (AutoCSer.Pub.IE) {
				var EditorFrame = frames[this.EditorFrameId];
				EditorFrame.focus();
				var Document = EditorFrame.document, Selection = Document['selection'].createRange();
				if (Selection.htmlText != null && (!IsAll || Selection.htmlText)) {
					Selection.pasteHTML(Html);
					IsAll = false;
				}
				if (IsAll)
					this.SetHtml(Html);
			}
			else
				this.ExecCommand(null, 'insertHTML', Html);
		};
		HtmlEditor.prototype.GetSelectionText = function (IsAll) {
			if (IsAll === void 0) { IsAll = false; }
			var EditorHtml = AutoCSer.HtmlElement.$IdElement(this.TextAreaId);
			if (AutoCSer.Pub.IE) {
				var Text = EditorHtml['document'].selection.createRange().text;
				return Text == '' && IsAll ? EditorHtml.value : Text;
			}
			var StartIndex = EditorHtml.selectionStart, EndIndex = EditorHtml.selectionEnd;
			return (StartIndex - EndIndex ? EditorHtml.value.substring(StartIndex, EndIndex) : EditorHtml.value);
		};
		HtmlEditor.prototype.PasteText = function (Text, IsAll) {
			if (IsAll === void 0) { IsAll = false; }
			AutoCSer.HtmlElement.$Paste(AutoCSer.HtmlElement.$IdElement(this.TextAreaId), Text, IsAll);
		};
		HtmlEditor.prototype.SelectAll = function () {
			if (AutoCSer.Pub.IE || this.IsDesign)
				this.ExecCommand(null, 'selectAll');
			else {
				var TextArea = AutoCSer.HtmlElement.$Id(this.TextAreaId);
				TextArea.Focus0().Set('selectionStart', 0).Set('selectionEnd', TextArea.Element0().value.length);
			}
		};
		HtmlEditor.prototype.Cut = function () {
			if (!this.ExecCommand(null, 'cut') && !AutoCSer.Pub.IE) {
				if (this.IsDesign)
					this.SaveCode = this.CutSave(false);
				else {
					var Text = AutoCSer.HtmlElement.$IdElement(this.TextAreaId), StartIndex = Text.selectionStart, EndIndex = Text.selectionEnd, OldValue = Text.value;
					this.SaveCode = OldValue.substring(StartIndex, EndIndex);
					Text.value = OldValue.substring(0, StartIndex) + OldValue.substring(EndIndex);
					Text.selectionEnd = Text.selectionStart = StartIndex;
				}
			}
		};
		HtmlEditor.prototype.Copy = function () {
			if (!this.ExecCommand(null, 'copy') && !AutoCSer.Pub.IE) {
				if (this.IsDesign)
					this.PasteHtml(this.SaveCode = this.CutSave(false));
				else
					this.SaveCode = this.GetSelectionText();
			}
		};
		HtmlEditor.prototype.Paste = function () {
			if (!this.ExecCommand(null, 'paste') && !AutoCSer.Pub.IE) {
				if (this.IsDesign)
					this.PasteHtml(this.SaveCode);
				else
					this.PasteText(this.SaveCode);
			}
		};
		HtmlEditor.prototype.ExecCommand = function (Event, Command, Value) {
			if (Value === void 0) { Value = null; }
			try {
				if (this.IsDesign) {
					var EditorFrame = frames[this.EditorFrameId];
					EditorFrame.focus();
					EditorFrame.document.execCommand(Command, false, Value);
				}
				else {
					AutoCSer.HtmlElement.$IdElement(this.TextAreaId).focus();
					document.execCommand(Command, false, Value);
				}
				this.CheckHtml();
			}
			catch (e) {
				return false;
			}
			return true;
		};
		HtmlEditor.prototype.CheckHtml = function () {
			if (this.IsAutoHeight) {
				var Document = frames[this.SaveFrameId].document;
				Document.body.innerHTML = frames[this.EditorFrameId].document.body.innerHTML;
				var Height = Math.max(Document.body.scrollHeight || Document.documentElement.scrollHeight) + this.LineHeight;
				if (this.MaxHeight && Height > this.MaxHeight)
					Height = this.MaxHeight;
				if (this.IsAutoMaxHeight)
					Height = Math.min(Height, AutoCSer.HtmlElement.$Height() - this.AutoPadHeight);
				if (this.GetMaxHeight)
					Height = Math.min(Height, this.GetMaxHeight());
				var HeightPx = Math.max(Height, this.MinHeight) + 'px';
				AutoCSer.HtmlElement.$Id(this.EditorFrameId).Style('height', HeightPx);
				AutoCSer.HtmlElement.$SetStyle(this.Element, 'height', HeightPx);
			}
		};
		HtmlEditor.prototype.AddCode = function (Event, Start, End) {
			this.PasteRange(Start + this.GetSelectionHtml(false) + End, true);
		};
		HtmlEditor.prototype.SetColor = function (Event, IsFore) {
			if (IsFore != null)
				this.ColorFont = IsFore;
			var EditorFrame = frames[this.EditorFrameId];
			EditorFrame.focus();
			var Color = AutoCSer.HtmlElement.$GetValueById(this.GetId('CurrentColor'));
			if (AutoCSer.Pub.IE) {
				EditorFrame.document.execCommand(((IsFore == null ? this.ColorFont : IsFore) ? 'fore' : 'back') + 'color', false, Color);
			}
			else {
				try {
					if (IsFore == null ? this.ColorFont : IsFore)
						EditorFrame.document.execCommand((IsFore == null ? this.ColorFont : IsFore ? 'fore' : 'back') + 'color', false, Color);
					else
						this.AddCode(null, "<font style='background-color:#" + Color + "'>", '</font>');
				}
				catch (e) { }
			}
		};
		HtmlEditor.prototype.OverColor = function (Event, IsOver) {
			this.GetElement(this.ColorFont ? 'FontColor' : 'BgColor').AddClass(IsOver ? this.OverClassName : this.OutClassName);
		};
		HtmlEditor.prototype.ClearCode = function () {
			(this.IsDesign ? frames[this.EditorFrameId] : AutoCSer.HtmlElement.$IdElement(this.TextAreaId)).focus();
			var html = (this.IsDesign ? this.GetSelectionHtml(true) : this.GetSelectionText(true)).replace(/[\r\n\t]/g, '').replace(/<p>/gi, '\n').replace(/<p [^>]*>/g, '\n').replace(/<\/p>/gi, '\r').replace(/<br>/gi, '	').replace(/<br [^>]*>/gi, '	').replace(/<[^>]*>/g, '').replace(/\t/g, '<br />').replace(/\r\n/g, '<p />').replace(/[\r|\n]/g, '<p />');
			if (this.IsDesign)
				this.PasteHtml(html, true);
			else
				this.PasteText(html, true);
		};
		HtmlEditor.prototype.InsertLink = function (Url, Text) {
			if (Text === void 0) { Text = null; }
			this.PasteRange('<a href="' + Url.ToHTML() + '" target="_blank">' + (this.GetSelectionHtml() || Text || Url) + '</a>', true);
		};
		HtmlEditor.prototype.InsertImage = function (Src) {
			if (this.SelectRange)
				this.PasteRange('<img src="' + Src.ToHTML() + '" />');
			else {
				this.ExecCommand(null, 'InsertImage', Src);
				this.CheckHtml();
			}
		};
		HtmlEditor.prototype.Quote = function () {
			this.AddCode(null, '<' + this.QuoteTagName + '>', '</' + this.QuoteTagName + '>');
		};
		HtmlEditor.prototype.Save = function () {
			this.SaveText = (this.IsDesign ? frames[this.EditorFrameId].document.body.innerHTML : AutoCSer.HtmlElement.$IdElement(this.TextAreaId).value);
		};
		HtmlEditor.prototype.LoadSave = function () {
			if (this.IsDesign)
				frames[this.EditorFrameId].document.body.innerHTML = this.SaveText;
			else
				AutoCSer.HtmlElement.$IdElement(this.TextAreaId).value = this.SaveText;
		};
		HtmlEditor.prototype.ClearAll = function () {
			if (this.IsDesign)
				frames[this.EditorFrameId].document.body.innerHTML = '';
			else
				AutoCSer.HtmlElement.$IdElement(this.TextAreaId).value = '';
		};
		HtmlEditor.prototype.FormatCode = function () {
			var OldMode = this.IsDesign;
			this.SetMode(true);
			var Body = frames[this.EditorFrameId].document.body;
			this.SetHtml(Body.innerHTML);
			for (var Spans = new AutoCSer.HtmlElement("/span", Body).GetElements(), Index = Spans.length; Index; AutoCSer.Skin.DeleteMark(Spans[--Index]))
				;
			this.SetMode(OldMode);
		};
		HtmlEditor.prototype.ChangeParagraph = function () {
			var Paragraph = AutoCSer.HtmlElement.$IdElement(this.GetId('paragraph')), Value = Paragraph.options[Paragraph.selectedIndex].value;
			this.AddCode(null, '<' + Value + '>', '</' + Value + '>');
			Paragraph.selectedIndex = 0;
		};
		HtmlEditor.prototype.ChangeFontName = function () {
			var FontName = AutoCSer.HtmlElement.$IdElement(this.GetId('fontName'));
			this.ExecCommand(null, 'fontname', HtmlEditor.FontNames[FontName.selectedIndex - 1]);
			FontName.selectedIndex = 0;
		};
		HtmlEditor.prototype.ChangeFontSize = function () {
			var FontSize = AutoCSer.HtmlElement.$IdElement(this.GetId('fontSize'));
			frames[this.EditorFrameId].document.execCommand('fontsize', false, FontSize.selectedIndex);
			FontSize.selectedIndex = 0;
		};
		HtmlEditor.prototype.SetHtml = function (Html) {
			frames[this.EditorFrameId].document.body.innerHTML = Html || '<br />';
			this.CheckHtml();
			return this;
		};
		HtmlEditor.prototype.SetMode = function (IsDesign, IsFocus) {
			if (IsFocus === void 0) { IsFocus = true; }
			if (IsDesign !== this.IsDesign) {
				for (var Name in this.Buttons) {
					if (this.Buttons[Name].OnlyDesign)
						this.GetElement(this.Buttons[Name].Name).Disabled(!IsDesign).Cursor(IsDesign ? 'pointer' : 'auto');
				}
				this.GetElement('DesignButton').AddClass(IsDesign ? this.OnClassName : this.OffClassName).Disabled(IsDesign);
				this.GetElement('HtmlButton').AddClass(IsDesign ? this.OffClassName : this.OnClassName).Disabled(!IsDesign);
				AutoCSer.HtmlElement.$Id(this.GetId('fontName')).Disabled(!IsDesign);
				AutoCSer.HtmlElement.$Id(this.GetId('fontSize')).Disabled(!IsDesign);
				AutoCSer.HtmlElement.$Id(this.GetId('paragraph')).Disabled(!IsDesign);
				if (this.Color)
					this.Color.Show(IsDesign);
				AutoCSer.HtmlElement.$Id(this.EditorFrameId).Display(IsDesign);
				AutoCSer.HtmlElement.$Id(this.TextAreaId).Display(!IsDesign);
				if (IsDesign) {
					this.SetHtml(this.TextAreaId ? AutoCSer.HtmlElement.$IdElement(this.TextAreaId).value : '');
					if (IsFocus)
						frames[this.EditorFrameId].focus();
				}
				else {
					var TextArea = AutoCSer.HtmlElement.$IdElement(this.TextAreaId);
					TextArea.value = frames[this.EditorFrameId].document.body.innerHTML;
					if (IsFocus)
						TextArea.focus();
				}
				this.IsDesign = IsDesign;
			}
		};
		HtmlEditor.prototype.StartButton = function () {
			this.Buttons = {};
			for (var Index = this.ButtonArray.length; --Index >= 0; this.Buttons[this.ButtonArray[Index].Name] = this.ButtonArray[Index]) {
				if (this.ButtonArray[Index].DefaultSet)
					this.SetButton(this.ButtonArray[Index]);
			}
			var Paragraph = this.GetElement('Paragraph');
			if (Paragraph.Element0()) {
				if (!HtmlEditor.ParagraphOptionHtml) {
					var Values = ('p	h1	h2	h3	h4	h5	h6	h7	pre	address').split('	');
					var Texts = ('普通格式	标题 1	标题 2	标题 3	标题 4	标题 5	标题 6	标题 7	已编排格式	地址').split('	');
					for (var Html = [], Index = -1; ++Index != Values.length;)
						Html.push("<option value='" + Values[Index].ToHTML() + "'>" + Texts[Index].ToHTML() + '</option>');
					HtmlEditor.ParagraphOptionHtml = Html.join('');
				}
				Paragraph.Html("<select id='" + this.GetId('paragraph') + "'><option style='color:green;'>--段落格式--</option>" + HtmlEditor.ParagraphOptionHtml + '</select>');
				AutoCSer.HtmlElement.$Id(this.GetId('paragraph')).AddEvent('change', this.ChangeParagraphFunction);
			}
			var FontName = this.GetElement('FontName');
			if (FontName.Element0()) {
				if (!HtmlEditor.FontNameOptionHtml) {
					HtmlEditor.FontNames = ('宋体	黑体	楷体	仿宋	隶书	幼圆	新宋体	细明体	Arial	Arial Black	Arial Narrow	Bradley Hand ITC	Brush Script MT	Century Gothic	Comic Sans MS	Courier	Courier New	MS Sans Serif	Script	Sys	Times New Roman	Viner Hand ITC	Verdana	Wide Latin	Wingdings').split('	');
					for (var Html = [], Index = -1; ++Index != HtmlEditor.FontNames.length;)
						Html.push("<option value='" + HtmlEditor.FontNames[Index].ToHTML() + "' style='font-family:" + (AutoCSer.Pub.IE ? HtmlEditor.FontNames[Index].ToHTML() : HtmlEditor.FontNames[Index]) + ";'>" + HtmlEditor.FontNames[Index].ToHTML() + '</option>');
					HtmlEditor.FontNameOptionHtml = Html.join('');
				}
				FontName.Html("<select id='" + this.GetId('fontName') + "'><option style='color:green;'>--字体--</option>" + HtmlEditor.FontNameOptionHtml + '</select>');
				AutoCSer.HtmlElement.$Id(this.GetId('fontName')).AddEvent('change', this.ChangeFontNameFunction);
			}
			var FontSize = this.GetElement('FontSize');
			if (FontSize.Element0()) {
				if (!HtmlEditor.FontSizeOptionHtml) {
					for (var Html = [], Index = 1; Index <= 7; Index++)
						Html.push("<option style='font-size:" + (Index * 7) + "px'>" + Index + '</option>');
					HtmlEditor.FontSizeOptionHtml = Html.join('');
				}
				FontSize.Html("<select id='" + this.GetId('fontSize') + "' style='height:20px'><option style='color:green'>--字号--</option>" + HtmlEditor.FontSizeOptionHtml + '</select>');
				AutoCSer.HtmlElement.$Id(this.GetId('fontSize')).AddEvent('change', this.ChangeFontSizeFunction);
			}
			var Color = this.GetElement('Color');
			if (Color.Element0()) {
				var CurrentColor = this.GetElement('CurrentColor');
				if (CurrentColor.Element0())
					CurrentColor = AutoCSer.HtmlElement.$Create('input').Set('size', 6).Set('id', this.GetId('CurrentColor')).Display(0).To(CurrentColor);
				var CurrentColorSpan = this.GetElement('CurrentColorSpan');
				if (CurrentColorSpan.Element0())
					CurrentColorSpan = AutoCSer.HtmlElement.$Create('span').Set('id', this.GetId('CurrentColorSpan')).Display(0).To(CurrentColorSpan);
				CurrentColorSpan.Html('&nbsp;&nbsp;');
				this.Color = new AutoCSer.Color512_64({ Id: Color.Id0(), CurrentColor: CurrentColor.Id0(), CurrentColorSpan: CurrentColorSpan.Id0(), OnClick: this.SetColorFunction, OnOver: this.OverColorFunction });
				this.Color.Start();
				this.SetButton(this.Buttons['FontColor']);
				this.SetButton(this.Buttons['BgColor']);
				this.ColorFont = true;
			}
			this.GetElement('DesignButton').Set('title', '设计模式').AddEvent('click', this.SetDesignModeFunction);
			this.GetElement('HtmlButton').Set('title', 'HTML代码').AddEvent('click', this.SetInputModeFunction);
		};
		HtmlEditor.prototype.Start = function (Event) {
			if (!Event.IsGetOnly) {
				var Element = AutoCSer.HtmlElement.$IdElement(this.Id);
				if (Element != this.Element) {
					this.Element = Element;
					if (!this.MaxHeight)
						this.MaxHeight = parseInt(0 + AutoCSer.HtmlElement.$GetStyle(Element, 'max-height')) || 0;
					if (this.DefaultHtml == null)
						this.DefaultHtml = Element.innerHTML;
					Element.innerHTML = '<iframe id="' + this.EditorFrameId + '" name="' + this.EditorFrameId + '" width="100%" style="height:' + (this.MinHeight = parseInt(0 + AutoCSer.HtmlElement.$GetStyle(Element, 'min-height')) || AutoCSer.HtmlElement.$Height(Element) || 32) + 'px" marginwidth="0" marginheight="0" scroll="no" frameborder="0"></iframe><iframe id="' + this.SaveFrameId + '" name="' + this.SaveFrameId + '" width="100%" height="0px" marginwidth="0" marginheight="0" scroll="no" frameborder="0"></iframe><textarea id="' + this.TextAreaId + '" style="width:100%;height100%;display:none"></textarea>';
					var SaveFrame = frames[this.SaveFrameId], EditorFrame = frames[this.EditorFrameId];
					SaveFrame.document.open();
					SaveFrame.document.write('<html><head>' + (this.Style ? '<link href="' + this.Style + '" rel="stylesheet" type="text/css" />' : '') + '</head><body></body></html>');
					SaveFrame.document.close();
					EditorFrame.document.open();
					EditorFrame.document.write('<html><head>' + (this.Style ? '<link href="' + this.Style + '" rel="stylesheet" type="text/css" />' : '') + '</head><body></body></html>');
					EditorFrame.document.close();
					this.StartButton();
					var Frame = frames[this.EditorFrameId], Document = Frame.document;
					if (AutoCSer.Pub.IE)
						frames[this.SaveFrameId].document.body.contentEditable = Document.body.contentEditable = true;
					else
						frames[this.SaveFrameId].document.designMode = Document.designMode = 'on';
					AutoCSer.HtmlElement.$AddEvent(Document, ['keypress'], AutoCSer.Pub.ThisEvent(this, this.KeyPress, null, Frame));
					AutoCSer.HtmlElement.$AddEvent(Document, ['keydown'], AutoCSer.Pub.ThisEvent(this, this.KeyDown, null, Frame));
					AutoCSer.HtmlElement.$AddEvent(Document, ['keyup'], AutoCSer.Pub.ThisEvent(this, this.KeyUp, null, Frame));
					AutoCSer.HtmlElement.$AddEvent(Document.body, ['mousemove'], AutoCSer.Pub.ThisEvent(this, this.MouseMove, null, Frame));
					AutoCSer.HtmlElement.$AddEvent(Document.body, ['dblclick'], AutoCSer.Pub.ThisEvent(this, this.DoubleClick, null, Frame));
					AutoCSer.HtmlElement.$AddEvent(Document.body, ['paste'], AutoCSer.Pub.ThisEvent(this, this.PasteFilter, null, Frame));
					this.IsDesign = null;
					this.SetMode(true, true);
					this.SetHtml(this.DefaultHtml);
				}
			}
		};
		HtmlEditor.prototype.GetHtml = function () {
			this.FormatCode();
			return this.IsDesign ? frames[this.EditorFrameId].document.body.innerHTML : AutoCSer.HtmlElement.$GetValueById(this.TextAreaId);
		};
		HtmlEditor.prototype.GetHtmlTrimImage = function () {
			this.FormatCode();
			var Html = (this.IsDesign ? frames[this.EditorFrameId].document.body.innerHTML : AutoCSer.HtmlElement.$GetValueById(this.TextAreaId)).Trim();
			return Html.toLowerCase().indexOf('<img ') >= 0 || AutoCSer.HtmlElement.$GetText(frames[this.EditorFrameId].document.body).Trim() ? Html : '';
		};
		HtmlEditor.prototype.GetText = function () {
			this.FormatCode();
			return AutoCSer.HtmlElement.$GetText(frames[this.EditorFrameId].document.body);
		};
		HtmlEditor.prototype.GetXY = function () {
			return AutoCSer.HtmlElement.$Id(this.EditorFrameId).XY0();
		};
		HtmlEditor.prototype.SetForeColor = function (Color) {
			frames[this.EditorFrameId].focus();
			frames[this.EditorFrameId].document.execCommand('forecolor', false, Color);
		};
		HtmlEditor.prototype.Focus = function () {
			(this.IsDesign ? frames[this.EditorFrameId] : AutoCSer.HtmlElement.$IdElement(this.TextAreaId)).focus();
		};
		HtmlEditor.DefaultParameter = { Id: null, Event: null, DefaultHtml: null, ButtonIds: null, PasteLinkAjaxCallName: null, PasteImageAjaxCallName: null, FormatPasteImage: null, IsAutoHeight: true, IsAutoMaxHeight: true, LineHeight: 20, AutoPadHeight: 0, MaxHeight: 0, GetMaxHeight: null, Style: null, QuoteTagName: 'blockquote', ButtonOverClassName: null, ButtonOutClassName: null, OnClassName: null, OffClassName: null, OverClassName: null, OutClassName: null };
		HtmlEditor.DefaultEvents = { OnKeyPress: null, OnKeyDown: null, OnKeyUp: null, OnMouseMove: null, OnDoubleClick: null, OnPasteFilter: null };
		HtmlEditor.PasteLinkRegex = /https?\:\/\/[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]+\??[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]*(#!?)?[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]*/gi;
		HtmlEditor.IsPasteImage = !AutoCSer.Pub.IE && window.atob && window['Blob'] && window['Uint8Array'] && window['FormData'];
		return HtmlEditor;
	}(HtmlEditorParameter));
	AutoCSer.HtmlEditor = HtmlEditor;
	new AutoCSer.Declare(HtmlEditor, 'HtmlEditor', 'click', 'Src');
	AutoCSer.Pub.LoadModule('htmlEditor');
})(AutoCSer || (AutoCSer = {}));
