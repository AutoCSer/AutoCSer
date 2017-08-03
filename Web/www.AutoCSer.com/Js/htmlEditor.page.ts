/// <reference path = "./color.ts" />
/// <reference path = "./crawlTitle.ts" />
/*include:Js\color*/
/*include:Js\crawlTitle*/
module AutoCSer {
    interface IHtmlEditorButton {
        Name: string;
        Title: string;
        OnlyDesign: boolean;
        OnClick: Function;
        DefaultSet: boolean;
    }
    export class HtmlEditorParameter {
        Id: string;
        Event: DeclareEvent;
        DefaultHtml: string;
        ButtonIds: { [key: string]: string };
        PasteLinkAjaxCallName: string;
        PasteImageAjaxCallName: string;
        FormatPasteImage: (string) => string;
        IsAutoHeight: boolean;
        IsAutoMaxHeight: boolean;
        LineHeight: number;
        AutoPadHeight: number;
        MaxHeight: number;
        Style: string;
        QuoteTagName: string;
        ButtonOverClassName: string;
        ButtonOutClassName: string;
        OnClassName: string;
        OffClassName: string;
        OverClassName: string;
        OutClassName: string;
        GetMaxHeight: () => number;
    }
    export class HtmlEditor extends HtmlEditorParameter implements IDeclare {
        private static DefaultParameter: HtmlEditorParameter = { Id: null, Event: null, DefaultHtml: null, ButtonIds: null, PasteLinkAjaxCallName: null, PasteImageAjaxCallName: null, FormatPasteImage: null, IsAutoHeight: true, IsAutoMaxHeight: true, LineHeight: 20, AutoPadHeight: 0, MaxHeight: 0, GetMaxHeight: null, Style: null, QuoteTagName: 'blockquote', ButtonOverClassName: null, ButtonOutClassName: null, OnClassName: null, OffClassName: null, OverClassName: null, OutClassName: null };
        private static DefaultEvents = { OnKeyPress: null, OnKeyDown: null, OnKeyUp: null, OnMouseMove: null, OnDoubleClick: null, OnPasteFilter: null};

        private OnKeyPress: Events;
        private OnKeyDown: Events;
        private OnKeyUp: Events;
        private OnMouseMove: Events;
        private OnDoubleClick: Events;
        private OnPasteFilter: Events;

        private Element: HTMLElement;
        private OverButtonFunction: (Event: Event) => any;
        private OutButtonFunction: (Event: Event) => any;
        private OnCrawlTitleFunction: (Link: ICrawlTitleLink) => any;
        private ReplaceLinkFunction: (string) => string;
        private Identity: number;
        private PasteImageIdentity: number;
        private EditorFrameId: string;
        private SaveFrameId: string;
        private TextAreaId: string;
        private TempId: string;
        private MinHeight: number;
        private SaveText: string;
        private ButtonArray: IHtmlEditorButton[];
        private Buttons: { [key: string]: IHtmlEditorButton };
        private ChangeParagraphFunction: Function;
        private ChangeFontNameFunction: Function;
        private ChangeFontSizeFunction: Function;
        private SetColorFunction: Function;
        private OverColorFunction: Function;
        private SetDesignModeFunction: Function;
        private SetInputModeFunction: Function;
        private Color: Color512_64;
        private ColorFont: boolean;
        private SelectRange: Range;
        private IsDesign: boolean;
        private SaveCode: string;
        private ClipboardImageCount: number;
        private ClipboardItems: DataTransferItemList;
        private PasteImageFormData: FormData;
        private CurrentPasteImageIdentity: number;
        constructor(Parameter: HtmlEditorParameter) {
            super();
            Pub.GetParameter(this, HtmlEditor.DefaultParameter, Parameter);
            Pub.GetEvents(this, HtmlEditor.DefaultEvents, Parameter);
            this.OverButtonFunction = Pub.ThisEvent(this, this.OverButton);
            this.OutButtonFunction = Pub.ThisEvent(this, this.OutButton);
            this.OnCrawlTitleFunction = Pub.ThisFunction(this, this.OnCrawlTitle);
            this.ReplaceLinkFunction = Pub.ThisFunction(this, this.ReplaceLink);
            if (!Pub.IE) {
                this.PasteImageIdentity = 0;
                if (!HtmlEditor.IsPasteImage) this.PasteImageAjaxCallName = null;
            }
            this.Identity = ++Pub.Identity;
            this.EditorFrameId = '_' + this.Identity + 'EDITORIFRAME_';
            this.SaveFrameId = '_' + this.Identity + 'EDITORSAVE_';
            this.TextAreaId = '_' + this.Identity + 'EDITORINPUT_';
            this.TempId = '_' + this.Identity + 'EDITORTEMP_';
            this.SaveText = '';
            this.ButtonArray = ['Name,Title,OnlyDesign,OnClick,DefaultSet',
                ['FontColor', '字体颜色', 1, Pub.ThisEvent(this, this.SetColor, [true]), 0],
                ['BgColor', '字体背景颜色', 1, Pub.ThisEvent(this, this.SetColor, [false]), 0],
                ['ClearCode', '清理代码', 0, Pub.ThisFunction(this, this.ClearCode), 1],
                ['SelectAll', '全选', 0, Pub.ThisFunction(this, this.SelectAll), 1],
                ['Cut', '切剪', 0, Pub.ThisFunction(this, this.Cut), 1],
                ['Copy', '复制', 0, Pub.ThisFunction(this, this.Copy), 1],
                ['Paste', '粘贴', 0, Pub.ThisFunction(this, this.Paste), 1],
                ['Undo', '撤消', 0, Pub.ThisEvent(this, this.ExecCommand, ['undo']), 1],
                ['Redo', '重做', 0, Pub.ThisEvent(this, this.ExecCommand, ['redo']), 1],
                ['Unlink', '去掉超级链接', 1, Pub.ThisEvent(this, this.ExecCommand, ['Unlink']), 1],
                ['InsertHorizontalRule', '插入水平线', 1, Pub.ThisEvent(this, this.ExecCommand, ['InsertHorizontalRule']), 1],
                ['Bold', '加粗', 1, Pub.ThisEvent(this, this.ExecCommand, ['bold']), 1],
                ['Italic', '斜体', 1, Pub.ThisEvent(this, this.ExecCommand, ['italic']), 1],
                ['UnderLine', '下划线', 1, Pub.ThisEvent(this, this.ExecCommand, ['underline']), 1],
                ['Superscript', '上标', 1, Pub.ThisEvent(this, this.ExecCommand, ['superscript']), 1],
                ['Subscript', '下标', 1, Pub.ThisEvent(this, this.ExecCommand, ['subscript']), 1],
                ['StrikeThrough', '删除线', 1, Pub.ThisEvent(this, this.ExecCommand, ['strikethrough']), 1],
                ['RemoveFormat', '取消格式', 1, Pub.ThisEvent(this, this.ExecCommand, ['RemoveFormat']), 1],
                ['JustifyLeft', '左对齐', 1, Pub.ThisEvent(this, this.ExecCommand, ['justifyleft']), 1],
                ['JustifyCenter', '居中', 1, Pub.ThisEvent(this, this.ExecCommand, ['justifycenter']), 1],
                ['JustifyRight', '右对齐', 1, Pub.ThisEvent(this, this.ExecCommand, ['justifyright']), 1],
                ['InsertOrderedList', '编号', 1, Pub.ThisEvent(this, this.ExecCommand, ['insertorderedlist']), 1],
                ['InsertUnOrderedList', '项目符号', 1, Pub.ThisEvent(this, this.ExecCommand, ['insertunorderedlist']), 1],
                ['OutDent', '减少缩进量', 1, Pub.ThisEvent(this, this.ExecCommand, ['outdent']), 1],
                ['InDent', '增加缩进量', 1, Pub.ThisEvent(this, this.ExecCommand, ['indent']), 1],
                ['Quote', '插入引用', 1, Pub.ThisFunction(this, this.Quote), 1],
                ['Save', '保存', 0, Pub.ThisFunction(this, this.Save), 1],
                ['LoadSave', '加载', 0, Pub.ThisFunction(this, this.LoadSave), 1],
                ['ClearAll', '清除', 0, Pub.ThisFunction(this, this.ClearAll), 1],
                ['Print', '打印', 0, Pub.ThisEvent(this, this.ExecCommand, ['print']), 1],
                ['FormatCode', '整理代码', 0, Pub.ThisFunction(this, this.FormatCode), 1]
            ].FormatAjax() as IHtmlEditorButton[];

            this.ChangeParagraphFunction = Pub.ThisFunction(this, this.ChangeParagraph);
            this.ChangeFontNameFunction = Pub.ThisFunction(this, this.ChangeFontName);
            this.ChangeFontSizeFunction = Pub.ThisFunction(this, this.ChangeFontSize);
            this.SetColorFunction = Pub.ThisEvent(this, this.SetColor);
            this.OverColorFunction = Pub.ThisEvent(this, this.OverColor);
            this.SetDesignModeFunction = Pub.ThisFunction(this, this.SetMode, [true]);
            this.SetInputModeFunction = Pub.ThisFunction(this, this.SetMode, [false]);
            this.Start(this.Event || DeclareEvent.Default);
        }
        private KeyPress(Event: BrowserEvent) {
            this.OnKeyPress.Function(Event);
            this.CheckHtml();
        }
        private KeyDown(Event: BrowserEvent) {
            if (Pub.IE && (Event.Event as KeyboardEvent).ctrlKey) {
                if (Event.keyCode == 90) this.ExecCommand(null, 'undo');
                else if (Event.keyCode == 89) this.ExecCommand(null, 'redo');
            }
            this.OnKeyDown.Function(Event);
        }
        private KeyUp(Event: BrowserEvent) {
            this.OnKeyUp.Function(Event);
            this.CheckHtml();
        }
        private MouseMove(Event: BrowserEvent) {
            this.OnMouseMove.Function(Event);
        }
        private DoubleClick(Event: BrowserEvent) {
            this.OnDoubleClick.Function(Event);
        }
        private OverButton(Event: BrowserEvent) {
            if (this.ButtonOverClassName) Event.srcElement.className = this.ButtonOverClassName;
        }
        private OutButton(Event: BrowserEvent) {
            if (this.ButtonOutClassName) Event.srcElement.className = this.ButtonOutClassName;
        }
        private OnCrawlTitle(Value: ICrawlTitleLink) {
            for (var Links = (frames[this.EditorFrameId] as Window).document.getElementsByTagName('a'), Index = Links.length; Index;) {
                var Link = Links[--Index];
                if (Link.href == Value.Link && HtmlElement.$Attribute(Link, 'name') == 'AutoCSerEditorLink') Link.innerHTML = Value.Title.ToHTML();
            }
        }
        Select(Element: HtmlElement) {
            frames[this.EditorFrameId].focus();
            if (Pub.IE) {
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
        }
        private ReplaceLink(Link: string): string{
            var Value = CrawlTitle.Get(Link, this.OnCrawlTitleFunction);
            return '<a name="AutoCSerEditorLink" href="' + Value.Link.ToHTML() + '">' + Value.Title.ToHTML() + '</a>';
        }
        private PasteFilter(Event: BrowserEvent) {
            if (this.PasteLinkAjaxCallName || this.PasteImageAjaxCallName || this.OnPasteFilter.Get().length) {
                this.SaveRange();
                var Document = (frames[this.EditorFrameId] as Window).document, Div = Document.createElement('div');
                Div.id = Div['name'] = this.TempId;
                if (Pub.IE) {
                    var SaveFrame = frames[this.SaveFrameId] as Window;
                    SaveFrame.focus();
                    SaveFrame.document.execCommand('selectAll', false, 0);
                    SaveFrame.document.execCommand('paste', false, 0);
                    Div.style.display = 'none';
                    Div.innerHTML = SaveFrame.document.body.innerHTML;
                    this.OnPasteFilter.Function([Div]);
                    if (this.PasteLinkAjaxCallName) this.PasteLink([Div]);
                    this.CheckHtml();
                    Event.CancelBubble();
                }
                else {
                    this.ClipboardImageCount = 0;
                    if (this.PasteImageAjaxCallName) {
                        var Items = this.ClipboardItems = (Event.Event as ClipboardEvent).clipboardData && (Event.Event as ClipboardEvent).clipboardData.items;
                        if (Items) {
                            var IsImage = 0, Form = new FormData();
                            for (var Index = 0; Index - Items.length; ++Index) {
                                var Item = Items[Index];
                                if (Item.kind == 'file' && Item.type.indexOf('image') == 0) {
                                    var Blob = Item.getAsFile();
                                    Form.append('i' + (++this.PasteImageIdentity), Blob);
                                    if (Blob.size) IsImage = Blob.size;
                                    ++this.ClipboardImageCount;
                                }
                            }
                            if (IsImage) this.PasteImageFormData = Form;
                        }
                        else {
                            var FileItems = (Event.Event as ClipboardEvent).clipboardData && (Event.Event as ClipboardEvent).clipboardData.files;
                            if (FileItems) {
                                var IsImage = 0, Form = new FormData();
                                for (var Index = 0; Index - FileItems.length; ++Index) {
                                    var FileItem = FileItems[Index];
                                    if (FileItem.type.indexOf('image') == 0) {
                                        Form.append('i' + (++this.PasteImageIdentity), FileItem);
                                        if (FileItem.size) IsImage = FileItem.size;
                                        ++this.ClipboardImageCount;
                                    }
                                }
                                if (IsImage) this.PasteImageFormData = Form;
                            }
                        }
                    }
                    Div.innerHTML = "&nbsp;"
                    Div.style.left = "-99999px";
                    Div.style.height = Div.style.width = "1px";
                    Div.style.position = "absolute";
                    Div.style.overflow = "hidden";
                    Document.body.appendChild(Div);
                    var Range = Document.createRange();
                    Range.setStart(Div.firstChild, 0);
                    Range.setEnd(Div.firstChild, 1);
                    var Selection = HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection();
                    Selection.removeAllRanges();
                    Selection.addRange(Range);
                    setTimeout(Pub.ThisFunction(this, this.PasteFilterEnd), 0);
                }
            }
        }
        //NOT IE
        private PasteFilterEnd() {
            var Document = (frames[this.EditorFrameId] as Window).document, Identity = this.PasteImageIdentity, Divs = [];
            if (this.OnPasteFilter.Get().length) {
                for (var Nodes = Document.body.childNodes, Index = 0; Index - Nodes.length; ++Index) {
                    var Node = Nodes[Index] as HTMLElement;
                    if (Node.id == this.TempId || HtmlElement.$Attribute(Node, 'name') == this.TempId) Divs.push(Node);
                }
                this.OnPasteFilter.Function(Divs);
            }
            this.CurrentPasteImageIdentity = this.PasteImageIdentity - this.ClipboardImageCount;
            if (this.PasteImageAjaxCallName) {
                for (var Nodes = Document.body.childNodes, Index = 0; Index - Nodes.length; ++Index) {
                    var Node = Nodes[Index] as HTMLElement;
                    if (Node.id == this.TempId || HtmlElement.$Attribute(Node, 'name') == this.TempId) this.PasteFilterCheckImage(Node);
                }
                if (Identity == this.PasteImageIdentity && this.ClipboardImageCount == 0) this.PasteImageFormData = null;
            }
            for (var Divs = [], Nodes = Document.body.childNodes, Index = Nodes.length; Index;) {
                var Node = Nodes[--Index] as HTMLElement;
                if (Node.id == this.TempId || HtmlElement.$Attribute(Node, 'name') == this.TempId) {
                    this.PasteFilterCheck(Node, Divs);
                    Document.body.removeChild(Node);
                }
            }
            while (this.CurrentPasteImageIdentity < Identity) {
                if (Divs.length == 1 && Divs[0].innerHTML == '&nbsp') Divs = [];
                var Div = Document.createElement('div');
                Div.innerHTML = '<img name="AutoCSerEditorImage' + (++this.CurrentPasteImageIdentity) + '" style="display:none" />';
                Divs.push(Div);
            }
            if (Divs.length) {
                this.PasteLink(Divs);
                if (this.PasteImageFormData) {
                    this.PasteImageFormData.append('identity', this.PasteImageIdentity);
                    var Query = new HttpRequestQuery(this.PasteImageAjaxCallName, null, Pub.ThisFunction(this, this.OnUploadImage));
                    Query.FormData = this.PasteImageFormData;
                    HttpRequest.PostQuery(Query);
                }
            }
            this.PasteImageFormData = null;
            this.CheckHtml();
        }
        private PasteFilterCheckImage(Parent: HTMLElement) {
            for (var CheckNode, Nodes = Parent.childNodes, Index = 0; Index - Nodes.length; ++Index) {
                var Node = Nodes[Index] as HTMLElement;
                if (Node.id == this.TempId || HtmlElement.$Attribute(Node, 'name') == this.TempId) this.CheckPasteImage(CheckNode = Node);
            }
            if (!CheckNode) this.CheckPasteImage(Parent);
        }
        private CheckPasteImage(Parent: HTMLElement) {
            for (var Nodes = Parent.childNodes, Index = 0; Index - Nodes.length; ++Index) {
                var Node = Nodes[Index] as HTMLImageElement;
                if (Node.tagName && Node.tagName.toLowerCase() == 'img') {
                    if (Node.src.indexOf('file://') == 0) {
                        if (this.ClipboardItems) Node['name'] = 'AutoCSerEditorImage' + (++this.CurrentPasteImageIdentity);
                        Node.style.display = 'none';
                    }
                    else if (Node.src.indexOf('data:') == 0) {
                        var Match = Node.src.match(/^data\:([^\;]+)\;base64\,(.+)$/);
                        if (Match) {
                            var Bytes = atob(Match[2]), Codes = [];
                            for (var ByteIndex = 0; ByteIndex - Bytes.length; ++ByteIndex)	Codes.push(Bytes.charCodeAt(ByteIndex));
                            if (!this.PasteImageFormData) this.PasteImageFormData = new FormData();
                            this.PasteImageFormData.append('i' + (++this.PasteImageIdentity), new Blob([new Uint8Array(Codes)], { type: Match[1] }));
                            Node.name = 'AutoCSerEditorImage' + this.PasteImageIdentity;
                            Node.style.display = 'none';
                        }
                    }
                }
                else if (Node.nodeType == 1) this.CheckPasteImage(Node);
            }
        }
        private PasteFilterCheck(Parent: HTMLElement, Divs: HTMLElement[]) {
            for (var PushNode, Nodes = Parent.childNodes, Index = Nodes.length; Index;) {
                var Node = Nodes[--Index] as HTMLElement;
                if (Node.id == this.TempId || HtmlElement.$Attribute(Node, 'name') == this.TempId) Divs.push(PushNode = Node);
            }
            if (!PushNode) Divs.push(Parent);
        }
        private OnUploadImage(Value: IHttpRequestReturn) {
            var Images = Value.__AJAXRETURN__ as string[];
            if (Images) {
                for (var Identity = Value['identity'], Identitys: { [key: string]: string } = {}, Index = Images.length; Index; Identitys[(Identity--).toString()] = this.FormatPasteImage ? this.FormatPasteImage(Images[--Index]) : Images[--Index]);
                for (var ImageNodes = (frames[this.EditorFrameId] as Window).document.getElementsByTagName('img'), Index = ImageNodes.length; Index;) {
                    var Image = ImageNodes[--Index] as HTMLImageElement, Name = Image.name;
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
        }
        private CutSave(IsAll: boolean) {
            var SaveFrame = frames[this.SaveFrameId] as Window;
            var Body = SaveFrame.document.body, Selection = HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection();
            Body.innerHTML = '';
            if (Selection.rangeCount) Body.appendChild(Selection.getRangeAt(0).cloneContents());
            if (IsAll && Body.innerHTML == '') {
                var EditorFrame = frames[this.EditorFrameId] as Window;
                EditorFrame.focus();
                EditorFrame.document.execCommand('selectAll', false, null);
                Selection = HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection();
                if (Selection.rangeCount) Body.appendChild(Selection.getRangeAt(0).extractContents());
            }
            return Body.innerHTML;
        }
        //NOT IE
        private PasteLink(Divs: HTMLElement[]) {
            for (var TempDiv = HtmlElement.$((frames[this.EditorFrameId] as Window).document.createElement('div')), Html = [], Index = Divs.length; Index;) {
                var Div = Divs[--Index];
                if (this.PasteLinkAjaxCallName) this.PasteCheckLink(Div, TempDiv);
                Html.push(Div.innerHTML);
            }
            this.PasteRange(Html.join('<br />'));
            if (this.PasteLinkAjaxCallName) CrawlTitle.TryRequest(this.PasteLinkAjaxCallName);
        }
        private PasteCheckLink(Div: HTMLElement, TempDiv: HtmlElement) {
            for (var Nodes = Div.childNodes, NodeIndex = Nodes.length; NodeIndex;) {
                var Node = Nodes[--NodeIndex] as HTMLLinkElement;
                if (Node.nodeType == 3) {
                    var Text = HtmlElement.$GetText(Node), NewText = Text.ToHTML().replace(HtmlEditor.PasteLinkRegex, this.ReplaceLinkFunction);
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
                        if (Node.childNodes.length == 1 && Node.childNodes[0].nodeType == 3 && Node.href == HtmlElement.$GetText(Node.childNodes[0] as HTMLElement) && HtmlElement.$Attribute(Node, 'name') != 'AutoCSerEditorLink') {
                            TempDiv.Html(this.ReplaceLink(Node.href)).Child().InsertBefore(Node, Div);
                            Div.removeChild(Node);
                        }
                    }
                    else if (Node.tagName != 'a') this.PasteCheckLink(Node, TempDiv);
                }
            }
        }
        private GetId(Name: string): string {
            return '_' + this.Identity + 'EDITOR_' + Name + '_';
        }
        private GetElement(Name: string): HtmlElement {
            return HtmlElement.$Id(this.ButtonIds ? this.ButtonIds[Name] : Name);
        }
        SetButton(Button: IHtmlEditorButton) {
            this.GetElement(Button.Name).Set('unselectable', 'on').Set('title', Button.Title).AddClass(this.ButtonOutClassName).Cursor('pointer')
                .AddEvent('mouseover', this.OverButtonFunction).AddEvent('mouseout', this.OutButtonFunction).AddEvent('click', Button.OnClick);
        }
        private GetSelectionHtml(IsAll = false) {
            if (Pub.IE) {
                var EditorFrame = frames[this.EditorFrameId] as Window;
                EditorFrame.focus();
                var Document = EditorFrame.document, Html = '', Selection = Document['selection'].createRange() as TextRange;
                if (Selection.htmlText) Html = (IsAll && Selection.htmlText.length == 0 ? Document.body.innerHTML : Selection.htmlText);
                else if (IsAll) Html = Document.body.innerHTML;
                return Html;
            }
            return this.CutSave(IsAll);
        }
        private SaveRange() {
            var EditorFrame = frames[this.EditorFrameId] as Window;
            EditorFrame.focus();
            if (Pub.IE) {
                this.SelectRange = EditorFrame.document['selection'].createRange() as Range;
                if (this.SelectRange['type'] == 'Control') this.SelectRange = this.SelectRange['length'] ? Range['item'](0) : null;
            }
            else this.SelectRange = HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection().getRangeAt(0);
        }
        PasteRange(Html: string, IsSelect = false) {
            if (!this.SelectRange && IsSelect) this.SaveRange();
            if (this.SelectRange) {
                var EditorFrame = frames[this.EditorFrameId] as Window;
                EditorFrame.focus();
                if (Pub.IE) {
                    this.SelectRange['pasteHTML'](Html);
                    this.SelectRange['select']();
                    this.SelectRange = null;
                }
                else {
                    var Selection = HtmlElement.$IdElement(this.EditorFrameId)['contentWindow'].getSelection();
                    this.SelectRange.deleteContents();
                    var Div = document.createElement('div');
                    Div.style.display = 'none';
                    Div.innerHTML = Html;
                    EditorFrame.document.body.appendChild(Div);
                    for (var Nodes = Div.childNodes, Index = Nodes.length; Index; this.SelectRange.insertNode(Nodes[--Index]));
                    EditorFrame.document.body.removeChild(Div);
                    this.SelectRange.setStart(this.SelectRange.endContainer, this.SelectRange.endOffset);
                    Selection.removeAllRanges();
                    Selection.addRange(this.SelectRange);
                    this.SelectRange = null;
                    EditorFrame.focus();
                }
                this.CheckHtml();
            }
            else this.PasteHtml(Html);
        }
        private PasteHtml(Html: string, IsAll = false) {
            if (Pub.IE) {
                var EditorFrame = frames[this.EditorFrameId] as Window;
                EditorFrame.focus();
                var Document = EditorFrame.document, Selection = Document['selection'].createRange();
                if (Selection.htmlText != null && (!IsAll || Selection.htmlText)) {
                    Selection.pasteHTML(Html);
                    IsAll = false;
                }
                if (IsAll) this.SetHtml(Html);
            }
            else this.ExecCommand(null, 'insertHTML', Html);
        }
        private GetSelectionText(IsAll = false) {
            var EditorHtml = HtmlElement.$IdElement(this.TextAreaId) as HTMLTextAreaElement;
            if (Pub.IE) {
                var Text = EditorHtml['document'].selection.createRange().text;
                return Text == '' && IsAll ? EditorHtml.value : Text;
            }
            var StartIndex = EditorHtml.selectionStart, EndIndex = EditorHtml.selectionEnd;
            return (StartIndex - EndIndex ? EditorHtml.value.substring(StartIndex, EndIndex) : EditorHtml.value);
        }
        private PasteText(Text: string, IsAll = false) {
            HtmlElement.$Paste(HtmlElement.$IdElement(this.TextAreaId) as HTMLTextAreaElement, Text, IsAll);
        }
        private SelectAll() {
            if (Pub.IE || this.IsDesign) this.ExecCommand(null, 'selectAll');
            else {
                var TextArea = HtmlElement.$Id(this.TextAreaId);
                TextArea.Focus0().Set('selectionStart', 0).Set('selectionEnd', (TextArea.Element0() as HTMLTextAreaElement).value.length);
            }
        }
        private Cut() {
            if (!this.ExecCommand(null, 'cut') && !Pub.IE) {
                if (this.IsDesign) this.SaveCode = this.CutSave(false);
                else {
                    var Text = HtmlElement.$IdElement(this.TextAreaId) as HTMLTextAreaElement, StartIndex = Text.selectionStart, EndIndex = Text.selectionEnd, OldValue = Text.value;
                    this.SaveCode = OldValue.substring(StartIndex, EndIndex);
                    Text.value = OldValue.substring(0, StartIndex) + OldValue.substring(EndIndex);
                    Text.selectionEnd = Text.selectionStart = StartIndex;
                }
            }
        }
        private Copy() {
            if (!this.ExecCommand(null, 'copy') && !Pub.IE) {
                if (this.IsDesign) this.PasteHtml(this.SaveCode = this.CutSave(false));
                else this.SaveCode = this.GetSelectionText();
            }
        }
        private Paste() {
            if (!this.ExecCommand(null, 'paste') && !Pub.IE) {
                if (this.IsDesign) this.PasteHtml(this.SaveCode);
                else this.PasteText(this.SaveCode);
            }
        }
        private ExecCommand(Event: BrowserEvent, Command: string, Value: any = null): boolean{
            try {
                if (this.IsDesign) {
                    var EditorFrame = frames[this.EditorFrameId] as Window;
                    EditorFrame.focus();
                    EditorFrame.document.execCommand(Command, false, Value);
                }
                else {
                    HtmlElement.$IdElement(this.TextAreaId).focus();
                    document.execCommand(Command, false, Value);
                }
                this.CheckHtml();
            }
            catch (e) {
                return false;
            }
            return true;
        }
        private CheckHtml() {
            if (this.IsAutoHeight) {
                var Document = (frames[this.SaveFrameId] as Window).document;
                Document.body.innerHTML = (frames[this.EditorFrameId] as Window).document.body.innerHTML;
                var Height = Math.max(Document.body.scrollHeight || Document.documentElement.scrollHeight) + this.LineHeight;
                if (this.MaxHeight && Height > this.MaxHeight) Height = this.MaxHeight;
                if (this.IsAutoMaxHeight) Height = Math.min(Height, HtmlElement.$Height() - this.AutoPadHeight);
                if (this.GetMaxHeight) Height = Math.min(Height, this.GetMaxHeight());
                var HeightPx = Math.max(Height, this.MinHeight) + 'px';
                HtmlElement.$Id(this.EditorFrameId).Style('height', HeightPx);
                HtmlElement.$SetStyle(this.Element, 'height', HeightPx);
            }
        }
        private AddCode(Event: BrowserEvent, Start: string, End: string) {
            this.PasteRange(Start + this.GetSelectionHtml(false) + End, true);
        }
        private SetColor(Event: BrowserEvent, IsFore: boolean) {
            if (IsFore != null) this.ColorFont = IsFore;
            var EditorFrame = frames[this.EditorFrameId] as Window;
            EditorFrame.focus();
            var Color = HtmlElement.$GetValueById(this.GetId('CurrentColor'));
            if (Pub.IE) {
                EditorFrame.document.execCommand(((IsFore == null ? this.ColorFont : IsFore) ? 'fore' : 'back') + 'color', false, Color);
            }
            else {
                try {
                    if (IsFore == null ? this.ColorFont : IsFore) EditorFrame.document.execCommand((IsFore == null ? this.ColorFont : IsFore ? 'fore' : 'back') + 'color', false, Color);
                    else this.AddCode(null, "<font style='background-color:#" + Color + "'>", '</font>');
                }
                catch (e) { }
            }
        }
        private OverColor(Event: BrowserEvent, IsOver: boolean) {
            this.GetElement(this.ColorFont ? 'FontColor' : 'BgColor').AddClass(IsOver ? this.OverClassName : this.OutClassName);
        }
        private ClearCode() {
            (this.IsDesign ? frames[this.EditorFrameId] as Window : HtmlElement.$IdElement(this.TextAreaId)).focus();
            var html = (this.IsDesign ? this.GetSelectionHtml(true) : this.GetSelectionText(true)).replace(/[\r\n\t]/g, '').replace(/<p>/gi, '\n').replace(/<p [^>]*>/g, '\n').replace(/<\/p>/gi, '\r').replace(/<br>/gi, '	').replace(/<br [^>]*>/gi, '	').replace(/<[^>]*>/g, '').replace(/\t/g, '<br />').replace(/\r\n/g, '<p />').replace(/[\r|\n]/g, '<p />');
            if (this.IsDesign) this.PasteHtml(html, true);
            else this.PasteText(html, true);
        }
        private InsertLink(Url: string, Text: string = null) {
            this.PasteRange('<a href="' + Url.ToHTML() + '" target="_blank">' + (this.GetSelectionHtml() || Text || Url) + '</a>', true);
        }
        InsertImage(Src: string) {
            if (this.SelectRange) this.PasteRange('<img src="' + Src.ToHTML() + '" />');
            else {
                this.ExecCommand(null, 'InsertImage', Src);
                this.CheckHtml();
            }
        }
        private Quote() {
            this.AddCode(null, '<' + this.QuoteTagName + '>', '</' + this.QuoteTagName + '>');
        }
        private Save() {
            this.SaveText = (this.IsDesign ? (frames[this.EditorFrameId] as Window).document.body.innerHTML : (HtmlElement.$IdElement(this.TextAreaId) as HTMLTextAreaElement).value);
        }
        private LoadSave() {
            if (this.IsDesign) (frames[this.EditorFrameId] as Window).document.body.innerHTML = this.SaveText;
            else (HtmlElement.$IdElement(this.TextAreaId) as HTMLTextAreaElement).value = this.SaveText;
        }
        private ClearAll() {
            if (this.IsDesign) (frames[this.EditorFrameId] as Window).document.body.innerHTML = '';
            else (HtmlElement.$IdElement(this.TextAreaId) as HTMLTextAreaElement).value = '';
        }
        private FormatCode() {
            var OldMode = this.IsDesign;
            this.SetMode(true);
            var Body = (frames[this.EditorFrameId] as Window).document.body;
            this.SetHtml(Body.innerHTML);
            for (var Spans = new HtmlElement("/span", Body).GetElements(), Index = Spans.length; Index; Skin.DeleteMark(Spans[--Index]));
            this.SetMode(OldMode);
        }
        private ChangeParagraph() {
            var Paragraph = HtmlElement.$IdElement(this.GetId('paragraph')) as HTMLSelectElement, Value = (Paragraph.options[Paragraph.selectedIndex] as HTMLInputElement).value;
            this.AddCode(null, '<' + Value + '>', '</' + Value + '>');
            Paragraph.selectedIndex = 0;
        }
        private ChangeFontName() {
            var FontName = HtmlElement.$IdElement(this.GetId('fontName')) as HTMLSelectElement;
            this.ExecCommand(null, 'fontname', HtmlEditor.FontNames[FontName.selectedIndex - 1]);
            FontName.selectedIndex = 0;
        }
        private ChangeFontSize() {
            var FontSize = HtmlElement.$IdElement(this.GetId('fontSize')) as HTMLSelectElement;
            (frames[this.EditorFrameId] as Window).document.execCommand('fontsize', false, FontSize.selectedIndex);
            FontSize.selectedIndex = 0;
        }
        SetHtml(Html: string): HtmlEditor {
            (frames[this.EditorFrameId] as Window).document.body.innerHTML = Html || '<br />';
            this.CheckHtml();
            return this;
        }
        private SetMode(IsDesign: boolean, IsFocus: boolean = true) {
            if (IsDesign !== this.IsDesign) {
                for (var Name in this.Buttons) {
                    if (this.Buttons[Name].OnlyDesign) this.GetElement(this.Buttons[Name].Name).Disabled(!IsDesign).Cursor(IsDesign ? 'pointer' : 'auto');
                }
                this.GetElement('DesignButton').AddClass(IsDesign ? this.OnClassName : this.OffClassName).Disabled(IsDesign);
                this.GetElement('HtmlButton').AddClass(IsDesign ? this.OffClassName : this.OnClassName).Disabled(!IsDesign);
                HtmlElement.$Id(this.GetId('fontName')).Disabled(!IsDesign);
                HtmlElement.$Id(this.GetId('fontSize')).Disabled(!IsDesign);
                HtmlElement.$Id(this.GetId('paragraph')).Disabled(!IsDesign);
                if (this.Color) this.Color.Show(IsDesign);
                HtmlElement.$Id(this.EditorFrameId).Display(IsDesign);
                HtmlElement.$Id(this.TextAreaId).Display(!IsDesign);
                if (IsDesign) {
                    this.SetHtml(this.TextAreaId ? (HtmlElement.$IdElement(this.TextAreaId) as HTMLTextAreaElement).value : '');
                    if (IsFocus) (frames[this.EditorFrameId] as Window).focus();
                }
                else {
                    var TextArea = HtmlElement.$IdElement(this.TextAreaId) as HTMLTextAreaElement;
                    TextArea.value = (frames[this.EditorFrameId] as Window).document.body.innerHTML;
                    if (IsFocus) TextArea.focus();
                }
                this.IsDesign = IsDesign;
            }
        }
        private StartButton() {
            this.Buttons = {};
            for (var Index = this.ButtonArray.length; --Index >= 0; this.Buttons[this.ButtonArray[Index].Name] = this.ButtonArray[Index]) {
                if (this.ButtonArray[Index].DefaultSet) this.SetButton(this.ButtonArray[Index]);
            }
            var Paragraph = this.GetElement('Paragraph');
            if (Paragraph.Element0()) {
                if (!HtmlEditor.ParagraphOptionHtml) {
                    var Values = ('p	h1	h2	h3	h4	h5	h6	h7	pre	address').split('	');
                    var Texts = ('普通格式	标题 1	标题 2	标题 3	标题 4	标题 5	标题 6	标题 7	已编排格式	地址').split('	');
                    for (var Html = [], Index = -1; ++Index != Values.length;)	Html.push("<option value='" + Values[Index].ToHTML() + "'>" + Texts[Index].ToHTML() + '</option>');
                    HtmlEditor.ParagraphOptionHtml = Html.join('');
                }
                Paragraph.Html("<select id='" + this.GetId('paragraph') + "'><option style='color:green;'>--段落格式--</option>" + HtmlEditor.ParagraphOptionHtml + '</select>');
                HtmlElement.$Id(this.GetId('paragraph')).AddEvent('change', this.ChangeParagraphFunction);
            }
            var FontName = this.GetElement('FontName');
            if (FontName.Element0()) {
                if (!HtmlEditor.FontNameOptionHtml) {
                    HtmlEditor.FontNames = ('宋体	黑体	楷体	仿宋	隶书	幼圆	新宋体	细明体	Arial	Arial Black	Arial Narrow	Bradley Hand ITC	Brush Script MT	Century Gothic	Comic Sans MS	Courier	Courier New	MS Sans Serif	Script	Sys	Times New Roman	Viner Hand ITC	Verdana	Wide Latin	Wingdings').split('	');
                    for (var Html = [], Index = -1; ++Index != HtmlEditor.FontNames.length;)	Html.push("<option value='" + HtmlEditor.FontNames[Index].ToHTML() + "' style='font-family:" + (Pub.IE ? HtmlEditor.FontNames[Index].ToHTML() : HtmlEditor.FontNames[Index]) + ";'>" + HtmlEditor.FontNames[Index].ToHTML() + '</option>');
                    HtmlEditor.FontNameOptionHtml = Html.join('');
                }
                FontName.Html("<select id='" + this.GetId('fontName') + "'><option style='color:green;'>--字体--</option>" + HtmlEditor.FontNameOptionHtml + '</select>');
                HtmlElement.$Id(this.GetId('fontName')).AddEvent('change', this.ChangeFontNameFunction);
            }
            var FontSize = this.GetElement('FontSize');
            if (FontSize.Element0()) {
                if (!HtmlEditor.FontSizeOptionHtml) {
                    for (var Html = [], Index = 1; Index <= 7; Index++)	Html.push("<option style='font-size:" + (Index * 7) + "px'>" + Index + '</option>');
                    HtmlEditor.FontSizeOptionHtml = Html.join('');
                }
                FontSize.Html("<select id='" + this.GetId('fontSize') + "' style='height:20px'><option style='color:green'>--字号--</option>" + HtmlEditor.FontSizeOptionHtml + '</select>');
                HtmlElement.$Id(this.GetId('fontSize')).AddEvent('change', this.ChangeFontSizeFunction);
            }
            var Color = this.GetElement('Color');
            if (Color.Element0()) {
                var CurrentColor = this.GetElement('CurrentColor');
                if (CurrentColor.Element0()) CurrentColor = HtmlElement.$Create('input').Set('size', 6).Set('id', this.GetId('CurrentColor')).Display(0).To(CurrentColor);
                var CurrentColorSpan = this.GetElement('CurrentColorSpan');
                if (CurrentColorSpan.Element0()) CurrentColorSpan = HtmlElement.$Create('span').Set('id', this.GetId('CurrentColorSpan')).Display(0).To(CurrentColorSpan);
                CurrentColorSpan.Html('&nbsp;&nbsp;');
                this.Color = new Color512_64({ Id: Color.Id0(), CurrentColor: CurrentColor.Id0(), CurrentColorSpan: CurrentColorSpan.Id0(), OnClick: this.SetColorFunction, OnOver: this.OverColorFunction });
                this.Color.Start();
                this.SetButton(this.Buttons['FontColor']);
                this.SetButton(this.Buttons['BgColor']);
                this.ColorFont = true;
            }
            this.GetElement('DesignButton').Set('title', '设计模式').AddEvent('click', this.SetDesignModeFunction);
            this.GetElement('HtmlButton').Set('title', 'HTML代码').AddEvent('click', this.SetInputModeFunction);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                var Element = HtmlElement.$IdElement(this.Id);
                if (Element != this.Element) {
                    this.Element = Element;
                    if (!this.MaxHeight) this.MaxHeight = parseInt(0 + HtmlElement.$GetStyle(Element, 'max-height')) || 0;
                    if (this.DefaultHtml == null) this.DefaultHtml = Element.innerHTML;
                    Element.innerHTML = '<iframe id="' + this.EditorFrameId + '" name="' + this.EditorFrameId + '" width="100%" style="height:' + (this.MinHeight = parseInt(0 + HtmlElement.$GetStyle(Element, 'min-height')) || HtmlElement.$Height(Element) || 32) + 'px" marginwidth="0" marginheight="0" scroll="no" frameborder="0"></iframe><iframe id="' + this.SaveFrameId + '" name="' + this.SaveFrameId + '" width="100%" height="0px" marginwidth="0" marginheight="0" scroll="no" frameborder="0"></iframe><textarea id="' + this.TextAreaId + '" style="width:100%;height100%;display:none"></textarea>';
                    var SaveFrame = frames[this.SaveFrameId] as Window, EditorFrame = frames[this.EditorFrameId] as Window;
                    SaveFrame.document.open();
                    SaveFrame.document.write('<html><head>' + (this.Style ? '<link href="' + this.Style + '" rel="stylesheet" type="text/css" />' : '') + '</head><body></body></html>');
                    SaveFrame.document.close();
                    EditorFrame.document.open();
                    EditorFrame.document.write('<html><head>' + (this.Style ? '<link href="' + this.Style + '" rel="stylesheet" type="text/css" />' : '') + '</head><body></body></html>');
                    EditorFrame.document.close();
                    this.StartButton();
                    var Frame = frames[this.EditorFrameId] as Window, Document = Frame.document;
                    if (Pub.IE) (frames[this.SaveFrameId] as Window).document.body.contentEditable = Document.body.contentEditable = true as Object as string;
                    else (frames[this.SaveFrameId] as Window).document.designMode = Document.designMode = 'on';
                    HtmlElement.$AddEvent(Document as Object as HTMLElement, ['keypress'], Pub.ThisEvent(this, this.KeyPress, null, Frame));
                    HtmlElement.$AddEvent(Document as Object as HTMLElement, ['keydown'], Pub.ThisEvent(this, this.KeyDown, null, Frame));
                    HtmlElement.$AddEvent(Document as Object as HTMLElement, ['keyup'], Pub.ThisEvent(this, this.KeyUp, null, Frame));
                    HtmlElement.$AddEvent(Document.body, ['mousemove'], Pub.ThisEvent(this, this.MouseMove, null, Frame));
                    HtmlElement.$AddEvent(Document.body, ['dblclick'], Pub.ThisEvent(this, this.DoubleClick, null, Frame));
                    HtmlElement.$AddEvent(Document.body, ['paste'], Pub.ThisEvent(this, this.PasteFilter, null, Frame));
                    this.IsDesign = null;
                    this.SetMode(true, true);
                    this.SetHtml(this.DefaultHtml);
                }
            }
        }
        GetHtml(): string {
            this.FormatCode();
            return this.IsDesign ? (frames[this.EditorFrameId] as Window).document.body.innerHTML : HtmlElement.$GetValueById(this.TextAreaId);
        }
        GetHtmlTrimImage(): string {
            this.FormatCode();
            var Html = (this.IsDesign ? (frames[this.EditorFrameId] as Window).document.body.innerHTML : HtmlElement.$GetValueById(this.TextAreaId)).Trim();
            return Html.toLowerCase().indexOf('<img ') >= 0 || HtmlElement.$GetText(frames[this.EditorFrameId].document.body).Trim() ? Html : '';
        }
        GetText(): string{
            this.FormatCode();
            return HtmlElement.$GetText(frames[this.EditorFrameId].document.body);
        }
        GetXY(): IPointer{
            return HtmlElement.$Id(this.EditorFrameId).XY0();
        }
        SetForeColor(Color: string) {
            frames[this.EditorFrameId].focus();
            frames[this.EditorFrameId].document.execCommand('forecolor', false, Color);
        }
        Focus() {
            (this.IsDesign ? frames[this.EditorFrameId] : HtmlElement.$IdElement(this.TextAreaId)).focus();
        }
        static ParagraphOptionHtml: string;
        static FontNames: string[];
        static FontNameOptionHtml: string;
        static FontSizeOptionHtml: string;
        static PasteLinkRegex = /https?\:\/\/[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]+\??[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]*(#!?)?[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]*/gi;
        static IsPasteImage = !Pub.IE && window.atob && window['Blob'] && window['Uint8Array'] && window['FormData'];
    }
    new AutoCSer.Declare(HtmlEditor, 'HtmlEditor', 'click', 'Src');
    Pub.LoadModule('htmlEditor');
}