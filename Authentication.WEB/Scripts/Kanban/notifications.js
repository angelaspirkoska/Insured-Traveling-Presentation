var connection;

function sendNotifications() {

    connection = $.connection.notificationsHub;

    connection.client.showNotification = function (notification) {
        console.log(notification);
    };

    $.connection.hub.start().done(function () { console.log("notification hub connected."); });

    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000); // Restart connection after 5 seconds.
    });

    var assigneesNotification = $("#assignees-notification").val();
    var watchersNotification = $("#watchers-notification").val();
    //connection.server.showNotification(assigneesNotification);
    //connection.server.showNotification(watchersNotification);
    console.log(assigneesNotification);
    console.log(connection);

    connection.server.addNotification(assigneesNotification);
    connection.server.addNotification(watchersNotification);

}