/* A variable that is used to identify the user. */
let id = 1;

/* Calling the getUser function when the page loads. */
$(function () {
    getUser();
});

/**
 * The function gets the user from the server and then formats the stocks
 */
function getUser() {
    $.get("/api/v1/Trading/GetUser", function (user) {
        formaterStocks(user);
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}

/**
 * It takes a user object as input and returns a string containing a table with the user's information
 * @param user - The user object that is returned from the server.
 */
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