/// <reference path = "./overDiv.ts" />
/*Include:Js\overDiv*/
//界面居中弹出层	<div body="true" floatcenter="{}" id="XXX" style="display:none"></div>
//AutoCSer.FloatCenter.FloatCenters.XXX.Show();
module AutoCSer {
    export class FloatCenter {
        private static DefaultParameter = { Id: null, IsFixed: false, IsEsc: true, Timeout: 200, SkinView: null, ZIndex: 0, AutoClass: null };
        private Id: string;
        private Timeout: number;
        private ZIndex: number;
        private AutoClass: string;
        private SkinView: Object;
        private IsFixed: boolean;
        private IsEsc: boolean;

        private Skin: Skin;
        private KeyDownFunction: (Event: Event) => boolean;
        private ShowFunction: Function;
        private ShowInterval: number;
        constructor(Parameter) {
            Pub.GetParameter(this, FloatCenter.DefaultParameter, Parameter);
            this.KeyDownFunction = Pub.ThisEvent(this, this.KeyDown);
            if (this.SkinView) this.Skin = Skin.Skins[this.Id] || new Skin(this.Id);
            if (Pub.IsFixed) {
                if (!this.AutoClass) HtmlElement.$Id(this.Id).Style('position', 'fixed').Style('top,left', '50%').Style('transform', 'translateX(-50%) translateY(-50%)');
            }
            else {
                HtmlElement.$Id(this.Id).Style('position', 'absolute');
                this.ShowFunction = Pub.ThisFunction(this, this.Show);
                this.Hide();
            }
        }
        Show() {
            var Element = HtmlElement.$Id(this.Id);
            if (Pub.IsFixed) {
                OverDiv.Default.Show(this.Id, this.ZIndex || !this.AutoClass ? HtmlElement.ZIndex + this.ZIndex : 0);
                Element.Style('zIndex', HtmlElement.ZIndex + this.ZIndex);
                if (this.AutoClass) {
                    if (this.Skin) this.Skin.SetHtml(this.SkinView);
                    Element.AddClass(this.AutoClass);
                }
                else {
                    if (this.Skin) this.Skin.Show(this.SkinView);
                    else Element.Display(1);
                }
            }
            else {
                OverDiv.Default.Show();
                if (this.Skin) this.Skin.Show(this.SkinView);
                else Element.Display(1);
                var Left = (HtmlElement.$GetScrollLeft() + (HtmlElement.$Width() - HtmlElement.$Width(Element.Element0())) / 2), Top = (HtmlElement.$GetScrollTop() + (HtmlElement.$Height() - HtmlElement.$Height(Element.Element0())) / 2);
                Element.Style('zIndex', HtmlElement.ZIndex + this.ZIndex).ToXY(Left < 0 ? 0 : Left, Top < 0 ? 0 : Top);
                if (this.IsFixed) {
                    if (this.ShowInterval) clearTimeout(this.ShowInterval);
                    this.ShowInterval = setTimeout(this.ShowFunction, this.Timeout);
                }
            }
            if (this.IsEsc) HtmlElement.$(document.body).AddEvent('keydown', this.KeyDownFunction);
        }
        Hide() {
            HtmlElement.$(document.body).DeleteEvent('keypress', this.KeyDownFunction);
            var Element = HtmlElement.$Id(this.Id);
            if (Pub.IsFixed) {
                if (this.AutoClass) {
                    Element.RemoveClass(this.AutoClass);
                    Element.Style('zIndex', -100);
                }
                else Element.Display(0);
                OverDiv.Default.Hide(this.Id);
            }
            else {
                if (this.ShowInterval) clearTimeout(this.ShowInterval);
                this.ShowInterval = null;
                Element.Display(0);
                OverDiv.Default.Hide();
            }
        }
        private KeyDown(Event: BrowserEvent) {
            if (Event.keyCode == 27) {
                var Element = Event.$Name('floatcenter');
                if (Element && Element.id == this.Id) this.Hide();
            }
        }
        static FloatCenters: { [key: string]: FloatCenter };
        static GetFloatCenters() {
            if (!this.FloatCenters) Pub.OnLoadedHash.Add(Pub.ThisFunction(this, this.GetFloatCenters));
            this.FloatCenters = {};
            for (var Childs = HtmlElement.$('@floatcenter').GetElements(), Index = Childs.length; --Index >= 0;) {
                var Element = Childs[Index], Parameter = HtmlElement.$Attribute(Element, 'floatcenter');
                (Parameter = Parameter ? eval('(' + Parameter + ')') : {}).Id = Element.id;
                this.FloatCenters[Element.id] = new FloatCenter(Parameter);
            }
        }
    }
    Pub.OnLoad(FloatCenter.GetFloatCenters, FloatCenter, true);
}