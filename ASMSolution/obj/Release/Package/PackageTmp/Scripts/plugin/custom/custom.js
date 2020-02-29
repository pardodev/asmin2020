var color_input_text = '#defafc';
$(document).ready(function () {
    //function auto numeric
    $(".numeric").autoNumeric("init", {
        aSep: ',',
        aDec: '.',
        aPad: false,
        mDec: '2',
        vMin : 0,
    });
    $('.select2').select2();
});