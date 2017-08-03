@Type.Type.Name:function(Id)
	{/*LOOP:PathMembers*/
	this.@Member.MemberName='@Path/*IF:IsIdentity*//*IF:IsHash*/#!/*IF:IsHash*/@OtherQuery@QueryName=/*IF:IsIdentity*/'/*IF:IsIdentity*/+Id/*IF:IsIdentity*/;/*LOOP:PathMembers*/
	},
