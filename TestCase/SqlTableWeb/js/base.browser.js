fastCSharp.Copy=function(Left,Right)
	{
	for(var Name in Right)	Left[Name]=Right[Name];
	return Left;
	};
fastCSharp.CopyView=function(Left,Right,Values)
	{
	if(Left===Right)	return Left;
	if(Values.IndexOf(Right)==-1)
		{
		Values.push(Right);
		for(var Name in Right)
			{
			if(Name!='__VIEWONLY__')
				{
				if(Left[Name]==null||Right[Name]==null||typeof(Left[Name])!='object')	Left[Name]=Right[Name];
				else	fastCSharp.CopyView(Left[Name],Right[Name],Values);
				}
			}
		Values.pop();
		}
	return Left;
	};
fastCSharp.LoadViewType=function(Type,Name)
	{
	if(Name==null)	Name='Id';
	Type.Views={};
	Type.Get=function(Value,IsGetOnly)
		{
		if(IsGetOnly)	return Type.Views[Value[Name]||Value];
		var Id=Value[Name];
		if(Id)
			{
			var ViewValue=Type.Views[Id];
			if(ViewValue)
				{
				var Values=[];
				fastCSharp.CopyView(ViewValue,Value,Values);
				if(ViewValue.OnCopyView)	ViewValue.OnCopyView();
				return ViewValue;
				}
			return Type.Views[Id]=new Type(Value);
			}
		return new Type(Value);
		};
	};
fastCSharp.ViewFlagEnum=function(EnumString,Enum)
	{
	var Value={},IntValue=parseInt(EnumString);
	if(isNaN(IntValue))
		{
		IntValue=0;
		for(var Values=EnumString.split(','),Index=Values.length;Index;IntValue|=Enum[Values[--Index].Trim()]||0);
		}
	for(var Name in Enum)
		{
		var EnumValue=Enum[Name];
		if(typeof(EnumValue)=='number')	Value[Name]=(IntValue&EnumValue)==EnumValue?EnumValue:0;
		}
	return Value;
	};
fastCSharp.ServerTime=function(Value)
	{
	this.Time=-(new Date().getTime()-Value.Now.getTime());
	};
fastCSharp.ServerTime.prototype={
Now:function()
	{
	return new Date().AddMilliseconds(this.Time);
	}
		};
fastCSharp.ClientView={};
fastCSharp.ViewObjects=[];
//自动加载的底层基础类库
(function(){
//数组类型转换
Array.prototype.ToArray=function(GetValue,StartIndex)
	{
	if(StartIndex==null)	StartIndex=0;
	if(!GetValue&&StartIndex==0)	return this;
	for(var Values=[],Index=(StartIndex||0)-1;++Index<this.length;Values.push(GetValue?GetValue(this[Index]):this[Index]));
	return Values;
	};
//数组复制
Array.prototype.Copy=function()
	{
	for(var Value=[],Index=0;Index-this.length;Value.push(this[Index++]));
	return Value;
	};
//数组排序
Array.prototype.Sort=function(GetKey)
	{
	return this.sort(function(Left,Right){return GetKey(Left)-GetKey(Right);});
	};
//foreach模拟
Array.prototype.For=function(Action)
	{
	for(var Index=0;Index-this.length;Action(this[Index++]));
	return this;
	};
Array.prototype.Add=function(Value)
	{
	this.push(Value);
	return this;
	};
//数组分割
Array.prototype.Split=function(Count)
	{
	if(Count>0)
		{
		for(var Value=[],Index=0;Index<this.length;Value.push(this.slice(Index,Index+=Count)));
		return Value;
		}
	return this.length?[this]:[];
	};
//数组转HASH表
Array.prototype.ToHash=function(GetValue)
	{
	for(var Value={},Index=0;Index-this.length;++Index)
		{
		if(GetValue)	Value[GetValue(this[Index])]=this[Index];
		else	Value[this[Index]]=1;
		}
	return Value;
	};
//查找第一个匹配值的位置，函数判断必须指定IsFunction
Array.prototype.IndexOf=function(Value,IsFunction)
	{
	if(typeof(Value)!='function')	IsFunction=0;
	for(var Index=-1;++Index!=this.length&&(IsFunction?!Value(this[Index]):(Value!=this[Index])););
	return Index-this.length?Index:-1;
	};
//查找符合条件的值集合
Array.prototype.Find=function(IsValue)
	{
	for(var Value=[],Index=-1;++Index-this.length;)	if(IsValue(this[Index]))	Value.push(this[Index]);
	return Value;
	};
//数组二维转换成一维集合
Array.prototype.Many=function(GetValue)
	{
	for(var Value=[],Index=-1;++Index-this.length;)
		{
		var NewValue=GetValue(this[Index]);
		if(NewValue)	Value.push(NewValue);
		}
	return this.concat.apply([],Value);
	};
//数组去重
Array.prototype.Distinct=function(GetValue)
	{
	for(var Value=[],Hash={},Index=-1;++Index-this.length;)
		{
		var HashValue=GetValue?GetValue(this[Index]):this[Index];
		if(!Hash[HashValue])
			{
			Hash[HashValue]=1;
			Value.push(HashValue);
			}
		}
	return Value;
	};
//查找第一个匹配值
Array.prototype.First=function(IsValue)
	{
	for(var Index=-1;++Index-this.length;)	if(IsValue(this[Index]))	return this[Index];
	};
//匹配值替换，函数判断必须指定IsFunction
Array.prototype.Replace=function(NewValue,Value,IsFunction)
	{
	if(typeof(Value)!='function')	IsFunction=0;
	for(var Index=this.length;--Index>=0;)	if(IsFunction?Value(this[Index]):(this[Index]==Value))	this[Index]=NewValue;
	return this;
	};
//删除匹配值，函数判断必须指定IsFunction
Array.prototype.Remove=function(Value,IsFunction)
	{
	if(typeof(Value)!='function')	IsFunction=0;
	for(var Index=this.length;--Index>=0;)	if(IsFunction?Value(this[Index]):(this[Index]==Value))	this.splice(Index,1);
	return this;
	};
//在指定位置插入数据
Array.prototype.Insert=function(Index,Value)
	{
	this.splice(Index,0,Value);
	return this;
	};
//求和
Array.prototype.Sum=function(GetValue)
	{
	for(var Value=0,Index=-1;++Index-this.length;Value+=GetValue?GetValue(this[Index]):this[Index]);
	return Value;
	};
//分组
Array.prototype.Group=function(GetValue)
	{
	for(var Value=[],Group={},Index=-1;++Index-this.length;Array.push(this[Index]))
		{
		var Key=GetValue(this[Index]),Array=Group[Key];
		if(Array==null)	Value.push(Group[Key]=Array=[])
		}
	return Value;
	};
//查找最大值数据
Array.prototype.Max=function(GetValue)
	{
	if(this.length)
		{
		var Value=this[0];
		if(GetValue)
			{
			for(var MaxValue=GetValue(Value),Index=0;++Index-this.length;)
				{
				var NextValue=GetValue(this[Index]);
				if(NextValue>MaxValue)
					{
					Value=this[Index];
					MaxValue=NextValue;
					}
				}
			}
		else	{
			for(var Index=0;++Index-this.length;)
				{
				if(this[Index]>Value)	Value=this[Index];
				}
			}
		return Value;
		}
	};
//二分查找
Array.prototype.IndexFast=function(Value,NoInsert,CmpFunction)
	{
	var Length=this.length,Index=-1,InsertIndex=0,Offset=(NoInsert==null||NoInsert?1:0);
	for(var MiddleIndex,EndIndex=Length-Offset;Offset?InsertIndex<=EndIndex:InsertIndex<EndIndex;)
		{
		MiddleIndex=(InsertIndex+EndIndex+Offset)>>1;
		if((CmpFunction?(CmpFunction(this[MiddleIndex],Value)==0):(this[MiddleIndex]==Value))&&Offset)
			{
			Index=MiddleIndex;
			break;
			}
		else if(CmpFunction?CmpFunction(this[MiddleIndex],Value)>0:(this[MiddleIndex]>Value))	EndIndex=MiddleIndex-Offset;
		else	InsertIndex=MiddleIndex+1;
		}
	if(Offset==0&&InsertIndex<Length)
		{
		if(CmpFunction?CmpFunction(this[InsertIndex],Value)<=0:(Value>this[InsertIndex]))	InsertIndex++;
		}
	return Offset?Index:InsertIndex;
	};
//Ajax数据格式化[fastCSharp专用]
Array.prototype.FormatAjax=function()
	{
	var Name=this.length?this[0].split(','):null;
	return Name?this.ToArray(function(Values)
		{
		for(var Value={},Index=-1;++Index-Name.length;Value[Name[Index]]=Values[Index]);
		return Value;
		},1):null;
	};
//视图数据格式化[fastCSharp专用]
Array.prototype.FormatView=function()
	{
	return this.length>1?fastCSharp.FormatAjaxs(Names=fastCSharp.AjaxName(this[0],0),this,1):[];
	};
//unicode数值数组转字符串
Array.prototype.MakeString=function()
	{
	return String.fromCharCode.apply(null,this);
	};
//URI的Query值编码转换
String.prototype.Escape=function()
	{
	return escape(this.replace(/\xA0/g,' ')).replace(/\+/g,'%2b');
	};
//文本值转HTML
String.prototype.ToHTML=function()
	{
	return this.ToTextArea().replace(/ /g,'&nbsp;').replace(/"/g,'&#34;').replace(/'/g,'&#39;');
	};
//文本值转TextArea值
String.prototype.ToTextArea=function()
	{
	return this.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
	};
//首字符转小写
String.prototype.ToLower=function()
	{
	return this.substring(0,1).toLowerCase()+this.substring(1);
	};
//首字符转大写
String.prototype.ToUpper=function()
	{
	return this.substring(0,1).toUpperCase()+this.substring(1);
	};
//删除首位空格
String.prototype.Trim=function()
	{
	return this.replace(/(^\s*)|(\s*$)/g,'');
	};
//左侧填充字符
String.prototype.PadLeft=function(Count,Char)
	{
	var Value='';
	if((Count-=this.length)>0)
		{
		for(Char=Char.charAt(0);Count;Char+=Char,Count>>=1)
			{
			if(Count&1)	Value+=Char;
			}
		}
	return Value+this;
	};
//简单的显示字符宽度计算
String.prototype.Length=function()
	{
	for(var Value=this.length,Index=0;Index-this.length;)	if(this.charCodeAt(Index++)>0xff)	++Value;
	return Value;
	};
//取指定显示字符宽度的左侧串
String.prototype.Left=function(Length)
	{
	for(var Value=0,Index=-1;++Index!=this.length&&Length>0;)	if((Length-=this.charCodeAt(Index)>0xff?2:1)>=0)	++Value;
	return this.substring(0,Value);
	};
String.prototype.Right=function(Length)
	{
	return this.length>Length?this.substring(this.Length-Length):this;
	};
String.prototype.SplitInt=function(Split)
	{
	var Value=this.Trim();
	return Value.length?this.split(Split).ToArray(function(Value){return parseInt(0+Value);}):[];
	};
Date.prototype.AddMilliseconds=function(AddValue)
	{
	var Value=new Date;
	Value.setTime(this.getTime()+AddValue);
	return Value;
	};
Date.prototype.AddSeconds=function(Value)
	{
	return this.AddMilliseconds(Value*1000);
	};
Date.prototype.AddMinutes=function(Value)
	{
	return this.AddMilliseconds(Value*1000*60);
	};
Date.prototype.AddHours=function(Value)
	{
	return this.AddMilliseconds(Value*1000*60*60);
	};
Date.prototype.AddDays=function(Value)
	{
	return this.AddMilliseconds(Value*1000*60*60*24);
	};
Date.prototype.Cmp=function(Value)
	{
	return this.getTime()-Value.getTime();
	};
//格式化
Date.prototype.ToString=function(Format,IsFixed)
	{
	var Value={y:this.getFullYear(),M:this.getMonth()+1,d:this.getDate(),h:this.getHours(),m:this.getMinutes(),s:this.getSeconds(),S:this.getMilliseconds()};
	Format=Format.replace(/Y/g,'y').replace(/D/g,'d').replace(/H/g,'h').replace(/W/g,'w');
	if(IsFixed==null||IsFixed)
		{
		Format+=Value[Format.charAt(Format.length-1)]?'.':'y';
		for(var Values=[],LastChar='',LastIndex=0,Index=-1;++Index-Format.length;LastChar=Char)
			{
			var Char=Format.charAt(Index);
			if(Char!=LastChar)
				{
				if(Value[LastChar]!=null)
					{
					Values.push(Value[LastChar].toString().Right(Index-LastIndex).PadLeft(Index-LastIndex,'0'));
					LastIndex=Index;
					}
				else if(Value[Char]!=null)	Values.push(Format.substring(LastIndex,LastIndex=Index));
				}
			}
		Format=Values.join('');
		}
	else	{
		for(var Name in Value)	Format=Format.replace(new RegExp(Name,'g'),Value[Name]);
		}
	return Format.replace(/w/g,['日','一','二','三','四','五','六'][this.getDay()]);
	};
Date.prototype.ToDateString=function()	{	return this.ToString('yyyy/MM/dd');	};
Date.prototype.ToTimeString=function()	{	return this.ToString('HH:mm:ss');	};
Date.prototype.ToMinuteString=function()	{	return this.ToString('yyyy/MM/dd HH:mm');	};
Date.prototype.ToSecondString=function()	{	return this.ToString('yyyy/MM/dd HH:mm:ss');	};
Date.prototype.ToMinuteOrDateString=function()	{	return this.ToInt()==new Date().ToInt()?this.ToString('HH:mm'):this.ToDateString();	};
//日期转二进制整数
Date.prototype.ToInt=function()
	{
	return (this.getFullYear()<<9)+((this.getMonth()+1)<<5)+this.getDate();
	};
//获取某年某月的天数
Date.GetDays=function(Year,Month)
	{
	return [31,(--Month)==1&&(new Date(Year,1,29)).getMonth()==1?29:28,31,30,31,30,31,31,30,31,30,31][Month];
	};
Date.Parse=function(Value)
	{
	if(Value&&(Value=Value.Trim()))
		{
		var DateValue=new Date(Value=Value.replace(/\-/g,'/'));
		if(!isNaN(DateValue.getTime()))	return DateValue;
		Value=Value.replace(/[ :\/]+/g,' ').split(' ');
		DateValue=new Date(Value[0],parseInt(Value[1])-1,Value[2],Value[3],Value[4],Value[5]);
		if(!isNaN(DateValue.getTime()))	return DateValue;
		}
	};
//二进制整数转日期
Date.FromInt=function(DateInt)
	{
	return new Date(DateInt>>9,((DateInt>>5)&15)-1,DateInt&31);
	};
//Web视图style.display转换
Number.prototype.ToDisplay=function()	{	return this.toString()=='0'?'none':'';	};
Number.prototype.ToDisplayNone=function()	{	return this.toString()=='0'?'':'none';	};
Boolean.prototype.ToDisplay=function()	{	return this.toString()=='true'?'':'none';	};
Boolean.prototype.ToDisplayNone=function()	{	return this.toString()=='true'?'none':'';	};
//Web视图checked转换
Number.prototype.ToTrue=function()	{	return this.toString()!='0';	};
Number.prototype.ToFalse=function()	{	return this.toString()=='0';	};
Boolean.prototype.ToTrue=function()	{	return this.toString()=='true';	};
Boolean.prototype.ToFalse=function()	{	return this.toString()!='true';	};
//继承处理
//Function.prototype.Inherit=function(Father)
//	{
//	var This=this,Value=function(Argument)
//		{
//		var Value=new Father(Argument);
//		fastCSharp.Copy(Value,This.prototype);
//		This.apply(Value,fastCSharp.ToArray(arguments));
//		return Value;
//		};
//#if(IE){
//	Value.Close=function()
//		{
//		This=Value=Father=null;
//		};
//}#else{
//}#endif
//	return Value;
//	};
//原型继承处理
Function.prototype.Inherit=function(Father,Prototype)
	{
	this.prototype=new Father();
	fastCSharp.Copy(this.prototype,Prototype);
	this.prototype.constructor=this;
	this.prototype.Base=function(Argument)
		{
		if(typeof(Father.Base)=='function')	Father.Base.apply(this,[Argument]);
		Father.apply(this,[Argument]);
		}
	}
//公共基本类型定义
var BaseFunction=function(){};
BaseFunction.prototype={
//对象转换成URI的Query的查询串
ToQuery:function(Value,IsIgnore)
	{
	var Values=[];
	for(var Name in Value)
		{
		if(Value[Name]!=null)
			{
			var Type=typeof(Value[Name]);
			if(Type!='function'&&(!IsIgnore||Value[Name]))	Values.push(Name.Escape()+'='+Value[Name].toString().Escape());
			}
		}
	return Values.join('&');
	},
//对象转JSON字符串
ToJson:function(Value,IsIgnore,Parents)
	{
	var Type=typeof(Value);
	if(Type!='undefined'&&Type!='function'&&(!IsIgnore||Value))
		{
		if(Type=='string')
			{
			if(Value!=null)	return '"'+Value.toString().replace(/[\\"]/g,'\\$&').replace(/\n/g,'\\n')+'"';
			}
		else if(Type=='number'||Type=='boolean')	return Value.toString();
		else if(Value!=null)
			{
			Type=Object.prototype.toString.apply(Value);
			if(Type=='[object Date]')	return 'new Date('+Value.getTime()+')';
			else	{
				if(!Parents)	Parents=[];
				for(var Index=0;Index-Parents.length;++Index)
					{
					if(Parents[Index]==Value)	return 'null';
					}
				Parents.push(Value);
				var Values=[];
				if(Type=='[object Array]')
					{
					for(var Index=0;Index-Value.length;++Index)	Values.push(this.ToJson(Value[Index],IsIgnore,Parents));
					return '['+Values.join(',')+']';
					}
				else	{
					for(var Name in Value)
						{
						Type=typeof(Value[Name]);
						if(Type!='undefined'&&Type!='function'&&(!IsIgnore||Value[Name]))
							{
							Values.push(this.ToJson(Name.toString())+':'+this.ToJson(Value[Name],IsIgnore,Parents));
							}
						}
					return '{'+Values.join(',')+'}';
					}
				Parents.pop();
				}
			}
		}
	return 'null';
	},
//构造函数参数初始化
//GetArgument:function(Argument,Value,IsSys)
//	{
//	if(IsSys)	this.PushObject();
//	for(var Name in Value) this[Name]=Argument==null||typeof(Argument[Name])=='undefined'?Value[Name]:Argument[Name];
//	},
//构造函数参数初始化
GetArgument:function(Argument,Value,IsSys)
	{
	if(IsSys)	this.PushObject();
	if(typeof(this.Base)=='function')	this.Base.apply(this,[Argument]);
	if(Value)
		{
		for(var Name in Value) this[Name]=Argument==null||typeof(Argument[Name])=='undefined'?Value[Name]:Argument[Name];
		}
	},
//对象添加到标识对象池，用于字符串形式的回调识别
PushObject:function()
	{
	if(this.SysId==null||typeof(this.GetId)!='function')
		{
		fastCSharp.Object[this.SysId=fastCSharp.Object.length]=this;
		this.SysObject='fastCSharp.Object['+this.SysId+']';
		}
	},
//生成控件标识
GetId:function()
	{
	for(var Value=['fastCSharp'+this.SysId],Chars='abcdefghijklmnopqrstuvwxyz012568',NameIndex=arguments.length;--NameIndex>=0;)
		{
		for(var Codes=[],Name=arguments[NameIndex].toString(),Index=0;Index-Name.length;)
			{
			var Code=Name.charCodeAt(Index++);
			if(Code>255)	Codes.push(Chars.charCodeAt((Code+=65536)>>12));
			Codes.push(Chars.charCodeAt(Code>>6));
			Codes.push(Chars.charCodeAt(Code&31));
			}
		Value.push(Codes.MakeString());
		}
	return Value.join('_');
	},
PopObject:function()
	{
	fastCSharp.Object[this.SysId]=null;
	},
//视图对象名称处理[fastCSharp专用]
AjaxName:function(Name,StartIndex)
	{
	for(var Values=[],Index=StartIndex;Index-Name.length;)
		{
		var Code=Name.charCodeAt(Index);
		if(Code==91)
			{
			var Value=this.AjaxName(Name,Index+1);
			Value.Name=Name.substring(StartIndex,Index);
			StartIndex=Index=Value.Index;
			Values.push(Value);
			}
		else if(Code==93)
			{
			var SubName=Name.substring(StartIndex,Index++);
			if(SubName)	Values.push({Name:SubName});
			Values.Index=Index;
			return Values;
			}
		else if(Code==44)
			{
			var SubName=Name.substring(StartIndex,Index++);
			if(SubName)
				{
				if(SubName.charAt(0)=='@')	Values.ViewType=SubName.substring(1);
				else Values.push({Name:SubName});
				}
			StartIndex=Index;
			}
		else	++Index;
		}
	if(StartIndex-Name.length)
		{
		var SubName=Name.substring(StartIndex);
		if(SubName.charAt(0)=='@')	Values.ViewType=SubName.substring(1);
		else Values.push({Name:SubName});
		}
	return Values;
	},
//视图数据处理[fastCSharp专用]
FormatAjaxs:function(Names,Values,StartIndex)
	{
	for(var Value=[],Index=StartIndex-1;++Index-Values.length;Value.push(Values[Index]?this.FormatAjax(Names,Values[Index]):null));
	return Value;
	},
//视图数据处理[fastCSharp专用]
FormatAjax:function(Names,Values)
	{
	if(Names.length)
		{
		if(Names[0].Name)
			{
			for(var Value={},Index=Names.length;--Index>=0;)
				{
				Value[Names[Index].Name]=Values[Index]!=null?(Names[Index].length?this.FormatAjax(Names[Index],Values[Index],0):Values[Index]):null;
//				if(Values[Index]!=null)	Value[Names[Index].Name]=Names[Index].length?this.FormatAjax(Names[Index],Values[Index],0):Values[Index];
				}
			if(Names.ViewType)
				{
				if(Names.ViewType.charAt(0)=='.')	Value=eval(Names.ViewType.substring(1)+'.Get(Value)');
				else	Value=eval('new '+Names.ViewType+'(Value)');
				}
			return Value;
			}
		else if(Values[0])	return this.FormatAjaxs(Names[0],Values[0],0);
		}
	else	return Values;
	},
//包装HTML控件
$:function(Value,Parent)
	{
	return new fastCSharp.Functions.Element(Value,Parent);
	},
//获取URI的HASH值
$Hash:function(Location)
	{
	return (Location||location).hash.toString().replace(/^#(\!|\%21)?/g,'');
	},
//获取URI的Query值
$Search:function(Location)
	{
	return (Location||location).search.toString().replace(/^\?/g,'');
	},
//设置或者删除HTML控件的class属性
$Class:function(Element,Names,IsRemove)
	{
	if(Element.classList)
		{
		if(IsRemove)
			{
			for(var Index=Names.length;--Index>=0;Element.classList.remove(Names[Index]));
			}
		else	{
			for(var Index=Names.length;--Index>=0;Element.classList.add(Names[Index]));
			}
		}
	else	{
		var Value=Element.className;
		if(Value)
			{
			Value=Value.split(' ');
			if(IsRemove)
				{
				for(var Index=Names.length;--Index>=0&&Value.length;Value.Remove(Names[Index]));
				}
			else	{
				for(var Index=Names.length;--Index>=0;)	if(Value.IndexOf(Names[Index])==-1)	Value.push(Names[Index]);
				}
			Element.className=Value.length?Value.join(' '):'';
			}
		else if(!IsRemove)	Element.className=Names.join(' ');
		}
	},
//获取界面内容宽度
$Width:function()
	{
	return window.innerWidth||document.documentElement.clientWidth||document.body.clientWidth;
	},
//获取界面内容高度
$Height:function()
	{
	return window.innerHeight||document.documentElement.clientHeight||document.body.clientHeight;
	},
//获取或者设置滚动条横向位置
$ScrollLeft:function(Value)
	{
	if(arguments.length)	document.body.scrollLeft=document.documentElement.scrollLeft=Value;
	else	return document.body.scrollLeft||document.documentElement.scrollLeft;
	},
//获取或者设置滚动条纵向位置
$ScrollTop:function(Value)
	{
	if(arguments.length)	document.body.scrollTop=document.documentElement.scrollTop=Value;
	else	return document.body.scrollTop||document.documentElement.scrollTop;
	},
//获取HTML控件的name值 或者 获取指定name值的HTML控件集合
$Name:function(Value,Element)
	{
	if(typeof(Value)=='string')
		{
#if(IE){
		return this.$Childs(Element||document.body,function(Element){return fastCSharp.$Name(Element)==Value;});
}#else{
		return Element?this.$Childs(Element,function(Element){return fastCSharp.$Name(Element)==Value;}):this.$(fastCSharp.ToArray(document.getElementsByName(Value)));
}#endif
		}
	else	return this.$Attribute(Value,'name');
	},
//获取HTML控件的属性值
$Attribute:function(Element,Name)
	{
	var Value=Element[Name];
	return typeof(Value)=='undefined'&&Element.attributes&&(Value=Element.attributes[Name])?Value.value:Value;
	},
//获取HTML子孙节点集合
$Childs:function(Element,IsElement)
	{
	var Value=[],Elements=[Element],ElementIndex=0;
	while(ElementIndex<Elements.length)
		{
		for(var Childs=Elements[ElementIndex++].childNodes,Index=-1;++Index-Childs.length;)
			{
			if(IsElement==null||IsElement(Childs[Index]))	Value.push(Childs[Index]);
			Elements.push(Childs[Index]);
			}
		}
	return this.$(Value);
	},
//判断HTML控件是否存在指定名称的属性
$IsAttribute:function(Element,Name)
	{
	return typeof(Element[Name])!='undefined'||(Element.attributes!=null&&typeof(Element.attributes[Name])!='undefined');
	},
//获取HTML父控件
$Parent:function(Element)
	{
#if(IE){
	return this.$(Element.parentElement||Element.parentNode);
}#else{
	return this.$(Element.parentNode);
}#endif
	},
//判断是否存在HTML父控件
$IsParent:function(Element,Parent)
	{
#if(IE){
	while(Element!=Parent&&Element)	Element=Element.parentElement||Element.parentNode;
}#else{
	while(Element!=Parent&&Element)	Element=Element.parentNode;
}#endif
	return Element;
	},
//获取下一个HTML兄弟控件
$Next:function(Element)
	{
	if(Element)	return this.$(Element.nextSibling);
	},
//获取上一个HTML兄弟控件
$Previous:function(Element)
	{
	if(Element)	return this.$(Element.previousSibling);
	},
//创建一个指定Tag名称的HTML控件
$Create:function(Name,Document)
	{
	return this.$((Document||document).createElement(Name));
	},
//删除HTML控件
$Delete:function(Element)
	{
	this.$(Element).Delete();
	},
//获取指定id的HTML控件
$Id:function(Id)
	{
	return this.$(document.getElementById(Id));
	},
//获取或者设置指定id的HTML控件值
$Value:function(Id,Value)
	{
	var Element=this.$Id(Id).Element();
	if(Element)
		{
		if(arguments.length>1)
			{
			if(Element.tagName.toLowerCase()=='select')
				{
				for(var Index=Element.options.length;Index;)
					{
					if(Element.options[--Index].value==Value)	break;
					}
				Element.selectedIndex=Index;
				}
			else	Element.value=Value;
			}
		else	{
			if(Element.tagName.toLowerCase()=='select')
				{
				if(Element.selectedIndex>=0)	return Element.options[Element.selectedIndex].value;
				}
			else	return Element.value;
			}
		}
	},
$SelectOption:function(Id)
	{
	var Element=this.$Id(Id).Element();
	if(Element&&Element.selectedIndex>=0)	return Element.options[Element.selectedIndex];
	},
//设置指定id的HTML控件的checked值
$Checked:function(Id,Value)
	{
	var Element=this.$Id(Id).Element();
	if(Element)
		{
		if(arguments.length>1)	Element.checked=!!Value;
		else	return Element.checked;
		}
	return false;
	},
//获取指定name的并且checked选中的HTML控件值
$CheckValue:function(Name)
	{
	for(var Elements=this.$Name(Name).GetElements(),Index=0;Index-Elements.length;++Index)
		{
		if(Elements[Index].checked)	return Elements[Index].value;
		}	
	},
//获取指定id的HTML控件整数值
$Int:function(Id,DefaultValue)
	{
	var Value=this.$Value(Id);
	return Value?parseInt(Value,10):(DefaultValue||0);
	},
//获取指定id的HTML控件浮点值
$Float:function(Id,DefaultValue)
	{
	var Value=this.$Value(Id);
	return Value?parseFloat(Value):(DefaultValue||0);
	},
//设置或者获取HTML控件的透明度
$Opacity:function(Element,Value)
	{
#if(IE){
	if(arguments.length>1)	Element.style.filter='alpha(opacity='+Value+')';
	else	return Element.style.filter.alphas.opacity;
}#else{
	if(arguments.length>1)	Element.style.opacity=Element.style.MozOpacity=Value/100;
	else	{
		var Value=this.$Css(Element).opacity;
		return Value?Value*100:Value;
		}
}#endif
	},
//获取HTML控件的css样式表
$Css:function(Element)
	{
#if(IE){
	return Element.currentStyle;
}#else{
	return document.defaultView.getComputedStyle(Element,false);
}#endif
	},
//获取HTML控件的绝对位置
$XY:function(Element)
	{
	for(var Left=0,Top=0;Element!=null&&Element!=document.body;Element=Element.offsetParent)
		{
		Left+=Element.offsetLeft;
		Top+=Element.offsetTop;
		if(fastCSharp.IsFixed)
			{
			var Css=this.$Css(Element),Transform=Css['transform']||Css['-webkit-transform'];
			if(Css['position']=='fixed')
				{
				Left+=this.$ScrollLeft();
				Top+=this.$ScrollTop();
				}
			if(Transform&&Transform.indexOf('matrix(')!=-1)
				{
				var XY=eval('fastCSharp.Transform_'+Transform);
				if(XY.Left)	Left+=XY.Left;
				if(XY.Top)	Top+=XY.Top;
				}
			}
		if(this.IsBorder)
			{
			var Css=this.$Css(Element);
			Left-=parseInt(0+Css['border-left-width'],10);
			Top-=parseInt(0+Css['border-top-width'],10);
			if(this.IsPadding)
				{
				Left-=parseInt(0+Css['padding-left'],10);
				Top-=parseInt(0+Css['padding-top'],10);
				}
			}
		}
	return {Left:Left,Top:Top};
	},
Transform_matrix:function(a,b,c,d,Left,Top)
	{
	return {Left:Left,Top:Top};
	},
//ForatTransform:function(Value,Number)
//	{
//	if(Value.charAt(Value.length-1)=='%')
//		{
//		Number*=parseFloat(Value.substring(0,Value.length-1))/100;
//		return Value.charAt(0)=='-'?Number:-Number;
//		}
//	return -parseFloat(Value);
//	},
//将HTML控件移动到绝对位置
$ToXY:function(Element,Left,Top)
	{
	var Value=this.$XY(Element.offsetParent);
	Element.style.left=(Left-Value.Left)+'px';
	Element.style.top=(Top-Value.Top)+'px';
	},
//HTML控件添加事件
$AddEvent:function(Element,Names,Value,IsStop)
	{
#if(IE){
	var IsBody=Element==document.body||(Element.tagName&&Element.tagName.toLowerCase()=='body');
}#else{
	if(arguments.length<4)	IsStop=1;
}#endif
	this.$DeleteEvent(Element,Names,Value,IsStop);
	for(var Index=Names.length;--Index>=0;)
		{
		var Name=Names[Index].toLowerCase(0);
#if(IE){
		if(Name.substring(0,2)!='on')	Name='on'+Name;
		if(IsBody&&Name=='onfocus')	Name='onfocusin';
		Element.attachEvent(Name,Value);
}#else{
		if(Name.substring(0,2)=='on')	Name=Name.substring(2);
		Element.addEventListener(Name,Value,!!IsStop);
}#endif
		}
#if(IE){
	fastCSharp.AddEvents.push({Element:Element,Names:Names,Value:Value,IsStop:IsStop});
}#else{
}#endif
	},
//HTML控件删除事件
$DeleteEvent:function(Element,Names,Value,IsStop)
	{
#if(IE){
	var IsBody=Element==document.body||(Element.tagName&&Element.tagName.toLowerCase()=='body');
}#else{
	if(arguments.length<4)	IsStop=1;
}#endif
	for(var Index=Names.length;--Index>=0;)
		{
		var Name=Names[Index].toLowerCase(0);
#if(IE){
		if(Name.substring(0,2)!='on')	Name='on'+Name;
		if(IsBody&&Name=='onfocus')	Name='onfocusin';
		Element.detachEvent(Name,Value);
}#else{
		if(Name.substring(0,2)=='on')	Name=Name.substring(2);
		Element.removeEventListener(Name,Value,!!IsStop);
}#endif
		}
	},
//获取HTML控件的text值
$Text:function(Element,Value)
	{
#if(IE){
	if(Element.nodeType==3)
		{
		if(Value!=null)	Element.nodeValue=Value;
		else	return Element.nodeValue;
		}
	else	{
		if(Value!=null)	Element.innerText=Value;
		else	return Element.innerText;
		}
}#else{
	if(Value!=null)	Element.textContent=Value;
	else	return Element.textContent;
}#endif
	},
//在选定位置粘贴文本数据，一般用于TextArea
$Paste:function(Element,Text,IsAll)
	{
#if(IE){
	var Selection=Element.document.selection.createRange();
	if(IsAll&&Selection.text=='')	Element.value=Text;
	else	{
		Selection.text=Text;
		Selection.moveStart('character',0);
		Selection.select();
		}
}#else{
	var StartIndex=Element.selectionStart,EndIndex=Element.selectionEnd;
	if(IsAll&&StartIndex==EndIndex)
		{
		Element.value=Text;
		StartIndex=0;
		}
	else	{
		var OldText=Element.value;
		Element.value=OldText.substring(0,StartIndex)+Text+OldText.substring(EndIndex);
		}
	Element.selectionStart=Element.selectionEnd=StartIndex+Text.length;
}#endif
	},
$ElementName:function(Element,Name,Value)
	{
	Element=fastCSharp.$(Element);
	if(Value==null)	while(Element&&Element.Get(Name)==null)	Element=Element.Parent();
	else	while(Element&&Element.Get(Name)!=Value)	Element=Element.Parent();
	return Element;
	},
Eval:function(Function,Arguments,ArgumentIndex)
	{
	if(Arguments.length>(ArgumentIndex||0))	Function.apply(null,Arguments.slice(ArgumentIndex||0));
	else	Function();
	},
//返回URI的Qeury的整数值
QueryInt:function(Name)
	{
	var Value=fastCSharp.Query[Name];
	return Value?parseInt(0+Value,10):null;
	},
//创建Ajax请求
Request:function()
	{
	var Value=null;
#if(IE){
	if(window.ActiveXObject)
		{
		var Xmls=['Microsoft.XMLHTTP','Msxml2.XMLHTTP'];
//		var Xmls=['Microsoft.XMLHTTP','MSXML2.XMLHTTP','MSXML2.XMLHTTP.3.0','MSXML2.XMLHTTP.4.0','MSXML2.XMLHTTP.5.0'];
		for(var Index=Xmls.length;--Index>=0&&(!Value);)
			{
			try	{
				Value=new ActiveXObject(Xmls[Index]);
				}
			catch(e){}
			}
		}
}#else{
	if(window.XMLHttpRequest&&(Value=new XMLHttpRequest).overrideMimeType)	Value.overrideMimeType('text/xml');
}#endif
	if(Value==null)	alert('你的浏览器不支持服务器请求,请升级您的浏览器！');
	return Value;
	}
//,LoadedFunction:function(Function)
//	{
//	var IsLoad=0,OnLoads=[],Eval=function(OnLoad)
//		{
//		if(OnLoad.IsOnLoad)	this.OnLoad(OnLoad.OnLoad);
//		else	OnLoad.OnLoad();
//		};
//	var Proxy=function()
//		{
//		this.Eval(Function,arguments);
//		if(!IsLoad)
//			{
//			IsLoad=1;
//			for(var Index=-1;++Index!=OnLoads.length;Eval(OnLoads[Index]));
//			OnLoads=null;
//			}
//		};
//	Proxy.OnLoad=function(OnLoad,IsOnLoad)
//		{
//		if(IsLoad)	Eval({OnLoad:OnLoad,IsOnLoad:IsOnLoad});
//		else	OnLoads.push({OnLoad:OnLoad,IsOnLoad:IsOnLoad});
//		};
//	return Proxy;
//	}
		};
fastCSharp.Copy(fastCSharp,BaseFunction.prototype);
fastCSharp.Copy(fastCSharp,{
BaseFunction:BaseFunction,
Void:function(){},
$Cache:{},
Object:[],
Modules:{},
Functions:{},
LoadModules:{},
OnLoads:[],
#if(IE){
Events:[],
AddEvents:[],
}#else{
}#endif
//document加载后事件
OnLoad:function(OnLoad,This,IsOnce)
	{
	if(This)	OnLoad=this.ThisFunction(This,OnLoad);
	if(!IsOnce)	this.OnLoadedHash.Add(OnLoad);
	if(this.IsLoad)	OnLoad();
	else	this.OnLoads.push(OnLoad);
	},
//模块加载后的通知
LoadModule:function(Name)
	{
	this.Modules[Name]=1;
	for(var Loads=this.LoadModules[Name],Index=Loads?Loads.length:0;--Index>=0;)
		{
		var Load=Loads[Index];
		if(Load&&Load[Name])
			{
			Load[Name]=null;
			if(--Load.Count==0)
				{
				var OnLoad=Load.OnLoad,IsLoad=Load.IsLoad;
				Load=null;
				if(OnLoad)
					{
					if(IsLoad)	this.OnLoad(OnLoad);
					else	OnLoad();
					}
				}
			}
		}
	},
//加载指定名称的模块
AppendJsWhenNull:function(Name,Version)
	{
	if(this.Modules[Name])
		{
		if(this.Modules[Name]!=Name)	this.InsertFunction(this.Modules[Name],Name)
		}
	else	{
		this.Modules[Name]=Name;
#if(IE){
		this.AppendJs(this.JsDomain+'js/'+Name.replace('.','/')+(Name=='htmlEditor'?'.ie':'')+'.js?v='+(Version||this.Version));
}#else{
		this.AppendJs(this.JsDomain+'js/'+Name.replace('.','/')+(Name=='htmlEditor'?'.ns':'')+'.js?v='+(Version||this.Version));
}#endif
		}
	},
//模块加载事件
OnModule:function(Name,OnLoad,IsLoad,Version)
	{
	if(typeof(Name)=='string')	Name=[Name];
	for(var Value=[],Index=Name.length;--Index>=0;)
		{
		if(this.Modules[Name[Index]]==null||this.Modules[Name[Index]]==Name[Index])	Value.push(Name[Index]);
		}
	if(IsLoad==null)	IsLoad=1;
	if(Value.length)
		{
		var Load={IsLoad:IsLoad,OnLoad:OnLoad,Count:Value.length};
		for(Index=Value.length;--Index>=0;)
			{
			if(this.LoadModules[Value[Index]]==null)	this.LoadModules[Value[Index]]=[];
			this.LoadModules[Value[Index]].push(Load);
			Load[Value[Index]]=1;
			this.AppendJsWhenNull(Value[Index],Version);
			}
		}
	else if(OnLoad)
		{
		if(IsLoad)	this.OnLoad(OnLoad);
		else	OnLoad();
		}
	},
//创建一个指定名称的模块对象
NewObject:function(Name,Argument)
	{
	if(this.Functions[Name]&&this.Functions[Name]!=Name)	return new this.Functions[Name](Argument||{});
	else if(!fastCSharp.PageView.LoadError)	fastCSharp.AjaxGet(null,'__PUBERROR__',{error:'Not Found '+Name});
	},
IsArray:function(Value)
	{
	return Object.prototype.toString.apply(Value)=='[object Array]';
	},
//类似数组数据转换为数组
ToArray:function(Value,GetValue,StartIndex)
	{
	if(this.IsArray(Value))	return Value.ToArray(GetValue,StartIndex);
	if(StartIndex==null)	StartIndex=0;
	if(Value.length!=null)
		{
		for(var Values=[],Index=(StartIndex||0)-1;++Index<Value.length;Values.push(GetValue?GetValue(Value[Index]):Value[Index]));
		return Values;
		}
	},
//动态加载css文件
AppendCss:function(Href,Charset)
	{
	this.AppendHead('link',{rel:'stylesheet',type:'text/css',href:Href,charset:Charset||this.Charset});
	},
//URI的Query解析
FillQuery:function(Value,Search)
	{
	for(var Query=Search.split('&'),Index=Query.length;--Index>=0;)
		{
		Value[unescape((Search=Query.pop().split('='))[0])]=Search.length<2?'':unescape(Search[1]);
		}
	},
//URI的Query+HASH解析
CreateQuery:function(Location)
	{
	var Value={},Search=this.$Search(Location),Hash=this.$Hash(Location);
	if(Hash.length&&Hash.indexOf('=')+1)	this.FillQuery(Value,Hash);
	if(Search.length)	this.FillQuery(Value,Search);
	return Value;
	},
//页面重定向
LocationReferrer:function(Path)
	{
	location=Path+(Path.indexOf('#')+1?'&':'#')+'referrer='+(document.location.hash.toString()||'#').Escape();
	},
JsonLoopObjects:[],
SetJsonLoop:function(Index,Value)
	{
	var CacheValue=this.JsonLoopObjects[Index];
	if(CacheValue)
		{
		if(this.IsArray(CacheValue))	CacheValue.push.apply(CacheValue,Value);
		else	fastCSharp.Copy(CacheValue,Value);
		return CacheValue;
		}
	return this.JsonLoopObjects[Index]=Value;
	},
GetJsonLoop:function(Index,Array)
	{
	return this.JsonLoopObjects[Index]||(this.JsonLoopObjects[Index]=Array||{});
	},
EvalJson:function(Text)
	{
	this.JsonLoopObjects=[];
	return eval(Text);
	},
JsStart:"\n<script language='javascript' type='text/javascript'>\n//<![CDATA[\n",
JsEnd:'\n//]]>\n</script>\n',
IsTest:document.location.toString().toLowerCase().substring(4,16)=='://localhost',
OverZIndex:100,
ZIndex:10000,
#if(IE){
IsIE:1,
}#else{
//IsIE11:/rv\:\d+\.\d+/.test(navigator.appVersion),
}#endif
IsLoad:0
		});
//----常用类定义 开始----
//浏览器时间包装
var BrowserEvent=function(E)
	{
	fastCSharp.Copy(this,this.Event=(E||event));
	this.Return=true;
#if(IE){
	this.target=this.srcElement;
	this.pageX=this.clientX;
	this.pageY=this.clientY;
	this.which=this.keyCode;
}#else{
	this.srcElement=this.target;
	this.clientX=this.pageX;
	this.clientY=this.pageY;
	this.keyCode=this.which;
}#endif
	};
BrowserEvent.prototype={
//取消冒泡
CancelBubble:function()
	{
	this.Return=false;
#if(IE){
	this.Event.returnValue=false;
}#else{
	this.Event.preventDefault();
}#endif
	},
//获取事件匹配的HTML控件
Element:function(Type,Value)
	{
	var Element=fastCSharp.$(this.srcElement);
	if(Type)	while(Element&&Element[Type]()!=Value)	Element=Element.Parent();
	return Element;
	},
//获取事件匹配的HTML控件
ElementName:function(Name,Value)
	{
	return fastCSharp.$ElementName(this.srcElement,Name,Value);
	}
		};
//包装指定函数的this值
fastCSharp.ThisFunction=function(This,Function,Arguments,IsArgument)
	{
	var Value=function()
		{
		return Function.apply(This,IsArgument==null||IsArgument?fastCSharp.ToArray(arguments).concat(Arguments||[]):Arguments);
		};
	Arguments=Arguments?fastCSharp.ToArray(Arguments):null;
	return Value;
	};
//包装指定事件函数的this值
fastCSharp.ThisEvent=function(This,Function,Arguments,Frame)
	{
	var Value=function(E)
		{
#if(IE){
		var Event=new fastCSharp.Functions.BrowserEvent(Frame?Frame.event||event:event);
}#else{
		var Event=new fastCSharp.Functions.BrowserEvent(E);
}#endif
		Function.apply(This,[Event].concat(Arguments?fastCSharp.ToArray(Arguments):[]));
		return Event.Return;
		};
	return Value;
	};
fastCSharp.Functions.BrowserEvent=BrowserEvent;
//HTML空间包装
var Element=function(Value,Parent)
	{
	if(typeof(Value)=='string')
		{
		this.FilterString=Value;
		this.Parent=Parent?(Parent.IsElement?Parent.Element():Parent):document.body;
		}
	else if(Value)
		{
		if(Value.IsElement)
			{
			this.FilterString=Value.FilterString;
			this.Parent=Value.Parent;
			this.Elements=Value.Elements;
			}
		else 	this.Elements=fastCSharp.IsArray(Value)?Value:[Value];
		}
	else	this.Elements=[];
	};
(fastCSharp.Functions.Element=Element).Inherit(fastCSharp.BaseFunction,{
IsElement:1,
IsParent:function(Element)
	{
	return !this.Parent||(Element&&this.$IsParent(Element.IsElement?Element.Element():Element,this.Parent));
	},
//创建id筛选器
FilterId:function()
	{
	var Id=this.FilterValue();
	this.FilterBuilder.push('function(Element,Value){if(Element==this.Parent?Value.IsParent(Element=document.getElementById("');
	this.FilterBuilder.push(Id);
	this.FilterBuilder.push('")):Element.id=="');
	this.FilterBuilder.push(Id);
	this.FilterBuilder.push('")(');
	this.FilterNext(1);
	},
//创建节点tag名称筛选器
FilterChildTag:function()
	{
	if(this.FilterIndex==this.FilterString.length||this.FilterString.charCodeAt(this.FilterIndex)-47)
		{
		var Name=this.FilterValue();
		this.FilterBuilder.push('function(Element,Value){for(var Elements=Element.childNodes,Index=0;Index-Elements.length;)if((Element=Elements[Index++])');
		if(Name){
			this.FilterBuilder.push('.tagName&&Element.tagName.toLowerCase()=="');
			this.FilterBuilder.push(Name.toLowerCase());
			this.FilterBuilder.push('"');
			}
		this.FilterBuilder.push(')(');
		this.FilterNext(1);
		}
	else	{
		++this.FilterIndex;
		this.FilterTag();
		}
	},
//创建tag名称筛选器
FilterTag:function()
	{
	var Name=this.FilterValue();
	if(Name)	this.FilterChildren('tagName',Name);
	else	{
		this.FilterChildren();
		this.FilterBuilder.push('if(Element=Childs[Index++])(');
		this.FilterNext(1);
		}
	},
//创建节点筛选器
FilterChildren:function(Name,Value)
	{
	this.FilterBuilder.push('function(Element,Value){var Elements=[],ElementIndex=-1;while(ElementIndex-Elements.length)for(var Childs=ElementIndex+1?Elements[ElementIndex++].childNodes:[arguments[++ElementIndex]],Index=0;Index-Childs.length;Elements.push(Element))');
	if(Name){
		if(!Value)	Value=this.FilterValue();
		this.FilterBuilder.push('if((Element=Childs[Index++]).');
		this.FilterBuilder.push(Name);
		this.FilterBuilder.push('&&Element.');
		this.FilterBuilder.push(Name);
		this.FilterBuilder.push('.toLowerCase()');
		this.FilterBuilder.push('=="');

		this.FilterBuilder.push(Value.toLowerCase());
		this.FilterBuilder.push('")(');
		this.FilterNext(1);
		}
	},
//创建class筛选器
FilterClass:function()
	{
	this.FilterChildren();
	this.FilterBuilder.push('if((Element=Childs[Index++]).className&&Element.className.split(" ").IndexOf("');
	this.FilterBuilder.push(this.FilterValue());
	this.FilterBuilder.push('")+1)(');
	this.FilterNext(1);
	},
//创建属性筛选器
FilterAttribute:function()
	{
	this.FilterChildren();
	var Value=this.FilterValue().split('=');
	if(Value.length==1)
		{
		this.FilterBuilder.push('if(fastCSharp.$IsAttribute(Element=Childs[Index++],"');
		this.FilterBuilder.push(Value[0]);
		this.FilterBuilder.push('"))(');
		}
	else	{
		this.FilterBuilder.push('if(fastCSharp.$Attribute(Element=Childs[Index++],"');
		this.FilterBuilder.push(Value[0]);
		this.FilterBuilder.push('")=="');
		this.FilterBuilder.push(Value[1]);
		this.FilterBuilder.push('")(');
		}
	this.FilterNext(1);
	},
//创建name筛选器
FilterName:function()
	{
	this.FilterChildren();
	this.FilterBuilder.push('if(fastCSharp.$Attribute(Element=Childs[Index++],"name")=="');
	this.FilterBuilder.push(this.FilterValue());
	this.FilterBuilder.push('")(');
	this.FilterNext(1);
	},
//创建css筛选器
FilterCss:function()
	{
	this.FilterChildren();
	var Value=this.FilterValue().split('=');
	this.FilterBuilder.push('if(fastCSharp.$Css(Element=Childs[Index++],"');
	this.FilterBuilder.push(Value[0]);
	this.FilterBuilder.push('")=="');
	this.FilterBuilder.push(Value[1]);
	this.FilterBuilder.push('")(');
	this.FilterNext(1);
	},
//筛选器分段值解析
FilterValue:function()
	{
	var Index=this.FilterIndex;
	while(this.FilterIndex!=this.FilterString.length&&!this.FilterCreator[this.FilterString.charCodeAt(this.FilterIndex)])	++this.FilterIndex;
	return this.FilterString.substring(Index,this.FilterIndex);
	},
//筛选器分段解析
FilterNext:function(IsEnd)
	{
	if(this.FilterIndex!=this.FilterString.length)
		{
		var Creator=this.FilterCreator[this.FilterString.charCodeAt(this.FilterIndex)];
		if(Creator)
			{
			++this.FilterIndex;
			Creator();
			}
		else	this.FilterTag();
		if(IsEnd)	this.FilterBuilder.push(')(Element,Value);}');
		}
	else if(this.FilterBuilder.length)	this.FilterBuilder.push('Value.Elements.push)(Element);}');
	},
//获取HTML控件集合
GetElements:function()
	{
	if(this.Elements)	return this.Elements;
	if(!this.Filter)
		{
		var Filter=this.FilterString?fastCSharp.$Cache[this.FilterString]:function(){};
		if(!Filter)
			{
			this.FilterIndex=0;
//						#id						*name						.className					/tagName						:type							?css							@value
			this.FilterCreator={35:fastCSharp.ThisFunction(this,this.FilterId),42:fastCSharp.ThisFunction(this,this.FilterName),46:fastCSharp.ThisFunction(this,this.FilterClass),47:fastCSharp.ThisFunction(this,this.FilterChildTag),58:fastCSharp.ThisFunction(this,this.FilterChildren,['type']),63:fastCSharp.ThisFunction(this,this.FilterCss),64:fastCSharp.ThisFunction(this,this.FilterAttribute)};
			this.FilterBuilder=[];
			this.FilterNext();
			eval('Filter='+this.FilterBuilder.join('')+';');
			fastCSharp.$Cache[this.FilterString]=Filter;
			this.FilterBuilder=this.FilterCreator=null;
			}
		this.Filter=Filter;
		}
	this.Elements=[];
	this.Filter(this.Parent,this);
	return this.Elements;
	},
Count:function()
	{
	return this.GetElements().length;
	},
//获取或者设置HTML控件的id值
Id:function(Value)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		if(Elements.length)	Elements[0].id=Value;
		return this;
		}
	else if(Elements.length)	return Elements[0].id;
	},
//获取第一个HTML控件的name值 或者 设置HTML控件的name值
Name:function(Value)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		for(var Index=0;Index-Elements.length;Elements[Index++].name=Value);
		return this;
		}
	else if(Elements.length)	return this.$Name(Elements[0]);
	},
//获取第一个HTML控件的class值 或者 设置HTML控件的class值
Class:function(Name,IsRemove)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		if(Name!=null)
			{
			Name=fastCSharp.IsArray(Name)?Name:Name.split(' ');
			for(var Index=0;Index-Elements.length;this.$Class(Elements[Index++],Name,IsRemove));
			}
		return this;
		}
	else if(Elements.length)	return Elements[0].className;
	},
//获取第一个HTML控件的属性值 或者 设置HTML控件的属性值
Attribute:function(Name,Value)
	{
	var Elements=this.GetElements();
	if(arguments.length>1)
		{
		for(var Index=0;Index-Elements.length;Elements[Index++][Name]=Value);
		return this;
		}
	else if(Elements.length)	return this.$Attribute(Elements[0],Name);
	},
//设置HTML控件的属性值
Set:function(Name,Value)
	{
	return this.Attribute(Name,Value);
	},
//获取第一个HTML控件的属性值
Get:function(Name)
	{
	return this.Attribute(Name);
	},
//切换逻辑状态
ChangeBool:function(Name)
	{
	var Elements=this.GetElements();
	for(var Index=0;Index-Elements.length;++Index)	Elements[Index][Name]=!Elements[Index][Name];
	return this;
	},
//获取第一个HTML控件的样式表 或者 设置HTML控件的样式表
Css:function(Name,Value)
	{
	var Elements=this.GetElements();
	if(arguments.length>1)
		{
		Name=Name.split(',');
		for(var ElementIndex=0;ElementIndex-Elements.length;)
			{
			for(var Element=Elements[ElementIndex++],Index=0;Index-Name.length;Element.style[Name[Index++]]=Value);
			}
		return this;
		}
	else if(Elements.length)
		{
		var Css=this.$Css(Elements[0]);
		return arguments.length?Css[Name]:Css;
		}
	},
//获取HTML控件的样式表集合
CssArray:function(Name)
	{
	for(var Value=[],Elements=this.GetElements(),Index=0;Index-Elements.length;Value.push(this.$Css[Elements[Index++]][Name]));
	return Value;
	},
//获取第一个HTML控件的样式表
CssValue:function(Name)
	{
	return this.Attribute(Name)||this.Css(Name);
	},
//显示或者隐藏HTML控件
Display:function(IsShow)
	{
	return arguments.length?this.Css('display',typeof(IsShow)=='string'?IsShow:(IsShow?'':'none')):this.Css('display');
	},
//显示或者隐藏HTML控件
ChangeDisplay:function()
	{
	for(var Elements=this.GetElements(),Index=0;Index-Elements.length;)
		{
		var Element=Elements[Index++];
		Element.style.display=Element.style.display?'':'none';
		}
	return this;
	},
//获取第一个HTML控件可用状态 或者 设置HTML控件可用状态
Disabled:function(Value)
	{
	return arguments.length?this.Css('disabled',Value):this.Css('disabled');
	},
//设置HTML控件显示层级
TopIndex:function()
	{
	if(this.CssArray('zIndex').Max()!=fastCSharp.ZIndex&&this.Elements.length)	this.Css('zIndex',++fastCSharp.ZIndex);
	},
//获取第一个HTML控件光标状态 或者 设置HTML控件光标状态
Cursor:function(Value)
	{
	return arguments.length?this.Css('cursor',Value):this.Css('cursor');
	},
//设置HTML控件可见状态
Hide:function(IsShow)
	{
	return this.Css('visibility',IsShow?'':'hidden');
	},
//第一个HTML控件聚焦
Focus:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)	Elements[0].focus();
	return this;
	},
//第一个HTML控件失焦
Blur:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)	Elements[0].blur();
	return this;
	},
//获取第一个HTML控件的透明度 或者 设置HTML控件的透明度
Opacity:function(Value)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		for(var Index=0;Index-Elements.length;this.$Opacity(Elements[Index++],Value));
		return this;
		}
	else if(Elements.length)	return this.$Opacity(Elements[0]);
	},
//获取第一个HTML控件的横向位置 或者 设置HTML控件的横向位置
Left:function(Value)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		for(var Index=0;Index-Elements.length;Elements[Index++].style.left=Value+'px');
		return this;
		}
	else if(Elements.length)	return Elements[0].offsetLeft;
	},
//获取第一个HTML控件的纵向位置 或者 设置HTML控件的纵向位置
Top:function(Value)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		for(var Index=0;Index-Elements.length;Elements[Index++].style.top=Value+'px');
		return this;
		}
	else if(Elements.length)	return Elements[0].offsetTop;
	},
//获取第一个HTML控件的像素宽度
Width:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)
		{
		do	{
			var Element=Elements[0];
			if(!Element)	break;
			if(Element.offsetWidth)	return Element.offsetWidth;
			Elements=Element.children;
			}
		while(Elements.length);
		}
	return 0;
	},
//获取第一个HTML控件的像素高度
Height:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)
		{
		do	{
			var Element=Elements[0];
			if(!Element)	break;
			if(Element.offsetHeight)	return Element.offsetHeight;
			Elements=Element.children;
			}
		while(Elements.length);
		}
	return 0;
	},
//获取第一个HTML控件的绝对位置
XY:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)	return this.$XY(Elements[0]);
	},
//将HTML控件移动到绝对位置
ToXY:function(Left,Top)
	{
	for(var Elements=this.GetElements(),Index=0;Index-Elements.length;this.$ToXY(Elements[Index++],Left,Top));
	return this;
	},
//获取第一个HTML控件的滚动条宽度
ScrollWidth:function()
	{
	var Elements=this.GetElements();
	return Elements.length?Elements[0].scrollWidth:0;
	},
//获取第一个HTML控件的滚动条高度
ScrollHeight:function()
	{
	var Elements=this.GetElements();
	return Elements.length?Elements[0].scrollHeight:0;
	},
//获取第一个HTML控件的滚动条的横向位置 或者 设置HTML控件的滚动条的横向位置
ScrollLeft:function(Value)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		for(var Index=0;Index-Elements.length;Elements[Index++].scrollLeft=Value);
		}
	else	return Elements.length?Elements[0].scrollLeft:0;
	},
//获取第一个HTML控件的滚动条的纵位置 或者 设置HTML控件的滚动条的纵位置
ScrollTop:function(Value)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		for(var Index=0;Index-Elements.length;Elements[Index++].scrollTop=Value);
		}
	else	return Elements.length?Elements[0].scrollTop:0;
	},
//获取第一个HTML控件的滚动条的坐标位置与宽高
Scroll:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)
		{
		var Element=Elements[0];
		return {Left:Element.scrollLeft,Top:Element.scrollTop,Width:Element.scrollWidth,Height:Element.scrollHeight};
		}
	},
//HTML控件添加事件
AddEvent:function(Name,Value,IsStop)
	{
#if(IE){
}#else{
	if(arguments.length<3)	IsStop=1;
}#endif
	Name=Name.split(',');
	for(var Elements=this.GetElements(),Index=0;Index-Elements.length;this.$AddEvent(Elements[Index++],Name,Value,IsStop));
	return this;
	},
//HTML控件删除事件
DeleteEvent:function(Name,Value,IsStop)
	{
#if(IE){
}#else{
	if(arguments.length<3)	IsStop=1;
}#endif
	Name=Name.split(',');
	for(var Elements=this.GetElements(),Index=0;Index-Elements.length;this.$DeleteEvent(Elements[Index++],Name,Value,IsStop));
	return this;
	},
//获取第一个HTML控件的HTML代码 或者 设置HTML控件的HTML代码
Html:function(Value,ToHtml)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		if(ToHtml)	Value=Value.ToHTML();
		for(var Index=0;Index-Elements.length;Elements[Index++].innerHTML=Value);
		return this;
		}
	else if(Elements.length)	return Elements[0].innerHTML;
	},
//获取第一个HTML控件的文本代码 或者 设置HTML控件的文本代码
Text:function(Value)
	{
	var Elements=this.GetElements();
	if(arguments.length)
		{
		for(var Index=0;Index-Elements.length;this.$Text(Elements[Index++],Value));
//Elements[Index++].innerHTML=Value.ToHTML().replace(/\r\n/g,'<br />').replace(/[\r\n]/g,'<br />'));
		return this;
		}
	else if(Elements.length)	return this.$Text(Elements[0]);
	},
//获取第一个HTML控件的value值 或者 设置HTML控件的value值
Value:function(Value)
	{
	return arguments.length?this.Attribute('value',Value):this.Attribute('value');
	},
//获取第一个HTML控件的tag名称
TagName:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)	return Elements[0].tagName.toLowerCase();
	},
//移动HTML控件到指定的父级控件
To:function(Parent)
	{
	if(!Parent)	Parent=document.body;
	else if(Parent.$)	Parent=Parent.Element();
	for(var Elements=this.GetElements(),Index=-1;++Index-Elements.length;)
		{
		if(this.$Parent(Elements[Index]).Element()!=Parent)	Parent.appendChild(Elements[Index]);
		}
	return this;
	},
//删除HTML控件
Delete:function()
	{
	for(var Elements=this.GetElements(),Index=-1;++Index-Elements.length;this.$Parent(Elements[Index]).Element().removeChild(Elements[Index]));
	return this;
	},
//获取第一个HTML控件
Element:function()
	{
	return this.GetElements()[0];
	},
//获取第一个HTML控件的父控件
Parent:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)	return this.$Parent(Elements[0]);
	},
//获取HTML控件的子节点集合
Child:function()
	{
	for(var Value=[],Elements=this.GetElements(),Index=0;Index-Elements.length;++Index)
		{
		if(Elements[Index].childNodes)	Value.push(fastCSharp.ToArray(Elements[Index].childNodes));
		}
	return this.$(Value.concat.apply([],Value));
	},
//替换第一个HTML控件
Replace:function(Element)
	{
	var Elements=this.GetElements();
	if(Elements.length)	this.$Parent(Elements[0]).Element().replaceChild(Element.GetElements()[0],Elements[0]);
	},
//获取第一个HTML控件下一个兄弟节点
Next:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)	return this.$Next(Elements[0]);
	},
//获取第一个HTML控件上一个兄弟节点
Previous:function()
	{
	var Elements=this.GetElements();
	if(Elements.length)	return this.$Previous(Elements[0]);
	},
//获取HTML控件的子孙节点集合
Childs:function(IsChild)
	{
	for(var Value=[],Elements=this.GetElements(),Index=0;Index-Elements.length;++Index)
		{
		var Values=this.$Childs(Elements[Index],IsChild).GetElements();
		if(Values)	Value.push(Values)
		}
	return this.$(Value.concat.apply([],Value));
	},
//将HTML控件插入到指定节点的前面
InsertBefore:function(Element,Parent)
	{
	if(Element.IsElement)	Element=Element.Element();
	if(Element)
		{
		if(!Parent)	Parent=this.$Parent(Element);
		if(Parent.IsElement)	Parent=Parent.Element();
		for(var Elements=this.GetElements(),Index=0;Index-Elements.length;Parent.insertBefore(Elements[Index++],Element));
		}
	else	this.To(Parent);
	return this;
	},
//HTML控件集合转换为数组集合
ToArray:function(GetValue,StartIndex)
	{
	return this.GetElements().ToArray(GetValue,StartIndex);
	},
//foreache遍历HTML控件集合
For:function(Action)
	{
	return this.GetElements().For(Action);
	},
//获取第一个匹配的HTML控件
First:function(IsValue)
	{
	return this.GetElements().First(IsValue);
	}
		});
//自定义事件
fastCSharp.Event=function(OnAdd,This)
	{
	if(This)	OnAdd=this.ThisFunction(OnAdd,This);
	var Functions=[],Function=function()
		{
		for(var Argument=fastCSharp.ToArray(arguments),Index=0;Index-Functions.length;Functions[Index++].apply(null,Argument));
		};
	Function.AddFunction=function(Value)
		{
		if(Functions.IndexOf(Value)==-1)
			{
			Functions.push(Value);
			if(OnAdd&&OnAdd())	Value();
			}
		};
	Function.Add=function(Value)
		{
		if(Value)
			{
			if(Value.IsShowEvent)
				{
				Value=Value.Get();
				for(var Index=0;Index-Value.length;this.AddFunction(Value[Index++]));
				}
			else	this.AddFunction(Value);
			}
		return this;
		};
	Function.Remove=function(Value)
		{
		if(Value)
			{
			if(Value.IsShowEvent)
				{
				Value=Value.Get();
				for(var Index=0;Index-Value.length;Functions.Remove(Value[Index++]));
				}
			else	Functions.Remove(Value);
			}
		return this;
		};
	Function.Clear=function()
		{
		Functions.length=0;
		return this;
		};
	Function.Get=function()
		{
		return Functions;
		};
	Function.IsShowEvent=1;
	return Function;
	};
//Ajax请求
var HttpRequest=function()
	{
	this.GetArgument(arguments[0],{OnResponse:null},1);
	this.Requesting=false;
	this.WriteOrder=0;
	this.ReadOrder=-1;
	this.Queue=[];
	this.OnResponse=fastCSharp.Event().Add(this.OnResponse);
	this.ReadyStateChange=fastCSharp.ThisFunction(this,this.OnReadyStateChange);
//	this.AbortFunction=fastCSharp.ThisFunction(this,this.Abort);
	};
(fastCSharp.Functions.HttpRequest=HttpRequest).Inherit(fastCSharp.BaseFunction,{
Get:function(Url,IsRandom)
	{
	this.Request({Method:'GET',Url:Url,IsRandom:IsRandom==null?0:IsRandom});
	},
Post:function(Url,Send,IsRandom)
	{
	this.Request({Method:'POST',Url:Url,Send:Send,IsRandom:IsRandom});
	},
Request:function(Request)
	{
	if(Request.Send)
		{
		if(window.FormData&&(Request.Send instanceof FormData))	Request.IsFormData=1;
		else	{
			var Send=this.ToJson(Request.Send);
			Request.Send=Send=='{}'?'':Send;
//			Request.Send=Send=='{}'?'':('__JSON__='+Send.Escape());
			}
		}
	this.Queue[this.WriteOrder++]=Request;
	if(!this.Requesting)
		{
		this.Requesting=true;
		this.MakeXMLHttpRequest();
		}
	},
MakeXMLHttpRequest:function()
	{
	var Request=this.ReadXMLHttpRequest=fastCSharp.Request(),Info=this.Queue[++this.ReadOrder];
	var Url=Info.Url;
	if(Info.Method==null||Info.IsFormData)	Info.Method='POST';
	if(Info.Send&&!Info.IsFormData)
		{
		if(Info.Method=='GET')
			{
			Url+=(Url.indexOf('?')+1?'&':'?')+'__JSON__='+Info.Send.Escape();
			Info.Send=null;
			}
		else	Info.Send=Info.Send.replace(/\xA0/g,' ');
		}
	if(Info.IsRandom==null||Info.IsRandom)	Url+=(Url.indexOf('?')+1?'&':'?')+'t='+(new Date).getTime();
	Info.Request=Request;
#if(IE){
}#else{
	if(Info.Method=='GET'&&!Info.UserName&&!Info.IsOnLoad)
		{
		Info.RetryCount=2;
		fastCSharp.AppendJs(Url,null,null,(fastCSharp.AjaxAppendJs=Info).OnError);
		}
	else	{
}#endif
		Request.onreadystatechange=this.ReadyStateChange;
		if(Info.UserName==null||Info.UserName=='')	Request.open(Info.Method,Url,true);
		else	Request.open(Info.Method,Url,true,Info.UserName,Info.Password);
		if(Info.Header)
			{
			for(var Name in Info.Header)	Request.setRequestHeader(Name,Info.Header[Name]);
			}
		else if(Info.Method=='POST'&&!Info.IsFormData)	Request.setRequestHeader('Content-Type','application/json; charset=utf-8');
//		else	Request.setRequestHeader('Content-Type','text/javascript;charset='+fastCSharp.CharSet);
//		fastCSharp.$OnClose.Add(this.AbortFunction);
		Request.send(Info.Send);
#if(IE){
}#else{
		}
}#endif
	},
//Abort:function()
//	{
//	var Request=this.Queue[this.ReadOrder];
//	if(Request.Request)	Request.Request.abort();
//	},
NextRequest:function()
	{
	if(this.ReadOrder==this.WriteOrder-1)
		{
		this.WriteOrder=0;
		this.ReadOrder=-1;
		this.Requesting=false;
		}
	else	this.MakeXMLHttpRequest();
	},
GetError:function(Request,Text)
	{
	var ErrorPath='__AJAX__?__AJAXCALL__=__PUBERROR__&';
	if(!Request)
		{
		var Request=this.Queue[this.ReadOrder];
		this.NextRequest();
		}
	if(Request.OnError)	Request.OnError(this);
	if(Request.Url.substring(0,ErrorPath.length)!=ErrorPath)
		{
		fastCSharp.Ajax(null,'__PUBERROR__',{error:'服务器请求失败 : '+location.toString()+'\r\n'+Request.Url+(Request.Send&&!Request.IsFormData?('\r\n'+Request.Send.length+'\r\n'+Request.Send):'')+(Text?'\r\neval '+Text:'')},null,null,0);
		}
//	if(confirm('服务器请求失败 : '+location.toString()+'\n'+Request.Url+'\n'+Request.Send+'\n\n是否需要重试?'))
//		{
//		this.ReadOrder--;
//		this.MakeXMLHttpRequest();
//		}
//	else	this.NextRequest();
	},
OnLoad:function(CallBack,Arguments)
	{
	CallBack.apply(null,Arguments);
	},
CallBack:function()
	{
	var Request=this.Queue[this.ReadOrder];
	this.NextRequest();
	if(Request.CallBack)
		{
		if(Request.IsOnLoad)	fastCSharp.OnLoad(fastCSharp.ThisFunction(this,this.OnLoad,[Request.CallBack,fastCSharp.ToArray(arguments)]),null,1);
		else	Request.CallBack.apply(null,fastCSharp.ToArray(arguments));
		}
	},
OnReadyStateChange:function()
	{
	var Request=this.ReadXMLHttpRequest;
	if(Request.readyState==4)
		{
//		fastCSharp.$OnClose.Remove(this.AbortFunction);
		if(Request.status==200||Request.status==304)
			{
			var Text=Request.responseText,Request=this.Queue[this.ReadOrder];
			this.OnResponse(Text);
			if(Request.CallBack)
				{
				try	{	fastCSharp.EvalJson(Text);	}
				catch(e){	this.GetError(Request,Text);	}
				}
			else	{
				this.NextRequest();
				fastCSharp.EvalJson(Text);
				}
			}
		else	this.GetError();
		}
	}
		});
fastCSharp.HttpRequest=new HttpRequest();
fastCSharp.Ajax=function(CallBack,Name,Send,OnError,IsOnLoad,IsRandom)
	{
	if(!CallBack)	CallBack=function(){};
	var HttpRequest=new fastCSharp.Functions.HttpRequest;
	HttpRequest.Request({Method:'POST',Url:'__AJAX__?__AJAXCALL__='+Name+'&__CALLBACK__='+HttpRequest.SysObject.Escape()+'.CallBack',CallBack:CallBack,Send:Send,OnError:OnError,IsOnLoad:IsOnLoad,IsRandom:IsRandom});
//	return HttpRequest;
	};
#if(IE){
}#else{
fastCSharp.AjaxGetRequest=new fastCSharp.Functions.HttpRequest;
}#endif
fastCSharp.AjaxGet=function(CallBack,Name,Send,OnError,IsOnLoad,IsRandom)
	{
	if(!CallBack)	CallBack=function(){};
#if(IE){
	var HttpRequest=new fastCSharp.Functions.HttpRequest;
	HttpRequest.Request({Method:'GET',Url:'__AJAX__?__AJAXCALL__='+Name+'&__CALLBACK__='+HttpRequest.SysObject.Escape()+'.CallBack',CallBack:CallBack,Send:Send,OnError:OnError,IsOnLoad:IsOnLoad,IsRandom:1});
}#else{
	if(IsRandom!=null&&IsRandom)
		{
		var HttpRequest=new fastCSharp.Functions.HttpRequest;
		HttpRequest.Request({Method:'GET',Url:'__AJAX__?__AJAXCALL__='+Name+'&__CALLBACK__='+HttpRequest.SysObject.Escape()+'.CallBack',CallBack:CallBack,Send:Send,OnError:OnError,IsOnLoad:IsOnLoad,IsRandom:1});
		}
	else	fastCSharp.AjaxGetRequest.Request({Method:'GET',Url:'__AJAX__?__AJAXCALL__='+Name+'&__CALLBACK__=fastCSharp.AjaxGetRequest.CallBack',CallBack:CallBack,Send:Send,OnError:OnError,IsOnLoad:IsOnLoad,IsRandom:0});
}#endif
	};
fastCSharp.ClearViewObjects=function()
	{
	for(var Index=this.ViewObjects.length;Index;)
		{
		var SkinValue=this.ViewObjects[--Index];
		SkinValue.SkinTarget['@']=null;
		}
	this.ViewObjects.length=0;
	};
fastCSharp.ToViewObject=function(SkinValue)
	{
	var Target=SkinValue.SkinTarget;
	if(Target&&(Target=Target.SkinValue))
		{
		if(!Target['@'])	Target['@']={};
		if(Target=Target['@'])
			{
			(Target[SkinValue.SkinName]||(Target[SkinValue.SkinName]=[])).push(SkinValue);
			SkinValue.ViewIdentity=fastCSharp.ViewObjects.length;
			fastCSharp.ViewObjects.push(SkinValue);
			}
		}
	};
fastCSharp.CheckViewTarget=function(SkinValue)
	{
	var Target=SkinValue.SkinTarget;
	if(Target&&(Target=Target.SkinValue)&&(Target=Target['@']))
		{
		var Skins=Target[SkinValue.SkinName]||(Target[SkinValue.SkinName]=[]);
		if(Skins.IndexOf(SkinValue)==-1)	Skins.push(SkinValue);
		}
	};
//模板视图值
var SkinValue=function(Target,Name)
	{
	if(arguments.length>1)
		{
		this.SkinTarget=Target;
		this.SkinName=Name;
		this.Reset();
		this.SkinTarget[Name]=this;
		}
	else	this.SkinValue=Target;
	this.SkinValues={};
	this.SkinMarks=[];
	this.SkinSons={};
	fastCSharp.ToViewObject(this);
	};
SkinValue.prototype={
toString:function()
	{
	return this.SkinValue!=null?this.SkinValue.toString():'';
	},
Reset:function()
	{
	this.SkinValue=this.SkinTarget.SkinValue;
	if(this.SkinValue!=null)
		{
		if(typeof(this.SkinValue=this.SkinValue[this.SkinName])=='function')	this.SkinValue=this.SkinValue.apply(this.SkinTarget.SkinValue);
		}
	},
//设置对象值并重新渲染相关UI
SetThis:function(Value)
	{
	if(arguments.length)
		{
		this.SkinValue=Value;
		if(this.SkinTarget)
			{
			if(typeof(this.SkinTarget.SkinValue[this.SkinName])=='function')	this.SkinTarget.SkinValue[this.SkinName](Value);
			else	this.SkinTarget.SkinValue[this.SkinName]=Value;
			}
		}
	this.SkinValues={};
	for(var Marks=this.SkinMarks,Index=-1;++Index-Marks.length;)
		{
		if(fastCSharp.$Id(Marks[Index].Skin.GetMarkId(Marks[Index].Son.Mark)).Count())
			{
			for(var Son=Marks[Index].Son.Parent;Son;Son=Son.Parent)
				{
				if(Son.Type=='NoMark')
					{
					Marks[Index].Son=Son;
					break;
					}
				else if(this.SkinSons[Son.Mark])
					{
					this.SkinSons[Marks[Index].Son.Mark]=this.SkinMarks=null;
					break;
					}
				}
			}
		else	this.SkinSons[Marks[Index].Son.Mark]=this.SkinMarks=null;
		}
	if(!this.SkinMarks)
		{
		for(this.SkinMarks=[],Index=-1;++Index-Marks.length;)
			{
			if(this.SkinSons[Marks[Index].Son.Mark])	this.SkinMarks.push(Marks[Index]);
			}
		}
	if(this.SkinMarks.length)
		{
//		fastCSharp.ToViewObject(this);
		for(Marks=this.SkinMarks.Copy(),Index=-1;++Index-Marks.length;)
			{
			var Html={IsRound:1,Boot:Marks[Index].Skin,Html:[]},Son=Marks[Index].Son,MarkStart=fastCSharp.$Id(Html.Boot.GetMarkId(Son.Mark,1));
			if(MarkStart.Count())
				{
				var MarkEndId=Html.Boot.GetMarkId(Son.Mark),Parent=MarkStart.Parent();
				for(var Element=MarkStart.Next();Element.Count()&&Element.Id()!=MarkEndId;Element=MarkStart.Next())	Element.Delete();
				fastCSharp.$Id(MarkEndId).Delete();
				var MarkEnd=MarkStart.Next();
				MarkStart.Delete();
				if(Son.Type)	Son['Get'+Son.Type](Son,this.CreateValues(Son.MarkValues),Html,Son.Names);
				else if(Son.SkinSons)
					{
					Html.Boot.PushMark(Html,Son,1);
					Son.CreateHtml(this.CreateValues(Son.MarkValues),Html);
					Html.Boot.PushMark(Html,Son);
					}
				else	Html.Boot.GetHtml(Son,this.CreateValues(Son.MarkValues),Html);
				fastCSharp.DeleteElements.Html(Skin.FormatHtml(Html.Html.join(''))).Child().InsertBefore(MarkEnd,Parent);
				}
			}
		fastCSharp.DeleteElements.Html('');
		Marks[0].Skin.OnSet();
		}
	},
Set:function(Value)
	{
	var Target=this.SkinTarget;
	if(Target&&(Target=Target.SkinValue)&&(Target=Target['@']))
		{
		var Skins=Target[this.SkinName];
		if(Skins)
			{
			Target[this.SkinName]=null;
			if(arguments.length)	for(var Index=Skins.length;Index;Skins[--Index].SetThis(Value));
			else	for(var Index=Skins.length;Index;Skins[--Index].SetThis());
			return;
			}
		}
	if(arguments.length)	this.SetThis(Value);
	else	this.SetThis();
	},
CreateValues:function(Values)
	{
	for(var NewValues=[Values[0]],Index=1;Index-Values.length;++Index)
		{
		for(var Names=Values[Index].Names,NameIndex=0;NameIndex-Names.length;++NameIndex)
			{
			var Name=Names[NameIndex];
			NewValues.push(typeof(Name)=='number'?{Names:[Name],Value:Values[Index-1].Value.Get(Name,1),IsSkinValue:Values[Index].IsSkinValue}:Skin.GetValue(NewValues,Name,0,1));
			}
		}
	return NewValues;
	},
Not:function()
	{
	this.Set(!this.SkinValue);
	},
Add:function(Value)
	{
	this.Set(this.SkinValue+Value);
	},
Push:function(Value)
	{
	this.SkinValue.push(Value);
	this.Set();
	},
GetArray:function()
	{
	if(this.SkinValue)
		{
		for(var Values=[],Index=0;Index-this.SkinValue.length;Values.push(this[Index++]));
		return Values;
		}
	},
Copy:function(Value)
	{
	fastCSharp.Copy(this.SkinValue,Value);
	this.Set();
	},
Get:function(Name,IsValue)
	{
	if(Name=='[@]')	return new SkinValue(this.ViewIdentity);
	if(IsValue&&(this.SkinValue==null||typeof(this.SkinValue[Name])=='undefined'))	return null;
	var Value=this.SkinValues[Name];
	if(Value)
		{
		if(Value.SkinValue!=(this.SkinValue==null?null:this.SkinValue[Name]))	Value.Reset();
		fastCSharp.CheckViewTarget(Value);
		return Value;
		}
	return this.SkinValues[Name]=new SkinValue(this,Name);
//	return Value&&Value.SkinValue==(this.SkinValue==null?null:this.SkinValue[Name])?Value:(this.SkinValues[Name]=new SkinValue(this,Name));
	
	},
//创建UI关联标识
Mark:function(Values,Skin,Son)
	{
	if(!Son.Mark)
		{
		Son.Mark=Skin.NoMark||++Skin.NextMark;
		Son.MarkValues=Values.Copy();
		}
	if(!this.SkinSons[Son.Mark])
		{
		var Mark={Skin:Skin,Son:Son};
		this.SkinMarks.push(this.SkinSons[Son.Mark]=Mark);
		if(this.SkinTarget)	this.SkinTarget.Mark(Values,Skin,Son);
		}
	}
		};
//视图模板
var Skin=function()
	{
	this.GetArgument(arguments[0],{Id:null,Html:null,OnShowed:null,OnSet:null,Parent:null,Names:null,Type:null,IsMark:true,IsClient:null},1);
	if(this.Html==null)	this.Html=(this.Id?this.$Id(this.Id):this.$(document.body)).Html();
	this.Create();
	this.OnShowed=fastCSharp.Event(this,this.IsShowed,this).Add(this.OnShowed);
	this.OnSet=fastCSharp.Event().Add(this.OnSet);
	this.Mark=this.NextMark=this.NoMark=0;
	this.MarkId=this.GetId('mark');
//	if(this.Id)	this.Hide();
	};
Skin.ViewIdentity=0;
(fastCSharp.Functions.Skin=Skin).Inherit(fastCSharp.BaseFunction,{
//清除UI关联标识
ClearMark:function()
	{
	this.Mark=0;
	for(var Index=-1;++Index!=this.SkinSons.length;)
		{
		if(this.SkinSons[Index].ClearMark)	this.SkinSons[Index].ClearMark();
		else	this.SkinSons[Index].Mark=0;
		}
	},
//HTML模板解析
Create:function()
	{
	this.SkinSons=[];
	for(var EndIndex,StartIndex=0,Index=this.Html.indexOf('<!--');Index!=-1;Index=this.Html.indexOf('<!--',EndIndex))
		{
		var Type=this.Html.substring(Index+4,EndIndex=this.Html.indexOf('-->',Index)).split(':');
		if(Type.length<=2&&typeof(this['Get'+Type[0]])=='function')
			{
			var Html=this.Html.substring(StartIndex,Index);
			this.CreateValue(Html);
			StartIndex=this.Html.indexOf(this.Html.substring(Index,EndIndex+=3),EndIndex);
			if(StartIndex+1)
				{
				var Names=null;
				if(Type.length==2)
					{
					Names=Type[1].split('|');
					for(var NameIndex=Names.length;NameIndex;)
						{
						var Name=Names[--NameIndex],ClientIndex=Name.indexOf('#')+1;
						if(ClientIndex)	Name=Name.substring(ClientIndex);
						Names[NameIndex]=Skin.GetName(Name,ClientIndex);
						}
					}
				this.SkinSons.push(new fastCSharp.Functions.Skin({Html:this.Html.substring(EndIndex,StartIndex),Parent:this,IsClient:Html.substring(Html.length-13).toLowerCase()=='<!--client-->',Type:Type[0],Names:Names}));
				EndIndex=(StartIndex+=EndIndex-Index);
				}
			else	{
				fastCSharp.Ajax(null,'__PUBERROR__',{error:this.Error=navigator.appName+' : '+navigator.appVersion+'\r\nSkin解析失败: '+document.location.toString()+'\r\n'+this.Html.substring(0,256)},null,null,0);
				break;
				}
			}
		}
	this.CreateValue(this.Html.substring(StartIndex));
	},
//HTML模板视图值解析
CreateValue:function(Html)
	{
	Htmls=Html.split('=@');
	this.SkinSons.push({Parent:this,Html:Htmls[0]});
	for(var Index=0;++Index!=Htmls.length;)
		{
		if(Html=Htmls[Index])
			{
			var EscapeChar=Html.charAt(0);
			if(EscapeChar=='[')
				{
				if(Html.charAt(1)==']')
					{
					this.SkinSons.push({Parent:this,Names:[Skin.GetName('[]')]});
					if(Html.length>2)	this.SkinSons.push({Parent:this,Html:Html.substring(2)});
					}
				else if(Html.charAt(1)=='@'&&Html.charAt(2)==']')
					{
					this.SkinSons.push({Parent:this,Names:[Skin.GetName('[@]')]});
					if(Html.length>3)	this.SkinSons.push({Parent:this,Html:Html.substring(3)});
					}
				else	this.SkinSons.push({Parent:this,Html:'=@'+Html});
				}
			else	{
				var IsHtml=EscapeChar=='@',IsTextArea=EscapeChar=='*',CharIndex=-1;
				if(IsHtml||IsTextArea)	Html=Html.substring(1);
				while(++CharIndex!=Html.length&&Skin.ValueMap[Html.charCodeAt(CharIndex)]);
				var Name=Html.substring(0,CharIndex),ClientName='';
				if(Html.charAt(CharIndex)=='#')
					{
					for(var ClientIndex=CharIndex;++CharIndex!=Html.length&&Skin.ValueMap[Html.charCodeAt(CharIndex)];);
					if(++ClientIndex==CharIndex)	--CharIndex;
					else	ClientName=Html.substring(ClientIndex,CharIndex);
					}
				this.SkinSons.push({Parent:this,Names:[Skin.GetName(ClientName||Name,ClientName)],IsHtml:IsHtml,IsTextArea:IsTextArea});
				if(CharIndex-Html.length)
					{
					if(Html.charAt(CharIndex)=='$')	++CharIndex;
					if(CharIndex-Html.length)	this.SkinSons.push({Parent:this,Html:Html.substring(CharIndex)});
					}
				}
			}
		else	this.SkinSons.push({Parent:this,Html:'=@'});
		}
	},
//获取UI关联标识
GetMarkId:function(Mark,IsStart)
	{
	return this.MarkId+'_'+(IsStart?'s':'e')+Mark;
	},
//添加UI关联标识
PushMark:function(Html,Son,IsStart)
	{
	Html.Html.push('<span id="'+Html.Boot.GetMarkId(Son.Mark,IsStart)+'" style="display:none"></span>');
	},
//添加HTML代码
PushHtml:function(Html,Value)
	{
	var IsRound=(Value=Value.toString()).lastIndexOf('>')-Value.lastIndexOf('<');
	if(IsRound)	Html.IsRound=IsRound>0;
	if(Value)	Html.Html.push(Value);
	},
//生成目标HTML代码
CreateHtml:function(Values,Html,Row)
	{
	if(Html==null)
		{
		Html={IsRound:1,Boot:this,Html:[]};
		if(this.IsMark&&!this.NoMark)
			{
			Values[0].Value.Mark(Values,Html.Boot,this);
			this.PushMark(Html,this,1);
			}
		}
	for(var Index=-1;++Index!=this.SkinSons.length;)
		{
		var Son=this.SkinSons[Index];
		if(Son.Type)
			{
			if(Son.Names)
				{
				for(var IsValue=0,NameIndex=0;NameIndex-Son.Names.length;++NameIndex)
					{
					var Value=Skin.GetValue(Values,Son.Names[NameIndex],Son.IsClient,Html.Boot.IsMark&&!Html.Boot.NoMark);
					Values.push(Value);
					if(Value)	IsValue=1;
					}
				if(IsValue)	this['Get'+Son.Type](Son,Values,Html,Son.Names);
				for(var NameIndex=Son.Names.length;NameIndex;--NameIndex)	Values.pop();
				}
			else	this['Get'+Son.Type](Son,Values,Html);
			}
		else if(Son.Html!=null)	this.PushHtml(Html,Son.Html);
		else	this.GetHtml(Son,Values,Html);
		}
	if(arguments.length==1)
		{
		if(this.IsMark&&!this.NoMark)	this.PushMark(Html,this);
		return Skin.FormatHtml(Html.Html.join(''));
		}
	},
GetHtmlString:function(Son,Value)
	{
	Value=Value==null?'':Value.toString();
	return Son.IsHtml?Value.ToHTML():(Son.IsTextArea?Value.ToTextArea():Value);
	},
//获取HTML代码
GetHtml:function(Son,Values,Html)
	{
	var Value=Skin.GetValue(Values,Son.Names[0],0,Html.Boot.IsMark&&!Html.Boot.NoMark);
	if(Value==null)
		{
		if(Values[0].IsSkinValue)	Html.Html.push(Son.Names[0].Name);
		}
	else if(Value.IsSkinValue)
		{
		HtmlValue=this.GetHtmlString(Son,Value.Value.SkinValue);
		if(Html.Boot.IsMark&&!Html.Boot.NoMark)
			{
			var IsRound=Son.IsHtml||Son.IsTextArea?0:(HtmlValue.lastIndexOf('>')-HtmlValue.lastIndexOf('<')),IsMark=Html.IsRound&&IsRound>=0;
			Value.Value.Mark(Values,Html.Boot,Son=IsMark?Son:this);
			if(IsMark)	this.PushMark(Html,Son,1);
			if(HtmlValue)	Html.Html.push(HtmlValue);
			if(IsMark)	this.PushMark(Html,Son);
			}
		else if(HtmlValue)	Html.Html.push(HtmlValue);
		}
	else if(Value=this.GetHtmlString(Son,Value.Value))	Html.Html.push(Value);
	},
//获取循环节点HTML代码
GetLoop:function(Son,Values,Html)
	{
	var LoopValue=Values[Values.length-1];
	if(LoopValue.IsSkinValue)
		{
		if(LoopValue=LoopValue.Value)
			{
			if(Html.Boot.IsMark&&!Html.Boot.NoMark)
				{
				var LoopSon=Skin.Copy(Son,this);
				LoopValue.Mark(Values,Html.Boot,LoopSon);
				this.PushMark(Html,LoopSon,1);
				if(LoopValue.SkinValue)
					{
					var StartIndex=LoopValue.SkinValue.loopIndex==null?0:LoopValue.SkinValue.loopIndex,Count=LoopValue.SkinValue.loopCount?LoopValue.SkinValue.loopCount:LoopValue.SkinValue.length;
					for(var Index=StartIndex,EndIndex=Math.min(LoopValue.SkinValue.length,Index+Count);Index<EndIndex;++Index)
						{
						var NextValue=LoopValue.Get(Index,1),NextSon=Skin.Copy(Son,LoopSon);
						Values.push({Names:[Index],Value:NextValue,IsSkinValue:1});
						if(NextValue.SkinValue!=null)	NextValue.SkinValue['[]']=++Skin.ViewIdentity;
						NextSon.Type='Value';
						NextValue.Mark(Values,Html.Boot,NextSon);
						this.PushMark(Html,NextSon,1);
						NextSon.CreateHtml(Values,Html,Index-StartIndex);
						this.PushMark(Html,NextSon);
						Values.pop();
						}
					}
				this.PushMark(Html,LoopSon);
				return;
				}
			LoopValue=LoopValue.SkinValue;
			}
		}
	else	LoopValue=LoopValue.Value;
	if(LoopValue)
		{
		var StartIndex=LoopValue.loopIndex==null?0:LoopValue.loopIndex,Count=LoopValue.loopCount?LoopValue.loopCount:LoopValue.length;
		(Son=Skin.Copy(Son,this)).Type='Value';
		for(var Index=StartIndex,EndIndex=Math.min(LoopValue.length,Index+Count);Index<EndIndex;++Index)
			{
			var Value=LoopValue[Index];
			if(Value!=null)	Value['[]']=++Skin.ViewIdentity;
			Values.push({Names:[Index],Value:Value});
			Son.CreateHtml(Values,Html,Index-StartIndex);
			Values.pop();
			}
		}
	},
//获取子节点HTML代码
GetValue:function(Son,Values,Html)
	{
	var Value=Values[Values.length-1];
	if(Value.IsSkinValue)
		{
		if(Value.Value)
			{
			if(Html.Boot.IsMark&&!Html.Boot.NoMark)
				{
				Value.Value.Mark(Values,Html.Boot,Son);
				this.PushMark(Html,Son,1);
				}
			if(Value.Value.SkinValue)	Son.CreateHtml(Values,Html);
			if(Html.Boot.IsMark&&!Html.Boot.NoMark)	this.PushMark(Html,Son);
			}
		}
	else if(Value.Value)	Son.CreateHtml(Values,Html);
	},
//获取判断HTML代码
GetIf:function(Son,Values,Html,Names)
	{
	var IsMark=this.MarkOr(Son,Values,Html,Names);
	if(this.CheckOr(Values,Names))	this.GetOr(Son,Values,Html,Names);
	if(IsMark)	this.PushMark(Html,Son);
	},
//获取判断HTML代码
GetNot:function(Son,Values,Html,Names)
	{
	var IsMark=this.MarkOr(Son,Values,Html,Names);
	if(!this.CheckOr(Values,Names))	this.GetOr(Son,Values,Html,Names);
	if(IsMark)	this.PushMark(Html,Son);
	},
MarkOr:function(Son,Values,Html,Names)
	{
	if(Html.Boot.IsMark&&!Html.Boot.NoMark)
		{
		for(var Index=Values.length-Names.length;Index-Values.length;++Index)
			{
			var Value=Values[Index];
			if(Value&&Value.IsSkinValue)
				{
				Value.Value.Mark(Values,Html.Boot,Son);
				this.PushMark(Html,Son,1);
				return 1;
				}
			}
		}
	},
GetOr:function(Son,Values,Html,Names)
	{
	var PopValues=Values.slice(Values.length-Names.length,Values.length);
	Values.length-=Names.length;
	Son.CreateHtml(Values,Html);
	for(var Index=0;Index-PopValues.length;Values.push(PopValues[Index++]));
	},
CheckOr:function(Values,Names)
	{
	for(var Index=Values.length,NameIndex=Names.length;NameIndex;)
		{
		var Value=Values[--Index];
		if(Value)
			{
			var Name=Names[--NameIndex]
			if(Value.IsSkinValue?(Name.Value?Value.Value.SkinValue==Name.Value:Value.Value.SkinValue):(Name.Value?Value.Value==Name.Value:Value.Value))	return 1;
			}
		else	--NameIndex;
		}
	},
//获取禁用UI关联标识HTML代码
GetNoMark:function(Son,Values,Html)
	{
	var Value=Values[Values.length-1];
	if(Value.IsSkinValue)
		{
		if((Value=Value.Value)&&Value.SkinValue)
			{
			var NoMark=Html.Boot.NoMark;
			if(Html.Boot.IsMark&&!NoMark)
				{
				Value.Mark(Values,Html.Boot,Son);
				this.PushMark(Html,Son,1);
				}
			Html.Boot.NoMark=Son.Mark;
			Son.CreateHtml(Values,Html);
			Html.Boot.NoMark=NoMark;
			if(Html.Boot.IsMark&&!NoMark)	this.PushMark(Html,Son);
			}
		}
	else	Son.CreateHtml(Values,Html);
	},
//隐藏当前模板HTML容器
Hide:function()
	{
	(this.Id?this.$Id(this.Id):this.$(document.body)).Display(0);
	},
//生成目标HTML代码
ToHtml:function(Value)
	{
	var IsMark=this.IsMark;
	this.IsMark=0;
	var Html=this.CreateHtml([{Value:Value}]);
	this.IsMark=IsMark;
	return Html;
	},
SetHtml:function(Value)
	{
	this.ClearMark();
	(this.Id?this.$Id(this.Id):this.$(document.body)).Html(this.CreateHtml([{Value:this.SkinValue=this.IsMark?new SkinValue(Value):Value,IsSkinValue:this.IsMark}]));
	this.OnSet(Value);
	},
//生成目标HTML代码并渲染UI
Show:function(Value,IsOnLoad)
	{
	if(IsOnLoad)	fastCSharp.OnLoad(fastCSharp.ThisFunction(this,this.Show,[Value]),null,1);
	else if(Value)
		{
		this.ClearMark();
		(this.Id?this.$Id(this.Id):this.$(document.body)).Html(this.CreateHtml([{Value:this.SkinValue=this.IsMark?new SkinValue(Value):Value,IsSkinValue:this.IsMark}])).Display(1);
		this.OnShowed(Value);
		this.OnSet(Value);
		}
	else	(this.Id?this.$Id(this.Id):this.$(document.body)).Display(1);
	},
IsShowed:function()
	{
	return this.SkinValue;
	}
		});
//合法名称解析字符位图
Skin.ValueMap=(function()
	{
	var Map=new Array(0x7b);
	for(var Index=0x30;Index!=0x3a;Map[Index++]=true);
	for(var Index=0x41;Index!=0x5b;Map[Index++]=true);
	for(var Index=0x61;Index!=0x7b;Map[Index++]=true);
	Map[0x2e]=1;
	Map[0x5f]=1;
	return Map;
	})();
//名称格式化
Skin.GetName=function(Name,IsClient)
	{
	var ValueIndex=Name.indexOf('=')+1,Value=ValueIndex?Name.substring(ValueIndex):null;
	if(Value)	Name=Name.substring(0,--ValueIndex);
	for(var Count=0;Count-Name.length&&Name.charCodeAt(Count)==46;++Count);
	return Count-Name.length?{Count:Count,Name:Name,Value:Value,Names:Name.substring(Count).split('.'),IsClient:IsClient}:{Count:Count,Name:Name,Value:Value,Names:[],IsClient:IsClient};
	};
//HTML兼容性格式化
Skin.FormatHtml=function(Html)
	{
	Html=Html.replace(/ @(src|style)=/gi,' $1=').replace(/ src="=@[^"]+"/gi,'')
		.replace(/select@/gi,'select')
		.replace(/ @check="true"/g,' checked="checked"');
	return location.protocol=='https:'?Html.replace(/http\:\/\/__IMAGEDOMAIN__\//g,'https://__IMAGEDOMAIN__/'):Html;
//.replace(/<(select|option)view /gi,'<$1 ').replace(/<\/(select|option)view>/gi,'</$1>')
//		.replace(/ @display="(()|(false)|(\!true)|(0)|(\![1-9]\d*))"/g,' style="display:none"')
//		.replace(/ @checked="((true)|(\!false)|([1-9]\d*)|(\!0))"/g,' checked="true"');
	};
Skin.TryGetFunctionValue=function(Value,Target)
	{
	return typeof(Value)=='function'?Value.apply(Target):Value;
	};
//获取视图对象
Skin.GetValue=function(Values,Name,IsClient,IsMark)
	{
	var Index=Values.length-Name.Count-1,Names=Name.Names;
	if(Index<0)	Index=0;
	if(Names.length)
		{
		for(var ClientIndex=Index;Index>=0;--Index)
			{
			var Value=Values[Index];
			if(Value.IsSkinValue)
				{
				if(Value=Value.Value.Get(Names[0],1))
					{
					for(Index=0;++Index!=Names.length&&Value;Value=Value.Get(Names[Index],0));
					return {Names:[Name],Value:IsMark?Value:Value.SkinValue,IsSkinValue:IsMark};
					}
				}
			else if(Value=Value.Value)
				{
				var Target=Value;
				if(typeof(Value=Value[Names[0]])!='undefined')
					{
					for(Index=0;++Index!=Names.length&&Value;Value=Value[Names[Index]])	Target=Value;
					return {Names:[Name],Value:this.TryGetFunctionValue(Value,Target)};
					}
				}
			}
		if((IsClient||Name.IsClient)&&Names.length==1)
			{
			var Target=(Value=Values[ClientIndex]).Value,ClientName=Names[0];
			if(typeof(Target[ClientName])=='undefined')	Target[ClientName]=null;
			if(Value.IsSkinValue)
				{
				if(IsMark)	return {Names:[Name],Value:Target.Get(ClientName,0),IsSkinValue:1};
				Target=Target.SkinValue;
				}
			return {Names:[Name],Value:this.TryGetFunctionValue(Target[ClientName],Target)};
			}
		return null;
		}
	var Value=Values[Index];
	if(Value.IsSkinValue)	return {Names:[Name],Value:IsMark?Value.Value:Value.Value.SkinValue,IsSkinValue:IsMark};
	return {Names:[Name],Value:this.TryGetFunctionValue(Value.Value)};
	};
//视图对象复制名称集合
Skin.CopyName=('Html,Type,Names,IsHtml,IsTextArea,IsClient').split(',');
//视图对象复制
Skin.Copy=function(CopyValue,Parent)
	{
	var Value;
	if(CopyValue.SkinSons)
		{
		Value=new fastCSharp.Functions.Skin({Html:'',Parent:Parent});
		for(var Index=0;Index-CopyValue.SkinSons.length;Value.SkinSons.push(this.Copy(CopyValue.SkinSons[Index++],Value)));
		}
	else	Value={Parent:Parent};
	for(var Index=Skin.CopyName.length;--Index>=0;Value[Skin.CopyName[Index]]=CopyValue[Skin.CopyName[Index]]);
	return Value;
	};
//js文件动态加载器
fastCSharp.LoadJs=function(Element,OnLoad,OnError)
	{
	this.OnLoad=OnLoad;
	this.OnError=OnError;
	this.LoadFunction=fastCSharp.ThisFunction(this,this.OnLoadJs);
	this.ErrorFunction=fastCSharp.ThisFunction(this,this.OnErrorJs);
//	Element.crossOrigin='anonymous';
	(this.Element=Element).onload=this.LoadFunction;
	Element.onerror=this.ErrorFunction;
	fastCSharp.$Head.appendChild(Element);
	};
fastCSharp.LoadJs.prototype={
//Close:function()
//	{
//	if(this.LoadFunction.Close)	this.LoadFunction.Close();
//	if(this.ErrorFunction.Close)	this.ErrorFunction.Close();
//	fastCSharp.$Head.removeChild(this.Element);
//	},
//js文件加载后的处理
OnLoadJs:function()
	{
	if(this.OnLoad)	this.OnLoad();
	fastCSharp.$Head.removeChild(this.Element);
//	this.Close();
	},
OnErrorJs:function()
	{
	if(this.OnError)	this.OnError();
	fastCSharp.$Head.removeChild(this.Element);
//	this.Close();
	}
		};
#if(IE){
fastCSharp.LoadJs.IsRandom=1;
}#else{
}#endif
//动态加载js文件
fastCSharp.AppendJs=function(Src,Charset,OnLoad,OnError)
	{
	var Node={language:'javascript',type:'text/javascript',src:Src,charset:Charset||this.Charset};
	if(OnLoad||OnError) new fastCSharp.LoadJs(this.CreateNode('script',Node),OnLoad,OnError);
	else	this.AppendHead('script',Node);
	},
//----常用类定义 结束----
//document加载完成检测与界面UI初始化
fastCSharp.ReadyState=function()
	{
	var View=this.PageView,IsLoad=document.body&&(document.readyState==null||document.readyState.toLowerCase()=='complete');
	if(IsLoad&&this.LoadComplete==null)
		{
		if(View.IsLoadView)	this.HeaderView=new this.Functions.Skin({IsMark:0,Html:(fastCSharp.GetViewTitle?fastCSharp.GetViewTitle():document.title).replace(/\=@@/g,'=@')});
		this.$('@body').To();
		this.Skins={};
		for(var Childs=this.$('@skin').GetElements(),Index=Childs.length;--Index>=0;this.Skins[Id]=new this.Functions.Skin({Id:Id}))
			{
			var Child=Childs[Index],Id=Child.id;
			if(!Id)	Child.id=Id=fastCSharp.$Attribute(Child,'skin');
			}
		if(View.IsLoadView)
			{
			var ViewOver=document.getElementById('fastCSharpViewOver');
			if(ViewOver)
				{
				var Display=ViewOver.style.display;
				ViewOver.style.display='none';
				View.ViewSkin=new this.Functions.Skin({OnShowed:View.OnShowed,OnSet:View.OnSet});
				ViewOver.style.display=Display||'';
				}
			else	View.ViewSkin=new this.Functions.Skin({OnShowed:View.OnShowed,OnSet:View.OnSet});
//			if(View.ViewSkin.Error)	location.reload(true);
			}
		this.LoadComplete=1;
		}
	if(IsLoad&&View.IsLoad)
		{
		if(View.IsLoadView)
			{
			if(View.LoadError)
				{
				View.IsLoad=View.IsLoadView=View.LoadError=0;
				var ViewOver=document.getElementById('fastCSharpViewOver');
				if(ViewOver)	ViewOver.innerHTML='错误：视图数据加载失败，稍后尝试重新加载';
				document.title='Server Error';
				setTimeout(this.ThisFunction(this,this.ReLoad),2000);
				return;
				}
			else	{
				this.ClearViewObjects();
				(this.PageView=View.ViewSkin).Show(View);
				this.ChangeHeader();
				}
			}
		else	{
			document.body.innerHTML=document.body.innerHTML.replace(/ @(src|style)=/gi,' $1=');
			var ViewOver=document.getElementById('fastCSharpViewOver');
			if(ViewOver)	document.body.removeChild(ViewOver);
			}
		this.DeleteElements=this.$Create('div').Css('padding,margin','10px').Css('border','10px solid red').Opacity(0).To();
		this.IsBorder=this.DeleteElements.XY().Left-10;
		this.DeleteElements.Css('padding,margin,border','0px');
		this.IsBorder-=this.DeleteElements.XY().Left;
		if(this.IsBorder==20)	this.IsPadding=1;
#if(IE){
		this.IsFixed=this.DeleteElements.Css('position','fixed').Css('left','50%').Left();
}#else{
		this.IsFixed=1;
}#endif
		this.DeleteElements.Display(0);
		this.LocationHash=this.$Hash();
		this.$(document.body).AddEvent('focus',this.$Focus=fastCSharp.Event());
#if(IE){
		setTimeout(this.CheckHashFunction=this.ThisFunction(this,this.CheckHash),100);
}#else{
		window.onhashchange=this.ThisEvent(this,this.CheckHash);
}#endif
		this.IsLoad=1;
		for(var Index=-1;++Index-this.OnLoads.length;this.OnLoads[Index]());
#if(IE){
//		this.ReadyFunction.Close();
}#else{
}#endif
		this.Load=this.OnLoads=this.ReadyState=this.ReadyFunction=null;
		}
	else	setTimeout(this.ReadyFunction,1);
	};
fastCSharp.ChangeHeader=function()
	{
	document.title=this.HeaderView.ToHtml(fastCSharp.PageView.SkinValue.SkinValue);
	};
fastCSharp.$OnQuery=fastCSharp.Event();
//URI的HASH修改检测处理
fastCSharp.CheckHash=function()
	{
	var Hash=this.$Hash();
	if(Hash.length&&Hash!=this.LocationHash)
		{
		this.LocationHash=Hash;
		this.Query=fastCSharp.CreateQuery(location);
		this.AjaxGet(this.ThisFunction(this,this.LoadHash),document.location.pathname,this.Query,null,1);
		fastCSharp.$OnQuery();
		}
#if(IE){
	setTimeout(this.CheckHashFunction,100);
}#else{
}#endif
	};
//HASH修改重置界面模板绑定视图对象
fastCSharp.LoadHash=function(PageView)
	{
	this.BeforeUnLoad();
	var SkinValue=this.PageView.SkinValue.SkinValue;
	for(var Name in PageView)
		{
		var Value=PageView[Name];
		if((!Value||!Value['__VIEWONLY__'])&&typeof(Value)!='function')	SkinValue[Name]=Value;
		}
	this.OnLoadHash(SkinValue);
	this.LoadView(SkinValue,1);
	this.ClearViewObjects();
	this.PageView.Show(SkinValue);
	this.ChangeHeader();
	this.OnLoadedHash();
	this.$ScrollTop(0);
	};
//视图数据加载完成
fastCSharp.LoadView=function(PageView,IsReView)
	{
	if(!PageView)	PageView={ErrorPath:'__VIEWLOCATION__'};
	if(PageView.ErrorPath)
		{
		var HashIndex=PageView.ErrorPath.indexOf('#');
		if(HashIndex+1)
			{
			var Hash=PageView.ErrorPath.substring(HashIndex);
			PageView.ErrorPath=PageView.ErrorPath.substring(0,HashIndex);
			location.replace(PageView.ErrorPath+(PageView.ErrorPath.indexOf('?')+1?'&':'?')+'url='+(PageView.ReturnPath||location.toString()).Escape()+Hash);
			}
		else	location.replace(PageView.ErrorPath+'#url='+(PageView.ReturnPath||location.toString()).Escape());
		}
	else if(PageView.LocationPath)	location.replace(PageView.LocationPath);
	PageView.Client=this.ClientView;
	if(!IsReView)
		{
		PageView.OnShowed=this.PageView.OnShowed;
		PageView.OnSet=this.PageView.OnSet;
		PageView.ViewSkin=this.PageView.ViewSkin;
		PageView.IsLoadView=PageView.IsLoad=PageView.IsView=1;
		this.PageView=PageView;
		}
	};
fastCSharp.BeforeUnLoad=function()
	{
	this.OnBeforeUnLoad();
	};
fastCSharp.ReLoad=function()
	{
	var ViewOver=document.getElementById('fastCSharpViewOver');
	if(ViewOver)	ViewOver.innerHTML='正在尝试重新加载视图数据...';
	this.AjaxGet(this.ThisFunction(this,this.LoadView),document.location.pathname,this.Query,this.ThisFunction(this,this.LoadError),0,1);
	setTimeout(this.ReadyFunction,1);
	};
//视图数据加载失败
fastCSharp.LoadError=function()
	{
	this.PageView.IsLoadView=this.PageView.IsLoad=this.PageView.IsView=this.PageView.LoadError=1;
	};
//初始化处理
fastCSharp.Load=function()
	{
	this.OnLoadHash=this.Event();
	this.OnLoadedHash=this.Event();
	this.OnBeforeUnLoad=this.Event();
	window.onbeforeunload=this.ThisFunction(this,this.BeforeUnLoad);
	this.Query=this.CreateQuery(self.location);
//	for(var Css=document.getElementsByTagName('css'),Index=Css.length;--Index>=0;)
//		{
//		this.AppendCss(Css[Index].href);
//		this.$Parent(Css[Index]).removeChild(Css[Index]);
//		}
	if(this.PageView)
		{
		this.PageView={IsLoadView:1,OnShowed:fastCSharp.Event(),OnSet:fastCSharp.Event()};
		this.AjaxGet(this.ThisFunction(this,this.LoadView),document.location.pathname,this.Query,this.ThisFunction(this,this.LoadError));
		}
	else	this.PageView={IsLoad:1};
	var Path=document.location.pathname,Index=Path.lastIndexOf('/'),EndIndex=Path.lastIndexOf('.');
	if(EndIndex>Index)	Path=Path.substring(0,EndIndex);
	if(Path.charAt(Path.length-1)=='/')	Path+='index';
	this.AppendJs(this.JsDomain+Path.substring(1)+'.js?v='+this.Version);
	setTimeout(this.ReadyFunction=this.ThisFunction(this,this.ReadyState),0);
	};
fastCSharp.Errors={};
#if(IE){
window.onerror=function(Message,Url,Line)
	{
	var Location=document.location.toString(),Error='IE => '+navigator.appName+' : '+navigator.appVersion+'\r\n'+Location+'\r\n'+Url+' ['+Line+'] '+Message;
	if(((Message!='语法错误'&&Message!='Syntax error'&&Message!='語法錯誤')||Location.substring(0,Url.length)!=Url)&&!fastCSharp.Errors[Error])
		{
		fastCSharp.Errors[Error]=Error;
		fastCSharp.AjaxGet(null,'__PUBERROR__',{error:Error});
		}
	};
}#else{
window.onerror=function(Message,Url,Line)
	{
	var Ajax=fastCSharp.AjaxAppendJs||{};
	if(Line==1&&Ajax.RetryCount&&document.location.origin+Ajax.Url==Url&&--Ajax.RetryCount)	fastCSharp.AppendJs(Ajax.Url,null,null,Ajax.OnError);
	else	{
		var Error='NS => '+navigator.appName+' : '+navigator.appVersion+'\r\n'+document.location.toString()+'\r\n'+Url+' ['+Line+'] '+Message;
		if((Line||Message!='Script error.')&&(Line!=1||Message!='Error loading script')&&!fastCSharp.Errors[Error])
			{
			fastCSharp.Errors[Error]=Error;
			fastCSharp.AjaxGet(null,'__PUBERROR__',{error:Error});
			}
		}
	};
}#endif
fastCSharp.Load();
})();