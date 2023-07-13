
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

    ctrl.isConfirmSave = false;
    ctrl.isSaving = false;
    ctrl.isDeleting = false;
    ctrl.isConfirmDelete = false;

    ctrl.confirmSave = function () {
        ctrl.confirmSave = true
        ctrl.saveTimeout = setTimeout(
            function () {
                ctrl.isConfirmSave = false;
            }, 3000);
    }

    ctrl.confirmDelete = function () {
        ctrl.isConfirmDelete = true
        ctrl.deleteTimeout = setTimeout(
            function () {
                ctrl.isConfirmDelete = false;
            }, 3000);
    }

    ctrl.saveTask = function () {
        var url = "api/tasks?taskId=" + ctrl.thisTask.taskId;

        clearTimeout(ctrl.saveTimeout)
        ctrl.isSaving = true;

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
            ctrl.isSaving = false;
            ctrl.isConfirmSave = false;
            ctrl.refresh()
        }, function myError(response) {
            ctrl.isSaving = false;
            ctrl.isConfirmSave = false;
        });
    }

    ctrl.deleteTask = function () {
        var url = "api/tasks?taskId=" + ctrl.thisTask.taskId;

        clearTimeout(ctrl.deleteTimeout);
        ctrl.isDeleting = true;

        $http({
            url: url,
            method: "DELETE",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            ctrl.isDeleting = false
            ctrl.isConfirmDelete = false;
            ctrl.refresh()
        }, function myError(response) {
            ctrl.isDeleting = false
            ctrl.isConfirmDelete = false;
        });
    }

    ctrl.$onChanges = function () {
        console.log(ctrl.task);
        ctrl.thisTask = structuredClone(ctrl.task)
        ctrl.thisSeed = structuredClone(ctrl.seed)

        ctrl.confirmSave = false;
        ctrl.isSaving = false;
        ctrl.isDeleting = false;
        ctrl.confirmDelete = false;
    }
}
