/*include:js\overIframe*/
/*include:js\color*/
/*include:js\crawlTitle*/
//HTML编辑器组件核心
//fastCSharp.NewObject('HtmlEditor',{ElementId:'XXX'}).Start();
(function(){
var HtmlEditor=function()
	{
	this.GetArgument(arguments[0],{Element:null,ElementId:null,DefaultHtml:null,Path:fastCSharp.JsDomain+'js/htmlEditor/',ButtonIds:null,OnKeyPress:null,OnKeyDown:null,OnKeyUp:null,PasteLinkAjaxCallName:null,PasteImageAjaxCallName:null,FormatPasteImage:null,OnPasteFilter:null,OnMouseMove:null,IsAutoHeight:1,MaxHeight:0,GetMaxHeight:null,Style:null,QuoteTagName:null,OnInsertImage:null},1);
	this.OnKeyPress=fastCSharp.Event().Add(this.OnKeyPress);
	this.OnKeyDown=fastCSharp.Event().Add(this.OnKeyDown);
	this.OnKeyUp=fastCSharp.Event().Add(this.OnKeyUp);
	this.OnMouseMove=fastCSharp.Event().Add(this.OnMouseMove);
	this.OnDoubleClick=fastCSharp.Event().Add(this.OnDoubleClick);
	this.OnPasteFilter=fastCSharp.Event().Add(this.OnPasteFilter);
	this.OverButtonFunction=fastCSharp.ThisEvent(this,this.OverButton);
	this.OutButtonFunction=fastCSharp.ThisEvent(this,this.OutButton);
	this.OnCrawlTitleFunction=fastCSharp.ThisFunction(this,this.OnCrawlTitle);
	this.ReplaceLinkFunction=fastCSharp.ThisFunction(this,this.ReplaceLink);
#if(IE){
}#else{
	this.PasteImageIdentity=0;
	if(!HtmlEditor.IsPasteImage)	this.PasteImageAjaxCallName=null;
}#endif
	if(!this.QuoteTagName)	this.QuoteTagName='blockquote';
	if(this.Element)	this.Element=this.$(this.Element);
	else	this.Element=this.$Id(this.ElementId);
	if(!this.MaxHeight)	this.MaxHeight=parseInt(0+this.Element.Css('max-height'))||0;
	if(this.DefaultHtml==null)	this.DefaultHtml=this.Element.Html();
	this.EditorFrame=this.GetId('Iframe');
	this.SaveFrame=this.GetId('save');
	this.EditorHtml=this.GetId('input');
	this.Element.Html('<iframe id="'+this.EditorFrame+'" name="'+this.EditorFrame+'" width="100%" style="height:'+(this.MinHeight=parseInt(0+this.Element.Css('min-height'))||this.Element.Height()||32)+'px" marginwidth="0" marginheight="0" scroll="no" frameborder="0"></iframe><iframe id="'+this.SaveFrame+'" name="'+this.SaveFrame+'" width="100%" height="0px" marginwidth="0" marginheight="0" scroll="no" frameborder="0"></iframe><textarea id="'+this.EditorHtml+'" style="width:100%;height100%;display:none"></textarea>').Display(1);
	this.Element.HtmlEditor=this;
//	this.MinHeight=this.$Id(this.EditorFrame).Height(); height="100%"
	frames[this.SaveFrame].document.open();
	frames[this.SaveFrame].document.write('<html><head>'+(this.Style?'<link href="'+this.Style+'" rel="stylesheet" type="text/css" />':'')+'</head><body></body></html>');
	frames[this.SaveFrame].document.close();
	frames[this.EditorFrame].document.open();
	frames[this.EditorFrame].document.write('<html><head>'+(this.Style?'<link href="'+this.Style+'" rel="stylesheet" type="text/css" />':'')+'</head><body></body></html>');
	frames[this.EditorFrame].document.close();
	this.Htmls={};
	var Htmls=['Char','Replace','Mood','Help','Multimedia','Table'];
	for(var Index=Htmls.length;--Index>=0;)	this.Htmls[Htmls[Index]]=this.Path+Htmls[Index]+'.htm#id='+this.SysId;
	this.TempId=this.GetId('temp');
	this.SaveText='';
	};
HtmlEditor.PasteLinkRegex=/https?\:\/\/[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]+\??[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]*(#!?)?[a-z0-9\/~@%&_,;'=\$\^\(\)\+\{\}\.\[\]\-]*/gi;
#if(IE){
}#else{
HtmlEditor.IsPasteImage=window.atob&&window.Blob&&window.Uint8Array&&window.FormData;
}#endif
(fastCSharp.Functions.HtmlEditor=HtmlEditor).Inherit(fastCSharp.BaseFunction,{
#if(IE){
Close:function()
	{
	this.SelectRange=null;
	},
}#else{
}#endif
GetElement:function(Button)
	{
	return this.$Id(this.ButtonIds?this.ButtonIds[Button.Name]:Button.Name);
	},
SetButton:function(Button)
	{
	this.GetElement(Button).Set('unselectable','on').Set('title',Button.Title).Class(this.ButtonOutStyle).Cursor('pointer')
		.AddEvent('mouseover',this.OverButtonFunction).AddEvent('mouseout',this.OutButtonFunction).AddEvent('click',Button.OnClick);
	},
OverButton:function(Event)
	{
	if(this.ButtonOverStyle)	Event.srcElement.className=this.ButtonOverStyle;
	},
OutButton:function(Event)
	{
	if(this.ButtonOutStyle)	Event.srcElement.className=this.ButtonOutStyle;
	},
ClearCode:function()
	{
	(this.IsDesign?frames[this.EditorFrame]:this.$Id(this.EditorHtml).Element()).focus();
	var html=(this.IsDesign?this.GetSelectionHtml(true):this.GetSelectionText(true)).replace(/[\r\n\t]/g,'').replace(/<p>/gi,'\n').replace(/<p [^>]*>/g,'\n').replace(/<\/p>/gi,'\r').replace(/<br>/gi,'	').replace(/<br [^>]*>/gi,'	').replace(/<[^>]*>/g,'').replace(/\t/g,'<br />').replace(/\r\n/g,'<p />').replace(/[\r|\n]/g,'<p />');
	if(this.IsDesign)	this.PasteHtml(html,1);
	else	this.PasteText(html,1);
	},
SelectAll:function()
	{
#if(IE){
	this.ExecCommand(0,'selectAll');
}#else{
	if(this.IsDesign)	this.ExecCommand(0,'selectAll');
	else	this.$Id(this.EditorHtml).Focus().Set('selectionStart',0).Set('selectionEnd',Text.value.length);
}#endif
	},
Cut:function()
	{
#if(IE){
	this.ExecCommand(0,'cut');
}#else{
	if(!this.ExecCommand(0,'cut'))
		{
		if(this.IsDesign)	this.SaveCode=this.CutSave(false);
		else	{
			var Text=this.$Id(this.EditorHtml),StartIndex=Text.Get('selectionStart'),EndIndex=Text.Get('selectionEnd'),OldValue=Text.Value();
			this.SaveCode=OldValue.substring(StartIndex,EndIndex);
			Text.Value(OldValue.substring(0,StartIndex)+OldValue.substring(EndIndex)).Set('selectionStart',StartIndex).Set('selectionEnd',StartIndex);
			}
		}
}#endif
	},
Copy:function()
	{
#if(IE){
	this.ExecCommand(0,'copy');
}#else{
	if(!this.ExecCommand(0,'copy'))
		{
		if(this.IsDesign)	this.PasteHtml(this.SaveCode=this.CutSave(false));
		else	this.SaveCode=this.GetSelectionText(false);
		}
}#endif
	},
Paste:function()
	{
#if(IE){
	this.ExecCommand(0,'paste');
}#else{
	if(!this.ExecCommand(0,'paste'))
		{
		if(this.IsDesign)	this.PasteHtml(this.SaveCode);
		else	this.PasteText(Editor.SaveCode,false);
		}
}#endif
	},
FormatUrl:function(Url)
	{
	return Url.indexOf('://')+1?Url:('http://'+Url);
	},
CreateLink:function()
	{
	var Url=prompt('Please enter the link address:','http://');
	if(Url)	{
		Url=this.FormatUrl(Url).ToHTML();
		Html=this.GetSelectionHtml();
		this.PasteRange('<a href="'+Url+'" target="_blank">'+(Html||Url)+'</a>',1);
//		this.PasteHtml('<a href="'+Url+'" target="_blank">'+(Html||Url)+'</a>',0);
		}
	},
InsertImage:function(Src)
	{
	var IsSrc=typeof(Src)=='string';
	if(this.OnInsertImage&&!IsSrc)
		{
		this.SaveRange();
		this.OnInsertImage(this);
		}
	else	{
		var Url=IsSrc?Src:prompt('Please enter the picture Address:','http://');
		if(Url)	{
#if(IE){
			if(this.SelectRange)	this.PasteRange('<img src="'+Url.ToHTML()+'" />');
			else	{
				this.ExecCommand(null,'InsertImage',this.FormatUrl(Url));
				this.CheckHtml();
				}
//				{
//				frames[this.EditorFrame].focus();
//				frames[this.EditorFrame].document.execCommand('InsertImage',false,this.FormatUrl(Url));
//				this.CheckHtml();
//				}
}#else{
			if(this.SelectRange)	this.PasteRange('<img src="'+Url.ToHTML()+'" />');
			else	{
				this.ExecCommand(null,'InsertImage',this.FormatUrl(Url));
				this.CheckHtml();
				}
//			frames[this.EditorFrame].focus();
//			frames[this.EditorFrame].document.execCommand('InsertImage',false,this.FormatUrl(Url));
//			this.CheckHtml();
}#endif
			}
		}
	},
InsertRow:function()
	{
	var Element=this.GetElementByTags({table:1,tbody:1,tr:1,td:1});
	if(Element)
		{
		var TagName=Element.tagName.toLowerCase(),Row=null,RowIndex=0,ColIndex=0,Table=Element,NewRow;
		if(TagName=='td')
			{
			Row=this.$Parent(Element);
			ColIndex=Element.cellIndex;
			}
		else if(TagName=='tr')	Row=Element;
		else if(TagName=='tbody')	Table=this.$Parent(Table);
		if(Row)	{
			RowIndex=Row.rowIndex;
			Table=this.$Parent(this.$Parent(Row));
			}
#if(IE){
		var NewTable=Table.cloneNode(true);
		NewRow=NewTable.insertRow(RowIndex+1);
		for(var Index=-1;++Index!=NewTable.rows[0].cells.length;)
			{
			var NewCol=NewRow.insertCell();
			if(TagName=='td'&&Index==ColIndex)	NewCol.id=this.TempId;
			}
		Table.outerHTML=NewTable.outerHTML;
		if(TagName=='td')	this.SelectTemp();
}#else{
		NewRow=Table.insertRow(RowIndex+1);
		NewRow.innerHTML=Row.innerHTML;
}#endif
		}
	},
DeleteRow:function()
	{
	var Element=this.GetElementByTags({tr:1,td:1});
	if(Element)
		{
		var Row=(Element.tagName.toLowerCase()=='td'?this.$Parent(Element):Element),RowIndex=Row.rowIndex,Table=this.$Parent(this.$Parent(Row)),ColIndex=Element.cellIndex;
		Table.deleteRow(RowIndex);
#if(IE){
		if(Element.tagName.toLowerCase()=='td')
			{
			if(RowIndex>=Table.rows.length)	RowIndex=Table.rows.length-1;
			if(RowIndex>=0)
				{
				var Range=frames[this.EditorFrame].document.body.createTextRange();
				Range.moveToElementText(Table.rows[RowIndex].cells[ColIndex]);
				Range.moveStart('character',Range.text.length);
				Range.select();
				}
			else	Table.removeNode(true);
			}
}#else{
}#endif
		}
	},
InsertCol:function()
	{
	var Element=this.GetElementByTags({table:1,tbody:1,tr:1,td:1});
	if(Element)
		{
		var TagName=Element.tagName.toLowerCase(),Row=null,RowIndex=0,ColIndex=0,Table=Element;
		if(TagName=='td')
			{
			Row=this.$Parent(Element);
			ColIndex=Element.cellIndex;
			}
		else if(TagName=='tr')	Row=Element;
		else if(TagName=='tbody')	Table=this.$Parent(Table);
		if(Row)	{
			RowIndex=Row.rowIndex;
			Table=this.$Parent(this.$Parent(Row));
			}
#if(IE){
		var NewTable=Table.cloneNode(true);
		for(var Index=-1;++Index!=NewTable.rows.length;)
			{
			var NewCol=NewTable.rows[Index].insertCell(ColIndex+1);
			if(TagName=='td'&&Index==RowIndex)	NewCol.id=this.TempId;
			}
		Table.outerHTML=NewTable.outerHTML;
		if(TagName=='td')	this.SelectTemp();
}#else{
		for(var NewCol,Index=-1;++Index!=Table.rows.length;)
			{
			NewCol=Table.rows[Index].insertCell(ColIndex+1);
			NewCol.innerHTML=Table.rows[Index].cells[ColIndex].innerHTML;
			}
}#endif
		}
	},
DeleteCol:function()
	{
	var Element=this.GetElementByTags({td:1});
	if(Element)
		{
		var Row=this.$Parent(Element),RowIndex=Row.rowIndex,ColIndex=Element.cellIndex,Table=this.$Parent(this.$Parent(Row));
#if(IE){
		var NewTable=Table.cloneNode(true);
		if(NewTable.rows[0].cells.length==1)	Table.removeNode(true);
		else	{
			for(var Index=-1;++Index!=NewTable.rows.length;)
				{
				if(NewTable.rows[Index].cells[ColIndex]=='[object]')	NewTable.rows[Index].deleteCell(ColIndex);
				}
			if(ColIndex>=NewTable.rows[0].cells.length)	ColIndex=NewTable.rows[0].cells.length-1;
			if(ColIndex>=0)	NewTable.rows[RowIndex].cells[ColIndex].id=this.TempId;
			Table.outerHTML=NewTable.outerHTML;
			if(ColIndex>=0)	this.SelectTemp();
			}
}#else{
		for(var Index=-1;++Index!=Table.rows.length;Table.rows[Index].deleteCell(ColIndex));
}#endif
		}
	},
Save:function()
	{
	this.SaveText=(this.IsDesign?frames[this.EditorFrame].document.body.innerHTML:this.$Value(this.EditorHtml));
	},
LoadSave:function()
	{
	if(this.IsDesign)	frames[this.EditorFrame].document.body.innerHTML=this.SaveText;
	else	this.$Id(this.EditorHtml).Value(this.SaveText);
	},
ClearAll:function()
	{
	if(this.IsDesign)	frames[this.EditorFrame].document.body.innerHTML='';
	else	this.$Id(this.EditorHtml).Value('');
	},
FormatCode:function()
	{
	var OldMode=this.IsDesign;
	this.SetMode(1);
	this.SetHtml(frames[this.EditorFrame].document.body.innerHTML);
	this.SetMode(OldMode);
	},
ChangeParagraph:function()
	{
	var Paragraph=this.$Id(this.GetId('paragraph')).Element(),Value=Paragraph.options[Paragraph.selectedIndex].value;
	this.AddCode(0,'<'+Value+'>','</'+Value+'>');
	Paragraph.selectedIndex=0;
	},
ChangeFontName:function()
	{
	var FontName=this.$Id(this.GetId('fontName')).Element();
	this.ExecCommand(0,'fontname',this.FontNameText[FontName.selectedIndex-1]);
	FontName.selectedIndex=0;
	},
ChangeFontSize:function()
	{
	var FontSize=this.$Id(this.GetId('fontSize')).Element();
	frames[this.EditorFrame].document.execCommand('fontsize',false,FontSize.selectedIndex);
	FontSize.selectedIndex=0;
	},
//执行单参数execCommand命令
//Command:	执行IE的execCommand
ExecCommand:function(Event,Command,Value)
	{
	try	{
		(this.IsDesign?frames[this.EditorFrame]:this.$Id(this.EditorHtml).Element()).focus();
		(this.IsDesign?frames[this.EditorFrame]:window).document.execCommand(Command,false,Value);
		this.CheckHtml();
		}
	catch(e){
		return 0;
		}
	return 1;
	},
//获取当前代码
GetHtml:function()
	{
	this.FormatCode();
	return this.IsDesign?frames[this.EditorFrame].document.body.innerHTML:this.$Value(this.EditorHtml);
	},
//获取当前代码
GetText:function()
	{
	this.FormatCode();
	return fastCSharp.$(frames[this.EditorFrame].document.body).Text();
	},
//聚焦编辑器
Focus:function()
	{
	(this.IsDesign?frames[this.EditorFrame]:this.$Id(this.EditorHtml).Element()).focus();
	},
//设置编辑器模式
//IsDesign:	为tue表示设计视图模式,否则表示代码模式
SetMode:function(IsDesign,IsFocus)
	{
	if(IsDesign?!this.IsDesign:(this.IsDesign&&this.EditorHtml))
		{
		for(var Name in this.Buttons)
			{
			if(this.Buttons[Name].OnlyDesign)	this.GetElement(this.Buttons[Name]).Disabled(!IsDesign).Cursor(IsDesign?'pointer':'auto');
			}
		this.GetElement({Name:'DesignButton'}).Class(IsDesign?this.OnStyle:this.OffStyle).Disabled(IsDesign);
		this.GetElement({Name:'HtmlButton'}).Class(IsDesign?this.OffStyle:this.OnStyle).Disabled(!IsDesign);
		this.$Id(this.GetId('fontName')).Disabled(!IsDesign);
		this.$Id(this.GetId('fontSize')).Disabled(!IsDesign);
		this.$Id(this.GetId('paragraph')).Disabled(!IsDesign);
		if(this.Color)	this.Color.Show(IsDesign?1:0);
		this.$Id(this.EditorFrame).Display(IsDesign);
		this.$Id(this.EditorHtml).Display(!IsDesign);
		if(IsDesign)
			{
			this.SetHtml(this.EditorHtml?this.$Value(this.EditorHtml):'');
			if(IsFocus==null||IsFocus)	frames[this.EditorFrame].focus();
			}
		else	{
			this.$Id(this.EditorHtml).Value(frames[this.EditorFrame].document.body.innerHTML);
			if(IsFocus==null||IsFocus)	this.$Id(this.EditorHtml).Focus();
			}
		this.IsDesign=IsDesign?1:0;
		}
	},
//重设编辑器的HTML代码
//Html:	HTML代码
SetHtml:function(Html)
	{
	frames[this.EditorFrame].document.body.innerHTML=Html||'<br />';
	this.CheckHtml();
	return this;
	},
CheckHtml:function()
	{
	if(this.IsAutoHeight)
		{
		var Document=frames[this.SaveFrame].document;
		Document.body.innerHTML=frames[this.EditorFrame].document.body.innerHTML;
		var Height=(Document.body.scrollHeight||Document.documentElement.scrollHeight)+20;
		if(this.MaxHeight&&Height>this.MaxHeight)	Height=this.MaxHeight;
		if(this.GetMaxHeight)	Height=Math.min(Height,this.GetMaxHeight());
		Height=Math.max(Height,this.MinHeight)+'px';
		this.Element.Css('height',Height);
		this.$Id(this.EditorFrame).Css('height',Height);
		}
	},
KeyPress:function(Event)
	{
	this.OnKeyPress(Event);
	this.CheckHtml();
	},
KeyDown:function(Event)
	{
#if(IE){
	if(Event.ctrlKey)
		{
		if(Event.keyCode==90)	this.ExecCommand(null,'undo');
		else if(Event.keyCode==89)	this.ExecCommand(null,'redo');
		}
}#else{
}#endif
	this.OnKeyDown(Event);
	},
KeyUp:function(Event)
	{
	this.OnKeyUp(Event);
	this.CheckHtml();
	},
MouseMove:function(Event)
	{
	this.OnMouseMove(Event);
	},
DoubleClick:function(Event)
	{
	this.OnDoubleClick(Event);
	},
PasteFilter:function(Event)
	{
	if(this.PasteLinkAjaxCallName||this.PasteImageAjaxCallName||this.OnPasteFilter.Get().length)
		{
		this.SaveRange();
		var Document=frames[this.EditorFrame].document,Div=Document.createElement('div');
		Div.id=Div.name=this.TempId;
#if(IE){
		var SaveFrame=frames[this.SaveFrame];
		SaveFrame.focus();
		SaveFrame.document.execCommand('selectAll',false,0);
		SaveFrame.document.execCommand('paste',false,0);
		Div.style.display='none';
		Div.innerHTML=SaveFrame.document.body.innerHTML;
		this.OnPasteFilter([Div]);
		if(this.PasteLinkAjaxCallName)	this.PasteLink([Div]);
		this.CheckHtml();
		Event.CancelBubble();
}#else{
		this.ClipboardImageCount=0;
		if(this.PasteImageAjaxCallName)
			{
			var Items=this.ClipboardItems=Event.clipboardData&&Event.clipboardData.items;
			if(Items)
				{
				var IsImage=0,Form=new FormData();
				for(var Index=0;Index-Items.length;++Index)
					{
					var Item=Items[Index];
					if(Item.kind=='file'&&Item.type.indexOf('image')==0)
						{
						var Blob=Item.getAsFile();
						Form.append('i'+(++this.PasteImageIdentity),Blob);
						if(Blob.size)	IsImage=Blob.size;
						++this.ClipboardImageCount;
						}
					}
				if(IsImage)	this.PasteImageFormData=Form;
				}
			else if(Items=Event.clipboardData&&Event.clipboardData.files)
				{
				var IsImage=0,Form=new FormData();
				for(var Index=0;Index-Items.length;++Index)
					{
					var Item=Items[Index];
					if(Item.type.indexOf('image')==0)
						{
						Form.append('i'+(++this.PasteImageIdentity),Item);
						if(Item.size)	IsImage=Item.size;
						++this.ClipboardImageCount;
						}
					}
				if(IsImage)	this.PasteImageFormData=Form;
				}
			}
		Div.innerHTML="&nbsp;"
		Div.style.left="-99999px";
		Div.style.height=Div.style.width="1px";
		Div.style.position="absolute";
		Div.style.overflow="hidden";
		Document.body.appendChild(Div);
		var Range=Document.createRange();
		Range.setStart(Div.firstChild,0);
		Range.setEnd(Div.firstChild,1);
		var Selection=this.$Id(this.EditorFrame).Element().contentWindow.getSelection();
		Selection.removeAllRanges();
		Selection.addRange(Range);
		setTimeout(fastCSharp.ThisFunction(this,this.PasteFilterEnd),0);
}#endif
		}
	},
#if(IE){
}#else{
PasteFilterEnd:function()
	{
	var Document=frames[this.EditorFrame].document,Identity=this.PasteImageIdentity,Divs=[];
	if(this.OnPasteFilter.Get().length)
		{
		for(var Nodes=Document.body.childNodes,Index=0;Index-Nodes.length;++Index)
			{
			var Node=Nodes[Index];
			if(Node.id==this.TempId||fastCSharp.$Name(Node)==this.TempId)	Divs.push(Node);
			}
		this.OnPasteFilter(Divs);
		}
	this.CurrentPasteImageIdentity=this.PasteImageIdentity-this.ClipboardImageCount;
	if(this.PasteImageAjaxCallName)
		{
		for(var Nodes=Document.body.childNodes,Index=0;Index-Nodes.length;++Index)
			{
			var Node=Nodes[Index];
			if(Node.id==this.TempId||fastCSharp.$Name(Node)==this.TempId)	this.PasteFilterCheckImage(Node);
			}
		if(Identity==this.PasteImageIdentity&&this.ClipboardImageCount==0)	this.PasteImageFormData=null;
		}
	for(var Divs=[],Nodes=Document.body.childNodes,Index=Nodes.length;Index;)
		{
		var Node=Nodes[--Index];
		if(Node.id==this.TempId||fastCSharp.$Name(Node)==this.TempId)
			{
			this.PasteFilterCheck(Node,Divs);
			Document.body.removeChild(Node);
			}
		}
	while(this.CurrentPasteImageIdentity<Identity)
		{
		if(Divs.length==1&&Divs[0].innerHTML=='&nbsp')	Divs=[];
		var Div=Document.createElement('div');
		Div.innerHTML='<img name="fastCSharpEditorImage'+(++this.CurrentPasteImageIdentity)+'" style="display:none" />';
		Divs.push(Div);
		}
	if(Divs.length)
		{
		this.PasteLink(Divs);
		if(this.PasteImageFormData)
			{
			this.PasteImageFormData.append('identity',this.PasteImageIdentity);
			fastCSharp.Ajax(fastCSharp.ThisFunction(this,this.OnUploadImage),this.PasteImageAjaxCallName,this.PasteImageFormData);
			}
		}
	this.PasteImageFormData=null;
	this.CheckHtml();
	},
PasteFilterCheckImage:function(Parent)
	{
	for(var CheckNode,Nodes=Parent.childNodes,Index=0;Index-Nodes.length;++Index)
		{
		var Node=Nodes[Index];
		if(Node.id==this.TempId||fastCSharp.$Name(Node)==this.TempId)	this.CheckPasteImage(CheckNode=Node);
		}
	if(!CheckNode)	this.CheckPasteImage(Parent);
	},
CheckPasteImage:function(Parent)
	{
	for(var Nodes=Parent.childNodes,Index=0;Index-Nodes.length;++Index)
		{
		var Node=Nodes[Index];
		if(Node.tagName&&Node.tagName.toLowerCase()=='img')
			{
			if(Node.src.indexOf('file://')==0)
				{
				if(this.ClipboardItems)	Node.name='fastCSharpEditorImage'+(++this.CurrentPasteImageIdentity);
				Node.style.display='none';
				}
			else if(Node.src.indexOf('data:')==0)
				{
				var Match=Node.src.match(/^data\:([^\;]+)\;base64\,(.+)$/);
				if(Match)
					{
					var Bytes=atob(Match[2]),Codes=[];
					for(var ByteIndex=0;ByteIndex-Bytes.length;++ByteIndex)	Codes.push(Bytes.charCodeAt(ByteIndex));
					if(!this.PasteImageFormData)	this.PasteImageFormData=new FormData();
					this.PasteImageFormData.append('i'+(++this.PasteImageIdentity),new Blob([new Uint8Array(Codes)],{type:Match[1]}));
					Node.name='fastCSharpEditorImage'+this.PasteImageIdentity;
					Node.style.display='none';
					}
				}
			}
		else if(Node.nodeType==1)	this.CheckPasteImage(Node);
		}
	},
PasteFilterCheck:function(Parent,Divs)
	{
	for(var PushNode,Nodes=Parent.childNodes,Index=Nodes.length;Index;)
		{
		var Node=Nodes[--Index];
		if(Node.id==this.TempId||fastCSharp.$Name(Node)==this.TempId)	Divs.push(PushNode=Node);
		}
	if(!PushNode)	Divs.push(Parent);
	},
OnUploadImage:function(Value)
	{
	var Images=Value.__AJAXRETURN__;
	if(Images)
		{
		for(var Identitys={},Index=Images.length;Index;Identitys[(Value.identity--).toString()]=this.FormatPasteImage?this.FormatPasteImage(Images[--Index]):Images[--Index]);
		for(var Images=frames[this.EditorFrame].document.getElementsByTagName('img'),Index=Images.length;Index;)
			{
			var Image=Images[--Index],Name=fastCSharp.$Attribute(Image,'name');
			if(Name.substring(0,21)=='fastCSharpEditorImage')
				{
				if(!Image.src||Image.src.indexOf('file://')==0||Image.src.indexOf('data:')==0)
					{
					var Src=Identitys[Name.substring(21)];
					if(Src)	{
						Image.src=Src;
						Image.identity=Image.name=Image.style.display='';
						}
					}
				}
			}
		}
	},
}#endif
PasteLink:function(Divs)
	{
	for(var TempDiv=fastCSharp.$(frames[this.EditorFrame].document.createElement('div')),Html=[],Index=Divs.length;Index;)
		{
		var Div=Divs[--Index];
		if(this.PasteLinkAjaxCallName)	this.PasteCheckLink(Div,TempDiv);
		Html.push(Div.innerHTML);
		}
	this.PasteRange(Html.join('<br />'));
	if(this.PasteLinkAjaxCallName)	fastCSharp.Functions.CrawlTitle.TryRequest(this.PasteLinkAjaxCallName);
	},
PasteCheckLink:function(Div,TempDiv)
	{
	for(var Nodes=Div.childNodes,NodeIndex=Nodes.length;NodeIndex;)
		{
		var Node=Nodes[--NodeIndex];
		if(Node.nodeType==3)
			{
			var Text=fastCSharp.$Text(Node),NewText=Text.ToHTML().replace(HtmlEditor.PasteLinkRegex,this.ReplaceLinkFunction);
			if(NewText.indexOf('<')+1)
				{
				var Texts=NewText.split('<'),Html=[Texts[0]];
				for(var Index=1;Index<Texts.length;)
					{
					Html.push('<');
					Html.push(Texts[Index++]);
					Html.push('</a>&nbsp;');
					Html.push(Texts[Index++].substring(3));
					}
				TempDiv.Html(Html.join('')).Child().InsertBefore(Node,Div);
				Div.removeChild(Node);
				}
			}
		else if(Node.nodeType==1)
			{
			if(Node.tagName=='A')
				{
				if(Node.childNodes.length==1&&Node.childNodes[0].nodeType==3&&Node.href==fastCSharp.$Text(Node.childNodes[0])&&fastCSharp.$Name(Node)!='fastCSharpEditorLink')
					{
					TempDiv.Html(this.ReplaceLink(Node.href)).Child().InsertBefore(Node,Div);
					Div.removeChild(Node);
					}
				}
			else if(Node.tagName!='a') this.PasteCheckLink(Node,TempDiv);
			}
		}
	},
ReplaceLink:function(Link)
	{
	var Value=fastCSharp.Functions.CrawlTitle.Get(Link,this.OnCrawlTitleFunction);
	return '<a name="fastCSharpEditorLink" href="'+Value.Link.ToHTML()+'">'+Value.Title.ToHTML()+'</a>';
	},
OnCrawlTitle:function(Value)
	{
	for(var Links=frames[this.EditorFrame].document.getElementsByTagName('a'),Index=Links.length;Index;)
		{
		var Link=Links[--Index];
		if(Link.href==Value.Link&&fastCSharp.$Attribute(Link,'name')=='fastCSharpEditorLink')	Link.innerHTML=Value.Title.ToHTML();
		}
	},
//选定区域添加代码
//Start:	HTML代码前缀
//End:		HTML代码后缀
AddCode:function(Event,Start,End)
	{
	var Html=Start+this.GetSelectionHtml(0)+End;
	this.PasteRange(Html,1);
//	this.PasteHtml(Start+this.GetSelectionHtml(0)+End,0);
	},
//获取选定区域Html代码
//IsAll:	为true时表示当没有选定区域时则选则全部
GetSelectionHtml:function(IsAll)
	{
#if(IE){
	frames[this.EditorFrame].focus();
	var Document=frames[this.EditorFrame].document,Selection=Document.selection.createRange(),Html='';
	if(Selection.htmlText)	Html=(IsAll&&Selection.htmlText.length==0?Document.body.innerHTML:Selection.htmlText);
	else if(IsAll)	Html=Document.body.innerHTML;
	return Html;
}#else{
	return this.CutSave(IsAll);
}#endif
	},
SaveRange:function()
	{
	frames[this.EditorFrame].focus();
#if(IE){
	this.SelectRange=frames[this.EditorFrame].document.selection.createRange();
	if(this.SelectRange.type=='Control')	this.SelectRange=this.SelectRange.length?Range.item(0):null;
}#else{
	this.SelectRange=this.$Id(this.EditorFrame).Element().contentWindow.getSelection().getRangeAt(0);
}#endif
	},
PasteRange:function(Html,IsSelect)
	{
	if(!this.SelectRange&&IsSelect)	this.SaveRange();
	if(this.SelectRange)
		{
		frames[this.EditorFrame].focus();
#if(IE){
		this.SelectRange.pasteHTML(Html);
		this.SelectRange.select();
		this.SelectRange=null;
}#else{
		var Selection=this.$Id(this.EditorFrame).Element().contentWindow.getSelection();
//		if(fastCSharp.IsIE11)
//			{
			this.SelectRange.deleteContents();
			var Div=document.createElement('div');
			Div.style.display='none';
			Div.innerHTML=Html;
			frames[this.EditorFrame].document.body.appendChild(Div);
			for(var Nodes=Div.childNodes,Index=Nodes.length;Index;this.SelectRange.insertNode(Nodes[--Index]));
			frames[this.EditorFrame].document.body.removeChild(Div);
			this.SelectRange.setStart(this.SelectRange.endContainer,this.SelectRange.endOffset);
			Selection.removeAllRanges();
			Selection.addRange(this.SelectRange);
			this.SelectRange=null;
			frames[this.EditorFrame].focus();
//			}
//		else	{
//			Selection.removeAllRanges();
//			Selection.addRange(this.SelectRange);
//			this.SelectRange=null;
//			this.PasteHtml(Html);
//			}
}#endif
		this.CheckHtml();
		}
	else	this.PasteHtml(Html);
	},
//选定区域粘贴Html代码
//Html:		HTML代码
//IsAll:	为true时表示当没有选定区域时则选则全部
PasteHtml:function(Html,IsAll)
	{
#if(IE){
	frames[this.EditorFrame].focus();
	var Document=frames[this.EditorFrame].document,Selection=Document.selection.createRange();
	if(Selection.htmlText!=null&&(!IsAll||Selection.htmlText))
		{
		Selection.pasteHTML(Html);
		IsAll=0;
		}
	if(IsAll)	SetHtml(Html);
}#else{
	this.ExecCommand(0,'insertHTML',Html);
}#endif
	},
//获取选定区域文本
//IsAll:	为true时表示当没有选定区域时则选则全部
GetSelectionText:function(IsAll)
	{
	var EditorHtml=this.$Id(this.EditorHtml).Element();
#if(IE){
	var Text=EditorHtml.document.selection.createRange().text;
	return Text==''&&IsAll?EditorHtml.value:Text;
}#else{
	var StartIndex=EditorHtml.selectionStart,EndIndex=EditorHtml.selectionEnd;
	return (StartIndex-EndIndex?EditorHtml.value.substring(StartIndex,EndIndex):EditorHtml.value);
}#endif
	},
//选定区域粘贴文本
//Text:		文本代码
//IsAll:	为true时表示当没有选定区域时则选则全部
PasteText:function(Text,IsAll)
	{
	this.$Paste(this.$Id(this.EditorHtml).Element(),Text,IsAll);
	},
#if(IE){
}#else{
//保存选定区域的代码
//IsAll:	为true时表示当没有选定区域时则选则全部
CutSave:function(IsAll)
	{
	var Body=frames[this.SaveFrame].document.body,Selection=this.$Id(this.EditorFrame).Element().contentWindow.getSelection();
	Body.innerHTML='';
	if(Selection.rangeCount)	Body.appendChild(Selection.getRangeAt(0).cloneContents());
//	this.ExecCommand(null,'delete');
	if(IsAll&&Body.innerHTML=='')
		{
		frames[this.EditorFrame].focus();
		frames[this.EditorFrame].document.execCommand('selectAll',false,null);
		Selection=this.$Id(this.EditorFrame).Element().contentWindow.getSelection();
		if(Selection.rangeCount)	Body.appendChild(Selection.getRangeAt(0).extractContents());
		}
	return Body.innerHTML;
	},
}#endif
//document.execCommand('saveas','true','a.htm')	//另存为
//E.PasteElements=function(IN)
//	{
//	for(var i=IN.length-1;i>=0;i--)	this.PasteElement(IN[i]);
//	};
//E.PasteElement=function(IN)
//	{
//	frames[this.EditorFrame].focus();
//	var GS=System.GE(this.EditorFrame).contentWindow.getSelection(),R=GS.getRangeAt(0);
//	GS.removeAllRanges();
//	R.deleteContents();
//	var SN=R.startContainer,SO=R.startOffset,NR=document.createRange(),AN;
//	if(SN&&SN.nodeType==3&&IN.nodeType==3)
//		{
//		SN.insertData(SO,IN.nodeValue);
//		NR.setEnd(SN,SO+IN.length);	NR.setStart(SN,SO+IN.length);
//		}
//	else	{
//		if(SN.nodeType==3)
//			{
//			var T=SN.nodeValue,PN=this.$Parent(SN);
//			PN.insertBefore(AN=document.createTextNode(T.substr(SO)),SN);
//			PN.insertBefore(IN,AN);
//			PN.insertBefore(document.createTextNode(T.substr(0,SO)),IN);
//			PN.removeChild(SN);
//           	 	}
//		else	{
//			if(SN.tagName.toLowerCase()!='html')	AN=SN.childNodes[SO];
//			else	{	SN=SN.childNodes[0].nextSibling;	AN=SN.childNodes[0];	}
//			SN.insertBefore(IN,AN);
//			}
//		NR.setEnd(AN,0);	NR.setStart(AN,0);
//		GS.addRange(NR);
//		}
//	NR.detach();
//	};
//获取选定控件
GetSelectElement:function()
	{
	frames[this.EditorFrame].focus();
#if(IE){
	var Selection=frames[this.EditorFrame].document.selection,Type=Selection.type,Range=Selection.createRange();
	if(Range.length>0)	return Range.item(0);
}#else{
	var Selection=this.$Id(this.EditorFrame).Element().contentWindow.getSelection();
	if(Selection.rangeCount)
		{
		var Range=Selection.getRangeAt(0);
		if(Range)	return Range.startContainer;
		}
}#endif
	return null;
	},
//获取选定标签控件
GetSelectTagElement:function()
	{
#if(IE){
	frames[this.EditorFrame].focus();
	var Selection=frames[this.EditorFrame].document.selection,Type=Selection.type,Range=Selection.createRange();
	if(Type=='Control')
		{
		if(Range.length>0)	return Range.item(0);
		}
	else if(Type=='Text')	return Range.parentElement();
	else	return Range.parentElement();
}#else{
	var Element=this.GetSelectElement();
	if(Element)	return Element.nodeType-3?Element:this.$Parent(Element);
}#endif
	return null;
	},
//获取tagName匹配T的父控件
//TagNames:	待匹配的标签名称集合
GetElementByTags:function(TagNames)
	{
	var Element=this.GetSelectTagElement();
	while(TagNames[Element.tagName.toLowerCase()]==null&&Element.tagName.toLowerCase()!='body')	Element=this.$Parent(Element);
	return TagNames[Element.tagName.toLowerCase()]?Element:null;
	},
SelectText:function(Element,StartIndex,EndIndex)
	{
	frames[this.EditorFrame].focus();
#if(IE){
	var Range=frames[this.EditorFrame].document.body.createTextRange();
	Range.moveToElementText(Element.Element());
	var TextLength=Range.text.length;
	Range.moveStart('character',StartIndex==null?TextLength:StartIndex);
	if(EndIndex)	Range.moveEnd('character',EndIndex-TextLength);
	Range.select();
}#else{
	var Selection=this.$Id(this.EditorFrame).Element().contentWindow.getSelection();
	if(Selection.rangeCount)
		{
		var Range=Selection.getRangeAt(0);
		Selection.removeAllRanges();
		var TextLength=Element.Text().length;
		if(StartIndex==null)	StartIndex=Element.Text().length;
		Element=Element.Element().childNodes[0];
		Range.setStart(Element,StartIndex==null?TextLength:StartIndex);
		if(EndIndex!=null)	Range.setEnd(Element,EndIndex>TextLength?TextLength:EndIndex);
		Selection.addRange(Range);
		}
}#endif
	},
Select:function(Element)
	{
	frames[this.EditorFrame].focus();
#if(IE){
	var Range=frames[this.EditorFrame].document.body.createTextRange();
	Range.moveToElementText(Element.Element());
	Range.select();
}#else{
	var Selection=this.$Id(this.EditorFrame).Element().contentWindow.getSelection();
	if(Selection.rangeCount)
		{
		var Range=Selection.getRangeAt(0);
		Selection.removeAllRanges();
		Element=Element.Element();
		Range.setStartBefore(Element);
		Range.setEndAfter(Element);
		Selection.addRange(Range);
		}
}#endif
	},
SelectTemp:function()
	{
	this.Select(this.$(frames[this.EditorFrame].document.getElementById(this.TempId)).Id(''));
	},
Quote:function()
	{
	this.AddCode(null,'<'+this.QuoteTagName+'>','</'+this.QuoteTagName+'>');
//	this.PasteHtml('<'+this.QuoteTagName+' id="quote">QUOTE</'+this.QuoteTagName+'>');
//	var Element=frames[this.EditorFrame].document.getElementById('quote');
//	Element.innerHTML=Html;
//	Element.id=null;
	},
GetXY:function()
	{
	return this.$Id(this.EditorFrame).XY();
	},
OverColor:function(Event,IsOver)
	{
	this.GetElement({Name:this.ColorFont?'FontColor':'BgColor'}).Class(IsOver?this.OverStyle:this.OutStyle);
	},
SetColor:function(Event,IsFore)
	{
	if(IsFore!=null)	this.ColorFont=IsFore;
	frames[this.EditorFrame].focus();
	var Color=this.$Value(this.GetId('CurrentColor'));
#if(IE){
	frames[this.EditorFrame].document.execCommand(((IsFore==null?this.ColorFont:IsFore)?'fore':'back')+'color',false,Color);
}#else{
	try	{
		if(IsFore==null?this.ColorFont:IsFore)	frames[this.EditorFrame].document.execCommand((IsFore==null?this.ColorFont:IsFore?'fore':'back')+'color',false,Color);
		else	this.AddCode(0,"<font style='background-color:#"+Color+"'>",'</font>');
		}
	catch(e){}
}#endif
	},
SetForeColor:function(Color)
	{
	frames[this.EditorFrame].focus();
	frames[this.EditorFrame].document.execCommand('forecolor',false,Color);
	},
//加载显示覆盖窗口
//Url:		覆盖窗口页面地址
//Width:	宽度
//Height:	高度
ShowOver:function(Event,Url,Width,Height)
	{
	if(arguments.length)
		{
		this.OverIframe.Width=Width;
		this.OverIframe.Height=Height;
		this.OverIframe.ShowPath(Url);
		}
	else	this.OverIframe.Hide();
	},
Start:function(IsFocus)
	{
	this.OverIframe=fastCSharp.NewObject('OverIframe');
	var Buttons=['Name,Title,OnlyDesign,OnClick,DefaultSet',
		['FontColor','字体颜色',1,fastCSharp.ThisEvent(this,this.SetColor,[true]),0],
		['BgColor','字体背景颜色',1,fastCSharp.ThisEvent(this,this.SetColor,[false]),0],
		['Char','插入特殊符号',0,fastCSharp.ThisEvent(this,this.ShowOver,[this.Htmls.Char,800,600]),1],
		['Replace','替换',0,fastCSharp.ThisEvent(this,this.ShowOver,[this.Htmls.Replace,300,220]),1],
		['ClearCode','清理代码',0,fastCSharp.ThisFunction(this,this.ClearCode),1],
		['Mood','心情图标',1,fastCSharp.ThisEvent(this,this.ShowOver,[this.Htmls.Mood,500,300]),1],
		['Help','帮助',0,fastCSharp.ThisEvent(this,this.ShowOver,[this.Htmls.Help,300,220]),1],
		['SelectAll','全选',0,fastCSharp.ThisFunction(this,this.SelectAll),1],
		['Cut','切剪',0,fastCSharp.ThisFunction(this,this.Cut),1],
		['Copy','复制',0,fastCSharp.ThisFunction(this,this.Copy),1],
		['Paste','粘贴',0,fastCSharp.ThisFunction(this,this.Paste),1],
		['Undo','撤消',0,fastCSharp.ThisEvent(this,this.ExecCommand,['undo']),1],
		['Redo','重做',0,fastCSharp.ThisEvent(this,this.ExecCommand,['redo']),1],
		['CreateLink','插入超级链接',1,fastCSharp.ThisFunction(this,this.CreateLink),1],
		['Unlink','去掉超级链接',1,fastCSharp.ThisEvent(this,this.ExecCommand,['Unlink']),1],
		['InsertImage','插入图片',1,fastCSharp.ThisFunction(this,this.InsertImage),1],
		['InsertHorizontalRule','插入水平线',1,fastCSharp.ThisEvent(this,this.ExecCommand,['InsertHorizontalRule']),1],
		['Table','插入表格',1,fastCSharp.ThisEvent(this,this.ShowOver,[this.Htmls.Table,530,400]),1],
		['InsertRow','插入行',1,fastCSharp.ThisFunction(this,this.InsertRow),1],
		['DeleteRow','删除行',1,fastCSharp.ThisFunction(this,this.DeleteRow),1],
		['InsertCol','插入列',1,fastCSharp.ThisFunction(this,this.InsertCol),1],
		['DeleteCol','删除列',1,fastCSharp.ThisFunction(this,this.DeleteCol),1],
		['Flash','插入Flash',1,fastCSharp.ThisEvent(this,this.ShowOver,[this.Htmls.Multimedia+'&type=0',500,240]),1],
		['WindowsMedia','插入Windows Media',1,fastCSharp.ThisEvent(this,this.ShowOver,[this.Htmls.Multimedia+'&type=1',500,240]),1],
		['RealMedia','插入Real Media',1,fastCSharp.ThisEvent(this,this.ShowOver,[this.Htmls.Multimedia+'&type=2',500,240]),1],
		['Bold','加粗',1,fastCSharp.ThisEvent(this,this.ExecCommand,['bold']),1],
		['Italic','斜体',1,fastCSharp.ThisEvent(this,this.ExecCommand,['italic']),1],
		['UnderLine','下划线',1,fastCSharp.ThisEvent(this,this.ExecCommand,['underline']),1],
		['Superscript','上标',1,fastCSharp.ThisEvent(this,this.ExecCommand,['superscript']),1],
		['Subscript','下标',1,fastCSharp.ThisEvent(this,this.ExecCommand,['subscript']),1],
		['StrikeThrough','删除线',1,fastCSharp.ThisEvent(this,this.ExecCommand,['strikethrough']),1],
		['RemoveFormat','取消格式',1,fastCSharp.ThisEvent(this,this.ExecCommand,['RemoveFormat']),1],
		['JustifyLeft','左对齐',1,fastCSharp.ThisEvent(this,this.ExecCommand,['justifyleft']),1],
		['JustifyCenter','居中',1,fastCSharp.ThisEvent(this,this.ExecCommand,['justifycenter']),1],
		['JustifyRight','右对齐',1,fastCSharp.ThisEvent(this,this.ExecCommand,['justifyright']),1],
		['InsertOrderedList','编号',1,fastCSharp.ThisEvent(this,this.ExecCommand,['insertorderedlist']),1],
		['InsertUnOrderedList','项目符号',1,fastCSharp.ThisEvent(this,this.ExecCommand,['insertunorderedlist']),1],
		['OutDent','减少缩进量',1,fastCSharp.ThisEvent(this,this.ExecCommand,['outdent']),1],
		['InDent','增加缩进量',1,fastCSharp.ThisEvent(this,this.ExecCommand,['indent']),1],
		['Quote','插入引用',1,fastCSharp.ThisFunction(this,this.Quote),1],
		['Save','保存',0,fastCSharp.ThisFunction(this,this.Save),1],
		['LoadSave','加载',0,fastCSharp.ThisFunction(this,this.LoadSave),1],
		['ClearAll','清除',0,fastCSharp.ThisFunction(this,this.ClearAll),1],
		['Print','打印',0,fastCSharp.ThisEvent(this,this.ExecCommand,['print']),1],
		['FormatCode','整理代码',0,fastCSharp.ThisFunction(this,this.FormatCode),1]
			].FormatAjax();
	this.Buttons={};
	for(var Index=Buttons.length;--Index>=0;this.Buttons[Buttons[Index].Name]=Buttons[Index])
		{
		if(Buttons[Index].DefaultSet)	this.SetButton(Buttons[Index]);
		}
	var Paragraph=this.GetElement({Name:'Paragraph'});
	if(Paragraph.Element())
		{
		var Value=('p	h1	h2	h3	h4	h5	h6	h7	pre	address').split('	');
		var Text=('普通格式	标题 1	标题 2	标题 3	标题 4	标题 5	标题 6	标题 7	已编排格式	地址').split('	');
		var Html=[];
		for(var Index=-1;++Index!=Value.length;)	Html.push("<option value='"+Value[Index].ToHTML()+"'>"+Text[Index].ToHTML()+'</option>');
		Paragraph.Html("<select id='"+this.GetId('paragraph')+"'><option style='color:green;'>--段落格式--</option>"+Html.join('')+'</select>');
		this.$Id(this.GetId('paragraph')).AddEvent('change',fastCSharp.ThisFunction(this,this.ChangeParagraph));
		}
	var FontName=this.GetElement({Name:'FontName'});
	if(FontName.Element())
		{
		this.FontNameText=('宋体	黑体	楷体	仿宋	隶书	幼圆	新宋体	细明体	Arial	Arial Black	Arial Narrow	Bradley Hand ITC	Brush Script MT	Century Gothic	Comic Sans MS	Courier	Courier New	MS Sans Serif	Script	Sys	Times New Roman	Viner Hand ITC	Verdana	Wide Latin	Wingdings').split('	');
		var Html=[];
#if(IE){
		for(var Index=-1;++Index!=this.FontNameText.length;)	Html.push("<option value='"+this.FontNameText[Index].ToHTML()+"' style='font-family:"+this.FontNameText[Index].ToHTML()+";'>"+this.FontNameText[Index].ToHTML()+'</option>');
}#else{
		for(var Index=-1;++Index!=this.FontNameText.length;)	Html.push("<option value='"+this.FontNameText[Index].ToHTML()+"' style='font-family:"+this.FontNameText[Index]+";'>"+this.FontNameText[Index].ToHTML()+'</option>');
}#endif
		FontName.Html("<select id='"+this.GetId('fontName')+"'><option style='color:green;'>--字体--</option>"+Html.join('')+'</select>');
		this.$Id(this.GetId('fontName')).AddEvent('change',fastCSharp.ThisFunction(this,this.ChangeFontName));
		}
	var FontSize=this.GetElement({Name:'FontSize'});
	if(FontSize.Element())
		{
		var Html='';
		for(var Index=1;Index<=7;Index++)	Html+="<option style='font-size:"+(Index*7)+"px'>"+Index+'</option>';
		FontSize.Html("<select id='"+this.GetId('fontSize')+"' style='height:20px'><option style='color:green'>--字号--</option>"+Html+'</select>');
		this.$Id(this.GetId('fontSize')).AddEvent('change',fastCSharp.ThisFunction(this,this.ChangeFontSize));
		}
	var Color=this.GetElement({Name:'Color'});
	if(Color.Element())
		{
		var CurrentColor=this.GetElement({Name:'CurrentColor'});
		if(CurrentColor.Element())	CurrentColor=this.$Create('input').Set('size',6).Id(this.GetId('CurrentColor')).Display(0).To(CurrentColor);
		var CurrentColorSpan=this.GetElement({Name:'CurrentColorSpan'});
		if(CurrentColorSpan.Element())	CurrentColorSpan=this.$Create('span').Id(this.GetId('CurrentColorSpan')).Display(0).To(CurrentColorSpan);
		CurrentColorSpan.Html('&nbsp;&nbsp;');
		this.Color=fastCSharp.NewObject('Color512_64',{Id:Color.Id(),CurrentColor:CurrentColor.Id(),CurrentColorSpan:CurrentColorSpan.Id(),OnClick:fastCSharp.ThisEvent(this,this.SetColor),OnOver:fastCSharp.ThisEvent(this,this.OverColor)});
		this.Color.Start();
		this.SetButton(this.Buttons['FontColor']);
		this.SetButton(this.Buttons['BgColor']);
		this.ColorFont=1;
		}
	this.GetElement({Name:'DesignButton'}).Set('title','设计模式').AddEvent('click',fastCSharp.ThisFunction(this,this.SetMode,[true]));
	this.GetElement({Name:'HtmlButton'}).Set('title','HTML代码').AddEvent('click',fastCSharp.ThisFunction(this,this.SetMode,[false]));
	var Frame=frames[this.EditorFrame],Document=Frame.document;
#if(IE){
	frames[this.SaveFrame].document.body.contentEditable=Document.body.contentEditable=true;
}#else{
	frames[this.SaveFrame].document.designMode=Document.designMode='on';
}#endif
	this.$AddEvent(Document,['keypress'],fastCSharp.ThisEvent(this,this.KeyPress,null,Frame));
	this.$AddEvent(Document,['keydown'],fastCSharp.ThisEvent(this,this.KeyDown,null,Frame));
	this.$AddEvent(Document,['keyup'],fastCSharp.ThisEvent(this,this.KeyUp,null,Frame));
	this.$AddEvent(Document.body,['mousemove'],fastCSharp.ThisEvent(this,this.MouseMove,null,Frame));
	this.$AddEvent(Document.body,['dblclick'],fastCSharp.ThisEvent(this,this.DoubleClick,null,Frame));
	this.$AddEvent(Document.body,['paste'],fastCSharp.ThisEvent(this,this.PasteFilter,null,Frame));
	this.SetMode(1,IsFocus?1:0);
	this.SetHtml(this.DefaultHtml||'');
	}
		});
fastCSharp.LoadModule('htmlEditor');
})();