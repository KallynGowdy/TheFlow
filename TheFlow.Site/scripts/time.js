//enables basic time display for elements decorated with the .time class and time in the data-time tag

var utcTimeElements = $('.time');

//find all of the elements with .time class
var timeElements = $('.time-from-now');
setInterval(function () {
    //set the text
    timeElements.html(function (index, text) {
        timeElements[index].innerText = moment.utc(timeElements[index].getAttribute('data-time')).local().fromNow();
    });
    utcTimeElements.html(function (index, text) {
        utcTimeElements[index].innerText = moment.utc(utcTimeElements[index].getAttribute('data-time')).local().format("MMM DD `YY [at] h:mm a");
    });
}, 1000);