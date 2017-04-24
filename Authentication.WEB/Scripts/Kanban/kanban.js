var dropCount = 0;

function makeDraggableAgain(elem) {
    elem.draggable({
        connectToSortable: '.inner-sortable',
        revert: 'invalid',
        revertDuration: 200,
        containment: 'window',
        scroll: false,
        appendTo: 'body',
        helper: 'clone',
        start: function (event, ui) {
            dropCount = 0;
            $(ui.helper).addClass("ui-helper");
            $(ui.helper).data('dropped', false);
            $(this).hide();
        },
        stop: function (event, ui) {
            var isDropped = ui.helper.data('dropped');
            $(ui.helper).data('dropped', false);
            if (isDropped == true) {
                $(this).remove();
            } else {
                $(this).show();
            }
            dropCount = 0;
        }
    });
}

$(function () {

    $(".list-item-add-placeholder").click(function () {
        $(this).hide();
        $(this).closest(".list-item-wrapper").removeClass("list-item-add-hidden").addClass("list-item-add-shown");
        $(".list-item-add-input").removeClass("list-item-add-input-hidden").addClass("list-item-add-input-shown");
        $(".list-item-add-controls").show();
    });

    $(".icon-cancel").click(function () {
        $(this).closest(".list-item-wrapper").addClass("list-item-add-hidden").removeClass("list-item-add-shown");
        $(".list-item-add-input").addClass("list-item-add-input-hidden").removeClass("list-item-add-input-shown");
        $(".list-item-add-controls").hide();
        $(".list-item-add-placeholder").show();
    });

    $("form").submit(function () {

        var name = $(this).find(".list-item-add-input").val();
        if (name.trim() != "") {
            $.ajax({
                type: "post",
                url: "/Home/AddPoolList",
                data: {
                    boardId: "@Model.Id",
                    name: name
                },
                success: function (result) {
                    $(result).insertBefore(".unsortable");
                    $(".icon-cancel").click();
                }
            });
        }
        return false;
    });

    $(".sortable").sortable({
        handle: '.list-item-header',
        items: "> .is-sortable"
    });

    $(".inner-sortable").sortable({
        revert: '200',
        update: function () {

        }
    });

    $(".list-item-card").draggable({
        connectToSortable: '.inner-sortable',
        revert: 'invalid',
        revertDuration: 200,
        containment: 'window',
        scroll: false,
        appendTo: 'body',
        helper: 'clone',
        start: function (event, ui) {
            dropCount = 0;
            $(ui.helper).addClass("ui-helper");
            $(ui.helper).data('dropped', false);
            $(this).hide();
        },
        stop: function (event, ui) {
            var isDropped = ui.helper.data('dropped');
            $(ui.helper).data('dropped', false);
            if (isDropped == true) {
                $(this).remove();
            } else {
                $(this).show();
            }
            dropCount = 0;
        }
    });

    $(".inner-sortable").droppable({
        accept: ".list-item-card",
        drop: function (event, ui) {
            dropCount++;
            ui.helper.data('dropped', dropCount == 2);
            makeDraggableAgain(ui.draggable);
        }
    });

})