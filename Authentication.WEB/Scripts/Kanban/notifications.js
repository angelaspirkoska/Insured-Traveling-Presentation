$(function () {

    var connection = $.connection.notificationsHub;

    connection.client.showNotification = function (notification) {
        console.log(notification);
    };

    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000); // Restart connection after 5 seconds.
    });

})