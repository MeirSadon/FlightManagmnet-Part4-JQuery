
// http://stackoverflow.com/questions/41211875/angularjs-1-6-0-latest-now-routes-not-working
module.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.hashPrefix('');
}]);

//another example doing it using text/ng-template
module.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state("deals", {
            url: "/deals",
            templateUrl: "HomePage/Deals",
            controller: "dealsCtrl"
        })
        .state("customer-register", {
            url: "/customer-register",
            templateUrl: "HomePage/CusReg",
            controller: "cusRegCtrl"
        })
        .state("company-register", {
            url: "/company-register",
            templateUrl: "HomePage/CmpReg",
            controller: "cmpRegCtrl"
        })

    $urlRouterProvider.when("", "/deals");
    //$urlRouterProvider.otherwise('/404');
});