'use strict';
angular.module('AngularAuthApp')
    .controller('loginController',
        function ($scope, $location, authService) {

            $scope.loginData = {
                usuario: "",
                password: ""
            };

            $scope.message = "";

            $scope.login = function () {

                authService.login($scope.loginData).then(function (response) {

                    $location.path('/orders');

                },
                 function (err) {
                     $scope.message = err.error_description;
                 });
            };

        });