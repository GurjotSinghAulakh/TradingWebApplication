/* The code is using jQuery to call the getAllTransactions() function when the page loads. */
$(function () {
    getAllTransactions();
});


/**
 * The function getAllStocks() is a function that uses the get method to get the data from the
 * api/v1/Trading/GetAllTransactions endpoint. The data is then passed to the formaterStocks() function
 */
function getAllStocks() {
    $.get("/api/v1/Trading/GetAllTransactions", function (stocks) {
        console.log(stocks)
        formaterStocks(stocks);
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}


/**
 * It takes an array of stocks and returns a table with the stocks in it
 * @param stocks - The array of stocks that you want to display.
 */

function formaterStocks(stocks) {

    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Id</th><th>Symbol</th><th>Name</th><th>Price Per Stock</th><th>Quantity</th><th>Created_At</th><th>Totalprice</th>" +
        "</tr>";

    for (let stock of stocks) {
        ut += "<tr>" +
            "<td>" + stock.id + "</td>" +
            "<td>" + stock.status + "</td>" +
            "<td>" + stock.name + "</td>" +
            "<td>" + stock.symbol + "</td>" +
            "<td>" + stock.quantity + "</td>" +
            "<td>" + stock.createdAt + "</td>" +
            "<td>" + stock.totalPrice + "</td>" +
            "</tr>";
    }

    ut += "</table>";
    $("#myTransactions").html(ut);
}
