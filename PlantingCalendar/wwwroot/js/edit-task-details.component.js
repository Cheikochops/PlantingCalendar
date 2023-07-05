
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

    ctrl.saveTask = function (task) {
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
}
