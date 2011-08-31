$(document).ready(function () {
    if ($('#message').length > 0)
        setInterval("check()", 10000);
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
