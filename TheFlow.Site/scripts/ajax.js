//enable post back functionality with the .post-back class and data-post attribute
$('.post-back').click(function () {
    var clicked = $(this);
    var data = clicked.attr('data-post');
    $.ajax({
        type: "post",
        dataType: "json",
        url: data,
        data: AddAntiForgeryToken({ ajax: true }),
        success: function (data, textStaus) {
            if (data.redirect) {
                window.location.replace(data.redirect);
            }
        }
    });
});

////find all of the elements with the up-vote class
////and send an ajax request to up-vote the post
//$('.up-vote').click(function () {
//    var clicked = $(this);
//    var data = clicked.attr('data-post');
//    $.ajax({
//        type: "post",
//        dataType: "json",
//        url: '@Url.Action("UpVote", "Posts")/' + data,
//        data: AddAntiForgeryToken({ ajax: true }),
//        success: function (data, textStaus) {
//            if (data.redirect) {
//                window.location.replace(data.redirect);
//            }
//        }
//    });
//});

////find all of the elements with the down-vote class
////and send an ajax request to up-vote the post
//$('.down-vote').click(function () {
//    var clicked = $(this);
//    var data = clicked.attr('data-post');
//    $.ajax({
//        type: "post",
//        dataType: "json",
//        url: '@Url.Action("DownVote", "Posts")/' + data,
//        data: AddAntiForgeryToken({ ajax: true }),
//        success: function (data, textStaus) {
//            if (data.redirect) {
//                window.location.replace(data.redirect);
//            }
//        }
//    });
//});

