﻿
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

        var data = {
            calendarId: ctrl.calendarId,
            seeds: ctrl.seedIds,
            name: ctrl.name,
            description: ctrl.description,
            isRanged: ctrl.isRanged,
            rangeStartDate: ctrl.rangeStartDate,
            rangeEndDate: ctrl.rangeEndDate,
            repeatableType: ctrl.newTaskRepeatableType,
            singleDate: ctrl.setDate,
            fromDate: ctrl.fromDate,
            toDate: ctrl.toDate
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

        }, function myError(response) {
            ctrl.isSaving = false;
            ctrl.isConfirmSave = false;
        });
    }

    ctrl.$onChanges = function () {
        console.log(ctrl.seeds)
        ctrl.newTaskRepeatableType = '0'
        ctrl.isRanged = false
        ctrl.name = null
        ctrl.seedIds = []
        ctrl.description = null
        ctrl.rangeStartDate = null
        ctrl.rangeEndDate = null
        ctrl.setDate = null
        ctrl.fromDate = null
        ctrl.toDate = null

        ctrl.isSaving = false;
        ctrl.isConfirmSave = false;

    }

    ctrl.isNever = function () {
        return ctrl.newTaskRepeatableType == '0'
    }

    ctrl.getRepeatableType();
}
