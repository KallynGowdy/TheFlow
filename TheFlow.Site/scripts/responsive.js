//provides responsive js

//makes button groups appear vertically on extra small devices (Less than 768 px)
var width = -1;

var btnGroupElements = $('.responsive.btn-group');
var verticalBtnGroupElements = $('.responsive.btn-group-vertical');

function resize() {
    width = $(window).width();

    //mobile - extra small screen
    if (width < 768) {
        

        //make the views, votes and answers buttons display horizontally
        btnGroupElements.removeClass('btn-group');
        btnGroupElements.addClass('btn-group-vertical');
        verticalBtnGroupElements.removeClass('btn-group-vertical');
        verticalBtnGroupElements.addClass('btn-group');
    }
        //tablet or up - small screen and up
    else {
        //make the views, votes and answers buttons display vertically
        btnGroupElements.removeClass('btn-group-vertical');
        btnGroupElements.addClass('btn-group');

        verticalBtnGroupElements.removeClass('btn-group');
        verticalBtnGroupElements.addClass('btn-group-vertical');
    }
}
resize();
$(window).bind("resize", function () {
    resize();
});