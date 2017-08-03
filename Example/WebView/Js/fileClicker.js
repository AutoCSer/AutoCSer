/// <reference path = "./base.page.ts" />
'use strict';
//文件选择自定义界面	<input type="file" id="YYY" /><div fileclicker="{FileId:'YYY'}" id="XXX"></div>
var AutoCSer;
(function (AutoCSer) {
    var FileClicker = (function () {
        function FileClicker(Parameter) {
            AutoCSer.Pub.GetParameter(this, FileClicker.DefaultParameter, Parameter);
            this.Start(this.Event || AutoCSer.DeclareEvent.Default);
        }
        FileClicker.prototype.Start = function (Event) {
            if (!Event.IsGetOnly) {
                var Element = AutoCSer.HtmlElement.$Id(this.Id), Input = Element.Element0();
                if (Input != this.Element) {
                    this.Element = Input;
                    var FileInput = AutoCSer.HtmlElement.$Id(this.FileId).Set('FILECLICKER', '{Id:"' + this.Id + '"}');
                    if (!this.IsFixed) {
                        Element.AddEvent('mousemove,mouseover,click', AutoCSer.Pub.ThisEvent(this, this.Move));
                        FileInput.Style('outline', '0px').AddEvent('mousemove,mouseover', AutoCSer.Pub.ThisEvent(this, this.Move));
                        this.SetCss();
                    }
                }
            }
        };
        FileClicker.prototype.SetCss = function () {
            AutoCSer.HtmlElement.$Id(this.Id).Cursor('pointer');
            AutoCSer.HtmlElement.$Id(this.FileId).Opacity(0).Style('position', 'absolute').Display(0).Set('size', 1).Cursor('pointer');
        };
        FileClicker.prototype.Move = function (Event) {
            AutoCSer.HtmlElement.$Id(this.FileId).Style('left', (Event.clientX - 80) + 'px').Style('top', (Event.clientY - 8) + 'px').Display(1);
        };
        FileClicker.DefaultParameter = { Id: null, Event: null, FileId: null, IsFixed: false };
        return FileClicker;
    }());
    AutoCSer.FileClicker = FileClicker;
    new AutoCSer.Declare(FileClicker, 'FileClicker', 'mouseover', 'ParameterId');
})(AutoCSer || (AutoCSer = {}));
