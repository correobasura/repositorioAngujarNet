'use strict';
angular.module('AngularAuthApp')
    .controller('indexController', function ($scope, $location, authService) {
        $scope.logOut = function () {
            authService.logOut();
            $location.path('/home');
        }
        $scope.authentication = authService.authentication;
    });