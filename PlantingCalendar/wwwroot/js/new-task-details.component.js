
angular.module('seedApp')
    .component('newTaskDetails', {
        templateUrl: 'templates/new-task-details.component.html',
        controller: NewTaskController,
        bindings: {
            refresh: '&',
            seeds: '<',
            refreshTrigger: '<',
            calendarId: '<'
        }
    });

function NewTaskController($http) {

    var ctrl = this;
    ctrl.repeatableType = [];
    ctrl.newTaskRepeatableType = '0';

    ctrl.isSaving = false;
    ctrl.isConfirmSave = false;

    ctrl.confirmSave = function () {
        ctrl.isConfirmSave = true
        ctrl.confirmSaveTimeout = setTimeout(
            function () {
                ctrl.isConfirmSave = false;
            }, 3000);
    }

    ctrl.getRepeatableType = function () {
        var url = "api/tasks/types"

        $http.get(url).then(
            function (response) {
                ctrl.repeatableTypes = response.data;
            });
    }

    ctrl.saveNewTask = function () {
        var url = "api/tasks/new";

        clearTimeout(ctrl.confirmSaveTimeout);

        var colour = null;
        var character = null;
        if (ctrl.isDisplay) {
            colour = ctrl.displayColour
            character = ctrl.displayChar
        }

        var data = {
            calendarId: ctrl.calendarId,
            seeds: ctrl.seedIds,
            name: ctrl.name,
            description: ctrl.taskDescription,
            isRanged: ctrl.isRanged,
            rangeStartDate: ctrl.rangeStartDate != null ? ctrl.rangeStartDate.toLocaleDateString('af-ZA') : null,
            rangeEndDate: ctrl.rangeEndDate != null ? ctrl.rangeEndDate.toLocaleDateString('af-ZA') : null,
            repeatableType: ctrl.newTaskRepeatableType,
            singleDate: ctrl.setDate != null ? ctrl.setDate.toLocaleDateString('af-ZA') : null,
            fromDate: ctrl.fromDate != null ? ctrl.fromDate.toLocaleDateString('af-ZA') : null,
            toDate: ctrl.toDate != null ? ctrl.toDate.toLocaleDateString('af-ZA') : null, 
            isDisplay: ctrl.isDisplay,
            displayColour: colour,
            displayChar: character
        }

        ctrl.isSaving = true;

        $http({
            url: url,
            data: data,
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            ctrl.isSaving = false;
            ctrl.isConfirmSave = false;
            ctrl.refresh();

        }, function myError(response) {
            ctrl.isSaving = false;
            ctrl.isConfirmSave = false;
        });
    }

    ctrl.$onChanges = function () {
        ctrl.newTaskRepeatableType = '0'
        ctrl.isRanged = false
        ctrl.name = null
        ctrl.seedIds = []
        ctrl.taskDescription = null
        ctrl.rangeStartDate = null
        ctrl.rangeEndDate = null
        ctrl.setDate = null
        ctrl.fromDate = null
        ctrl.toDate = null
        ctrl.displayColour = null
        ctrl.displayChar = null
        ctrl.isDisplay = false

        ctrl.isSaving = false;
        ctrl.isConfirmSave = false;

    }

    ctrl.isNever = function () {
        return ctrl.newTaskRepeatableType == '0'
    }

    ctrl.getRepeatableType();
}
