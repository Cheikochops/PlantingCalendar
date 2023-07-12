
angular.module('seedApp')
    .component('newTaskDetails', {
        templateUrl: 'templates/new-task-details.component.html',
        controller: NewTaskController,
        bindings: {
            refresh: '&',
            seeds: '<',
            refreshTrigger: '<'
        }
    });

function NewTaskController($http) {

    var ctrl = this;
    ctrl.repeatableType = [];
    ctrl.newTask = false;
    ctrl.newTaskRepeatableType = '0';

    ctrl.getRepeatableType = function () {
        var url = "api/tasks/types"

        $http.get(url).then(
            function (response) {
                ctrl.repeatableTypes = response.data;
            });
    }

    ctrl.saveNewtask = function (task) {
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
            task.isRanged = false;

        }, function myError(response) {

        });
    }

    ctrl.isNever = function () {
        return ctrl.newTaskRepeatableType == '0'
    }

    ctrl.getRepeatableType();
}
