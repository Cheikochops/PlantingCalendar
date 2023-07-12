angular.module('seedApp').controller('seeds', function ($scope, $http) {
    $scope.seedList = [];
    $scope.order = "plantType";
    $scope.displaySeedId = null;

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

    $scope.setInformationDisplay = function (seedId) {
        if ($scope.displaySeedId == seedId) {
            $scope.displaySeedId = null
        }
        else {
            $scope.displaySeedId = seedId
        }
    }

    $scope.refresh = function () {
        $scope.loadSeedList();

        $scope.displaySeedId = null;
    }

    $scope.unfocus = function () {
        $scope.displaySeedId = null;
    }

    $scope.loadSeedList();
});
