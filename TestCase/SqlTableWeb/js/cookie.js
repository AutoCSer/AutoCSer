//Cookie操作
(function(){
var Cookie=function()
	{
	this.GetArgument(arguments[0],{Expires:null,Path:'/',Domain:location.hostname,Secure:null});
	};
(fastCSharp.Functions.Cookie=Cookie).Inherit(fastCSharp.BaseFunction,{
Write:function(Value)
	{
	var Cookie=Value.Name.Escape()+'='+(Value.Value==null?'.':Value.Value.toString().Escape()),Expires=Value.Expires;
	if(Value.Value==null)	Expires=new Date;
	else if(!Expires&&this.Expires)	Expires=(new Date).AddMilliseconds(this.Expires);
	if(Expires)	Cookie+='; expires='+Expires.toGMTString();
	if(Value.Path)	Cookie+='; path='+Value.Path;
	else if(this.Path)	Cookie+='; path='+this.Path;
	if(Value.Domain)	Cookie+='; domain='+Value.Domain;
	else if(this.Domain)	Cookie+='; domain='+this.Domain;
	if(Value.Secure)	Cookie+='; secure='+Value.Secure;
	else if(this.Secure)	Cookie+='; secure='+this.Secure;
	document.cookie=Cookie;
	this.Name='';
	},
Read:function(Name,DefaultValue)
	{
	for(var Values=document.cookie.split('; '),Value=null,Index=Values.length;--Index>=0&&Value==null;)
		{
		var IndexOf=Values[Index].indexOf('=');
		if(unescape(Values[Index].substring(0,IndexOf))==Name)	Value=unescape(Values[Index].substring(IndexOf+1));
		}
	return Value||DefaultValue;
	},
ReadJson:function(Name,DefaultValue)
	{
	var Value=this.Read(Name,null);
	return Value?eval('('+Value+')'):DefaultValue;
	}
		});
fastCSharp.Cookie=new Cookie({Expires:24*3600*1000});
})();