var isAdmin = false;

$(document).ready(function () {
    $(".hover-tooltip button").tooltip('show');
    $("#searchClients").hide();
    $("#searchFNOL").hide();
    $("#searchChats").hide();
    $("#tableClients").DataTable().destroy();
    $("#tableClients").DataTable();
    $("#tablePolicies").DataTable().destroy();
    $("#tablePolicies").DataTable();
    $("#tableFNOL").DataTable().destroy();
    $("#tableFNOL").DataTable();
    $("#tableFNOLPolicies").css("display", "none");
    $("#tablePoliciesClients").css("display", "none");
    $("#tablePoliciesHolders").css("display", "none");

    $.ajax({
        type: "GET",
        url: "/Search/IsAdmin",
        success: function (result) {
            var result = JSON.parse(result);
            isAdmin = result.isAdmin;      
        }
    });
});

$("#DateI").focusout(function () {
    var Sdat = $("#DateI").val();
    $("#startDate").attr('min', Sdat);
    $("#endDate").attr('min', Sdat);
    $("#DateS").attr('min', Sdat);
});

$("#endDate").focusout(function () {
    var Edat = $("#endDate").val();
    $("#DateS").attr('max', Edat);
    $("#startDate").attr('max', Edat);
});

$("#PolicyNumber").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/Search/ShowPolicies",
            type: "POST",
            dataType: "json",
            data: { Prefix: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.Name, value: item.Name };
                }))
            }
        })
    },
    messages: {
        noResults: "", results: ""
    }
});

$("#PolicyNumberFNL").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/Search/ShowPolicies",
            type: "POST",
            dataType: "json",
            data: { Prefix: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.Name, value: item.Name };
                }))
            }
        })
    },
    messages: {
        noResults: "", results: ""
    }
});

function ShowClients() {
    $("#searchChats").hide();
    $("#searchPolycies").hide();
    $("#searchFNOL").hide();
    $("#searchClients").show();
    $("#polycies").removeAttr("class");
    $("#clients").attr("class", "active");
    $("#first_notice_of_loss").removeAttr("class");
    $("#clients").attr("class", "active");
    $("#tablePoliciesClients").css("display", "none");
    $("#tablePoliciesHolders").css("display", "none");
};

function ShowPolycies() {
    $("#searchChats").hide();
    $("#searchFNOL").hide();
    $("#searchClients").hide();
    $("#searchPolycies").show();
    $("#clients").removeAttr("class");
    $("#first_notice_of_loss").removeAttr("class");
    $("#polycies").attr("class", "active");
    $("#tableFNOLPolicies").css("display", "none");
};

function ShowFNOL() {
    $("#searchChats").hide();
    $("#searchPolycies").hide();
    $("#searchClients").hide();
    $("#searchFNOL").show();
    $("#polycies").removeAttr("class");
    $("#clients").removeAttr("class");
    $("#first_notice_of_loss").attr("class", "active");
};

function ShowChats() {

    $("#searchClients").hide();
    $("#searchPolycies").hide();
    $("#searchFNOL").hide();
    $("#searchChats").show();

    $("#polycies").removeAttr("class");
    $("#clients").removeAttr("class");
    $("#first_notice_of_loss").removeAttr("class");

    $("#tablePoliciesClients").css("display", "none");
    $("#tablePoliciesHolders").css("display", "none");
    $("#tableFNOLPolicies").css("display", "none");
};

function SearchPolycies(){
    $("#tablePolicies").DataTable().destroy();
    $("#tablePolicies").DataTable({
        dom: "Bfrtip",
        ajax: "/Search/GetPolicies?name=" + $("#namePolicy").val() + "&embg=" + $("#embgPolicy").val() + "&land=" + $("#land").val() + "&address=" + $("#addressPolicy").val() + "&agency=" + $("#agency").val() + "&TypePolicy=" + $("#TypePolicy").val() + "&Country=" + $("#Country").val() + "&startDate=" + $("#startDate").val() + "&endDate=" + $("#endDate").val() + "&dateI=" + $("#dateI").val() + "&dateS=" + $("#dateS").val() + "&operatorStartDate=" + $("#operatorStartDate").val() + "&operatorEndDate=" + $("#operatorEndDate").val() + "&operatorDateI=" + $("#operatorDateI").val() + "&operatorDateS=" + $("#operatorDateS").val() + "&PolicyNumber=" + $("#PolicyNumber").val(),
        columns: [
           { data: "Polisa_Broj" },
           { data: "InsuredName" },
           { data: "Country" },
           { data: "Policy_type" },
           { data: "Zapocnuva_Na" },
           { data: "Zavrsuva_Na" },
           { data: "Datum_Na_Izdavanje" },
           { data: "Datum_Na_Storniranje" },
           {
               data: "Polisa_Broj",
               "render": function (data, type, row, meta) {
                   if (type === 'display') {
                       var value = data;
                       return $('<a>')
                           .attr('href', '/Policy/PolicyDetails?policyNumber=' + data)
                           .attr('target', '_blank')
                           .attr('class', 'details')
                           .text("@InsuredTraveling.Resource.Search_DetailsLink")
                           .wrap('<div></div>')
                           .parent()
                           .html();
                   } else {
                       return data;
                   }
               }
           },
           {
               data: "Polisa_Id",
               "render": function (data, type, row, meta) {
                   if (type === 'display') {
                       return $('<a>')
                           .attr('href', 'javascript:;')
                           .attr('class', 'loss')
                           .attr('id', data)
                           .text("@InsuredTraveling.Resource.Search_FNOLLink")
                           .wrap('<div></div>')
                           .parent()
                           .html();
                   } else {
                       return data;
                   }
               }
           },
           {
               data: "Polisa_Id",
               "render": function (data, type, row, meta) {
                   if (type === 'display') {
                       return $('<a>')
                           .attr('href', '/FirstNoticeOfLoss/Index?policyId=' + data)
                           .attr('class', 'newLoss')
                           .attr('id', data)
                           .text("@InsuredTraveling.Resource.Search_AddFNOLLink")
                           .wrap('<div></div>')
                           .parent()
                           .html();
                   } else {
                       return data;
                   }
               }
           }

        ],
        select: true
    });

    $('#tablePolicies tbody').on('click', 'td .loss', function () {
        var $row = $(this).closest('td');
        var policy_num = $row.children().attr('id');
        CreateFNOLTablePoliciesByPolicyNumber(policy_num);

    });
};

function CreateFNOLTablePoliciesByPolicyNumber(policy_number) {
    $("#tableFNOLPolicies").DataTable().destroy();
    $("#tableFNOLPolicies").DataTable({
        dom: "Bfrtip",
        ajax: "/Search/GetFNOLByPolicyNumber?number=" + policy_number,
        columns: [
                { data: "PolicyNumber" },
                { data: "InsuredName" },
                { data: "ClaimantPersonName" },
                { data: "Claimant_insured_relation" },
                { data: "AllCosts" },
                { data: "Date" },
                { data: "HealthInsurance" },
                { data: "LuggageInsurance" },
                {
                    data: "ID",
                    "render": function (data, type, row, meta) {
                        if (type === 'display') {
                            var value = data;
                            return $('<a>')
                                .attr('href', '/FirstNoticeOfLoss/View?id=' + data)
                                .attr('target', '_blank')
                                .attr('class', 'lossDetails')
                                .text("@InsuredTraveling.Resource.Search_DetailsLink")
                                .wrap('<div></div>')
                                .parent()
                                .html();
                        } else {
                            return data;
                        }
                    }
                },
        ],
        select: true
    });
    $("#tableFNOLPolicies").css("display", "table");
};

function SearchFNOL() {
    $("#tableFNOL").DataTable().destroy();
    $("#tableFNOL").DataTable({
        dom: "Bfrtip",
        ajax: "/Search/GetFNOL?PolicyNumber=" + $("#PolicyNumberFNL").val() + "&holderName=" + $("#holderName").val() + "&holderLastName=" + $("#holderLastName").val() + "&clientName=" + $("#clientName").val() + "&clientLastName=" + $("#clientLastName").val() + "&insuredName=" + $("#insuredName").val() + "&insuredLastName=" + $("#insuredLastName").val() + "&totalPrice=" + $("#totalPrice").val() + "&healthInsurance=" + $('#healthInsurance').is(':checked') + "&luggageInsurance=" + $('#luggageInsurance').is(':checked') + "&DateAdded=" + $("#DateAdded").val() + "&operatorDateAdded=" + $("#operatorDateAdded").val() + "&operatorTotalCost=" + $("#totalCostOperator").val(),
        columns: [
                { data: "PolicyNumber" },
                { data: "InsuredName" },
                { data: "ClaimantPersonName" },
                { data: "Claimant_insured_relation" },
                { data: "AllCosts" },
                { data: "Date" },
                { data: "HealthInsurance" },
                { data: "LuggageInsurance" },
                {
                    data: "ID",
                    "render": function (data, type, row, meta) {
                        if (type === 'display') {
                            var value = data;
                            return $('<a>')
                                .attr('href', '/FirstNoticeOfLoss/View?id=' + data)
                                .attr('target', '_blank')
                                .attr('class', 'lossDetails')
                                .text("@InsuredTraveling.Resource.Search_DetailsLink")
                                .wrap('<div></div>')
                                .parent()
                                .html();
                        } else {
                            return data;
                        }
                    }
                },
        ],
        select: true
    });
};

function SearchClients() {
    $("#tableClients").DataTable().destroy();
    var table = $("#tableClients").DataTable({
        dom: "Bfrtip",
        ajax: "/Search/GetUsers?name=" + $("#name").val() + "&lastname=" + $("#lastname").val() + "&embg=" + $("#embg").val() + "&email=" + $("#email").val() + "&address=" + $("#address").val() + "&phone=" + $("#phone").val() + "&postal_code=" + $("#postal_code").val() + "&passport=" + $("#passport").val() + "&city=" + $("#city").val(),
        columns: [
                    { data: "Name" },
                    { data: "Lastname" },
                    { data: "SSN" },
                    { data: "Address" },
                    { data: "City" },
                    { data: "Postal_Code" },
                    { data: "Phone_Number" },
                    { data: "Email" },
                    { data: "Passport_Number_IdNumber" },
                    {
                        data: "ID",
                        "render": function (data, type, row, meta) {
                            if (type === 'display') {
                                return $('<a>')
                                    .attr('href', 'javascript:;')
                                    .attr('class', 'clients')
                                    .attr('id', data)
                                    .text("@InsuredTraveling.Resource.Search_PolicyLink")
                                    .wrap('<div></div>')
                                    .parent()
                                    .html();
                            } else {
                                return data;
                            }
                        }
                    }
        ],
        select: true
    });
    $('#tableClients tbody').on('click', 'td .clients', function () {
        var $row = $(this).closest('td');
        var insuredId = $row.children().attr('id');
        CreatePoliciesByInsuredId(insuredId);
        CreatePoliciesByHolderId(insuredId);
    });
};

function SearchChat() {
    $("#tableChats").DataTable().destroy();
    $("#tableChats").DataTable({
        dom: "Bfrtip",
        ajax: "/Search/GetChats?username=" + $("#nameChat").val() + "&chatId=" + $("#chatId").val() + "&all=" + $("#all").is(':checked') + "&active=" + $("#active").is(':checked') + "&discarded=" + $("#discarded").is(':checked') + "&noticed=" + $("#noticed").is(':checked'),
        columns: [
            { data: "chatId" },
            { data: "chatWith" },
            { data: "noticed" },
            { data: "discarded" },
            {
                data: "chatId",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        return $('<a>')
                            .attr('href', 'javascript:;')
                            .attr('onclick', 'ShowChatDetails("' + row.chatWith + '",' + data + ');')
                            .attr('class', 'chat')
                            .attr('id', data)
                            .text("@InsuredTraveling.Resource.Search_DetailsLink")
                            .wrap('<div></div>')
                            .parent()
                            .html();
                    } else {
                        return data;
                    }
                }
            },
        ],
        select: true
    });
}

$('#tableChats tbody').on('click', 'tr', function () {
    console.log($(this));
    $(this).toggleClass('selected');
});

function ShowChatDetails(chatWith, requestId) {
    if (isAdmin) {
        openChat(chatWith, requestId);
    } else {
        openChatEndUser(chatWith, requestId);
    }
    getLastTenMessages(requestId);
}

function CreatePoliciesByInsuredId(insuredId) {
    $("#tablePoliciesClients").DataTable().destroy();
    $("#tablePoliciesClients").DataTable({
        dom: "Bfrtip",
        ajax: "/Search/CreatePoliciesByInsuredId?insuredId=" + insuredId,
        columns: [
           { data: "Polisa_Broj" },
           { data: "Country" },
           { data: "Policy_type" },
           { data: "Zapocnuva_Na" },
           { data: "Zavrsuva_Na" },
           { data: "Datum_Na_Izdavanje" },
           { data: "Datum_Na_Storniranje" },
           {
               data: "Polisa_Broj",
               "render": function (data, type, row, meta) {
                   if (type === 'display') {
                       var value = data;
                       return $('<a>')
                           .attr('href', '/Policy/PrintPolicy?id=' + data)
                           .attr('target', '_blank')
                           .attr('class', 'detailsInsured')
                           .text("@InsuredTraveling.Resource.Search_DetailsLink")
                           .wrap('<div></div>')
                           .parent()
                           .html();
                   } else {
                       return data;
                   }
               }
           },
                {
                    data: "Polisa_Id",
                    "render": function (data, type, row, meta) {
                        if (type === 'display') {
                            return $('<a>')
                                .attr('href', '/FirstNoticeOfLoss/Index?policyId=' + data)
                                .attr('class', 'newLoss')
                                .attr('id', data)
                                .text("@InsuredTraveling.Resource.Search_AddFNOLLink")
                                .wrap('<div></div>')
                                .parent()
                                .html();
                        } else {
                            return data;
                        }
                    }
                }
        ],
        select: true
    });
    $("#tablePoliciesClients").css("display", "table");
};

function CreatePoliciesByHolderId(holderId) {
    $("#tablePoliciesHolders").DataTable().destroy();
    $("#tablePoliciesHolders").DataTable({
        dom: "Bfrtip",
        ajax: "/Search/CreatePoliciesByHolderId?holderId=" + holderId,
        columns: [
           { data: "Polisa_Broj" },
           { data: "Country" },
           { data: "Policy_type" },
           { data: "Zapocnuva_Na" },
           { data: "Zavrsuva_Na" },
           { data: "Datum_Na_Izdavanje" },
           { data: "Datum_Na_Storniranje" },
           {
               data: "Polisa_Broj",
               "render": function (data, type, row, meta) {
                   if (type === 'display') {
                       var value = data;
                       return $('<a>')
                           .attr('href', '/Policy/PrintPolicy?id=' + data)
                           .attr('target', '_blank')
                           .attr('class', 'detailsHolder')
                           .text("@InsuredTraveling.Resource.Search_AddFNOLLink")
                           .wrap('<div></div>')
                           .parent()
                           .html();
                   } else {
                       return data;
                   }
               }
           },
        ],
        select: true
    });
    $("#tablePoliciesHolders").css("display", "table");
};
