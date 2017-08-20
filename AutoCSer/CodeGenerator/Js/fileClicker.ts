/// <reference path = "./base.page.ts" />
'use strict';
//文件选择自定义界面	<input type="file" id="YYY" /><div fileclicker="{FileId:'YYY'}" id="XXX"></div>
module AutoCSer {
    export class FileClicker implements IDeclare {
        private static DefaultParameter = { Id: null, Event: null, FileId: null, IsFixed: false };
        private Id: string;
        private Event: DeclareEvent;
        private FileId: string;
        private IsFixed: boolean;

        private Element: HTMLInputElement;
        private MoveFucntion: Function;
        constructor(Parameter) {
            Pub.GetParameter(this, FileClicker.DefaultParameter, Parameter);
            this.Start(this.Event || DeclareEvent.Default);
        }
        Start(Event: DeclareEvent) {
            if (!Event.IsGetOnly) {
                var Element = HtmlElement.$Id(this.Id), Input = Element.Element0() as HTMLInputElement;
                if (Input != this.Element) {
                    this.Element = Input;
                    var FileInput = HtmlElement.$Id(this.FileId).Set('FILECLICKER', '{Id:"' + this.Id + '"}');
                    if (!this.IsFixed) {
                        Element.AddEvent('mousemove,mouseover,click', Pub.ThisEvent(this, this.Move));
                        FileInput.Style('outline', '0px').AddEvent('mousemove,mouseover', Pub.ThisEvent(this, this.Move));
                        this.SetCss();
                    }
                }
            }
        }
        private SetCss() {
            HtmlElement.$Id(this.Id).Cursor('pointer');
            HtmlElement.$Id(this.FileId).Opacity(0).Style('position', 'absolute').Display(0).Set('size', 1).Cursor('pointer');
        }
        private Move(Event: BrowserEvent) {
            HtmlElement.$Id(this.FileId).Style('left', (Event.clientX - 80) + 'px').Style('top', (Event.clientY - 8) + 'px').Display(1);
        }
        //private NewInput() {
        //    var Input = HtmlElement.$Id(this.Id);
        //    Input.Replace0(HtmlElement.$Create('input').Set('type', 'file').Set('name', this.FileId).Set('id', this.FileId).Set('onchange', Input.Element0().onchange).Element0());
        //    this.SetCss();
        //}
    }
    new AutoCSer.Declare(FileClicker, 'FileClicker', 'mouseover', 'ParameterId');
}