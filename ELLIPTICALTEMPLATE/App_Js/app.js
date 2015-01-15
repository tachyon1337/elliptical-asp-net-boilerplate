elliptical.module = (function (app) {

    app = elliptical();
    $.controller.config.template.autoRender = true;
    $.controller.config.template.bindHTML5Imports = true;
    var Model = elliptical.Model;
    var Service = elliptical.Service;
    var Provider = elliptical.Provider;
    var providers = elliptical.providers;
    var Session = elliptical.Session;
    var Location = elliptical.Router.Location;
    var Cookie = elliptical.Cookie;
    var $local = providers.session.$local;
    var session = new Session($local);
    var utils = elliptical.utils;


    //define web api rest provider
    var $Rest = providers.$rest;
    var $rest = new $Rest({});
    var apiProtocol, apiHost, apiPort, apiPath;
    apiPath = '/api';

    //dev
    app.configure('development', function () {
        apiProtocol = 'http';
        apiHost = 'localhost';
        apiPort = '49173';
    });

    //production
    app.configure('production', function () {
        apiProtocol = location.protocol.replace(':', '');
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
    app.use(elliptical.service(Service, session));
    //app.router
    app.use(app.router);

    /* end middleware */

    // controller service injection
    $.controller.service(Service, Location, session,Cookie);

    /* listen */
    app.listen();

    return app;

})(elliptical.module);