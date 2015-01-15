Elliptical(function () {
    var lookItems = $('ui-item-strip');

    $.controller('app.lookTab', 'look-tab', {
        options: {
            inAnimation: 'slideInRight',
            inDuration: 1000,
            outAnimation: 'fadeOut',
            outDuration: 300
            
        },

        _initController:function(){
            this._data.isVisible = false;
            if (this._support.mq.touch) {
                //this.options.inAnimation = 'fadeIn';
                //this.options.inDuration = 3400;
            }
        },

        _events: function () {
            var click = this._data.click;
            this._event(this.element, click, this._onClick.bind(this));
        },

        _onClick: function (event) {
            this._hide();
            setTimeout(function () {
                lookItems.itemStrip('show');
            }, 300);
           
        },

        _show: function () {
            if (this._data.isVisible) {
                return false;
            }
            this._data.isVisible = true;
            if (this._support.mq.touch) {
                this._touchShow();
                return false;
            }
            this.element.removeClass('hide');
            this.element.addClass('show');
            var self = this;
            this._transitions(this.element, {
                preset: this.options.inAnimation,
                duration:this.options.inDuration
            }, function () {
               
            })
        },

        _touchShow: function () {
            this.element.attr('style', '');
            this.element.removeClass('animated');
            this.element.removeClass(this.options.outAnimation);
            this.element.removeClass('hide');
            this.element.addClass('show');
        },

        _hide: function () {
            this._data.isVisible = false;
            this.element.removeClass('show');
            this.element.addClass('hide');
            this._transitions(this.element, {
                preset: this.options.outAnimation,
                duration:this.options.outDuration
            }, function () {

            })

        },

        show: function () {
            this._show();
        }
    });
});