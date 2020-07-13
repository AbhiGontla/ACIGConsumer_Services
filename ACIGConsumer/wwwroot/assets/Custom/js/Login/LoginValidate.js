$('#loginbutton').on('click', function (e) {
    e.preventDefault();
    var nid = $("#natId").val();
    var pin = $("#pincode").val();
    var status = "true";
    if (nid == "" && pin == "") {
        $("#errormessage").text("Please enter valid credentials");
        status = "false";
    } else if (nid == "") {
        $("#Niderror").text("Please enter 10 digit nationalId");
        status = "false";
    } else if (pin == "") {
        $("#pinerror").text("Please enter 4 digit pin");
        status = "false";
    } else if(status == "true"){
        $.ajax({
            type: "GET",
            url: "/Login/ValidateUser",
            data: { nid: nid, pin: pin },
            dataType: "json",
            success: function (result) {
                debugger;
                if ($.trim(result.success).toUpperCase() === "FALSE") {
                    $("#errormessage").show();
                    $("#errormessage").text("Please enter valid credentials");
                    $("#natId").val("");
                    $("#pincode").val("");

                }
                if ($.trim(result.success).toUpperCase() === "TRUE") {
                    alert("login success");
                    location.href = "https://localhost:44310/Home/Index";
                }
            },
            error: function (error) {
                //$("#dataDiv").html("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
            }
        });
    }




});
$("#natId").keyup(function () {
    checkNid();
    $("#errormessage").hide();

});

$("#pincode").keyup(function () {
    checkpin();
    $("#errormessage").hide();

});

function checkNid() {
    var nid = $("#natId").val();

    if (nid == "") {
        $("#Niderror").text("Please enter valid nationalId");
    }
    if (nid.length < 10) {
        $("#Niderror").show();
        $("#Niderror").text("Please enter 10 digit nationalId");
    } else if (nid.length > 10) {
        $("#Niderror").show();
        $("#Niderror").text("Please enter 10 digit nationalId");
    } else if (!(/^\S{3,}$/.test(nid))) {
        $("#Niderror").show();
        $("#Niderror").text("Please enter whithout space");
        return false;
    } else if (!$.isNumeric(nid)) {
        $("#Niderror").show();
        $("#Niderror").text("Please enter number only");
    }
    else if (nid.length = 10) {
        $("#Niderror").hide();
    }
    else {
        return undefined;
    }


}
function checkpin() {
    var pin = $("#pincode").val();
    if (pin == "") {
        $("#pinerror").show();
        $("#pinerror").text("Please enter 4 digit pin");
    }
    if (pin.length < 4) {
        $("#pinerror").show();
        $("#pinerror").text("Please enter 4 digit pin");
    } else if (pin.length > 4) {
        $("#pinerror").show();
        $("#pinerror").text("Please enter 4 digit pin");
    } else if (!(/^\S{3,}$/.test(pin))) {
        $("#pinerror").show();
        $("#pinerror").text("Please enter without space");
        return false;
    } else if (!$.isNumeric(pin)) {
        $("#pinerror").show();
        $("#pinerror").text("Please enter number only");
    }
    else if (pin.length = 4) {
        $("#pinerror").hide();
    }
    else {
        return undefined;
    }
}