elliptical.module = (function (app) {
    var Provider = elliptical.Provider;
    var Service = elliptical.Service;
    var utils = elliptical.utils;
    var Session = elliptical.Session;
    var Membership = elliptical.Membership;
    var providers = elliptical.providers;
    //use cookie as the session provider
    var $local = providers.session.$cookie;
    var session = new Session($local);

    var SignIn = Service.extend({
        "@resource": '/Account/Login'
    }, {});

    var SignUp = Service.extend({
        "@resource": '/Account/Register'
    }, {});

    Membership.$provider = Provider.extend({
        login: function (params, callback) {
            SignIn.post(params, function (err, data) {
                if (!err) {
                    var model = data.model;
                    session.put({ key: 'profile', session: model });
                    callback(err, model);
                } else {
                    var statusCode = err.statusCode;
                    if (statusCode == 303)
                    {
                        location.href = data;//redirect if further authentication steps
                    }
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
                    var statusCode = err.statusCode;
                    if (statusCode == 303) {
                        var url = data.message;
                        session.put({ key: 'RegisterConfirmEmail',session:'true' })
                        location.href = url;//redirect if further authentication steps
                    }
                    callback(err, data);
                }
            });
        }
    }, {});

    //inject service into controller
    $.controller.service(Membership);

    return app;
})(elliptical.module);