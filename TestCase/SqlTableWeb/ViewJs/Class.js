/// <reference path = "../../../AutoCSer/CodeGenerator/js/base.page.ts" />
/// <reference path = "./WebPath.ts" />
'use strict';
var Demo;
(function (Demo) {
    var Class = (function () {
        function Class(Value) {
            AutoCSer.Pub.Copy(this, Value);
            this.Path = new AutoCSerPath.Class(this.Id);
        }
        return Class;
    }());
    Demo.Class = Class;
})(Demo || (Demo = {}));
AutoCSer.Pub.LoadViewType(Demo.Class);
