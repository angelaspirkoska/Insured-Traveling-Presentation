var hProxy;
var openChats = 0;

function prepareSocket() {
    if (window.location.pathname.split("/")[1] === "Login") {
        console.log("Terminate socket init");
        return;
    }
        
    var connection = $.hubConnection();
    hProxy = connection.createHubProxy("chatHub");

    hProxy.on("MessageRequest", function (data) {
        console.log("message request function response");
        console.log(data);
        if (data.numberRequests !== 0)
        {
            console.log("ima requesti");
            $("#messageRequests span").css("color", "#FF0000");
            $.each(data.data, function (i, val) {
                $("#ul_alerts").prepend("<li><a href='#'><p><label class='label label-default text-center'>" + this.from + "</label></p> <p>" + this.timestamp + "</p><input type='button' value='Accept' onclick='acceptChat(\"" + this.from + "\")' class='btn btn-primary acceptChat' text=" + this.from + "/>Accept</a></li>");

                console.log(this.from);
           });
        }
        else {
            $("#messageRequests span").css("color", "#FFFFFF");
        }
       // $("#messageRequests").text("new " + data.numberRequests + " requests");
        $("#chatRequests").show();
    });

    hProxy.on("RequestId", function (data) {
        console.log("RequestID: "+data);
    });

    hProxy.on("ReceiveMessage", function (data) {
        console.log("message");
        pushMessageToChat(data);
        console.log(data);
        
    });

    hProxy.on("ReceiveId", function (data) {
        console.log("receiveid");
        openChat(data.enduser, data.requestId);

    });
    
    hProxy.on("Discarded", function (data) {
        console.log("reqid " + data.requestId + " dali " + data.discarded + " message " + data.message)
        if (data.discarded === "true") {
            $("#" + data.requestId).attr("readonly", true);
            $("#" + data.requestId).attr("placeholder", data.message);
        }
    });


    hProxy.on("FnolCreated", function (data) {
        console.log("reqid " + data.requestId + " dali " + data.fnolCreated + " message " + data.message)
        if (data.fnolCreated === "true") {
            $("#" + data.requestId).attr("readonly", true);
            $("#" + data.requestId).attr("placeholder", data.message);
        }
    });

    hProxy.on("SendAcknowledge", function (data) {
        console.log("accepted request");
        console.log(data);
        openChatEndUser(data.admin, data.requestId);
    });

    $("#requestChatBtn").click(function () {
        hProxy.invoke("SendRequest");
        console.log("request sent");
    });
    connection.start().done(function () {
        console.log("connection established");
    });


}
function acceptChat(data) {
    console.log(data);
    hProxy.invoke("AcceptRequest", data);
    console.log(" accepted ");
   // openChat(data);
}



function openChatEndUser(data, requestId) {
    console.log("otvorennn");
    console.log("div#" + data + " #discardChat");

    
    if ($("div#" + data).length) {

        $("#" + data + " textarea").focus();
        return;
    }
    var chat = $("#chat_template").html();
    $("#chats").append(chat);
    $("#none").attr("requestId", requestId);
    $("#none").attr("id", data);

    $(".hideable").css("display", "none");

    var offset = 260 * openChats + 2 * openChats;
    $("div#" + data).css("right", offset + "px");

    $("div#" + data + " .portlet-title>h4").text(data);
    $("div#" + data + " textarea").focus();
    var children = $("div#" + data + " .row").children();
    //children[children.length - 1].scrollIntoView();
    $("div#" + data + " textarea").attr("id", requestId);
    openChats++;

    $("div#" + data + " #close").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).parent().parent().parent().parent().remove();
        openChats--;
        shiftToRight();
    });


    $("div#" + data + " textarea").keydown(function (e) {
        if (e.which === 13) {

            var message = $(this).val();
            if (message === "")
                return false;

            hProxy.invoke("SendMessage", data, message, requestId);
            console.log("podatoci: " + data);
            var last = $("div#" + data + " .row")[$("div#" + data + " .row").length - 1];

            var $div = $("div#" + data + " .portlet-body");

            if (last !== undefined && $(last).hasClass(data.from)) {
                var p = "<p>" + data.message + "</p>";
                $(last).children().children().children().append(p);
            }
            else {
                console.log("pratil poraka: " + localStorage.getItem("username"));
                var date = new Date();
                var row = "<div class='row " + localStorage.getItem("username") + "'>" +
                    "<div class='col-lg-12'>" +
                    "<div class='media'>" +
                    "<div class='media-body'>" +
                    "<h4 class='media-heading'>" +
                    localStorage.getItem("username") +
                    "<span class='small pull-right'>" + date.getHours() + ":" + date.getMinutes() + "</span>" +
                    "</h4>" +
                    "<p>" +
                    message +
                    "</p>" +
                    "</div>" +
                    "</div>" +
                    "</div>" +
                    "</div>" +
                    "<hr/>";
                $div.append(row);
            }
            children = $div.children();
            children[children.length - 1].scrollIntoView();
            $(this).val("");
            return false;
        }
        $("div#" + data + " #discardChat").hide();
        $("div#" + data + " #createFnol").hide();
    });
}

function openChat(data, requestId) {
    
    if ($("div#" + data).length) {

        $("#" + data + " textarea").focus();
        return;
    }
    var chat = $("#chat_template").html();
    $("#chats").append(chat);
    $("#none").attr("requestId", requestId);
    $("#none").attr("id", data);
   

    var offset = 260 * openChats + 2 * openChats;
    $("div#" + data).css("right", offset + "px");

    $("div#" + data + " .portlet-title>h4").text(data);
    $("div#" + data + " textarea").focus();
    var children = $("div#" + data + " .row").children();
    //children[children.length - 1].scrollIntoView();
    $("div#" + data + " textarea").attr("id", requestId);
    openChats++;

    $("div#" + data + " #discardChat").click(function (e) {
        var requestId = $("div#" + data).attr("requestid");
        hProxy.invoke("DiscardMessage", $("#Sofija").attr("requestid"));
    });

    $("div#" + data + " #createFnol").click(function (e) {
        var requestId = $("div#" + data).attr("requestid");
        hProxy.invoke("CreateFnol", $("#Sofija").attr("requestid"));
    });

    $("div#" + data + " #close").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).parent().parent().parent().parent().remove();
        openChats--;
        shiftToRight();
    });


    $("div#" + data + " textarea").keydown(function (e) {
        if (e.which === 13) {

            var message = $(this).val();
            if (message === "")
                return false;

            hProxy.invoke("SendMessage", data, message, requestId);
            console.log("podatoci: " + data);
            var last = $("div#" + data + " .row")[$("div#" + data + " .row").length - 1];

            var $div = $("div#" + data + " .portlet-body");

            if (last !== undefined && $(last).hasClass(data.from)) {
                var p = "<p>" + data.message + "</p>";
                $(last).children().children().children().append(p);
            }
            else
            {
                console.log("pratil poraka: "+localStorage.getItem("username"));
                var date = new Date();
                var row = "<div class='row " + localStorage.getItem("username") + "'>" +
                    "<div class='col-lg-12'>" +
                    "<div class='media'>" +
                    "<div class='media-body'>" +
                    "<h4 class='media-heading'>" +
                    localStorage.getItem("username") +
                    "<span class='small pull-right'>" + date.getHours() + ":" + date.getMinutes() + "</span>" +
                    "</h4>" +
                    "<p>" +
                    message +
                    "</p>" +
                    "</div>" +
                    "</div>" +
                    "</div>" +
                    "</div>" +
                    "<hr/>";
                $div.append(row);
            }
            children = $div.children();
            children[children.length - 1].scrollIntoView();
            $(this).val("");
            return false;
        }
    });
}


function pushMessageToChat(data) {
   
    if (!$("div#" + data.from).length) {
        openChat(data.from, data.requestId);
    }

    var $div = $("div#" + data.from + " .portlet-body");

    var last = $("div#" + data.from + " .row")[$("div#" + data.from + " .row").length - 1];

    if (last !== undefined && $(last).hasClass(data.from)) {
        var p = "<p>" + data.message + "</p>";
        $(last).children().children().children().append(p);
    } else {
        var date = new Date();
        var message = "<div class='row " + data.from + "'>" +
            "<div class='col-lg-12'>" +
            "<div class='media'>" +
            "<div class='media-body " + data.from + "'>" +
            "<h4 class='media-heading'>" +
            data.from +
            "<span class='small pull-right'>" + date.getHours() + ":" + date.getMinutes() + "</span>" +
            "</h4>" +
            "<p>" +
            data.message +
            "</p>" +
            "</div>" +
            "</div>" +
            "</div>" +
            "</div>" +
            "<hr/>";

        $div.append(message);
    }   
    var children = $div.children();
    children[children.length - 1].scrollIntoView();
}


function shiftToRight() {
    $.each($("#chats").children(), function (i, value) {
        var offset = 160 + i * 280 + 2 * i;
        value.style.right = offset + "px";
    });
}
