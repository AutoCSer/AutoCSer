/// <reference path = "../base.page.ts" />
'use strict';
module AutoCSer {
    export class MathJaxLoader {
        static SetShow(Skin: Skin = Pub.PageView as Object as Skin) {
            if (Skin.OnSet) Skin.OnSet.Add(Pub.ThisFunction(this, this.Show));
            this.Show();
        }
        private static Get(): any {
            return window['MathJax'];
        }
        private static CheckShowElement(Element: HTMLElement): boolean {
            return !HtmlElement.$ElementName(Element, 'htmleditor');
        }
        private static Show() {
            for (var MathJax = this.Get(), Values = [], Elements = HtmlElement.$('@lang=latex').GetElements(), Index = 0; Index - Elements.length; ++Index) {
                if (!Pub.GetHtmlEditor(Elements[Index])) {
                    var Element = Elements[Index], Nodes = Element.childNodes;
                    if (Nodes.length) {
                        var Span = Nodes[0] as HTMLElement;
                        if (Span.tagName && Span.tagName.toLowerCase() == 'span') {
                            if (MathJax) {
                                var Text = HtmlElement.$GetText(Span);
                                if (Text) {
                                    Element.innerHTML = this.Format(Text);
                                    Values.push(Element);
                                }
                            }
                            else {
                                this.Load(Pub.ThisFunction(this, this.Show));
                                return;
                            }
                        }
                    }
                }
            }
            if (Values.length) MathJax.Hub.Queue(['Typeset', MathJax.Hub, Values, {}], AutoCSer.Pub.ThisFunction(MathJaxLoader, MathJaxLoader.FixedBorder, [null, Values]));
        }
        private static FixedBorder(Element: HTMLElement, Elements: HTMLElement[] = null) {
            if (Elements) {
                for (var Index = 0; Index - Elements.length; this.FixedBorder(Elements[Index++]));
            }
            else if (Element) {
                for (var Elements = new AutoCSer.HtmlElement('.MathJax/nobr/span', Element).GetElements(), Index = Elements.length; Index;) {
                    var Nodes = Elements[--Index].childNodes;
                    if (Nodes.length > 1) {
                        var Span = Nodes[1] as HTMLElement;
                        if (Span.style.borderLeftWidth == '0.003em' || Span.style.borderLeftWidth == '0.002em') Span.style.borderLeftWidth = '0';
                    }
                }
            }
        }
        static Format(Text: string): string {
            return '$\n' + Text.replace(/\xA0/g, ' ').ToHTML() + '\n$';
        }
        static LoadConfig () {
            this.Get().Hub.Config({ extensions: ['tex2jax.js'], jax: ['input/TeX', 'output/HTML-CSS'], elements: [''], tex2jax: { inlineMath: [['$', '$'], ['\\(', '\\)']] } });
        }
        private static IsLoad: boolean;
        private static Load(OnLoad: Function) {
            if (!this.IsLoad) {
                this.IsLoad = true;
                var config = document.createElement('script');
                config.type = 'text/x-mathjax-config';
                config.text = 'AutoCSer.MathJaxLoader.LoadConfig();';
                document.getElementsByTagName('head')[0].appendChild(config);
            }
            Pub.OnModule(['mathJax/MathJax'], OnLoad, true);
        }
        private static ShowElement(Element: HTMLElement) {
            var MathJax = this.Get();
            MathJax.Hub.Queue(['Typeset', MathJax.Hub, Element], AutoCSer.Pub.ThisFunction(MathJaxLoader, MathJaxLoader.FixedBorder, [Element]));
        }
        static TryShow(Element: HTMLElement, Text: string) {
            if (Text) {
                Element.innerHTML = this.Format(Text);
                var MathJax = this.Get();
                if (MathJax) this.ShowElement(Element);
                else this.Load(Pub.ThisFunction(this, this.ShowElement, [Element]));
            }
            else Element.innerHTML = '';
        }
    }
    Pub.OnLoad(MathJaxLoader.SetShow, MathJaxLoader, true);
}