/// <reference path = "./overDiv.ts" />
/*Include:Js\overDiv*/
//界面居中弹出层	<div body="true" floatcenter="{}" id="XXX" style="display:none"></div>
//AutoCSer.FloatCenter.FloatCenters.XXX.Show();
var AutoCSer;
(function (AutoCSer) {
    var FloatCenter = (function () {
        function FloatCenter(Parameter) {
            AutoCSer.Pub.GetParameter(this, FloatCenter.DefaultParameter, Parameter);
            this.KeyDownFunction = AutoCSer.Pub.ThisEvent(this, this.KeyDown);
            if (this.SkinView)
                this.Skin = AutoCSer.Skin.Skins[this.Id] || new AutoCSer.Skin(this.Id);
            if (AutoCSer.Pub.IsFixed) {
                if (!this.AutoClass)
                    AutoCSer.HtmlElement.$Id(this.Id).Style('position', 'fixed').Style('top,left', '50%').Style('transform', 'translateX(-50%) translateY(-50%)');
            }
            else {
                AutoCSer.HtmlElement.$Id(this.Id).Style('position', 'absolute');
                this.ShowFunction = AutoCSer.Pub.ThisFunction(this, this.Show);
                this.Hide();
            }
        }
        FloatCenter.prototype.Show = function () {
            var Element = AutoCSer.HtmlElement.$Id(this.Id);
            if (AutoCSer.Pub.IsFixed) {
                AutoCSer.OverDiv.Default.Show(this.Id, this.ZIndex || !this.AutoClass ? AutoCSer.HtmlElement.ZIndex + this.ZIndex : 0);
                Element.Style('zIndex', AutoCSer.HtmlElement.ZIndex + this.ZIndex);
                if (this.AutoClass) {
                    if (this.Skin)
                        this.Skin.SetHtml(this.SkinView);
                    Element.AddClass(this.AutoClass);
                }
                else {
                    if (this.Skin)
                        this.Skin.Show(this.SkinView);
                    else
                        Element.Display(1);
                }
            }
            else {
                AutoCSer.OverDiv.Default.Show();
                if (this.Skin)
                    this.Skin.Show(this.SkinView);
                else
                    Element.Display(1);
                var Left = (AutoCSer.HtmlElement.$GetScrollLeft() + (AutoCSer.HtmlElement.$Width() - AutoCSer.HtmlElement.$Width(Element.Element0())) / 2), Top = (AutoCSer.HtmlElement.$GetScrollTop() + (AutoCSer.HtmlElement.$Height() - AutoCSer.HtmlElement.$Height(Element.Element0())) / 2);
                Element.Style('zIndex', AutoCSer.HtmlElement.ZIndex + this.ZIndex).ToXY(Left < 0 ? 0 : Left, Top < 0 ? 0 : Top);
                if (this.IsFixed) {
                    if (this.ShowInterval)
                        clearTimeout(this.ShowInterval);
                    this.ShowInterval = setTimeout(this.ShowFunction, this.Timeout);
                }
            }
            if (this.IsEsc)
                AutoCSer.HtmlElement.$(document.body).AddEvent('keydown', this.KeyDownFunction);
        };
        FloatCenter.prototype.Hide = function () {
            AutoCSer.HtmlElement.$(document.body).DeleteEvent('keypress', this.KeyDownFunction);
            var Element = AutoCSer.HtmlElement.$Id(this.Id);
            if (AutoCSer.Pub.IsFixed) {
                if (this.AutoClass) {
                    Element.RemoveClass(this.AutoClass);
                    Element.Style('zIndex', -100);
                }
                else
                    Element.Display(0);
                AutoCSer.OverDiv.Default.Hide(this.Id);
            }
            else {
                if (this.ShowInterval)
                    clearTimeout(this.ShowInterval);
                this.ShowInterval = null;
                Element.Display(0);
                AutoCSer.OverDiv.Default.Hide();
            }
        };
        FloatCenter.prototype.KeyDown = function (Event) {
            if (Event.keyCode == 27) {
                var Element = Event.$Name('floatcenter');
                if (Element && Element.id == this.Id)
                    this.Hide();
            }
        };
        FloatCenter.GetFloatCenters = function () {
            if (!this.FloatCenters)
                AutoCSer.Pub.OnLoadedHash.Add(AutoCSer.Pub.ThisFunction(this, this.GetFloatCenters));
            this.FloatCenters = {};
            for (var Childs = AutoCSer.HtmlElement.$('@floatcenter').GetElements(), Index = Childs.length; --Index >= 0;) {
                var Element = Childs[Index], Parameter = AutoCSer.HtmlElement.$Attribute(Element, 'floatcenter');
                (Parameter = Parameter ? eval('(' + Parameter + ')') : {}).Id = Element.id;
                this.FloatCenters[Element.id] = new FloatCenter(Parameter);
            }
        };
        FloatCenter.DefaultParameter = { Id: null, IsFixed: false, IsEsc: true, Timeout: 200, SkinView: null, ZIndex: 0, AutoClass: null };
        return FloatCenter;
    }());
    AutoCSer.FloatCenter = FloatCenter;
    AutoCSer.Pub.OnLoad(FloatCenter.GetFloatCenters, FloatCenter, true);
})(AutoCSer || (AutoCSer = {}));
