
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

    ctrl.allSeedList = [];
    ctrl.currentSeedList = [];
    ctrl.availableSeeds = [];

    ctrl.seedListFilter = "";

    ctrl.addSeed = function (seed) {
        ctrl.currentSeedList.push(seed)
        ctrl.allSeedList.splice(ctrl.allSeedList.indexOf(seed), 1);
    }

    ctrl.removeSeed = function (seed) {
        ctrl.allSeedList.push(seed)
        ctrl.currentSeedList.splice(ctrl.currentSeedList.indexOf(seed), 1);
    }

    ctrl.loadSeedList = function () {

        var url = "api/seeds/list";

        $http.get(url).then(
            function (response) {
                ctrl.availableSeeds = response.data;
                ctrl.allSeedList = structuredClone(ctrl.availableSeeds);

                if (ctrl.currentSeeds != null) {
                    ctrl.currentSeeds.forEach(function (c) {
                        ctrl.allSeedList.forEach(function (a) {
                            if (c.id == a.id) {
                                ctrl.currentSeedList.push(a);
                                ctrl.allSeedList.splice(ctrl.allSeedList.indexOf(a), 1)
                            }
                        })
                    })
                }
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
        ctrl.allSeedList = structuredClone(ctrl.availableSeeds);
        ctrl.currentSeedList = [];


        if (ctrl.currentSeeds != null) {
            ctrl.currentSeeds.forEach(function (c) {
                ctrl.allSeedList.forEach(function (a) {
                    if (c.id == a.id) {
                        ctrl.currentSeedList.push(a);
                        ctrl.allSeedList.splice(ctrl.allSeedList.indexOf(a), 1)
                    }
                })
            })
        }
    }

    ctrl.loadSeedList();
}
