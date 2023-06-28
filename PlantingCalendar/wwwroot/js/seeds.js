angular.module('seedApp').controller('seeds', function ($scope, $http) {
    $scope.seedList = [];
    $scope.order = "plantType";
    $scope.displaySeedId = null;

    $scope.loadSeedList = function () {

        var url = "api/seeds/list";

        $http.get(url).then(
            function (response) {
                $scope.seedList = response.data;

                console.log($scope.seedList)
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

    $scope.loadSeedList();
});
