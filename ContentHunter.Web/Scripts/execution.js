$(document).ready(function () {
    //if ($('#message').length > 0)
    //     setInterval("check()", 10000);

    $('.status').click(function () {
        var id = $(this).attr('id').replace('status', '');
        var instruction = $(this);
        $.post('/Execution/Start', { id: id }, function (data) {
            instruction.removeClass('off');
            instruction.addClass('on');
            instruction.attr('title', 'Running');
        });
    });
});

function check() {
    $.getJSON('/Show', function (data) {
        var items = [];

        $.each(data, function (key, val) {
            alert(val);
        });

        if (val) {
            $('#message').html('Execution finished!');
        }
    });
}
