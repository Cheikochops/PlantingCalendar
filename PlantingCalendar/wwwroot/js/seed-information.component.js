
angular.module('seedApp')
    .component('seedInfo', {
        templateUrl: 'templates/seed-information.component.html',
        controller: SeedInfoController,
        bindings: {
            seedId: '<'
        }
    });

function SeedInfoController($http) {

    var ctrl = this;

    ctrl.$onChanges = function (changes) {
        if (ctrl.seedId != null) {
            ctrl.getSeedDetails(ctrl.seedId);
        }
        else {
            ctrl.seedData = {
                sowAction: {
                    actionName: "Sow",
                    displayChar: "S"
                },
                harvestAction: {
                    actionName: "Harvest",
                    displayChar: "H"
                },
                actions: []
            }
        }
    }

    ctrl.addAction = function () {
        ctrl.seedData.actions.push(
            {

            }
        )
    }

    ctrl.removeRow = function (index) {
        console.log(index)
        ctrl.seedData.actions.splice(index, 1);
    }

    ctrl.saveSeed = function() {

    }

    ctrl.deleteSeed = function () {

    }

    ctrl.photoUploaded = function () {

    }

    ctrl.getSeedDetails = function () {
        if (ctrl.seedId != null) {
            var url = "api/seeds?seedId=" + ctrl.seedId;

            $http.get(url).then(
                function (response) {
                    ctrl.seedData = response.data;

                    console.log(ctrl.seedData)
                });
        }
    }

    ctrl.getSeedDetails();
}
