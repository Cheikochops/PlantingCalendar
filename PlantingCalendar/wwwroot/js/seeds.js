angular.module('seedApp').controller('seeds', function ($scope, $http) {
    $scope.seedList = [];
    $scope.order = "plantType";
    $scope.displaySeedId = null;
    $scope.showExpired = true;

    $scope.selectedSeedId = null;

    $scope.loadSeedList = function () {

        var url = "api/seeds/list";

        $http.get(url).then(
            function (response) {
                $scope.seedList = response.data;
            });
    };

    $scope.orderBy = function (o) {
        $scope.order = o
    };

    $scope.setInformationDisplay = function (seed) {
        if (seed == null) {
            $scope.displaySeedId = null
            $scope.selectedSeedId = null
        }

        if ($scope.displaySeedId == seed.id) {
            $scope.displaySeedId = null
            $scope.selectedSeedId = null
        }
        else {
            $scope.displaySeedId = seed.id
            $scope.selectedSeedId = seed.id
        }
    }

    $scope.refresh = function () {
        $scope.loadSeedList();

        $scope.displaySeedId = null;
    }

    $scope.shouldShow = function (item) {
        return $scope.showExpired || (!$scope.showExpired && !item.isExpired)
    }

    $scope.loadSeedList();
});
