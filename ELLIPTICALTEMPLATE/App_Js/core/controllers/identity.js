Elliptical(function () {
    $.controller('app.identity', {
        options: {
            dataBind:false
        },

        _initController: function () {
            try{
                this._setIdentityName();
            } catch (ex) {
                this._showIcon();
            }
           
        },

        _setIdentityName: function () {
            var Cookie = this.service('Cookie');
            var profile = Cookie.get('profile');
            var firstName = profile.firstName;
            this.element.text(firstName);
            this._showIcon();
        },

        _showIcon: function () {
            var profileIcon = $('[data-role="profile-icon"]');
            profileIcon.addClass('visible');
        }
    });
});