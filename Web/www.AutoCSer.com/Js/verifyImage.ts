//验证图片	<img verifyimage="{Width:80,Height:20,ButtonId:'YYY'}" id="XXX" />
module AutoCSer {
    export class VerifyImage implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, Width: null, Height: null, ButtonId: null };
        private static DefaultEvents = { OnClick: null };
        private Id: string;
        private Event: DeclareEvent;
        private Width: number;
        private Height: number;
        private ButtonId: string;

        private OnClick: Events;

        private Element: HTMLElement;
        constructor(Parameter) {
            Pub.GetParameter(this, VerifyImage.DefaultParameter, Parameter);
            Pub.GetEvents(this, VerifyImage.DefaultEvents, Parameter);
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                var Element = HtmlElement.$Id(this.Id), Image = Element.Element0();
                if (Image != this.Element) {
                    this.Element = Image;
                    Element.Set('alt', '验证码').Set('border', 0);
                    if (this.Width) Element.Set('width', this.Width);
                    if (this.Height) Element.Set('height', this.Height);
                    HtmlElement.$Id(this.ButtonId).Cursor('pointer').AddEvent('click', Pub.ThisFunction(this, this.ClickButton));
                    this.Show(false);
                }
            }
        }
        private ClickButton () {
            this.Show(true);
            this.OnClick.Function();
        }
        private Show(IsRefresh: boolean) {
            var Verify = IsRefresh ? null : Cookie.Default.Read('VerifyImage') as any;
            if (!Verify) Verify = (new Date).getTime();
            Cookie.Default.Write({ Name: 'VerifyImage', Value: Verify, Expires: (new Date).AddMinutes(20) });
            HtmlElement.$Id(this.Id).Set('src', '/verifyImage?t=' + Verify).Display(1);
        }
        private Clear() {
            Cookie.Default.Write({ Name: 'VerifyImage' });
        }
    }
    new AutoCSer.Declare(VerifyImage, 'VerifyImage', 'mouseover', 'AttributeName');
}