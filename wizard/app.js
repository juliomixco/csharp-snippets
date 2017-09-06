angular.module('RecursionHelper', []).factory('RecursionHelper', ['$compile', function ($compile) {
    return {
        /**
         * Manually compiles the element, fixing the recursion loop.
         * @@param element
         * @@param [link] A post-link function, or an object with function(s) registered via pre and post properties.
         * @@returns An object containing the linking functions.
         */
        compile: function (element, link) {
            // Normalize the link parameter
            if (angular.isFunction(link)) {
                link = { post: link };
            }

            // Break the recursion loop by removing the contents
            var contents = element.contents().remove();
            var compiledContents;
            return {
                pre: (link && link.pre) ? link.pre : null,
                /**
                 * Compiles and re-adds the contents
                 */
                post: function (scope, element) {
                    // Compile the contents
                    if (!compiledContents) {
                        compiledContents = $compile(contents);
                    }
                    // Re-add the compiled contents to the element
                    compiledContents(scope, function (clone) {
                        element.append(clone);
                    });

                    // Call the post-linking function, if any
                    if (link && link.post) {
                        link.post.apply(null, arguments);

                    }
                }
            };
        }
    };
}]);
var app = angular.module('app', ["RecursionHelper", "ngSanitize", "ngRoute", 'naif.base64', 'blockUI', 'pubnub.angular.service', 'ui.bootstrap', 'ui.bootstrap.datetimepicker', 'ngMask',  'ngMaterial', 'ngMessages','fcsa-number']);
angular.module('app').provider('apiRoot', function () {     
    this.$get = function () {       
        return $("#linkApiRoot").attr("href");
    };
    return $("#linkApiRoot").attr("href");
});
//angular.module('app').config(["$provide", function ($provide) {
//        $provide.value("apiRoot", $("#linkApiRoot").attr("href"));
//    }]);

angular.module('app').config(['$httpProvider', function($httpProvider) {
    //initialize get if not there
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};    
    }    

    // Answer edited to include suggestions from comments
    // because previous version of code introduced browser-related errors

    //disable IE ajax request caching
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // extra
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
}]);
var multilineString = function (f) {
    return f.toString().
    replace(/^[^\/]+\/\*!?/, '').
    replace(/\*\/[^\/]+$/, '');
}

//// Storing a single value
//var app = angular.module(‘myApp’, []);

//app.value(‘usersOnline’, 0);

//// Now we inject our constant value into a test controller
//app.controller(‘TestCtrl’, [‘usersOnline’, function TestCtrl(usersOnline) {
//    console.log(usersOnline);
//    usersOnline = 15;
//    console.log(usersOnline);
//}]);
// Storing multiple constant values inside of an object
// Keep in mind the values in the object mean they can be modified
// Which makes no sense for a constant, use wisely if you do this
//var app = angular.module(‘myApp’, []);

//app.constant(‘config’, {
//    appName: ‘My App’,
//    appVersion: 2.0,
//apiUrl: ‘http://www.google.com?api’
//});

//// Now we inject our constant value into a test controller
//app.controller(‘TestCtrl’, [‘config’, function TestCtrl(config) {
//    console.log(config);
//    console.log(‘App Name’, config.appName);
//    console.log(‘App Name’, config.appVersion);
//}]);


angular.module('app').constant('cfg', {
    urlPrefix: '', //'/SQRely_QA',
    publishkey: 'pub-c-1cdf474f-de16-455c-b8de-f807255eb458',
    subscribekey: 'sub-c-05b8b244-b4aa-11e5-a705-0619f8945a4f',
});

angular.module('app').config(function (blockUIConfig) {

    // Change the default overlay message
    blockUIConfig.message = 'Loading...';
    // blockUIConfig.template = '<div id="fountainTextG"><div id="fountainTextG_1" class="fountainTextG">L</div><div id="fountainTextG_2" class="fountainTextG">o</div><div id="fountainTextG_3" class="fountainTextG">a</div><div id="fountainTextG_4" class="fountainTextG">d</div><div id="fountainTextG_5" class="fountainTextG">i</div><div id="fountainTextG_6" class="fountainTextG">n</div><div id="fountainTextG_7" class="fountainTextG">g</div></div>';
    //'<pre><code>{{ state | json }}</code><img src="/squirrel.gif"></img></pre>';
    //blockUIConfig.templateUrl = '/Async/Loading/';
    // Change the default delay to 100ms before the blocking is visible
    blockUIConfig.delay = 100;

});

//angular.module('app').constant('uiDatetimePickerConfig', {
//        dateFormat: 'MM/dd/yyyy HH:mm',
//        defaultTime: '00:00:00',
//        html5Types: {
//            date: 'MM/dd/yyyy HH:mm',
//            'datetime-local': 'MM/dd/yyyy HH:mm',
//            'month': 'MM-yyyy'
//        },
//        enableDate: true,
//        enableTime: true,
//        buttonBar: {
//            show: true,
//            now: {
//                show: true,
//                text: 'Now'
//            },
//            today: {
//                show: true,
//                text: 'Today'
//            },
//            clear: {
//                show: true,
//                text: 'Clear'
//            },
//            date: {
//                show: true,
//                text: 'Date'
//            },
//            time: {
//                show: true,
//                text: 'Time'
//            },
//            close: {
//                show: true,
//                text: 'Close'
//            }
//        },
//        closeOnDateSelection: true,
//        appendToBody: false,
//        altInputFormats: [],
//        ngModelOptions: { }
//    })