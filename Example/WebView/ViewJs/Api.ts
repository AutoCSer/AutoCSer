//本文件由程序自动生成,请不要自行修改
module AutoCSerAPI.Ajax {
	export class RefOut {
		
		static Add(left:number,right:number,Callback = null) {
			AutoCSer.Pub.GetAjaxPost()('RefOut.Add',{left: left, right: right }, Callback);	
		}
		
	}
}