/* The code is using jQuery to call the getAllTransactions() function when the page loads. */
$(function () {
    getAllTransactions();
});


/**
 * The function getAllStocks() is a function that uses the get method to get the data from the
 * api/v1/Trading/GetAllTransactions endpoint. The data is then passed to the formaterStocks() function
 */
function getAllTransactions() {
    $.get("/api/v1/Trading/GetAllTransactions", function (transactions) {
        formaterStocks(transactions);
    })
        .fail(function (feil) {
            $("#myTransactions").html("Feil på server - prøv igjen senere");
        });
}


/**
 * It takes an array of stocks and returns a table with the stocks in it
 * @param stocks - The array of stocks that you want to display.
 */

function formaterStocks(transactions) {

    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Order ID</th><th>Status</th><th>Name</th><th>Symbol</th><th>Quantity</th><th>Price Per Stock</th><th>Created At</th><th>Totalprice</th>" +
        "</tr>";

    for (let transaction of transactions) {
        ut += "<tr>" +
            "<td>" + transaction.orderID + "</td>" +
            "<td>" + transaction.status + "</td>" +
            "<td>" + transaction.name + "</td>" +
            "<td>" + transaction.symbol + "</td>" +
            "<td>" + transaction.quantity + "</td>" +
            "<td>" + transaction.price + "</td>" +
            "<td>" + transaction.createdAt + "</td>" +
            "<td>" + transaction.totalPrice + "</td>" +
            "</tr>";
    }

    ut += "</table>";
    $("#myTransactions").html(ut);
}
