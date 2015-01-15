elliptical.module=(function (app) {
    
    app = elliptical();
    $.controller.config.template.autoRender = true;
    $.controller.config.template.bindHTML5Imports = true;
    var Model = elliptical.Model;
    var Service = elliptical.Service;
    var Provider = elliptical.Provider;
    var providers = elliptical.providers;
    var Session = elliptical.Session;
    var Location = elliptical.Router.Location;
    var Validation = elliptical.Validation;
    //Validation.schemas = app_data.schemas;
    var $local = providers.session.$local;
    var session = new Session($local);
    var utils = elliptical.utils;

   
    //define web api rest provider
    var $Rest = providers.$rest;
    var $rest = new $Rest({});
    var apiProtocol,apiHost, apiPort, apiPath;
    apiPath = '/api';

    //dev
    app.configure('development', function () {
        apiProtocol = 'http';
        apiHost = 'localhost';
        apiPort = '49173';
    });

    //production
    app.configure('production', function () {
        apiProtocol = location.protocol.replace(':','');
        apiHost = location.hostname;
        apiPort = (apiProtocol === 'https') ? 443 : 80;
    });

    $rest.protocol = apiProtocol;
    $rest.host = apiHost;
    $rest.path = apiPath;
    $rest.port = apiPort;

    

    //assign rest web api provider as the default service provider
    Service.$provider = $rest;

    //asp.net mvc odata settings
    Service.$paginationProvider.count = 'count';
    Service.$paginationProvider.data = 'items';

    /* middleware */

    // middleware service injection
    app.use(elliptical.service(Service,session));
    //app.router
    app.use(app.router);

    /* end middleware */

    // controller service injection
    $.controller.service(Service, Location,Validation,session);

    /* listen */
    app.listen();

    return app;

})(elliptical.module);

Elliptical(function () {
    var logo = $('#brand-logo');
    var touchLogo = $('.touch-logo');
    logo.on('click', function (event) {
        location.href = '/';
    });
    touchLogo.on('touchclick', function (event) {
        location.href = '/';
    });
});



Elliptical(function () {
    var search = $('ui-search');
    var button = search.find('button');
    var input = search.find('input');

    search.keypress(function (e) {
        if (e.which == '13') {
            e.preventDefault();
            var keyword = $(this).children().eq(0).val();
            if (keyword != "") {
                go(keyword);
            }
        }
    });

    button.on('click', function (event) {
        var keyword = input.val();
        if (keyword != "") {
            go(keyword);
        }
    });

    function go(keyword) {
        var url = location.pathname;
        if (url.toLowerCase().indexOf('women') != -1) {
            location.href = '/Womens/Product/Search/' + keyword + '/1';
        }
        else {
            location.href = '/Mens/Product/Search/' + keyword + '/1';
        }
    }

});



        
 
elliptical.module = (function (app) {
    var Provider = elliptical.Provider;
    var Service = elliptical.Service;
    var utils = elliptical.utils;
    var Session = elliptical.Session;
    var Membership = elliptical.Membership;
    var providers = elliptical.providers;
    //use html5 localStorage as the session provider
    var $local = providers.session.$local;
    var session = new Session($local);

    var SignIn = Service.extend({
        "@class": 'SignIn'
    }, {});

    var SignUp = Service.extend({
        "@class": 'SignUp'
    }, {});

    Membership.$provider = Provider.extend({
        login: function (params, callback) {
            SignIn.post(params, function(err,data){
                if (!err) {
                    //store result as a session variable
                    session.put({ key: 'profile', session: data });
                    callback(err, data);
                } else {
                    callback(err, data);
                }
            });
        },
        signUp: function (params, callback) {
            SignUp.post(params, function (err, data) {
                if (!err) {
                    session.put({ key: 'profile', session: data });
                    callback(err, data);
                } else {
                    callback(err, data);
                }
            });
        }
    }, {});

    //inject service into controller
    $.controller.service(Membership);

    return app;
})(elliptical.module);


