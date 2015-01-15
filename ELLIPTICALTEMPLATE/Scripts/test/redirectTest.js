(function () {
   
    testMutationObserver();
    testFlexWrap();

    function testMutationObserver()
    {
        if (typeof window.MutationObserver ==='undefined') {
            location.href = "http://classic.thehubltd.com";
        }
    }
    function testFlexWrap()
    {
        var s = document.body || document.documentElement, s = s.style;
        if (!(s.webkitFlexWrap == '' || s.msFlexWrap == '' || s.flexWrap == '')) {
            location.href = "http://classic.thehubltd.com";
        }
    }
})();