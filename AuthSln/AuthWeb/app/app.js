angular.module('AngularAuthApp', [
    // Angular modules     
    'ngRoute',
    // Custom modules 

    // 3rd Party Modules
    'LocalStorageModule',
    'tmh.dynamicLocale',
    'pascalprecht.translate',
    'angucomplete-alt',
    'ngAnimate',
    'ui.bootstrap',
    'ngResource',
    'ui.router',
    'ngCookies',
    'ngAria',
    'ngCacheBuster',
    'ngFileUpload',
    'infinite-scroll'
])
    .run(['authService', function (authService, $translate) {
        authService.fillAuthData();
    }])
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    })
    .config(function ($httpProvider, $translateProvider/*, tmhDynamicLocaleProvider, httpRequestInterceptorCacheBusterProvider*/) {

        //enable CSRF
        $httpProvider.defaults.xsrfCookieName = 'CSRF-TOKEN';
        $httpProvider.defaults.xsrfHeaderName = 'X-CSRF-TOKEN';

        //Cache everything except rest api requests
        //httpRequestInterceptorCacheBusterProvider.setMatchlist([/.*api.*/, /.*protected.*/], true);

        /*$httpProvider.interceptors.push('errorHandlerInterceptor');
        $httpProvider.interceptors.push('authExpiredInterceptor');
        $httpProvider.interceptors.push('notificationInterceptor');*/

        // Initialize angular-translate
        $translateProvider.useLoader('$translatePartialLoader', {
            urlTemplate: 'app/i18n/{lang}/{part}.json'
        });

        $translateProvider.preferredLanguage('es');
        $translateProvider.useCookieStorage();
        $translateProvider.useSanitizeValueStrategy('escaped');
        $translateProvider.addInterpolation('$translateMessageFormatInterpolation');

        /*tmhDynamicLocaleProvider.localeLocationPattern('bower_components/angular-i18n/angular-locale_{{locale}}.js');
        tmhDynamicLocaleProvider.useCookieStorage();
        tmhDynamicLocaleProvider.storageKey('NG_TRANSLATE_LANG_KEY');*/
    })
;