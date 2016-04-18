"use strict";

angular.module('AngularAuthApp')

.constant('ENV', 'dev')

.constant('BASE_URL', 'https://localhost:44384/')

.constant('ngAuthSettings', {
    apiServiceBaseUri: 'https://localhost:44384/',
    clientId: 'ClienteAppAuthPrueba'
})

;