
AutoCSer.Pub.OnLoad(function () {
	var SkinData = AutoCSer.Skin.BodyData();
	SkinData.$Data.ClientData = SkinData.$Data.ServerData + 1;
	SkinData.$ReShow();
});
