/// <reference path = "../base.page.ts" />
'use strict';
var AutoCSer;
(function (AutoCSer) {
    var MathJaxLoader = (function () {
        function MathJaxLoader() {
        }
        MathJaxLoader.SetShow = function (Skin) {
            if (Skin === void 0) { Skin = AutoCSer.Pub.PageView; }
            if (Skin.OnSet)
                Skin.OnSet.Add(AutoCSer.Pub.ThisFunction(this, this.Show));
            this.Show();
        };
        MathJaxLoader.Get = function () {
            return window['MathJax'];
        };
        MathJaxLoader.CheckShowElement = function (Element) {
            return !AutoCSer.HtmlElement.$ElementName(Element, 'htmleditor');
        };
        MathJaxLoader.Show = function () {
            for (var MathJax = this.Get(), Values = [], Elements = AutoCSer.HtmlElement.$('@lang=latex').GetElements(), Index = 0; Index - Elements.length; ++Index) {
                if (!AutoCSer.Pub.GetHtmlEditor(Elements[Index])) {
                    var Element = Elements[Index], Nodes = Element.childNodes;
                    if (Nodes.length) {
                        var Span = Nodes[0];
                        if (Span.tagName && Span.tagName.toLowerCase() == 'span') {
                            if (MathJax) {
                                var Text = AutoCSer.HtmlElement.$GetText(Span);
                                if (Text) {
                                    Element.innerHTML = this.Format(Text);
                                    Values.push(Element);
                                }
                            }
                            else {
                                this.Load(AutoCSer.Pub.ThisFunction(this, this.Show));
                                return;
                            }
                        }
                    }
                }
            }
            if (Values.length)
                MathJax.Hub.Queue(['Typeset', MathJax.Hub, Values, {}], AutoCSer.Pub.ThisFunction(MathJaxLoader, MathJaxLoader.FixedBorder, [null, Values]));
        };
        MathJaxLoader.FixedBorder = function (Element, Elements) {
            if (Elements === void 0) { Elements = null; }
            if (Elements) {
                for (var Index = 0; Index - Elements.length; this.FixedBorder(Elements[Index++]))
                    ;
            }
            else if (Element) {
                for (var Elements = new AutoCSer.HtmlElement('.MathJax/nobr/span', Element).GetElements(), Index = Elements.length; Index;) {
                    var Nodes = Elements[--Index].childNodes;
                    if (Nodes.length > 1) {
                        var Span = Nodes[1];
                        if (Span.style.borderLeftWidth == '0.003em' || Span.style.borderLeftWidth == '0.002em')
                            Span.style.borderLeftWidth = '0';
                    }
                }
            }
        };
        MathJaxLoader.Format = function (Text) {
            return '$\n' + Text.replace(/\xA0/g, ' ').ToHTML() + '\n$';
        };
        MathJaxLoader.LoadConfig = function () {
            this.Get().Hub.Config({ extensions: ['tex2jax.js'], jax: ['input/TeX', 'output/HTML-CSS'], elements: [''], tex2jax: { inlineMath: [['$', '$'], ['\\(', '\\)']] } });
        };
        MathJaxLoader.Load = function (OnLoad) {
            if (!this.IsLoad) {
                this.IsLoad = true;
                var config = document.createElement('script');
                config.type = 'text/x-mathjax-config';
                config.text = 'AutoCSer.MathJaxLoader.LoadConfig();';
                document.getElementsByTagName('head')[0].appendChild(config);
            }
            AutoCSer.Pub.OnModule(['mathJax/MathJax'], OnLoad, true);
        };
        MathJaxLoader.ShowElement = function (Element) {
            var MathJax = this.Get();
            MathJax.Hub.Queue(['Typeset', MathJax.Hub, Element], AutoCSer.Pub.ThisFunction(MathJaxLoader, MathJaxLoader.FixedBorder, [Element]));
        };
        MathJaxLoader.TryShow = function (Element, Text) {
            if (Text) {
                Element.innerHTML = this.Format(Text);
                var MathJax = this.Get();
                if (MathJax)
                    this.ShowElement(Element);
                else
                    this.Load(AutoCSer.Pub.ThisFunction(this, this.ShowElement, [Element]));
            }
            else
                Element.innerHTML = '';
        };
        return MathJaxLoader;
    }());
    AutoCSer.MathJaxLoader = MathJaxLoader;
    AutoCSer.Pub.OnLoad(MathJaxLoader.SetShow, MathJaxLoader, true);
})(AutoCSer || (AutoCSer = {}));
