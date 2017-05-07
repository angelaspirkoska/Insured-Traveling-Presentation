function sendNotifications() {

    var connectionProxy = $.connection.notificationsHub;

    connectionProxy.client.showNotification = function (notification) {
        $("#notifications li").append(notification);
        $(".fa-bell-o").css("color", "red");
    };

    $.connection.hub.start().done(function () {
        connectionProxy.server.addNotification($("#assignees-notification").val());
        connectionProxy.server.addNotification($("#watchers-notification").val());
    });

}