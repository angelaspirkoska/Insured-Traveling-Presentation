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
    });

    hProxy.on("ReceiveMessage", function (messageDTO) {
        pushMessageToChat(messageDTO);
    });
    
    hProxy.on("ReceiveId", function (adminResponseDTO) {
        var chatDTO = { Sender: adminResponseDTO.EndUser, RequestId: adminResponseDTO.RequestId, Admin: true };
        openChatU(chatDTO);
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
        var chatDTO = { Sender: endUserResponseDTO.Admin, RequestId: endUserResponseDTO.RequestId, Admin: false };
        openChatU(chatDTO);
    });

    $("#requestChatBtn").click(function () {
        hProxy.invoke("SendRequest");
        $("#requestChatBtn").val("Request sent"); 
    });

}

function acceptChat(requestedBy) {
    hProxy.invoke("AcceptRequest", requestedBy);
}

function openMessageInChat(requestId, from, admin) {  
    var selection = $("div#" + from);
    var chat = document.getElementById("from");
            if (!document.getElementById(from)){                
                var chatDTO;
                if (admin) {
                    chatDTO = { Sender: from, RequestId: requestId, Admin: true };
                }
                else {
                    chatDTO = { Sender: from, RequestId: requestId, Admin: false }; 
                }
                openChatU(chatDTO);
                getLastTenMessages(requestId);
            }
            else {
                var $div = $('div[requestId = ' + requestId + ']');
                if ($div) {

                    if (admin) {
                        chatDTO = { Sender: from, RequestId: requestId, Admin: true };
                    }
                    else {
                        chatDTO = { Sender: from, RequestId: requestId, Admin: false };
                    }
                    openChatU(chatDTO);
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
            $.each(result.Messages, function (key, value) {
                if (first === 0) {
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

function pushOldMessage(messageDTO, ichatwith, requestId) {
   
    var PushMessageDTO = { Sender: messageDTO.From, Message: messageDTO.Text, Date: messageDTO.Date + " " + messageDTO.Hour + ":" + messageDTO.Minute };
    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    var row = generateMessage(PushMessageDTO);
    if (row !== undefined) {
        $div.append(row);
    }

    var children = $div.children();
    children[children.length - 1].scrollIntoView();
}

function pushOldMessageNext(data, requestId, lastMessageId) {
    var $button = $("div[requestId=" + requestId + "] button[messageId=" + lastMessageId + "]");
    var PushMessageDTO = { Sender: data.From, Message: data.Text, Date: data.Date + " " +data.Hour + ":" + data.Minute };
    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    var row = generateMessage(PushMessageDTO);
    if (row !== undefined) {
        $(row).insertAfter($button);
    }
}

function addShowMoreButton(requestId, lastMessageId)
{
    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    var message = "<div class='row'><div class='col-lg-12'><button type='button' class='btn btn-default' onclick='LoadNextTenMessages("+requestId+","+lastMessageId+")' messageId="+lastMessageId+"> Show more </button> </div></div>";
    $div.append(message);
}

function generateMessage(PushMessageDTO)
{
    var last = PushMessageDTO.Last;
    var lastSender = PushMessageDTO.LastSender
    var sender = PushMessageDTO.Sender;
    var message = PushMessageDTO.Message;
    var date = PushMessageDTO.Date;

    if (last !== undefined && $(last).hasClass(lastSender)) {
        var p = "<p>" + message + "</p>";
        $(last).children().children().children().append(p);
    }
    else {
        var row = "<div class='row " + sender + "'>" +
            "<div class='col-lg-12'>" +
            "<div class='media'>" +
            "<div class='media-body'>" +
            "<h4 class='media-heading'>" +
            sender +
            "<span class='small pull-right'>" + date + "</span>" +
            "</h4>" +
            "<p>" +
            message +
            "</p>" +
            "</div>" +
            "</div>" +
            "</div>" +
            "</div>" +
            "<hr/>";
        return row;
    }
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

function openChatU(chatDTO) {
    var sender = chatDTO.Sender;
    var requestId = chatDTO.RequestId;
    var isAdmin = chatDTO.Admin;

    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    var chat = $("#chat_template").html();
    $("#chats").append(chat);
    $("#none").attr("requestId", requestId);
    $("#none").attr("id", sender);
    var offset = 260 * openChats + 2 * openChats;
    $("div#" + sender).css("right", offset + "px");
    $("div#" + sender + " .portlet-title>h4").text(sender);
    $("div#" + sender + " textarea").focus();
    var children = $("div#" + sender + " .row").children();
    $("div#" + sender + " textarea").attr("id", requestId);

    if (!isAdmin) {
        $("div#" + sender + " #discardChat").hide();
        $("div#" + sender + " #createFnol").hide();
    }

    openChats++;

    $("div#" + sender + " #discardChat").click(function (e) {
        var BaseRequestIdDTO = { RequestId: requestId };
        hProxy.invoke("DiscardMessage", BaseRequestIdDTO);
    });
    $("div#" + sender + " #createFnol").click(function (e) {
        var BaseRequestIdDTO = { RequestId: requestId };
        hProxy.invoke("CreateFnol", BaseRequestIdDTO);
    });

    $("div#" + sender + " #close").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).parent().parent().parent().parent().remove();
        openChats--;
        shiftToRight();
    });

    $("div#" + sender + " textarea").keydown(function (e) {
        if (e.which === 13) {

            var message = $(this).val();
            if (message === "")
                return false;

            var MessageDTO = { To: sender, Message: message, RequestId: requestId };
            hProxy.invoke("SendMessage", MessageDTO);

            var date = new Date();

            var last = $("div#" + sender + " .row")[$("div#" + sender + " .row").length - 1];
            var $div = $("div#" + sender + " .portlet-body");
            console.log(date.getHours() + ":" + date.getMinutes());
            var PushMessageDTO = { Last: last, LastSender: localStorage.getItem("username"), Sender: localStorage.getItem("username"), Message: message, Date: date.getHours() + ":" + date.getMinutes() };
            console.log(PushMessageDTO);
            var row = generateMessage(PushMessageDTO);
            if (row !== undefined) {
                $div.append(row);
            }

            children = $div.children();
            children[children.length - 1].scrollIntoView();
            $(this).val("");
            return false;
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
        var chatDTO;
        if (isAdmin) {
            chatDTO = { Sender: sender, RequestId: requestId, Admin: true };          
        }
        else {
            chatDTO = { Sender: sender, RequestId: requestId, Admin: false };
        }
        openChatU(chatDTO);
        getLastTenMessages(requestId);
        return;
    }

    var date = new Date();

    var last = $("div#" + sender + " .row")[$("div#" + sender + " .row").length - 1];
    var $div = $("div#" + sender + " .portlet-body");
    var PushMessageDTO = { Last: last, LastSender: sender, Sender: sender, Message: message, Date: date.getHours() + ":" +date.getMinutes() };
    var row = generateMessage(PushMessageDTO);
    if (row !== undefined) {
        $div.append(row);
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
