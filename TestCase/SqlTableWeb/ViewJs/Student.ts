/// <reference path = "../../../AutoCSer/CodeGenerator/js/base.page.ts" />
/// <reference path = "./WebPath.ts" />
'use strict';
module Demo {
    export class Student {
        private Id: number;
        private Path: AutoCSerPath.Student;
        constructor(Value: Object) {
            AutoCSer.Pub.Copy(this, Value);
            this.Path = new AutoCSerPath.Student(this.Id);
        }
    }
}
AutoCSer.Pub.LoadViewType(Demo.Student);