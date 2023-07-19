
angular.module('seedApp')
    .component('seedInfo', {
        templateUrl: 'templates/seed-information.component.html',
        controller: SeedInfoController,
        bindings: {
            seedId: '<',
            refresh: '&',
            readOnly: '<'
        }
    });

function SeedInfoController($http, $timeout) {

    var ctrl = this;

    ctrl.hasData = false;

    ctrl.isConfirmSave = false;
    ctrl.isSaving = false;
    ctrl.isDeleting = false;
    ctrl.isConfirmDelete = false;

    ctrl.confirmSave = function () {
        ctrl.isConfirmSave = true
        ctrl.saveTimeout = $timeout(() => {
            ctrl.isConfirmSave = false;
        }, 3000);
    }

    ctrl.confirmDelete = function () {
        ctrl.isConfirmDelete = true
        ctrl.deleteTimeout = $timeout(() => {
            ctrl.isConfirmDelete = false;
        }, 3000);
    }

    ctrl.$onChanges = function (changes) {
        ctrl.expiryDate = null

        if (ctrl.seedId != null) {
            ctrl.getSeedDetails(ctrl.seedId);
            ctrl.hasData = true;
        }
        else {
            ctrl.seedData = {
                sowAction: {
                    actionName: "Sow",
                    displayChar: "S",
                    actionId: null,
                    isDisplay: true
                    
                },
                harvestAction: { 
                    actionName: "Harvest",
                    displayChar: "H",
                    actionId: null,
                    isDisplay: true
                },
                actions: []
            }
            ctrl.hasData = false;
        }
    }

    ctrl.addAction = function () {
        ctrl.seedData.actions.push(
            {

            }
        )
    }

    ctrl.removeRow = function (index) {
        ctrl.seedData.actions.splice(index, 1);
    }

    ctrl.saveSeed = function () {
        var url = "api/seeds"

        clearTimeout(ctrl.saveTimeout)
        ctrl.isSaving = true;

        var data = {
            description: ctrl.seedData.description ?? "",
            expiryDate: ctrl.expiryDate,
            actions: ctrl.seedData.actions,
            sowAction: ctrl.seedData.sowAction,
            harvestAction: ctrl.seedData.harvestAction,
            id: ctrl.seedData.id,
            plantType: ctrl.seedData.plantType ?? "",
            breed: ctrl.seedData.breed ?? "",
            sunRequirement: ctrl.seedData.sunRequirement ?? "",
            waterRequirement: ctrl.seedData.waterRequirement ?? ""
        }

        $http({
            url: url,
            data: data,
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            ctrl.refresh();
            ctrl.getSeedDetails();
            ctrl.isSaving = false;
            ctrl.isConfirmSave = false;

        }, function myError(response) {

            ctrl.isSaving = false;
            ctrl.isConfirmSave = false;
        });
    }

    ctrl.deleteSeed = function () {
        var url = "api/seeds?seedId=" + ctrl.seedId;

        clearTimeout(ctrl.deleteTimeout)
        ctrl.isDeleting = true;

        $http({
            url: url,
            method: "DELETE"
        }).then(function mySuccess(response) {
            ctrl.isDeleting = false
            ctrl.isConfirmDelete = false;
            ctrl.refresh();
        }, function myError(response) {
            ctrl.isDeleting = false
            ctrl.isConfirmDelete = false;
        });
    }

    ctrl.photoUploaded = function () {

    }

    ctrl.getSeedDetails = function () {
        if (ctrl.seedId != null) {
            var url = "api/seeds?seedId=" + ctrl.seedId;

            $http.get(url).then(
                function (response) {
                    ctrl.seedData = response.data;

                    if (ctrl.seedData.expiryDate == null) {
                        ctrl.expiryDate = null
                    }
                    else {
                        ctrl.expiryDate = new Date(ctrl.seedData.expiryDate)
                    }
                });
        }
    }

    ctrl.isValid = function () {

    }

    ctrl.getSeedDetails();
}
