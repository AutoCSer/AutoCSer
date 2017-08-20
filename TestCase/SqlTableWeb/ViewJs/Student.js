/// <reference path = "../../../AutoCSer/CodeGenerator/js/base.page.ts" />
/// <reference path = "./WebPath.ts" />
'use strict';
var Demo;
(function (Demo) {
    var Student = (function () {
        function Student(Value) {
            AutoCSer.Pub.Copy(this, Value);
            this.Path = new AutoCSerPath.Student(this.Id);
        }
        return Student;
    }());
    Demo.Student = Student;
})(Demo || (Demo = {}));
AutoCSer.Pub.LoadViewType(Demo.Student);
