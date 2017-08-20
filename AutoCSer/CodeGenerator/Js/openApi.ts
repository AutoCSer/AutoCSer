module AutoCSer {
    interface IOpenApi {
        Url: string;
        IsState: boolean;
    }
    export class OpenApi {
        private StateAjaxCallName: string;
        private APIs: { [key: string]: IOpenApi };
        private IsLoginCookie: boolean;
        constructor(StateAjaxCallName ='pub.GetOpenApiState') {
            this.StateAjaxCallName = StateAjaxCallName;
            this.APIs = {};
        }
        private Login(Name: string) {
            var Api = this.APIs[Name];
            if (Api && Api.Url) {
                if (Api.IsState == null || Api.IsState) HttpRequest.Post(this.StateAjaxCallName, null, Pub.ThisFunction(this, this.OnState, [Api]));
                else location = Api.Url as Object as Location;
            }
            else AutoCSer.Pub.Alert('未找到第三方 ' + Name + ' API信息');
        }
        private OnState(Value: AutoCSer.IHttpRequestReturn, Api: IOpenApi) {
            if (Value = Value.__AJAXRETURN__) {
                if (this.IsLoginCookie == null || this.IsLoginCookie) Cookie.Default.Write({ Name: 'OpenLoginUrl', Value: location.toString() } as ICookieValue);
                location = Api.Url + '&state=' + Value as Object as Location;
            }
            else AutoCSer.HttpRequest.CheckError(Value);
        }
        private Location(Url) {
            var Value = Cookie.Default.Read('OpenLoginUrl');
            if (Value) Cookie.Default.Write({ Name: 'OpenLoginUrl' } as ICookieValue);
            location.replace(Value || Url);
        }
        static Default: OpenApi = new OpenApi();
    }
}