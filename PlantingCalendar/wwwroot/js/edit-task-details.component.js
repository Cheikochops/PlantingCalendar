
angular.module('seedApp')
    .component('editTaskDetails', {
        templateUrl: 'templates/edit-task-details.component.html',
        controller: EditTaskController,
        bindings: {
            task: '<',
            seed: '<',
            refresh: '&'
        }
    });

function EditTaskController($http) {

    var ctrl = this;
    ctrl.task = {}
    ctrl.seed = {}

    ctrl.saveTask = function () {
        var url = "api/tasks";

        var data = {
            taskId: ctrl.task.taskId,
            taskName: ctrl.task.taskName,
            taskDescription: ctrl.task.taskDescription,
            displayChar: ctrl.task.displayChar,
            displayColour: ctrl.task.displayColour,
            isRanged: ctrl.task.isRanged,
            taskStartDate: ctrl.task.taskStartDate,
            taskEndDate: ctrl.task.taskEndDate,
            taskSetDate: ctrl.task.taskSetDate
        }

        $http({
            url: url,
            data: data,
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            ctrl.refresh()
        }, function myError(response) {

        });
    }

    ctrl.deleteTask = function () {
        var url = "api/tasks?taskId=" + ctrl.task.taskId;

        $http({
            url: url,
            method: "DELETE",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            ctrl.refresh()
        }, function myError(response) {

        });
    }

    ctrl.$onChanges = function () {

    }
}
