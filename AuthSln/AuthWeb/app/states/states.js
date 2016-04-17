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
                },
                resolve: {
                    translatePartialLoader: ['$translate', '$translatePartialLoader', function ($translate, $translatePartialLoader) {
                        $translatePartialLoader.addPart('seleccionarEntidad');
                        return $translate.refresh();
                    }]
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
