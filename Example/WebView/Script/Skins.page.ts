/// <reference path = "../../../AutoCSer/CodeGenerator/Js/load.page.ts" />
module AutoCSer.Example.Script {
    export class Skins {
        static Test() {
            AutoCSer.Skin.Skins['VersionSkin'].SkinData('Version').$Set(AutoCSer.HtmlElement.$GetValueById('VersionInput'));
        }
        static Load() {
            AutoCSer.Skin.Skins['VersionSkin'].Show({ Version: AutoCSer.HtmlElement.$GetValueById('VersionInput') });
        }
    }
}
AutoCSer.Pub.OnLoad(AutoCSer.Example.Script.Skins.Load);