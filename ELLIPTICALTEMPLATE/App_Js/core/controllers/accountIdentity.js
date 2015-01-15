Elliptical(function () {
    $.controller('app.accountIdentity', {
        options: {
            dataBind:false
        },

        _initController: function () {
            var Cookie = this.service('Cookie');
            var profile = Cookie.get('profile');
            if (profile) {
                this._setIdentityName(profile);
            }
            
        },

        _subscriptions:function(){
            this._subscribe('profile.updated', this._onUpdated.bind(this));
        },

        _setIdentityName: function (profile) {
            var firstName = profile.firstName;
            var lastName = profile.lastName;
            this.element.text(firstName + ' ' + lastName);
            this._setVisibility();
        },

        _setVisibility:function(){
            $('[data-id="account-icon"]').addClass('visible');
        },

        _onUpdated: function (profile) {
            this._setCookie(profile);
            this._setIdentityName(profile);
        },

        _setCookie: function(profile){
            var Cookie = this.service('Cookie');
            var profile = {
                firstName: profile.firstName,
                lastName: profile.lastName
            };
            var params = {
                key: 'profile',
                value:profile
            }
            Cookie.put(params);
        }

    });
});