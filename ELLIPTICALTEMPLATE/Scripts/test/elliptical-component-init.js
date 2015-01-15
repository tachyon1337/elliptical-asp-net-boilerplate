window.Elliptical = function (fn) {
    document.addEventListener('WebComponentsReady', function () {
        fn.call(this);
    });
}