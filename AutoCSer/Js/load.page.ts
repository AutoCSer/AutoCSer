'use strict';
module AutoCSer {
    export class Loader {
        static DocumentHead: HTMLHeadElement;
        static LoadScript: string;
        static Version: string;
        static Charset: string;
        static ViewVersion: string;
        static JsDomain: string;
        static PageView: boolean;

        static CreateJavascipt(Src: string, Charset = Loader.Charset): HTMLScriptElement {
            var Script = document.createElement('script');
            Script.lang = 'javascript';
            Script.type = 'text/javascript';
            Script.src = Src;
            Script.charset = Charset;
            return Script;
        }
        static AppendJavaScript(Src: string, Charset = Loader.Charset): void {
            this.DocumentHead.appendChild(this.CreateJavascipt(Src, Charset));
        }
        static Load(): void {
            Loader.DocumentHead = document.getElementsByTagName('head')[0];
            for (var Nodes = Loader.DocumentHead.childNodes, Index = Nodes.length; Index !== 0;) {
                var Node = Nodes[--Index] as HTMLScriptElement;
                if (Node.tagName && Node.tagName.toLowerCase() === 'script') {
                    var LoadJs = Node.src.match(/^(https?:\/\/[^\/]+\/)Js\/load(Page)?\.js\?__VERSIONNAME__(__VERSIONNAME__?)=(0?)([\dABCDEF]+)?$/i);
                    if (LoadJs && LoadJs[1] && LoadJs[5]) {
                        Loader.JsDomain = LoadJs[1];
                        Loader.ViewVersion = LoadJs[3];
                        Loader.LoadScript = LoadJs[4];
                        Loader.Version = LoadJs[5];
                        Loader.Charset = Node.charset;
                        break;
                    }
                }
            }
            if (!Loader.JsDomain) Loader.JsDomain = '/';
            Loader.PageView = false;
            Loader.AppendJavaScript(Loader.JsDomain + 'Js/base.js?__VERSIONNAME__=' + Loader.Version);
        }
    }
    Loader.Load();
}