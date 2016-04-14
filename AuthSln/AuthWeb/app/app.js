angular.module('AngularAuthApp', [
    // Angular modules     
    'ngRoute',
    // Custom modules 

    // 3rd Party Modules
    'ui.router',
    'angular-loading-bar',
    'LocalStorageModule'
]);


angular.module('AngularAuthApp')
    .run(['authService', function (authService) {
        authService.fillAuthData();
    }]);

angular.module('AngularAuthApp')
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    });