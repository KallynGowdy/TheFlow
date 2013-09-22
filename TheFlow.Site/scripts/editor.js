//Enables interception of the tab key to write a tab to the focused input element instead of changing elements
function enableTab(id) {
    var el = document.getElementById(id);
    el.onkeydown = function (e) {
        if (e.keyCode === 9) { // tab was pressed

            // get caret position/selection
            var val = this.value,
                start = this.selectionStart,
                end = this.selectionEnd;

            // set textarea value to: text before caret + tab + text after caret
            this.value = val.substring(0, start) + '    ' + val.substring(end);

            // put caret at right position again
            this.selectionStart = this.selectionEnd = start + 4;

            // prevent the focus loss
            return false;
        }
    };
}

//Enables the markdown editor for the page
function enableEditor(hooks) {
    var converter = new Markdown.getSanitizingConverter();

    var editor = new Markdown.Editor(converter);

    if (hooks) {
        hooks(editor);
    }

    //refresh prettyprint when the preview is refreshed
    editor.hooks.chain("onPreviewRefresh", function () {
        highlightCode();
    });

    editor.run();
}

//causes all of the code in the page to be highlighted.
function highlightCode() {
    $("pre").addClass("prettyprint");
    $("code").addClass("prettyprint");
    prettyPrint();
}

function defaultHooks(converter, editor) {
    if (converter) {
        converter.hooks.chain("preBlockGamut", function (text, runBlockGamut) {
            return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                return "<blockquote>" + runBlockGamut(inner) + "</blockquote>\n";
            });
        });
    }
    if (editor) {
        //refresh prettyprint when the preview is refreshed
        editor.hooks.chain("onPreviewRefresh", function () {
            highlightCode();
        });
    }
}

//Adds an anti-forgery token to the given set of data for an ajax request.
//Usage:
//$.ajax({
//  type: "post",
//      dataType: "html",
//  url: 'the url',
//  data: AddAntiForgeryToken({ id: someIntData }),
//  success: function (response) {
//      // ....
//  }
//  });
function AddAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
    return data;
};

loader.jqueryCallback.push(function () {
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
});