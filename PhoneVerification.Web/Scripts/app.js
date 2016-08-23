$(document).ready(function () {
    $("#enter_number").submit(function (e) {
        e.preventDefault();
        initiateCall();
    });
});

function initiateCall() {
    $.post("call/create", { phoneNumber: $("#phone_number").val() },
      function (data) { showCodeForm(data.verificationCode); }, "json");

    checkStatus();
}

function showCodeForm(code) {
    $("#verification_code").text(code);
    $("#verify_code").fadeIn();
    $("#enter_number").fadeOut();
}

function checkStatus() {
    $.post("call/status", { phoneNumber: $("#phone_number").val() },
      function (data) { updateStatus(data.status); }, "json");
}

function updateStatus(current) {
    if (current === "unverified") {
        $("#status").append(".");
        setTimeout(checkStatus, 3000);
    }
    else {
        success();
    }
}

function success() {
    $("#status").text("Verified!");
}