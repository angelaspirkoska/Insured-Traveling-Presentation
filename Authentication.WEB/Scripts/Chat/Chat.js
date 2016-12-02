function prepareSocket() {
    if (window.location.pathname.split("/")[1] === "Login") {
        console.log("Terminate socket init");
        return;
    }
        
    var connection = $.hubConnection();
    var hProxy = connection.createHubProxy("chatHub");
    hProxy.on("MessageRequest", function (data) {
        console.log(data);
    });

    $("#requestChatBtn").click(function() {
        hProxy.invoke("SendRequest");
        console.log("request sent");
    });

    connection.start().done(function () {
        console.log("connection established");
    });


}
