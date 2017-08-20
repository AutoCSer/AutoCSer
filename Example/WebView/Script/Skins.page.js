/// <reference path = "../../../AutoCSer/CodeGenerator/Js/load.page.ts" />
var AutoCSer;
(function (AutoCSer) {
    var Example;
    (function (Example) {
        var Script;
        (function (Script) {
            var Skins = (function () {
                function Skins() {
                }
                Skins.Test = function () {
                    AutoCSer.Skin.Skins['VersionSkin'].SkinData('Version').$Set(AutoCSer.HtmlElement.$GetValueById('VersionInput'));
                };
                Skins.Load = function () {
                    AutoCSer.Skin.Skins['VersionSkin'].Show({ Version: AutoCSer.HtmlElement.$GetValueById('VersionInput') });
                };
                return Skins;
            }());
            Script.Skins = Skins;
        })(Script = Example.Script || (Example.Script = {}));
    })(Example = AutoCSer.Example || (AutoCSer.Example = {}));
})(AutoCSer || (AutoCSer = {}));
AutoCSer.Pub.OnLoad(AutoCSer.Example.Script.Skins.Load);
