
angular.module('seedApp')
    .component('dayDetails', {
        templateUrl: 'templates/day-details.component.html',
        controller: DayDetailsController,
        bindings: {
            seedInfo: '<',
            refresh: '&'
        }
    });

function DayDetailsController($http) {

    var ctrl = this;
    ctrl.repeatableType = [];
    ctrl.newTask = false;
    ctrl.newTaskRepeatableType = '1';

    ctrl.getRepeatableType = function () {
        var url = "api/tasks/types"

        $http.get(url).then(
            function (response) {
                ctrl.repeatableTypes = response.data;
                console.log(ctrl.repeatableTypes);
            });
    }

    ctrl.switchDisplay = function () {
        ctrl.newTask = !ctrl.newTask;
    }

    ctrl.setTaskDate = function (task) {
        var url = "api/tasks";

        var data = {
            taskId: task.taskId,
            day: ctrl.seedInfo.currentDay,
            month: ctrl.seedInfo.month
        }

        $http({
            url: url,
            data: data,
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            var task = ctrl.seedInfo.tasks.tasks.filter(function (s) { return s.taskId == task.taskId })[0]
            task.isSet = true;

        }, function myError(response) {

        });
    }

    ctrl.isNever = function () {
        return ctrl.newTaskRepeatableType == '1'
    }

    ctrl.getRepeatableType();
}
