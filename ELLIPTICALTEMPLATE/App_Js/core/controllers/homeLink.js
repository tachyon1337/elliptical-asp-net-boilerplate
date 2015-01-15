Elliptical(function(){
    $.controller('app.homeLink', {
        options:{
            dataBind:false
        },
        _events: function () {
            this._event(this.element, 'click', function () {
                location.href = '/';
            });
        }
    });
});