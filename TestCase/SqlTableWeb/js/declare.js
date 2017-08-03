fastCSharp.Declare=function(Name,EventName,Type)
	{
	this.Type=Type;
	this.EventName=EventName;
	this.LowerName=(this.Name=Name).toLowerCase();
	this.fastCSharpName=Name+'s';
	fastCSharp['Get'+this.Name]=fastCSharp.ThisFunction(this,this.Get);
	fastCSharp.OnLoad(this.Load,this,1);
	};
fastCSharp.Declare.Identity=0;
fastCSharp.Declares=[];
fastCSharp.Declare.prototype={
Load:function()
	{
	fastCSharp[this.fastCSharpName]={};
	fastCSharp.$(document.body).AddEvent(this.EventName,fastCSharp.ThisEvent(this,this[this.Type]));
	},
Src:function(Event)
	{
	var Element=Event.srcElement,Parameter=fastCSharp.$Attribute(Element,this.LowerName);
	if(Parameter!=null)
		{
		var Id=Element.id;
		if(Id)	{
			var Values=fastCSharp[this.fastCSharpName];
			if(Values[Id])	Values[Id].Start(Event);
			else	{
				(Parameter=Parameter?eval('('+Parameter+')'):{}).Id=Id;
				Parameter.Event=Event;
				Values[Id]=new fastCSharp.Functions[this.Name](Parameter);
				}
			return Values[Id];
			}
		(Parameter=Parameter?eval('('+Parameter+')'):{}).Id=Element.id='fastCSharpDeclare'+(++fastCSharp.Declare.Identity);
		Parameter.Event=Event;
		return new fastCSharp.Functions[this.Name](Parameter);
		}
	return fastCSharp[this.fastCSharpName][Event.DeclareId];
	},
AttributeName:function(Event)
	{
	var Element=Event.ElementName(this.LowerName);
	if(Element)
		{
		var Id=Element.Id();
		if(Id)	{
			var Values=fastCSharp[this.fastCSharpName];
			if(Values[Id])	Values[Id].Start(Event);
			else	{
				var Parameter=Element.Get(this.LowerName);
				(Parameter=Parameter?eval('('+Parameter+')'):{}).Id=Id;
				Parameter.Event=Event;
				Values[Id]=new fastCSharp.Functions[this.Name](Parameter);
				}
			return Values[Id];
			}
		var Parameter=Element.Get(this.LowerName);
		Element.Id((Parameter=Parameter?eval('('+Parameter+')'):{}).Id='fastCSharpDeclare'+(++fastCSharp.Declare.Identity));
		Parameter.Event=Event;
		return new fastCSharp.Functions[this.Name](Parameter);
		}
	return fastCSharp[this.fastCSharpName][Event.DeclareId];
	},
ParameterId:function(Event)
	{
	var Element=Event.ElementName(this.LowerName);
	if(Element)
		{
		var Parameter=eval('('+Element.Get(this.LowerName)+')');
		if(Parameter.Id)
			{
			Element=fastCSharp.$Id(Parameter.Id);
			if(!Element.Count())	return fastCSharp[this.fastCSharpName][Parameter.Id];
			Parameter=null;
			}
		var Id=Element.Id(),Values=fastCSharp[this.fastCSharpName];
		if(!Id)	Element.Id(Id='fastCSharpDeclare'+(++fastCSharp.Declare.Identity));
		if(Values[Id])	Values[Id].Start(Event);
		else	{
			if(Parameter==null)	Parameter=eval('('+Element.Get(this.LowerName)+')');
			Parameter.Event=Event;
			Values[Parameter.Id=Id]=new fastCSharp.Functions[this.Name](Parameter);
			}
		return Values[Id];
		}
	return fastCSharp[this.fastCSharpName][Event.DeclareId];
	},
Get:function(Id)
	{
	var Element=fastCSharp.$Id(Id).Element();
	return this[this.Type](new fastCSharp.Functions.BrowserEvent({DeclareId:Id,srcElement:Element,target:Element,IsGetOnly:1}));
	}
		};