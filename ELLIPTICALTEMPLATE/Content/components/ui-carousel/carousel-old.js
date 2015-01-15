(function () {

    document.addEventListener('WebComponentsReady', function () {
        $.controller('elliptical.ellipticalCarousel',{
            _initController: function () {
                this._bind();
            },

            _bind: function () {
                if (this.options.debug !== undefined && this.options.debug === "true") {
                    //this._runHolder();
                }
            },

            _runHolder: function () {
                var self = this;
                setTimeout(function () {
                    Holder.run({});
                    self._setVisibility();
                }, 100);
            },

            _events:function(){
                //this._event($(window),'carousel.selected',function(event,data){
                    
                //});
            }

        });
    });

})();
