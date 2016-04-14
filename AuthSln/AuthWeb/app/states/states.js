/*app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.when("/orders", {
        controller: "ordersController",
        templateUrl: "/app/views/orders.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});*/

angular.module('AngularAuthApp')
    .config(function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/home');
        $stateProvider
            .state('home', {
                url: '/home',
                data: {
                    authorities: ['ROLE_USER'],
                    pageTitle: 'ekoguiApp.persona.home.title'
                },
                views: {
                    'content@': {
                        templateUrl: '/app/views/home.html',
                        controller: 'homeController'
                    }
                }
            })
            .state('login', {
                url: '/login',
                data: {
                    authorities: ['ROLE_USER'],
                    pageTitle: 'ekoguiApp.persona.home.title'
                },
                views: {
                    'content@': {
                        templateUrl: '/app/views/login.html',
                        controller: 'loginController'
                    }
                }
            })
            .state('signup', {
                url: '/signup',
                data: {
                    authorities: ['ROLE_USER'],
                    pageTitle: 'ekoguiApp.persona.home.title'
                },
                views: {
                    'content@': {
                        templateUrl: '/app/views/signup.html',
                        controller: 'signupController'
                    }
                }
            })
    });
