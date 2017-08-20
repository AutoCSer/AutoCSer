/// <reference path = "../../../AutoCSer/CodeGenerator/js/base.page.ts" />
/// <reference path = "./WebPath.ts" />
'use strict';
module Demo {
    export class Class {
        private Id: number;
        private Path: AutoCSerPath.Class;
        constructor(Value: Object) {
            AutoCSer.Pub.Copy(this, Value);
            this.Path = new AutoCSerPath.Class(this.Id);
        }
    }
}
AutoCSer.Pub.LoadViewType(Demo.Class);