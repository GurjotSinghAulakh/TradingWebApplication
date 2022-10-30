let id = 1;

$(function () {
    getUser();
});



function getUser() {
    $.get("/api/v1/Trading/GetUser", function (user) {
        formaterStocks(user);
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}


function formaterStocks(user) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Id</th><th>First Name</th><th>Last Name</th><th>Street Address</th><th>Postal Code</th><th>Postal Location</th><th>Phone</th><th>Email</th>" +
        "</tr>";

        ut += "<tr>" +
            "<td>" + user.id + "</td>" +
            "<td>" + user.firstName + "</td>" +
            "<td>" + user.lastName + "</td>" +
            "<td>" + user.streetAddress + "</td>" +
            "<td>" + user.postalCode + "</td>" +
            "<td>" + user.postallocation + "</td>" +
            "<td>" + user.phone + "</td>" +
            "<td>" + user.email + "</td>" +
            "</tr>";
   

    ut += "</table>";
    $("#myProfile").html(ut);
}