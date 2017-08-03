(function(){
var RTC=function(Value)
	{
	this.GetArgument(arguments[0],{Id:null,OnLoad:null,OnError:null,OnMetaData:null,IsVideo:true,IsAudio:false,IsAutoPlay:1,IsPeer:0,IsRemote:0,IceServer:null,OnPeerCandidate:null,OnPeerDescription:null,OnPeerChannelMessage:null},1);
	this.OnLoad=fastCSharp.Event().Add(this.OnLoad);
	this.OnError=fastCSharp.Event().Add(this.OnError);
	this.OnMetaData=fastCSharp.Event().Add(this.OnMetaData);
	this.OnPeerCandidate=fastCSharp.Event().Add(this.OnPeerCandidate);
	this.OnPeerDescription=fastCSharp.Event().Add(this.OnPeerDescription);
	this.OnPeerChannelMessage=fastCSharp.Event().Add(this.OnPeerChannelMessage);
	if(this.IsPeer&&RTC.PeerConnection)
		{
		this.Peer=new RTC.PeerConnection(IceServer||RTC.IceServer);
		this.Peer.onicecandidate=fastCSharp.ThisFunction(this,this.GetPeerCandidate);
		this.Peer.onaddstream=fastCSharp.ThisFunction(this,this.GetPeerStream);
//ordered: 数据通道是否保证按序传输数据
//maxRetrasmitTime：在信息失败前的最大重传时间（强迫进入不可靠模式）
//maxRetransmits：在信息失败前的最大重传次数（强迫进入不可靠模式）
//protocol：允许使用一个自协议，但如果协议不支持，将会失败
//negotiated：如果设为true，将一处对方的数据通道的自动设置，也就是说，将使用相同的id以自己配置的方式与对方建立数据通道
//id：为数据通道提供一个自己定义的ID
		this.PeerChannel=this.Peer.createDataChannel('name',{ordered:false,maxRetransmitTime:3000});
//		this.PeerChannel.onopen=null;
//		this.PeerChannel.onclose=null;
		this.PeerChannel.onmessage=fastCSharp.ThisFunction(this,this.GetPeerChannelMessage);
//		this.PeerChannel.onerror=null;
		}
	if(this.IsAutoPlay&&!this.IsRemote)	this.Play();
	};
if(RTC.GetUserMedia=navigator.getUserMedia||navigator.webkitGetUserMedia||navigator.mozGetUserMedia||navigator.msGetUserMedia)
	{
	RTC.GetUserMedia=fastCSharp.ThisFunction(navigator,RTC.GetUserMedia);
	RTC.PeerConnection=window.PeerConnection||window.webkitPeerConnection00||window.webkitRTCPeerConnection||window.mozRTCPeerConnection;
	RTC.URL=window.URL||window.webkitURL||window.msURL||window.oURL;
	RTC.IceServer={'iceServers':[{'url':'stun:stun.l.google.com:19302'}]};
	}
(fastCSharp.Functions.RTC=RTC).Inherit(fastCSharp.BaseFunction,{
Play:function()
	{
	if(RTC.GetUserMedia)
		{
		var Camera=fastCSharp.$Id(this.Id).GetElements()[0];
		if(this.Camera!=Camera)
			{
			this.Camera=Camera;
			Camera.onloadedmetadata=fastCSharp.ThisFunction(this,this.MetaData);
			}
//MinWidth: 视频流的最小宽度
//MaxWidth：视频流的最大宽度
//MinHeight：视频流的最小高度
//MaxHiehgt：视频流的最大高度
//MinAspectRatio：视频流的最小宽高比
//MaxAspectRatio：视频流的最大宽高比
//MinFramerate：视频流的最小帧速率
//MaxFramerate：视频流的最大帧速率
		RTC.GetUserMedia({video:this.IsVideo,audio:this.IsAudio},fastCSharp.ThisFunction(this,this.Load),fastCSharp.ThisFunction(this,this.LoadError));
		}
	else	this.LoadError();
	},
LoadError:function(Error)
	{
	this.OnError();
	},
MetaData:function()
	{
	this.OnMetaData();
	},
Load:function(Stream)
	{
	if(navigator.mozGetUserMedia)
		{
		this.Camera.mozSrcObject=Stream;
		this.Camera.play();
		}
	else	this.Camera.src=RTC.URL.createObjectURL(Stream);
	this.OnLoad();
	if(this.Peer)
		{
		this.Peer.addStream(Stream);
		if(this.IsRemote)	this.Peer.createAnswer(fastCSharp.ThisFunction(this,this.SetLocalDescription));
		else	this.Peer.createOffer(fastCSharp.ThisFunction(this,this.SetLocalDescription));
		}
	},
DataURLtoBlob:function(DataUrl)
	{
	var Array=DataUrl.split(','),String=atob(Array[1]),Code=new Uint8Array(String.length);
	for(var Size=String.length;Size--;Code[Size]=String.charCodeAt(Size));
	return new Blob([Code],{type:Array[0].match(/:(.*?);/)[1]});
	},
ToCanvasImageBlob:function(CanvasId,Left,Top,Width,Height)
	{
	var Canvas=fastCSharp.$Id(CanvasId).GetElements()[0];
	Canvas.getContext('2d').drawImage(this.Camera,Left,Top,Width,Height);
	return this.DataURLtoBlob(Canvas.toDataURL('image/png'));
	},
GetPeerCandidate:function(Value)
	{
	this.PeerCandidate=Value.candidate;
	this.OnPeerCandidate(this);
	},
GetPeerStream:function(Value)
	{
	this.Camera.src=RTC.URL.createObjectURL(Value.stream);
	},
SetLocalDescription:function(Description)
	{
	this.Peer.setLocalDescription(Description);
	this.OnPeerDescription(this);
	},
AddIceCandidate:function(Candidate)
	{
	this.Peer.addIceCandidate(new RTCIceCandidate(Candidate));
	},
SetRemoteDescription:function(Description)
	{
	this.Peer.setRemoteDescription(new RTCSessionDescription(Description));
	if(this.IsAutoPlay&&this.IsRemote)	this.Play();
	},
ClosePeerChannel:function()
	{
	this.PeerChannel.close();
	},
GetPeerChannelMessage:function(Message)
	{
	this.OnPeerChannelMessage(Message);
	},
SendPeerChannel:function(Value)
	{
	//支持string,Blob,ArrayBuffer,ArrayBufferView
	}
		});
})();