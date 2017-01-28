var hProxy;
var openChats = 0;

function prepareSocket() {
    if (window.location.pathname.split("/")[1] === "Login") {
        console.log("Terminate socket init");
        return;
    }
        
    var connection = $.hubConnection();
    hProxy = connection.createHubProxy("chatHub");
    connection.start().done(function () {
        console.log("connection established");
    });

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
        pushMessageToChat(data);       
    });


    hProxy.on("UpdateChat", function (data) {
        console.log("update");      
        //tuj neki refresh
        console.log(data);

    });
    

    hProxy.on("ReceiveId", function (data) {
        console.log("receiveid");
        openChat(data.enduser, data.requestId);
        //$("$a"+data.requestId).
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
 

    hProxy.on("ActiveMessages", function (data) {
        console.log("messages");
        console.log(data);
        fillMessages(data);
    });

    hProxy.on("SendAcknowledge", function (data) {
        console.log("accepted request");
        console.log(data);
        openChatEndUser(data.admin, data.requestId);
    });

    $("#requestChatBtn").click(function () {
        hProxy.invoke("SendRequest");
        //hProxy.invoke("SendRequestMobile", "Sofija");
        $("#requestChatBtn").val("Request sent");
        console.log("request sent");
    });


}

function acceptChat(data) {
    hProxy.invoke("AcceptRequest", data);
}

function openMessageInChat(requestId, from, admin) {  
    var selection = $("div#" + from);
    var chat = document.getElementById("from");
            if (!document.getElementById(from)){
                
                console.log("ne postoi");
                //open new chat
                if (admin) {
                    openChat(from, requestId);
                }
                else {
                    openChatEndUser(from, requestId);   
                }

                getLastTenMessages(requestId);
            }
            else {

                var $div = $('div[requestId = ' + requestId + ']');

                if ($div) {

                    if (admin) {
                        openChat(from, requestId);
                    }
                    else {
                        openChatEndUser(from, requestId);
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

function fillMessages(data) {
    var message;
    for (i = 0; i < data.messages.length; i++) {
        message = $("#message_template").html();
        $("#messages").append(message);
        $("#none .media").attr("onclick", "openMessageInChat(" + data.messages[i].requestId + ",'" + data.messages[i].ichatwith + "'," + data.messages[i].admin + ");");
        $("#none .media .media-body .timestamp").text(data.messages[i].timestamp);
        $("#none .media .media-body .message").text(data.messages[i].message);
        $("#none .media .media-body .media-heading").attr("name", data.messages[i].from);
        $("#none .media .media-body .media-heading strong").html(data.messages[i].from);
        $("#none").attr("id", data.messages[i].requestId);
        $("#messageId").attr("id", data.messages[i].messageId);
       
    }
}

function openChatEndUser(data, requestId) {
    console.log("div#" + data + " #discardChat");

    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    if ($div) {

        $($div + " textarea").focus();
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
    console.log("moeto username: " + localStorage.getItem("username"));
    var $div = $('div[requestId = ' + requestId + '] .portlet-body');
    console.log("divooo " + $div);
    //if ($div) {

    //    $('div[requestId = ' + requestId + '] textarea').focus();
    //    return;
    //}
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
            
            var last = $("div#" + data + " .row")[$("div#" + data + " .row").length - 1];

            var $div = $("div#" + data + " .portlet-body");

            if (last !== undefined && $(last).hasClass(data.from)) {
                var p = "<p>" + data.message + "</p>";
                $(last).children().children().children().append(p);
            }
            else
            {
              
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
    console.log("dali e admin: ", data.admin);
   // console.log("moeto username: " + localStorage.getItem("username"));
    if (!$("div#" + data.from).length) {
        if (data.admin) {

            openChatEndUser(data.from, data.requestId);
        }
        else {
            openChat(data.from, data.requestId);
        }
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
