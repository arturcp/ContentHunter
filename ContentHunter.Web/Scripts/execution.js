$(document).ready(function () {
    setInterval("Instruction.check()", 30000);

    $('.off').click(function () {
        var id = $(this).attr('id').replace('status', '');
        var instruction = $(this);
        $.post('/Execution/Start', { id: id }, function (data) {
            Instruction.clearClass(instruction);
            instruction.addClass('on');
            instruction.attr('title', 'Running');
        });
    });
});

Instruction = {
    clearClass: function (instruction) {
        instruction.removeClass('off');
        instruction.removeClass('on');
        instruction.removeClass('notRecurrent');
    },

    check: function () {
        $.getJSON('Execution/Status', function (data) {
            var items = [];
            var instruction;
            $.each(data, function (key, val) {
                instruction = $('#status' + val["Id"]);
                Instruction.clearClass(instruction);
                instruction.addClass(Instruction.getClass(val));
                instruction.attr('title', Instruction.getTitle(val));

            });
        });
    },

    getClass: function (instruction) {
        var resultClass = "";
        if (instruction["Running"]) resultClass = "on";
        else if (!instruction["IsRecurrent"] && instruction["HasRun"]) resultClass = "notRecurrent";
        else resultClass = "off";

        return resultClass;

    },

    getTitle: function (instruction) {
        var resultTitle = "";
        if (instruction["Running"]) resultTitle = "Running";
        else if (!instruction["IsRecurrent"] && instruction["HasRun"]) resultTitle = "This instruction is not recurrent and has already run";
        else resultTitle = "Click to start";
        return resultTitle;
    }
}