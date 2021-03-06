﻿Elliptical(function () {
    var _itemSelector = 'collapse-item';
    var _expandSelector = '.expand';
    var _expandClass = 'expand';
    var _toggleSelector = '[data-role="toggle"]';
    var _header = 'header';
    var _section = 'section';
    var _visible = 'visible';
    var _hidden = 'hidden';

    $.element.registerElement('collapse-item');


    $.element('elliptical.collapse', 'ui-collapse', {
        options: {
            animate: true,
            duration: 300,
            easing: 'linear'
        },

        /*==========================================
         PRIVATE
         *===========================================*/

        /**
         * init method
         * @private
         */
        _initElement: function () {
            this._data.collapsible = null;
            if (this.options.animate === 'false') {
                this.options.animate = false;
            }
            this._setCollapseItems();
            this._collapseEvents();
        },

        _events: function () {
            this._event($(window), 'collapse.toggle.disable', this._onDisableToggle.bind(this));
        },

        /**
         * show item
         * @param element {Object}
         * @private
         */
        _show: function (element) {
            if (typeof element === 'number') {
                element = this._getCollapsible(element);
            }
            this._data.collapsible = element;
            var isTouch = this._support.device.touch;
            var animated = this.options.animate;
            this._showCollapseItemToggle(element);
            (animated && !isTouch) ? this._expand() : this._open();
        },

        /**
         * hide item
         * @param element {Object}
         * @private
         */
        _hide: function (element) {
            if (typeof element === 'number') {
                element = this._getCollapsible(element);
            }
            this._data.collapsible = element;
            var isTouch = this._support.device.touch;
            var animated = this.options.animate;
            (animated && !isTouch) ? this._collapse() : this._close();
        },

        /**
         * toggle item
         * @param element {Object}
         * @private
         */
        _toggle: function (element) {
            if (typeof element === 'number') {
                element = this._getCollapsible(element);
            }
            this._onCollapse(element);
        },

        /**
         * animate expand item
         * @private
         */
        _expand: function () {
            var element = this.element;
            var collapsible = this._data.collapsible;
            var eventData = this._eventData();
            this._onEventTrigger('showing', eventData);

            var flyout = collapsible.children().not(_header);
            var duration = this.options.duration;
            var easing = this.options.easing;
            var out = element.find(_expandSelector + ' >*')
                .not(_header);

            if (out.length > 0) {
                out.each(function (i) {
                    var thisHeight = $(this)[0].offsetHeight;
                    $(this).css({ height: thisHeight + 'px' });
                });
                //collapse
                var opts = {};
                opts.duration = duration;
                opts.height = '0px';
                opts.easing = easing;
                this._transitions(out, opts, function () {
                    element.children(_itemSelector)
                        .not(collapsible)
                        .removeClass(_expandClass);
                });
            }
            flyout.css({ height: 'auto' });
            collapsible.addClass(_expandClass); //toggle the state of our element to be expanded
            var height = flyout[0].offsetHeight;
            flyout.hide().css({ height: '0px' }).show();

            //expand
            var self = this;
            var options = {};
            options.duration = duration;
            options.height = height + 'px';
            options.easing = easing;
            this._transitions(flyout, options, function () {
                //raise show event
                self._onEventTrigger('show', eventData);
            });
        },

        /**
         * animate collapse item
         * @private
         */
        _collapse: function () {
            var collapsible = this._data.collapsible;

            //raise the onHiding event
            var eventData = this._eventData();
            this._onEventTrigger('hiding', eventData);

            var flyout = collapsible.children().not(_header);
            var height = flyout[0].offsetHeight;

            //assign the css height, necessary to avoid animating from 'auto' height to zero, which you generally cannot do
            flyout.css({ height: height + 'px' });

            //collapse: animate to zero height
            var duration = this.options.duration;
            var easing = this.options.easing;
            var self = this;
            var options = {};
            options.duration = duration;
            options.height = '0px';
            options.easing = easing;
            this._transitions(flyout, options, function () {
                collapsible.removeClass(_expandClass);
                self._onEventTrigger('hide', eventData);
            });
        },

        /**
         * show item
         * @private
         */
        _open: function () {
            var element = this.element;
            var collapsible = this._data.collapsible;
            var eventData = this._eventData();

            element.children(_itemSelector)
                .not(collapsible)
                .removeClass(_expandClass);

            var self = this;
            collapsible.find(_section)
                .removeAttr('style');
            collapsible.addClass(_expandClass); //toggle the state of the shown element
            //raise show event
            self._onEventTrigger('show', eventData);

        },

        /**
         * hide item
         * @private
         */
        _close: function () {
            var collapsible = this._data.collapsible;

            //raise hiding event
            var eventData = this._eventData();
            this._onEventTrigger('hiding', eventData);

            collapsible.removeClass(_expandClass);
            collapsible.find(_section)
                .removeAttr('style');
            //raise hide event
            this._onEventTrigger('hide', eventData);
        },


        /**
         *
         * @param element {Object}
         * @private
         */
        _onCollapse: function (element) {
            this._data.collapsible = element;
            (element.hasClass(_expandClass)) ? this._hide(element) : this._show(element);

        },

        /**
         * gets collapse item from index
         * @param index {Number}
         * @returns {Object}
         * @private
         */
        _getCollapsible: function (index) {
            var element = this.element;
            var items = element.find(_itemSelector);
            var collapsible = items[index];
            this._data.collapsible = $(collapsible);
            return this._data.collapsible;
        },

        /**
         * gets collapse item from toggle element target
         * @param target {Object}
         * @returns {Object}
         * @private
         */
        _getCollapsibleFromTarget: function (target) {
            return target.parents(_itemSelector);
        },

        _setCollapseItems: function () {
            this._data.collapseItems = this.element.find(_itemSelector);
        },

        /**
         *
         * @param element {Number|Object}
         * @private
         */
        _showToggle: function (element) {
            if (typeof element === 'number') {
                element = this._getCollapsible(element);
            }
            this._showCollapseItemToggle(element);
        },

        /**
         *
         * @param element {Number|Object}
         * @private
         */
        _hideToggle: function (element) {
            if (typeof element === 'number') {
                element = this._getCollapsible(element);
            }
            this._hideCollapseItemToggle(element);
        },

        /**
         *
         * @param item {Object}
         * @private
         */
        _showCollapseItemToggle: function (item) {
            var toggle = item.find(_toggleSelector);
            toggle.show()
                .removeClass(_hidden)
                .addClass(_visible);
        },

        /**
         *
         * @param item {Object}
         * @private
         */
        _hideCollapseItemToggle: function (item) {
            var toggle = item.find(_toggleSelector);
            toggle.hide()
                .removeClass(_visible)
                .addClass(_hidden);
        },

        _hideToggleButtons: function () {
            var element = this.element;
            var toggles = element.find(_itemSelector);
            toggles.hide()
                .removeClass(_visible)
                .addClass(_hidden);
        },

        _showToggleButtons: function () {
            var element = this.element;
            var toggles = element.find(_itemSelector);
            toggles.show()
                .removeClass(_hidden)
                .addClass(_visible);
        },

        /**
         * builds event data object
         * @returns {Object}
         * @private
         */
        _eventData: function () {
            var data = {};
            data.target = this._data.collapsible;
            data.index = this._data.collapseItems.index(data.target);
            return data;
        },

        /**
         * component events
         * @private
         */
        _collapseEvents: function () {
            var self = this;
            var click = this._press();
            var element = this.element;
            this._event(element, click, _toggleSelector, function (event) {
                var collapsible = self._getCollapsibleFromTarget($(event.currentTarget));
                self._onCollapse(collapsible);
            });
        },

        _getToggles: function () {
            return this.element.find(_toggleSelector);
        },

        _disableTogglesByRange: function (toggles, index) {
            $.each(toggles, function (i, toggle) {
                if (i < index) {
                    $(toggle).css({ visibility: 'hidden' });
                }
            });
        },

        _onDisableToggle: function (event, data) {
            var self = this;
            var type = data.type;
            var index = data.index;
            var toggles = this._getToggles();
            if (type === 'range') {
                this._disableTogglesByRange(toggles, index);
            } else {
                var toggle = toggles[index];
                $(toggle).css({ visibility: 'hidden' });
            }
        },

        /*==========================================
         PUBLIC METHODS
         *===========================================*/

        /**
         *
         * @param index {Number|Object}
         * @public
         */
        show: function (index) {
            this._show(index);
        },

        /**
         *
         * @param index {Number|Object}
         * @public
         */
        hide: function (index) {
            this._hide(index);
        },

        /**
         *
         * @param index {Number|Object}
         * @public
         */
        toggle: function (index) {
            this._toggle(index);
        },

        /**
         * @public
         */
        hideToggles: function () {
            this._hideToggleButtons();
        },

        /**
         * @public
         */
        showToggles: function () {
            this._showToggleButtons();
        },

        /**
         *
         * @param index {Number|Object}
         * @public
         */
        hideToggle: function (index) {
            this._hideToggle(index);
        },

        /**
         *
         * @param index {Number|Object|
         * @public
         */
        showToggle: function (index) {
            this._showToggle(index);
        }


    });
});