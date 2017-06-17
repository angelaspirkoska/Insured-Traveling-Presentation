function OpenAddTicketModal(poolListId) {
    $("#addTicketPoolListId").val(poolListId);
    $("#addNewTicketModal").modal("show");
}

function ShowTicketDetails(ticketId) {
    $("#loader").show();
    $.ajax({
        type: "post",
        url: "/Kanban/TicketDetails",
        data: {
            ticketId: ticketId
        },
        success: function (result) {
            $("#ticketDetailsContainer").html(result);
            $("#ticketDetailsModal").modal("show");
            $("#loader").hide();
        },
        error: function () {
            $("#loader").hide();
        }
    });
}

function AddNewTicket() {

    $("#addNewTicketModal").modal("hide");
    $("#loader").show();
    var poolListId = $("#addNewTicketModal form #addTicketPoolListId").val();

    $.ajax({
        url: "/Kanban/AddTicket",
        type: "post",
        data: $("#addNewTicketModal form").serialize(),
        success: function (result) {
            $(".inner-sortable[data-poollist='" + poolListId + "']").append(result);
            $("#addNewTicketModal input").val("");
            $("#addNewTicketModal select").val(-1);
            $("#addNewTicketModal textarea").val("");
            $("#ticketTypeComponents").html("");
            $('.selectpicker').selectpicker('deselectAll');
            sendNotifications();
            $("#loader").hide();
        },
        error: function () {
            $("#loader").hide();
        }
    });
}

function deletePoolList(poolListId) {
    $("#loader").show();
    $.ajax({
        url: "/Kanban/DeletePoolList",
        type: "post",
        data: {
            poolListId: poolListId
        },
        success: function () {
            $("#loader").hide();
            $("#" + poolListId).remove();
        },
        error: function () {
            $("#loader").hide();
        }
    });
}

function deleteTicket(ticketId) {
    $("#loader").show();
    $.ajax({
        url: "/Kanban/DeleteTicket",
        type: "post",
        data: {
            ticketId: ticketId
        },
        success: function () {
            $("#loader").hide();
            $('div[data-ticketid="' + ticketId + '"]').remove();
        },
        error: function () {
            $("#loader").hide();
        }
    });
}

function changeTicketPool(ticketId, oldParent, newParent) {
    var newParentChildren = [];
    $('*[data-poollist="' + newParent + '"]').children().each(function () {
        newParentChildren.push($(this).data('ticketid'));
    });
    $("#loader").show();
    $.ajax({
        url: "/Kanban/ChangeTicketPool",
        type: "post",
        data: {
            ticketId: ticketId,
            newPoolId: newParent,
            order: newParentChildren
        },
        success: function () {
            $("#loader").hide();
        },
        error: function () {
            $("#loader").hide();
        }
    });
}

function changeTicketOrder(oldParent) {
    var oldParentChildren = [];
    $('*[data-poollist="' + oldParent + '"]').children().each(function () {
        oldParentChildren.push($(this).data('ticketid'));
    });
    $("#loader").show();
    $.ajax({
        url: "/Kanban/ChangeTicketOrder",
        type: "post",
        data: {
            order: oldParentChildren
        },
        success: function () {
            $("#loader").hide();
        },
        error: function () {
            $("#loader").hide();
        }
    });
}

function rearrangePools() {
    var pools = [];
    $('.is-sortable').each(function () {
        pools.push($(this).find('.inner-sortable').data('poollist'));
    });
    $("#loader").show();
    $.ajax({
        url: "/Kanban/ChangePoolsOrder",
        type: "post",
        data: {
            order: pools
        },
        success: function () {
            $("#loader").hide();
        },
        error: function () {
            $("#loader").hide();
        }
    });
}

function setInnerSortable() {
    var oldParent = 0;
    var newParent = 0;

    var oldPosition = -1;
    var newPosition = -1;

    var updates = 0;

    $(".inner-sortable").sortable({
        appendTo: document.body,
        connectWith: ".inner-sortable",
        start: function (event, ui) {
            oldParent = $(this).closest('.inner-sortable').data('poollist');
            oldPosition = ui.item.index();
        },
        update: function (event, ui) {
            updates++;
        },
        receive: function (event, ui) {
            newParent = $(this).closest('.inner-sortable').data('poollist');
        },
        stop: function (event, ui) {
            if (updates == 1) {
                changeTicketOrder(oldParent);
            }
            if (updates == 2) {
                changeTicketPool($(ui.item).data('ticketid'), oldParent, newParent);
            }
            updates = 0;
        }
    });

}

$(function () {

    $("body").on("keyup", "#kanban-board-comment-field", function (event) {
        if (event.keyCode == 13) {
            var comm = $(this).val();
            var comment = $(
                '<li class="kanban-board-list-group-item">' +
                    '<span class="kanban-board-badge">now</span>' +
                    comm +
                '</li>'
            );
            $("#kanban-board-comments").append(comment);
            $(this).val("");
        }
    });

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

    $("#new-list-form").submit(function () {

        var name = $(this).find(".list-item-add-input").val();
        if (name.trim() != "") {
            $("#loader").show();
            $.ajax({
                type: "post",
                url: "/Kanban/AddPoolList",
                data: {
                    boardId: $('#surface').data('boardid'),
                    name: name
                },
                success: function (result) {
                    $(result).insertBefore(".unsortable");
                    $(".icon-cancel").click();
                    $("#loader").hide();
                },
                error: function () {
                    $("#loader").hide();
                }
            });
        }
        return false;
    });

    $(".sortable").sortable({
        handle: '.list-item-header',
        items: "> .is-sortable",
        update: function (event, ui) {
            rearrangePools();
        }
    });

    setInnerSortable();

});