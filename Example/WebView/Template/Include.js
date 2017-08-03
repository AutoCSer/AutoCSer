AutoCSer.Pub.OnLoad(function () {
	AutoCSerAPI.Ajax.RefOut.Add(3, 5, function (Value) {
		AutoCSer.Skin.Skins['AjaxReturn'].Show(Value);
	});
});


 
