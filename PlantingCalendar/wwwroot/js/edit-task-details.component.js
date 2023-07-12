
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
    ctrl.thisTask = {}
    ctrl.thisSeed = {}

    ctrl.saveTask = function () {
        var url = "api/tasks?taskId=" + ctrl.thisTask.taskId;

        var data = {
            taskId: ctrl.thisTask.taskId,
            taskName: ctrl.thisTask.taskName,
            taskDescription: ctrl.thisTask.taskDescription,
            displayChar: ctrl.thisTask.displayChar,
            displayColour: ctrl.thisTask.displayColour,
            isRanged: ctrl.thisTask.isRanged,
            taskStartDate: ctrl.thisTask.taskStartDate ?? null,
            taskEndDate: ctrl.thisTask.taskEndDate ?? null,
            taskSetDate: ctrl.thisTask.taskSetDate ?? null
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
        var url = "api/tasks?taskId=" + ctrl.thisTask.taskId;

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
        console.log(ctrl.task);
        ctrl.thisTask = structuredClone(ctrl.task)
        ctrl.thisSeed = structuredClone(ctrl.seed)
    }
}
