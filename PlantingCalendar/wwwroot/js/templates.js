angular.module('seedInfo')
    .component('seedInfo', {
        templateUrl: 'seed-information-component.html',
        controller: SeedInfoController,
        bindings: {
            seedId: '@'
        }
    });
