elliptical.module = (function (app) {

    var Provider = elliptical.Provider;
    var Service = elliptical.Service;
   
    var ModelValidation = Service.extend({
        "@resource": 'ModelValidation'
    }, {});

    var Validation = elliptical.Validation;
   
    Validation.$provider = Provider.extend({
        post: function (data, name, callback) {
            if (data.__RequestVerificationToken) {
                delete data.__RequestVerificationToken;
            }
            var params = {
                model: data,
                type: name
            };
            ModelValidation.post(params, function (err, result) {
                var err_ = null;
                if (err) {
                    err_ = {
                        statusCode: err.statusCode,
                        message:'Form Validation Error'
                    }
                    if(err.statusCode==500)
                    {
                        err_.message = 'Validation Internal Server Error';
                    }
                }
                callback(err_, result.model);
            });
        }
    }, {});


    $.controller.service(Validation);

    return app;

})(elliptical.module);