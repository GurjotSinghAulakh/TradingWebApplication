$(function () {
    //BuyID = window.location.search.substring(1);
    getAllTransactions();
});


function getAllStocks() {
    $.get("/api/v1/Trading/GetAllTransactions", function (stocks) {
        console.log(stocks)
        formaterStocks(stocks);
    })
        .fail(function (feil) {
            $("#feil").html("Feil på server - prøv igjen senere");
        });
}


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
