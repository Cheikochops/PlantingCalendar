
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

function SeedInfoController($http) {

    var ctrl = this;

    ctrl.hasData = false;

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
                    
                },
                harvestAction: { 
                    actionName: "Harvest",
                    displayChar: "H",
                    actionId: null
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

        }, function myError(response) {
            
        });
    }

    ctrl.deleteSeed = function () {
        var url = "api/seeds?seedId=" + ctrl.seedId;

        $http({
            url: url,
            method: "DELETE"
        }).then(function mySuccess(response) {
            ctrl.refresh();
        }, function myError(response) {

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
