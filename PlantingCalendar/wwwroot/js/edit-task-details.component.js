
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

function EditTaskController($http, $timeout) {

    var ctrl = this;
    ctrl.thisTask = {}
    ctrl.thisSeed = {}

    ctrl.isConfirmSave = false;
    ctrl.isSaving = false;
    ctrl.isDeleting = false;
    ctrl.isConfirmDelete = false;

    ctrl.editConfirmSave = function () {
        ctrl.confirmSave = true
        ctrl.saveTimeout = $timeout(() => {
            ctrl.isConfirmSave = false;
        }, 3000);
    }

    ctrl.editConfirmDelete = function () {
        ctrl.isConfirmDelete = true
        ctrl.deleteTimeout = $timeout(() => {
            ctrl.isConfirmDelete = false;
        }, 3000);
    }

    ctrl.saveTask = function () {
        var url = "api/tasks?taskId=" + ctrl.thisTask.taskId;

        clearTimeout(ctrl.saveTimeout)
        ctrl.isSaving = true;

        var colour = null;
        var character = null;
        if (ctrl.thisTask.isDisplay) {
            colour = ctrl.displayColour
            character = ctrl.displayChar
        }

        var data = {
            taskId: ctrl.thisTask.taskId,
            taskName: ctrl.thisTask.taskName,
            taskDescription: ctrl.thisTask.taskDescription,
            isDisplay: ctrl.isDisplay,
            displayChar: character,
            displayColour: colour,
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

    ctrl.toggleComplete = function () {
        var url = "api/tasks/complete?taskId=" + ctrl.thisTask.taskId;

        $http({
            url: url,
            method: "PUT",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            ctrl.thisTask.isComplete = !ctrl.thisTask.isComplete;
        }, function myError(response) {
        });
    }

    ctrl.$onChanges = function () {
        ctrl.thisTask = structuredClone(ctrl.task)
        ctrl.thisSeed = structuredClone(ctrl.seed)

        ctrl.confirmSave = false;
        ctrl.isSaving = false;
        ctrl.isDeleting = false;
        ctrl.confirmDelete = false;
    }
}
