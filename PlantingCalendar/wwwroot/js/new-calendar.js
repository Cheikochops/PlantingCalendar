angular.module('newCalendarApp').controller('calendar', function ($scope, $http, $timeout) {

    $scope.allSeedList = [];
    $scope.selectedSeedList = [];
    $scope.calendarName = null;
    $scope.calendarYear = null;

    $scope.isConfirmSave = false;
    $scope.isSaving = false;

    $scope.confirmSave = function () {
        $scope.isConfirmSave = true
        $scope.saveTimeout = $timeout(() => {
            $scope.isConfirmSave = false;
        }, 3000);
    }

    $scope.loadSeedList = function () {
        var url = "api/seeds/list";

        $http.get(url).then(
            function (response) {
                $scope.allSeedList = response.data;
            });
    };

    $scope.createCalendar = function () {

        var url = "api/calendar";

        clearTimeout($scope.saveTimeout)
        $scope.isSaving = true;

        var seeds = []

        $scope.selectedSeedList.forEach(x => {
            seeds.push(x.id)
        })

        var data = {
            CalendarName: $scope.calendarName,
            CalendarYear: $scope.calendarYear,
            Seeds: seeds
        }

        $http({
            url: url,
            data: data,
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
        }).then(function mySuccess(response) {
            window.location.href = "calendar?calendarId=" + response.data
            $scope.isSaving = false;
            $scope.isConfirmSave = false;
        }, function myError(response) {
            $scope.isSaving = false;
            $scope.isConfirmSave = false;
        });
    };

    $scope.addSeed = function (seed) {
        $scope.selectedSeedList.push(seed)
        $scope.allSeedList.splice($scope.allSeedList.indexOf(seed), 1);
    }

    $scope.removeSeed = function (seed) {
        $scope.allSeedList.push(seed)
        $scope.selectedSeedList.splice($scope.selectedSeedList.indexOf(seed), 1);
    }

    $scope.loadSeedList();

});