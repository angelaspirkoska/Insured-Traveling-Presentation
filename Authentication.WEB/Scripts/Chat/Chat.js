var hProxy;
var openChats = 0;
var connection;

function prepareSocket() {
    if (window.location.pathname.split("/")[1] === "Login") {
        console.log("Terminate socket init");
        return;
    }
    connection = $.hubConnection();

    //continuously reconnect if disconnected
    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000); // Restart connection after 5 seconds.
    });

    hProxy = connection.createHubProxy("chatHub");

    connection.start().done(function () {
        console.log("connection established");
    });

    hProxy.on("MessageRequest", function (MessageRequestsDTO) {
        var numberRequests = MessageRequestsDTO.RequestNumber;       
        $("#ul_alerts").empty();
        if (numberRequests !== 0)
        {
            console.log("ima requesti");
            $("#messageRequests span").css("color", "#FF0000");
            $.each(MessageRequestsDTO.Requests, function (i, val) {
                var requestedBy = this.RequestedBy;
                var timestamp = this.Timestamp;
                $("#ul_alerts").prepend("<li><a href='#'><p><label class='label label-default text-center'>" + requestedBy + "</label></p> <p>" + timestamp + "</p><input type='button' value='Accept' onclick='acceptChat(\"" + requestedBy + "\")' class='btn btn-primary acceptChat' text=" + requestedBy + "/></a></li>");       
           });
        }
        else {
            $("#messageRequests span").css("color", "#FFFFFF"); 
        }
        if ($("#ul_alerts").children().length === 0)
            $("#ul_alerts").prepend("<p> There are no requests </p>")
        $("#chatRequests").show();
    });

    hProxy.on("RequestId", function (RequestIdDTO) {
        console.log("RequestID: " + RequestIdDTO);
    });

    hProxy.on("ReceiveMessage", function (messageDTO) {
        pushMessageToChat(messageDTO);
    });

    
    hProxy.on("ReceiveId", function (adminResponseDTO) {
        openChatU(adminResponseDTO.EndUser, adminResponseDTO.RequestId, true);
    });
    
    hProxy.on("Discarded", function (ChatStatusUpdateDTO) {
        var requestId = ChatStatusUpdateDTO.RequestId;
        var message = ChatStatusUpdateDTO.Message;
        var success = ChatStatusUpdateDTO.Success;
        console.log("reqid " + requestId + " dali " + success + " message " + message)
        if (success) {
            $("textarea").attr('id', requestId).attr("readonly", true)
            $("textarea").attr('id', requestId).attr("placeholder", message);
        }
    });
    hProxy.on("DiscardedMessage", function (ChatStatusUpdateDTO) {
        var requestId = ChatStatusUpdateDTO.RequestId;
        var message = ChatStatusUpdateDTO.Message;
        $("textarea").attr('id', requestId).attr("readonly", true)
            $("textarea").attr('id', requestId).attr("placeholder", message);        
    });

    hProxy.on("FnolCreated", function (ChatStatusUpdateDTO) {
        var requestId = ChatStatusUpdateDTO.RequestId;
        var message = ChatStatusUpdateDTO.Message;
        var success = ChatStatusUpdateDTO.Success;
        console.log("reqid " + requestId + " dali " + success + " message " + message)
        if (success === "true") {
            $("textarea").attr('id', requestId).attr("readonly", true)
            $("textarea").attr('id', requestId).attr("placeholder", message);
        }
    });

    hProxy.on("FnolCreatedMessage", function (ChatStatusUpdateDTO) {
        var requestId = ChatStatusUpdateDTO.RequestId;
        var message = ChatStatusUpdateDTO.Message;
            $("textarea").attr('id', requestId).attr("readonly", true)
            $("textarea").attr('id', requestId).attr("placeholder", message);
    });   

    hProxy.on("ActiveMessages", function (LastMessagesDTO) {
        console.log("messages");
        console.log(LastMessagesDTO);
        fillMessages(LastMessagesDTO);
    });

    hProxy.on("SendAcknowledge", function (endUserResponseDTO) {
        openChatU(endUserResponseDTO.Admin, endUserResponseDTO.RequestId, false);
        //openChatEndUser(data.admin, data.requestId);
    });

    $("#requestChatBtn").click(function () {
        hProxy.invoke("SendRequest");
        $("#requestChatBtn").val("Request sent");
        console.log("request sent");
    });

}

function acceptChat(data) {
    //if(connection.st)
    hProxy.invoke("AcceptRequest", data);
}

function openMessageInChat(requestId, from, admin) {  
    var selection = $("div#" + from);
    var chat = document.getElementById("from");
            if (!document.getElementById(from)){
                
                console.log("ne postoi");
                //open new chat
                if (admin) {
                    openChatU(from, requestId, true);
                    //openChat(from, requestId);
                }
                else {
                    openChatU(from, requestId, false);   
                }

                getLastTenMessages(requestId);
            }
            else {

                var $div = $('div[requestId = ' + requestId + ']');

                if ($div) {

                    if (admin) {
                        openChatU(from, requestId, true);
                        //openChat(from, requestId);
                    }
                    else {
                        openChatU(from, requestId, false);
                        //openChatEndUser(from, requestId);
                    }

                }
            }
    }

function getLastTenMessages(requestId)
{
    var username = localStorage.getItem("username");
    $.ajax({
        type: "GET",
        url: "/api/chat/lasttenmessagesweb",
        data: {"requestId": requestId, "username": username},
        dataType: "json",
        success: function (result) {
            console.log(result);
            var ichatwith = result.ichatwith;
            var first = 0;
            
            //addShowMoreButton(requestId, result.messages[0].ID);
            
            $.each(result.Messages, function (key, value) {
                if (first === 0) {
                    console.log("idto na porakata " + value.Id);
                    addShowMoreButton(requestId, value.Id);
                }
                pushOldMessage(value, ichatwith, requestId);
                first++;
            });
        }
    });
}

function LoadNextTenMessages(requestId, lastMessageId) {
    $.ajax({
        type: "GET",
        url: "/api/chat/nexttenmessagesweb",
        data: { "requestId": requestId, "messageId": lastMessageId, "username": localStorage.getItem("username") },
        dataType: "json",
        success: function (result) {
            if (result.Messages === "End")
            {
                console.log("kraj");
                $("div[requestId="+requestId+"] button[messageId=" + lastMessageId + "]").hide();
            }
            else {

                var $button = $("div[requestId=" + requestId + "] button[messageId=" + lastMessageId + "]");
                var lastMessageIdNew = lastMessageId;

                $.each(result.Messages, function (key, value) {                   
                    pushOldMessageNext(value, requestId, lastMessageId);
                    lastMessageIdNew = value.Id;
                });

                $("div[requestId="+requestId+"] button[messageId="+lastMessageId+"]").attr("messageid", lastMessageIdNew);
                $("div[requestId=" + requestId + "] button[messageId=" + lastMessageIdNew + "]").attr("onclick", "LoadNextTenMessages(" + requestId + "," + lastMessageIdNew + ")");
            }
            console.log(result);
          
        }
    });
}

function pushOldMessageNext(data, requestId, lastMessageId) {
    console.log("pushing");
    var $button = $("div[requestId=" + requestId + "] button[messageId=" + lastMessageId + "]");
    var message = "<div class='row " + data.From + "'>" +
        "<div class='col-lg-12'>" +
        "<div class='media'>" +
        "<div class='media-body " + data.From + "'>" +
        "<h4 class='media-heading'>" +
        data.From +
        "<span class='small pull-right'>" + data.Date + " " + data.Hour + ":" + data.Minute + "</span>" +
        "</h4>" +
        "<p>" +
        data.Text +
        "</p>" +
        "</div>" +
        "</div>" +
        "</div>" +
        "</div>" +
        "<hr/>";
    $(message).insertAfter($button);
   // $div.append(message);
   // var children = $div.children();
    //children[children.length - 1].scrollIntoView();
}
function addShowMoreButton(requestId, lastMessageId)
{
    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    var message = "<div class='row'><div class='col-lg-12'><button type='button' class='btn btn-default' onclick='LoadNextTenMessages("+requestId+","+lastMessageId+")' messageId="+lastMessageId+"> Show more </button> </div></div>";


    $div.append(message);

   
}

function pushOldMessage(data, ichatwith, requestId) {
    
    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    var message = "<div class='row " + data.From + "'>" +
        "<div class='col-lg-12'>" +
        "<div class='media'>" +
        "<div class='media-body " + data.From + "'>" +
        "<h4 class='media-heading'>" +
        data.From +
        "<span class='small pull-right'>" +data.Date +" " + data.Hour + ":" + data.Minute + "</span>" +
        "</h4>" +
        "<p>" +
        data.Text +
        "</p>" +
        "</div>" +
        "</div>" +
        "</div>" +
        "</div>" +
        "<hr/>";

    $div.append(message);
    var children = $div.children();
    children[children.length - 1].scrollIntoView();
}

function fillMessages(LastMessagesDTOs) {
    var message;

    for (i = 0; i < LastMessagesDTOs.length; i++) {
        var from = LastMessagesDTOs[i].From;
        var text = LastMessagesDTOs[i].Message;
        var isAdmin = LastMessagesDTOs[i].Admin;
        var timestamp = LastMessagesDTOs[i].Timestamp;
        var ChatWith = LastMessagesDTOs[i].ChatWith;
        var MessageId = LastMessagesDTOs[i].MessageId;
        var RequestId = LastMessagesDTOs[i].RequestId;
        message = $("#message_template").html();
        $("#messages").append(message);
        $("#none .media").attr("onclick", "openMessageInChat(" + RequestId + ",'" + ChatWith + "'," + isAdmin + ");");
        $("#none .media .media-body .timestamp").text(timestamp);
        $("#none .media .media-body .message").text(text);
        $("#none .media .media-body .media-heading").attr("name", ChatWith);
        $("#none .media .media-body .media-heading strong").html(ChatWith);
        $("#none").attr("id", RequestId);
        $("#messageId").attr("id", MessageId);   
    }
}

function openChatU(data, requestId, isAdmin) {

    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    var chat = $("#chat_template").html();
    $("#chats").append(chat);
    $("#none").attr("requestId", requestId);
    $("#none").attr("id", data);
    var offset = 260 * openChats + 2 * openChats;
    $("div#" + data).css("right", offset + "px");
    $("div#" + data + " .portlet-title>h4").text(data);
    $("div#" + data + " textarea").focus();
    var children = $("div#" + data + " .row").children();
    $("div#" + data + " textarea").attr("id", requestId);
    openChats++;

    $("div#" + data + " #discardChat").click(function (e) {
        var BaseRequestIdDTO = { RequestId: requestId };
        hProxy.invoke("DiscardMessage", BaseRequestIdDTO);
    });

    $("div#" + data + " #createFnol").click(function (e) {
        var BaseRequestIdDTO = { RequestId: requestId };
        hProxy.invoke("CreateFnol", BaseRequestIdDTO);
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

            var MessageDTO = { To: data, Message: message, RequestId: requestId };
            console.log("sending message " + MessageDTO);
            hProxy.invoke("SendMessage", MessageDTO);
            var last = $("div#" + data + " .row")[$("div#" + data + " .row").length - 1];
            var $div = $("div#" + data + " .portlet-body");

            if (last !== undefined && $(last).hasClass(data.from)) {
                var p = "<p>" + data.message + "</p>";
                $(last).children().children().children().append(p);
            }
            else {

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

        if(!isAdmin)
        {
            $("div#" + data + " #discardChat").hide();
            $("div#" + data + " #createFnol").hide();
        }
    });
}

function pushMessageToChat(messageDTO) {
    var isAdmin = messageDTO.Admin !== undefined && messageDTO.Admin;
    var sender = messageDTO.From;
    var requestId = messageDTO.RequestId;
    var message = messageDTO.Message;
    console.log("dali e admin: ", isAdmin);
    if (!$("div#" + sender).length) {
        if (isAdmin) {
            openChatU(sender, requestId, true);
            //openChatEndUser(sender, requestId);
            getLastTenMessages(requestId);
        }
        else {
            openChatU(sender, requestId, false);
            getLastTenMessages(requestId);
        }
        return;
    }

    var $div = $("div#" + sender + " .portlet-body");

    var last = $("div#" + sender + " .row")[$("div#" + sender + " .row").length - 1];

    if (last !== undefined && $(last).hasClass(sender)) {
        var p = "<p>" + message + "</p>";
        $(last).children().children().children().append(p);
    } else {
        var date = new Date();
         message = "<div class='row " + sender + "'>" +
            "<div class='col-lg-12'>" +
            "<div class='media'>" +
            "<div class='media-body " + sender + "'>" +
            "<h4 class='media-heading'>" +
            sender +
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
