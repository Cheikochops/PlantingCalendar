
angular.module('seedApp')
    .component('editCalendarSeeds', {
        templateUrl: 'templates/edit-calendar-seeds.component.html',
        controller: EditCalendarSeedController,
        bindings: {
            calendarId: '<',
            currentSeeds: '<',
            refreshTrigger: '<',
            refresh: '&'
        }
    });

function EditCalendarSeedController($http) {

    var ctrl = this;

    ctrl.allSeedList = null;
    ctrl.currentSeedList = ctrl.currentSeeds;
    ctrl.availableSeeds = null;

    ctrl.seedListFilter = "";

    ctrl.addSeed = function (seed) {
        console.log(seed);
        ctrl.currentSeedList.push({
            id: seed.id,
            plantTypeName: seed.plantType,
            plantBreed: seed.breed
        })
        ctrl.allSeedList.splice(ctrl.allSeedList.indexOf(seed), 1);
    }

    ctrl.removeSeed = function (seed) {
        console.log(seed);
        ctrl.allSeedList.push({
            id: seed.id,
            breed: seed.plantBreed,
            plantType: seed.plantTypeName
        })
        ctrl.currentSeedList.splice(ctrl.currentSeedList.indexOf(seed), 1);
    }

    ctrl.loadSeedList = function () {

        var url = "api/seeds/list";

        $http.get(url).then(
            function (response) {
                ctrl.availableSeeds = response.data;
                ctrl.allSeedList = structuredClone(ctrl.availableSeeds);
                ctrl.currentSeedList.forEach(function (c) {
                    ctrl.allSeedList.forEach(function (a) {
                        if (c.id == a.id) {
                            ctrl.allSeedList.splice(ctrl.allSeedList.indexOf(a), 1)
                        }
                    })
                });

                console.log(ctrl.allSeedList);
            });
    };

    ctrl.saveCalendarSeeds = function () {
        var url = "api/calendar/seeds?calendarId=" + ctrl.calendarId;

        data = [];
        ctrl.currentSeedList.forEach(function (s) {
            data.push(s.id)
        })

        $http({
            url: url,
            data: data,
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            ctrl.refresh();
        }, function myError(response) {

        });
    }

    ctrl.$onChanges = function (changes) {
        ctrl.currentSeedList = structuredClone(ctrl.currentSeeds);
        ctrl.allSeedList = structuredClone(ctrl.availableSeeds);

        if (ctrl.currentSeedList != null && ctrl.allSeedList != null) {
            ctrl.currentSeedList.forEach(function (c) {
                ctrl.allSeedList.forEach(function (a) {
                    if (c.id == a.id) {
                        ctrl.allSeedList.splice(ctrl.allSeedList.indexOf(a), 1)
                    }
                })
            });
        }

        console.log(ctrl);
    }

    ctrl.loadSeedList();
}
